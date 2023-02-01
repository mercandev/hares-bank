using System;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.Payment
{
	public interface IPaymentService
	{
        ReturnState<object> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model);
        Task<ReturnState<object>> PostPaymentAccount();
        Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model);
    }
}

