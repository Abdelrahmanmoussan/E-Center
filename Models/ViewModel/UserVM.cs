using IdentityText.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityText.Models.ViewModel
{
    public class UserVM
    {
        //[Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Display(Name = "Email")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [UniqueEmail]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public String? Photo { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        // student.
        [Required]
        [MaxLength(100)]
        public string ParentName { get; set; }

        [Required]
        [Phone]
        public string ParentPhone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        [UniqueEmail]
        public string ParentMail { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StudentDB { get; set; }
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
        [MaxLength(50)]

        public string EmergencyContact { get; set; }
        [Range(0, 100)]
        public decimal AttendancePercent { get; set; }
        [MaxLength(500)]
        public string StudentNotes { get; set; }
        //public int SubscriptionId { get; set; }
        public int AcademicYearId { get; set; }
        //public IEnumerable<SelectListItem> SubscriptionsList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> AcademicYearsList { get; set; } = Enumerable.Empty<SelectListItem>();


        //techer.
        public DateTime TeacherHireDate { get; set; }

        public DateTime? TeacherDB { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TeacherNetAmount { get; set; }

        public string TeacherNotes { get; set; }

        public int SubjectId { get; set; }
        public IEnumerable<SelectListItem> SubjectsList { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
