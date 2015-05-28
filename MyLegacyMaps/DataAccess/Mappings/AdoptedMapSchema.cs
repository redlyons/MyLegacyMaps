using System;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
{
    public class AdoptedMapSchema : EntityTypeConfiguration<AdoptedMap>
    {
        public AdoptedMapSchema()
        {
            HasKey(p => p.AdoptedMapId);

            Property(p => p.UserId)
                .IsRequired();

            Property(p => p.MapId)
                .IsRequired();

            HasRequired(p => p.Map);

            Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();

            
           
           // HasKey(p => p.MapId);
           // HasKey(p => p.UserId);
        }
    }
}