using MvcHer.Models;

namespace MvcHer.Services
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string phoneNumber);
        Task<bool> SendOtpAsync(string phoneNumber, string otpCode);
        bool ValidateOtp(string phoneNumber, string providedOtp);
        void ClearOtp(string phoneNumber);
    }
}
