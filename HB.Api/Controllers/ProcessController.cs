using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.Extension;
using HB.Service.Const;
using HB.Service.Process;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProcessController : Controller
    {
        private readonly IProcessService _processService;

        public ProcessController(IProcessService processService)
        => this._processService = processService;
        

        [HttpPost]
        [AuthHb(Roles = UserRoles.ALL_USERS)]
        public async Task<ReturnState<object>> GetCoalsDetail()
        => await _processService.CoalInformation();

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> BuyGold(ConvertMoneyToCoalViewModel model)
        => await _processService.BuyGold(model);
    }
}

