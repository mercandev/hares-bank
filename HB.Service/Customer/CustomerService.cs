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

namespace HB.Service.Customer
{
	public class CustomerService : ICustomerService
	{
        #region Data Access

        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtModel> _options;

        #endregion

        #region Constructor

        public CustomerService(
            HbContext hbContext,
            IDocumentSession documentSession,
            IQuerySession querySession,
            IMapper mapper,
            IOptions<JwtModel> options
            )
        {
            this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
            this._mapper = mapper;
            this._options = options;
        }

        #endregion

        #region Create Customer
        public ReturnState<object> CreateCustomer(CreateCustomerViewModel createCustomerViewModel)
        {
            var customerModel = new Customers
            {
                Name = createCustomerViewModel.Name,
                Surname = createCustomerViewModel.Surname,
                Email = createCustomerViewModel.Email,
                PhoneNumber = createCustomerViewModel.PhoneNumber,
                Password = createCustomerViewModel.Password,
                DateOfBrith = createCustomerViewModel.DateOfBrith,
                BranchOfficesId = createCustomerViewModel.BranchOfficesId,
            };

            var customer = _hBContext.Add(customerModel).Entity;

            _hBContext.SaveChanges();

            var addressModel = new Address
            {
                AddressExplanation = createCustomerViewModel.AddressExplanation,
                CustomerId = customer.Id,
            };

            var address = _hBContext.Add(addressModel);

            var accountModel = new Accounts
            {
                BranchOfficesId = createCustomerViewModel.BranchOfficesId,
                Name = createCustomerViewModel.AccountName,
                CurrencyId = (int)createCustomerViewModel.CurrencyId,
                CustomersId = customer.Id,
                Iban = GeneratorHelper.IbanGenerator()
            };

            var account = _hBContext.Add(accountModel);

            _hBContext.SaveChanges();

            return new ReturnState<object>(true);
        }
        #endregion

        #region Customer Information
        public ReturnState<object> CustomerInformation(int? customerId)
        {
            if (customerId == default || customerId ==null)
            {
                throw new Exception("CustomerId is not null!");
            }

            var customerResult = _hBContext.Customers.Where(x => x.Id == customerId).FirstOrDefault();

            if (customerResult == null)
            {
                throw new Exception("Customer not found!");
            }

            var result = _hBContext?.Accounts.Where(x => x.Customers.Id == customerResult.Id)
               .Include("Customers")
               .Include("BranchOffices")
               .ToList();

            var address = _hBContext?.Address.Where(x => x.CustomerId == customerResult.Id).FirstOrDefault();

            var accountResult = _mapper.Map<List<AccountsViewModel>>(result);

            var customerInformationResult = new CustomerInformationViewModel
            {
                Name = customerResult.Name,
                Surname = customerResult.Surname,
                PhoneNumber = customerResult.PhoneNumber,
                Email = customerResult.Email,
                Branch = new BranchViewModel
                {
                    Id = customerResult.BranchOffices.Id,
                    Name = customerResult.BranchOffices.Name
                },
                Address = new AddressViewModel
                {
                    Id = address.Id,
                    AddressExplanation = address.AddressExplanation
                },
                Accounts = accountResult
            };

            return new ReturnState<object>(customerInformationResult);

        }
        #endregion

        #region Customer Login
        public ReturnState<object> CustomerLogin(string email, string password)
        {
            var customerResult = _hBContext.Customers.Where(x => x.Email == email && x.Password == password).FirstOrDefault();

            if (customerResult == null) throw new HbBusinessException("Customer not found!");

            var claims = new[]
            {
                new Claim("CustomerId", customerResult.Id.ToString()),
                new Claim(ClaimTypes.Role , "Customer"),
            };

            var returnModel = new JwtReturnViewModel { Token = new JwtSecurity().CreateJwtToken(claims, _options.Value) };

            return new ReturnState<object>(returnModel);
        }
        #endregion


        public async Task<ReturnState<object>> DelegateCardCustomer(int customerId , CardType cardType)
        {
            if (customerId == default) throw new HbBusinessException("CustomerId cannot be null or default!");

            var customerResult = _hBContext?.Accounts.Where(x => x.Customers.Id == customerId)
               .Include("Customers")
               .Include("BranchOffices")
               .FirstOrDefault();

            if (customerResult == null) throw new HbBusinessException("Customer not found!");

            var emptyRandomCard = _querySession.Query<Cards>()
                .Where(x => !x.IsActive && x.CustomerName == null).ToList().PickRandom();

            if (emptyRandomCard == null) throw new HbBusinessException("Blank card not found! Please produce new cards!");

            emptyRandomCard.CustomerId = customerResult.Customers.Id;
            emptyRandomCard.CustomerName = $"{customerResult.Customers.Name} {customerResult.Customers.Surname}";
            emptyRandomCard.CardType = cardType;
            emptyRandomCard.IsActive = true;
            if (cardType == CardType.DebitCard) emptyRandomCard.AccountId = customerResult.Id;

            _documentSession.Update(emptyRandomCard);
            await _documentSession.SaveChangesAsync();

            return new ReturnState<object>(true);
        }
    }
}



