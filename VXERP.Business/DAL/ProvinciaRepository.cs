using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class ProvinciaRepository : DelRepository<Provincia, Int32>
    {
        public ProvinciaRepository()
            : base(new ConfigurationContext())
        {

        }
        

      
    }
}
