using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Card
{
	public interface ICardService
	{
        ReturnState<object> PostListCustomerCards();
        ReturnState<object> EmptyCardList();
        Task<ReturnState<object>> PostCreateRandomEmptyCard(int? count);
    }
}

