namespace IdentityText.Models.ViewModel
{
    public class AssessmentSolutionsVM
    {
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; }
        public List<StudentSolutionVM> StudentSolutions { get; set; }
    }
}
