using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using MACRO.Common;
using MACRO.Servicio;
using SCOM.WebApi.Authorization.Interfaces;
using SCOM.WebApi.Authorization.Implementaciones;
//using MACRO.WebApi.Authorization.Interfaces;
//using MACRO.WebApi.Authorization.Implementaciones;

namespace MACRO.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            ComponentLoader.LoadContainer(container, ".\\bin", "MACRO*.dll");
            container.RegisterType<IAuthService, AuthService>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}