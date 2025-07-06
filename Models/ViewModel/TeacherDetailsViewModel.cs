using IdentityText.Models;

namespace IdentityText.Models.ViewModel
{
    internal class TeacherDetailsViewModel
    {
        public Teacher Teacher { get; set; }
        public List<ClassGroup> ClassGroups { get; set; }
    }
}