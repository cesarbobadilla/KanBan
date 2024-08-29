using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad.EntidadesDTO
{
    public class Usuario
    {       
        public string cNombre { get; set; }       
        public string cUsuario { get; set; }       
        public short nCodUsuario { get; set; }       
        public int nCodPlanilla { get; set; }       
        public int nCodSesion { get; set; }       
        public short nAplicacion { get; set; }       
        public string cToken { get; set; }       
        public Acceso[] aAccesos { get; set; }       
        public string cPassword { get; set; }        
        public string cDominio { get; set; }
        public string Ip { get; set; }
        public string cFrase { get; set; }

        public Usuario() { 
        }
        public Usuario(string user, string pass, string domine) {
            cUsuario = user;
            cPassword = pass;
            cDominio = domine;
        }
    }
}
