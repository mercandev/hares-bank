using System;
using System.Net;
using HB.Domain.Model;
using HB.Infrastructure.Authentication;
using HB.Infrastructure.Repository;
using HB.Service.CustomMapping;
using HB.Service.Helpers;
using HB.SharedObject;
using HB.SharedObject.AccountViewModel;

namespace HB.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Customers> _customerRepository;
        private readonly IRepository<Accounts> _accountRepository;
        private readonly IAuthUserInformation _authUserInformation;


        public AccountService(IRepository<Customers> customerRepository , IRepository<Accounts> accountRepository, IAuthUserInformation authUserInformation)
        {
            this._customerRepository = customerRepository;
            this._accountRepository = accountRepository;
            this._authUserInformation = authUserInformation;
        }

        public async Task<ReturnState<object>> CreateAccount(CreateAccountViewModel model)
        {
            if (model.CurrencyId < 0 || model.CurrencyId > 5) return new ReturnState<object>(HttpStatusCode.NotAcceptable, "CurrencyId is not valid!");
            
            var customerInformation = await _customerRepository.FindAllFirstOrDefaultAsync(x => x.Id == _authUserInformation.CustomerId);

            if (customerInformation is null) return new ReturnState<object>(HttpStatusCode.NotFound, "Customer not found!");

            var accounts = CustomAccountMapping.CreateAccountMapping(customerInformation, model.CurrencyId);

            await _accountRepository.AddAsync(accounts);

            return new ReturnState<object>(HttpStatusCode.Created, data: true);
        }
    }
}

