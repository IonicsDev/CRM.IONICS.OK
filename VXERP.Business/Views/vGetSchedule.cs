using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vGetSchedule : BaseViews.BaseViewString
    {

        private const string SP_NAME = "GetSchedule";

        public vGetSchedule()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable GetSchedule(int mes, int anio, string estado)
        {
            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@nMonth", mes),
                                                    new System.Data.SqlClient.SqlParameter("@nYear", anio),
                                                    new System.Data.SqlClient.SqlParameter("@sEstado", estado));

            return datos;
        }
        
    }
}
