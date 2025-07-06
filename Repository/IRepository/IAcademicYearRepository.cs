using IdentityText.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Repository.IRepository
{
    public interface IAcademicYearRepository : IRepository<AcademicYear>
    {
        Task<IEnumerable<SelectListItem>> SelectListAcademicYearAsync();

    }
}
