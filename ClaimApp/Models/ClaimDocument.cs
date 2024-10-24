namespace ClaimApp.Models
{
    public class ClaimDocument
    {
        public int DocumentId { get; set; }
        public int ClaimId { get; set; } // Foreign key to Claims
        public string FileName { get; set; }
        public string FilePath { get; set; }

        // Navigation property
        public Claim Claim { get; set; }
    }
}
