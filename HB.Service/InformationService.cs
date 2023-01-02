using System;
using HB.Domain.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Marten;

namespace HB.Service
{
	public class InformationService : IInformationService
	{
        private readonly HbContext? _hBContext;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;

        public InformationService(HbContext hbContext , IDocumentSession documentSession, IQuerySession querySession)
		{
			this._hBContext = hbContext;
            this._documentSession = documentSession;
            this._querySession = querySession;
		}


		public List<Customers>? GetCustomers()
		{
			var result = _hBContext?.Customers
				.Include("Accounts")
				.ToList();

			return result;

		}


        public List<Accounts?> GetCustomerAccounts(int customerId)
        {
            var customerResult = _hBContext?.Accounts.Where(x => x.Customers.Id == customerId)
               .Include("Customers")
               .Include("BranchOffices")
               .ToList();

            return customerResult;
        }

        public async Task<bool> AddCard(Cards cards)
        {
            _documentSession.Store(cards);
            await _documentSession.SaveChangesAsync();
            return true;
        }

        public Cards? ListCardsByCustomerId(int customerId)
        {
            var result = _querySession.Query<Cards>().Where(x => x.CustomerId == customerId).FirstOrDefault();
            return result;
        }
    }
}

