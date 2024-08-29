using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Repositorio
{
    public interface IRepositorio
    {
        void reloadConection(string dbname, string app);
        IList<TObject> ExecuteSP<TObject>(String name, IList<SqlParameter> parameters);
        CabeceraDetalleGenerico<TCabecera, List<TDetalle>> GetMultipleResultSetFromSP<TCabecera, TDetalle>(string spName, IList<SqlParameter> parameters);
        Object ExecuteProcedureMulti(string spName, IList<Parameters> parameters);
    }
}
