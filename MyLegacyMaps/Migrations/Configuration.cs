namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MyLegacyMaps.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyLegacyMaps.DataAccess.MyLegacyMapsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MyLegacyMaps.DataAccess.ApplicationDbContext";
        }

        protected override void Seed(MyLegacyMaps.DataAccess.MyLegacyMapsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Maps.AddOrUpdate(m => m.Name,
                new Map
                {
                    Name = "Skiing of North America",
                    Description = "An adventure map of North America's most popular ski destinations.",
                    FileName = "skiing-north-america.png",
                    Orientation = 1,
                    IsActive = true
                },
                new Map
                {
                    Name = "Baseball Stadiums",
                    Description = "Visit all the great baseball statidums in America and collect your memories as you go.",
                    FileName = "baseball.png",
                    Orientation = 1,
                    IsActive = true
                }
            );
        }
    }
}
