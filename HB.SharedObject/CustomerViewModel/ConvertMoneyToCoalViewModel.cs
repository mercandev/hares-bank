using System;
using HB.Domain.Model;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.CustomerViewModel
{
	public class ConvertMoneyToCoalViewModel
    {
        [CustomHbValidation]
        public decimal Price { get; set; }

        [CustomHbValidation]
        public int AccountId { get; set; }

        [CustomHbValidation]
        public decimal CoalPrice { get; set; }
	}
}

