using IdentityText.Data;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityText.Repository
{
    public class ClassGroupRepository : Repository<ClassGroup>, IClassGroupRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ClassGroupRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            dbContext = appDbContext;
        }
        public async Task<int> CountAsync()
        {
            return await dbContext.ClassGroups.CountAsync();
        }


        public async Task<IEnumerable<SelectListItem>> SelectListClassGroupAsync()
        {
            return await dbSet.OrderBy(a => a.Title)
                             .Select(a => new SelectListItem
                             {
                                 Value = a.ClassGroupId.ToString(),
                                 Text = a.Title
                             }).ToListAsync();
        }

        public async Task<List<SelectListItem>> SelectListClassGroupByTeacherIdAsync(int teacherId)
        {
            return await dbContext.ClassGroups
                .Where(cg => cg.TeacherId == teacherId)
                .Select(cg => new SelectListItem
                {
                    Value = cg.ClassGroupId.ToString(),
                    Text = cg.Title
                }).ToListAsync();
        }


        public async Task<List<ClassGroup>> GetLatestCoursesAsync(int count = 5)
        {
            return await dbContext.ClassGroups
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.ApplicationUser)
                .OrderByDescending(c => c.CreatedAt) 
                .Take(count)
                .ToListAsync();
        }



        public async Task<int> CountByMonthAsync(int month, int year)
        {
            return await dbContext.ClassGroups
                .Where(s => s.CreatedAt.Month == month && s.CreatedAt.Year == year)
                .CountAsync();
        }


    }



}
