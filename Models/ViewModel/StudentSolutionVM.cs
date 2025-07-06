namespace IdentityText.Models.ViewModel
{
    public class StudentSolutionVM
    {
        public int AssessmentResultId { get; set; }
        public string StudentName { get; set; }
        public string SolutionLink { get; set; }
        public int? Score { get; set; }
        public string? Grade { get; set; }
        public string? Feedback { get; set; }
    }
}
