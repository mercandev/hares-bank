using System;
using HB.Domain.Model;

namespace HB.Service.Transaction
{
	public interface ITransactionService
	{
		bool CreateTransaction(Transactions transaction);
	}
}

