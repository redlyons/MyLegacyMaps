using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class FlagType
    {
        public int FlagTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Flag> Flags { get; set; }
    }
}