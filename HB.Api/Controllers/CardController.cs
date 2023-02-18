using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
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
    [Route("api/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        => this._cardService = cardService;


        [AuthHb(Roles = UserRoles.ALL_STAFF)]
        [HttpGet]
        public async Task<ReturnState<object>> GetEmptyCardList()
        =>await _cardService.EmptyCardList();

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> PostListCustomerCards()
        => await _cardService.PostListCustomerCards();

        [HttpPost]
        [AuthHb(Roles = UserRoles.ADMIN)]
        public async Task<ReturnState<object>> PostCreateRandomEmptyCard([FromBody] int count)
        => await _cardService.PostCreateRandomEmptyCard(count);

        [HttpPost]
        [AuthHb(Roles = UserRoles.CUSTOMER)]
        public async Task<ReturnState<object>> DelegateCardCustomer(CardType cardType)
        => await _cardService.DelegateCardCustomer(cardType);
    }
}

