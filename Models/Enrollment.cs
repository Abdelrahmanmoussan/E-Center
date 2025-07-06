

using IdentityText.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Required]
        public int ClassGroupId { get; set; }
        [ForeignKey("ClassGroupId")]
        public ClassGroup ClassGroup { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        public EnrollmentStatus EnrollmentStatus { get; set; } 

        public string? Notes { get; set; }
        [BindNever]
        public ICollection<Attendance> Attendances { get; set; }
        [BindNever]
        public ICollection<Subscription> Subscriptions { get; set; }
    }

}
