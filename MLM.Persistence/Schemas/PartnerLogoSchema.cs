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

            Property(p => p.Width)
                 .IsRequired();

            Property(p => p.Height)
                 .IsRequired();

            Property(p => p.ImageUrl)
               .HasMaxLength(500)
               .IsRequired();

            //One to Many
            HasMany(p => p.Flags)
                .WithRequired(p => p.PartnerLogo)
                .HasForeignKey(p => p.PartnerLogoId);
        }

    }
}