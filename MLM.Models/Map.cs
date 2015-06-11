﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class Map
    {
        public Map()
        {
            //prevent null ref exception
            AdoptedMaps = new HashSet<AdoptedMap>();
        }

        public int MapId { get; set; }
        public int? MapTypeId { get; set; }
        public virtual MapType MapType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public int Orientation { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<AdoptedMap> AdoptedMaps { get; set; }
    }
}