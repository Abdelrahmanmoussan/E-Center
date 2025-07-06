using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace IdentityText.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
    
      
        public string?  Photo { get; set; }

        //////////////
        ///
        ////////////
        [BindNever]
        public virtual ICollection<Notification> Notifications { get; set; }

    }
}
