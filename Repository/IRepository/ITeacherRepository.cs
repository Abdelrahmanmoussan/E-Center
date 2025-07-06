using IdentityText.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Repository.IRepository
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<int> CountAsync();

        Task<Teacher?> GetByIdWithIncludesAsync(int id);
        Task<IEnumerable<SelectListItem>> SelectListTeacherAsync();

        Task<string> GetTeacherNameByIdAsync(int teacherId);
        Task<int> CountByMonthAsync(int month, int year);



    }
}
