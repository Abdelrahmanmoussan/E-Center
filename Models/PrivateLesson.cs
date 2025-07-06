using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityText.Models
{
    public class PrivateLesson
    {
        [Key]
        public int PrivateLessonId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        [Required]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }
        [BindNever]
        public ICollection<PrivateLessonStudent> PrivateLessonStudents { get; set; }



    }

}
