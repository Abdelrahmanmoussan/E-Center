using IdentityText.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using IdentityText.Repository;
using IdentityText.Utility; 
using Microsoft.Extensions.DependencyInjection;
using IdentityText.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityText
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(opts =>
                opts.SignIn.RequireConfirmedEmail = true
            );

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
            builder.Services.AddScoped<ILectureRepository, LectureRepository>();
            builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            builder.Services.AddScoped<IAssessmentResultRepository, AssessmentResultRepository>();
            builder.Services.AddScoped<IPrivateLessonRepository, PrivateLessonRepository>();
            builder.Services.AddScoped<IClassGroupRepository, ClassGroupRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddScoped <INotificationRepository, NotificationRepository>();


            // قراءة إعدادات Stripe من appsettings.json
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            // تعيين مفتاح Stripe السري مباشرة
            Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // تسجيل خدمة الدفع الخاصة بك في DI
            builder.Services.AddScoped<StripePaymentService>();

            builder.Services.AddScoped<SubscriptionPdfService>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
