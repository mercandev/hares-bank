using System;
using HB.SharedObject;
using HB.SharedObject.AccountViewModel;

namespace HB.Service.Account
{
	public interface IAccountService
	{
        Task<ReturnState<object>> CreateAccount(CreateAccountViewModel model);
    }
}

