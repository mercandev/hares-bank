using System;
using HB.Domain.Model;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.Payment
{
	public interface IPaymentService
	{
        Task<ReturnState<object>> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model);
        Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model);
        Task<ReturnState<object>> CreateIbanTransfer(PostSendMoneyWithIbanViewModel model);
        Task<ReturnState<object>> PostPayInvoice(InvoicePaymentViewModel model);
    }
}
