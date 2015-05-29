using System;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
{
    public class MapSchema : EntityTypeConfiguration<Map>
    {
        public MapSchema()
        {
            //PK
            HasKey(p => p.MapId);

            Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();

            HasMany(p => p.AdoptedMaps)
                .WithRequired(p => p.Map)
                .HasForeignKey(p => p.MapId);
                   
        }
    }
}