using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Infrastructure.Authentication;
using HB.Service.Const;
using HB.Service.Organisation;
using HB.SharedObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HB.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OrganisationsController : Controller
{
    private readonly IOrganisationService _organisationService;

    public OrganisationsController(IOrganisationService organisationService)
    => this._organisationService = organisationService;

    [HttpGet]
    [AuthHb(Roles = UserRoles.ALL_USERS)]
    public async Task<ReturnState<object>> GetOrganisations()
        => await _organisationService.GetOrganisations();
}

