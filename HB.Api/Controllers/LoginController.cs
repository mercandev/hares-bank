using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Service.Login;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using HB.SharedObject.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            this._loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ReturnState<object>> PostLoginCustomer([FromBody] LoginInputViewModel model)
        => await _loginService.CustomerLogin(model);

        [HttpPost]
        [AllowAnonymous]
        public async Task<ReturnState<object>> PostLoginUser([FromBody] UserLoginPostViewModel model)
       => await _loginService.UserLogin(model);
    }
}

