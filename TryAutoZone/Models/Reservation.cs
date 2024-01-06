using Microsoft.AspNetCore.Identity;

namespace TryAutoZone.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime ReservationDate { get; set; }

        public virtual Car Car { get; set; }
        public virtual IdentityUser User { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
