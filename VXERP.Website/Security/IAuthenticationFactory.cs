using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Website.Security
{
    public interface IAuthenticationFactory
    {
        IAuthentication Create();
    }
}
