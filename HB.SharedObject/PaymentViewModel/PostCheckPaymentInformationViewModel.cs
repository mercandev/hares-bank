using System;
using System.ComponentModel.DataAnnotations;
using HB.Infrastructure.Validation;
using HB.SharedObject.CardViewModel;

namespace HB.SharedObject.PaymentViewModel
{
	public class PostCheckPaymentInformationViewModel
	{
        [Required(ErrorMessage = "Decimal is required")]
        public decimal Price { get; set; }

        [CustomHbValidation]
        public string PaymentSource { get; set; }

        [Required(ErrorMessage = "Card Information is required")]
        public CardInformationViewModel Card { get; set; }
    }
}

