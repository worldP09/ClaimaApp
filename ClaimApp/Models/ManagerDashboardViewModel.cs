using System.Collections.Generic;
using System.Security.Claims;
namespace ClaimApp.Models
{
    public class ManagerDashboardViewModel
    {
        public string ManagerName { get; set; }
        public int TotalClaims { get; set; }
        public int ApprovedClaims { get; set; }
        public int PendingClaims { get; set; }
        public int RejectedClaims { get; set; }
        public List<PendingClaimViewModel> PendingClaimsList { get; set; }
    }

    public class PendingClaimViewModel
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public string ModuleName { get; set; }
        public double HoursWorked { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
}
