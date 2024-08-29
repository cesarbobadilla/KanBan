using MACRO.Entidad.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Servicio
{
    public interface IServicioSeguridad
    {
        Usuario mxUsuarioTraerDatos(short tnCodUsuario, int tnSesion, short tnAplicacion, string tcToken, string tcFrase);
        Usuario mxLogearUsuario(UsuarioLogin user, string tcHost, string tcIP);
    }
}
