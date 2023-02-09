using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.Extension;
using HB.Service.Const;
using HB.Service.File;
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
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        => this._fileService = fileService;


        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public async Task<IActionResult> TransactionsPdf(Guid transactionsId)
        {
            var result = await _fileService.GenerateReceiptPdf(transactionsId);

            return File((Stream)result.Data, "application/pdf", $"{transactionsId}_Dekont");
        }

        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public async Task<ReturnState<object>> TransactionsHtml(Guid transactionsId)
         => await _fileService.TransactionsHtml(transactionsId);

    }
}

