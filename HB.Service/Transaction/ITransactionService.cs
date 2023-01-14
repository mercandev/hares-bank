using System;
using HB.Domain.Model;
using HB.SharedObject;

namespace HB.Service.Transaction
{
	public interface ITransactionService
	{
		bool CreateTransaction(Transactions transaction);
		ReturnState<object> ListTransactionsByCustomerId(int customerId);
	}
}

