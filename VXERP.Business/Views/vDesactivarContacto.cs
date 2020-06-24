using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vDesactivarContacto : BaseViews.BaseViewString
    {
        private const string SP_NAME = "DesactivarContacto";

        public vDesactivarContacto()
            :base(SP_NAME, true, null )
        {

        }

        public void DesactivarContacto(int id)
        {
            base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@Contacto_ID", id));
        }
    }
}
