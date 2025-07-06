using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using IdentityText.Models;
using IdentityText.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityText.Repository
{
    // Renamed the class to avoid conflict with an existing 'AssessmentRepository' class
    public class AcademicYearRepository : Repository<AcademicYear>, IAcademicYearRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AcademicYearRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            dbContext = appDbContext;
        }

        public async Task<IEnumerable<SelectListItem>> SelectListAcademicYearAsync()
        {
            return await dbSet.OrderBy(a => a.Name)
                             .Select(a => new SelectListItem
                             {
                                 Value = a.AcademicYearId.ToString(),
                                 Text = a.Name
                             }).ToListAsync();
        }

        
    }
}
