using IdentityText.Enums;

namespace IdentityText.Models.ViewModel
{
    public class SubscriptionVM
    {
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public string ClassGroupTitle { get; set; }
    }
}
