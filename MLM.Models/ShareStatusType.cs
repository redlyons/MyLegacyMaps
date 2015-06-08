using System;
using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class ShareStatusType
    {
        public int ShareStatusTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AdoptedMap> AdoptedMaps { get; set; }
    }
}
