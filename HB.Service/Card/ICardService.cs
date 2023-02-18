using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Card
{
	public interface ICardService
	{
        Task<ReturnState<object>> PostListCustomerCards();
        Task<ReturnState<object>> EmptyCardList();
        Task<ReturnState<object>> PostCreateRandomEmptyCard(int? count);
        Task<ReturnState<object>> DelegateCardCustomer(CardType cardType);
    }
}

