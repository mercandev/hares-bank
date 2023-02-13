using System;
namespace HB.Infrastructure.Repository
{
	public interface IWriteOnlyRepository<T>
	{
        T Add(T entities);

        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities);

        void Delete(IEnumerable<T> entities);

        bool Delete(int id);

        int Delete(T entity);

        Task DeleteAsync(IEnumerable<T> entities);

        Task<bool> DeleteAsync(int id);

        Task<int> DeleteAsync(T entity);

        IEnumerable<T> Update(IEnumerable<T> entities);

        T Update(int id, T entity);

        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(int id, T entity);
    }
}

