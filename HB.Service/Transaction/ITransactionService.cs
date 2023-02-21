using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Transaction
{
	public interface ITransactionService
	{
        Task<ReturnState<object>> CreateTransaction(Transactions transaction);
		Task<ReturnState<object>> ListTransactionsByCustomerId(int customerId, DateTime startDate, DateTime endDate);
	}
}

