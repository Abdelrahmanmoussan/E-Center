using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using IdentityText.ViewModel.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IdentityText.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public HomeController(ILogger<HomeController> logger,
            IClassGroupRepository classGroupRepository,
            ITeacherRepository teacherRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _logger = logger;
            _classGroupRepository = classGroupRepository;
            _teacherRepository = teacherRepository;
            _enrollmentRepository = enrollmentRepository;
        }


        public IActionResult Index()
        {
            ViewBag.WelcomeMessage = "أهلاً بيك في موقعنا!";
            ViewBag.AboutUs = "نحن نقدم أفضل الخدمات التعليمية مع مدرسين محترفين.";
            ViewBag.ContactUs = "يمكنك التواصل معنا على البريد الإلكتروني: info@example.com أو عبر الهاتف 0123456789";

            var allTeachers = _teacherRepository.Get(includes: [q => q.ApplicationUser, q => q.Subject]);

            // اختيار 4 مدرسين عشوائيًا
            var random = new Random();
            var randomTeachers = allTeachers
                .OrderBy(t => random.Next())
                .Take(4)
                .ToList();

            var allCourses = _classGroupRepository.Get(
                includes: [
                    e => e.Teacher,
                    e => e.Teacher.ApplicationUser,
                    e => e.Subject,
                    e => e.AcademicYear,
                    e => e.Enrollments
                ]);

            var popularCourses = allCourses?
                .Where(c => c.Enrollments != null)
                .OrderByDescending(c => c.Enrollments.Count)
                .Take(6)
                .ToList() ?? new List<ClassGroup>();

            var model = new HomeViewModel
            {
                PopularTeachers = randomTeachers,
                PopularClassGroups = popularCourses,
                Portfolio = popularCourses
            };
            
            return View(model);
        }



        public async Task<IActionResult> TeacherDetails(int TeacherId)
        {
            var teacher = await _teacherRepository.GetByIdWithIncludesAsync(TeacherId);
            if (teacher == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            var related = _teacherRepository.Get(filter:e=>e.SubjectId == teacher.SubjectId ,includes: [q => q.ApplicationUser, q => q.Subject]);

            if (teacher == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var model = new TeacherDetailsViewModel
            {
                Teacher = teacher,
                ClassGroups = (List<ClassGroup>)_classGroupRepository.Get(filter: c => c.TeacherId == teacher.TeacherId,
                includes: [ e=>e.Teacher,
                e=>e.Teacher.ApplicationUser,
                e=>e.Subject,
                e=>e.AcademicYear,
                e => e.Enrollments
                ])
            }; 

            ViewBag.relatedTeachers = related
                .OrderByDescending(t => t.Rating)
                .Take(5)
                .ToList();
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
