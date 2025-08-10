using System.ComponentModel.DataAnnotations;

namespace MvcHer.Models
{
    public class SocialLink
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Platform { get; set; } = string.Empty; // Twitter, Facebook, Instagram, etc.

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty; // Display name for the link

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string Url { get; set; } = string.Empty;

        [StringLength(50)]
        public string? IconClass { get; set; } // CSS class for icon (e.g., fab fa-twitter)

        [StringLength(20)]
        public string? Color { get; set; } // Brand color for the platform

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; } = 0; // Order in which to display

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
