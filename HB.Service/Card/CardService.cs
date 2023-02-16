using System;
using System.Net;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.MartenRepository;
using HB.Service.Engine;
using HB.Service.Helpers;
using HB.SharedObject;
using HB.SharedObject.CardViewModel;
using HB.SharedObject.CustomerViewModel;
using Marten;

namespace HB.Service.Card
{
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        private readonly IMartenRepository<Cards> _cardRepository;
        private readonly IAuthUserInformation _authUserInformation;


        public CardService(IMapper mapper, IMartenRepository<Cards> cardRepository , IAuthUserInformation authUserInformation)
        {
            this._mapper = mapper;
            this._cardRepository = cardRepository;
            this._authUserInformation = authUserInformation;
        }

        public ReturnState<object> EmptyCardList()
        {
           var cardsList = _cardRepository.All().Where(x => x.CustomerId == 0).ToList();

           if (cardsList.Count <= 0)
           {
              return new ReturnState<object>(HttpStatusCode.NotFound, "Empty Cards not found!");
           }
                
           var result = _mapper.Map<List<EmptyCardsViewModel>>(cardsList);

           return new ReturnState<object>(result);
        }

        public async Task<ReturnState<object>> PostCreateRandomEmptyCard(int? count)
        {
            if (count <= 0)
            {
               return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Count not equal zero or cant be low!");
            } 

            for (int i = 0; i < count; i++)
            {
               var card = _mapper.Map<Cards>(GeneratorHelper.CardGenerator());

                if (CheckCardNumberIsExist(card.CardNumber))
                {
                    i--;
                    continue;
                }

                await _cardRepository.AddAsync(card);
            }

            return new ReturnState<object>(true);
        }

        public ReturnState<object> PostListCustomerCards()
        {
            var result = _cardRepository.All().Where(x => x.CustomerId == _authUserInformation.CustomerId).ToList();

            if(result is null)
            {
               return new ReturnState<object>(HttpStatusCode.NotFound);
            }

            return new ReturnState<object>(result);
        }

        private bool CheckCardNumberIsExist(string cardNumber)
        {
            var card = _cardRepository.All().Where(x => x.CardNumber == cardNumber).FirstOrDefault();

            return card is null ? false : true;
        }
    }
}

