using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Service.Payment;
using HB.SharedObject;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

