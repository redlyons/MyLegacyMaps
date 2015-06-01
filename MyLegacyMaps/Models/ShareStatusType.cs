using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class ShareStatusType
    {
        public int ShareStatusTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AdoptedMap> AdoptedMaps { get; set; }
    }
}