using System;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.PaymentViewModel
{
	public class InvoicePaymentViewModel
	{
		[CustomHbValidation]
		public int SubscriberNumber { get; set; }

        [CustomHbValidation]
        public int OrganisationId { get; set; }

        [CustomHbValidation]
        public int AccountId { get; set; }
    }
}

