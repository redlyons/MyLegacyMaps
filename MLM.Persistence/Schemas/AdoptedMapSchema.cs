using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
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

            HasRequired(p => p.Map);

            //FK
            Property(p => p.ShareStatusTypeId)
                .IsRequired();

            HasRequired(p => p.ShareStatusType);
            
            //Name
            Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();

            Property(p => p.Description)
                .HasMaxLength(3000)
                .IsOptional();


            Property(p => p.DateCreated)
                .IsRequired();

            Property(p => p.DateModified)
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasMaxLength(50)
                .IsRequired();

            //Flags: One to Many
            HasMany(p => p.Flags)
                .WithRequired()
                .HasForeignKey(am => am.AdoptedMapId);

           
            
           
          
        }
    }
}