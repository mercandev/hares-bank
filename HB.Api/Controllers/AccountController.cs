using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.Extension;
using HB.Service.Account;
using HB.Service.Const;
using HB.SharedObject;
using HB.SharedObject.AccountViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        => this._accountService = accountService;

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> CreateCustomerAccount(CreateAccountViewModel model)
        => await _accountService.CreateAccount(model);
    }
}

