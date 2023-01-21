using System;
using HB.Domain.Model;
using HB.Service.Engine;
using HB.Service.Helpers;
using HB.SharedObject;
using Marten;

namespace HB.Service.Card
{
    public class CardService : ICardService
    {
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;

        public CardService(HbContext hbContext, IDocumentSession documentSession, IQuerySession querySession)
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
        }

        public ReturnState<object> EmptyCardList()
        {
           var result = _querySession.Query<Cards>().Where(x => x.CustomerId == 0).ToList();
            return new ReturnState<object>(result);
        }

        public ReturnState<object> PostListCustomerCards(int? customerId)
        {
            var result = _querySession.Query<Cards>().Where(x => x.CustomerId == customerId).ToList();
            return new ReturnState<object>(result);
        }
    }
}

