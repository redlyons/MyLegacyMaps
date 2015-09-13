using System;
using System.Data.Entity.ModelConfiguration;
using MLM.Models;

namespace MLM.Persistence.Schemas
{
    public class PaymentSchema : EntityTypeConfiguration<Payment>
    {
        public PaymentSchema()
        {
            //PK
            HasKey(p => p.TransactionId);

            //FK
            Property(p => p.UserId)
                .IsRequired();

            Property(p => p.PayerFirstName)
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.PayerLastName)
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.PayerEmail)
                 .HasMaxLength(100)
                 .IsOptional();

            Property(p => p.GrossTotal)
                .IsRequired();

            Property(p => p.Currency)
                .HasMaxLength(15)
               .IsOptional();

            Property(p => p.Tokens)
               .IsRequired();

            Property(p => p.TransactionDate)
                .IsRequired();

            Property(p => p.TransactionStatus)
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.TransactionDetails)
              .HasMaxLength(1500)
              .IsOptional();
            
        }


    }
}