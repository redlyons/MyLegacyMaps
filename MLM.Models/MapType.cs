using System;
using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class MapType
    {
        public int MapTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Map> Maps { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to MapType return false.
            var mt = obj as MapType;
            if ((System.Object)mt == null)
            {
                return false;
            }

            bool retVal =  this.MapTypeId == mt.MapTypeId;
            return retVal;
        }

        public override int GetHashCode()
        {
            return this.MapTypeId.GetHashCode();
        }
    }
}
