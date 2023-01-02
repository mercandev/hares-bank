using System;
namespace HB.Domain.Model
{
	public record class Cards
	{
		public Guid Id { get; set; }
		public int CustomerId { get; set; }
		public string? CardNumber { get; set; }
		public int LastUseDate { get; set; }
		public int LastUseYear { get; set; }
		public int Cvv { get; set; }
		public CardPaymentType CardPaymentType { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public enum CardPaymentType
	{
		MasterCard,
		Visa 
	}

}



