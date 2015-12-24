using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    public class ContactUsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required correct e-mail"), Display(Name = "E-mail"), EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone number"), Phone]
        public string PhoneNumber { get; set; }
        public string Company { get; set; }

        [Required(ErrorMessage = "Required Subject"), StringLength(100, ErrorMessage = "The value {0} must contain at least {2} characters.", MinimumLength = 2)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Required your Comments"), StringLength(2000, ErrorMessage = "The value {0} must contain at least {2} characters.", MinimumLength = 10)]
        public string Comments { get; set; }
    }
}