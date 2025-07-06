using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using IdentityText.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityText.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Repository
{
    public class AssessmentResultRepository : Repository<AssessmentResult>, IAssessmentResultRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AssessmentResultRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
