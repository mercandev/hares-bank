using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Service.Payment;
using HB.SharedObject.PaymentViewModel;
using HB.SharedObject;
using Microsoft.AspNetCore.Mvc;
using HB.Infrastructure.Extension;
using HB.Service.Const;
using HB.SharedObject.TransactionViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using HB.Infrastructure.Authentication;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        => this._paymentService = paymentService;

        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public ReturnState<object> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model)
        => _paymentService.PostOnlinePaymentCheckCardInformation(model);

        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public async Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model)
        => await _paymentService.PostOnlinePaymentCard(model);

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> PostCreateIbanTransfer(PostSendMoneyWithIbanViewModel model)
        => await _paymentService.CreateIbanTransfer(HttpContext.GetCurrentUserId(), model);

        [HttpGet]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public ReturnState<object> GetOrganisations()
        => _paymentService.GetOrganisations();

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public  ReturnState<object> PostPayInvoice(InvoicePaymentViewModel model)
        => _paymentService.PostPayInvoice(HttpContext.GetCurrentUserId(), model);

    }
}

