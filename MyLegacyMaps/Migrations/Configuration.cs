using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyLegacyMaps.Models.Account;
using MLM.Models;

namespace MyLegacyMaps.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<MLM.Persistence.MyLegacyMapsContext>
    {
        #region Maps Seed Data
        List<Map> _maps = new List<Map>
            {
                new Map
                {
                    Name = "Alaska",
                    Description = "The ultimate wilderness adventure.",
                    FileName = "alaska.png",
                    MapTypeId = 2,
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
                    MapTypeId = 5,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },    
                new Map
                {
                    Name = "California",
                    Description = "",
                    FileName = "california.png",
                    MapTypeId = 2,
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
                    MapTypeId = 2,
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
                    MapTypeId =3,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Fishing Bitteroot",
                    Description = "",
                    FileName = "fishing-bitteroot.png",
                    Orientation = 1,
                    MapTypeId =3,
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
                    MapTypeId =3,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }, 
                new Map
                {
                    Name = "Gibraltar",
                    Description = "",
                    FileName = "gibralter.png",
                    Orientation = 2,
                     MapTypeId = 5,
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
                    MapTypeId = 2,
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
                    MapTypeId = 4,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Port Angeles",
                    Description = "",
                    FileName = "mlm_map_rem_wa_clallam_port-angeles_2000x1333.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },                 
                new Map
                {
                    Name = "Quimper Penisula",
                    Description = "",
                    FileName = "mlm_map_rem_wa_clallam_quimper-peninsula_1333x2000.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                new Map
                {
                    Name = "Sequim",
                    Description = "",
                    FileName = "mlm_map_rem_wa_clallam_sequim_2000x1333.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }, 
                new Map
                {
                    Name = "Downtown Seattle",
                    Description = "",
                    FileName = "mlm_map_rem_wa_downtown-seattle_2000x1333.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }, 
                 new Map
                {
                    Name = "Bellevue",
                    Description = "",
                    FileName = "mlm_rem_wa_king_belle-mi_map_1333x2000.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                 new Map
                {
                    Name = "Kirkland",
                    Description = "",
                    FileName = "mlm_rem_wa_king_kirk-red_map_1333x2000.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                 new Map
                {
                    Name = "Mercer Island",
                    Description = "",
                    FileName = "mlm_rem_wa_king_mercer-is_map_1333x2000.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                 new Map
                {
                    Name = "North Seattle",
                    Description = "",
                    FileName = "mlm_rem_wa_king_n-seattle_map_2000x1333.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   
                 new Map
                {
                    Name = "South Seattle",
                    Description = "",
                    FileName = "mlm_rem_wa_king_s-seattle_map_2000x1333.png",
                    Orientation = 1,
                    MapTypeId = 1,
                    IsActive = true,
                    DateCreated = DateTime.Now
                },   

            };
        #endregion

        #region Flag Types Seed Data
        List<FlagType> _flagTypes = new List<FlagType>
        {
            new FlagType
            {
                FlagTypeId = 1,
                Name = "Was Here",  
                IsActive = true
            },
            new FlagType
            {
                FlagTypeId = 2,
                Name = "Here Now",  
                IsActive = true
            },
            new FlagType
            {
                FlagTypeId = 3,
                Name = "Want To Go Here",  
                IsActive = true
            },
            new FlagType
            {
                FlagTypeId = 4,
                Name = "Custom Logo",  
                IsActive = true
            },
        };
        #endregion

        #region MapTypes
        List<MapType> _mapTypes = new List<MapType>
        {
            new MapType
            {
                MapTypeId = 1,
                Name = "Real Estate",  
                IsActive = true
            },
            new MapType
            {
                MapTypeId = 2,
                Name = "U.S. States",  
                IsActive = true
            },
            new MapType
            {
                MapTypeId = 3,
                Name = "Fishing",  
                IsActive = true
            },
            new MapType
            {
                MapTypeId = 4,
                Name = "U.S. National Parks",  
                IsActive = true
            },
            new MapType
            { 
                MapTypeId = 5,
                Name = "Europe",  
                IsActive = true            
            }
        };
        #endregion

        #region SharedStatusTypes
        List<ShareStatusType> _shareStatusTypes = new List<ShareStatusType>
        {
            new ShareStatusType
            {
                ShareStatusTypeId = 1,
                Name = "Private"
            },
            new ShareStatusType
            {
                ShareStatusTypeId = 2,
                Name = "Public"
            }
        };
        #endregion

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "MLM.Persistence.MyLegacyMapsContext";
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
           
        //}

        protected override void Seed(MLM.Persistence.MyLegacyMapsContext context)
        {
            //  This method will be called after migrating to the latest version.
            _maps.ForEach(map => context.Maps.AddOrUpdate(m => m.Name, map));
            _flagTypes.ForEach(type => context.FlagTypes.AddOrUpdate(t => t.Name, type));
            _mapTypes.ForEach(type => context.MapTypes.AddOrUpdate(t => t.Name, type));
            _shareStatusTypes.ForEach(type => context.SharedStatusTypes.AddOrUpdate(t => t.Name, type));
            context.SaveChanges();


            var membershipContext = MyLegacyMapsMembershipContext.Create();
            AddUserAndRole(membershipContext);
            membershipContext.SaveChanges();
        }


        bool AddUserAndRole(MyLegacyMapsMembershipContext context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("mapManager"));
            var um = new UserManager<MyLegacyMaps.Models.Account.ApplicationUser>(
                new UserStore<MyLegacyMaps.Models.Account.ApplicationUser>(context));
            var user = new MyLegacyMaps.Models.Account.ApplicationUser
            {
                UserName = "mlm_admin@mylegacymaps.com",
            };
            ir = um.Create(user, "F7jvPhl2AV2XWpvpFFfx");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "mapManager");
            return ir.Succeeded;
        }


       
    }
}
