using System;
using HB.Domain.Model;

namespace HB.SharedObject.PaymentViewModel
{
	public class CreatePaymentViewModel
	{
		public int CustomerId { get; set; }
		public int AccountId { get; set; }
		public Guid CardId { get; set; }
		public decimal Amount { get; set; }
        public TransactionsType TransactionsType { get; set; }
    }
}

