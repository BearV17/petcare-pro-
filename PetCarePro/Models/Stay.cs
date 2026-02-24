namespace PetCarePro.Models
{
    public class Stay
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string Status { get; set; } = string.Empty; // Planned, Active, Completed, Cancelled
        public string KennelNumber { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Pet Pet { get; set; }
    }
}
