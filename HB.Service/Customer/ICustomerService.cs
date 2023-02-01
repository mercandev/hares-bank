using System;
using HB.Domain.Model;
using HB.SharedObject;
using HB.SharedObject.CustomerViewModel;

namespace HB.Service.Customer
{
	public interface ICustomerService
	{
        ReturnState<object> CreateCustomer(CreateCustomerViewModel createCustomerViewModel);
        ReturnState<object> CustomerInformation(int? customerId);
        ReturnState<object> CustomerLogin(string email, string password);
        Task<ReturnState<object>> DelegateCardCustomer(int customerId, CardType cardType);
    }
}

