using System.Security.Claims;

namespace ClaimApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Lecturer, Coordinator, Manager

        // Navigation property for Lecturer claims
        public ICollection<Claim> Claims { get; set; }
    }
}
