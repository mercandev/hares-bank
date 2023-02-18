using System;
using HB.Domain.Model;
using HB.Service.Const;

namespace HB.Service.CustomMapping
{
	public static class CustomProcessMapping
	{
        public static Transactions GoldBuyyingTransaction(Accounts model, decimal goldPrice, decimal moneyPrice)
        {
            return new Transactions
            {
                AccountId = model.Id,
                CustomerId = model.CustomersId,
                ProccessType = ProccessType.InHouseTransaction,
                TransactionsType = TransactionsType.BuyyingCoal,
                Explanation = string.Format(ProcessConst.BUY_GOLD_EXPLANATION, goldPrice),
                ReceiptInformation = new ReceiptInformation
                {
                    Balance = moneyPrice,
                    SenderIban = model.Iban,
                    SenderName = $"{model.Customers.Name} {model.Customers.Surname}",
                    ReciverName = PaymentIbanConst.HARES_BANK,
                    ReciverIban = PaymentIbanConst.HARES_BANK_IBAN,
                    TransactionExplanation = string.Format(ProcessConst.BUY_GOLD_TRANSACTION_EXPLANATION, goldPrice)
                }
            };
        }
    }
}

