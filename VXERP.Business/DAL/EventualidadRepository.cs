using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class EventualidadRepository : DelRepository<Eventualidad, Int32>
    {
        public EventualidadRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public EventualidadRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }
      
    }
}
