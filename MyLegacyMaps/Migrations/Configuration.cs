namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using MyLegacyMaps.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyLegacyMaps.DataAccess.MyLegacyMapsContext>
    {
        #region Maps Seed Data
        List<Map> _maps = new List<Map>
            {
                new Map
                {
                    Name = "Alaska",
                    Description = "The ultimate wilderness adventure.",
                    FileName = "alaska.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },
                new Map
                {
                    Name = "Baseball Stadiums",
                    Description = "Visit all the great baseball statidums in America and collect your memories as you go.",
                    FileName = "baseball.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },
                new Map
                {
                    Name = "Skiing of North America",
                    Description = "An adventure map of North America's most popular ski destinations.",
                    FileName = "skiing-north-america.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },       

            };
        #endregion

        List<Flag> _flags = new List<Flag>
        {
            new Flag
            {
                Name = "Was Here"
            },
            new Flag
            {
                Name = "Here Now"
            },
            new Flag
            {
                Name = "Want To Go Here"
            },
            new Flag
            {
                Name = "Custom Logo"
            },
        };

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MyLegacyMaps.DataAccess.ApplicationDbContext";
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
           
        //}

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

            _maps.ForEach(map => context.Maps.AddOrUpdate(m => m.Name, map));
            _flags.ForEach(flag => context.Flags.AddOrUpdate(f => f.Name, flag));
            context.SaveChanges();
        }
    }
}
