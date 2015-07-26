using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
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

            Property(p => p.Description)
                .HasMaxLength(null)
                .IsOptional();

            Property(p => p.VideoUrl)
               .HasMaxLength(500)
               .IsOptional();

            Property(p => p.PhotosUrl)
             .HasMaxLength(500)
             .IsOptional();

            Property(p => p.DateCreated)
              .IsRequired();

            Property(p => p.DateModified)
                .IsRequired();

            Property(p => p.Address1)
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.Address2)
               .HasMaxLength(50)
               .IsOptional();

            Property(p => p.City)
             .HasMaxLength(50)
             .IsOptional();

            Property(p => p.State)
            .HasMaxLength(3)
            .IsOptional();

            Property(p => p.PostalCode)
             .HasMaxLength(10)
             .IsOptional();

            
        }

    }
}