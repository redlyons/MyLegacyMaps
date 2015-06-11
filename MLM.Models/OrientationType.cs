using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class OrientationType
    {
        public int OrientationTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Map> Maps { get; set; }
    }
}
