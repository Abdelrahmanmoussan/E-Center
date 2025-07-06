using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearRepository _academicYearRepository;

        public AcademicYearController(IAcademicYearRepository academicYearRepository)
        {
            _academicYearRepository = academicYearRepository;
        }

        public IActionResult Index()
        {
            var academicYears = _academicYearRepository.Get();
            return View(academicYears);
        }
        [HttpGet]

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AcademicYear { Name = string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AcademicYear academicYear)
        {
            if (ModelState.IsValid)
            {
                _academicYearRepository.Create(academicYear);
                _academicYearRepository.Commit();
                TempData["notification"] = "Successfully Create ";
                return RedirectToAction(nameof(Index));
            }
            return View(academicYear);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var academicYear = _academicYearRepository.GetOne(e => e.AcademicYearId == id);
            if (academicYear == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(academicYear);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AcademicYear academicYear)
        {
 
             _academicYearRepository.Edit(academicYear);
            _academicYearRepository.Commit();
            TempData["notification"] = "Successfully Edit ";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var academicYear = _academicYearRepository.GetOne(e => e.AcademicYearId == id);
            if (academicYear == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _academicYearRepository.Delete(academicYear);
            _academicYearRepository.Commit();
            TempData["notification"] = "Successfully Delete ";
            return RedirectToAction("Index");
        }

    }
}
