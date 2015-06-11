using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;


namespace MLM.Persistence.Schemas
{
    public class OrientationTypeSchema : EntityTypeConfiguration<OrientationType>
    {
        public OrientationTypeSchema()
        {
            //PK
            HasKey(p => p.OrientationTypeId);

            Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();

            HasMany(p => p.Maps)
                .WithRequired(p => p.OrientationType)
                .HasForeignKey(p => p.OrientationTypeId);
        }
    }
}
