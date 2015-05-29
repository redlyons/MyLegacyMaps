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
            HasKey(p => p.ShareStatustypeId);

            Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}