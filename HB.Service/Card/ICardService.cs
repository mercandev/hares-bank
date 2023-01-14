using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Card
{
	public interface ICardService
	{
        Task<bool> AddCard(Cards cards);
        ReturnState<object> ListCardsByCustomerId(int? customerId);
        List<Cards> EmptyCardList();
        Cards RandomEmptyCard(CardType cardType);
        bool AssignmentCardForCustomer(int customerId, Guid cardId);
        List<Cards> GetCardForCardId(Guid cardId);
        ReturnState<object> GenerateIbanNumber();
        ReturnState<object> GenerateCard();
    }
}

