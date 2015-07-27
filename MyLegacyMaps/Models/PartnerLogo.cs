using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class PartnerLogo
    {
        [Required]
        public int PartnerLogoId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }       
        [StringLength(500)]
        public string ImageUrl { get; set; }
       
    }
}