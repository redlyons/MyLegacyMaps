using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
{
    public class MapSchema : EntityTypeConfiguration<Map>
    {
        public MapSchema()
        {
            //PK
            HasKey(p => p.MapId);

            //FK
            Property(p => p.OrientationTypeId)
                .IsRequired();

            HasRequired(p => p.OrientationType);

            Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();

            Property(p => p.Description)
                .HasMaxLength(500);

            Property(p => p.FileName)
                .HasMaxLength(100);

            Property(p => p.ImageUrl)
                .HasMaxLength(500);

            Property(p => p.ThumbUrl)
                .HasMaxLength(500);

            Property(p => p.IsActive)
                .IsRequired();

            Property(p => p.DateCreated)
                .IsRequired();

            Property(p => p.DateModified)
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasMaxLength(50)
                .IsRequired();

            //One to Many
            HasMany(p => p.AdoptedMaps)
                .WithRequired(p => p.Map)
                .HasForeignKey(p => p.MapId);

           //Many to Many
            HasMany(p => p.MapTypes)
               .WithMany(m => m.Maps)
               .Map(s =>
                   {
                       s.MapLeftKey("MapId");
                       s.MapRightKey("MapTypeId");
                   }
               );
        }


    }
}