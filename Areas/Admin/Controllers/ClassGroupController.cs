using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Teacher")]
    public class ClassGroupController : Controller
    {
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClassGroupController(IClassGroupRepository classGroupRepository,
            ISubjectRepository subjectRepository,
            ITeacherRepository teacherRepository,
            UserManager<ApplicationUser> userManager,
            IAcademicYearRepository academicYearRepository)
        {
            _classGroupRepository = classGroupRepository;
            _subjectRepository = subjectRepository;
            _teacherRepository = teacherRepository;
            _academicYearRepository = academicYearRepository;
            _userManager = userManager;
        }

  
        [HttpGet]
        public IActionResult Index(string searchTitle, string searchLocation, string searchTeacher, int? searchYearId, decimal? searchPrice, int page = 1, int pageSize = 5)
        {
            var classGroupsAll = _classGroupRepository.Get(
                includes: [e => e.AcademicYear, e => e.Subject, e => e.Teacher.ApplicationUser]
            ).ToList();

            var filteredClassGroups = classGroupsAll.Where(group =>
                (string.IsNullOrEmpty(searchTitle) || group.Title.StartsWith(searchTitle, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(searchLocation) || group.Location.StartsWith(searchLocation, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(searchTeacher) || $"{group.Teacher.ApplicationUser.FirstName} {group.Teacher.ApplicationUser.LastName}".StartsWith(searchTeacher, StringComparison.OrdinalIgnoreCase)) &&
                (!searchYearId.HasValue || group.AcademicYearId == searchYearId.Value) &&
                (!searchPrice.HasValue || group.Price == searchPrice.Value)
            ).ToList();

            int totalItems = filteredClassGroups.Count;
            var pagedData = filteredClassGroups.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.SearchTitle = searchTitle;
            ViewBag.SearchLocation = searchLocation;
            ViewBag.SearchTeacher = searchTeacher;
            ViewBag.SearchYearId = searchYearId;
            ViewBag.SearchPrice = searchPrice;

            return View(pagedData);
        }


        [HttpGet]
        public IActionResult GetTeachersBySubject(int subjectId)
        {
            var teachers = _teacherRepository.Get(
                filter: t => t.SubjectId == subjectId,
                includes: [ t => t.ApplicationUser ]
            )
            .Select(t => new
            {
                teacherId = t.TeacherId,
                fullName = t.ApplicationUser.FirstName + " " + t.ApplicationUser.LastName
            });
            return Json(teachers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var model = new ClassGroupVM
            {
                AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync(),
                SubjectsList = await _subjectRepository.SelectListSubjectAsync(),
                TeacherList = await _teacherRepository.SelectListTeacherAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassGroupVM model)
        {
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
                    SubjectId = model.SubjectId,
                    TeacherId = model.TeacherId
                };
                _classGroupRepository.Create(classGroup);
                _classGroupRepository.Commit();
                TempData["notification"] = "Successfully Created";
                return RedirectToAction(nameof(Index));
            }
            model.AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync();
            model.SubjectsList = await _subjectRepository.SelectListSubjectAsync();
            model.TeacherList = await _teacherRepository.SelectListTeacherAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var classGroup = _classGroupRepository.GetOne(e => e.ClassGroupId == id);
            if (classGroup == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var model = new ClassGroupVM
            {
                ClassGroupId = classGroup.ClassGroupId,
                Title = classGroup.Title,
                Location = classGroup.Location,
                Price = classGroup.Price,
                StartDate = classGroup.StartDate,
                EndDate = classGroup.EndDate,
                SubjectId = classGroup.SubjectId,
                TeacherId = classGroup.TeacherId,
                AcademicYearId = classGroup.AcademicYearId,
                AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync(),
                SubjectsList = await _subjectRepository.SelectListSubjectAsync(),
                TeacherList = await _teacherRepository.SelectListTeacherAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(ClassGroupVM model)
        {
            if (ModelState.IsValid)
            {
                var classGroup = _classGroupRepository.GetOne(e => e.ClassGroupId == model.ClassGroupId);
                if (classGroup == null)
                {
                    return RedirectToAction("NotFoundPage", "Home");
                }
                classGroup.Title = model.Title;
                classGroup.Location = model.Location;
                classGroup.Price = model.Price;
                classGroup.StartDate = model.StartDate;
                classGroup.EndDate = model.EndDate;
                classGroup.SubjectId = model.SubjectId;
                classGroup.TeacherId = model.TeacherId;
                classGroup.AcademicYearId = model.AcademicYearId;
                _classGroupRepository.Edit(classGroup);
                _classGroupRepository.Commit();
                TempData["notification"] = "Successfully Edited";
                return RedirectToAction(nameof(Index));
            }
            model.AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync();
            model.SubjectsList = await _subjectRepository.SelectListSubjectAsync();
            model.TeacherList = await _teacherRepository.SelectListTeacherAsync();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var classGroup = _classGroupRepository.GetOne(e => e.ClassGroupId == id);
            if (classGroup == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _classGroupRepository.Delete(classGroup);
            _classGroupRepository.Commit();
            TempData["notification"] = "Successfully Deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
