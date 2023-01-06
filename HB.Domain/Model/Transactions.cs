using System;
using Marten.Schema;

namespace HB.Domain.Model
{
	public record class Transactions
	{
        public Transactions()
        {
            Id = Guid.NewGuid();
        }

        [Identity]
        public Guid Id { get; set; }

		public string? Explanation { get; set; }
		public int CustomerId { get; set; }
		public int AccountId { get; set; }
		public Guid CardId { get; set; }
		public decimal Amount { get; set; }
		public TransactionsType TransactionsType { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public enum TransactionsType
	{
        Remitment = 1,
		Payment = 2,
        Corporation = 3,
        Indefinite = 4,
        Witdraw = 5
    }
	
}

