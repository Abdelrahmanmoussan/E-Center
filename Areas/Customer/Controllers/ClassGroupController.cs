
using Microsoft.AspNetCore.Identity;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using IdentityText.Models.ViewModel;
using IdentityText.Repository;
using IdentityText.Enums;
using System;
using Microsoft.EntityFrameworkCore;


namespace IdentityText.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ClassGroupController : Controller
    {
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassGroupController(
         IClassGroupRepository classGroupRepository,
         ITeacherRepository teacherRepository,
         IStudentRepository studentRepository,
         IAcademicYearRepository academicYearRepository,
         ISubjectRepository subjectRepository,
         UserManager<ApplicationUser> userManager)  // inject here
        {
            _classGroupRepository = classGroupRepository;
            _teacherRepository = teacherRepository;
            _academicYearRepository = academicYearRepository;
            _subjectRepository = subjectRepository;
            _userManager = userManager;
            _studentRepository = studentRepository;
        }
     

        public IActionResult Details(int id)
        {
            var classGroup = _classGroupRepository.GetOne(
                filter: e=>e.ClassGroupId == id,
                includes: [ e=>e.Teacher,
                e=>e.Teacher.ApplicationUser,
                e=>e.Subject,
                e=>e.AcademicYear,
                e => e.Enrollments
                ]);
            if(classGroup == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(classGroup);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var teacher = _teacherRepository.GetOne( t => t.UserId == currentUser.Id,
                includes: [e => e.Subject , e => e.ApplicationUser]);

            var model = new ClassGroupVM
            {
                AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync(),
                TeacherId = teacher?.TeacherId ?? 0,
                SubjectId = teacher?.SubjectId ?? 0,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassGroupVM model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var teacher = _teacherRepository.GetOne(t => t.UserId == currentUser.Id,
                includes: [e => e.Subject, e => e.ApplicationUser]);
            if (teacher == null)
            {
                ModelState.AddModelError("", "You must be a teacher to create a class group.");
                model.AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync();
                model.TeacherId = 0; // No teacher available
                model.SubjectId = 0; // No subject available
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var classGroup = new ClassGroup
                {
                    Title = model.Title,
                    Location = model.Location,
                    Price = model.Price,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    AcademicYearId = model.AcademicYearId,
                    SubjectId = teacher.SubjectId,
                    TeacherId = teacher.TeacherId
                };
                _classGroupRepository.Create(classGroup);
                _classGroupRepository.Commit();
                TempData["notification"] = "Successfully Created";
                return RedirectToAction(controllerName: "Home", actionName:"Index");
            }
            model.AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync();
            return View(model);
        }
        public async Task<IActionResult> CouresAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");


            var classGroups =  _classGroupRepository.Get( includes: [ 
                e => e.Teacher.ApplicationUser,
                e => e.Subject, 
                e => e.AcademicYear 
                ]);


            ViewBag.Subjects = _subjectRepository.Get();
            ViewBag.Teachers = _teacherRepository.Get(includes: [t => t.ApplicationUser]);




            string subjectName = Request.Query["subjectName"];
            string teacherName = Request.Query["teacherName"];
            decimal.TryParse(Request.Query["minPrice"], out decimal minPrice);
            decimal.TryParse(Request.Query["maxPrice"], out decimal maxPrice);

            if (!string.IsNullOrEmpty(subjectName))
            {
                classGroups = classGroups
                    .Where(c => c.Subject.Title == subjectName)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(teacherName))
            {
                classGroups = classGroups
                    .Where(c => c.Teacher.ApplicationUser.FirstName == teacherName)
                    .ToList();
            }

            if (minPrice > 0)
            {
                classGroups = classGroups
                    .Where(c => c.Price >= minPrice)
                    .ToList();
            }

            if (maxPrice > 0)
            {
                classGroups = classGroups
                    .Where(c => c.Price <= maxPrice)
                    .ToList();
            }
            return View(classGroups);
        }


    }
}
