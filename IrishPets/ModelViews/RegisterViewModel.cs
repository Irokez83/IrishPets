using System;
using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    public class RegisterViewModel : ContactInfoViewModel2
    {
        public RegisterViewModel() : base()
        {
        }

        [Required, Display(Name = "E-mail"), EmailAddress]
        public string Email { get; set; }

        [StringLength(25, ErrorMessage = "The value {0} must contain at least {2} characters.", MinimumLength = 6)]
        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and its confirmation do not match.")]
        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }
        
        [Display(Name = "Date of Birth"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public override DateTime? DateOfBirth { get; set; }
    }
}