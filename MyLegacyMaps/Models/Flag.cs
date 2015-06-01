using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using MyLegacyMaps.Classes;


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
            if(!Enum.IsDefined(typeof(enFlagTypes), this.FlagTypeId))
            {
                return String.Empty;
            }
            
            switch((enFlagTypes)this.FlagTypeId)
            {
                case enFlagTypes.WasHere:
                    return "flgWasHere";
                case enFlagTypes.HereNow:
                    return "flgHereNow";
                case enFlagTypes.WantToGo:
                    return "flgPlanToGo";
                case enFlagTypes.CustomLogo:
                    return "flgCustomLogo";
                default:
                    return String.Empty;
            }
        }

        public string GetStyle()
        {
            if (!Enum.IsDefined(typeof(enFlagTypes), this.FlagTypeId))
            {
                return String.Empty;
            }

            switch ((enFlagTypes)this.FlagTypeId)
            {
                case enFlagTypes.WasHere:                   
                case enFlagTypes.HereNow:                   
                case enFlagTypes.WantToGo:
                    return String.Format(" top:{0}px; left:{1}px;", this.Ypos, this.Xpos);
                case enFlagTypes.CustomLogo:
                    return String.Format(" top:{0}px; left:{1}px; height:75px; width:550px;", this.Ypos, this.Xpos);
                default:
                    return String.Empty;
            }
        }
    }
}