using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using System;
using System.Collections.Generic;

namespace MACRO.Servicio
{
    public interface IServicio
    {
        Object getProcedure(string appname, string procedure, List<Parameters> parametros);
    }
}
