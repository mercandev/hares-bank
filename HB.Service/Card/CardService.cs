using System;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.Exceptions;
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
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly IMapper _mapper;


        public CardService(
            HbContext hbContext,
            IDocumentSession documentSession,
            IQuerySession querySession,
            IMapper mapper
            )
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
            this._mapper = mapper;
        }

        public ReturnState<object> EmptyCardList()
        {
           var cardsList = _querySession.Query<Cards>().Where(x => x.CustomerId == 0).ToList();
           if (cardsList == null) return new ReturnState<object>(null);

           var result = _mapper.Map<List<EmptyCardsViewModel>>(cardsList);
           return new ReturnState<object>(result);
        }

        public async Task<ReturnState<object>> PostCreateRandomEmptyCard(int? count)
        {
            if (count <= 0) throw new HbBusinessException("Count not equal zero or cant be low!");

            for (int i = 0; i < count; i++)
            {
               var card = _mapper.Map<Cards>(GeneratorHelper.CardGenerator());

                if (CheckCardNumberIsExist(card.CardNumber))
                {
                    i--;
                    continue;
                }

                _documentSession.Insert(card);
                await _documentSession.SaveChangesAsync();
            }

            return new ReturnState<object>(true);
        }

        public ReturnState<object> PostListCustomerCards(int? customerId)
        {
            var result = _querySession.Query<Cards>().Where(x => x.CustomerId == customerId).ToList();
            return new ReturnState<object>(result);
        }

        private bool CheckCardNumberIsExist(string cardNumber)
        {
            var card = _querySession.Query<Cards>().Where(x => x.CardNumber == cardNumber).FirstOrDefault();

            if (card is null) return false;

            return true;
        }
    }
}

