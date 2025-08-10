using MvcHer.Models;

namespace MvcHer.Services
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string phoneNumber, string email);
        Task<bool> SendOtpAsync(string phoneNumber, string email, string otpCode, string fullName);
        bool ValidateOtp(string phoneNumber, string providedOtp);
        void ClearOtp(string phoneNumber);
    }
}
