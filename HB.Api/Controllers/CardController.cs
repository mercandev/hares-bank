using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HB.Domain.Model;
using HB.Service;
using HB.Service.Card;
using HB.SharedObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            this._cardService = cardService;
        }

        [HttpPost]
        public async Task<bool> PostCreateCard(Cards cards)
        {
            return await _cardService.AddCard(cards);
        }

        [HttpGet]
        public ReturnState<object> GetCustomerCard(int customerId)
        {
            return _cardService.ListCardsByCustomerId(customerId);
        }

        [HttpGet]
        public List<Cards>? GetEmptyCardList()
        {
            return _cardService.EmptyCardList();
        }

        [HttpPost]
        public List<Cards> PostCardsFromCardId(Guid cardId)
        {
            return _cardService.GetCardForCardId(cardId);
        }

        [HttpGet]
        public Cards RandomEmptyCard(CardType cardType)
        {
            return _cardService.RandomEmptyCard(cardType);
        }

        [HttpPost]
        public bool AssignmentCardForCustomer(int customerId, Guid cardId)
        {
            return _cardService.AssignmentCardForCustomer(customerId , cardId);
        }

        [HttpPost]
        public ReturnState<object> GenerateIbanNumber()
        {
            return _cardService.GenerateIbanNumber();
        }

        [HttpPost]
        public ReturnState<object> GenerateCard()
        {
            return _cardService.GenerateCard();
        }
    }
}

