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

    public class ParametrosGeneralesRepository : DelRepository<ParametrosGenerales, int>
    {
        public ParametrosGeneralesRepository()
            : base(new ConfigurationContext())
        {

        }
        private IUserContext UserContext { get; set; }

        public ParametrosGeneralesRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        public bool IsExist(ParametrosGenerales parametroGeneral)
        {
            return this.GetFiltered(o => o.Compania_Id == parametroGeneral.Compania_Id).Any();
        }

        public override IQueryable<ParametrosGenerales> GetFiltered(Expression<Func<ParametrosGenerales, bool>> filter, params Expression<Func<ParametrosGenerales, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Compania);

            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<ParametrosGenerales> listObjectResult = new List<ParametrosGenerales>();

            foreach (var item in listObjects)
            {
                if (this.UserContext.Has_Perm(item.Compania))
                    listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<ParametrosGenerales> GetAll(params Expression<Func<ParametrosGenerales, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var listObjects = base.GetAll(includes).ToList();

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Compania);
            List<ParametrosGenerales> listObjectResult = new List<ParametrosGenerales>();

            foreach (var item in listObjects)
            {
                if (this.UserContext.Has_Perm(item.Compania))
                    listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();

        }
    }
}
