using System;
using Marten.Schema;

namespace HB.Domain.Model
{
    public record class Cards
	{
		public Cards()
		{
			Id = Guid.NewGuid();
		}

		[Identity]
		public Guid Id { get; set; }

		public int CustomerId { get; set; }
		public string? CardNumber { get; set; }
		public int LastUseDate { get; set; }
		public int LastUseYear { get; set; }
		public int Cvv { get; set; }
		public CardPaymentType CardPaymentType { get; set; }
		public CardType CardType { get; set; }
		public bool IsCardBlocked { get; set; }
		public decimal CardLimit { get; set; }
		public decimal CardCurrentAmount { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public enum CardPaymentType
	{
		MasterCard = 1,
		Visa = 2
	}

	public enum CardType
	{
		DebitCard = 1,
		CreditCard = 2
	}

}



