namespace IdentityText.Models
{
public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ClassGroupId { get; set; }
        public ClassGroup ClassGroup { get; set; }
    }

}
