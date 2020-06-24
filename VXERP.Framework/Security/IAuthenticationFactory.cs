using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VXERP.Framework.Security
{
    public interface IAuthenticationFactory
    {
        IAuthentication Create();
    }
}
