using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class FlagType
    {
        [Key]
        public int FlagTypeId { get; set; }
        public string Name { get; set; }
    }
}