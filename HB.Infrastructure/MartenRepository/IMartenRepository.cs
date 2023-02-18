using System;
using System.Linq.Expressions;

namespace HB.Infrastructure.MartenRepository
{
	public interface IMartenRepository<T>
	{
		IQueryable<T> All();
		void Update(T entities);
        Task UpdateAsync(T entities);
        void Delete(T entities);
		void Add(T entities);
		Task AddAsync(T entities);
		List<T> FindAll(Expression<Func<T, bool>> match);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match);
        T FirstOrDefault(Expression<Func<T, bool>> match);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> match);

    }
}

