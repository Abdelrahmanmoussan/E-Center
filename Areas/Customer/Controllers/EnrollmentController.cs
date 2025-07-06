using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using IdentityText.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Stripe.Climate;
using System.Linq.Expressions;

namespace IdentityText.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Student")]
    public class EnrollmentController : Controller
    {
        private readonly IClassGroupRepository _clasGroupRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StripePaymentService _stripePaymentService;


        public EnrollmentController(
            IClassGroupRepository clasGroupRepository,
            IEnrollmentRepository enrollmentRepository,
            IPaymentRepository paymentRepository,
            IStudentRepository studentRepository,
            ISubscriptionRepository subscriptionRepository,
            UserManager<ApplicationUser> userManager,
            StripePaymentService stripePaymentService
            )
        {
            _clasGroupRepository = clasGroupRepository;
            _userManager = userManager;
            _enrollmentRepository = enrollmentRepository;
            _paymentRepository = paymentRepository;
            _studentRepository = studentRepository;
            _stripePaymentService = stripePaymentService;
            _subscriptionRepository = subscriptionRepository;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var enrollments = _enrollmentRepository
                .Get(e => e.Student.UserId == userId,
                includes: [e => e.ClassGroup.Teacher.ApplicationUser, e => e.Student.ApplicationUser])
                .ToList();
            if (enrollments == null || !enrollments.Any())
            {
                return View(new List<Enrollment>());
            }
            
            return View(enrollments);
        }

        public async Task<IActionResult> RegisterAsync(int id)
        {
            var currentCG = _clasGroupRepository.GetOne(e => e.ClassGroupId == id, tracked: true);
            if (currentCG == null)
            {
                TempData["Error"] = "المجموعة غير موجودة.";
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var student = _studentRepository.GetOne(s => s.UserId == currentUser.Id, tracked: true);
            if (student == null)
            {
                TempData["Error"] = "المجموعة غير موجودة.";
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            var userId = student.StudentId;

            if (student.AcademicYearId != currentCG.AcademicYearId)
            {
                TempData["Error"] = "هذا الكورس غير مناسب ليك";
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }


            // التحقق من عدم وجود تسجيل سابق لنفس الطالب في نفس المجموعة
            bool alreadyEnrolled = _enrollmentRepository
                .Get(e => e.ClassGroupId == id && e.StudentId == userId)
                .Any();
            if (alreadyEnrolled)
            {
                TempData["Error"] = "أنت مسجل بالفعل في هذه المجموعة.";
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            // create enrollment
            var enrollment = new Enrollment
            {
                ClassGroupId = currentCG.ClassGroupId,
                StudentId = userId, // Convert string to int
                EnrollmentDate = DateTime.Now,
                EnrollmentStatus = EnrollmentStatus.PendingPayment, // حالة الانتظار حتى الدفع
                Notes = "تم التسجيل في المجموعة " + currentCG.Title + " في " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

            };
            _enrollmentRepository.Create(enrollment);
            _enrollmentRepository.Commit();
            // create Payment
            var payment = new Models.Payment
            {
                EnrollmentId = enrollment.EnrollmentId,
                Amount = currentCG.Price,
                PaymentDate = DateTime.Now,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = PaymentMethod.Cash,
                PlatformPercentage = 0.10m,
                NetAmountForTeacher = currentCG.Price * (1 - 0.10m),
                Notes = "Payment for enrollment in class group " + currentCG.Title
            };
            _paymentRepository.Create(payment);
            _paymentRepository.Commit();
            TempData["notification"] = "تم التسجيل في الكورس وتم انشاء دفع له";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult ProcessPayment(int id)
        {
            var payment = _paymentRepository.GetOne(
                p => p.EnrollmentId == id,
                includes:[ p => p.Enrollment, p => p.Enrollment.ClassGroup]
                );

            if (payment == null)
            {
                TempData["notification"] = "عملية الدفع غير موجودة.";
                return RedirectToAction("Index", "Enrollment"); 
            }
            if (payment.Enrollment == null || payment.Enrollment.ClassGroup == null)
            {
                TempData["notification"] = "بيانات التسجيل غير مكتملة.";
                return RedirectToAction("Index", "Enrollment");
            }
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "EGP",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = payment.Enrollment.ClassGroup.Title,
                        Description = $"السنة الدراسية: {payment.Enrollment.ClassGroup.AcademicYear?.Name ?? "غير محدد"}  | بداية الكورس: {payment.Enrollment.ClassGroup.StartDate.ToString("yyyy-MM-dd")} | طريقة الدفع: {payment.PaymentMethod}"
                    },
                    UnitAmount = (long)(payment.Amount * 100), 
                     },
                     Quantity = 1,
                 }
                },
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Enrollment/ConfirmPayment?paymentId={payment.PaymentId}",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Enrollment/Cancel",
                };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }
        private string GenerateSubscriptionCode(int enrollmentId)
        {
            return $"SUB-{enrollmentId}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }

        [HttpGet]
        public IActionResult ConfirmPayment(int paymentId)
        {
            var payment = _paymentRepository.GetOne(p => p.PaymentId == paymentId);
            if (payment == null)
            {
                TempData["notification"] = "عملية الدفع غير موجودة.";
                return RedirectToAction("NotFoundPage", "Home");
            }

            payment.PaymentStatus = PaymentStatus.Paid;
            _paymentRepository.Edit(payment);
            _paymentRepository.Commit();

            var enrollment = _enrollmentRepository.GetOne(e => e.EnrollmentId == payment.EnrollmentId, includes: [e=>e.ClassGroup]);
            if(enrollment != null) {
                enrollment.EnrollmentStatus = EnrollmentStatus.Active;
                _enrollmentRepository.Edit(enrollment);
                _enrollmentRepository.Commit();

                var subscription = new Subscription
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    StartDate = DateTime.UtcNow,
                    EndDate = enrollment.ClassGroup?.EndDate ?? DateTime.UtcNow.AddMonths(1),
                    Code = GenerateSubscriptionCode(enrollment.EnrollmentId),
                    SubscriptionStatus = SubscriptionStatus.Active
                };

                _subscriptionRepository.Create(subscription);
                _subscriptionRepository.Commit();
            }
            TempData["notification"] = "تم تأكيد الدفع والتسجيل وتوليد كود الاشتراك.";
            return RedirectToAction(actionName: "Subscriptions", controllerName: "Enrollment");
        }

        public IActionResult Subscriptions()
        {
            var userId = _userManager.GetUserId(User);
            var student = _studentRepository.GetOne(s => s.UserId == userId);

            if (student == null)
            {
                TempData["notification"] = "الطالب غير موجود.";
                return RedirectToAction("Index", "Home");
            }

            var subscriptions = _subscriptionRepository
                .Get(s => s.Enrollment.StudentId == student.StudentId, includes: [e=>e.Enrollment.ClassGroup])
                .Select(s => new SubscriptionVM
                {
                    Code = s.Code,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    SubscriptionStatus = s.SubscriptionStatus,
                    ClassGroupTitle = s.Enrollment.ClassGroup.Title
                })
                .ToList();

            return View(subscriptions);
        }


        [HttpGet]
        public IActionResult DownloadPDF(string code)
        {
            var subscription = _subscriptionRepository.GetOne(
                filter: s => s.Code == code,
                includes: [
                 s => s.Enrollment.Student.ApplicationUser,
                 s => s.Enrollment.ClassGroup
                ]);

            if (subscription == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            // Create an instance of SubscriptionPdfService
            var subscriptionPdfService = new SubscriptionPdfService();

            // Use the instance to call the GenerateSubscriptionPdf method
            var pdf = subscriptionPdfService.GenerateSubscriptionPdf(subscription);
            return File(pdf, "application/pdf", $"Subscription_{subscription.Code}.pdf");
        }


        public IActionResult Delete(int id)
        {
            var enrollment = _enrollmentRepository.GetOne(e => e.EnrollmentId == id);
            if (enrollment == null )
            {
                return NotFound();
            }
            _enrollmentRepository.Delete(enrollment);
            _enrollmentRepository.Commit();
            TempData["notification"] = "Successfully Delete your enrollment ";
            return RedirectToAction("Index");
        }


    }
}
