using System.Collections.Generic;
using System.Linq;
using ClaimApp.Hubs;

namespace ClaimApp.Models
{
    public class LecturerClaim
    {
        public List<ModuleClaim> Modules { get; set; }
        public decimal TotalClaimAmount { get; set; }
    }

    public class ModuleClaim
    {
        public string ModuleName { get; set; }
        public int HoursWorked { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal TotalAmount => HoursWorked * RatePerHour;
    }
}
