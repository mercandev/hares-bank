using System;
using HB.Infrastructure.Enum;
using Microsoft.EntityFrameworkCore;


namespace HB.Infrastructure.Repository
{
	public interface IContext
	{
        void ChangeState<T>(T entity, DomainState state) where T : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        DbSet<T> Set<T>() where T : class;
    }
}

