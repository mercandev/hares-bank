using System;
using Marten.Schema;

namespace HB.Domain.Model
{
    public class Cards : MartenBaseModel
	{
		public Cards()
		{
			Id = Guid.NewGuid();
		}

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
		public bool IsNfcActive { get; set; } = false;
	}

	public enum CardPaymentType
	{
		MasterCard = 1,
		Visa = 2,
		Troy = 3
	}

	public enum CardType
	{
		DebitCard = 1,
		CreditCard = 2
	}

}



