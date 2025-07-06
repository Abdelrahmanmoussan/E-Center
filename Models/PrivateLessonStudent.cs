using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Models
{
    public class PrivateLessonStudent
    {
      
        public int PrivateLessonId { get; set; }
        public PrivateLesson PrivateLesson { get; set; }


        public int StudentId { get; set; }
        public Student Student { get; set; }
    }

}
