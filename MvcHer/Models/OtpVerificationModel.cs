using System.ComponentModel.DataAnnotations;

namespace MvcHer.Models
{
    public class OtpVerificationModel
    {
        //[Required(ErrorMessage = "Full name is required")]
        //[Display(Name = "Full Name")]
        //public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Please enter a valid email address")]
        //[Display(Name = "Email Address")]
        //public string Email { get; set; } = string.Empty;
    }

    public class OtpValidationModel
    {
        [Required(ErrorMessage = "OTP is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        [Display(Name = "Enter OTP")]
        public string OtpCode { get; set; } = string.Empty;

       // public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
      //  public string Email { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }

    public class OtpSession
    {
     //   public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
   //     public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public bool IsVerified { get; set; }
    }
}
