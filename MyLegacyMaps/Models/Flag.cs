using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace MyLegacyMaps.Models
{
    public class Flag
    {
        [Key]
        public int FlagId { get; set; }
        public int FlagTypeId { get; set; }
        public int AdoptedMapId { get; set; }       
        public string Name { get; set; }
        public double Xpos { get; set; }
        public double Ypos { get; set; }
    }
}