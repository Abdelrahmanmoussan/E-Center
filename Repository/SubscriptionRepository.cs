
using IdentityText.Data;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Repository
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SubscriptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<SelectListItem>> SelectListSubscriptionAsync()
        {
            return await dbSet.OrderBy(a => a.SubscriptionId)
                              .Select(a => new SelectListItem
                              {
                                  Value = a.SubscriptionId.ToString(),
                                  Text = a.Code
                              }).ToListAsync();
        }

    }
}
