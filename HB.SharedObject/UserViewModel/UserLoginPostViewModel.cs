using System;
using HB.Infrastructure.Validation;

namespace HB.SharedObject.UserViewModel
{
	public class UserLoginPostViewModel
	{
        [CustomHbValidation]
        public string Username { get; set; }

        [CustomHbValidation]
        public string Password { get; set; }
	}
}

