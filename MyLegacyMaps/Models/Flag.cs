using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using MyLegacyMaps.Classes.Flags;


namespace MyLegacyMaps.Models
{
    public class Flag
    {
        public int FlagId { get; set; }
        public int FlagTypeId { get; set; }
        public int AdoptedMapId { get; set; } 
        [StringLength(100)]
        public string Name { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public string Description { get; set; }
        [StringLength(500)]
        public string VideoUrl { get; set; }
        [StringLength(500)]
        public string PhotosUrl { get; set; }
        public DateTime? Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }
        [StringLength(50)]
        public string Address2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(3)]
        public string State { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }

        public int? PartnerLogoId { get; set; }
        public virtual PartnerLogo PartnerLogo { get; set; }

        public string GetCssClass()
        {
            if(!Enum.IsDefined(typeof(FlagTypes), this.FlagTypeId))
            {
                return String.Empty;
            }
            
            switch((FlagTypes)this.FlagTypeId)
            {
                case FlagTypes.WasHere:
                    return "flgWasHere";
                case FlagTypes.HereNow:
                    return "flgHereNow";
                case FlagTypes.WantToGo:
                    return "flgPlanToGo";
                case FlagTypes.CustomLogo:
                    return "flgPartnerLogo";
                default:
                    return String.Empty;
            }
        }

        public string GetStyle()
        {
            if (!Enum.IsDefined(typeof(FlagTypes), this.FlagTypeId))
            {
                return String.Empty;
            }

            switch ((FlagTypes)this.FlagTypeId)
            {
                case FlagTypes.WasHere:                   
                case FlagTypes.HereNow:                   
                case FlagTypes.WantToGo:
                    return String.Format("top:{0}px; left:{1}px;", this.Ypos, this.Xpos);
                case FlagTypes.CustomLogo:
                    var backgroundUrl = String.Empty;
                    var ht = String.Empty;
                    var wd = String.Empty;
                    
                    if(this.PartnerLogo != null)
                    {
                        backgroundUrl = String.Format("background-image: url('{0}');", this.PartnerLogo.ImageUrl);
                        ht = this.PartnerLogo.Height.ToString();
                        wd = this.PartnerLogo.Width.ToString();                
                    }
                    var style = String.Format("top:{0}px; left:{1}px; height:{2}px; width:{3}px; {4}",
                        this.Ypos, this.Xpos, ht, wd, backgroundUrl);

                    return style;

                default:
                    return String.Empty;
            }
        }
    }
}