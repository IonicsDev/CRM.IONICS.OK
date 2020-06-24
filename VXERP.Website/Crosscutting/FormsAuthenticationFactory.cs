using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Website.Security;


namespace CRM.Website.Crosscutting
{

    public class FormsAuthenticationFactory : IAuthenticationFactory
    {
        public IAuthentication Create()
        {
            return new FormsAuthenticationService();
        }
    }
}
