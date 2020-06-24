using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CRM.Business.DataService.Secutity
{
    /// <summary>
    ///  Esta clase contiene las propiedades para el manejo de la seguridad de la aplicación.
    ///  </summary>
    public class AppSettings
    {
        public static string ConnectionString
        {
            get
            {


                string connectionstring = ConfigurationManager.ConnectionStrings["EntitiesConnectionString"].ToString();
               
                return connectionstring;
            }
        }
    }
}
