using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CRM.Common.EntityDomain;

namespace CRM.Business.Entities.BaseEntities
{
   
    public interface IUserContext 
    {
        int UserID { get; set; }

        bool Has_Perm(string modulo, string accion, string permiso);

        bool Has_Perm(int modulo);

        string UserName {get;set;}


        ICollection<UsuarioRolCliente> RolesCliente { get; set; }

        List<Modulo> Modulos { get; set; }

        ICollection<RolEmpresa> RolesEmpresa { get; set; }

        Modulo CurrentModulo { get; set; }
    }
}
