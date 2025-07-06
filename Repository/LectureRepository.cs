using IdentityText.Repository.IRepository;
using IdentityText.Models;
using IdentityText.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityText.Repository
{
    public class LectureRepository : Repository<Lecture> , ILectureRepository
    {
     
        private readonly ApplicationDbContext dbContext;

        public LectureRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            dbContext = appDbContext;
        }
        public async Task<IEnumerable<SelectListItem>> SelectListLectureAsync()
        {
            return await dbSet.OrderBy(a => a.Title)
                             .Select(a => new SelectListItem
                             {
                                 Value = a.LectureId.ToString(),
                                 Text = a.Title
                             }).ToListAsync();
        }
    }
}
