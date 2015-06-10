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
        public string Name { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

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
                    return "flgCustomLogo";
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
                    return String.Format(" top:{0}px; left:{1}px;", this.Ypos, this.Xpos);
                case FlagTypes.CustomLogo:
                    return String.Format(" top:{0}px; left:{1}px; height:75px; width:550px;", this.Ypos, this.Xpos);
                default:
                    return String.Empty;
            }
        }
    }
}