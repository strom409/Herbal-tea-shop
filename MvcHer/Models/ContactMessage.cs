using System.ComponentModel.DataAnnotations;

namespace MvcHer.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? Phone { get; set; }

        [StringLength(255, ErrorMessage = "Secondary email cannot exceed 255 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid secondary email address")]
        public string? SecondaryEmail { get; set; }

        [StringLength(20, ErrorMessage = "Secondary phone cannot exceed 20 characters")]
        public string? SecondaryPhone { get; set; }
        
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
        public string Subject { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Message { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsRead { get; set; } = false;
        
        public string? AdminResponse { get; set; }
        
        public DateTime? ResponseDate { get; set; }
    }
}
