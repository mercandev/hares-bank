using System;
using System.Net;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.MartenRepository;
using HB.Infrastructure.Repository;
using HB.Service.CustomMapping;
using HB.Service.Engine;
using HB.Service.Helpers;
using HB.SharedObject;
using HB.SharedObject.CardViewModel;
using HB.SharedObject.CustomerViewModel;
using Microsoft.EntityFrameworkCore;

namespace HB.Service.Card
{
    public class CardService : ICardService
    {
        private readonly IMapper _mapper;
        private readonly IMartenRepository<Cards> _cardRepository;
        private readonly IRepository<Accounts> _accountsRepository;
        private readonly IAuthUserInformation _authUserInformation;


        public CardService(IMapper mapper, IMartenRepository<Cards> cardRepository , IAuthUserInformation authUserInformation, IRepository<Accounts> accountsRepository)
        {
            this._mapper = mapper;
            this._cardRepository = cardRepository;
            this._authUserInformation = authUserInformation;
            this._accountsRepository = accountsRepository;
        }

        public async Task<ReturnState<object>> DelegateCardCustomer(CardType cardType)
        {
            var customerResult = await _accountsRepository.All().Where(x => x.Customers.Id == _authUserInformation.CustomerId)
              .Include("Customers").FirstOrDefaultAsync();

            if (customerResult is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Customer not found!");
            }

            var emptyRandomCard = _cardRepository.FindAll(x => !x.IsActive && x.CustomerName == null).PickRandom();

            if (emptyRandomCard is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Blank card not found! Please produce new cards!");
            }

            var customeEmptyCardResult = CustomCardMapping.DelegateCardCustomerMapping(emptyRandomCard, customerResult, cardType);

            await _cardRepository.UpdateAsync(customeEmptyCardResult);

            return new ReturnState<object>(true);
        }

        public async Task<ReturnState<object>> EmptyCardList()
        {
            var cardsList = await _cardRepository.FindAllAsync(x => x.CustomerId == 0);

            if (cardsList.Count > 0)
            {
                var result = _mapper.Map<List<EmptyCardsViewModel>>(cardsList);

                return new ReturnState<object>(result);
            }

            return new ReturnState<object>(HttpStatusCode.NotFound, "Empty Cards not found!");
        }

        public async Task<ReturnState<object>> PostCreateRandomEmptyCard(int? count)
        {
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var card = _mapper.Map<Cards>(GeneratorHelper.CardGenerator());

                    if (await CheckCardNumberIsExist(card.CardNumber))
                    {
                        i--;
                        continue;
                    }

                    await _cardRepository.AddAsync(card);
                }

                return new ReturnState<object>(true);
            }

            return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Count not equal zero or cant be low!");
        }

        public async Task<ReturnState<object>> PostListCustomerCards()
        {
            var result = await _cardRepository.FindAllAsync(x => x.CustomerId == _authUserInformation.CustomerId);

            if (result is not null)
            {
                return new ReturnState<object>(result);
            }

            return new ReturnState<object>(HttpStatusCode.NotFound);
        }

        private async Task<bool> CheckCardNumberIsExist(string cardNumber)
        {
            var card = await _cardRepository.FirstOrDefaultAsync(x => x.CardNumber == cardNumber);

            return card is null ? false : true;
        }
    }
}

