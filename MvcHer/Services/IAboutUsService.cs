using MvcHer.Models;

namespace MvcHer.Services
{
    public interface IAboutUsService
    {
        Task<AboutUs?> GetAboutUsAsync();
        Task<AboutUs?> GetAboutUsByIdAsync(int id);
        Task<AboutUs> CreateAboutUsAsync(AboutUs aboutUs);
        Task<AboutUs> UpdateAboutUsAsync(AboutUs aboutUs);
        Task<bool> DeleteAboutUsAsync(int id);
        Task<List<AboutUs>> GetAllAboutUsAsync();
    }
}
