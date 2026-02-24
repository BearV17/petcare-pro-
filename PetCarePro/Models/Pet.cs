namespace PetCarePro.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ChipNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Owner Owner { get; set; }
    }
}
