using System;
namespace HB.Infrastructure.Repository
{
	public interface IRepository<T> : IReadOnlyRepository<T>, IWriteOnlyRepository<T>
    {
	}
}

