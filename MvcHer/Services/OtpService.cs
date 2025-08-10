using MvcHer.Models;
using System.Collections.Concurrent;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;

namespace MvcHer.Services
{
    public class OtpService : IOtpService
    {
        private readonly ILogger<OtpService> _logger;
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, OtpData> _otpStorage = new();

        public OtpService(ILogger<OtpService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            
            // Initialize Twilio
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);
        }

        public async Task<string> GenerateOtpAsync(string phoneNumber, string email)
        {
            // Generate 6-digit OTP
            var random = new Random();
            var otpCode = random.Next(100000, 999999).ToString();
            
            // Store OTP with 5-minute expiry
            var otpData = new OtpData
            {
                OtpCode = otpCode,
                PhoneNumber = phoneNumber,
                Email = email,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            _otpStorage.AddOrUpdate(phoneNumber, otpData, (key, oldValue) => otpData);
            
            _logger.LogInformation($"OTP generated for {phoneNumber}: {otpCode}");
            
            return otpCode;
        }

        public async Task<bool> SendOtpAsync(string phoneNumber, string email, string otpCode, string fullName)
        {
            try
            {
                var twilioPhoneNumber = _configuration["Twilio:PhoneNumber"];
                
                // Format phone number for international format if needed
                var formattedPhoneNumber = phoneNumber;
                if (!phoneNumber.StartsWith("+"))
                {
                    // Assuming Indian number, add +91 prefix
                    formattedPhoneNumber = "+91" + phoneNumber.TrimStart('0');
                }

                // Create SMS message
                var messageBody = $"Hello {fullName}!\n\nYour TEA House OTP verification code is: {otpCode}\n\nThis code will expire in 5 minutes.\n\nDo not share this code with anyone.\n\n- TEA House Team";

                _logger.LogInformation($"Sending OTP SMS to {formattedPhoneNumber}");
                
                // Send SMS via Twilio
                var message = await MessageResource.CreateAsync(
                    body: messageBody,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(formattedPhoneNumber)
                );

                _logger.LogInformation($"SMS sent successfully! Message SID: {message.Sid}");
                _logger.LogInformation($"SMS Status: {message.Status}");
                
                // Also log for debugging
                _logger.LogInformation($"=== OTP SENT VIA TWILIO ===");
                _logger.LogInformation($"Name: {fullName}");
                _logger.LogInformation($"Phone: {formattedPhoneNumber}");
                _logger.LogInformation($"Email: {email}");
                _logger.LogInformation($"OTP Code: {otpCode}");
                _logger.LogInformation($"Message SID: {message.Sid}");
                _logger.LogInformation($"Valid for: 5 minutes");
                _logger.LogInformation($"==========================");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send OTP via Twilio: {ex.Message}");
                _logger.LogError($"Exception Details: {ex}");
                return false;
            }
        }

        public bool ValidateOtp(string phoneNumber, string providedOtp)
        {
            if (_otpStorage.TryGetValue(phoneNumber, out var otpData))
            {
                // Check if OTP is expired
                if (DateTime.Now > otpData.ExpiryTime)
                {
                    _otpStorage.TryRemove(phoneNumber, out _);
                    return false;
                }

                // Check if OTP matches and not already used
                if (otpData.OtpCode == providedOtp && !otpData.IsUsed)
                {
                    // Mark as used
                    otpData.IsUsed = true;
                    return true;
                }
            }

            return false;
        }

        public void ClearOtp(string phoneNumber)
        {
            _otpStorage.TryRemove(phoneNumber, out _);
        }

        private class OtpData
        {
            public string OtpCode { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public DateTime ExpiryTime { get; set; }
            public bool IsUsed { get; set; }
        }
    }
}
