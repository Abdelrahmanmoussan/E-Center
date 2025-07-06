using IdentityText.Data;
using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IdentityText.Repository
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttendanceRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _dbContext = appDbContext;
        }
        public List<SelectListItem> GetAttendanceStatusSelectList()
        {
            return Enum.GetValues(typeof(AttendanceStatus))
                .Cast<AttendanceStatus>()
                .Select(status => new SelectListItem
                {
                    Value = status.ToString(),
                    Text = status.GetType()
                                 .GetMember(status.ToString())
                                 .First()
                                 .GetCustomAttribute<DisplayAttribute>()?.Name ?? status.ToString()
                }).ToList();
        }
        public async Task<IEnumerable<SelectListItem>> SelectListAttendanceAsync()
        {
            return await dbSet.OrderBy(a => a.AttendanceStatus)
                             .Select(a => new SelectListItem
                             {
                                 Value = a.AttendanceId.ToString(),
                                 Text = a.AttendanceStatus.ToString()
                             }).ToListAsync();
        }
    }
}
