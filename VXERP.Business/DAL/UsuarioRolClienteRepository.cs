using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Business.Contexts;
using CRM.Business.Entities;
using CRM.Common.EntityDomain;
using System.Data;
using CRM.Business.Views;
using System.Web;


namespace CRM.Business.DAL
{

    public class UsuarioRolClienteRepository : DelRepository<UsuarioRolCliente, Int32>
    {
        public UsuarioRolClienteRepository()
            : base(new ConfigurationContext())
        {

        }
      
    }
}
