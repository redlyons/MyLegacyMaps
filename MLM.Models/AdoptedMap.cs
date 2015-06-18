using System;
using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class AdoptedMap
    {
        public AdoptedMap()
        {
            //prevent null ref exception
            Flags = new HashSet<Flag>();
        }

        public int AdoptedMapId { get; set; }
        public string UserId { get; set; }
        public int MapId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int ShareStatusTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public virtual Map Map { get; set; }
        public virtual ShareStatusType ShareStatusType { get; set; }
        public virtual ICollection<Flag> Flags { get; set; }
    }
}
