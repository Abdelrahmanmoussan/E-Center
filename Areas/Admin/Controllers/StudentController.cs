using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        public StudentController(IStudentRepository studentRepository, IClassGroupRepository classGroupRepository, IEnrollmentRepository enrollmentRepository, ISubjectRepository subjectRepository, IAcademicYearRepository academicYearRepository, ISubscriptionRepository subscriptionRepository)
        {

            _studentRepository = studentRepository;
            _classGroupRepository = classGroupRepository;
            _enrollmentRepository = enrollmentRepository;
            _subjectRepository = subjectRepository;
            _academicYearRepository = academicYearRepository;
            _subscriptionRepository = subscriptionRepository;

        }


        public IActionResult Index()
        {
            var Students = _studentRepository.Get(includes: [e => e.ApplicationUser, e => e.AcademicYear, e => e.Enrollments]);
            if (Students == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(Students);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _studentRepository.GetOne(e => e.StudentId == id, includes: [e => e.ApplicationUser, e => e.AcademicYear, e => e.Enrollments]);
            if (student == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }


            _studentRepository.Delete(student);
            _studentRepository.Commit();

            TempData["notification"] = "Teacher deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
