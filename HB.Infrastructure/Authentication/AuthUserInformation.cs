using System;
using HB.Infrastructure.Const;
using HB.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;

namespace HB.Infrastructure.Authentication
{
    public class AuthUserInformation : IAuthUserInformation
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public AuthUserInformation()
        {

        }

        public AuthUserInformation(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.CustomerId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Equals(AuthHbConst.CUSTOMER_ID)).Select(x => x.Value).First());
        }

        public int CustomerId { get; set; } 
    }
}

