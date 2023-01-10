using System;
using HB.Domain.Model;
using Marten;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HB.SharedObject;
using AutoMapper;

namespace HB.Service.Customer
{
	public class CustomerService : ICustomerService
	{
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
        private readonly IMapper _mapper;

        public CustomerService(
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
                CustomersId = customer.Id
            };

            var account = _hBContext.Add(accountModel);

            _hBContext.SaveChanges();

            return new Customers();
        }

        public CustomerInformationViewModel CustomerLogin(string email, string password)
        {
            var customerResult = _hBContext.Customers.Where(x => x.Email == email && x.Password == password).FirstOrDefault();

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

            return new CustomerInformationViewModel
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
        }
    }
}

