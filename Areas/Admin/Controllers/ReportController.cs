using IdentityText.Enums;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ReportController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;

        public ReportController(
            IPaymentRepository paymentRepository,
            ITeacherRepository teacherRepository,
            IEnrollmentRepository enrollmentRepository,
            IStudentRepository studentRepository)
        {
            _paymentRepository = paymentRepository;
            _teacherRepository = teacherRepository;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
        }





        public async Task<IActionResult> PaymentsReport(int? teacherId, DateTime? from, DateTime? to)
        {
            var payments = await _paymentRepository.GetFilteredPaymentsAsync(teacherId, from, to);

            var teacherUser = _teacherRepository.Get(includes: [e => e.ApplicationUser]).ToList();
            var studentUser = _studentRepository.Get(includes: [e => e.ApplicationUser]).ToList();



            var teacherName = teacherId.HasValue
                ? await _teacherRepository.GetTeacherNameByIdAsync(teacherId.Value)
                : "جميع المدرسين";

            var report = new PaymentsReportViewModel
            {
                TeacherName = teacherName,
                Payments = payments,
                TotalRevenue = payments.Sum(p => p.Amount),
                TotalStudents = payments.Select(p => p.Enrollment.StudentId).Distinct().Count(),
                PaidCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Paid),
                PendingCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Pending),
                FailedCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Failed),
                CancelledCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Cancelled)
            };

            ViewBag.Students = new SelectList(studentUser, "StudentId", "FullName");
            ViewBag.Teachers = new SelectList(teacherUser, "TeacherId", "FullName");
            return View(report);
        }

        public async Task<IActionResult> ExportToPdf(int? teacherId, DateTime? from, DateTime? to)
        {
            var payments = await _paymentRepository.GetFilteredPaymentsAsync(teacherId, from, to);
            var model = new PaymentsReportViewModel
            {
                TeacherName = "اسم المدرس",
                Payments = payments,
                TotalRevenue = payments.Sum(p => p.Amount),
                TotalStudents = payments.Select(p => p.Enrollment.StudentId).Distinct().Count(),
                PaidCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Paid),
                PendingCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Pending),
                FailedCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Failed),
                CancelledCount = payments.Count(p => p.PaymentStatus == PaymentStatus.Cancelled)
            };

            var pdfGenerator = new PaymentReportPdfGenerator(model);
            var pdf = pdfGenerator.GeneratePdf();

            return File(pdf, "application/pdf", "تقرير المدفوعات.pdf");
        }




    }
}
