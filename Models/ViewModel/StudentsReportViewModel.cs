namespace IdentityText.Models.ViewModel
{
    public class StudentReportItemViewModel
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public int CoursesCount { get; set; }
        public int TeachersCount { get; set; }
        public decimal TotalPaid { get; set; }
    }

    public class StudentsReportViewModel
    {
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public List<StudentReportItemViewModel> Students { get; set; }
    }

}
