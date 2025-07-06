using IdentityText.Enums;

namespace IdentityText.Models.ViewModel
{
    public class AttendanceEntry
    {
        public int StudentId { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
        public string Remark { get; set; }
    }
}
