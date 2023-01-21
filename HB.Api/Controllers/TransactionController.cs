using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Infrastructure.Extension;
using HB.Service.Const;
using HB.Service.Engine;
using HB.Service.Payment;
using HB.Service.Transaction;
using HB.SharedObject;
using HB.SharedObject.TransactionViewModel;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.CUSTOMER)]
        public ReturnState<object> CustomerTransactions(CustomerTransactionsInputViewModel model)
        => _transactionService.ListTransactionsByCustomerId(HttpContext.GetCurrentUserId() , model.StartDate , model.EndDate);
    }
}

