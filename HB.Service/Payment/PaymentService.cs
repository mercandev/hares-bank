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
using HB.Infrastructure.MartenRepository;
using HB.Infrastructure.Repository;
using HB.Service.Const;
using HB.Service.CustomMapping;
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
        private readonly ITransactionService _transactionService;
        private readonly IOptions<Commission> _options;
        private readonly IMapper _mapper;
        private readonly IAuthUserInformation _authUserInformation;
        private readonly IRepository<Accounts> _accountsRepository;
        private readonly IRepository<Organisations> _organisationRepository;
        private readonly IMartenRepository<Cards> _cardRepository;

        public PaymentService(
            ITransactionService transactionService,
            IOptions<Commission> options,
            IMapper mapper,
            IAuthUserInformation authUserInformation,
            IRepository<Accounts> accountsRepository,
            IRepository<Organisations> organisationRepository,
            IMartenRepository<Cards> cardRepository
            )
        {
            this._transactionService = transactionService;
            this._options = options;
            this._mapper = mapper;
            this._authUserInformation = authUserInformation;
            this._accountsRepository = accountsRepository;
            this._organisationRepository = organisationRepository;
            this._cardRepository = cardRepository;
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

            var ibanTransactionModel = CustomPaymentMapping.CreateIbanTransaction(userInformation, model.Price, checkAmount);

            var receiptInformationModel = CustomPaymentMapping.CreateReceiptInformationIbanTransfer(model, userInformation);

            if (isIbanHaresBankOwned is not null)
            {
                await StartIbanPaymentComplate(userInformation , ibanTransactionModel , receiptInformationModel,  false);

                return new ReturnState<object>(true);
            }

            for (int i = 0; i < 2; i++)
            {
                if (i == 1) //commission
                {
                    await StartIbanPaymentComplate(userInformation, ibanTransactionModel, receiptInformationModel, true);
                    continue;
                }

                await StartIbanPaymentComplate(userInformation, ibanTransactionModel, receiptInformationModel, false);
            }

            return new ReturnState<object>(true);
        }
        #endregion

        #region OnlinePaymentCard
        public async Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model)
        {
            var result = await CheckCardIsExist(model);

            var card = (Cards) result.Data;

            if (card.CardType == CardType.CreditCard)
            {
                return await CreditCardPayment(model, card);
            }

            return await CreditDebidCardPayment(model, card);
        }
        #endregion

        #region OnlinePaymentCheckCardInformation
        public async Task<ReturnState<object>> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model)
        {
            var result = await CheckCardIsExist(model);

            if (result.ErrorMessage is not null)
            {
                return result;
            }

            return new ReturnState<object>(true);
        }
        #endregion

        #region Invoice Payment
        public async Task<ReturnState<object>> PostPayInvoice(InvoicePaymentViewModel model)
        {
            var accountInformation = _accountsRepository.All().Include("Customers")
                .FirstOrDefault(x => x.CustomersId == _authUserInformation.CustomerId);

            if (accountInformation is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "User not found!");
            }

            var organisation = await _organisationRepository.FindAllFirstOrDefaultAsync(x => x.Id == model.OrganisationId);

            var checkAmaount = accountInformation.Amount - organisation.InvoiceAmount;

            if (checkAmaount <= 0M)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Insufficient balance!");
            }

            accountInformation.Amount = checkAmaount;

            await _accountsRepository.UpdateAsync(accountInformation);

            var invoicePayment = CustomPaymentMapping.CreateInvoicePayment(accountInformation,organisation,checkAmaount);

            var receiptInformation = CustomPaymentMapping.CreateInvoicePaymentReceipt(accountInformation, organisation, model);

            var transactions = CustomPaymentMapping.CreateTransactionInvoicePayment(invoicePayment , receiptInformation);

            await _transactionService.CreateTransaction(transactions);

            return new ReturnState<object>(true);
        }
        #endregion

        private async Task<ReturnState<object>> CheckCardIsExist(PostCheckPaymentInformationViewModel model)
        {
            var cardInformationResult = await _cardRepository.FirstOrDefaultAsync(
               x => x.CardNumber == model.Card.CardNumber && x.CustomerName == model.Card.CustomerName &&
               x.LastUseMount == model.Card.LastUseMount && x.LastUseYear == model.Card.LastUseYear &&
               x.Cvv == model.Card.Cvv && x.IsActive);

            if (cardInformationResult is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound , "Card not found!");
            }

            return new ReturnState<object>(cardInformationResult);
        }

        private async Task StartIbanPaymentComplate(Accounts userInformation , IbanTransactionViewModel transactionInformation, ReceiptInformation receiptInformation , bool isCommission)
        {
            if (isCommission)
            {
                transactionInformation.TransactionsType = TransactionsType.PaymentWithIbanCommission;
                transactionInformation.AvailableBalance = userInformation.Amount - _options.Value.Rate;
                transactionInformation.Explanation = PaymentIbanConst.IBAN_TRANSFER_EXPLANATION_COMMISSION;
            }

            userInformation.Amount = transactionInformation.AvailableBalance;

            await _accountsRepository.UpdateAsync(userInformation);

            if (isCommission)
            {
                transactionInformation.Amount = _options.Value.Rate;
                receiptInformation.Balance = _options.Value.Rate;
                receiptInformation.ReciverName = HB.Service.Const.PaymentIbanConst.HARES_BANK;
                receiptInformation.ReciverIban = HB.Service.Const.PaymentIbanConst.HARES_BANK_IBAN;
                receiptInformation.TransactionExplanation = $"{receiptInformation.SenderName}'a iban ile para gönderimi. (Komisyon kesintisi. -Hares Bank) {_options.Value.Rate}₺";
            }

            var transaction = CustomPaymentMapping.CreateTransactionIbanTransfer(transactionInformation , receiptInformation);

            await _transactionService.CreateTransaction(transaction);
        }

        private async Task<ReturnState<object>> CreditCardPayment(PostCheckPaymentInformationViewModel model , Cards card)
        {
            var paymentAmountCreditCard = card.CardCurrentAmount - model.Price;

            if (paymentAmountCreditCard <= 0)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Insufficient card limit!");
            }

            card.CardCurrentAmount = paymentAmountCreditCard;

            await _cardRepository.UpdateAsync(card);

            var transactionCreditCard = CustomPaymentMapping.CreateTransactionCreditCard(card, model);

            await _transactionService.CreateTransaction(transactionCreditCard);

            return new ReturnState<object>(new PaymentResponseViewModel { PaymentRefNo = transactionCreditCard.Id.ToString(), PaymentResult = true });
        }

        private async Task<ReturnState<object>> CreditDebidCardPayment(PostCheckPaymentInformationViewModel model, Cards card)
        {
            var customerAccount = _accountsRepository.All().Include("Customers")
                .Where(x => x.Id == card.AccountId && x.IsActive).FirstOrDefault();

            if (customerAccount is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "User not found!");
            }

            var paymentAmount = customerAccount.Amount - model.Price;

            if (paymentAmount <= 0)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Insufficient balance!");
            }

            customerAccount.Amount = paymentAmount;

            await _accountsRepository.UpdateAsync(customerAccount);

            card.CardCurrentAmount = customerAccount.Amount;

            var receiptInformation = CustomPaymentMapping.CreateReceiptInformationOnlinePayment(model, customerAccount);

            var transaction = CustomPaymentMapping.CreateTransactionDebidCard(card, model, receiptInformation);

            await _transactionService.CreateTransaction(transaction);

            return new ReturnState<object>(new PaymentResponseViewModel { PaymentRefNo = transaction.Id.ToString(), PaymentResult = true });
        }

    }
}

