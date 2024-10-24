namespace ClaimApp.Models
{
    public class Coordinator
    {
        public int CoordinatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // For simplicity; hashed passwords are better

        public int ManagerId { get; set; }
        public Manager Manager { get; set; } // Navigation property to Manager

        // Collection of applications assigned to the coordinator
        public ICollection<LecturerApplication> Applications { get; set; } = new List<LecturerApplication>();
    }
}
