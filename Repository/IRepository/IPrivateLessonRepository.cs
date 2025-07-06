using IdentityText.Models;
using System.Linq.Expressions;

namespace IdentityText.Repository.IRepository
{
    public interface IPrivateLessonRepository : IRepository<PrivateLesson>
    {
        IEnumerable<PrivateLesson> GetWithFullIncludes(
                 Expression<Func<PrivateLesson, bool>>? filter = null,
                 Func<IQueryable<PrivateLesson>, IQueryable<PrivateLesson>>? include = null,
                 bool tracked = true);
        Task<int> CountAsync();

        Task<int> CountByMonthAsync(int month, int year);

    }

}
