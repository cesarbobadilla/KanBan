using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using MACRO.Servicio;
using SCOM.WebApi.Authorization.Interfaces;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MACRO.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    public class AutenticacionController : ApiController
    {
        private readonly IServicioSeguridad servicio;
        private readonly IAuthService authService;
        public AutenticacionController(IAuthService _authService, IServicioSeguridad _servicio)
        {
            authService = _authService;
            servicio = _servicio;
        }

        // POST: api/Autenticacion
        [HttpPost]
        [Route("api/Autenticacion/Login")]
        public RptContenido<Usuario> Login(UsuarioLogin user)
        {
            string lcHost = String.Empty;
            string lcIp = String.Empty;
            RptContenido<Usuario> loRespuesta = new RptContenido<Usuario>();
            try
            {
                lcHost = HttpContext.Current.Request.UserHostName;
                lcIp = HttpContext.Current.Request.UserHostAddress;
                user.usuario = user.usuario.Replace(@"AQP\", "").Trim();
                string Dominio = "AQP";
                using (Impersonator imp = new Impersonator(user.usuario, Dominio, user.password))
                {
                    Usuario loUsuario = servicio.mxLogearUsuario(user, lcHost, lcIp); // 55 nueva aplicacion interna
                    if (loUsuario != null)
                    {
                        loUsuario.nAplicacion = user.idApp;
                        List<short> codAccesos = new List<short>();
                        if (loUsuario.aAccesos != null && loUsuario.aAccesos.Length > 0)
                        {

                            foreach (var item in loUsuario.aAccesos)
                                codAccesos.Add(item.nCodAcceso);
                            // System.Web.HttpContext.Current.Session["Usuario"] = loUsuario;
                            authService.GenerateTokenAsync(loUsuario);
                            codAccesos.Sort();
                            // loUsuario.aAccesos = null;
                            // loUsuario.cPassword = string.Join(",", codAccesos);
                            loRespuesta.oContenido = loUsuario;
                        }
                        else
                        {
                            loRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                            loRespuesta.cMensaje = MENSAJE_REPT.MSJ_ERROR_NO_CONTROLADO;
                            loRespuesta.cMsjError = MENSAJE_REPT.LOGIN_SIN_ACCESOS;
                        }
                    }
                    else
                    {
                        loRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                        loRespuesta.cMensaje = MENSAJE_REPT.MSJ_ERROR_NO_CONTROLADO;
                        loRespuesta.cMsjError = MENSAJE_REPT.USUARIO_NO_REGISTRADO;
                    }
                }
            }
            catch (Exception ex)
            {
                // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                loRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                loRespuesta.cMensaje = MENSAJE_REPT.MSJ_ERROR_NO_CONTROLADO;
                loRespuesta.cMsjError = ex.Message;

            }
            return loRespuesta;
        }

        [HttpGet]
        public RptContenido<Usuario> Inicio(string usuario, string password, short idApp)
        {
            UsuarioLogin user = new UsuarioLogin();
            user.usuario = usuario;
            user.password = password;
            user.idApp = idApp;
            string lcHost = String.Empty;
            string lcIp = String.Empty;
            RptContenido<Usuario> loRespuesta = new RptContenido<Usuario>();
            try
            {
                lcHost = HttpContext.Current.Request.UserHostName;
                lcIp = HttpContext.Current.Request.UserHostAddress;
                user.usuario = user.usuario.Replace(@"AQP\", "").Trim();
                string Dominio = "AQP";
                //using (Impersonator imp = new Impersonator(user.usuario, Dominio, user.password)){
                    Usuario loUsuario = servicio.mxLogearUsuario(user, lcHost, lcIp); // 55 nueva aplicacion interna
                    if (loUsuario != null)
                    {
                        loUsuario.nAplicacion = user.idApp;
                        List<short> codAccesos = new List<short>();
                        if (loUsuario.aAccesos != null && loUsuario.aAccesos.Length > 0)
                        {

                            foreach (var item in loUsuario.aAccesos)
                                codAccesos.Add(item.nCodAcceso);
                            // System.Web.HttpContext.Current.Session["Usuario"] = loUsuario;
                            authService.GenerateTokenAsync(loUsuario);
                            codAccesos.Sort();
                            // loUsuario.aAccesos = null;
                            // loUsuario.cPassword = string.Join(",", codAccesos);
                            loRespuesta.oContenido = loUsuario;
                        }
                        else
                        {
                            loRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                            loRespuesta.cMensaje = MENSAJE_REPT.MSJ_ERROR_NO_CONTROLADO;
                            loRespuesta.cMsjError = MENSAJE_REPT.LOGIN_SIN_ACCESOS;
                        }
                    }
                    else
                    {
                        loRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                        loRespuesta.cMensaje = MENSAJE_REPT.MSJ_ERROR_NO_CONTROLADO;
                        loRespuesta.cMsjError = MENSAJE_REPT.USUARIO_NO_REGISTRADO;
                    }
              //  }
            }
            catch (Exception ex)
            {
                // Response.StatusCode = (int)HttpStatusCode.BadRequest;
                loRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                loRespuesta.cMensaje = MENSAJE_REPT.MSJ_ERROR_NO_CONTROLADO;
                loRespuesta.cMsjError = ex.Message;

            }
            return loRespuesta;
        }
    }
}
