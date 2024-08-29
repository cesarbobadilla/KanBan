using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Cryptography;
using SCOM.WebApi.Authorization.Interfaces;
using MACRO.Entidad.EntidadesDTO;
using System.Web;
using MACRO.Servicio;
using log4net;

namespace SCOM.WebApi.Authorization.Implementaciones
{
    public class AuthService : IAuthService
    {
        private readonly IServicioSeguridad servicio;
        protected ILog logger;

        public AuthService(IServicioSeguridad _servicio)
        {
            servicio = _servicio;
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = LogManager.GetLogger("MACRO");
            logger = Log;
        }

              
        public async Task<String> GenerateTokenAsync(Usuario usuario)
        {
            string tokenString = mxConstruirCadena(usuario);
            HttpCookie loCookie = new HttpCookie(MENSAJE_REPT.COOKIE_SGD, tokenString);
            loCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(loCookie);
            return tokenString;

            //HttpCookie loCookie = new HttpCookie(MENSAJE_REPT.COOKIE_USUARIO, mxConstruirCadena(usuario));
            //loCookie.Expires = DateTime.Now.AddYears(1);

            //HttpCookie loCookieHost = new HttpCookie(MENSAJE_REPT.COOKIE_HOST, lcIp);
            //loCookie.Expires = DateTime.Now.AddYears(1);

            //HttpContext.Current.Response.Cookies.Add(loCookie2);
            //HttpContext.Current.Response.Cookies.Add(loCookieHost);

        }

        public async Task<Usuario> ValidateTokenAsync(string TokenString)
        {
            //logger.InfoFormat("Validando token: {0}", TokenString);
            Boolean result = false;
            Usuario loUsuario = new Usuario();
            try
            {
                string[] laDatos = TokenString.Split('|');
               
                short lnCodUsuario = 0;
                int lnSesion = 0;
                short lnAplicacion = Convert.ToInt16(laDatos[3]);

                if (mxValidarCadena(TokenString))
                {
                    lnCodUsuario = short.Parse(laDatos[2]);
                    lnSesion = int.Parse(laDatos[1]);

                    //loUsuario = servicio.mxUsuarioTraerDatos(lnCodUsuario, lnSesion, lnAplicacion, laDatos[0], laDatos[5]);
                   // logger.InfoFormat("UsuarioLogeado:{0}, Dominio:{1}, password:{2}",loUsuario.cUsuario,loUsuario.cDominio, loUsuario.cPassword);
                    if (loUsuario != null)
                    {
                        return loUsuario;
                    }
                    else
                    {
                        loUsuario.cDominio = MENSAJE_REPT.USUARIO_NO_REGISTRADO;
                    }
                }
                else
                {
                    loUsuario.cDominio = MENSAJE_REPT.CADENA_INVALIDA;
                }                
            }
            catch (Exception ex)
            {
                loUsuario.cDominio = ex.Message;
            }

            return loUsuario;
        }

        public bool mxValidarCadena(string tcCadena)
        {
            bool llOk = true;

            try
            {

                string[] laDatos = tcCadena.Split('|');

                if (laDatos.Length != 4 && laDatos.Length != 5 && laDatos.Length != 6)
                {
                    llOk = false;
                }                
            }
            catch (Exception loError)
            {
                llOk = false;
            }

            return llOk;
        }

        public string mxConstruirCadena(Usuario toUsuario)
        {
            string lcCadena = String.Empty;

            lcCadena = toUsuario.cToken + "|" + toUsuario.nCodSesion + "|" + toUsuario.nCodUsuario + "|" + toUsuario.nAplicacion+ "|" + toUsuario.cDominio;

            return lcCadena;
        }
    }

    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialException()
        {
        }

        public InvalidCredentialException(string message)
            : base(message)
        {
        }
    }

    public class InvalidRSAKeyException : Exception
    {
        public InvalidRSAKeyException()
        {
        }

        public InvalidRSAKeyException(string message)
            : base(message)
        {
        }
    }
}