using System;
using AutoMapper;
using HB.Domain.Model;
using Marten;

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


    }
}

