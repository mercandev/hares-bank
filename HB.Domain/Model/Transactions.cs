using System;
using System.ComponentModel;
using Marten.Schema;

namespace HB.Domain.Model
{
	public class Transactions : MartenBaseModel
    {
        public Transactions()
        {
            Id = Guid.NewGuid();
        }

		public string? Explanation { get; set; }
		public int CustomerId { get; set; }
		public int AccountId { get; set; }
		public Guid CardId { get; set; }
		public decimal Amount { get; set; }
		public TransactionsType TransactionsType { get; set; }
		public ProccessType ProccessType { get; set; }
	}

	public enum TransactionsType 
	{
		[Description("Havale")]
        Remitment = 1,

        [Description("Ödeme")]
        Payment = 2,

        [Description("Kurum")]
        Corporation = 3,

        [Description("Bilinmeyen")]
        Indefinite = 4,

        [Description("Çekim")]
        Withdraw = 5,

        [Description("Online Harcama")]
        OnlinePayment = 6,
    }

	public enum ProccessType
	{
        [Description("Gelen Ödeme")]
        Incoming = 1,

        [Description("Giden Ödeme")]
        Outgoid = 2
	}
	
}

