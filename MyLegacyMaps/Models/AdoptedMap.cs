using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class AdoptedMap
    {
       
        [Key]
        public int AdoptedMapId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int MapId { get; set; }
        public string Name { get; set; }
        public int ShareStatus { get; set; }

        //Not Mapped
        public string FileName { get; set; }
    }
}