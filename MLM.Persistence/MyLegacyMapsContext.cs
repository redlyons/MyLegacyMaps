using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.ModelConfiguration.Conventions;
using MLM.Models;
using MLM.Schemas;

namespace MLM.Persistence
{
    public class MyLegacyMapsContext : DbContext
    {
        public MyLegacyMapsContext()
            : base("DefaultConnection")
        {
        }
          

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Configurations.Add(new MapSchema());
            modelBuilder.Configurations.Add(new MapTypeSchema());
            modelBuilder.Configurations.Add(new AdoptedMapSchema());
            modelBuilder.Configurations.Add(new FlagSchema());
            modelBuilder.Configurations.Add(new FlagTypeSchema());
            modelBuilder.Configurations.Add(new ShareStatusTypeSchema());

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Map> Maps { get; set; }
        public DbSet<AdoptedMap> AdoptedMaps { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<FlagType> FlagTypes { get; set; }
        public DbSet<MapType> MapTypes { get; set; }
        public DbSet<ShareStatusType> SharedStatusTypes { get; set; }
    }

    // EF follows a Code based Configration model and will look for a class that
    // derives from DbConfiguration for executing any Connection Resiliency strategies
    public class EFConfiguration : DbConfiguration
    {
        public EFConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}
