using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vRPT_PanelControl_Equivalente : BaseViews.BaseViewString
    {

        private const string SP_NAME = "rpt_PanelControl_Equivalente";

        public vRPT_PanelControl_Equivalente(params System.Data.SqlClient.SqlParameter[] SP_PARAMS)
            : base(SP_NAME, true, null)
        {

        }

        public DataTable Get_Datos(string fechaDesde = null, string fechaHasta = null)
        {
            if (fechaDesde == null || fechaDesde == "")
                fechaDesde = DateTime.Now.Date.ToString("yyyyMMdd");

            if (fechaHasta == null || fechaHasta == "")
                fechaHasta = DateTime.Now.Date.ToString("yyyyMMdd");


            DataTable datos = new DataTable();
            try
            {
                DateTime dtFechaDesde = DateTime.Parse(fechaDesde);
                DateTime dtFechaHasta = DateTime.Parse(fechaHasta);

                datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@FechaDesde", dtFechaDesde.ToString("yyyyMMdd")),
                                            new System.Data.SqlClient.SqlParameter("@FechaHasta", dtFechaHasta.ToString("yyyyMMdd")));
            }
            catch (Exception) { }

            return datos;
        }
        
    }
}
