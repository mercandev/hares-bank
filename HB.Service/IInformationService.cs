using System;
using HB.Domain.Model;

namespace HB.Service
{
	public interface IInformationService
	{
        List<Customers>? GetCustomers();
        List<Accounts?> GetCustomerAccounts(int customerId);
        Task<bool> AddCard(Cards cards);
        Cards ListCardsByCustomerId(int customerId);

    }
}

