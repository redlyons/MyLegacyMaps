﻿using System;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
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

            Property(p => p.Xpos)
                .HasPrecision(10, 2)
                .IsRequired();

            Property(p => p.Ypos)
                .HasPrecision(10, 2)
                .IsRequired();


        }

    }
}