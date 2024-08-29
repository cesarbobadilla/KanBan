using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MACRO.Common
{
    public class ComponentLoader
    {
        public static void LoadContainer(IUnityContainer container, string path, string pattern)
        {
            var dirCat = new DirectoryCatalog(path, pattern);
            var importDef = BuildImportDefinition();
            try
            {
                using (var aggregateCatalog = new AggregateCatalog())
                {
                    aggregateCatalog.Catalogs.Add(dirCat);

                    using (var componsitionContainer = new CompositionContainer(aggregateCatalog))
                    {
                        IEnumerable<Export> exports = componsitionContainer.GetExports(importDef);

                        IEnumerable<IComponent> modules =
                            exports.Select(export => export.Value as IComponent).Where(m => m != null);

                        var registerComponent = new RegisterComponent(container);
                        foreach (IComponent module in modules)
                        {
                            module.SetUp(registerComponent);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException typeLoadException)
            {
                var builder = new StringBuilder();
                foreach (Exception loaderException in typeLoadException.LoaderExceptions)
                {
                    builder.AppendFormat("{0}\n", loaderException.Message);
                }

                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }

        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(
                def => true, typeof(IComponent).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }
   
    internal class RegisterComponent : IRegisterComponent
    {
        private readonly IUnityContainer _container;

        public RegisterComponent(IUnityContainer container)
        {
            this._container = container;
        }

        public void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom
        {
            if (withInterception)
            {
                //register with interception
            }
            else
            {
                this._container.RegisterType<TFrom, TTo>();
            }
        }

        public void RegisterTypeWithControlledLifeTime<T>(bool withInterception = false)
        {
            this._container.RegisterType<T>(new HierarchicalLifetimeManager());
        }

        public void RegisterTypeWithControlledLifeTime<T>()
        {
            this._container.RegisterType<T>(new HierarchicalLifetimeManager());
        }

        public void RegisterInstance<T>(T instance)
        {
            this._container.RegisterInstance<T>(instance);
        }

        public void RegisterType<TFrom, TTo>(params InjectionMember[] members) where TTo : TFrom
        {
            this._container.RegisterType<TFrom, TTo>(members);
        }
    }
}
