using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace HB.Infrastructure.Jwt
{
	public class JwtSecurity : IJwtSecurity
	{
        private readonly IOptions<JwtModel> _options;

        public JwtSecurity(IOptions<JwtModel> options)
        {
            this._options = options;
        }
        
        public string CreateJwtToken(Claim[] claims)
		{
            var token = new JwtSecurityToken
            (
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key)),
                SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
	}
}

