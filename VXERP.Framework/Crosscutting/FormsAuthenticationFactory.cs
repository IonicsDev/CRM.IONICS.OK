using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VXERP.Framework.Security;

namespace VXERP.Framework.Crosscutting
{
    public class FormsAuthenticationFactory : IAuthenticationFactory
    {
        public IAuthentication Create()
        {
            return new FormsAuthenticationService();
        }
    }
}
