using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VXERP.Framework.Logging;
using VXERP.Framework.Security;

namespace VXERP.Framework.Helpers
{
    public class CrosscuttingHelper
    {
        public static void Initialise()
        {
           // LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            AuthenticationFactory.SetCurrent(new FormsAuthenticationFactory());
            
        }
    }
}
