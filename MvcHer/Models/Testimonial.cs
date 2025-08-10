using System.ComponentModel.DataAnnotations;

namespace MvcHer.Models
{
    public class Testimonial
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ClientProfession { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string TestimonialText { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ClientImageUrl { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        [StringLength(100)]
        public string? ClientCompany { get; set; }

        [StringLength(50)]
        public string? ClientLocation { get; set; }

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public bool IsApproved { get; set; } = true;

        [StringLength(200)]
        public string? AdminNotes { get; set; }
    }
}
