using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityText.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }


        [MaxLength(100)]
        public string? ParentName { get; set; }


        [Phone]
        public string? ParentPhone { get; set; }


        [EmailAddress]
        public string? ParentMail { get; set; }


        [DataType(DataType.Date)]
        public DateTime? StudentDB { get; set; }
        [DataType(DataType.Date)]

        public DateTime EnrollmentDate { get; set; }

        [MaxLength(100)]
        public string? EmergencyContact { get; set; }

        [MaxLength(500)]
        public string? StudentNotes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BindNever]
        public ICollection<AssessmentResult> AssessmentResults { get; set; }
        [BindNever]
        public ICollection<Payment> payments { get; set; }
        [BindNever]
        public ICollection<PrivateLessonStudent> PrivateLessonStudents { get; set; }
        [BindNever]
        public ICollection<Enrollment> Enrollments { get; set; }


        [Required]
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }

        [NotMapped]
        public string FullName =>
            $"{ApplicationUser?.FirstName ?? ""} {ApplicationUser?.LastName ?? ""}".Trim();
        [NotMapped]
        public bool IsActive => AcademicYearId != 0;


    }


}
