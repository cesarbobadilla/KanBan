using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad.EntidadesDTO
{
    public class Parameters
    {
        public string Parameter { get; set; }
        public string Value { get; set; }

        public Parameters(string name, string value)
        {
            this.Parameter = name;
            this.Value = value;
        }
    }  
}
