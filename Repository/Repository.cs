

using IdentityText.Data;
using IdentityText.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        public DbSet<T> dbSet { get; }

        public Repository(ApplicationDbContext appDbContext)
        {
            dbContext = appDbContext;
            dbSet = dbContext.Set<T>();
        }

        // CRUD
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        public void CreateAll(List<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void DeleteAll(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public void Commit()
        {
            dbContext.SaveChanges();
        }
        //public void Create(T entity, CancellationToken cancellationToken = default)
        //{
        //    dbSet.AddAsync(entity, cancellationToken);
        //    dbContext.SaveChangesAsync(cancellationToken);

        //    return entity;
        //}

        public void CreateAll(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
            dbContext.SaveChanges();
        }
        public void Delete(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            dbContext.SaveChanges();
        }

        public IEnumerable<T> Get(
                Expression<Func<T, bool>>? filter = null, 
                Expression<Func<T, object>>[]? includes = null,
                bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public T? GetOne(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public T Create(T entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Comitt()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
