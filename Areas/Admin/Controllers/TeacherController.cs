using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassGroupRepository _classGroupRepository;

        public TeacherController(ITeacherRepository teacherRepository, ISubjectRepository subjectRepository, IClassGroupRepository classGroupRepository)
        {
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
            _classGroupRepository = classGroupRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var teachers = _teacherRepository.Get(includes: [e => e.ApplicationUser, s => s.Subject]).ToList();
            Console.WriteLine("Full Name" + teachers[0].FullName);
            return View(teachers);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var teacher = _teacherRepository.GetOne(e => e.TeacherId == id, includes: [e => e.ClassGroups, s => s.PrivateLessons]);
            if (teacher == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }


            _teacherRepository.Delete(teacher);
            _teacherRepository.Commit();

            TempData["notification"] = "Teacher deleted successfully!";
            return RedirectToAction("Index");
        }





    }
}
