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

    public class CompaniaRepository : DelRepository<Compania, Int32>
    {
        private IUserContext UserContext { get; set; }

        public CompaniaRepository()
            : base(new ConfigurationContext())
        {

        }

        public CompaniaRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        /// <summary>
        /// Validacion de RUC repetido para nuevas companias
        /// </summary>
        /// <param name="compania"></param>
        /// <returns></returns>
        public bool IsValidCompania(Compania compania)
        {
            if (base.GetFiltered(f => f.RUC == compania.RUC).Any())
                return false;

            return true;
        }

        /// <summary>
        /// Sobrecarga del metodo Create para compañias con validacion y seteo de parametros generales.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userId"></param>
        public override void Add(Compania item, int userId)
        {
            if (IsValidCompania(item))
            {
                try
                {
                    //Alta Compañia
                    base.Add(item, userId);

                    //Alta Division y Sucursal Default
                    (new DivisionRepository()).CreateDivisionDefault(item, userId);


                   

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new Exception(" Ya existe otra compañia con el mismo RUC");
            }
        }

        public override IQueryable<Compania> GetFiltered(Expression<Func<Compania, bool>> filter, params Expression<Func<Compania, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            if(this.UserContext.RolesEmpresa.Any(f=>f.Rol_Id == 1))
                return base.GetFiltered(filter, includes);

            var listObjects =  base.GetFiltered(filter, includes).ToList();
            List<Compania> listObjectResult = new List<Compania>();

            foreach (var compania in listObjects)
            {
                if (this.UserContext.Has_Perm(compania))
                    listObjectResult.Add(compania);
            }

            return listObjectResult.AsQueryable();
        }

        public  override  IQueryable<Compania> GetAll(params Expression<Func<Compania, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            if (this.UserContext.RolesEmpresa.Any(f => f.Rol_Id == 1))
                return base.GetAll(includes);

            var listObjects = base.GetAll(includes).ToList();
            List<Compania> listObjectResult =  new  List<Compania>();

            foreach (var compania in listObjects)
            {
                if (this.UserContext.Has_Perm(compania))
                    listObjectResult.Add(compania);
            }
            return listObjectResult.AsQueryable();

        }

      
    }
}
