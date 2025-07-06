    using IdentityText.Models;
    using System.Collections.Generic;

namespace IdentityText.ViewModel.View
{
    public class HomeViewModel
    {
        public List<Teacher> PopularTeachers { get; set; }
        public List<ClassGroup> PopularClassGroups { get; set; }
        public List<ClassGroup> Portfolio { get; set; }
    }

}
