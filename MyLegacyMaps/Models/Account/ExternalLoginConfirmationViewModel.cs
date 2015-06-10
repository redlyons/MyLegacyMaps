using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLegacyMaps.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string HomeTown { get; set; }
        public System.DateTime? BirthDate { get; set; }
    }
}