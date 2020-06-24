using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vGetScheduleRetiro : BaseViews.BaseViewString
    {

        private const string SP_NAME = "GetScheduleRetiro";

        public vGetScheduleRetiro()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable GetScheduleRetiro(int mes, int anio)
        {
            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@nMonth", mes),
                                                    new System.Data.SqlClient.SqlParameter("@nYear", anio));

            return datos;
        }
        
    }
}
