using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Domain.Model;
using HB.Infrastructure.Extension;
using HB.Service;
using HB.Service.Card;
using HB.Service.Const;
using HB.Service.Engine;
using HB.SharedObject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        => this._cardService = cardService;
        

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.ALL_USERS)]
        [HttpGet]
        public ReturnState<object> GetEmptyCardList()
        => _cardService.EmptyCardList();

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.CUSTOMER)]
        public ReturnState<object> PostListCustomerCards()
        => _cardService.PostListCustomerCards(HttpContext.GetCurrentUserId());


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.ADMIN)]
        public async Task<ReturnState<object>> PostCreateRandomEmptyCard([FromBody] int count)
        => await _cardService.PostCreateRandomEmptyCard(count);


        
    }
}

