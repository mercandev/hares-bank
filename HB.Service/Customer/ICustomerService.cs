using System;
using HB.Domain.Model;
using HB.SharedObject;
using HB.SharedObject.AccountViewModel;
using HB.SharedObject.CustomerViewModel;

namespace HB.Service.Customer
{
	public interface ICustomerService
	{
        ReturnState<object> CreateCustomer(CreateCustomerViewModel createCustomerViewModel);
        ReturnState<object> CustomerInformation();
        ReturnState<object> CustomerLogin(string email, string password);
        Task<ReturnState<object>> CreateAccount(int customerId , CreateAccountViewModel model);
        Task<ReturnState<object>> BuyGold(int customerId , ConvertMoneyToCoalViewModel model);
        Task<ReturnState<object>> CoalInformation(CoalDetailViewModel model);
        Task<ReturnState<object>> DelegateCardCustomer(int customerId, CardType cardType);
    }
}

