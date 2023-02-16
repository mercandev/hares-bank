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
            this.CustomerId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Equals("CustomerId")).Select(x => x.Value).FirstOrDefault());
        }

        public int CustomerId { get; set; } 
    }
}

