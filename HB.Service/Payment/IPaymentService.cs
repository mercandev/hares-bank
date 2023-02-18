using System;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.Payment
{
	public interface IPaymentService
	{
        ReturnState<object> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model);
        Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model);
        Task<ReturnState<object>> CreateIbanTransfer(PostSendMoneyWithIbanViewModel model);
        ReturnState<object> GetOrganisations();
        ReturnState<object> PostPayInvoice(int customerId, InvoicePaymentViewModel model);


    }
}

