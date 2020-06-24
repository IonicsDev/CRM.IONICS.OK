using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;

namespace CRM.Business.DAL
{

    public class RolRepository : DelRepository<Rol, Int32>
    {
        public enum SystemROL
        {
            ADMIN = 1,
            CLIENTE = 25
        }
        public RolRepository()
            : base(new ConfigurationContext())
        {

        }


        public ICollection<Rol> GetRolesByIdUser(int idUser)
        {
            var repository = new RolRepository();
         //   var results = repository.GetFiltered(c => c.Usuarios.Select(p => p.Id).Contains(idUser));


          //  return results.ToList();

            return null;
        }
      
    }
}
