using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyLegacyMaps.MembershipContextMigrations
{
  

    internal sealed class Configuration : DbMigrationsConfiguration<MyLegacyMaps.MyLegacyMapsMembershipContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            MigrationsDirectory = @"MembershipContextMigrations";
            ContextKey = "MyLegacyMaps.MyLegacyMapsMembershipContext";
        }

        protected override void Seed(MyLegacyMaps.MyLegacyMapsMembershipContext context)
        {
            //  This method will be called after migrating to the latest version.
            AddDefaultUsers(context);
            context.SaveChanges();            
        }

        private bool AddDefaultUsers(MyLegacyMapsMembershipContext context)
        {
            try
            {
                IdentityResult ir;
                var rm = new RoleManager<IdentityRole>
                    (new RoleStore<IdentityRole>(context));

                if (!rm.RoleExists("mapManager"))
                {
                    ir = rm.Create(new IdentityRole("mapManager"));
                }                

                var um = new UserManager<MyLegacyMaps.Models.Account.ApplicationUser>(
                    new UserStore<MyLegacyMaps.Models.Account.ApplicationUser>(context));


                var admin1 = System.Configuration.ConfigurationManager.AppSettings["admin1"].Split(
                    new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);


                var user1 = new MyLegacyMaps.Models.Account.ApplicationUser
                {
                    UserName = admin1[0],
                    Email = admin1[0],
                    DisplayName = String.Empty,
                    Credits = 5,
                    EmailConfirmed = true,
                    DateCreated = System.DateTime.Now,
                    DateModified = System.DateTime.Now
                };
                ir = um.Create(user1, admin1[1]);
                if (ir.Succeeded)
                {
                    ir = um.AddToRole(user1.Id, "mapManager");                   
                }

                var admin2 = System.Configuration.ConfigurationManager.AppSettings["admin2"].Split(
                  new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                var user2 = new MyLegacyMaps.Models.Account.ApplicationUser
                {
                    UserName = admin2[0],
                    Email = admin2[0],
                    DisplayName = String.Empty,
                    Credits = 5,
                    EmailConfirmed = true,
                    DateCreated = System.DateTime.Now,
                    DateModified = System.DateTime.Now
                };
                ir = um.Create(user2, admin2[1]);
                if (ir.Succeeded)
                {
                    ir = um.AddToRole(user2.Id, "mapManager");                    
                }

                var admin3 = System.Configuration.ConfigurationManager.AppSettings["admin3"].Split(
                 new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                var user3 = new MyLegacyMaps.Models.Account.ApplicationUser
                {
                    UserName = admin3[0],
                    Email = admin3[0],
                    DisplayName = String.Empty,
                    Credits = 5,
                    EmailConfirmed = true,
                    DateCreated = System.DateTime.Now,
                    DateModified = System.DateTime.Now
                };
                ir = um.Create(user3, admin3[1]);
                if (ir.Succeeded)
                {
                    ir = um.AddToRole(user3.Id, "mapManager");
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
