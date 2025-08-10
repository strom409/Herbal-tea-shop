using MvcHer.Models;

namespace MvcHer.Services
{
    public interface ITestimonialService
    {
        Task<List<Testimonial>> GetActiveTestimonialsAsync();
        Task<List<Testimonial>> GetFeaturedTestimonialsAsync();
        Task<List<Testimonial>> GetAllTestimonialsAsync();
        Task<Testimonial?> GetTestimonialByIdAsync(int id);
        Task<Testimonial> CreateTestimonialAsync(Testimonial testimonial);
        Task<Testimonial> UpdateTestimonialAsync(Testimonial testimonial);
        Task<bool> DeleteTestimonialAsync(int id);
        Task<bool> ToggleFeaturedAsync(int id);
        Task<bool> ToggleApprovalAsync(int id);
        Task<List<Testimonial>> GetPendingTestimonialsAsync();
        Task<int> GetTestimonialCountAsync();
    }
}
