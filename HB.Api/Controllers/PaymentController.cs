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

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        => this._paymentService = paymentService;

        [HttpPost]
        public ReturnState<object> PostOnlinePaymentCheckCardInformation(PostCheckPaymentInformationViewModel model)
        => _paymentService.PostOnlinePaymentCheckCardInformation(model);

        [HttpPost]
        public async Task<ReturnState<object>> PostOnlinePaymentCard(PostCheckPaymentInformationViewModel model)
        => await _paymentService.PostOnlinePaymentCard(model);

        [HttpPost]
        public async Task<ReturnState<object>> PostCreateIbanTransfer(PostSendMoneyWithIbanViewModel model)
        => await _paymentService.CreateIbanTransfer(HttpContext.GetCurrentUserId(), model);

    }
}

