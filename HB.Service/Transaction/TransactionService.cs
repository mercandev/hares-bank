using System;
using System.Linq;
using System.Net;
using AutoMapper;
using HB.Domain.Model;
using HB.Infrastructure.MartenRepository;
using HB.SharedObject;
using HB.SharedObject.TransactionViewModel;
using Marten;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace HB.Service.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly IMartenRepository<Transactions> _transactionRepository;

        public TransactionService(IMapper mapper , IMartenRepository<Transactions> transactionRepository)
        {
            this._mapper = mapper;
            this._transactionRepository = transactionRepository;
        }

        public ReturnState<object> CreateTransaction(Transactions transaction)
        {
            _transactionRepository.Add(transaction);
            return new ReturnState<object>(HttpStatusCode.Created, data: true);
        }

        public ReturnState<object> ListTransactionsByCustomerId(int customerId , DateTime startDate , DateTime endDate)
        {
            var result = _transactionRepository.All()
                .Where(x => x.CustomerId == customerId && x.CreatedDate >= startDate && x.CreatedDate < endDate)
                .OrderByDescending(x => x.CreatedDate).ToList();

            if(result.Count <= 0)
            {
                return new ReturnState<object>(HttpStatusCode.NotFound , "Transaction not found!");
            }

            var mapperResult = _mapper.Map<List<TransactionsResponseViewModel>>(result);

            return new ReturnState<object>(mapperResult);
        }
    }
}

