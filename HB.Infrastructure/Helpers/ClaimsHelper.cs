using System;
using System.Security.Claims;
using HB.Domain.Model;
using HB.Infrastructure.Const;

namespace HB.Infrastructure.Helpers
{
	public static class ClaimsHelper
	{
		public static Claim[] CreateCustomerClaims(string customerId)
		{
            return new[]
            {
                new Claim(ClaimsConst.CUSTOMER_ID, customerId),
                new Claim(ClaimTypes.Role , ClaimsConst.CUSTOMER),
            };
        }

        public static Claim[] CreateUserClaims(string userId, string role)
        {
            return new[]
            {
                new Claim(ClaimsConst.USER_ID, userId),
                new Claim(ClaimTypes.Role , role),
            };
        }
    }
}

