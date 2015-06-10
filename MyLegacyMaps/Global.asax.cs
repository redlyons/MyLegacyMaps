using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using MyLegacyMaps.App_Start;
using MLM.Logging;
using MLM.Persistence;

namespace MyLegacyMaps
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer<MyLegacyMaps.DataAccess.MyLegacyMapsMembershipContext>(
            //new DropCreateDatabaseIfModelChanges<MyLegacyMaps.DataAccess.MyLegacyMapsMembershipContext>());

            Database.SetInitializer<MLM.Persistence.MyLegacyMapsContext>(
              new DropCreateDatabaseIfModelChanges<MLM.Persistence.MyLegacyMapsContext>()); 
    

            AreaRegistration.RegisterAllAreas();
            DependenciesConfig.RegisterDependencies();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

          
            DbConfiguration.SetConfiguration(new MLM.Persistence.EFConfiguration());
        }
    }
}
