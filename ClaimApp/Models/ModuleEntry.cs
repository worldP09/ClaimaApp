using System.ComponentModel.DataAnnotations;

namespace ClaimApp.Models
{
    public class ModuleEntry
    {
        [Key]
        public int ModuleEntryId { get; set; } // Ensure this property is defined only once

        public string ModuleName { get; set; } // Name of the module
        public int HoursWorked { get; set; } // Number of hours worked
        public double RatePerHour { get; set; } // Rate per hour
        public int LecturerApplicationId { get; set; } // Foreign key to LecturerApplication

        // Navigation property
        public LecturerApplication LecturerApplication { get; set; }

        // Calculated property to get total amount for this module entry
        public double TotalAmount => HoursWorked * RatePerHour;
    }
}
