using System;
namespace HB.SharedObject.PaymentViewModel
{
	public class InvoicePaymentTransactionViewModel
	{
		public int CustomerId { get; set; }
		public string Explanation { get; set; }
		public decimal Amount { get; set; }
		public decimal AvailableBalance { get; set; }
		public int AccountId { get; set; }
	}	
}

