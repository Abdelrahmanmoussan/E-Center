namespace IdentityText.Models.ViewModel
{
    public class AssessmentDetailsVM
    {
        public int AssessmentId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? AssessmentLink { get; set; }
        public int MaxScore { get; set; }

        public int? Score { get; set; }
        public string? Grade { get; set; }
        public string? Feedback { get; set; }

        public string? StudentSolutionPath { get; set; }
    }
}
