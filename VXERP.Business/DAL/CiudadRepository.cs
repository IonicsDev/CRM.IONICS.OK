using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class CiudadRepository : DelRepository<Ciudad, Int32>
    {
        public CiudadRepository()
            : base(new ConfigurationContext())
        {

        }
        

      
    }
}
