using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using System.Data.SqlClient;

namespace CRM.Business.DAL
{

    public class ModuloPermisoRepository : DelRepository<ModuloPermiso, int>
    {
        public ModuloPermisoRepository()
            : base(new ConfigurationContext())
        {

        }

        public void DeleteByRol(int idRol)
        {
            try
            {
                base.Context.DBContext.Database.ExecuteSqlCommand("DELETE FROM RolModulos WHERE Rol_Id =@IdRol", new SqlParameter("@IdRol", idRol));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


      
    }
}
