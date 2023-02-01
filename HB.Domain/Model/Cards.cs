using System;
using System.ComponentModel;
using Marten.Schema;

namespace HB.Domain.Model
{
    public class Cards : MartenBaseModel
	{
		public Cards()
		{
			Id = Guid.NewGuid();
			IsActive = false;
		}

		public int CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string? CardNumber { get; set; }
		public int LastUseMount { get; set; }
		public int LastUseYear { get; set; }
		public int Cvv { get; set; }
		public CardPaymentType CardPaymentType { get; set; }
		public CardType CardType { get; set; }
		public bool IsCardBlocked { get; set; }
		public decimal CardLimit { get; set; }
		public decimal CardCurrentAmount { get; set; }
		public bool IsNfcActive { get; set; } = false;
		public int AccountId { get; set; } = 0;
	}

	public enum CardPaymentType
	{
        [Description("MasterCard")]
        MasterCard = 1,
        [Description("Visa")]
        Visa = 2,
        [Description("Troy")]
        Troy = 3
	}

	public enum CardType
	{
        [Description("Debit Card")]
        DebitCard = 1,
        [Description("Credit Card")]
        CreditCard = 2
	}
}



