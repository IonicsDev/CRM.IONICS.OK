using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CRM.Business.Entities;

namespace CRM.Website.Crosscutting
{
    public interface ICustomPrincipal : IPrincipal
    {
        int UserID { get; set; }

        string UserName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        bool Has_Perm( string modulo, string accion, string permiso);

        bool Has_Perm(int modulo_id);


      

        ICollection<RolEmpresa> RolesEmpresa { get; set; }

        ICollection<UsuarioRolCliente> RolesCliente { get; set; }
    }
}
