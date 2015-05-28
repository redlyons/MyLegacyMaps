using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyLegacyMaps.Security;
using MyLegacyMaps.DataAccess.Mappings;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess
{
    public class MyLegacyMapsContext : IdentityDbContext<ApplicationUser>
    {
        public MyLegacyMapsContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static MyLegacyMapsContext Create()
        {
            return new MyLegacyMapsContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AdoptedMap>().Ignore(a => a.FileName);
           
            modelBuilder.Configurations.Add(new MapSchema());
            modelBuilder.Configurations.Add(new AdoptedMapSchema());
           
               
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
       
        public DbSet<Map> Maps { get; set; }
        public DbSet<AdoptedMap> AdoptedMaps { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<FlagType> FlagTypes { get; set; }
        public DbSet<MapType> MapTypes { get; set; }
    }
}