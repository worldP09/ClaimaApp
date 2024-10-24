using System;
using System.ComponentModel.DataAnnotations;

namespace ClaimApp.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        public string ModuleName { get; set; }

        public decimal TotalAmount { get; set; }

        public int HoursWorked { get; set; }

        public DateTime DateSubmitted { get; set; }

        public string Status { get; set; }

        // Foreign Key for Lecturer
        public int LecturerId { get; set; }

        // Navigation Property for Lecturer
        public Lecturer Lecturer { get; set; }
        public string Description { get; set; }
        public string LecturerName { get; set; }
        public decimal RatePerHour { get; set; }
        


    }
}
