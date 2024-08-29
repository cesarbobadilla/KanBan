using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad.EntidadesDTO
{
    public class Request
    {
        public string procedure { get; set; }
        public List<Parameters> parameters { get; set; }

        public Request() {
            parameters = new List<Parameters>();
        }
    }

    public class RequestReport
    {
        public string ruta { get; set; }
        public List<Parameters> parameters { get; set; }

        public RequestReport()
        {
            parameters = new List<Parameters>();
        }
    }
}
