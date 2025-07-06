using IdentityText.Data;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityText.Repository
{
    public class PrivateLessonRepository : Repository<PrivateLesson>, IPrivateLessonRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PrivateLessonRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            dbContext = appDbContext;
        }
        public IEnumerable<PrivateLesson> GetWithFullIncludes(
                 Expression<Func<PrivateLesson, bool>>? filter = null,
                 Func<IQueryable<PrivateLesson>, IQueryable<PrivateLesson>>? include = null,
                 bool tracked = true)
        {
            IQueryable<PrivateLesson> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public async Task<int> CountAsync()
        {
            return await dbContext.PrivateLessons.CountAsync();
        }



        public async Task<int> CountByMonthAsync(int month, int year)
        {
            return await dbContext.PrivateLessons
                .Where(s => s.CreatedAt.Month == month && s.CreatedAt.Year == year)
                .CountAsync();
        }


    }
}
