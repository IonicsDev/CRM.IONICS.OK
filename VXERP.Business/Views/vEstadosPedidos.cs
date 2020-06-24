using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vEstadosPedidos : BaseViews.BaseView
    {
        public const string VIEW_NAME = "v_EstadosPedido";


        public vEstadosPedidos()
            : base(VIEW_NAME)
        {

        }

        public DataTable GetAll()
        {
            DataTable datos = base.GetViewModel();
            return datos;
        }
    }
}
