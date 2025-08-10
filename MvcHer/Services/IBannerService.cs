using MvcHer.Models;

namespace MvcHer.Services
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAllBannersAsync();
        Task<List<Banner>> GetActiveBannersAsync();
        Task<Banner?> GetBannerByIdAsync(int id);
        Task<bool> CreateBannerAsync(Banner banner);
        Task<bool> UpdateBannerAsync(Banner banner);
        Task<bool> DeleteBannerAsync(int id);
        Task<bool> ToggleBannerStatusAsync(int id);
    }
}
