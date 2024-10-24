using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClaimApp.Hubs;

namespace ClaimApp.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string? Department { get; set; }
        public ICollection<Claim> Claims { get; set; }
        public ICollection<LecturerApplication> Applications { get; set; } = new List<LecturerApplication>();
    }
}
