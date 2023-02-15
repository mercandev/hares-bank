using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HB.Infrastructure.Repository
{
	public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IContext context;

        private DbSet<T> entities;

        public Repository(IContext context)
        {
            this.context = context;
        }

        protected DbSet<T> Table => entities ?? (entities = context.Set<T>());

        public T Add(T entities)
        {

            Table.AddRange(entities);
            context.SaveChanges();

            return entities;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
            await context.SaveChangesAsync();

            return entities;
        }

        public virtual IQueryable<T> All()
        {
            return Table;
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return Table.Any(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await Table.AnyAsync(expression);
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return Table.Count(expression);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await Table.CountAsync(expression).ConfigureAwait(false);
        }

        public void Delete(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            Table.UpdateRange(entities);
            context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var entity = Table.Find(id);

            if (entity == null)
            {
                return false;
            }

            Table.Update(entity);

            return context.SaveChanges() > 0;
        }

        public int Delete(T entity)
        {
            Table.Update(entity);

            return context.SaveChanges();
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                return;
            }


            Table.UpdateRange(entities);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await Table.FindAsync(id);

            if (entity == null)
            {
                return false;
            }


            Table.Update(entity);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<int> DeleteAsync(T entity)
        {

            Table.Update(entity);

            return await context.SaveChangesAsync();
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return Table.SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return Table.Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await Table.Where(match).ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await Table.SingleOrDefaultAsync(match);
        }

        public T Get(int id)
        {
            return Table.Find(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        public IEnumerable<T> Update(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                return entities;
            }

            Table.UpdateRange(entities);
            context.SaveChanges();

            return entities;
        }

        public T Update(int id, T entity)
        {
            if (entity == null)
            {
                return null;
            }

            Table.Update(entity);
            context.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                return entities;
            }

            Table.UpdateRange(entities);
            await context.SaveChangesAsync();

            return entities;
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            if (entity == null)
            {
                return null;
            }

            Table.Update(entity);

            await context.SaveChangesAsync();

            return entity;
        }
    }
}

