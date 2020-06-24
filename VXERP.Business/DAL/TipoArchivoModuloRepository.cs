using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class TipoArchivoModuloRepository : DelRepository<TipoArchivoModulo, Int32>
    {
        public TipoArchivoModuloRepository()
            : base(new ConfigurationContext())
        {

        }
        

      
    }
}
