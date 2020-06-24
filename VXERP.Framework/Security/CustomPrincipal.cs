using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace VXERP.Framework.Security
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
          //  return Roles.Where(r => r == role).Any();
            return true;
        }

        public ICollection<RolEmpresa> RolesEmpresa { get; set; }
    }
}
