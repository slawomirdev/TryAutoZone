using Microsoft.AspNetCore.Identity;

namespace TryAutoZone.Models
{
    public class FavoriteCar
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual Car Car { get; set; }
    }
}
