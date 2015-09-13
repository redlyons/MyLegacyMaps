using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;
using MLM.Models;
using MLM.Persistence.Schemas;

namespace MLM.Persistence
{
    public class MyLegacyMapsContext : DbContext
    {
        public MyLegacyMapsContext()
            : base("DefaultConnection")
        {
        }

        //public static MyLegacyMapsContext Create()
        //{
        //    return new MyLegacyMapsContext();
        //}
          

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Configurations.Add(new MapSchema());
            modelBuilder.Configurations.Add(new MapTypeSchema());
            modelBuilder.Configurations.Add(new AdoptedMapSchema());
            modelBuilder.Configurations.Add(new FlagSchema());
            modelBuilder.Configurations.Add(new FlagTypeSchema());
            modelBuilder.Configurations.Add(new ShareStatusTypeSchema());
            modelBuilder.Configurations.Add(new OrientationTypeSchema());
            modelBuilder.Configurations.Add(new PartnerLogoSchema());
            modelBuilder.Configurations.Add(new AspectRatioSchema());
            modelBuilder.Configurations.Add(new PaymentSchema());

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
        public DbSet<OrientationType> OrientationTypes { get; set; }
        public DbSet<PartnerLogo> PartnerLogos { get; set; }
        public DbSet<AspectRatio> AspectRatios { get; set; }
        public DbSet<Payment> Payments { get; set; }
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
