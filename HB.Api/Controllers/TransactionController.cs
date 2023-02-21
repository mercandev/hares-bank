using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Infrastructure.Authentication;
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
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        => this._transactionService = transactionService;
        

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> CustomerTransactions(CustomerTransactionsInputViewModel model)
        => await _transactionService.ListTransactionsByCustomerId(HttpContext.GetCurrentUserId() , model.StartDate , model.EndDate);
    }
}

