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

    public class DivisionRepository : DelRepository<Division, Int32>
    {
        private IUserContext UserContext { get; set; }

        public DivisionRepository()
            : base(new ConfigurationContext())
        {

        }

        public DivisionRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        /// <summary>
        /// Creacion de Division por default
        /// </summary>
        /// <param name="compania"></param>
        /// <param name="userID"></param>
        public void CreateDivisionDefault(Compania compania, int userID)
        {
            try
            {
                Division divisionDefault = new Division();
                divisionDefault.Descripcion = "Div. Default";
                divisionDefault.IdCompania = compania.Id;
                divisionDefault.Responsable = compania.ResponsableLegal;
                divisionDefault.FechaCreacion = DateTime.Now;

                base.Add(divisionDefault, userID);

                //Creacion de Sucursal Default
                (new SucursalRepository()).CreateSucursalDefault(divisionDefault, compania, userID);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Crear División por defecto para la Compañia. Error: " + ex.Message);
            }
        }

        public override IQueryable<Division> GetFiltered(Expression<Func<Division, bool>> filter, params Expression<Func<Division, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Compania);
            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<Division> listObjectResult = new List<Division>();

            foreach (var item in listObjects)
            {
                if (this.UserContext.Has_Perm(item))
                    listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<Division> GetAll(params Expression<Func<Division, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Compania);
            List<Division> listObjectResult = new List<Division>();

            var listObjects = base.GetAll(filterExpressions.ToArray());

            foreach (var item in listObjects)
            {
                if (this.UserContext.Has_Perm(item))
                    listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();

        }
      
    }
}
