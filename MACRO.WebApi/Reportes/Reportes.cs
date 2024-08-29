using System;
using System.Net;
using Microsoft.Reporting.WebForms;
using MACRO.Entidad.EntidadesDTO;
using System.Collections.Generic;

namespace MACRO.WebApi.Reportes
{
    public class Reportes
    {
        public static NetworkCredential UtilUserNTL()
        {

            return new NetworkCredential(
                      System.Configuration.ConfigurationManager.AppSettings["UserREPORT"],
                     System.Configuration.ConfigurationManager.AppSettings["PasswordREPORT"],
                     System.Configuration.ConfigurationManager.AppSettings["DomainREPORT"]);

        }

        public static byte[] ObtenerReporte(string ruta, List<Parameters> parametros)
        {
            string URLBASE = System.Configuration.ConfigurationManager.AppSettings["URLBASE"];

            ReportViewer rptreporte = new ReportViewer();
            rptreporte.Reset();
            rptreporte.ProcessingMode = ProcessingMode.Remote;
            ServerReport serverreport = rptreporte.ServerReport;

            // sruta = "/ventas/venta/tickets/ticketventapollovivo";        
            var UserNTL = UtilUserNTL();
            serverreport.ReportServerCredentials = new CustomReportCredentials(UserNTL.UserName, UserNTL.Password, UserNTL.Domain);
            serverreport.ReportServerUrl = new Uri(URLBASE);
            serverreport.ReportPath = ruta;

            List<ReportParameter> list = new List<ReportParameter>();

            foreach (var param in parametros)
            {
                list.Add(new ReportParameter(param.Parameter, param.Value, false));
            }

            serverreport.SetParameters(list);
            serverreport.Refresh();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bResultado = rptreporte.ServerReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return bResultado;
        }
    }
}