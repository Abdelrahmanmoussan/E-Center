using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Teacher")]

    public class PrivateLessonController : Controller
    {
        private readonly IPrivateLessonRepository _privateLessonRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITeacherRepository _teacherRepository;

        public PrivateLessonController(IPrivateLessonRepository privateLessonRepository, ISubjectRepository subjectRepository, ITeacherRepository teacherRepository)
        {
            _privateLessonRepository = privateLessonRepository;
            _subjectRepository = subjectRepository;
            _teacherRepository = teacherRepository;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var privateLessons = _privateLessonRepository.Get(includes: [e => e.Teacher.ApplicationUser, e => e.Subject]);


            if (privateLessons == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(privateLessons);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PrivateLessonVM
            {
                TeacherList = await _teacherRepository.SelectListTeacherAsync(),
                SubjectList = await _subjectRepository.SelectListSubjectAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PrivateLessonVM model)
        {
            if (ModelState.IsValid)
            {
                var privateLesson = new PrivateLesson
                {
                    Price = model.Price,
                    Title = model.Title,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    SubjectId = model.SubjectId,
                    TeacherId = model.TeacherId,
                };
                _privateLessonRepository.Create(privateLesson);
                _privateLessonRepository.Commit();
                TempData["notification"] = "Successfully Created";
                return RedirectToAction(nameof(Index));
            }
            model.TeacherList = await _teacherRepository.SelectListTeacherAsync();
            model.SubjectList = await _subjectRepository.SelectListSubjectAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var privateLesson = _privateLessonRepository.GetOne(p => p.PrivateLessonId == id);
            if (privateLesson == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            var model = new PrivateLessonVM
            {
                PrivateLessonId = privateLesson.PrivateLessonId,
                Price = privateLesson.Price,
                Title = privateLesson.Title,
                StartDate = privateLesson.StartDate,
                EndDate = privateLesson.EndDate,
                SubjectId = privateLesson.SubjectId,
                TeacherId = privateLesson.TeacherId,
                TeacherList = await _teacherRepository.SelectListTeacherAsync(),
                SubjectList = await _subjectRepository.SelectListSubjectAsync()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(PrivateLessonVM model)
        {
            if (ModelState.IsValid)
            {
                var PLesson = _privateLessonRepository.GetOne(e => e.PrivateLessonId == model.PrivateLessonId);
                PLesson.Price = model.Price;
                PLesson.Title = model.Title;
                PLesson.StartDate = model.StartDate;
                PLesson.EndDate = model.EndDate;
                PLesson.SubjectId = model.SubjectId;
                PLesson.TeacherId = model.TeacherId;

                _privateLessonRepository.Edit(PLesson);
                _privateLessonRepository.Commit();
                TempData["notification"] = "Successfully Edited";
                return RedirectToAction(nameof(Index));
            }
            model.TeacherList = await _teacherRepository.SelectListTeacherAsync();
            model.SubjectList = await _subjectRepository.SelectListSubjectAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var privateLesson = _privateLessonRepository.GetOne(p => p.PrivateLessonId == id);
            if (privateLesson == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _privateLessonRepository.Delete(privateLesson);
            _privateLessonRepository.Commit();
            TempData["notification"] = "Successfully Deleted";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetTeachersBySubject(int subjectId)
        {
            var teachers = _teacherRepository.Get(s => s.SubjectId == subjectId, includes: [e => e.ApplicationUser])
                .Select(t => new
                {
                    TeacherId = t.TeacherId,
                    FullName = t.ApplicationUser.FirstName + " " + t.ApplicationUser.LastName
                }).ToList();

            return Json(teachers);
        }





    }
}
