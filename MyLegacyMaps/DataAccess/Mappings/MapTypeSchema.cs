﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Mappings
{
    public class MapTypeSchema : EntityTypeConfiguration<MapType>
    {
        public MapTypeSchema()
        {
            //PK
            HasKey(p => p.MapTypeId);

            Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired();

            HasMany(p => p.Maps)
                .WithOptional(p => p.MapType)
                .HasForeignKey(p => p.MapTypeId);

        }
    }
}