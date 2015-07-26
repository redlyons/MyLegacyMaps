using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
{
    public class PartnerLogoSchema : EntityTypeConfiguration<PartnerLogo>
    {
        public PartnerLogoSchema()
        {
            //PK
            HasKey(p => p.PartnerLogoId);                     

            Property(p => p.IsActive)
                 .IsRequired();

            Property(p => p.ImageUrl)
               .HasMaxLength(500)
               .IsRequired();
        }

    }
}