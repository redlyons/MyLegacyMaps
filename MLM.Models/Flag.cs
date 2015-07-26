using System;
using System.Collections.Generic;
using System.Globalization;

namespace MLM.Models
{
    public class Flag
    {
        public int FlagId { get; set; }
        public int FlagTypeId { get; set; }
        public int AdoptedMapId { get; set; }
        public int? PartnerLogoId { get; set; }
        public string Name { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string PhotosUrl { get; set; }
        public DateTime? Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public virtual PartnerLogo PartnerLogo { get; set; }
    }
}
