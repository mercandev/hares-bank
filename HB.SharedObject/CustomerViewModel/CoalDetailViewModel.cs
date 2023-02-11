using System;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.CustomerViewModel
{
	public class CoalDetailViewModel
	{
        [CustomHbValidation]
        public int CoalId { get; set; }
	}
}

