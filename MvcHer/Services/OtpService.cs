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
            
            try
            {
                // Initialize Twilio
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                var twilioPhoneNumber = _configuration["Twilio:PhoneNumber"];
                
                if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(twilioPhoneNumber))
                {
                    _logger.LogError("Twilio configuration is missing. Please check appsettings.json");
                }
                else
                {
                    TwilioClient.Init(accountSid, authToken);
                    _logger.LogInformation("Twilio client initialized successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Twilio client");
                throw;
            }
        }

        public async Task<string> GenerateOtpAsync(string phoneNumber)
        {
            // Generate 6-digit OTP
            var random = new Random();
            var otpCode = random.Next(100000, 999999).ToString();
            
            // Store OTP with 5-minute expiry
            var otpData = new OtpData
            {
                OtpCode = otpCode,
                PhoneNumber = phoneNumber,
               
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            _otpStorage.AddOrUpdate(phoneNumber, otpData, (key, oldValue) => otpData);
            
            _logger.LogInformation($"OTP generated for {phoneNumber}: {otpCode}");
            
            return otpCode;
        }

        public async Task<bool> SendOtpAsync(string phoneNumber, string otpCode)
        {
            try
            {
                var twilioPhoneNumber = _configuration["Twilio:PhoneNumber"];

                // Validate phone number
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    _logger.LogWarning("Phone number is empty.");
                    return false;
                }

                // Keep only digits
                var formattedPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

                // Add country code if missing
                if (!formattedPhoneNumber.StartsWith("+"))
                {
                    if (formattedPhoneNumber.StartsWith("0"))
                        formattedPhoneNumber = "+91" + formattedPhoneNumber.TrimStart('0');
                    else if (formattedPhoneNumber.Length == 10)
                        formattedPhoneNumber = "+91" + formattedPhoneNumber;
                    else if (formattedPhoneNumber.Length == 12 && formattedPhoneNumber.StartsWith("91"))
                        formattedPhoneNumber = "+" + formattedPhoneNumber;
                    else
                        formattedPhoneNumber = "+91" + formattedPhoneNumber;
                }

                // Send only the OTP digits
                var message = await MessageResource.CreateAsync(
                    body: otpCode,
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(formattedPhoneNumber)
                );

                _logger.LogInformation($"OTP sent to {formattedPhoneNumber}, SID: {message.Sid}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send OTP: {ex}");
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
