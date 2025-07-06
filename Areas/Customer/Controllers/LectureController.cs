using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityText.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Student")]
    public class LectureController : Controller
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public LectureController(
            ILectureRepository lectureRepository,
            IEnrollmentRepository enrollmentRepository,
            IClassGroupRepository classGroupRepository,
            ISubscriptionRepository subscriptionRepository,
            IStudentRepository studentRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _lectureRepository = lectureRepository;
            _studentRepository = studentRepository;
            _classGroupRepository = classGroupRepository;
            _enrollmentRepository = enrollmentRepository;
            _subscriptionRepository = subscriptionRepository;
            _userManager = userManager;
        }
        public IActionResult Index(int classGroupId)
        {
            var userId = _userManager.GetUserId(User);
            var currentStudent = _studentRepository.GetOne(s => s.UserId == userId);
            if (currentStudent == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var isEnrolled = _enrollmentRepository.Get(filter:
                e => e.ClassGroupId == classGroupId &&
                e.StudentId == currentStudent.StudentId &&
                e.EnrollmentStatus == EnrollmentStatus.Active,
                includes: [e => e.ClassGroup, e => e.Student.ApplicationUser]
            ).Any();

            if (!isEnrolled)
            {
                TempData["Error"] = "You need to be enrolled in this course to access this content.";
                return RedirectToAction("Index", "Home");
            }

            var lectures = _lectureRepository.Get(
                filter: l => l.ClassGroupId == classGroupId,
                includes: [e => e.ClassGroup, e => e.Assessment.AssessmentResults, e => e.Attendances]
                );
            ViewBag.ClassGroupId = classGroupId;
            ViewBag.StudentId = currentStudent.StudentId;
            return View(lectures);
        }

        [HttpGet]
        public IActionResult EnterCode(int classGroupId)
        {
            var model = new CourseCodeVM { ClassGroupId = classGroupId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnterCode(CourseCodeVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = _userManager.GetUserId(User);
            var subscription = _subscriptionRepository.GetOne(
                filter: e => e.Enrollment.ClassGroupId == model.ClassGroupId &&
                             e.Enrollment.EnrollmentStatus == EnrollmentStatus.Active &&
                             e.Enrollment.Student.UserId == userId,
                includes: [e => e.Enrollment.ClassGroup , e => e.Enrollment.Student]
                );
            if (subscription == null)
            {
                model.ErrorMessage = "The course does not exist.";
                return View(model);
            }

            if (model.EnteredCode == subscription.Code) 
            {
                return RedirectToAction("Index", "Lecture", new { classGroupId = model.ClassGroupId });
            }
            else
            {
                model.ErrorMessage = "Invalid code. Please try again."

;
                return View(model);
            }
        }
      
    }
}
