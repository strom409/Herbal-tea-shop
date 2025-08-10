using Microsoft.EntityFrameworkCore;
using MvcHer.Data;
using MvcHer.Models;

namespace MvcHer.Services
{
    public class SocialLinkService : ISocialLinkService
    {
        private readonly TeaShopDbContext _context;
        private readonly ILogger<SocialLinkService> _logger;

        public SocialLinkService(TeaShopDbContext context, ILogger<SocialLinkService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<SocialLink>> GetAllSocialLinksAsync()
        {
            try
            {
                return await _context.SocialLinks
                    .OrderBy(s => s.DisplayOrder)
                    .ThenBy(s => s.Platform)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all social links");
                return new List<SocialLink>();
            }
        }

        public async Task<IEnumerable<SocialLink>> GetActiveSocialLinksAsync()
        {
            try
            {
                return await _context.SocialLinks
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.DisplayOrder)
                    .ThenBy(s => s.Platform)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active social links");
                return new List<SocialLink>();
            }
        }

        public async Task<SocialLink?> GetSocialLinkByIdAsync(int id)
        {
            try
            {
                return await _context.SocialLinks.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving social link with ID {Id}", id);
                return null;
            }
        }

        public async Task<SocialLink> CreateSocialLinkAsync(SocialLink socialLink)
        {
            try
            {
                socialLink.CreatedAt = DateTime.UtcNow;
                socialLink.UpdatedAt = DateTime.UtcNow;
                
                _context.SocialLinks.Add(socialLink);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Created social link for platform {Platform}", socialLink.Platform);
                return socialLink;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating social link for platform {Platform}", socialLink.Platform);
                throw;
            }
        }

        public async Task<SocialLink> UpdateSocialLinkAsync(SocialLink socialLink)
        {
            try
            {
                var existingLink = await _context.SocialLinks.FindAsync(socialLink.Id);
                if (existingLink == null)
                {
                    throw new ArgumentException($"Social link with ID {socialLink.Id} not found");
                }

                existingLink.Platform = socialLink.Platform;
                existingLink.DisplayName = socialLink.DisplayName;
                existingLink.Url = socialLink.Url;
                existingLink.IconClass = socialLink.IconClass;
                existingLink.Color = socialLink.Color;
                existingLink.IsActive = socialLink.IsActive;
                existingLink.DisplayOrder = socialLink.DisplayOrder;
                existingLink.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Updated social link for platform {Platform}", socialLink.Platform);
                return existingLink;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating social link with ID {Id}", socialLink.Id);
                throw;
            }
        }

        public async Task<bool> DeleteSocialLinkAsync(int id)
        {
            try
            {
                var socialLink = await _context.SocialLinks.FindAsync(id);
                if (socialLink == null)
                {
                    return false;
                }

                _context.SocialLinks.Remove(socialLink);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Deleted social link for platform {Platform}", socialLink.Platform);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting social link with ID {Id}", id);
                return false;
            }
        }

        public async Task<bool> ToggleActiveStatusAsync(int id)
        {
            try
            {
                var socialLink = await _context.SocialLinks.FindAsync(id);
                if (socialLink == null)
                {
                    return false;
                }

                socialLink.IsActive = !socialLink.IsActive;
                socialLink.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Toggled active status for social link {Platform} to {Status}", 
                    socialLink.Platform, socialLink.IsActive);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling active status for social link with ID {Id}", id);
                return false;
            }
        }

        public async Task<bool> UpdateDisplayOrderAsync(int id, int newOrder)
        {
            try
            {
                var socialLink = await _context.SocialLinks.FindAsync(id);
                if (socialLink == null)
                {
                    return false;
                }

                socialLink.DisplayOrder = newOrder;
                socialLink.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Updated display order for social link {Platform} to {Order}", 
                    socialLink.Platform, newOrder);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating display order for social link with ID {Id}", id);
                return false;
            }
        }
    }
}
