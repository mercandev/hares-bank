using System;
using HB.Domain.Model;
using HB.Infrastructure.Exceptions;
using HB.Service.Helpers;
using HB.Service.Transaction;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;
using Marten;
using Microsoft.EntityFrameworkCore;

namespace HB.Service.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly ITransactionService _transactionService; 

        public PaymentService(
            HbContext hbContext,
            IDocumentSession documentSession,
            IQuerySession querySession,
            ITransactionService transactionService
            )
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
            this._transactionService = transactionService;
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

        public Task<ReturnState<object>> PostPaymentAccount()
        {
            throw new NotImplementedException();
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
                AccountId = card.AccountId
            };
        }
    }
}

