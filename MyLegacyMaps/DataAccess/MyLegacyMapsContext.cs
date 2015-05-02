using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyLegacyMaps.Security;


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

        public System.Data.Entity.DbSet<MyLegacyMaps.Models.Map> Maps { get; set; }
    }
}