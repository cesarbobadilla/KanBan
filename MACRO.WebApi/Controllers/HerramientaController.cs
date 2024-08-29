using log4net;
using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using MACRO.Servicio;
using MACRO.WebApi.Util;
using SCOM.WebApi.Authorization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Web;
using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using MACRO.Servicio;
using log4net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web;
using System.Linq;

namespace MACRO.WebApi.Controllers
{
    [TokenAuthenticate]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HerramientaController : ApiController
    {
        private readonly IServicio servicio;
        protected ILog logger;

        public HerramientaController(IServicio _servicio)
        {
            servicio = _servicio;
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = LogManager.GetLogger("HERRAMIENTA");
            logger = Log;
        }

        public string GetAppNAme()
        {
            string authorization = null;
            if (Request.Headers.Authorization != null)
                authorization = Request.Headers.Authorization.Parameter;
            if (authorization == null)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[MENSAJE_REPT.COOKIE_SGD];
                if (authCookie != null)
                    authorization = authCookie.Value;
                else if (HttpContext.Current.Request.Headers[MENSAJE_REPT.COOKIE_SGD] != null)
                    authorization = HttpContext.Current.Request.Headers[MENSAJE_REPT.COOKIE_SGD];
            }
            if (authorization == null || authorization.Split('|').Length == 4) return "AppMacroServicio";
            else return authorization.Split('|')[4];
        }

        [HttpGet]
        [Route("api/Herramienta/PrintZebraPrueba")]
        public bool PrintPrueba(String IpPrinter)
        {
            String URL_File = AppDomain.CurrentDomain.BaseDirectory + @"assets\ticket.prn";
            return RawPrinterHelper.SendFileToPrinter(IpPrinter, URL_File,1);
        }


