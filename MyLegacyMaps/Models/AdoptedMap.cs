using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLegacyMaps.Models
{
    public class AdoptedMap
    {
        public int AdoptedMapdId { get; set; }
        public string UserId { get; set; }
        public int MapId { get; set; }
        public string Name { get; set; }
        public int ShareStatus { get; set; }
    }
}