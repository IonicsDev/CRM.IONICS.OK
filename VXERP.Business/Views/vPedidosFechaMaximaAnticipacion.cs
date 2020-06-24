using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vPedidosFechaMaximaAnticipacion : BaseViews.BaseViewString
    {

        private const string SP_NAME = "PedidosFechaMaximaAnticipacion";



        public vPedidosFechaMaximaAnticipacion()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable PedidosFechaMaximaAnticipacion()
        {
            DataTable datos = base.GetViewModel_SP();

            return datos;
        }
        
    }
}
