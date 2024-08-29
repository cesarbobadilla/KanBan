using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad.EntidadesDTO
{
    public class UsuarioLogin
    {
        public string usuario { get; set; }
        public string password { get; set; }
        //public string dominio { get; set; }
        public short idApp { get; set; }
        public bool bPersistente { get; set; }
    }
}
