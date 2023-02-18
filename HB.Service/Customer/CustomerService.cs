using System;
using HB.Domain.Model;
using Marten;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;
using AutoMapper;
using System.Security.Claims;
using HB.Service.Engine;
using Microsoft.Extensions.Options;
using HB.Service.Helpers;
using HB.Infrastructure.Jwt;
using HB.Infrastructure.Exceptions;
using HB.SharedObject.AccountViewModel;
using System.ComponentModel;
using System.Reflection;
using HB.SharedObject.ExchangeViewModel;
using HB.Service.Const;
using HB.Service.Transaction;
using Microsoft.AspNetCore.SignalR;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Repository;
using HB.Infrastructure.Authentication;
using System.Net;

namespace HB.Service.Customer
{
	public class CustomerService : ICustomerService
	{
        #region Data Access

        private readonly IMapper _mapper;
        private readonly IRepository<Customers> _customerRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Accounts> _accountRepository;
        private readonly IAuthUserInformation _userInformation;

        #endregion

        #region Constructor

        public CustomerService(
            IMapper mapper,
            IRepository<Customers> customerRepository,
            IRepository<Address> addressRepository,
            IRepository<Accounts> accountRepository,
            IAuthUserInformation userInformation
            )
        {
            this._mapper = mapper;
            this._customerRepository = customerRepository;
            this._userInformation = userInformation;
            this._addressRepository = addressRepository;
            this._accountRepository = accountRepository;
        }
        #endregion

       

        #region Create Customer
        public async Task<ReturnState<object>> CreateCustomer(CreateCustomerViewModel createCustomerViewModel)
        {
            var customerMappingModel = _mapper.Map<Customers>(createCustomerViewModel);

            var customer = await _customerRepository.AddAsync(customerMappingModel);

            createCustomerViewModel.CustomerId = customer.Id;

            var addressMappingModel = _mapper.Map<Address>(createCustomerViewModel);

            await _addressRepository.AddAsync(addressMappingModel);

            var accountMappingModel = _mapper.Map<Accounts>(createCustomerViewModel);

            await _accountRepository.AddAsync(accountMappingModel);

            return new ReturnState<object>(statusCode: HttpStatusCode.Created, data: true);
        }
        #endregion
        
        #region Customer Information
        public async Task<ReturnState<object>> CustomerInformation()
        {
            var customerAccountResult = _accountRepository.All().Where(x => x.CustomersId == _userInformation.CustomerId)
              .Include("Customers").Include("BranchOffices").ToList();

            if (customerAccountResult.Count <= 0)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound , "Customer not found!");
            }

            var addressResult = await _addressRepository.FindAllFirstOrDefaultAsync(x => x.CustomerId == _userInformation.CustomerId);

            if (addressResult is null)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound, "Customer address not found!");
            }

            var addressMapping = _mapper.Map<AddressViewModel>(addressResult);

            var branchMapping = _mapper.Map<BranchViewModel>(customerAccountResult[0].BranchOffices);

            var accountMapping = _mapper.Map<List<AccountsViewModel>>(customerAccountResult);

            var customerMapping = _mapper.Map<CustomerViewModel>(customerAccountResult[0].Customers);

            var customerInformationResult = new CustomerInformationViewModel
            {
                Accounts = accountMapping,
                Address = addressMapping,
                Branch = branchMapping,
                Customer = customerMapping
            };

            return new ReturnState<object>(customerInformationResult);

        }
        #endregion
    }
}



