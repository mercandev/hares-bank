using System;
using HB.Domain.Model;
using HB.Infrastructure.DbContext;
using HB.Infrastructure.Enum;
using Microsoft.EntityFrameworkCore;

namespace HB.Infrastructure.Repository
{
	public class Context : IContext
	{
        public HbContext context;

        public Context(HbContext context)
        {
            this.context = context;
        }

        public void ChangeState<T>(T entity, DomainState state) where T : class
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync();
        }

        public DbSet<T> Set<T>() where T : class
        {
            return context.Set<T>();
        }
    }
}

