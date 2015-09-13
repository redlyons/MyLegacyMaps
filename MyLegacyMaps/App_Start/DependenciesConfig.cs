using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Util;
using Autofac.Integration.Mvc;
using MLM.Logging;
using MLM.Persistence;
using MLM.Persistence.Interfaces;
using MyLegacyMaps.Classes.Cookies;


namespace MyLegacyMaps.App_Start
{
    public class DependenciesConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            builder.RegisterType<CookieHelper>().As<ICookieHelper>();
            builder.RegisterType<MapsRepository>().As<IMapsRepository>();
            builder.RegisterType<AdoptedMapsRepository>().As<IAdoptedMapsRepository>();
            builder.RegisterType<FlagsRepository>().As<IFlagsRepository>();
            builder.RegisterType<PhotoService>().As<IPhotoService>().SingleInstance();
            builder.RegisterType<PartnerLogosRepository>().As<IPartnerLogosRepository>();
            builder.RegisterType<PaymentsRepository>().As<IPaymentsRepository>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}