using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityText.Models.ViewModel
{
    public class StudentAttendanceVM
    {
        public int ClassGroupId { get; set; }
        public int LectureId { get; set; }

        public List<SelectListItem> ClassGroups { get; set; }
        public List<SelectListItem> Lectures { get; set; }
        public List<SelectListItem> AttendanceStatusList { get; set; }

        public List<StudentVM> Students { get; set; }
        public List<AttendanceEntry> Attendances { get; set; }
    }

}

