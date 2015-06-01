using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
{
    public class ShareStatusTypeSchema : EntityTypeConfiguration<ShareStatusType>
    {
        public ShareStatusTypeSchema()
        {
            //PK
            HasKey(p => p.ShareStatusTypeId);

            Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();

            HasMany(p => p.AdoptedMaps)
                .WithRequired(p => p.ShareStatusType)
                .HasForeignKey(p => p.ShareStatusTypeId);
        }
    }
}