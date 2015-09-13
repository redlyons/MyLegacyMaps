using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLegacyMaps.Models.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "Email address")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [MaxLength(50)]
        [RegularExpression("([a-zA-Z0-9&#32;.&amp;amp;&amp;#39;-]+)", ErrorMessage = "Enter only alphabets and numbers for Display Name")]
        [Display(Name = "Display Name (what others see)")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password (minimum 6 characters)")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public int Credit { get; set; }
    }
}