using System;
using System.Security.Claims;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.Jwt;
using HB.Service.Engine;
using HB.Service.Helpers;
using HB.SharedObject;
using Marten;
using Microsoft.Extensions.Options;

namespace HB.Service.User
{
    public class UserService : IUserService
    {
        private readonly HbContext? _hBContext;
        private readonly IOptions<JwtModel> _options;

        public UserService(HbContext hbContext,IOptions<JwtModel> options)
        {
            this._hBContext = hbContext;
            this._options = options;
        }


        public ReturnState<object> UserLogin(string username, string password)
        {
            var userResult = _hBContext.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();

            if (userResult == null)
            {
                throw new HbBusinessException("User not found!");
            }

            var claims = new[]
            {
                new Claim("UserId", userResult.Id.ToString()),
                new Claim(ClaimTypes.Role , userResult.UserRole.GetEnumDescription()),
            };

            var returnModel = new JwtReturnViewModel { Token = new JwtSecurity().CreateJwtToken(claims, _options.Value) };

            return new ReturnState<object>(returnModel);
        }
    }
}

