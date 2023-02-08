using System;
namespace HB.SharedObject.PaymentViewModel
{
	public class ReceiptInformationViewModel
	{
		public string SenderName { get; set; }
		public string SenderIban { get; set; }
		public string ReciverName { get; set; }
		public string ReciverIban { get; set; }
		public string TransactionExplanation { get; set; }
		public decimal Balance { get; set; }
	}
}

