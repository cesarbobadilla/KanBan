using log4net;
using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using MACRO.Servicio;
using SCOM.WebApi.Authorization;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MACRO.WebApi.Controllers
{
    [TokenAuthenticate]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MacroServicioController : ApiController
    {
        private readonly IServicio servicio;
        protected ILog logger;

        public MacroServicioController(IServicio _servicio)
        {                
                servicio = _servicio;
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = LogManager.GetLogger("MACRO");
            logger = Log;
        }

        [HttpPost]
        [Route("api/MacroServicio/getProcedure")]  // optiene data de un procedimiento simple         
        public HttpResponseMessage getProcedure(Request data)
        {          
            try
            {
                //using (Impersonator imp = new Impersonator(RequestContext.Principal.Identity.Name, "AQP", RequestContext.Principal.Identity.AuthenticationType))
                //{
                    RptContenido<Object> objRespuesta = new RptContenido<Object>();
                    try
                    {
                        Object objResultado = servicio.getProcedure("Car", data.procedure, data.parameters);// servicio.getMaestros();                    
                        objRespuesta.nRegistros = 0;
                        objRespuesta.oContenido = objResultado;
                        objRespuesta.nCodError = ERROR.SIN_ERROR;
                        objRespuesta.cMensaje = MENSAJE_REPT.OPERACION_EXITOSA;
                    }
                    catch (Exception ex)
                    {
                        objRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                        objRespuesta.cMsjError = ex.Message;
                    }

                    return this.Request.CreateResponse<RptContenido<Object>>(HttpStatusCode.OK, objRespuesta);
                    //return objRespuesta;
               // }
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized,ex.Message);                
            }           
        }
       
    }
}
