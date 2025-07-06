using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace IdentityText.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassGroupRepository _classGroupRepository;

        public PaymentController(IPaymentRepository paymentRepository, ITeacherRepository teacherRepository, IStudentRepository studentRepository, IClassGroupRepository classGroupRepository)
        {
            _paymentRepository = paymentRepository;
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _classGroupRepository = classGroupRepository;
        }
        public IActionResult Index()
        {
            var payments = _paymentRepository.Get();
            return View(payments);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var classGroup = _classGroupRepository.GetOne(e => e.ClassGroupId == id);
            var teacher = _teacherRepository.GetOne(e => e.TeacherId == id);
            if (teacher == null)
            {
                var student = _studentRepository.GetOne(e => e.StudentId == id);
            }


            var payment = _paymentRepository.GetOne(e=>e.PaymentId == id);
            if (payment == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(payment);
        }






    }
}
