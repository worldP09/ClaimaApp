namespace ClaimApp.Models // Ensure this matches the namespace in your controller
{
    public class TrackClaimsViewModel
    {
        public List<TrackClaim> ClaimsList { get; set; } = new List<TrackClaim>();
    }

    public class TrackClaim
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public int HoursWorked { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }
    }
}
