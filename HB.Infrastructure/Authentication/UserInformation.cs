using System;
using HB.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;

namespace HB.Infrastructure.Authentication
{
    public class UserInformation : IUserInformation
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public UserInformation()
        {

        }

        public UserInformation(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public int CustomerId { get; set; } 

        public UserInformation GetUserInformation()
        {
            return new UserInformation {
                CustomerId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Equals("CustomerId")).Select(x => x.Value).First())
            };
            
        }
    }
}

