using System;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.AccountViewModel
{
	public class CreateAccountViewModel
	{
		[CustomHbValidation]
		public int CurrencyId { get; set; }
	}
}

