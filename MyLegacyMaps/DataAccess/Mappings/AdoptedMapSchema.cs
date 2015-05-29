using System;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
{
    public class AdoptedMapSchema : EntityTypeConfiguration<AdoptedMap>
    {
        public AdoptedMapSchema()
        {
            //PK
            HasKey(p => p.AdoptedMapId);

            //FK
            Property(p => p.UserId)
                .IsRequired();

            //FK
            Property(p => p.MapId)
                .IsRequired();

            //FK
            Property(p => p.ShareStatusTypeId)
                .IsRequired();
            
            Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();

            HasMany(p => p.Flags)
                .WithRequired()
                .HasForeignKey(am => am.AdoptedMapId);

            HasRequired(p => p.Map);
           
          
        }
    }
}