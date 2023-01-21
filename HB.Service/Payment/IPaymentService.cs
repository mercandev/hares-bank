using System;
using HB.SharedObject;
using HB.SharedObject.PaymentViewModel;

namespace HB.Service.Payment
{
	public interface IPaymentService
	{
		bool CreatePayment(CreatePaymentViewModel model);
	}
}

