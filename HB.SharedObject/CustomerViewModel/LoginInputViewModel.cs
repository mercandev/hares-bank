using System;
using System.ComponentModel.DataAnnotations;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.CustomerViewModel
{
	public class LoginInputViewModel
	{
        [CustomHbValidation]
        public string Email { get; set; }

        [CustomHbValidation]
        public string Password { get; set; }
	}
}

