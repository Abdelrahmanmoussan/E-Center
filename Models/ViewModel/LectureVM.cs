using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Models.ViewModel
{
    public class LectureVM
    {
        public int LectureId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime LectureDate { get; set; }
        public string? VideoURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ClassGroupId { get; set; }
        public int? AssessmentId { get; set; }
        public IEnumerable<SelectListItem> AssessmentList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ClassGroupList { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
