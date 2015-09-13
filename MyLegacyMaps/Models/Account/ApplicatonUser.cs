using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyLegacyMaps.Models.Account
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
          
        [Display(Name = "Display Name (what others see)")]
        [MaxLength(50)]
        [RegularExpression("([a-zA-Z0-9\\s&#32;.&amp;amp;&amp;#39;-]+)", ErrorMessage = "Enter only alphabets and numbers for Display Name")]
        public string DisplayName { get; set; }
       

        [Display(Name = "Email address")]
        [MaxLength(100)]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public override string Email { get; set; }

        [MaxLength(100)]
        public string EmailPrevious { get; set; }

        [MaxLength(500)]
        public string ProfileImageUrl { get; set; }

        public int Credits { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {            
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


}