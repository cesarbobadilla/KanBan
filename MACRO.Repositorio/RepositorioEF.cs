
using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MACRO.Repositorio
{
    public class RepositorioEF<TContext> : IRepositorio
            where TContext : DbContext
    {
        protected readonly TContext context;

        public RepositorioEF(TContext _context)
        {
            context = _context;            
        }

        public void reloadConection(string dbname, string app) {
            dbname = dbname.Replace("[", "").Replace("]", "");
            context.Database.Connection.ConnectionString = "data source=DESKTOP-U8N83KU\\SQLEXPRESS;initial catalog=Proyectos;Trusted_Connection=Yes;MultipleActiveResultSets=True;App="+ app;
        }

        public IList<TObject> ExecuteSP<TObject>(string spName, IList<SqlParameter> parameters)
        {
            try
            {                
                string parString = "@" + parameters[0].ParameterName;

                for (int i = 1; i < parameters.Count; i++)
                {
                    parString += ", @" + parameters[i].ParameterName;
                }

                object[] parametros = new object[parameters.Count];

                for (int i = 0; i < parameters.Count; i++)
                {
                    parametros[i] = parameters[i];
                }
                  context.Database.CommandTimeout = 720;         // tiempo de espera de base de datos
                return context.Database.SqlQuery<TObject>("exec " + spName + " " + parString, parametros).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Object ExecuteProcedureMulti(string spName, IList<Parameters> parameters)
        {
            dynamic consulta = new ExpandoObject();
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            foreach (var param in parameters)
            {
                object value = param.Value;
                if (param.Value == "NULL")
                    value = DBNull.Value;
                command.Parameters.Add(new SqlParameter(param.Parameter, value));
            }
               
            try
            {
                context.Database.Connection.Open();
                var reader = command.ExecuteReader();
                int count = 0;
                do
                {
                    List<Object> ObjConsult = new List<Object>();
                    DataTable schemaTable = reader.GetSchemaTable();
                    List<string> columnas = new List<string>();
                    count++;
                   
                    if (reader.HasRows)
                    {
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            foreach (DataColumn column in schemaTable.Columns)
                            {
                                if (column.ColumnName.Equals("ColumnName"))
                                    columnas.Add(row[column].ToString());
                            }
                        }
                        while (reader.Read())
                        {
                            dynamic row = new ExpandoObject();
                            var dictionary = (IDictionary<string, object>)row;
                            for (int i = 0; i < columnas.Count; i++)
                            {
                                dictionary.Add(columnas[i], reader.GetValue(i));
                            }
                            ObjConsult.Add(row);
                        }
                    }                    
                    var objecto = (IDictionary<string, object>)consulta;
                    string name = "Detalle";
                    if (count == 1) name = "Cabecera";
                    else if (count > 2) name= name + (count-1).ToString();
                    objecto.Add(name, ObjConsult);
                } while (reader.NextResult());
                reader.Close();

                return consulta;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }

        public CabeceraDetalleGenerico<TCabecera, List<TDetalle>> GetMultipleResultSetFromSP<TCabecera, TDetalle>(string spName, IList<SqlParameter> parameters)
        {
           CabeceraDetalleGenerico<TCabecera, List<TDetalle>> ObjReps= new CabeceraDetalleGenerico<TCabecera, List<TDetalle>>();
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            foreach(var param in parameters)
                command.Parameters.Add(param);
            try
            {
                context.Database.Connection.Open();
                var reader = command.ExecuteReader();

                ObjReps.Cabecera =  ((IObjectContextAdapter)context).ObjectContext.Translate<TCabecera>
                (reader).FirstOrDefault();
                reader.NextResult();
                ObjReps.Detalle =
                    ((IObjectContextAdapter)context).ObjectContext.Translate<TDetalle>
                (reader).ToList();
                return ObjReps;
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }
    }
}