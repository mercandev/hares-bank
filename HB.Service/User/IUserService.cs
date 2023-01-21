using System;
using HB.SharedObject;

namespace HB.Service.User
{
	public interface IUserService
	{
        ReturnState<object> UserLogin(string username, string password);
    }
}

