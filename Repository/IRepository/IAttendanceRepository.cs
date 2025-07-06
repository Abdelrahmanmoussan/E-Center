using IdentityText.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Repository.IRepository
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        List<SelectListItem> GetAttendanceStatusSelectList();
        Task<IEnumerable<SelectListItem>> SelectListAttendanceAsync();

    }
}
