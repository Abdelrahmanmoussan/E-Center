    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace IdentityText.Models
    {
        public class AcademicYear
        {
            [Key]
            public int AcademicYearId { get; set; }

            [Required]
            [MaxLength(50)]
            public required string Name { get; set; }

            [BindNever]
            public ICollection<Student> Students { get; set; } = new List<Student>();

            public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
            [BindNever]
            public ICollection<TeacherAcademicYear> TeacherAcademicYears { get; set; } = new List<TeacherAcademicYear>();
            [BindNever]
            public ICollection<ClassGroup> ClassGroups { get; set; } = new List<ClassGroup>();
        }

    }
    