using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MACRO.Entidad
{
    public partial class Entidadmodel : DbContext
    {
        public Entidadmodel()
            : base("name=seguridad_model")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
