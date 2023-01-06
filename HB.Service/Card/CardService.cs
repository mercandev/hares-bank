using System;
using HB.Domain.Model;
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

        public async Task<bool> AddCard(Cards cards)
        {
            _documentSession.Insert(cards);
            await _documentSession.SaveChangesAsync();
            return true;
        }

        public bool AssignmentCardForCustomer(int customerId, Guid cardId)
        {
            var updateCard = _querySession.Query<Cards>().Where(x => x.Id == cardId).FirstOrDefault();
            updateCard.CustomerId = customerId;

            _documentSession.Update<Cards>(updateCard);
            _documentSession.SaveChanges();

            return true;
        }

        public List<Cards> EmptyCardList()
        {
           return _querySession.Query<Cards>().Where(x => x.CustomerId == 0).ToList();
        }

        public List<Cards> GetCardForCardId(Guid cardId)
        {
            return _querySession.Query<Cards>().Where(x => x.Id == cardId).ToList();
        }

        public Cards? ListCardsByCustomerId(int? customerId)
        {
            return _querySession.Query<Cards>().Where(x => x.CustomerId == customerId).FirstOrDefault();
            
        }

        public Cards? RandomEmptyCard(CardType cardType)
        {
            return _querySession.Query<Cards>().Where(x => x.CardType == cardType && x.CustomerId == 0).FirstOrDefault();
        }


    }
}

