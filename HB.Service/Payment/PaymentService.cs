using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Transactions;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.Const;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.Jwt;
using HB.Infrastructure.Repository;
using HB.Service.Const;
using HB.Service.Helpers;
using HB.Service.Transaction;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;
using Marten;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HB.Service.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly ITransactionService _transactionService;
        private readonly IOptions<Commission> _options;
        private readonly IMapper _mapper;
        private readonly IAuthUserInformation _authUserInformation;
        private readonly IRepository<Accounts> _accountsRepository;

        public PaymentService(
            HbContext hbContext,
            IDocumentSession documentSession,
            IQuerySession querySession,
            ITransactionService transactionService,
            IOptions<Commission> options,
            IMapper mapper,
            IAuthUserInformation authUserInformation,
            IRepository<Accounts> accountsRepository
            )
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
            this._transactionService = transactionService;
            this._options = options;
            this._mapper = mapper;
            this._authUserInformation = authUserInformation;
            this._accountsRepository = accountsRepository;
        }

        #region CreateIbanTransfer
        public async Task<ReturnState<object>> CreateIbanTransfer(PostSendMoneyWithIbanViewModel model)
        {
            PaymentHelper.IbanValidation(model.Iban);

            var userInformation = _accountsRepository.All().Where(x => x.CustomersId == _authUserInformation.CustomerId)
                .Include("Customers").FirstOrDefault();

            if (userInformation is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "User not found!");
            } 

            var checkAmount = userInformation.Amount - model.Price;

            if (checkAmount <= 0M)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Insufficient balance!");
            }

            var isIbanHaresBankOwned = await _accountsRepository.FindAllFirstOrDefaultAsync(x => x.Iban == model.Iban);

            if (isIbanHaresBankOwned is null)
            {
                var checkBalance = checkAmount - _options.Value.Rate;

                if (checkBalance <= 0M)
                {
                    return new ReturnState<object>(HttpStatusCode.NotAcceptable, "The balance is not enough for the commission amount");
                } 
            }

            var ibanTransactionModel = new IbanTransactionViewModel()
            {
                AccountId = userInformation.Id,
                Amount = model.Price,
                AvailableBalance = checkAmount,
                CustomerId = userInformation.CustomersId,
                Explanation = PaymentIbanConst.IBAN_TRANSFER_EXPLANATION,
                TransactionsType = TransactionsType.PaymentWithIban
            };

            ReceiptInformation receiptInformationViewModel = new()
            {
                Balance = model.Price,
                SenderIban = userInformation.Iban,
                SenderName = $"{userInformation.Customers.Name} {userInformation.Customers.Surname}",
                ReciverName = model.UserNameAndSurname,
                ReciverIban = model.Iban,
                TransactionExplanation = $"{model.UserNameAndSurname}'a iban ile para gönderimi. {model.Price}₺"
            };

            if (isIbanHaresBankOwned is not null)
            {
                StartIbanPaymentComplate(userInformation , ibanTransactionModel , receiptInformationViewModel,  false);

                return new ReturnState<object>(true);
            }

            for (int i = 0; i < 2; i++)
            {
                if (i == 1) //commission
                {
                    StartIbanPaymentComplate(userInformation, ibanTransactionModel, receiptInformationViewModel, true);
                    continue;
                }

                StartIbanPaymentComplate(userInformation, ibanTransactionModel, receiptInformationViewModel, false);
            }

            return new ReturnState<object>(true);
        }
        #endregion

        #region Organisations
        public ReturnState<object> GetOrganisations()
        {
            var result = _hBContext.Organisations.ToList();

            var mapperResult = _mapper.Map<List<OrganisationsViewModel>>(result);

            return new ReturnState<object>(mapperResult);
        }
        #endregion

        #region OnlinePaymentCard
        public async Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model)
        {
            var card = CheckCardIsExist(model);

            if (card.CardType == CardType.CreditCard)
            {
                var paymentAmount = card.CardCurrentAmount - model.Price;

                if (paymentAmount < 0) throw new HbBusinessException("Insufficient card limit!");

                card.CardCurrentAmount = paymentAmount;

                _documentSession.Update(card);
                await _documentSession.SaveChangesAsync();

                var transaction = CreateTransactionCreditCard(card, model);

                _transactionService.CreateTransaction(transaction);

                return new ReturnState<object>(new PaymentResponseViewModel { PaymentRefNo = transaction.Id.ToString() , PaymentResult =true});
            }

            if (card.CardType == CardType.DebitCard)
            {
                var customerAccount = _hBContext.Accounts
                    .Include("Customers")
                    .Where(x => x.Id == card.AccountId && x.IsActive).FirstOrDefault();

                if (customerAccount == null) throw new HbBusinessException("User account not found!");

                var paymentAmount = customerAccount.Amount - model.Price;

                if (paymentAmount < 0) throw new HbBusinessException("Insufficient balance!");

                customerAccount.Amount = paymentAmount;

                _hBContext.Entry(customerAccount).State = EntityState.Modified;
                await _hBContext.SaveChangesAsync();

                card.CardCurrentAmount = customerAccount.Amount;

                ReceiptInformation receiptInformationViewModel = new()
                {
                    Balance = model.Price,
                    SenderIban = customerAccount.Iban,
                    SenderName = $"{customerAccount.Customers.Name} {customerAccount.Customers.Surname}",
                    ReciverName = model.PaymentSource,
                    ReciverIban = string.Empty,
                    TransactionExplanation = $"{model.PaymentSource} online harcama tahsilatı. {model.Price}₺"
                };

                var transaction = CreateTransactionDebidCard(card, model, receiptInformationViewModel);

                _transactionService.CreateTransaction(transaction);

                return new ReturnState<object>(new PaymentResponseViewModel { PaymentRefNo = transaction.Id.ToString(), PaymentResult = true });
            }

            throw new HbBusinessException("Out-of-process!");
        }
        #endregion

        public ReturnState<object> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model)
        {
            CheckCardIsExist(model);

            return new ReturnState<object>(true);
        }

        #region Invoice Payment
        public ReturnState<object> PostPayInvoice(int customerId, InvoicePaymentViewModel model)
        {
            var accountInformation = _hBContext.Accounts
                .Include("Customers")
                .Where(x => x.CustomersId == customerId).FirstOrDefault();

            if (accountInformation == null) throw new HbBusinessException("User not found!");

            var organisation = _hBContext.Organisations.Where(x => x.Id == model.OrganisationId).FirstOrDefault();

            var checkAmaount = accountInformation.Amount - organisation.InvoiceAmount;

            if (checkAmaount <= 0M) throw new HbBusinessException("Insufficient balance!");

            accountInformation.Amount = checkAmaount;

            _hBContext.Entry(accountInformation).State = EntityState.Modified;
            _hBContext.SaveChanges();

            var invoicePayment = new InvoicePaymentTransactionViewModel
            {
                AccountId = accountInformation.Id,
                Amount = organisation.InvoiceAmount,
                AvailableBalance = checkAmaount,
                CustomerId = accountInformation.CustomersId,
                Explanation = $"{organisation.OrganisationType.GetEnumDescription()} - ({organisation.Name})"
            };

            ReceiptInformation receiptInformationViewModel = new()
            {
                Balance = organisation.InvoiceAmount,
                SenderIban = accountInformation.Iban,
                SenderName = $"{accountInformation.Customers.Name} {accountInformation.Customers.Surname}",
                ReciverName = organisation.Name,
                ReciverIban = organisation.Iban,
                TransactionExplanation = $"{model.SubscriberNumber} abonelik için {organisation.Name} ücret tahsilatı. {organisation.InvoiceAmount}₺"
            };

            var transactions = CreateTransactionInvoicePayment(invoicePayment , receiptInformationViewModel);

            _transactionService.CreateTransaction(transactions);

            return new ReturnState<object>(true);
        }
        #endregion

        private Cards CheckCardIsExist(PostCheckPaymentInformationViewModel model)
        {
            var cardInformationResult = _querySession.Query<Cards>()
                .Where(
                x =>
                x.CardNumber == model.Card.CardNumber &&
                x.CustomerName == model.Card.CustomerName &&
                x.LastUseMount == model.Card.LastUseMount &&
                x.LastUseYear == model.Card.LastUseYear &&
                x.Cvv == model.Card.Cvv &&
                x.IsActive).FirstOrDefault();

            if (cardInformationResult == null) throw new HbBusinessException("Card not found!");

            return cardInformationResult;
        }

        #region Transactions

        private Transactions CreateTransactionCreditCard(Cards card , PostCheckPaymentInformationViewModel model)
        {
            return new Transactions()
            {
                CardId = card.Id,
                ProccessType = ProccessType.Outgoid,
                TransactionsType = TransactionsType.OnlinePayment,
                CustomerId = card.CustomerId,
                Explanation = model.PaymentSource,
                AvailableBalance = card.CardCurrentAmount,
                Amount = model.Price
            };
        }

        private Transactions CreateTransactionDebidCard(Cards card, PostCheckPaymentInformationViewModel model , ReceiptInformation receiptModel)
        {
            return new Transactions()
            {
                CardId = card.Id,
                ProccessType = ProccessType.Outgoid,
                TransactionsType = TransactionsType.OnlinePayment,
                CustomerId = card.CustomerId,
                Explanation = model.PaymentSource,
                Amount = model.Price,
                AvailableBalance = card.CardCurrentAmount,
                AccountId = card.AccountId,
                ReceiptInformation = receiptModel
            };
        }

        private Transactions CreateTransactionIbanTransfer(IbanTransactionViewModel model , ReceiptInformation receiptInformation)
        {
            return new Transactions()
            {
                ProccessType = ProccessType.Outgoid,
                TransactionsType = model.TransactionsType,
                CustomerId = model.CustomerId,
                Explanation = model.Explanation,
                Amount = model.Amount,
                AvailableBalance = model.AvailableBalance,
                AccountId = model.AccountId,
                ReceiptInformation = receiptInformation
            };
        }

        private Transactions CreateTransactionInvoicePayment(InvoicePaymentTransactionViewModel model, ReceiptInformation receiptInformationModel)
        {
            return new Transactions()
            {
                ProccessType = ProccessType.Outgoid,
                TransactionsType = TransactionsType.Corporation,
                CustomerId = model.CustomerId,
                Explanation = model.Explanation,
                Amount = model.Amount,
                AvailableBalance = model.AvailableBalance,
                AccountId = model.AccountId,
                ReceiptInformation = receiptInformationModel
            };
        }

        #endregion

        private void StartIbanPaymentComplate(Accounts userInformation , IbanTransactionViewModel transactionInformation, ReceiptInformation receiptInformation , bool isCommission)
        {
            if (isCommission)
            {
                transactionInformation.TransactionsType = TransactionsType.PaymentWithIbanCommission;
                transactionInformation.AvailableBalance = userInformation.Amount - _options.Value.Rate;
                transactionInformation.Explanation = PaymentIbanConst.IBAN_TRANSFER_EXPLANATION_COMMISSION;
            }

            userInformation.Amount = transactionInformation.AvailableBalance;

            _hBContext.Entry(userInformation).State = EntityState.Modified;
            _hBContext.SaveChanges();

            if (isCommission)
            {
                transactionInformation.Amount = _options.Value.Rate;
                receiptInformation.Balance = _options.Value.Rate;
                receiptInformation.ReciverName = HB.Service.Const.PaymentIbanConst.HARES_BANK;
                receiptInformation.ReciverIban = HB.Service.Const.PaymentIbanConst.HARES_BANK_IBAN;
                receiptInformation.TransactionExplanation = $"{receiptInformation.SenderName}'a iban ile para gönderimi. (Komisyon kesintisi. -Hares Bank) {_options.Value.Rate}₺";
            }

            var transaction = CreateTransactionIbanTransfer(transactionInformation , receiptInformation);

            _transactionService.CreateTransaction(transaction);
        }
    }
}

