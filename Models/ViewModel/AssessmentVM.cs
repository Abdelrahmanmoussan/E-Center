using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Models.ViewModel
{
    public class AssessmentVM
    {
        public int AssessmentId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? AssessmentLink { get; set; }
        public int MaxScore { get; set; }
        public int LectureId { get; set; }
        public int ClassGroupId { get; set; }
        public IEnumerable<SelectListItem> LectureList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ClassGroupList { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
