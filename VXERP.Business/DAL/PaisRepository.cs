using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using CRM.Business.Entities.BaseEntities;

namespace CRM.Business.DAL
{

    public class PaisRepository : DelRepository<Pais, Int32>
    {
        private IUserContext UserContext { get; set; }

        public PaisRepository()
            : base(new ConfigurationContext())
        {
        }
       
        public PaisRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }
        

      
    }
}
