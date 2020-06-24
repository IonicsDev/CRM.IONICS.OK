using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vDetalleEntrega : BaseViews.BaseViewString
    {

        private const string SP_NAME = "DetalleEntrega";



        public vDetalleEntrega()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable DetalleEntrega(int PedidoId)
        {
            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@PedidoId", PedidoId));

            return datos;
        }
        
    }
}
