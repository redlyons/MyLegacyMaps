using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
{
    public class MapTypeSchema : EntityTypeConfiguration<MapType>
    {
        public MapTypeSchema()
        {
            //PK
            HasKey(p => p.MapTypeId);

            Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();

            Property(p => p.IsActive)
                .IsRequired();
         

        }
    }
}