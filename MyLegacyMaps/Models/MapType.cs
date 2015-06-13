using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class MapType
    {
        [Required]
        public int MapTypeId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }  
        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Map> Maps { get; set; }
    }
}