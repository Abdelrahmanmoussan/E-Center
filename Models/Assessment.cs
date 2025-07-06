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
    public class Assessment
    {
        [Key]
        public int AssessmentId { get; set; }

        [Required]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Url]
        public string? AssessmentLink { get; set; }

        [Required]  
        public int MaxScore { get; set; }

        // ربط Assessment بمحاضرة فقط
        [Required]
        public int LectureId { get; set; }
        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; }


        [BindNever]
        public ICollection<AssessmentResult> AssessmentResults { get; set; } = new HashSet<AssessmentResult>();

    }

}
