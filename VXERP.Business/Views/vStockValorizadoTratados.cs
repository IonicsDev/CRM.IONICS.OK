using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vStockValorizadoTratados : BaseViews.BaseViewString
    {

        private const string SP_NAME = "StockValorizadoTratados";

        public vStockValorizadoTratados()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable Get_Datos(string fecha = null)
        {
            if (fecha == null || fecha == "")
                fecha = DateTime.Now.Date.ToString("yyyyMMdd");

              
            DataTable datos = new DataTable();
            try
            {
                DateTime dt = DateTime.Parse(fecha);
                //DateTime dt;
                //try
                //{
                //    dt = Convert.ToDateTime(dateString[1] + "/" + dateString[0] + "/" + dateString[2]);
                //}
                //catch (Exception)
                //{
                //    dt = Convert.ToDateTime(fecha);
                //}

                datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@FeHasta", dt.ToString("yyyyMMdd")));
            }
            catch (Exception) { }

            if (datos.Rows.Count == 0)
                datos.Rows.Add();

            if (datos.Columns.Count == 0)
            {
                datos.Columns.Add("Cg_Art"); datos.Columns.Add("Des_Art"); datos.Columns.Add("Stock");
                datos.Columns.Add("Importe1"); datos.Columns.Add("StockVal");
                datos.Columns.Add("Dosis"); datos.Columns.Add("Ancho"); datos.Columns.Add("Largo");
                datos.Columns.Add("Altura"); datos.Columns.Add("Diametro"); datos.Columns.Add("Unidad");
                datos.Columns.Add("VOLFISICOUNIT"); datos.Columns.Add("VOLFISICOTOT");
                datos.Columns.Add("VOLEQUIVTOTAL");
            }

            return datos;
        }

    }
}
