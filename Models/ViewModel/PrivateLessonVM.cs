using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Models.ViewModel
{
    public class PrivateLessonVM
    {
        public int PrivateLessonId { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public IEnumerable<SelectListItem> TeacherList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> SubjectList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
