using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Business.Entities.BaseEntities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class ContactoRepository : DelRepository<Contacto, Int32>
    {
        private IUserContext UserContext { get; set; }
        public ContactoRepository()
            : base(new ConfigurationContext())
        {

        }
        public ContactoRepository(IUserContext userContext)
            : base(new ConfigurationContext())
        {
            this.UserContext = userContext;
        }

        public override void Remove(Contacto item, int userId)
        {
            try
            {
                base.Context.DBContext.Database.ExecuteSqlCommand("DELETE FROM ContactosCliente WHERE Contacto_Id =@Id", new SqlParameter("@Id", item.Id));
                base.Context.DBContext.Database.ExecuteSqlCommand("DELETE FROM ContactosProveedor WHERE Contacto_Id =@Id", new SqlParameter("@Id", item.Id));
                base.Context.DBContext.Database.ExecuteSqlCommand("DELETE FROM Contactos WHERE Id =@Id", new SqlParameter("@Id", item.Id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override IQueryable<Contacto> GetFiltered(Expression<Func<Contacto, bool>> filter, params Expression<Func<Contacto, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetFiltered(filter, includes);

            var filterExpressions = includes.ToList();
       
            var listObjects = base.GetFiltered(filter, filterExpressions.ToArray()).ToList();
            List<Contacto> listObjectResult = new List<Contacto>();

           

            return listObjectResult.AsQueryable();
        }

        public override IQueryable<Contacto> GetAll(params Expression<Func<Contacto, object>>[] includes)
        {
            if (this.UserContext == null)
                return base.GetAll(includes);

            var filterExpressions = includes.ToList();
           
            List<Contacto> listObjectResult = new List<Contacto>();

            var listObjects = base.GetAll(filterExpressions.ToArray());

           
            return listObjectResult.AsQueryable();

        }

         
        

      
    }
}
