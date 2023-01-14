using System;
using HB.Domain.Model;
using Marten;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HB.SharedObject;
using AutoMapper;
using System.Security.Claims;
using HB.Service.Engine;
using Microsoft.Extensions.Options;
using HB.Service.Helpers;

namespace HB.Service.Customer
{
	public class CustomerService : ICustomerService
	{
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly IMapper _mapper;
        private readonly IOptions<JwtModel> _options;

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

        public List<Customers>? GetCustomers()
        =>  _hBContext?.Customers.Include("Accounts").ToList();
        

        public List<Accounts?> GetCustomerAccounts(int customerId)
        => _hBContext?.Accounts.Where(x => x.Customers.Id == customerId)
               .Include("Customers")
               .Include("BranchOffices")
               .ToList();


        public Customers? CreateCustomer(CreateCustomerViewModel createCustomerViewModel)
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

            return new Customers();
        }

        public ReturnState<object> CustomerInformation(int customerId)
        {
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

        public ReturnState<object> CustomerLogin(string email, string password)
        {
            var customerResult = _hBContext.Customers.Where(x => x.Email == email && x.Password == password).FirstOrDefault();

            if (customerResult == null)
            {
                throw new Exception("Customer not found!");
            }

            var claims = new[]
            {
                new Claim("CustomerId", customerResult.Id.ToString()),
            };

            var returnModel = new JwtReturnViewModel { Token = new JwtSecurity().CreateJwtToken(claims, _options.Value) };

            return new ReturnState<object>(returnModel);
        }

    }
}



