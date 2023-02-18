using System;
using HB.Domain.Model;
using HB.Service.Const;

namespace HB.Service.Helpers
{
	public static class FileHelper
	{
        public static string ReceiptFormatWithTransactionsData(Transactions transactions)
        {
            return ReceiptConst.RECEIPT_HTML
            .Replace("{0}", transactions.Id.ToString())
            .Replace("{1}", transactions.CreatedDate.ToString())
            .Replace("{2}", transactions.ReceiptInformation.SenderName)
            .Replace("{3}", transactions.ReceiptInformation.SenderIban)
            .Replace("{4}", transactions.ReceiptInformation.ReciverName)
            .Replace("{5}", transactions.ReceiptInformation.ReciverIban)
            .Replace("{6}", transactions.ReceiptInformation.TransactionExplanation)
            .Replace("{7}", transactions.ReceiptInformation.Balance.ToString())
            .Replace("\n", @"")
            .Replace("\"", @"'");
        }
    }
}

