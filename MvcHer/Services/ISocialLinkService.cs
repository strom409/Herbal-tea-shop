using MvcHer.Models;

namespace MvcHer.Services
{
    public interface ISocialLinkService
    {
        Task<IEnumerable<SocialLink>> GetAllSocialLinksAsync();
        Task<IEnumerable<SocialLink>> GetActiveSocialLinksAsync();
        Task<SocialLink?> GetSocialLinkByIdAsync(int id);
        Task<SocialLink> CreateSocialLinkAsync(SocialLink socialLink);
        Task<SocialLink> UpdateSocialLinkAsync(SocialLink socialLink);
        Task<bool> DeleteSocialLinkAsync(int id);
        Task<bool> ToggleActiveStatusAsync(int id);
        Task<bool> UpdateDisplayOrderAsync(int id, int newOrder);
    }
}
