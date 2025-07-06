using IdentityText.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IdentityText.Models.ViewModel
{
    public class ClassGroupVM
    {
        public int ClassGroupId { get; set; }
        [Required]
        public string Title { get; set; }

        [MaxLength(200)]
        [Required]
        public string Location { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000$.")]
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public ClassGroupStatus CGStatus { get; set; } = ClassGroupStatus.NotPurchased;

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int AcademicYearId { get; set; }
        public IEnumerable<SelectListItem> AcademicYearsList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> SubjectsList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> TeacherList { get; set; } = Enumerable.Empty<SelectListItem>();

    }

}
