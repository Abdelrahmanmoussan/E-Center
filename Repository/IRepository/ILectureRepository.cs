using IdentityText.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Repository.IRepository
{
    public interface ILectureRepository : IRepository<Lecture>
    {
        Task<IEnumerable<SelectListItem>> SelectListLectureAsync();

    }
}
