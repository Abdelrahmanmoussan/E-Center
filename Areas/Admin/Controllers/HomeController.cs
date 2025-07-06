using IdentityText.Repository.IRepository;
using IdentityText.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Teacher")]
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPrivateLessonRepository _privateLessonRepository;

        public HomeController(IStudentRepository studentRepository, ITeacherRepository teacherRepository, IClassGroupRepository classGroupRepository, IPaymentRepository paymentRepository, IPrivateLessonRepository privateLessonRepository)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _classGroupRepository = classGroupRepository;
            _paymentRepository = paymentRepository;
            _privateLessonRepository = privateLessonRepository;
        }
        public async Task<IActionResult> Index()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var previousMonth = DateTime.Now.AddMonths(-1).Month;
            var previousYear = DateTime.Now.AddMonths(-1).Year;

            //  الطلاب
            var studentsCount = await _studentRepository.CountAsync();
            var currentStudentCount = await _studentRepository.CountByMonthAsync(currentMonth, currentYear);
            var previousStudentCount = await _studentRepository.CountByMonthAsync(previousMonth, previousYear);
            double studentGrowth = previousStudentCount == 0 ? 100 : ((double)(currentStudentCount - previousStudentCount) / previousStudentCount) * 100;

            //  المدرسين
            var teachersCount = await _teacherRepository.CountAsync();
            var currentTeacherCount = await _teacherRepository.CountByMonthAsync(currentMonth, currentYear);
            var previousTeacherCount = await _teacherRepository.CountByMonthAsync(previousMonth, previousYear);
            double teacherGrowth = previousTeacherCount == 0 ? 100 : ((double)(currentTeacherCount - previousTeacherCount) / previousTeacherCount) * 100;

            //  الكورسات
            var totalCourses = await _classGroupRepository.CountAsync();
            var currentCourseCount = await _classGroupRepository.CountByMonthAsync(currentMonth, currentYear);
            var previousCourseCount = await _classGroupRepository.CountByMonthAsync(previousMonth, previousYear);
            double courseGrowth = previousCourseCount == 0 ? 100 : ((double)(currentCourseCount - previousCourseCount) / previousCourseCount) * 100;

            //  الإيرادات
            var totalRevenue = await _paymentRepository.GetTotalRevenueAsync();
            var currentRevenue = await _paymentRepository.GetTotalRevenueByMonthAsync(currentMonth, currentYear);
            var previousRevenue = await _paymentRepository.GetTotalRevenueByMonthAsync(previousMonth, previousYear);
            double revenueGrowth = previousRevenue == 0 ? 100 : ((double)(currentRevenue - previousRevenue) / (double)previousRevenue) * 100;

            //  الدروس الخاصة
            var privateLessonsCount = await _privateLessonRepository.CountAsync();
            var currentPrivateLessons = await _privateLessonRepository.CountByMonthAsync(currentMonth, currentYear);
            var previousPrivateLessons = await _privateLessonRepository.CountByMonthAsync(previousMonth, previousYear);
            double privateLessonGrowth = previousPrivateLessons == 0 ? 100 : ((double)(currentPrivateLessons - previousPrivateLessons) / previousPrivateLessons) * 100;

            //  مبيعات اليوم
            var todaySales = await _paymentRepository.GetTodaySalesAsync();

            //  أحدث الطلاب
            var latestStudents = (await _studentRepository.GetLatestStudentsAsync())
                .Select(s => new StudentDto
                {
                    FullName = s.FullName,
                    EnrollmentDate = s.EnrollmentDate,
                    AvatarUrl = s.ApplicationUser.Photo
                }).ToList();

            //  أحدث الكورسات
            var latestCourses = await _classGroupRepository.GetLatestCoursesAsync();

            var model = new DashboardViewModel
            {
                StudentsCount = studentsCount,
                StudentGrowthPercentage = Math.Round(studentGrowth, 1),

                TeachersCount = teachersCount,
                TeacherGrowthPercentage = Math.Round(teacherGrowth, 1),

                TotalCourses = totalCourses,
                CourseGrowthPercentage = Math.Round(courseGrowth, 1),

                TotalRevenue = totalRevenue,
                RevenueGrowthPercentage = Math.Round(revenueGrowth, 1),

                TodaySales = todaySales,

                PrivateLessonsCount = privateLessonsCount,
                PrivateLessonGrowthPercentage = Math.Round(privateLessonGrowth, 1),

                LatestStudents = latestStudents,
                LatestCourses = latestCourses
            };

            return View(model);
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }


    }
}
