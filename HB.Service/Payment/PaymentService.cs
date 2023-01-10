using System;
using HB.Domain.Model;
using HB.Service.Helpers;
using HB.Service.Transaction;
using HB.SharedObject;
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

        public bool CreatePayment(CreatePaymentViewModel model)
        {
            var accountInformation = new Accounts();

            var customer = _hBContext.Customers.Where(x => x.Id == model.CustomerId)
                .Include("Accounts")
                .FirstOrDefault();

            if (customer == null)
            {
                throw new Exception("Customer not found!");
            }

            if (model.AccountId != default(int))
            {
               accountInformation = customer.Accounts.Where(x => x.Id == model.AccountId).FirstOrDefault();
            }
            
            if (model.Amount != default && model.CardId == default(Guid))
            {
                //account payment
                var accountAmountCheck = accountInformation.Amount - model.Amount < 0 ? false : true;

                if (!accountAmountCheck)
                {
                    throw new Exception("insufficient balance"); 
                }

                _transactionService.CreateTransaction(
                    new Transactions
                    {
                        AccountId = accountInformation.Id,
                        CustomerId = customer.Id,
                        TransactionsType = model.TransactionsType,
                        Explanation = $"{model.TransactionsType.GetEnumDescription()} - {ProccessType.Outgoid.GetEnumDescription()}",
                        ProccessType = ProccessType.Outgoid,
                        Amount = model.Amount,
                    });


                accountInformation.Amount = accountInformation.Amount - model.Amount;

                _hBContext.Accounts.Update(accountInformation);
                _hBContext.SaveChanges();

                return true;
            }
            else
            {
                //card payment 
            }


            return true;
        }

    }
}

