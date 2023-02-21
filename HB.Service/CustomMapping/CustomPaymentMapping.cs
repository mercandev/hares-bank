using System;
using HB.Domain.Model;
using HB.Service.Const;
using HB.Service.Helpers;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.CustomMapping
{
	public static class CustomPaymentMapping
	{
		public static IbanTransactionViewModel CreateIbanTransaction(Accounts userInformation , decimal price , decimal checkAmount)
		{
            return new IbanTransactionViewModel()
            {
                AccountId = userInformation.Id,
                Amount = price,
                AvailableBalance = checkAmount,
                CustomerId = userInformation.CustomersId,
                Explanation = PaymentIbanConst.IBAN_TRANSFER_EXPLANATION,
                TransactionsType = TransactionsType.PaymentWithIban
            };
        }

        public static ReceiptInformation CreateReceiptInformationIbanTransfer(PostSendMoneyWithIbanViewModel ibanInformation, Accounts userInformation)
        {
            return new ReceiptInformation()
            {
                Balance = ibanInformation.Price,
                SenderIban = ibanInformation.Iban,
                SenderName = $"{userInformation.Customers.Name} {userInformation.Customers.Surname}",
                ReciverName = ibanInformation.UserNameAndSurname,
                ReciverIban = ibanInformation.Iban,
                TransactionExplanation = $"{ibanInformation.UserNameAndSurname}'a iban ile para gönderimi. {ibanInformation.Price}₺"
            };
        }

        public static ReceiptInformation CreateReceiptInformationOnlinePayment(PostCheckPaymentInformationViewModel paymentInformation, Accounts userInformation)
        {
            return new ReceiptInformation()
            {
                Balance = paymentInformation.Price,
                SenderIban = userInformation.Iban,
                SenderName = $"{userInformation.Customers.Name} {userInformation.Customers.Surname}",
                ReciverName = paymentInformation.PaymentSource,
                ReciverIban = string.Empty,
                TransactionExplanation = $"{paymentInformation.PaymentSource} online harcama tahsilatı. {paymentInformation.Price}₺"
            };
        }

        public static Transactions CreateTransactionCreditCard(Cards card, PostCheckPaymentInformationViewModel model)
        {
            return new Transactions()
            {
                CardId = card.Id,
                ProccessType = ProccessType.Outgoid,
                TransactionsType = TransactionsType.OnlinePayment,
                CustomerId = card.CustomerId,
                Explanation = model.PaymentSource,
                AvailableBalance = card.CardCurrentAmount,
                Amount = model.Price
            };
        }

        public static Transactions CreateTransactionDebidCard(Cards card, PostCheckPaymentInformationViewModel model, ReceiptInformation receiptModel)
        {
            return new Transactions()
            {
                CardId = card.Id,
                ProccessType = ProccessType.Outgoid,
                TransactionsType = TransactionsType.OnlinePayment,
                CustomerId = card.CustomerId,
                Explanation = model.PaymentSource,
                Amount = model.Price,
                AvailableBalance = card.CardCurrentAmount,
                AccountId = card.AccountId,
                ReceiptInformation = receiptModel
            };
        }

        public static Transactions CreateTransactionIbanTransfer(IbanTransactionViewModel model, ReceiptInformation receiptInformation)
        {
            return new Transactions()
            {
                ProccessType = ProccessType.Outgoid,
                TransactionsType = model.TransactionsType,
                CustomerId = model.CustomerId,
                Explanation = model.Explanation,
                Amount = model.Amount,
                AvailableBalance = model.AvailableBalance,
                AccountId = model.AccountId,
                ReceiptInformation = receiptInformation
            };
        }

        public static Transactions CreateTransactionInvoicePayment(InvoicePaymentTransactionViewModel model, ReceiptInformation receiptInformationModel)
        {
            return new Transactions()
            {
                ProccessType = ProccessType.Outgoid,
                TransactionsType = TransactionsType.Corporation,
                CustomerId = model.CustomerId,
                Explanation = model.Explanation,
                Amount = model.Amount,
                AvailableBalance = model.AvailableBalance,
                AccountId = model.AccountId,
                ReceiptInformation = receiptInformationModel
            };
        }

        public static InvoicePaymentTransactionViewModel CreateInvoicePayment(Accounts accountInformation , Organisations organisation , decimal checkAmaount)
        {
            return new InvoicePaymentTransactionViewModel()
            {
                AccountId = accountInformation.Id,
                Amount = organisation.InvoiceAmount,
                AvailableBalance = checkAmaount,
                CustomerId = accountInformation.CustomersId,
                Explanation = $"{organisation.OrganisationType.GetEnumDescription()} - ({organisation.Name})"
            };
        }

        public static ReceiptInformation CreateInvoicePaymentReceipt(Accounts accountInformation, Organisations organisation, InvoicePaymentViewModel model)
        {
            return new ReceiptInformation
            {
                Balance = organisation.InvoiceAmount,
                SenderIban = accountInformation.Iban,
                SenderName = $"{accountInformation.Customers.Name} {accountInformation.Customers.Surname}",
                ReciverName = organisation.Name,
                ReciverIban = organisation.Iban,
                TransactionExplanation = $"{model.SubscriberNumber} abonelik için {organisation.Name} ücret tahsilatı. {organisation.InvoiceAmount}₺"
            };
        }

    }
}

