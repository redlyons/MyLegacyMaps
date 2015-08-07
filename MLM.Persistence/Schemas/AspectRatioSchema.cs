using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
{
    public class AspectRatioSchema : EntityTypeConfiguration<AspectRatio>
    {
        public AspectRatioSchema()
        {
            //PK
            HasKey(p => p.AspectRatioId);

            Property(p => p.Name)
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}
