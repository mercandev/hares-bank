using System;
using System.Net;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.Repository;
using HB.Service.Const;
using HB.Service.CustomMapping;
using HB.Service.Helpers;
using HB.Service.Transaction;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using HB.SharedObject.ExchangeViewModel;
using Marten;
using Microsoft.EntityFrameworkCore;

namespace HB.Service.Process
{
    public class ProcessService : IProcessService
    {
        private readonly HbContext? _hBContext;
        private readonly IRepository<Accounts> _accountRepository;
        private readonly IAuthUserInformation _userInformation;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public ProcessService(
            HbContext hbContext,
            IAuthUserInformation userInformation,
            IRepository<Accounts> accountRepository,
            IMapper mapper,
            ITransactionService transactionService
         )
        {
            this._hBContext = hbContext;
            this._userInformation = userInformation;
            this._mapper = mapper;
            this._accountRepository = accountRepository;
            this._transactionService = transactionService;
        }


        #region Convert Money Coal
        public async Task<ReturnState<object>> BuyGold(ConvertMoneyToCoalViewModel model)
        {
            if (model.Price < 1M)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Price cannot under the 1.0!");
            }
            
            var customerAccount = _accountRepository.All().Where(x => x.CustomersId == _userInformation.CustomerId)
                .Include("Customers").ToList();

            if (customerAccount is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Account not found!");
            }

            var goldAccount = customerAccount.FirstOrDefault(x => x.CurrencyId == (int)Currency.GOLD);

            if (goldAccount is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Coal account not found!");
            }

            var mainAccount = customerAccount.FirstOrDefault(x => x.Id == model.AccountId);

            if (mainAccount is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Main account not found!");
            }

            var isAccountAmountEnough = mainAccount.Amount - model.Price;

            if (isAccountAmountEnough < 0M)
            {
                return new ReturnState<object>(HttpStatusCode.NotAcceptable, "Balance not enough!");
            }

            var poundPrice = Math.Round(model.Price / model.CoalPrice, 5);

            mainAccount.Amount = mainAccount.Amount - model.Price;
            goldAccount.Amount = goldAccount.Amount + poundPrice;
            goldAccount.UpdatedDate = DateTime.Now;

            await _accountRepository.UpdateAsync(goldAccount);
            await _accountRepository.UpdateAsync(mainAccount);
            
            var transactions = CustomProcessMapping.GoldBuyyingTransaction(goldAccount, poundPrice, model.Price);

            await _transactionService.CreateTransaction(transactions);

            return new ReturnState<object>(true);
        }

        #endregion

        #region Coal Information

        public async Task<ReturnState<object>> CoalInformation()
        {
            var exchangeResult = await RestRequestHelper<ExchangeResponseViewModel>.GetService(ExchangeConst.EXCHANGE_URL);

            if (exchangeResult is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Coal information not found!");
            }

            var mapping = _mapper.Map<ExchangeMappingResponseViewModel>(exchangeResult);

            return new ReturnState<object>(mapping);
        }

        #endregion
    }
}

