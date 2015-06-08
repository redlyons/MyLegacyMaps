using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Schemas
{
    public class FlagTypeSchema : EntityTypeConfiguration<FlagType>
    {
        public FlagTypeSchema()
        {
            //PK
            HasKey(p=>p.FlagTypeId);

            Property(p=>p.Name)
                .HasMaxLength(30)
                .IsRequired();

            Property(p => p.IsActive)
                .IsRequired();
                


        }
    }
}