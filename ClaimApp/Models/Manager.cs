namespace ClaimApp.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // For simplicity; hashed passwords are better
        // Collection of coordinators supervised by the manager
        public ICollection<Coordinator> Coordinators { get; set; } = new List<Coordinator>();

        // You can also add a collection for LecturerApplications if necessary
        public ICollection<LecturerApplication> Applications { get; set; } = new List<LecturerApplication>();
    }
}
