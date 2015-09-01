using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyLegacyMaps.Models.Account;

namespace MyLegacyMaps
{
    public class MyLegacyMapsMembershipContext : IdentityDbContext<ApplicationUser>
    {
        public MyLegacyMapsMembershipContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static MyLegacyMapsMembershipContext Create()
        {
            return new MyLegacyMapsMembershipContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }

    
}