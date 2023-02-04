using System;
using System.ComponentModel.DataAnnotations;
using HB.Infrastructure.Const;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.PaymentViewModel
{
	public class PostSendMoneyWithIbanViewModel
	{
        [CustomHbValidation]
        public string Iban { get; set; }

        [CustomHbValidation]
        public decimal Price { get; set; }

        [CustomHbValidation]
        public string Explanation { get; set; }

        [CustomHbValidation]
        public string UserNameAndSurname { get; set; }
	}
}

