using System;
using System.Linq.Expressions;
using System.Threading;
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

        public List<T> FindAll(Expression<Func<T, bool>> match)
        => _querySession.Query<T>().Where(match).ToList();

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match)
        => (List<T>)await _querySession.Query<T>().Where(match).ToListAsync();

        public T FirstOrDefault(Expression<Func<T, bool>> match)
        => _querySession.Query<T>().Where(match).FirstOrDefault();

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match)
        => await _querySession.Query<T>().Where(match).FirstOrDefaultAsync();

        public void Update(T entity)
        {
            _documentSession.Update<T>(entity);
            _documentSession.SaveChanges();
        }

        public async Task UpdateAsync(T entities)
        {
            _documentSession.Update<T>(entities);
            await _documentSession.SaveChangesAsync();
        }


    }
}

