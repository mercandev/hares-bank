using System;
using HB.Domain.Model;

namespace HB.SharedObject.PaymentViewModel
{
	public class IbanTransactionViewModel
	{
		public int CustomerId { get; set; }
		public decimal Amount { get; set; }
        public decimal AvailableBalance { get; set; }
        public int AccountId { get; set; }
		public TransactionsType TransactionsType { get; set; }
		public string Explanation { get; set; }
	}
}

