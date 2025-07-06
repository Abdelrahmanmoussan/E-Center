using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityText.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(INotificationRepository notificationRepository, UserManager<ApplicationUser> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var unreadNotifications = _notificationRepository.Get(n => n.UserId == userId && !n.IsRead);

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                _notificationRepository.Edit(notification); 
            }

            _notificationRepository.Commit(); 

            var notifications = _notificationRepository.Get(n => n.UserId == userId)
                .OrderByDescending(n => n.Date)
                .Select(n => new NotificationVM
                {
                    Message = n.Message,
                    Date = n.Date,
                    IsRead = n.IsRead
                })
                .ToList();

            return View(notifications);
        }

    }
}
