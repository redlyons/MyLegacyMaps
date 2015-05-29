using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class MapType
    {
        public int MapTypeId { get; set; }
        public string Name { get; set; }        
        public bool IsActive { get; set; }

        public virtual ICollection<Map> Maps { get; set; }
    }
}