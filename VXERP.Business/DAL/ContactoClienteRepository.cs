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

    public class ContactoClienteRepository : DelRepository<ContactoCliente, Int32>
    {
        private IUserContext UserContext { get; set; }

        public ContactoClienteRepository()
            : base(new ConfigurationContext())
        {

        }

        public ContactoClienteRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


        public override IQueryable<ContactoCliente> GetFiltered(Expression<Func<ContactoCliente, bool>> filter, params Expression<Func<ContactoCliente, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Cliente);
           

            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<ContactoCliente> listObjectResult = new List<ContactoCliente>();

           

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<ContactoCliente> GetAll(params Expression<Func<ContactoCliente, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Cliente);
         

            List<ContactoCliente> listObjectResult = new List<ContactoCliente>();

            var listObjects = base.GetAll(filterExpressions.ToArray());

         

            return listObjectResult.AsQueryable();

        }


        
    }
}
