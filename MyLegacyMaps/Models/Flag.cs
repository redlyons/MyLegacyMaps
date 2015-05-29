using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace MyLegacyMaps.Models
{
    public class Flag
    {
        public int FlagId { get; set; }
        public int FlagTypeId { get; set; }
        public int AdoptedMapId { get; set; }       
        public string Name { get; set; }
        public decimal Xpos { get; set; }
        public decimal Ypos { get; set; }
    }
}