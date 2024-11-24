using Microsoft.AspNetCore.Identity;

namespace TryAutoZone.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }
}