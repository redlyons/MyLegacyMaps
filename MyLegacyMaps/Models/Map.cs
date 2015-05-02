using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLegacyMaps.Models
{
    public class Map
    {
        public int MapId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public int Orientation { get; set; }
        public bool IsActive { get; set; }
    }
}