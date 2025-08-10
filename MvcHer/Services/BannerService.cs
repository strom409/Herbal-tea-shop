using MvcHer.Data;
using MvcHer.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcHer.Services
{
    public class BannerService : IBannerService
    {
        private readonly TeaShopDbContext _context;
        private readonly ILogger<BannerService> _logger;

        public BannerService(TeaShopDbContext context, ILogger<BannerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Banner>> GetAllBannersAsync()
        {
            try
            {
                return await _context.Banners
                    .OrderBy(b => b.DisplayOrder)
                    .ThenByDescending(b => b.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all banners: {ex.Message}");
                return new List<Banner>();
            }
        }

        public async Task<List<Banner>> GetActiveBannersAsync()
        {
            try
            {
                return await _context.Banners
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.DisplayOrder)
                    .ThenByDescending(b => b.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting active banners: {ex.Message}");
                return new List<Banner>();
            }
        }

        public async Task<Banner?> GetBannerByIdAsync(int id)
        {
            try
            {
                return await _context.Banners.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting banner by ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateBannerAsync(Banner banner)
        {
            try
            {
                banner.CreatedAt = DateTime.Now;
                _context.Banners.Add(banner);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Banner created successfully: {banner.Title}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating banner: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateBannerAsync(Banner banner)
        {
            try
            {
                var existingBanner = await _context.Banners.FindAsync(banner.Id);
                if (existingBanner == null)
                {
                    return false;
                }

                existingBanner.Title = banner.Title;
                existingBanner.Subtitle = banner.Subtitle;
                existingBanner.Description = banner.Description;
                existingBanner.ImageUrl = banner.ImageUrl;
                existingBanner.ButtonText = banner.ButtonText;
                existingBanner.ButtonUrl = banner.ButtonUrl;
                existingBanner.DisplayOrder = banner.DisplayOrder;
                existingBanner.IsActive = banner.IsActive;
                existingBanner.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Banner updated successfully: {banner.Title}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating banner: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            try
            {
                var banner = await _context.Banners.FindAsync(id);
                if (banner == null)
                {
                    return false;
                }

                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Banner deleted successfully: {banner.Title}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting banner: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ToggleBannerStatusAsync(int id)
        {
            try
            {
                var banner = await _context.Banners.FindAsync(id);
                if (banner == null)
                {
                    return false;
                }

                banner.IsActive = !banner.IsActive;
                banner.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Banner status toggled: {banner.Title} - Active: {banner.IsActive}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error toggling banner status: {ex.Message}");
                return false;
            }
        }
    }
}
