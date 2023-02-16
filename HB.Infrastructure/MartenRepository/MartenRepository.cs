using System;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Repository;
using Marten;

namespace HB.Infrastructure.MartenRepository
{
	public class MartenRepository<T> : IMartenRepository<T>
	{
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;
       

        public MartenRepository(IDocumentSession documentSession,IQuerySession querySession)
        {
            this._documentSession = documentSession;
            this._querySession = querySession;
        }

        public void Add(T entity)
        {
            _documentSession.Insert<T>(entity);
            _documentSession.SaveChanges();
        }

        public async Task AddAsync(T entities)
        {
            _documentSession.Insert<T>(entities);
            await _documentSession.SaveChangesAsync();
        }

        public IQueryable<T> All()
          => _querySession.Query<T>();

        public void Delete(T entity)
        {
            _documentSession.Update<T>(entity);
            _documentSession.SaveChanges();
        }

        public void Update(T entity)
        {
            _documentSession.Update<T>(entity);
            _documentSession.SaveChanges();
        }
    }
}

