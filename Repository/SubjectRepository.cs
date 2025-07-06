
using IdentityText.Data;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Repository
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SubjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<SelectListItem>> SelectListSubjectAsync()
        {
            return await dbSet.OrderBy(a => a.Title)
                              .Select(a => new SelectListItem
                              {
                                  Value = a.SubjectId.ToString(),
                                  Text = a.Title
                              }).ToListAsync();
        }

    }
}
