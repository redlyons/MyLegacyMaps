using System;
using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class Map
    {
        public Map()
        {
            //prevent null ref exception
            AdoptedMaps = new HashSet<AdoptedMap>();
            MapTypes = new HashSet<MapType>();
        }

        public int MapId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string FileName { get; set; }
        public int OrientationTypeId { get; set; }
  
       
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<AdoptedMap> AdoptedMaps { get; set; }
        public virtual OrientationType OrientationType { get; set; }
        public virtual ICollection<MapType> MapTypes { get; set; }
    }
}
