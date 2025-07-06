

using IdentityText.Models;

namespace IdentityText.ViewModel
{
    public class DashboardViewModel
    {
        public int StudentsCount { get; set; }
        public double StudentGrowthPercentage { get; set; }
        public int TeachersCount { get; set; }
        public double TeacherGrowthPercentage { get; set; }

        public int TotalCourses { get; set; }
        public double CourseGrowthPercentage { get; set; }

        public decimal TotalRevenue { get; set; }
        public double RevenueGrowthPercentage { get; set; }

        public decimal TodaySales { get; set; }
        public int PrivateLessonsCount { get; set; }
        public double PrivateLessonGrowthPercentage { get; set; }

        public List<string> RevenueLabels { get; set; } = new();
        public List<decimal> RevenueData { get; set; } = new();
        public List<RecentTransaction> RecentTransactions { get; set; } = new();
        public List<DashboardMessage> RecentMessages { get; set; } = new();
        public List<CalendarEvent> CalendarEvents { get; set; } = new();
        public List<TodoItem> TodoList { get; set; } = new();
        public List<StudentDto> LatestStudents { get; set; } // New
        public List<ClassGroup> LatestCourses { get; set; } = new();

    }

    public class StudentDto
    {
        public string FullName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? AvatarUrl { get; set; } // Optional if you have image
    }

    public class RecentTransaction
    {
        public string StudentName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class DashboardMessage
    {
        public string SenderName { get; set; }
        public string MessagePreview { get; set; }
        public DateTime SentAt { get; set; }
    }

    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
    }

    public class TodoItem
    {
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
    }

}
