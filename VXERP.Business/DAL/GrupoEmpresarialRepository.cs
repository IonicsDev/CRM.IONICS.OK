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

    public class GrupoEmpresarialRepository : DelRepository<GrupoEmpresarial, Int32>
    {
        private IUserContext UserContext { get; set; }

        public GrupoEmpresarialRepository()
            : base(new ConfigurationContext())
        {

        }

        public GrupoEmpresarialRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        public override IQueryable<GrupoEmpresarial> GetFiltered(Expression<Func<GrupoEmpresarial, bool>> filter, params Expression<Func<GrupoEmpresarial, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            if (this.UserContext.RolesEmpresa.Any(f => f.Rol_Id == 1))
                return base.GetFiltered(filter, includes);

            var listObjects = base.GetFiltered(filter, includes).ToList();
            List<GrupoEmpresarial> listObjectResult = new List<GrupoEmpresarial>();

            foreach (var item in listObjects)
            {
                if ((new CompaniaRepository(this.UserContext)).GetFiltered(o => o.IdGrupoEmpresarial == item.Id).Any())
                    listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<GrupoEmpresarial> GetAll(params Expression<Func<GrupoEmpresarial, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            if (this.UserContext.RolesEmpresa.Any(f => f.Rol_Id == 1))
                return base.GetAll(includes);

            var listObjects = base.GetAll(includes).ToList();
            List<GrupoEmpresarial> listObjectResult = new List<GrupoEmpresarial>();

            foreach (var item in listObjects)
            {
                if ((new CompaniaRepository(this.UserContext)).GetFiltered(o => o.IdGrupoEmpresarial == item.Id).Any())
                {
                    listObjectResult.Add(item);
                }
                else
                {
                    if (!(new CompaniaRepository()).GetFiltered(o => o.IdGrupoEmpresarial == item.Id).Any())
                    {
                        listObjectResult.Add(item);
                    }
                }
            }
            return listObjectResult.AsQueryable();

        }

      
    }
}
