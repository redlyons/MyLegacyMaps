using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLM.Models
{ 
    public class PartnerLogo
    {
        public int PartnerLogoId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
