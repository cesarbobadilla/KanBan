using MACRO.Common;
using MACRO.Entidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Repositorio
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IRepositorio, RepositorioEF<Entidadmodel>>();
            registerComponent.RegisterTypeWithControlledLifeTime<Entidadmodel>();
        }
    }
}
