using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace IdentityText.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        void Create(T entity);

        void CreateAll(List<T> entities);

        public void Edit(T entity);
        void Commit();

        public void Delete(T entity);

        public void DeleteAll(List<T> entities);

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);
    }
}
