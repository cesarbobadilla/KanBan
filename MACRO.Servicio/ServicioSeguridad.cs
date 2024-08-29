using log4net;
using MACRO.Entidad.EntidadesDTO;
using MACRO.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using MACRO.Entidad;

namespace MACRO.Servicio
{
    public class ServicioSeguridad: IServicioSeguridad
    {
        protected readonly IRepositorio repositorio;
        protected ILog logger;

        public ServicioSeguridad(IRepositorio _repositorio)
        {
            repositorio = _repositorio;            
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = LogManager.GetLogger("MACRO");
            logger = Log;
        }    

        public Usuario mxUsuarioTraerDatos(short tnCodUsuario, int tnSesion, short tnAplicacion, string tcToken, string tcFrase)
        {
            Usuario loUsuario = null;
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("tnCodUsuario", tnCodUsuario));
            parametros.Add(new SqlParameter("tnCodSesion", tnSesion));
            parametros.Add(new SqlParameter("tnAplicacion", tnAplicacion));
            parametros.Add(new SqlParameter("tcToken", tcToken));
            parametros.Add(new SqlParameter("tcFrase", tcFrase));
            //loUsuario = repositorio.ExecuteSP<Usuario>("Seguridad.Seguridad.prc_SeguridadAcceso_TraerDatosFrase", parametros).FirstOrDefault();
            loUsuario = repositorio.ExecuteSP<Usuario>("Car.dbo.test", parametros).FirstOrDefault();

            return loUsuario;
        }

        public Usuario mxLogeoUsuario(string tcUsuario, string tcPassword, short tnAplicacion, string tcHost, string tcIP)
        {
            Usuario loUsuario;
            IList<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("tcUsuario", tcUsuario));
            parametros.Add(new SqlParameter("tcPassword", tcPassword));
            parametros.Add(new SqlParameter("tnAplicacion", tnAplicacion));
            parametros.Add(new SqlParameter("tcHost", tcHost));
            parametros.Add(new SqlParameter("tcIP", tcIP));
            //CabeceraDetalleGenerico<Usuario, List<Acceso>> data = repositorio.GetMultipleResultSetFromSP <Usuario, Acceso>("Seguridad.Seguridad.prc_SeguridadAcceso_Logeofrase", parametros);
            CabeceraDetalleGenerico<Usuario, List<Acceso>> data = repositorio.GetMultipleResultSetFromSP <Usuario, Acceso>("Car.dbo.test", parametros);
            loUsuario = data.Cabecera;
            loUsuario.aAccesos = data.Detalle.ToArray();
            return loUsuario;
        }

        public Usuario mxLogearUsuario(UsuarioLogin user,string tcHost,string tcIP)
        {           
            Usuario loUsuario = null;
            String cNombreUsuario = mxEsAutentificadoAD(user.usuario, user.password);
            if (!String.IsNullOrEmpty(cNombreUsuario))
            {
                loUsuario = mxLogeoUsuario(user.usuario, user.password, user.idApp, tcHost, tcIP);

                if (loUsuario != null) {                    
                    loUsuario.cNombre = cNombreUsuario;
                    loUsuario.Ip = tcIP;
                }
                else throw new Exception(MENSAJE_REPT.USUARIO_NO_REGISTRADO);
            }            
            else
            {
                throw new Exception(MENSAJE_REPT.USUARIO_NO_ACTIVE_DIRECTORY);
            }
            return loUsuario;
        }

        public string mxEsAutentificadoAD(string tcUsuario, string tcPassword)
        {
            string cNombreUsuario = "";
            DirectoryContext loContextoDirectorio = null;
            DirectoryEntry loDirectorio = null;
            DirectorySearcher loBusqueda = null;
            SearchResult loResultado = null;

            try
            {
                loContextoDirectorio = new DirectoryContext(DirectoryContextType.Domain, MENSAJE_REPT.DOMINO, tcUsuario, tcPassword);
                loDirectorio = Domain.GetDomain(loContextoDirectorio).GetDirectoryEntry();
                loBusqueda = new DirectorySearcher(loDirectorio);

                loBusqueda.Filter = "(SAMAccountName=" + tcUsuario + ")";
                loBusqueda.PropertiesToLoad.Add("cn");
                loBusqueda.PropertiesToLoad.Add("givenName");   // first name
                loBusqueda.PropertiesToLoad.Add("sn");          // last name
                loResultado = loBusqueda.FindOne();

                if (loResultado != null)
                {
                    cNombreUsuario = loResultado.Properties["cn"][0].ToString();
                    if (loResultado.Properties["givenname"].Count > 0 && loResultado.Properties["sn"].Count > 0)
                        cNombreUsuario = loResultado.Properties["givenname"][0].ToString() + "  " + loResultado.Properties["sn"][0].ToString();
                    else if (cNombreUsuario.Split(' ').Length > 1) cNombreUsuario = cNombreUsuario.Split(' ')[0] + " " + cNombreUsuario.Replace(cNombreUsuario.Split(' ')[0], "");
                    else cNombreUsuario = cNombreUsuario.Trim() + "  - ";
                }
            }
            catch (Exception loError)
            {
                throw new Exception(MENSAJE_REPT.ACTIVE_DIRECTORY_ERROR_AUTENTIFICACION + loError.Message);
            }

            return cNombreUsuario;
        }


    }
}
