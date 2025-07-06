
using IdentityText.Repository.IRepository;
using IdentityText.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Repository.IRepository
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
         Task<IEnumerable<SelectListItem>> SelectListSubscriptionAsync();
    }
}
