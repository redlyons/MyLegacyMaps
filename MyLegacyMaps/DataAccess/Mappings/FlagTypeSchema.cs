using System;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
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