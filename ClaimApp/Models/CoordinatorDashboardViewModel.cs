using System.Collections.Generic;

namespace ClaimApp.Models
{
    public class CoordinatorDashboardViewModel
    {
        public string CoordinatorName { get; set; }
        public List<Claim> Claims { get; set; }

        
        public int TotalClaims { get; set; }
        public int ApprovedClaims { get; set; }
        public int PendingClaims { get; set; }
        public int RejectedClaims { get; set; }
        public List<Claim> PendingClaimsList { get; set; }
    }
}
