using CRM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Website.Security
{
    public class CustomPrincipalSerializeModel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public bool IsAdmin { get; set; }

        //public List<UsuarioRolCliente> RolesCliente
        //{
        //    get;
        //    set;
        //}

     // public ICollection<RolEmpresa> RolesEmpresa { get; set; }

       
    }
}
