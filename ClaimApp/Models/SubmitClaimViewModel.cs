
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ClaimApp.Models
{
    public class SubmitClaimViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>();

        public string Description { get; set; }

        // Adding an IFormFile for uploading supporting documents
        public IFormFile SupportingDocument { get; set; }
    }

    public class ModuleViewModel
    {
        [Required]
        public string ModuleName { get; set; }

        [Required]
        public int HoursWorked { get; set; }

        [Required]
        public decimal RatePerHour { get; set; }

        public decimal TotalAmount => HoursWorked * RatePerHour; // Auto-calculated total amount
    }
}

