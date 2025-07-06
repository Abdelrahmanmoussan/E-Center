using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityText.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required]
        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime TeacherHireDate { get; set; }

        public DateTime? TeacherDB { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal? TeacherNetAmount { get; set; }

        public string? TeacherNotes { get; set; }

        // ✅ هنا الإضافة الجديدة
        [Range(0, 5)]
        [Column(TypeName = "decimal(2,1)")]
        public decimal Rating { get; set; } = 0;  // القيمة الافتراضية صفر

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public Subject Subject { get; set; }
        [BindNever]
        public ICollection<AcademicYear> AcademicYears { get; set; }
        [BindNever]
        public ICollection<Payment> Payments { get; set; }
        [BindNever]
        public ICollection<PrivateLesson> PrivateLessons { get; set; }
        [BindNever]
        public ICollection<ClassGroup> ClassGroups { get; set; }
        [BindNever]
        public ICollection<TeacherAcademicYear> TeacherAcademicYears { get; set; }

        [NotMapped]
        public string FullName =>
    $"{ApplicationUser?.FirstName ?? ""} {ApplicationUser?.LastName ?? ""}".Trim();

    }

}


