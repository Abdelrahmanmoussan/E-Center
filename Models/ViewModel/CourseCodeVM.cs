using System.ComponentModel.DataAnnotations;

namespace IdentityText.Models.ViewModel
{
    public class CourseCodeVM
    {
        public int ClassGroupId { get; set; }
        [Required(ErrorMessage = "يرجى إدخال كود الكورس")]
        public string EnteredCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
