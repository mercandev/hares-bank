using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.Extension;
using HB.Service;
using HB.Service.Const;
using HB.Service.Customer;
using HB.Service.Engine;
using HB.Service.Firebase;
using HB.SharedObject;
using HB.SharedObject.AccountViewModel;
using HB.SharedObject.CustomerViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        => this._customerService = customerService;

        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_STAFF)]
        public ReturnState<object> PostAddCustomer(CreateCustomerViewModel createCustomerViewModel)
        => _customerService.CreateCustomer(createCustomerViewModel);

        [HttpGet]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public ReturnState<object> CustomerInformation()
        => _customerService.CustomerInformation(HttpContext.GetCurrentUserId());

        [HttpPost]
        [AllowAnonymous]
        public ReturnState<object> PostLoginCustomer(LoginInputViewModel model)
        => _customerService.CustomerLogin(model.Email, model.Password);

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> DelegateCardCustomer([FromBody] int customerId, CardType cardType)
        => await _customerService.DelegateCardCustomer(customerId,cardType);

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> CreateCustomerAccount(CreateAccountViewModel model)
        => await _customerService.CreateAccount(HttpContext.GetCurrentUserId() , model);

        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public async Task<ReturnState<object>> GetCoalsDetail(CoalDetailViewModel model)
        => await _customerService.CoalInformation(model);

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> BuyGold(ConvertMoneyToCoalViewModel model)
        => await _customerService.BuyGold(HttpContext.GetCurrentUserId(), model);

    }
}

