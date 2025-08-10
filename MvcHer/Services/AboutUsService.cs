using Microsoft.EntityFrameworkCore;
using MvcHer.Data;
using MvcHer.Models;

namespace MvcHer.Services
{
    public class AboutUsService : IAboutUsService
    {
        private readonly TeaShopDbContext _context;
        private readonly ILogger<AboutUsService> _logger;

        public AboutUsService(TeaShopDbContext context, ILogger<AboutUsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AboutUs?> GetAboutUsAsync()
        {
            try
            {
                return await _context.AboutUs
                    .Where(a => a.IsActive)
                    .OrderByDescending(a => a.UpdatedAt)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving AboutUs information");
                return null;
            }
        }

        public async Task<AboutUs?> GetAboutUsByIdAsync(int id)
        {
            try
            {
                return await _context.AboutUs
                    .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving AboutUs with ID {Id}", id);
                return null;
            }
        }

        public async Task<AboutUs> CreateAboutUsAsync(AboutUs aboutUs)
        {
            try
            {
                aboutUs.CreatedAt = DateTime.UtcNow;
                aboutUs.UpdatedAt = DateTime.UtcNow;
                aboutUs.IsActive = true;

                _context.AboutUs.Add(aboutUs);
                await _context.SaveChangesAsync();

                _logger.LogInformation("AboutUs created successfully with ID {Id}", aboutUs.Id);
                return aboutUs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating AboutUs");
                throw;
            }
        }

        public async Task<AboutUs> UpdateAboutUsAsync(AboutUs aboutUs)
        {
            try
            {
                var existingAboutUs = await _context.AboutUs.FindAsync(aboutUs.Id);
                if (existingAboutUs == null)
                {
                    throw new ArgumentException("AboutUs not found");
                }

                // Update properties
                existingAboutUs.Title = aboutUs.Title;
                existingAboutUs.Subtitle = aboutUs.Subtitle;
                existingAboutUs.Description = aboutUs.Description;
                existingAboutUs.ImageUrl = aboutUs.ImageUrl;
                existingAboutUs.FounderName = aboutUs.FounderName;
                existingAboutUs.FounderTitle = aboutUs.FounderTitle;
                existingAboutUs.FounderMessage = aboutUs.FounderMessage;
                existingAboutUs.FounderImageUrl = aboutUs.FounderImageUrl;
                existingAboutUs.Mission = aboutUs.Mission;
                existingAboutUs.Vision = aboutUs.Vision;
                existingAboutUs.Values = aboutUs.Values;
                existingAboutUs.YearsOfExperience = aboutUs.YearsOfExperience;
                existingAboutUs.HappyCustomers = aboutUs.HappyCustomers;
                existingAboutUs.TeaVarieties = aboutUs.TeaVarieties;
                existingAboutUs.CountriesServed = aboutUs.CountriesServed;
                existingAboutUs.Awards = aboutUs.Awards;
                existingAboutUs.Certifications = aboutUs.Certifications;
                existingAboutUs.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("AboutUs updated successfully with ID {Id}", aboutUs.Id);
                return existingAboutUs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating AboutUs with ID {Id}", aboutUs.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAboutUsAsync(int id)
        {
            try
            {
                var aboutUs = await _context.AboutUs.FindAsync(id);
                if (aboutUs == null)
                {
                    return false;
                }

                // Soft delete
                aboutUs.IsActive = false;
                aboutUs.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("AboutUs soft deleted with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting AboutUs with ID {Id}", id);
                return false;
            }
        }

        public async Task<List<AboutUs>> GetAllAboutUsAsync()
        {
            try
            {
                return await _context.AboutUs
                    .Where(a => a.IsActive)
                    .OrderByDescending(a => a.UpdatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all AboutUs records");
                return new List<AboutUs>();
            }
        }
    }
}
