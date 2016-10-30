using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Senparc.Weixin.MP.Sample.App_Code
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            //container.Register(null
            //          /*  Component.For<ILogService>()
            //                .ImplementedBy<LogService>()
            //                .LifeStyle.PerWebRequest,

            //            Component.For<IDatabaseFactory>()
            //                .ImplementedBy<DatabaseFactory>()
            //                .LifeStyle.PerWebRequest,
            //            Component.For<IUnitOfWork>()
            //                .ImplementedBy<UnitOfWork>()
            //                .LifeStyle.PerWebRequest*/,

            //            AllTypes.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient(),
            //            AllTypes.FromThisAssembly().BasedOn<IController>().LifestyleTransient(),
            //            AllTypes.FromAssemblyNamed("WebChat.Service")
            //                .Where(type => type.Name.EndsWith("Service")).WithServiceAllInterfaces().LifestylePerWebRequest(),
            //            AllTypes.FromAssemblyNamed("WebChat.Repository")
            //                .Where(type => type.Name.EndsWith("Repository")).WithServiceAllInterfaces().LifestylePerWebRequest()

            //            );


        }
    }
}