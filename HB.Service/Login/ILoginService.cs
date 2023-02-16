using System;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using HB.SharedObject.UserViewModel;

namespace HB.Service.Login;

public interface ILoginService
{
    Task<ReturnState<object>> CustomerLogin(LoginInputViewModel model);
    Task<ReturnState<object>> UserLogin(UserLoginPostViewModel model);
}

