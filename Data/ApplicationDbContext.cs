using IdentityText.Enums;
using IdentityText.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityText.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<ClassGroup> ClassGroups { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PrivateLesson> PrivateLessons { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<TeacherAcademicYear> TeacherAcademicYears { get; set; }
        public DbSet<PrivateLessonStudent> PrivateLessonStudents { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<Favorite> favorites { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // ===== TeacherAcademicYear =====
            builder.Entity<TeacherAcademicYear>()
                .HasKey(x => new { x.TeacherId, x.AcademicYearId });
            // ===== PrivateLessonStudent =====
            builder.Entity<PrivateLessonStudent>()
                .HasKey(x => new { x.PrivateLessonId, x.StudentId });
            // ===== NotificationRecipient =====
            builder.Entity<NotificationRecipient>()
                .HasKey(x => new { x.NotificationId, x.NotificationRecipientId });


            builder.Entity<Teacher>()
                .HasOne(t => t.ApplicationUser)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attendance>()
                .HasOne(a => a.Enrollment)
                .WithMany()
                .HasForeignKey(a => a.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attendance>()
               .HasOne(a => a.Student)
               .WithMany()
               .HasForeignKey(a => a.StudentId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ClassGroup>()
                .HasOne(cg => cg.Teacher)
                .WithMany(t => t.ClassGroups)
                .HasForeignKey(cg => cg.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PrivateLesson>()
                .HasOne(pl => pl.Teacher)
                .WithMany(t => t.PrivateLessons)
                .HasForeignKey(pl => pl.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ClassGroup>()
                .HasOne(cg => cg.AcademicYear)
                .WithMany(ay => ay.ClassGroups)
                .HasForeignKey(cg => cg.AcademicYearId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Subscription>()
                .HasOne(s => s.Enrollment)
                .WithMany(e => e.Subscriptions)
                .HasForeignKey(s => s.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NotificationRecipient>()
                .HasOne(nr => nr.User)
                .WithMany()
                .HasForeignKey(nr => nr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NotificationRecipient>()
                .HasOne(nr => nr.Notification)
                .WithMany(n => n.NotificationRecipients)
                .HasForeignKey(nr => nr.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            // seeding data for subject table
            builder.Entity<Subject>().HasData(
                new Subject
                {
                    SubjectId = 1,
                    Title = "mathematics",
                    Description = "Basic mathematics subject",
                    SubjectType = SubjectType.General
                },
                new Subject
                {
                    SubjectId = 2,
                    Title = "sciences",
                    Description = "Basic science subject",
                    SubjectType = SubjectType.General
                },
                new Subject
                {
                    SubjectId = 3,
                    Title = "Arabic",
                    Description = "Basic Arabic language subject",
                    SubjectType = SubjectType.General
                },
                new Subject
                {
                    SubjectId = 4,
                    Title = "english",
                    Description = "Basic English subject",
                    SubjectType = SubjectType.Optional
                },
                new Subject
                {
                    SubjectId = 5,
                    Title = "Social studies",
                    Description = "Basic social studies subject",
                    SubjectType = SubjectType.Optional
                }
            );
            builder.Entity<AcademicYear>().HasData(
                new AcademicYear
                {
                    AcademicYearId = 1,
                    Name = "First year of middle school "
                },
                new AcademicYear
                {
                    AcademicYearId = 2,
                    Name = "Second year of middle school"
                },
                new AcademicYear
                {
                    AcademicYearId = 3,
                    Name = "Third year of middle school"
                }
            );



            var hasher = new PasswordHasher<ApplicationUser>();

            // First admin user (Amin)
            var adminUser1 = new ApplicationUser
            {
                Id = "7aafd540-fdf8-482b-804d-780fb6726703",
                UserName = "amin",
                NormalizedUserName = "AMIN",
                Email = "amin@gmail.com",
                NormalizedEmail = "AMIN@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Amin",
                LastName = "Mohamed",
                Address = "Quesna,Menofia",
                Photo = "admin.jpg"
            };
            adminUser1.PasswordHash = hasher.HashPassword(adminUser1, "Admin@1234");

            // Second admin user (Abdelrahman Moussan)
            var adminUser2 = new ApplicationUser
            {
                Id = "9b4cd611-6c35-4c98-a0dc-1d2e1349ab91", // Generate a new GUID
                UserName = "abdelrahman",
                NormalizedUserName = "ABDELRAHMAN",
                Email = "abdelrahmanmoussan@gmail.com",
                NormalizedEmail = "ABDELRAHMANMOUSSAN@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Abdelrahman",
                LastName = "Moussan",
                Address = "Port Said",
                Photo = "Moussan.jpg"
            };
            adminUser2.PasswordHash = hasher.HashPassword(adminUser2, "Admin@1234");
            // Customer 
            var Customer = new ApplicationUser
            {
                Id = "7zzfd540-fdf8-482b-804d-780fb6726703",
                UserName = "ahmed",
                NormalizedUserName = "AHMED",
                Email = "ahmed@gmail.com",
                NormalizedEmail = "AHMED@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Ahmed",
                LastName = "Samir",
                Address = "Quesna,Menofia",
                Photo = "Customer.jpg"
            };

            Customer.PasswordHash = hasher.HashPassword(Customer, "Ahmed@1234");
            // Teacher 
            var Teacher = new ApplicationUser
            {
                Id = "7nnfd540-fdf8-482b-804d-780fb6726703",
                UserName = "omar",
                NormalizedUserName = "Omar",
                Email = "omar@gmail.com",
                NormalizedEmail = "Omar@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Omar",
                LastName = "Fathi",
                Address = "Quesna,Menofia",
                Photo = "Teacher.jpg"
            };

            Teacher.PasswordHash = hasher.HashPassword(Teacher, "Omar@1234");
            // Seed users
            builder.Entity<ApplicationUser>().HasData(adminUser1, adminUser2, Customer, Teacher);
            // 
            var student1 = new Student
            {
                StudentId = 1,
                UserId = Customer.Id,
                ParentName = "Mr. Samir",
                ParentPhone = "01012345678",
                ParentMail = "samir@gmail.com",
                StudentDB = new DateTime(2008, 5, 10),
                EnrollmentDate = DateTime.Now,
                EmergencyContact = "01098765432",
                //AttendancePercent = 100,
                AcademicYearId = 2
            };

            builder.Entity<Student>().HasData(student1);

            builder.Entity<Teacher>().HasData(new Teacher
            {
                TeacherId = 1,
                UserId = Teacher.Id,
                TeacherHireDate = new DateTime(2023, 1, 15),
                TeacherDB = new DateTime(1995, 5, 20),
                TeacherNetAmount = 3000,
                TeacherNotes = "Expert in physics",
                SubjectId = 1,
                Rating = 4.5m
            });

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "5aa54943-8b55-4399-91b7-d247ab235cf3",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "8bbc91c3-0b30-4451-b78e-a26edf6e1c61",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "7vvfd540-fdf8-482b-804d-780fb6726703",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
                new IdentityRole
                {
                    Id = "8ddc91c3-0b30-4451-b78e-a26edf6e1c61",
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                }
            );
            builder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   UserId = "7aafd540-fdf8-482b-804d-780fb6726703",
                   RoleId = "5aa54943-8b55-4399-91b7-d247ab235cf3"
               },
                new IdentityUserRole<string>
                {
                    UserId = "9b4cd611-6c35-4c98-a0dc-1d2e1349ab91", // Abdelrahman
                    RoleId = "5aa54943-8b55-4399-91b7-d247ab235cf3"
                },
                new IdentityUserRole<string>
                {
                    UserId = "7zzfd540-fdf8-482b-804d-780fb6726703", // student
                    RoleId = "7vvfd540-fdf8-482b-804d-780fb6726703"
                },
                new IdentityUserRole<string>
                {
                    UserId = "7nnfd540-fdf8-482b-804d-780fb6726703", // teacher
                    RoleId = "8ddc91c3-0b30-4451-b78e-a26edf6e1c61"
                }
           );

        }

    }
}