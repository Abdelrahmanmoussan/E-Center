namespace IdentityText.Models.ViewModel
{
    public class PaymentVM
    {

        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string Notes { get; set; }
    }
}
