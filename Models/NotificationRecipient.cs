using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Models
{
    public class NotificationRecipient
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationRecipientId { get; set; }
        [Required]
        public int NotificationId { get; set; }
        [ForeignKey("NotificationId")]
        public Notification Notification { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public bool? DeliveryByGmail { get; set; }
        public bool? IsDelivered { get; set; }
    }

}
