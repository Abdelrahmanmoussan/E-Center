using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Models
{
    public class AssessmentResult
    {
        [Key]
        public int AssessmentResultId { get; set; }

        [Required]
        public int AssessmentId { get; set; }
        [ForeignKey("AssessmentId")]
        public Assessment Assessment { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Required]
        public int Score { get; set; }
        public string Grade { get; set; }

        public string? Feedback { get; set; }
        public string? StudentSolutionPath { get; set; }
    }

}
