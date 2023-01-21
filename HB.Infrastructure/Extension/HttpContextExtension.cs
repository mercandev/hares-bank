using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace HB.Infrastructure.Extension
{
	public static class HttpContextExtension
	{
        public static int GetCurrentUserId(this HttpContext context)
        {
            if (context.User == null)
            {
                return default;
            }

            return int.Parse(context.User.Claims.Single(x => x.Type == "CustomerId").Value);
        }
    }
}

