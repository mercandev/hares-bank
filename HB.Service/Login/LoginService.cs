using System;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.Jwt;
using System.Security.Claims;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using HB.SharedObject.UserViewModel;
using HB.Infrastructure.Repository;
using HB.Domain.Model;
using System.Net;
using HB.Infrastructure.Helpers;
using HB.Service.Helpers;

namespace HB.Service.Login;

public class LoginService : ILoginService
{
    private readonly IRepository<Customers> _customerRepository;
    private readonly IRepository<Users> _userRepository;
    private readonly IJwtSecurity _jwtSecurity;

    public LoginService(IRepository<Customers> customerRepository , IRepository<Users> userRepository, IJwtSecurity jwtSecurity)
    {
        this._customerRepository = customerRepository;
        this._userRepository = userRepository;
        this._jwtSecurity = jwtSecurity;
    }

    public async Task<ReturnState<object>> CustomerLogin(LoginInputViewModel model)
    {
        var customerResult = await _customerRepository.FindAllFirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

        if (customerResult is null)
        {
            return new ReturnState<object>(HttpStatusCode.NotFound, "Customer not found!");
        }

        var claims = ClaimsHelper.CreateCustomerClaims(customerResult.Id.ToString());

        var tokenResult = CreateToken(claims);

        return new ReturnState<object>(tokenResult);
    }

    public async Task<ReturnState<object>> UserLogin(UserLoginPostViewModel model)
    {
        var userResult = await _userRepository.FindAllFirstOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);

        if (userResult is null)
        {
            return new ReturnState<object>(HttpStatusCode.NotFound, "User not found!");
        }

        var claims = ClaimsHelper.CreateUserClaims(userResult.Id.ToString(), userResult.UserRole.GetEnumDescription());

        var tokenResult = CreateToken(claims);

        return new ReturnState<object>(tokenResult);
    }

    private JwtReturnViewModel CreateToken(Claim[] claims)
    {
        return new JwtReturnViewModel
        {
            Token = _jwtSecurity.CreateJwtToken(claims)
        };
    }
}

