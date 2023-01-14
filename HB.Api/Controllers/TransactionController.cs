using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Service.Payment;
using HB.Service.Transaction;
using HB.SharedObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ReturnState<object> CustomerTransactions(int customerId)
        => _transactionService.ListTransactionsByCustomerId(customerId);
    }
}

