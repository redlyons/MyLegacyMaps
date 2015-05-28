using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class AdoptedMap
    {        
        public int AdoptedMapId { get; set; }       
        public string UserId { get; set; }       
        public int MapId { get; set; }
        public virtual Map Map { get; set; }
        public string Name { get; set; }
        public int ShareStatus { get; set; }        
    }
}