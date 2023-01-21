using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Transaction
{
	public interface ITransactionService
	{
        ReturnState<object> CreateTransaction(Transactions transaction);
		ReturnState<object> ListTransactionsByCustomerId(int customerId, DateTime startDate, DateTime endDate);
	}
}

