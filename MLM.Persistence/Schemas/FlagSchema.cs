using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Schemas
{
    public class FlagSchema  : EntityTypeConfiguration<Flag>
    {
        public FlagSchema()
        {
            //PK
            HasKey(p => p.FlagId);

            //FK
            Property(p => p.FlagTypeId)
                .IsRequired();

            //FK
            Property(p => p.AdoptedMapId)
                .IsRequired();

            Property(p => p.Name)
                .HasMaxLength(100)
                .IsOptional();

            
        }

    }
}