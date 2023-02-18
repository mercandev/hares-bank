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
        public async Task<ReturnState<object>> PostAddCustomer(CreateCustomerViewModel createCustomerViewModel)
        => await _customerService.CreateCustomer(createCustomerViewModel);

        [HttpGet]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> CustomerInformation()
        => await _customerService.CustomerInformation();
    }
}

