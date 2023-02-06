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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.ALL_USERS)]
        public ReturnState<object> PostAddCustomer(CreateCustomerViewModel createCustomerViewModel)
        => _customerService.CreateCustomer(createCustomerViewModel);

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.CUSTOMER)]
        public ReturnState<object> CustomerInformation()
        => _customerService.CustomerInformation(HttpContext.GetCurrentUserId());

        [HttpPost]
        [AllowAnonymous]
        public ReturnState<object> PostLoginCustomer(LoginInputViewModel model)
        => _customerService.CustomerLogin(model.Email, model.Password);


        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public async Task<ReturnState<object>> DelegateCardCustomer([FromBody] int customerId, CardType cardType)
        => await _customerService.DelegateCardCustomer(customerId,cardType);

    }
}

