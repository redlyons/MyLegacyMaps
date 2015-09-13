using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MyLegacyMaps.Models
{
    public class AdoptedMap
    {        
        public AdoptedMap()
        {
            //prevent null ref exception
            Flags = new HashSet<Flag>();

            //Default to Private
            ShareStatusTypeId = 1;
            IsActive = true;
        }

        [Required]
        public int AdoptedMapId { get; set; } 
        [Required]
        public string UserId { get; set; }   
        [Required]
        public int MapId { get; set; }      
        [Required (ErrorMessage="Name is required")]
        [StringLength(60)]
        [RegularExpression("([a-zA-Z0-9\\s&#32;.&amp;amp;&amp;#39;-]+)", ErrorMessage = "Enter only alphabets and numbers for Name")]
        public string Name { get; set; }
        [RegularExpression("([a-zA-Z0-9\\s&#32;.&amp;amp;&amp;#39;-;]+)", ErrorMessage = "Enter only alphabets and numbers for Name")]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int ShareStatusTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Map Map { get; set; }
        public virtual ShareStatusType ShareStatusType { get; set; }
        public virtual ICollection<Flag> Flags { get; set; }
    }
}