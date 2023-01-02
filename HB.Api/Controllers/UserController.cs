using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Domain.Model;
using HB.Service;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IInformationService _information;

        public UserController(IInformationService information)
        {
            this._information = information;
        }

        [HttpGet]
        public string TestMethod()
        {
            return "test kaan mercan";
        }

        [HttpGet]
        public List<Customers>? GetCustomers()
        {
            return _information.GetCustomers();
        }

        [HttpGet]
        public List<Accounts?> GetCustomerAccounts(int customerId)
        {
            return _information.GetCustomerAccounts(customerId);
        }

        [HttpPost]
        public async Task<bool> PostCreateCard(Cards cards)
        {
            return await _information.AddCard(cards);
        }

        [HttpGet]
        public Cards GetCustomerCard(int customerId)
        {
            return _information.ListCardsByCustomerId(customerId);
        }

    }
}

