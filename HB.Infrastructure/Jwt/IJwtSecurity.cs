using System;
using System.Security.Claims;

namespace HB.Infrastructure.Jwt
{
	public interface IJwtSecurity
	{
        string CreateJwtToken(Claim[] claims);

    }
}

