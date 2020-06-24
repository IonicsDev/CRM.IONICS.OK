using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Framework.Logging;
using CRM.Website.Security;
using CRM.Website.Crosscutting;

namespace CRM.Website.App_Code
{
    public class CrosscuttingHelper
    {
        public static void Initialise()
        {
        
            AuthenticationFactory.SetCurrent(new FormsAuthenticationFactory());
            
        }
    }
}
