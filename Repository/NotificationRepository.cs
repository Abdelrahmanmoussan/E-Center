using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using IdentityText.Models;
using IdentityText.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityText.Repository
{
    public class NotificationRepository(ApplicationDbContext appDbContext) : Repository<Notification>(appDbContext), INotificationRepository
    {
        private readonly ApplicationDbContext dbContext;
    }
}
