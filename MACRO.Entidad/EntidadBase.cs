using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad
{
    public class EntidadBase
    {

        [StringLength(1)]
        public string Estado { get; set; }
    }
}
