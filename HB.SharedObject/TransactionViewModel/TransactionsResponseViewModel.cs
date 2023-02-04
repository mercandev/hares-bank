using System;
namespace HB.SharedObject.TransactionViewModel
{
	public class TransactionsResponseViewModel
	{
		public Guid Id { get; set; }
		public string Explanation { get; set; }
		public int AccountId { get; set; }
		public decimal Amount { get; set; }
		public string TransactionsType { get; set; }
		public string ProccessType { get; set; }
		public decimal AvailableBalance { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

