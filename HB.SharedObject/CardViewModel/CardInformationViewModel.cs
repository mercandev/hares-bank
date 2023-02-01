using System;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.CardViewModel
{
	public class CardInformationViewModel
	{
        [CustomHbValidation]
        public string CustomerName { get; set; }

        [CustomHbValidation]
        public string CardNumber { get; set; }

        [CustomHbValidation]
        public int LastUseMount { get; set; }

        [CustomHbValidation]
        public int LastUseYear { get; set; }

        [CustomHbValidation]
        public int Cvv { get; set; }
    }
}

