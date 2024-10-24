using System.Collections.Generic;

namespace ClaimApp.Models
{
    public class LecturerApplication
    {
        public int LecturerApplicationId { get; set; }
        public string Description { get; set; }
        public int LecturerId { get; set; }
        public Lecturer Lecturer { get; set; }
        public int CoordinatorId { get; set; }
        public Coordinator Coordinator { get; set; } // Add this navigation property
        public int ManagerId { get; set; }
        public Manager Manager { get; set; } // Navigation property to Manager
        public ICollection<ModuleEntry> Modules { get; set; } = new List<ModuleEntry>();
        public double TotalAmount { get; set; }
    }
}
