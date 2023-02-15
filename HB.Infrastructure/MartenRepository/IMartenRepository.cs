using System;
namespace HB.Infrastructure.MartenRepository
{
	public interface IMartenRepository<T>
	{
		IQueryable<T> All();
		void Update(T entities);
        void Delete(T entities);
		void Add(T entities);
    }
}

