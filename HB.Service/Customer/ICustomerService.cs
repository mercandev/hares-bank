using System;
using HB.Domain.Model;
using HB.SharedObject;
using HB.SharedObject.AccountViewModel;
using HB.SharedObject.CustomerViewModel;

namespace HB.Service.Customer
{
	public interface ICustomerService
	{
        Task<ReturnState<object>> CreateCustomer(CreateCustomerViewModel createCustomerViewModel);
        Task<ReturnState<object>> CustomerInformation();
       
    }
}

