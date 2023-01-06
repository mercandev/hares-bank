using System;
using HB.Domain.Model;

namespace HB.Service.Card
{
	public interface ICardService
	{
        Task<bool> AddCard(Cards cards);
        Cards? ListCardsByCustomerId(int? customerId);
        List<Cards> EmptyCardList();
        Cards RandomEmptyCard(CardType cardType);
        bool AssignmentCardForCustomer(int customerId, Guid cardId);
        List<Cards> GetCardForCardId(Guid cardId);
    }
}

