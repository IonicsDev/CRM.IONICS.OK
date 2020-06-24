using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class ArchivoModuloRepository : DelRepository<ArchivoModulo, Int32>
    {
        public ArchivoModuloRepository()
            : base(new ConfigurationContext())
        {

        }
        
        private IUserContext UserContext { get; set; }

        public ArchivoModuloRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }
    }
}
