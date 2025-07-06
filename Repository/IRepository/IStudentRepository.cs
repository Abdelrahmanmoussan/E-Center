
using IdentityText.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<int> CountAsync();

        Task<IEnumerable<SelectListItem>> SelectListStudentAsync();
        Task<IEnumerable<Student>> GetAllWithIncludesAsync();
        Task<Student?> GetByIdWithIncludesAsync(int id);


        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);

        Task<List<Student>> GetLatestStudentsAsync(int count = 5);

        Task<int> CountByMonthAsync(int month, int year);


    }
}
