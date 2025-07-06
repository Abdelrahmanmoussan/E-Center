
using IdentityText.Data;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityText.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        readonly ApplicationDbContext appDbContext;
        public StudentRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<SelectListItem>> SelectListStudentAsync()
        {
            return await dbSet.OrderBy(a => a.ApplicationUser.FirstName)
                             .Select(a => new SelectListItem
                             {
                                 Value = a.StudentId.ToString(),
                                 Text = a.ApplicationUser.FirstName + " " + a.ApplicationUser.LastName
                             }).ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await appDbContext.Students.CountAsync();
        }

        public async Task<IEnumerable<Student>> GetAllWithIncludesAsync()
        {
            return await appDbContext.Students
                .Include(s => s.ApplicationUser)
                .Include(s => s.AcademicYear)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdWithIncludesAsync(int id)
        {
            return await appDbContext.Students
                .Include(s => s.ApplicationUser)
                .Include(s => s.AcademicYear)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }




        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await appDbContext.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await appDbContext.Students.FindAsync(id);
        }

        public async Task AddAsync(Student student)
        {
            await appDbContext.Students.AddAsync(student);
            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            appDbContext.Students.Update(student);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await appDbContext.Students.FindAsync(id);
            if (student != null)
            {
                appDbContext.Students.Remove(student);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetLatestStudentsAsync(int count = 5)
        {
            return await appDbContext.Students
                .Include(s => s.ApplicationUser)
                .OrderByDescending(s => s.EnrollmentDate)
                .Take(count)
                .ToListAsync();
        }



        public async Task<int> CountByMonthAsync(int month, int year)
        {
            return await appDbContext.Students
                .Where(s => s.EnrollmentDate.Month == month && s.EnrollmentDate.Year == year)
                .CountAsync();
        }







    }
}
