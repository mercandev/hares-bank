using System;
using System.Linq.Expressions;

namespace HB.Infrastructure.Repository
{
	public interface IReadOnlyRepository<T>
	{
        IQueryable<T> All();

        bool Any(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        
        T Find(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        T Get(int id);

        Task<T> GetAsync(int id);
    }
}

