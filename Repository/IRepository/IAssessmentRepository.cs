using IdentityText.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Repository.IRepository
{
    public interface IAssessmentRepository : IRepository<Assessment>
    {
        Task<IEnumerable<SelectListItem>> SelectListAssessmentAsync();

        IEnumerable<Assessment> GetWithFullIncludes(
                Expression<Func<Assessment, bool>>? filter = null,
                Func<IQueryable<Assessment>, IQueryable<Assessment>>? include = null,
                bool tracked = true);
        public Assessment? GetOneWithFullIncludes(
              Expression<Func<Assessment, bool>>? filter = null,
              Func<IQueryable<Assessment>, IQueryable<Assessment>>? include = null,
              bool tracked = true);

    }
}
