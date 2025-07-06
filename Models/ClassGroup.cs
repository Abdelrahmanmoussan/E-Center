using IdentityText.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityText.Models
{
    public class ClassGroup
    {
        [Key]
        public int ClassGroupId { get; set; }
        [Required]
        public required string Title { get; set; }
        [MaxLength(200)]
        public required string Location { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000$.")]
        public decimal Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public ClassGroupStatus CGStatus { get; set; } = ClassGroupStatus.NotPurchased;

        [Required]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }
        [Required]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public AcademicYear AcademicYear { get; set; }

        [BindNever]
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        [BindNever]
        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();

        [BindNever]
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }

}
