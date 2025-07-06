using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var subjects = _subjectRepository.Get();
            return View(subjects);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SubjectTypeList = Enum.GetValues(typeof(SubjectType))
            .Cast<SubjectType>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.ToString()
            }).ToList();

            return View(new Subject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubjectTypeList = Enum.GetValues(typeof(SubjectType))
                .Cast<SubjectType>()
                .Select(e => new SelectListItem
                {
                Value = e.ToString(),
                Text = e.ToString()
            }).ToList();
                return View(subject);
            }
            _subjectRepository.Create(subject);
            _subjectRepository.Commit();
            TempData["notification"] = "Subject created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subject = _subjectRepository.GetOne(e => e.SubjectId == id);
           
            if (subject == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            ViewBag.SubjectTypeList = Enum.GetValues(typeof(SubjectType))
              .Cast<SubjectType>()
              .Select(e => new SelectListItem
              {
                  Value = e.ToString(),
                  Text = e.ToString()
              }).ToList();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubjectTypeList = Enum.GetValues(typeof(SubjectType))
                .Cast<SubjectType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
                return View(subject);
            }
            _subjectRepository.Edit(subject);
            _subjectRepository.Commit();
            TempData["notification"] = "Subject Edited successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var subject = _subjectRepository.GetOne(e => e.SubjectId == id);
            if (subject == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _subjectRepository.Delete(subject);
            _subjectRepository.Commit();
            TempData["notification"] = "Subject Deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
