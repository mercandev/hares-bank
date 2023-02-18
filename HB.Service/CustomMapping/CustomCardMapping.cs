using System;
using HB.Domain.Model;
using Microsoft.VisualBasic;

namespace HB.Service.CustomMapping
{
	public static class CustomCardMapping
	{
		public static Cards DelegateCardCustomerMapping(Cards cards , Accounts customerResult , CardType cardType)
		{
            cards.CustomerId = customerResult.Customers.Id;
            cards.CustomerName = $"{customerResult.Customers.Name} {customerResult.Customers.Surname}";
            cards.CardType = cardType;
            cards.IsActive = true;
            if (cardType == CardType.DebitCard) cards.AccountId = customerResult.Id;

            return cards;
        }

    }
}

