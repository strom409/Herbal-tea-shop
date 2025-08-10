using System.ComponentModel.DataAnnotations;

namespace MvcHer.Models
{
    public class AboutUs
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Subtitle { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string FounderName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FounderTitle { get; set; } = string.Empty;

        [Required]
        public string FounderMessage { get; set; } = string.Empty;

        [StringLength(255)]
        public string? FounderImageUrl { get; set; }

        [Required]
        public string Mission { get; set; } = string.Empty;

        [Required]
        public string Vision { get; set; } = string.Empty;

        [Required]
        public string Values { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public int HappyCustomers { get; set; }

        public int TeaVarieties { get; set; }

        public int CountriesServed { get; set; }

        [StringLength(500)]
        public string? Awards { get; set; }

        [StringLength(500)]
        public string? Certifications { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
