using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class RolEmpresaRepository : DelRepository<RolEmpresa, int>
    {
        public RolEmpresaRepository()
            : base(new ConfigurationContext())
        {

        }

        public void Delete(int idRolEmpresa)
        {

            try
            {
                base.Context.DBContext.Database.ExecuteSqlCommand("DELETE FROM UsuarioRols WHERE Id =@Id", new SqlParameter("@Id", idRolEmpresa));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

      
    }
}
