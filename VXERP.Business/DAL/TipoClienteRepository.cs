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

    public class TipoClienteRepository : DelRepository<TipoCliente, int>
    {
        public TipoClienteRepository()
            : base(new ConfigurationContext())
        {

        }
        private IUserContext UserContext { get; set; }

        public TipoClienteRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


        public string GetAgrupacionCompleta(int id)
        {
            StringBuilder sb = new StringBuilder();
            TipoCliente item = base.Get(id, o => o.Parent).FirstOrDefault();
            sb.Insert(0, item.Descripcion);

            if (item.Parent != null)
            {
                sb.Insert(0, "|");
                GetParentAgrupacion(ref sb, item.Parent_Id.Value);

            }
            return sb.ToString();
        }

        private void GetParentAgrupacion(ref StringBuilder sb, int parentId)
        {
            TipoCliente item = base.Get(parentId, o => o.Parent).FirstOrDefault();
            sb.Insert(0, item.Descripcion);
            if (item.Parent != null)
            {
                sb.Insert(0, ".");
                GetParentAgrupacion(ref sb, item.Parent_Id.Value);

            }
        }

        public override IQueryable<TipoCliente> GetFiltered(Expression<Func<TipoCliente, bool>> filter, params Expression<Func<TipoCliente, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
           

            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<TipoCliente> listObjectResult = new List<TipoCliente>();

            foreach (var item in listObjects)
            {
             
              listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<TipoCliente> GetAll(params Expression<Func<TipoCliente, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var listObjects = base.GetAll(includes).ToList();

            var filterExpressions = includes.ToList();
            
            List<TipoCliente> listObjectResult = new List<TipoCliente>();

            foreach (var item in listObjects)
            {

                listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();

        }
    }
}
