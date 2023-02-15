using System;
using System.Security.Claims;
using HB.Infrastructure.Const;
using HB.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HB.Infrastructure.Authentication
{
    public class AuthHb : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }

            if (!string.IsNullOrWhiteSpace(Roles))
            {
                var splitUser = Roles.Trim().Split(",").ToArray();
                var claimPayload = context.HttpContext.User.Claims.Where(x => x.Type.Equals(ClaimTypes.Role)).Select(x => x.Value).ToArray();

                if (!claimPayload.Intersect(splitUser).Any())
                {
                    context.Result = new ForbidResult();
                }               
            }
        }
    }
}

