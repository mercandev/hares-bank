using System;
using AutoMapper;
using HB.Domain.Model;
using HB.SharedObject;
using Marten;
using Microsoft.VisualBasic;

namespace HB.Service.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;

        public TransactionService(
            IDocumentSession documentSession,
            IQuerySession querySession
            )
        {
            this._documentSession = documentSession;
            this._querySession = querySession;
        }

        public bool CreateTransaction(Transactions transaction)
        {
            _documentSession.Insert(transaction);
            _documentSession.SaveChanges();
            return true;
        }

        public ReturnState<object> ListTransactionsByCustomerId(int customerId)
        {
            var result = _querySession.Query<Transactions>().Where(x => x.CustomerId == customerId).ToList();

            return new ReturnState<object>(result);

        }
    }
}

