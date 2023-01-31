using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Service.Payment;
using HB.SharedObject.PaymentViewModel;
using HB.SharedObject;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this._paymentService = paymentService;
        }

        [HttpPost]
        public bool CreatePayment(CreatePaymentViewModel model)
        {
            return _paymentService.CreatePayment(model);
        }
    }
}

