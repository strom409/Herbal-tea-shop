using Microsoft.EntityFrameworkCore;
using MvcHer.Data;
using MvcHer.Models;

namespace MvcHer.Services
{
    public class TestimonialService : ITestimonialService
    {
        private readonly TeaShopDbContext _context;
        private readonly ILogger<TestimonialService> _logger;

        public TestimonialService(TeaShopDbContext context, ILogger<TestimonialService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Testimonial>> GetActiveTestimonialsAsync()
        {
            try
            {
                // Temporarily show all testimonials to debug the issue
                var testimonials = await _context.Testimonials
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
                
                _logger.LogInformation("Found {Count} testimonials in database", testimonials.Count);
                
                // Log details of each testimonial for debugging
                foreach (var t in testimonials)
                {
                    _logger.LogInformation("Testimonial ID: {Id}, Name: {Name}, IsActive: {IsActive}, IsApproved: {IsApproved}", 
                        t.Id, t.ClientName, t.IsActive, t.IsApproved);
                }
                
                return testimonials;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active testimonials");
                return new List<Testimonial>();
            }
        }

        public async Task<List<Testimonial>> GetFeaturedTestimonialsAsync()
        {
            try
            {
                return await _context.Testimonials
                    .Where(t => t.IsActive && t.IsApproved && t.IsFeatured)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured testimonials");
                return new List<Testimonial>();
            }
        }

        public async Task<List<Testimonial>> GetAllTestimonialsAsync()
        {
            try
            {
                return await _context.Testimonials
                    .Where(t => t.IsActive) // Only show active testimonials in admin panel
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all testimonials");
                return new List<Testimonial>();
            }
        }

        public async Task<Testimonial?> GetTestimonialByIdAsync(int id)
        {
            try
            {
                return await _context.Testimonials
                    .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving testimonial with ID {Id}", id);
                return null;
            }
        }

        public async Task<Testimonial> CreateTestimonialAsync(Testimonial testimonial)
        {
            try
            {
                testimonial.CreatedAt = DateTime.UtcNow;
                testimonial.UpdatedAt = DateTime.UtcNow;
                testimonial.IsActive = true;

                _context.Testimonials.Add(testimonial);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial created successfully with ID {Id}", testimonial.Id);
                return testimonial;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating testimonial");
                throw;
            }
        }

        public async Task<Testimonial> UpdateTestimonialAsync(Testimonial testimonial)
        {
            try
            {
                var existingTestimonial = await _context.Testimonials.FindAsync(testimonial.Id);
                if (existingTestimonial == null)
                {
                    throw new ArgumentException("Testimonial not found");
                }

                // Update properties - only update if new value is provided
                existingTestimonial.ClientName = testimonial.ClientName;
                existingTestimonial.ClientProfession = testimonial.ClientProfession;
                existingTestimonial.TestimonialText = testimonial.TestimonialText;
                
                // Only update image URL if it's provided (not null or empty)
                if (!string.IsNullOrEmpty(testimonial.ClientImageUrl))
                {
                    existingTestimonial.ClientImageUrl = testimonial.ClientImageUrl;
                }
                
                existingTestimonial.Rating = testimonial.Rating;
                existingTestimonial.ClientCompany = testimonial.ClientCompany;
                existingTestimonial.ClientLocation = testimonial.ClientLocation;
                existingTestimonial.IsFeatured = testimonial.IsFeatured;
                existingTestimonial.IsApproved = testimonial.IsApproved;
                existingTestimonial.AdminNotes = testimonial.AdminNotes;
                existingTestimonial.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial updated successfully with ID {Id}. Image URL: {ImageUrl}", 
                    testimonial.Id, existingTestimonial.ClientImageUrl);
                return existingTestimonial;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating testimonial with ID {Id}", testimonial.Id);
                throw;
            }
        }

        public async Task<bool> DeleteTestimonialAsync(int id)
        {
            try
            {
                var testimonial = await _context.Testimonials.FindAsync(id);
                if (testimonial == null)
                {
                    return false;
                }

                // Soft delete
                testimonial.IsActive = false;
                testimonial.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial soft deleted with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting testimonial with ID {Id}", id);
                return false;
            }
        }

        public async Task<bool> ToggleFeaturedAsync(int id)
        {
            try
            {
                var testimonial = await _context.Testimonials.FindAsync(id);
                if (testimonial == null)
                {
                    return false;
                }

                testimonial.IsFeatured = !testimonial.IsFeatured;
                testimonial.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial featured status toggled for ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling featured status for testimonial with ID {Id}", id);
                return false;
            }
        }

        public async Task<bool> ToggleApprovalAsync(int id)
        {
            try
            {
                var testimonial = await _context.Testimonials.FindAsync(id);
                if (testimonial == null)
                {
                    return false;
                }

                testimonial.IsApproved = !testimonial.IsApproved;
                testimonial.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Testimonial approval status toggled for ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling approval status for testimonial with ID {Id}", id);
                return false;
            }
        }

        public async Task<List<Testimonial>> GetPendingTestimonialsAsync()
        {
            try
            {
                return await _context.Testimonials
                    .Where(t => t.IsActive && !t.IsApproved)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending testimonials");
                return new List<Testimonial>();
            }
        }

        public async Task<int> GetTestimonialCountAsync()
        {
            try
            {
                return await _context.Testimonials
                    .Where(t => t.IsActive && t.IsApproved)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving testimonial count");
                return 0;
            }
        }
    }
}
