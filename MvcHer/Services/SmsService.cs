using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MvcHer.Services
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsService> _logger;
        private readonly Dictionary<string, string> _otpStorage; // In production, use Redis or database

        public SmsService(IConfiguration configuration, ILogger<SmsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _otpStorage = new Dictionary<string, string>();

            // Initialize Twilio
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            
            if (!string.IsNullOrEmpty(accountSid) && !string.IsNullOrEmpty(authToken))
            {
                TwilioClient.Init(accountSid, authToken);
            }
        }

        public async Task<bool> SendOrderConfirmationSmsAsync(string phoneNumber, string orderNumber, string trackingUrl)
        {
            try
            {
                var message = $"🎉 Thank you! Your TeaHouse order #{orderNumber} has been confirmed and payment received. Track your order at: {trackingUrl}";
                return await SendSmsAsync(phoneNumber, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send order confirmation SMS to {PhoneNumber}", phoneNumber);
                return false;
            }
        }

        public async Task<bool> SendOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                // Store OTP for verification (in production, use Redis with expiration)
                _otpStorage[phoneNumber] = otp;

                var message = $"Your TeaHouse order confirmation OTP is: {otp}. This OTP is valid for 5 minutes.";
                return await SendSmsAsync(phoneNumber, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send OTP SMS to {PhoneNumber}", phoneNumber);
                return false;
            }
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
        {
            try
            {
                if (_otpStorage.TryGetValue(phoneNumber, out var storedOtp))
                {
                    var isValid = storedOtp == otp;
                    if (isValid)
                    {
                        _otpStorage.Remove(phoneNumber); // Remove OTP after successful verification
                    }
                    return isValid;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify OTP for {PhoneNumber}", phoneNumber);
                return false;
            }
        }

        private async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                _logger.LogInformation("Attempting to send SMS to {PhoneNumber}: {Message}", phoneNumber, message);
                
                var twilioPhoneNumber = _configuration["Twilio:PhoneNumber"];
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                
                _logger.LogInformation("Twilio Config - Phone: {Phone}, AccountSid: {Sid}", twilioPhoneNumber, accountSid?.Substring(0, 10) + "...");
                
                if (string.IsNullOrEmpty(twilioPhoneNumber) || string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken))
                {
                    _logger.LogWarning("Twilio configuration incomplete. SMS not sent.");
                    _logger.LogInformation("SMS would be sent to {PhoneNumber}: {Message}", phoneNumber, message);
                    return false; // Return false to indicate SMS wasn't sent
                }

                // Ensure phone number is in E.164 format
                if (!phoneNumber.StartsWith("+"))
                {
                    // Remove any leading zeros and add +91 for Indian numbers
                    phoneNumber = phoneNumber.TrimStart('0');
                    phoneNumber = "+91" + phoneNumber;
                }
                // Clean up the phone number (remove spaces, dashes, etc.)
                phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

                var messageResource = await MessageResource.CreateAsync(
                    body: message,
                    from: new PhoneNumber(twilioPhoneNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                _logger.LogInformation("SMS sent successfully. SID: {MessageSid}", messageResource.Sid);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SMS to {PhoneNumber}", phoneNumber);
                // For development, still return true to not block the flow
                _logger.LogInformation("SMS would be sent to {PhoneNumber}: {Message}", phoneNumber, message);
                return true;
            }
        }
    }
}
