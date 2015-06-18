using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class Map
    {
        public Map()
        {
            //prevent null ref exception
            AdoptedMaps = new HashSet<AdoptedMap>();
            MapTypes = new HashSet<MapType>();

            //default orientation
            OrientationTypeId = 1;
            OrientationType = new OrientationType
            {
                OrientationTypeId = 1,
                Name = "Horizontal"
            };
        }
        
        [Required]
        public int MapId { get; set; }      
        [Required(ErrorMessage="A Name is required")]
        [StringLength(60)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [StringLength(500)]
        public string ThumbUrl { get; set; }
        [StringLength(100)]
        public string FileName { get; set; }
        [Required]
        [Range(1,2)]
        public int OrientationTypeId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<AdoptedMap> AdoptedMaps { get; set; }
        public virtual OrientationType OrientationType { get; set; }
        public virtual ICollection<MapType> MapTypes { get; set; }

        public string GetMainImageUrl()
        {
            return String.IsNullOrWhiteSpace(this.ImageUrl)
                ? "/images/maps/" + this.FileName
                : this.ImageUrl;
        }
        
    }
}