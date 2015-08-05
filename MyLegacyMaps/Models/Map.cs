using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using MyLegacyMaps.Classes;

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
        }
        
        [Required]
        public int MapId { get; set; }      
        [Required(ErrorMessage="A Name is required")]
        [StringLength(60)]
        public string Name { get; set; }
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
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

        public string GetThumbImageUrl()
        {
            return String.IsNullOrWhiteSpace(this.ThumbUrl)
                ? GetMainImageUrl()
                : this.ThumbUrl;
        }

        /// <summary>
        /// Adventure Maps
        ///     Horizontal: 1000h x 1500w
        ///     Vertical:   1500h x 1000w
        /// Real Estate Maps
        ///     Horizontal: 1000h x 1250w
        ///     Vertical:   1250h x 1000w 
        /// </summary>
        /// <returns></returns>
        public string GetCanvasHeight()
        {
            if (OrientationTypeId == (int)Enums.OrientationType.Vertical)
            {
                return (IsRealEstateMap()) ? "1250px" : "1500px";
            }
            return "1000px";
        }

        public string GetCanvasWidth()
        {
            if (OrientationTypeId == (int)Enums.OrientationType.Vertical)
            {
                return "1000px";
            }

            return (IsRealEstateMap()) ? "1250px" : "1500px";
           
        }


        /// <summary>
        /// Adventure Maps
        ///     Horizontal: 210h x 315w 
        ///     Vertical:   315h x 210w
        /// Real Estate Maps
        ///     Horizontal: 210h x 315w   
        ///     Vertical:   315h x 210w 
        /// </summary>
        /// <returns></returns>
        public string GetThumbHeight()
        {
            if (OrientationTypeId == (int)Enums.OrientationType.Vertical)
            {
                return "315px";
            }
            return "210px";
        }

        public string GetThumbWidth()
        {
            if (OrientationTypeId == (int)Enums.OrientationType.Vertical)
            {
                return "210px";
            }
            return "315px";
        }

        public bool IsRealEstateMap()
        {
            return MapTypes.Contains(MapType.RealEstate);
        }
        
    }
}