using System.ComponentModel.DataAnnotations;

namespace MvcHer.Models
{
    public class Banner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subtitle is required")]
        [StringLength(200, ErrorMessage = "Subtitle cannot exceed 200 characters")]
        public string Subtitle { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Button text cannot exceed 200 characters")]
        public string? ButtonText { get; set; }

        [StringLength(500, ErrorMessage = "Button URL cannot exceed 500 characters")]
        public string? ButtonUrl { get; set; }

        [Range(1, 100, ErrorMessage = "Display order must be between 1 and 100")]
        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
