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

    public class ClienteRepository : DelRepository<Cliente, Int32>
    {
        public ClienteRepository()
            : base(new ConfigurationContext())
        {

        }

        private IUserContext UserContext { get; set; }

        public ClienteRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


        

        public override IQueryable<Cliente> GetFiltered(Expression<Func<Cliente, bool>> filter, params Expression<Func<Cliente, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
            

            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<Cliente> listObjectResult = new List<Cliente>();

            

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<Cliente> GetAll(params Expression<Func<Cliente, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var listObjects = base.GetAll(includes).ToList();

            var filterExpressions = includes.ToList();
           
            List<Cliente> listObjectResult = new List<Cliente>();

        

            return listObjectResult.AsQueryable();

        }
        

      
    }
}
