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
        CustomerInformationViewModel CustomerInformation(int customerId);
        string CustomerLogin(string email, string password);
    }
}

