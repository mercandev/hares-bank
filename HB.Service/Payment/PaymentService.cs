using System;
using System.Text.RegularExpressions;
using HB.Domain.Model;
using HB.Infrastructure.Const;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.Jwt;
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

        public PaymentService(
            HbContext hbContext,
            IDocumentSession documentSession,
            IQuerySession querySession,
            ITransactionService transactionService,
            IOptions<Commission> options
            
            )
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
            this._transactionService = transactionService;
            this._options = options;
        }

        public async Task<ReturnState<object>> CreateIbanTransfer(int customerId , PostSendMoneyWithIbanViewModel model)
        {
            PaymentHelper.IbanValidation(model.Iban);

            var userInformation = _hBContext.Accounts.Where(x => x.CustomersId == customerId).FirstOrDefault();

            if (userInformation == null) throw new HbBusinessException("User not found!");

            var checkAmount = userInformation.Amount - model.Price;

            if (checkAmount <= 0M) throw new HbBusinessException("Insufficient balance!");

            var isIbanHaresBankOwned = _hBContext.Accounts.Where(x => x.Iban == model.Iban).FirstOrDefault();

            if (isIbanHaresBankOwned == null)
            {
                var checkBalance = checkAmount - _options.Value.Rate;

                if (checkBalance <= 0M) throw new HbBusinessException("The balance is not enough for the commission amount");
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

            if (isIbanHaresBankOwned != null)
            {
                StartIbanPaymentComplate(userInformation , ibanTransactionModel , false);

                return new ReturnState<object>(true);
            }

            for (int i = 0; i < 2; i++)
            {
                if (i == 1) //commission
                {
                    StartIbanPaymentComplate(userInformation, ibanTransactionModel, true);
                    continue;
                }

                StartIbanPaymentComplate(userInformation, ibanTransactionModel, false);
            }

            return new ReturnState<object>(true);
        }

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

                return new ReturnState<object>(true);
            }

            if (card.CardType == CardType.DebitCard)
            {
                var customerAccount = _hBContext.Accounts.Where(x => x.Id == card.AccountId && x.IsActive).FirstOrDefault();

                if (customerAccount == null) throw new HbBusinessException("User account not found!");

                var paymentAmount = customerAccount.Amount - model.Price;

                if (paymentAmount < 0) throw new HbBusinessException("Insufficient balance!");

                customerAccount.Amount = paymentAmount;

                _hBContext.Entry(customerAccount).State = EntityState.Modified;
                await _hBContext.SaveChangesAsync();

                var transaction = CreateTransactionDebidCard(card, model);

                _transactionService.CreateTransaction(transaction);

                return new ReturnState<object>(true);
            }

            throw new HbBusinessException("Out-of-process!");
        }

        public ReturnState<object> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model)
        {
            CheckCardIsExist(model);

            return new ReturnState<object>(true);
        }

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

        private Transactions CreateTransactionDebidCard(Cards card, PostCheckPaymentInformationViewModel model)
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
                AccountId = card.AccountId
            };
        }

        private Transactions CreateTransactionIbanTransfer(IbanTransactionViewModel model)
        {
            return new Transactions()
            {
                ProccessType = ProccessType.Outgoid,
                TransactionsType = model.TransactionsType,
                CustomerId = model.CustomerId,
                Explanation = model.Explanation,
                Amount = model.Amount,
                AvailableBalance = model.AvailableBalance,
                AccountId = model.AccountId
            };
        }

        private void StartIbanPaymentComplate(Accounts userInformation , IbanTransactionViewModel transactionInformation, bool isCommission)
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
            }

            var transaction = CreateTransactionIbanTransfer(transactionInformation);

            _transactionService.CreateTransaction(transaction);
        }
    }
}

