using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace HB.Service.Engine
{
	public class JwtSecurity
	{
        public string CreateJwtToken(Claim[] claims , JwtModel jwtModel)
		{
            var token = new JwtSecurityToken
            (
                issuer: jwtModel.Issuer,
                audience: jwtModel.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtModel.Key)),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
	}
}

