using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.DependencyResolver;
using System.Linq.Expressions;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Teacher")]

    public class LectureController : Controller
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public LectureController(IAttendanceRepository attendanceRepository ,
            ILectureRepository lectureRepository,
            IStudentRepository studentRepository,
            IEnrollmentRepository enrollmentRepository,
            IClassGroupRepository classGroupRepository,
            ITeacherRepository teacherRepository,
             UserManager<ApplicationUser> userManager,
            IAssessmentRepository assessmentRepository)
        {
            _lectureRepository = lectureRepository;
            _teacherRepository = teacherRepository;
            _classGroupRepository = classGroupRepository;
            _assessmentRepository = assessmentRepository;
            _enrollmentRepository = enrollmentRepository;
            _attendanceRepository = attendanceRepository;
            _userManager = userManager;
            _studentRepository = studentRepository;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var teacher = _teacherRepository.GetOne(t => t.UserId == userId);
            if (teacher == null)
                return RedirectToAction("NotFoundPage", "Home"); var lectures = _lectureRepository.Get(filter: e => e.ClassGroup.TeacherId == teacher.TeacherId, includes: [e => e.Assessment, e => e.ClassGroup.Teacher]);
            return View(lectures);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            var userId = _userManager.GetUserId(User);
            var teacher = _teacherRepository.GetOne(t => t.UserId == userId);
            if (teacher == null)
                return RedirectToAction("NotFoundPage", "Home");
            var model = new LectureVM
            {
                AssessmentList = await _assessmentRepository.SelectListAssessmentAsync(),
                ClassGroupList = await _classGroupRepository.SelectListClassGroupByTeacherIdAsync(teacher.TeacherId)
            };


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(LectureVM model)
        {
            if (ModelState.IsValid)
            {
                var lecture = new Lecture
                {
                    Title = model.Title,
                    Description= model.Description,
                    LectureDate = model.LectureDate,
                    VideoURL = model.VideoURL,
                    CreatedAt = model.CreatedAt,
                    ClassGroupId = model.ClassGroupId,
                    AssessmentId = model.AssessmentId
                };
                _lectureRepository.Create(lecture);
                _lectureRepository.Commit();
                TempData["notification"] = "Successfully Created";
                return RedirectToAction(nameof(Index));
            }
            model.AssessmentList = await _assessmentRepository.SelectListAssessmentAsync();
            model.ClassGroupList = await _classGroupRepository.SelectListClassGroupAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var teacher = _teacherRepository.GetOne(t => t.UserId == userId);
            if (teacher == null)
                return RedirectToAction("NotFoundPage", "Home");
            var lecture = _lectureRepository.GetOne( filter: e => e.LectureId == id);

            if (lecture == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var model = new LectureVM
            {
                LectureId = lecture.LectureId,
                Title = lecture.Title,
                Description = lecture.Description,
                LectureDate = lecture.LectureDate,
                VideoURL = lecture.VideoURL,
                CreatedAt = lecture.CreatedAt,
                ClassGroupId = lecture.ClassGroupId,
                AssessmentId = lecture.AssessmentId,
                AssessmentList = await _assessmentRepository.SelectListAssessmentAsync(),
                ClassGroupList = await _classGroupRepository.SelectListClassGroupByTeacherIdAsync(teacher.TeacherId),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(LectureVM model)
        {

            if (ModelState.IsValid)
            {
                var lecture = _lectureRepository.GetOne(filter: e => e.LectureId == model.LectureId);
                if (lecture == null)
                {
                    return RedirectToAction("NotFoundPage", "Home");
                }
                lecture.LectureId =model.LectureId;
                lecture.Title = model.Title;
                lecture.Description = model.Description;
                lecture.LectureDate = model.LectureDate;
                lecture.VideoURL = model.VideoURL;
                lecture.CreatedAt = model.CreatedAt;
                lecture.ClassGroupId = model.ClassGroupId;
                lecture.AssessmentId = model.AssessmentId;
                _lectureRepository.Edit(lecture);
                _lectureRepository.Commit();
                TempData["notification"] = "Successfully Edited";
                return RedirectToAction(nameof(Index));
            }
            model.AssessmentList = await _assessmentRepository.SelectListAssessmentAsync();
            model.ClassGroupList = await _classGroupRepository.SelectListClassGroupAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var lecture = _lectureRepository.GetOne(e => e.LectureId == id);
            if (lecture == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _lectureRepository.Delete(lecture);
            _lectureRepository.Commit();
            TempData["notification"] = "Successfully Deleted";
            return RedirectToAction(nameof(Index));
        }


    }
}
