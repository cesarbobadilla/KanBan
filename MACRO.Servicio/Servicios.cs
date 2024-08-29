using log4net;
using MACRO.Entidad;
using MACRO.Entidad.EntidadesDTO;
using MACRO.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MACRO.Servicio
{
    public class Servicios : IServicio
    {
        protected readonly IRepositorio repositorio;
        protected ILog logger;

        public Servicios(IRepositorio _repositorio)
        {
            repositorio = _repositorio;
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = LogManager.GetLogger("MACRO");
            logger = Log;
        }

        #region [Seguridad]


        #endregion

        public Object getProcedure(string appname, string procedure, List<Parameters> parametros)
        {
            repositorio.reloadConection(procedure.Split('.')[0], appname);           
            return repositorio.ExecuteProcedureMulti(procedure, parametros);           
        }

    }
}
