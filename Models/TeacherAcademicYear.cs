using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Models
{
    public class TeacherAcademicYear
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
    }

}
