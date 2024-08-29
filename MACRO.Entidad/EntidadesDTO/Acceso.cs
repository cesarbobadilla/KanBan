using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad.EntidadesDTO
{
    public class Acceso
    {        
        public short nCodAcceso { get; set; }        
        public short nCodPadre { get; set; }        
        public short nCodUsuario { get; set; }        
        public string cNombre { get; set; }        
        public string cDescripcion { get; set; }        
        public string cComando { get; set; }        
        public byte nPeso { get; set; }        
        public string cEstado { get; set; }        
        public string cClaseIcono { get; set; }
    }
}
