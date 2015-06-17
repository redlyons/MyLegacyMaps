﻿using System;
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

            //FK
            Property(p => p.ShareStatusTypeId)
                .IsRequired();
            
            Property(p => p.Name)
                .HasMaxLength(60)
                .IsRequired();

            HasMany(p => p.Flags)
                .WithRequired()
                .HasForeignKey(am => am.AdoptedMapId);

            Property(p => p.DateCreated)
                .IsRequired();

            Property(p => p.DateModified)
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasMaxLength(50)
                .IsRequired();

            HasRequired(p => p.Map);
            HasRequired(p => p.ShareStatusType);
           
          
        }
    }
}