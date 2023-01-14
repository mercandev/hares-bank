using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Customer
{
	public interface ICustomerService
	{
        List<Customers>? GetCustomers();
        List<Accounts?> GetCustomerAccounts(int customerId);
        Customers? CreateCustomer(CreateCustomerViewModel createCustomerViewModel);
        ReturnState<object> CustomerInformation(int customerId);
        ReturnState<object> CustomerLogin(string email, string password);
    }
}

