using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using IdentityText.Models;
using IdentityText.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityText.Repository
{
    // Renamed the class to avoid conflict with an existing 'AssessmentRepository' class
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public EnrollmentRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            dbContext = appDbContext;
        }

        
    }
}
