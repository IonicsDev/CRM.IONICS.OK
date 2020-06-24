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

    public class SucursalRepository : DelRepository<Sucursal, Int32>
    {
        private IUserContext UserContext { get; set; }

        public SucursalRepository()
            : base(new ConfigurationContext())
        {

        }

        public SucursalRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }


        /// <summary>
        /// Creacion de Sucursal Por Default
        /// </summary>
        /// <param name="Division"></param>
        /// <param name="userID"></param>
        public void CreateSucursalDefault(Division division, Compania compania, int userID)
        {
            try
            {
                Sucursal sucursalDefault = new Sucursal();

                sucursalDefault.IdDivision = division.Id;
                sucursalDefault.IdCiudad = compania.IdCiudad;
                sucursalDefault.Calle = compania.Calle;
                sucursalDefault.Descripcion = "Suc. Default";
                sucursalDefault.Numero = compania.Numero;
                sucursalDefault.FechaCreacion = DateTime.Now;

                base.Add(sucursalDefault, userID);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Crear División por defecto para la Compañia. Error: " + ex.Message);
            }
        }

        public override IQueryable<Sucursal> GetFiltered(Expression<Func<Sucursal, bool>> filter, params Expression<Func<Sucursal, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Division);
            filterExpressions.Add(c => c.Division.Compania);
            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<Sucursal> listObjectResult = new List<Sucursal>();

            foreach (var item in listObjects)
            {
                if (this.UserContext.Has_Perm(item))
                    listObjectResult.Add(item);
            }

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<Sucursal> GetAll(params Expression<Func<Sucursal, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var filterExpressions = includes.ToList();
            filterExpressions.Add(c => c.Division);
            filterExpressions.Add(c => c.Division.Compania);
            List<Sucursal> listObjectResult = new List<Sucursal>();

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
