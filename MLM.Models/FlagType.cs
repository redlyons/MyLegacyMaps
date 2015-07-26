using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class FlagType
    {
        public int FlagTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Flag> Flags { get; set; }
    }
}