        [HttpPost]
        [Route("api/Herramienta/PrintZebra")]
        public HttpResponseMessage PrintEtiqueta(Request data)
        {
            try
            {
                using (Impersonator imp = new Impersonator(RequestContext.Principal.Identity.Name, "AQP", RequestContext.Principal.Identity.AuthenticationType))
                {
                    RptContenido<IEnumerable<Object>> objRespuesta = new RptContenido<IEnumerable<Object>>();
                    try
                    {
                        /*CollectionResponse<Object> objResultado = servicio.getProcedure(GetAppNAme(), data.procedure, data.parameters);// servicio.getMaestros();                    
                       

                        Guid guid = Guid.NewGuid();
                        string str = guid.ToString();
                        int Copies = 1;
                        bool resp = true;
                        if (data.parameters.Count > 1) {
                            string IpPrinter = data.parameters.First(item => item.Parameter.ToLower() == "printname").Value;
                            if (IpPrinter.Length > 1) {
                                if (data.parameters.Any(item => item.Parameter.ToLower() == "copias")) {
                                    Copies = int.Parse(data.parameters.First(item => item.Parameter.ToLower() == "copias").Value);
                                }                                
                                String URL_File = AppDomain.CurrentDomain.BaseDirectory + @"assets\" + str + ".prn";
                                var byName = (IDictionary<string, object>)objResultado.Data.First();
                                String CadenaImpr = (String)byName["CadenaImpresion"];

                                using (StreamWriter writer = File.CreateText(URL_File))
                                {
                                    foreach (string line in CadenaImpr.Split('@'))
                                    {
                                        writer.WriteLine(line);
                                    }
                                }                               
                                resp = RawPrinterHelper.SendFileToPrinter(IpPrinter, URL_File, Copies);
                                File.Delete(Path.Combine(URL_File));
                            }
                        }                                                                        
                        
                        if (resp)
                        {
                            objRespuesta.nRegistros = objResultado.Count;
                            objRespuesta.oContenido = objResultado.Data;
                            objRespuesta.nCodError = ERROR.SIN_ERROR;
                            objRespuesta.cMensaje = MENSAJE_REPT.OPERACION_EXITOSA;
                        }
                        else {
                            objRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                            objRespuesta.cMsjError = "Error al Imprimir";
                        }*/

                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("error:{0}", ex.Message);
                        objRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                        objRespuesta.cMsjError = ex.Message;
                    }
                    return this.Request.CreateResponse<RptContenido<IEnumerable<Object>>>(HttpStatusCode.OK, objRespuesta);
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }


          
        }

        // GET: api/Herramienta/5
        [HttpGet]
        [Route("api/Herramienta/Imprimirdoc")]
        public bool Imprimirdoc(string cEtiqueta) // string ipPrinter,short copies,string Etiqueta, String FechaReg, String FechaVenc
        {
            string ipPrinter = "Zebra-generic";
            short copies = 1;

           

            String URL_File = AppDomain.CurrentDomain.BaseDirectory + @"assets\etiqueta.prn";
            //Check if the file exists
            if (!File.Exists(URL_File))
            {
                //File.WriteAllText(URL_File, PrintTEXT);
                // Create the file and use streamWriter to write text to it.
                //If the file existence is not check, this will overwrite said file.
                //Use the using block so the file can close and vairable disposed correctly
                using (StreamWriter writer = File.CreateText(URL_File))
                {
                    writer.WriteLine(@"CT~~CD,~CC^~CT~");
                    writer.WriteLine(@"^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^PR6,6~SD18^JUS^LRN^CI0^XZ");
                    writer.WriteLine(@"~DG000.GRF,02304,024,");
                    writer.WriteLine(@",::::::::::::::::::S0QFBFA0,R01FPFBFHF,O03FE3FPFBFIFE,N01FFE7FVFE0,N0IFE3FPFBFKF8,M01FIF7FPFBFLF,M0JFE3FPFBFLFC0,L01FIFE7FPFBFLFE0,L03FIFE3FPFBFMF0,L0LF7FPFBFMFC,K01FJFE3FPFBFMFC,K01FC0H0E7FPFBFMF0,K07E02A003FPFBFLFE380,K0703FF007FPF3FLF8780,K083FHFE03FPF3FLF1F80,L0KF83FPF3FKFC1F01F0,K03FJFE3FOFE3EBFIF87E0FHF,K07FKF1FOFC0H01FHF0F81FHFC0,K0MF8FOFC0FE87FE1F83FHFE0,J01FLFC7FNF87FFE1F87E07FIF8,J03FLFE3FNF8FIF8F0FE0FJF0,J03FMF1FMFE1FIFC61FC1FIFC0,J03FMF8AMA03FIFE07FC3FHFE,J03FMFC0N07FJF0FFC7FHF8,J03FMFE7FLF8FKF8FFC7FFE020,I0H7NFE7FLF9FKFCFFC7FF87F0,I0H7NFE3FLF9FKFCFFE3FE0FF8,I0E7FMFE7FLF9FKFC7FF0703FF8,H01E7FNF3FLFBFKFE7FF803FHF8,H01F7FNF3FLF3FLF7FHF0FIF0,H03E7FNF3FLFBFLF7FKF0,H03E7FNF3FLF3FLF3FJFE0,H03E7FNFBFLFBFLF3FKF0,H03E7FNF3FLF3FLF3FKF0,H03EFOFBFLFBFLFBFKFC,H03E7FNF3FLF3FLF3FKFC,H03E7FNF3FLFBFLFBFKFE,H03E7FNF3FLFBFLF1FLF,H03E7FNF3FLFBFLFBFLF80,H01E3FNF7FLFBFLF3FLFE0,H01E3FMFE7FLFBFLFBFMF0,H01F3FMFE7FLF9FLF3FMFC,I0E3FMFCFMFDFLF3FNF80,I0F1FMF8FMFDFLF7FNF80,I070FMF8FMFEFKFE7FNF80,I0707FKFE1FMFE7FJFC7FNF80,I0387FKFE3FMFE7FJFCFOF80,J081FKFC7FNF3FJFCFOF80,L0LF87FNFBFJF1FOF80,L07FJF0FOF9FJF3FOF80,L03FIFE1FOF9FIFE3FKFEFFE,L03FIF83FOF8FIFC3FKFE1FC,L03FHFE07FOFCFIF93FKFE020,L03FHF847FOFCFIF7CFLF0,L07FHF0C3FOFCFHFE7CFJFE80,L07FFC3C0FOFC7FFCFE01FC,L0IF9FE02FMFE8FHFBFF,L0IF3F80H017FHFD0H07FF3FF,L0HFCFF0R0HFE7FE,L0HF1FE0R0HFC7FC,L0HF1FE0R0HF87FC,L0HF1FF0Q01FF07FC,L0HF0FF80P03FE03FE,L07F07FC0P07FC03FF,L0HF07FE0P0HF803FF,L07F01FF0P0HF800FF,L07F80FF80O0HFI0HF80,L07FC07FC0O07F0H07F80,L03FC03FE0O07F8003F80,L01FE01FF0O07F8001FC0,M07F007F80N03F80H0FE0,M03F003F80N03F80H0FE0,M03E0S03FC0H0HF0,gI03FC0I010,gI03EE,,:^XA");
                    writer.WriteLine(@"^MMT");
                    writer.WriteLine(@"^PW831");
                    writer.WriteLine(@"^LL2233");
                    writer.WriteLine(@"^LS0");
                    writer.WriteLine(@"^FT530,425^XG000.GRF,1,1^FS");
                    writer.WriteLine(@"^FT126,162^A0N,86,79^FH\^FD111.11^FS");
                    writer.WriteLine(@"^BY3,3,110^FT80,450^BCN,,Y,N");
                    writer.WriteLine(@"^FD>;01010000010101>77774^FS");
                    writer.WriteLine(@"^FT710,401^A0N,34,33^FH\^FDOGN^FS");
                    writer.WriteLine(@"^FT536,333^A0N,34,33^FH\^FDAREQUIPA-PER\E9^FS");
                    writer.WriteLine(@"^FT445,225^A0N,34,33^FH\^FDEXT:^FS");
                    writer.WriteLine(@"^FT32,232^A0N,34,33^FH\^FDUND:^FS");
                    writer.WriteLine(@"^FT458,169^A0N,34,33^FH\^FDFV:^FS");
                    writer.WriteLine(@"^FT530,460^A0N,28,16^FH\^FDparametroplantilla^FS");
                    writer.WriteLine(@"^FT34,280^A0N,28,20^FH\^FDRegistro:^FS");
                    writer.WriteLine(@"^FT126,235^A0N,68,50^FH\^FDUUU^FS");
                    writer.WriteLine(@"^FT522,228^A0N,39,38^FH\^FD12345-66^FS");
                    writer.WriteLine(@"^FT521,172^A0N,39,38^FH\^FD66-66-66^FS");
                    writer.WriteLine(@"^FT455,111^A0N,34,33^FH\^FDLT:^FS");
                    writer.WriteLine(@"^FT126,280^A0N,25,25^FH\^FD99/99/999900:00am^FS");
                    writer.WriteLine(@"^FT36,330^A0N,25,21^FH\^FDCod.A.S.000045-MINAGRI-SENASA-AREQUIPA^FS");
                    writer.WriteLine(@"^FT521,113^A0N,39,38^FH\^FDLL-LL-LL^FS");
                    writer.WriteLine(@"^FT31,125^A0N,28,31^FH\^FDPeso^FS");
                    writer.WriteLine(@"^FT31,155^A0N,28,31^FH\^FDNeto^FS");
                    writer.WriteLine(@"^FT95,140^A0N,28,31^FH\^FD:^FS");
                    writer.WriteLine(@"^FT31,61^A0N,31,28^FH\^FDparametrotipoproductoxxxxxxxxxxxxxxxxxxxx^FS");
                    writer.WriteLine(@"^PQ1,0,1,Y^XZ");
                    writer.WriteLine(@"^XA^ID000.GRF^FS^XZ");
                }
            }
            
            //RawPrinterHelper obj = new RawPrinterHelper();             return obj.PrintText(URL_File, ipPrinter, 1);
            bool rep = RawPrinterHelper.SendFileToPrinter(ipPrinter, URL_File,1);
            File.Delete(Path.Combine(URL_File));                       
            return rep;
            // RawPrinterHelper.SendStringToPrinter(ipPrinter, PrintTEXT);

            //ASCIIEncoding enc = new ASCIIEncoding();
            //byte[] bytes = enc.GetBytes(Texto);

            //Print Printer = new Print();
            //PrintService Printer2 = new PrintService();
            //Printer2.PrintData(bytes, ipPrinter);



            //return true;//Printer.PrintData(bytes, ipPrinter, copies, (int)SizePrint.Zebra);            
        }

        //[HttpGet]
        //[Route("api/Herramienta/FirtsPrint")]
        //public bool FirtsPrint() {
        //    return RawPrinterHelper.SendFileToPrinter(@"10.82.2.180", @"\\sistemas-37\temporal\ticket.prn");

        //    //string strCmdText = "start /min notepad /pt " ZplBuilder.Append(@"'C:\Users\roger.calla\Documents\Flutter\salidacerdocong\assets\ticket.prn' "+"'10.82.2.180'";
        //    //string strCmdText = "start notepad";
        //    //PrintService Printer2 = new PrintService();
        //    //Printer2.printFile(strCmdText);
        //    //printer2.printButton();
        //    //Shell("cmd.exe /c " & sCommand, AppWinStyle.Hide)
        //    //NetworkCredential networkCredential = new NetworkCredential("roger.calla", "arlenne5*","AQP");
        //    //System.Diagnostics.Process.Start("CMD.exe", strCmdText, networkCredential.UserName, networkCredential.SecurePassword, networkCredential.Domain);
        //}       

        [HttpPost]
        [Route("api/Herramienta/GetReporte")]
        public HttpResponseMessage GetReporte(RequestReport data)
        {
            try
            {
                using (Impersonator imp = new Impersonator(RequestContext.Principal.Identity.Name, "AQP", RequestContext.Principal.Identity.AuthenticationType))
                {
                    RptContenido<Object> objRespuesta = new RptContenido<Object>();
                    try
                    {
                        //Object objResultado = Reportes.Reportes.ObtenerReporte(data.ruta, data.parameters);                   
                        objRespuesta.nRegistros = 0;
                        //objRespuesta.oContenido = objResultado;
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
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
        }


        [HttpPost]
        [Route("api/Herramienta/ImprimirEtiquetas")]
        public HttpResponseMessage ImprimirEtiquetas(Request data)
        {
            try
            {
                using (Impersonator imp = new Impersonator(RequestContext.Principal.Identity.Name, "AQP", RequestContext.Principal.Identity.AuthenticationType))
                {
                    RptContenido<IEnumerable<Object>> objRespuesta = new RptContenido<IEnumerable<Object>>();
                    try
                    {
                        /*CollectionResponse<Object> objResultado = servicio.getProcedure(GetAppNAme(), data.procedure, data.parameters);

                        for (int i = 0; i < objResultado.Data.Count(); i++)
                        {
                            Guid guid = Guid.NewGuid();
                            string str = guid.ToString();
                            String URL_File = AppDomain.CurrentDomain.BaseDirectory + @"assets\" + str + ".prn";
                            string IpPrinter = data.parameters.First(item => item.Parameter.ToLower() == "@printname").Value;
                            var byName = (IDictionary<string, object>)objResultado.Data.ElementAt(i);
                            String CadenaImpr = (String)byName["CadenaImpresion"];

                            using (StreamWriter writer = File.CreateText(URL_File))
                            {
                                foreach (string line in CadenaImpr.Split('@'))
                                {
                                    writer.WriteLine(line);
                                }
                            }

                            bool resp = RawPrinterHelper.SendFileToPrinter(IpPrinter, URL_File,1);
                            File.Delete(Path.Combine(URL_File));
                            if (resp)
                            {
                                objRespuesta.nRegistros = objResultado.Count;
                                objRespuesta.oContenido = objResultado.Data;
                                objRespuesta.nCodError = ERROR.SIN_ERROR;
                                objRespuesta.cMensaje = MENSAJE_REPT.OPERACION_EXITOSA;
                            }
                            else
                            {
                                objRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                                objRespuesta.cMsjError = "Error al Imprimir";
                            }
                        }
                        */

                        //Guid guid = Guid.NewGuid();
                        //string str = guid.ToString();
                        //String URL_File = AppDomain.CurrentDomain.BaseDirectory + @"assets\" + str + ".prn";
                        //string IpPrinter = data.parameters.First(item => item.Parameter.ToLower() == "printname").Value;
                        //var byName = (IDictionary<string, object>)objResultado.Data.First();
                        //String CadenaImpr = (String)byName["CadenaImpresion"];

                        //using (StreamWriter writer = File.CreateText(URL_File))
                        //{
                        //    foreach (string line in CadenaImpr.Split('@'))
                        //    {
                        //        writer.WriteLine(line);
                        //    }
                        //}

                        //bool resp = RawPrinterHelper.SendFileToPrinter(IpPrinter, URL_File);
                        //File.Delete(Path.Combine(URL_File));
                        //if (resp)
                        //{
                        //    objRespuesta.nRegistros = objResultado.Count;
                        //    objRespuesta.oContenido = objResultado.Data;
                        //    objRespuesta.nCodError = ERROR.SIN_ERROR;
                        //    objRespuesta.cMensaje = MENSAJE_REPT.OPERACION_EXITOSA;
                        //}
                        //else
                        //{
                        //    objRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                        //    objRespuesta.cMsjError = "Error al Imprimir";
                        //}

                    }
                    catch (Exception ex)
                    {
                        logger.ErrorFormat("error:{0}", ex.Message);
                        objRespuesta.nCodError = ERROR.ERROR_NO_CONTROLADO;
                        objRespuesta.cMsjError = ex.Message;
                    }
                    return this.Request.CreateResponse<RptContenido<IEnumerable<Object>>>(HttpStatusCode.OK, objRespuesta);
                }
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
        }
    }
}
