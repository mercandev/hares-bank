using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Service.Customer;
using HB.Service.Engine;
using HB.Service.User;
using HB.SharedObject;
using HB.SharedObject.UserViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        => this._userService = userService;
        

        [HttpPost]
        [AllowAnonymous]
        public ReturnState<object> PostUserLogin(UserLoginPostViewModel model)
        => _userService.UserLogin(model.Username, model.Password);
    }
}

