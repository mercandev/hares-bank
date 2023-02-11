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
        public decimal AvailableBalance { get; set; }
        public TransactionsType TransactionsType { get; set; }
		public ProccessType ProccessType { get; set; }
        public ReceiptInformation ReceiptInformation { get; set; }
    }

    public class ReceiptInformation
    {
        public string SenderName { get; set; }
        public string SenderIban { get; set; }
        public string ReciverName { get; set; }
        public string ReciverIban { get; set; }
        public string TransactionExplanation { get; set; }
        public decimal Balance { get; set; }
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

        [Description("Iban ile Gönderim")]
        PaymentWithIban = 7,

        [Description("Iban ile Gönderim Komisyonu - Başka Banka")]
        PaymentWithIbanCommission = 7,

        [Description("Değerli Maden Alım")]
        BuyyingCoal = 8,

        [Description("Değerli Maden Satış")]
        SellingCoal = 9,

    }

    public enum ProccessType
	{
        [Description("Gelen Ödeme")]
        Incoming = 1,

        [Description("Giden Ödeme")]
        Outgoid = 2,

        [Description("Kurum içi İşlem")]
        InHouseTransaction = 3
    }
	
}

