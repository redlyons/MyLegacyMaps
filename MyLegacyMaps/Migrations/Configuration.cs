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
                new Map
                {
                    Name = "Motor Sports Raceways",
                    Description = "",
                    FileName = "mlm_map_raceways-adv4.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Dinosaurs of the World",
                    Description = "",
                    FileName = "dinosaurs.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Air France",
                    Description = "",
                    FileName = "air-france.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "California",
                    Description = "",
                    FileName = "california.png",
                    Orientation = 2,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Cheiftans",
                    Description = "",
                    FileName = "cheiftans.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Children of the World",
                    Description = "",
                    FileName = "children-world.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Colorado",
                    Description = "",
                    FileName = "colorado.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                 new Map
                {
                    Name = "Cheiftans",
                    Description = "",
                    FileName = "cheiftans.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Endangered Species",
                    Description = "",
                    FileName = "endangered-species.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Fishing",
                    Description = "",
                    FileName = "fishing.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Fishing Bitteroot",
                    Description = "",
                    FileName = "fishing-bitteroot.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Fishing Yakima",
                    Description = "",
                    FileName = "fishing-yakima.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Fishing Yellowstone",
                    Description = "",
                    FileName = "fishing-yellowstone.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }, 
                new Map
                {
                    Name = "Gibraltar",
                    Description = "",
                    FileName = "gibralter.png",
                    Orientation = 2,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Hawaii Scuba",
                    Description = "",
                    FileName = "hawaii-scuba.png",
                    Orientation = 2,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Horse Breeds",
                    Description = "",
                    FileName = "horse-breeds.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }, 
                new Map
                {
                    Name = "Kitsap County Adventure",
                    Description = "",
                    FileName = "mlm_kcoa_map.png",
                    Orientation = 2,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Kitsap County Memorials",
                    Description = "",
                    FileName = "mlm_rsvp_kc_memorial_map.png",
                    Orientation = 2,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "Oahu",
                    Description = "",
                    FileName = "oahu.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Washington",
                    Description = "",
                    FileName = "washington-adv.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Yosemite",
                    Description = "",
                    FileName = "yosemite.png",
                    Orientation = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   

            };
        #endregion

        #region Flags Seed Data
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
        #endregion

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
