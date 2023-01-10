using System;
using HB.SharedObject;

namespace HB.Service.Payment
{
	public interface IPaymentService
	{
		bool CreatePayment(CreatePaymentViewModel model);
	}
}

