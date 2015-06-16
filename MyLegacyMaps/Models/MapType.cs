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

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var mt = obj as MapType;
            if ((System.Object)mt == null)
            {
                return false;
            }

            return this.MapTypeId == mt.MapTypeId;
        }
    }
}