using System;
using System.Collections.Generic;


namespace MyLegacyMaps.Models
{
    public class MapType
    {
        public int MapTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Map> Maps { get; set; }
    }
}