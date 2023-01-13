using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Domain.Model;
using HB.Service;
using HB.Service.Customer;
using HB.SharedObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        [HttpGet]
        public List<Customers>? GetCustomers()
        => _customerService.GetCustomers();
        

        [HttpGet]
        public List<Accounts?> GetCustomerAccounts(int customerId)
        => _customerService.GetCustomerAccounts(customerId);


        [HttpPost]
        public Customers? PostAddCustomer(CreateCustomerViewModel createCustomerViewModel)
        => _customerService.CreateCustomer(createCustomerViewModel);

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public CustomerInformationViewModel? CustomerInformation(int customerId)
        => _customerService.CustomerInformation(customerId);

        [HttpPost]
        public string? PostLoginCustomer(string username, string password)
        => _customerService.CustomerLogin(username, password);

    }
}

