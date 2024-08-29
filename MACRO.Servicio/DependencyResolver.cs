using MACRO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Servicio
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IServicio, Servicios>();
            registerComponent.RegisterType<IServicioSeguridad, ServicioSeguridad>();
        }
    }
}
