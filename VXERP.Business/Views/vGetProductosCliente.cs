using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vGetProductosCliente : BaseViews.BaseViewString
    {

        private const string SP_NAME = "Get_ProductosCliente";



        public vGetProductosCliente()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable Get_ProductosCliente(int Cliente_Id)
        {
            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@ClieID", Cliente_Id));

            return datos;
        }
        
    }
}
