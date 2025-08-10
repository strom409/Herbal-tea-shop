using System.Threading.Tasks;

namespace MvcHer.Services
{
    public interface ISmsService
    {
        Task<bool> SendOrderConfirmationSmsAsync(string phoneNumber, string orderNumber, string trackingUrl);
        Task<bool> SendOtpAsync(string phoneNumber, string otp);
        Task<bool> VerifyOtpAsync(string phoneNumber, string otp);
    }
}
