using System;
namespace HB.Domain.Model
{
	public record class Transactions
	{
		public Guid Id { get; set; }
		public string? Explanation { get; set; }
		public int AccountId { get; set; }
		public decimal Amount { get; set; }
		public TransactionsType TransactionsType { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public enum TransactionsType
	{
        Remitment,
		Payment,
        Corporation,
        Indefinite
    }
	
}

