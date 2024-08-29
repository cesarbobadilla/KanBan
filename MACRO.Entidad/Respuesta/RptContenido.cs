using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad
{
    public class RptContenido<T> : RptBasica
    {
        public T oContenido { get; set; }
        public bool lPaginacion { get; set; }
        public int nRegistros;
    }
}
