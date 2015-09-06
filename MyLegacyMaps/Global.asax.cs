using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using MyLegacyMaps.App_Start;
using MLM.Logging;
using MLM.Persistence;

namespace MyLegacyMaps
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            //Database.SetInitializer<MLM.Persistence.MyLegacyMapsContext>(
            //  new  DropCreateDatabaseIfModelChanges<MLM.Persistence.MyLegacyMapsContext>()); 

           //Database.SetInitializer<MyLegacyMaps.MyLegacyMapsMembershipContext>(new DropCreateDatabaseAlways<MyLegacyMaps.MyLegacyMapsMembershipContext>());

            AreaRegistration.RegisterAllAreas();
            DependenciesConfig.RegisterDependencies();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PhotoService photoService = new PhotoService(new Logger());
            photoService.CreateAndConfigureAsync();
          
            DbConfiguration.SetConfiguration(new MLM.Persistence.EFConfiguration());

            //var configuration = new MyLegacyMaps.MembershipContextMigrations.Configuration();
            //var migrator = new DbMigrator(configuration);
            //migrator.Update();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}
