using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityText.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AssessmentController : Controller
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentResultRepository _resultRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssessmentController(
            IAssessmentRepository assessmentRepository,
            IAssessmentResultRepository resultRepository,
            IStudentRepository studentRepository,
            UserManager<ApplicationUser> userManager)
        {
            _assessmentRepository = assessmentRepository;
            _resultRepository = resultRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Details(int lectureId)
        {
            var userId = _userManager.GetUserId(User);
            var student = _studentRepository.GetOne(s => s.UserId == userId);
            if (student == null)
                return RedirectToAction("NotFoundPage", "Home");
            var assessment = _assessmentRepository.GetOne(
                 filter: e => e.LectureId == lectureId,
                  includes: [a => a.AssessmentResults]
              );

            if (assessment == null || student == null)
                return RedirectToAction("NotFoundPage", "Home");

            var studentResult = assessment.AssessmentResults
                .FirstOrDefault(r => r.StudentId == student.StudentId);

            var vm = new AssessmentDetailsVM
            {
                AssessmentId = assessment.AssessmentId,
                Title = assessment.Title,
                Description = assessment.Description,
                DeliveryDate = assessment.DeliveryDate,
                AssessmentLink = assessment.AssessmentLink,
                MaxScore = assessment.MaxScore,

                Score = studentResult?.Score,
                Grade = studentResult?.Grade,
                Feedback = studentResult?.Feedback,
                StudentSolutionPath = studentResult?.StudentSolutionPath
            };
            ViewBag.lectureId = assessment.LectureId;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadSolution(int assessmentId,int lectureId, IFormFile UploadedSolution)
        {
            var userId = _userManager.GetUserId(User);
            var student = _studentRepository.GetOne(s => s.UserId == userId);

            if (student == null || UploadedSolution == null)
                return RedirectToAction("NotFoundPage", "Home");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedSolution.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\AssessmentFile", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    UploadedSolution.CopyTo(stream);
                }

            var existingResult = _resultRepository.GetOne(
                r => r.AssessmentId == assessmentId && r.StudentId == student.StudentId);

            if (existingResult != null)
            {
                existingResult.StudentSolutionPath = fileName;
                _resultRepository.Edit(existingResult);
            }
            else
            {
                var result = new AssessmentResult
                {
                    AssessmentId = assessmentId,
                    StudentId = student.StudentId,
                    StudentSolutionPath = fileName,
                    Score = 0,
                    Grade = "لم يصحح",
                };
                _resultRepository.Create(result);
            }

            _resultRepository.Commit();

            return RedirectToAction("Details", new { lectureId });
        }

    }
}
