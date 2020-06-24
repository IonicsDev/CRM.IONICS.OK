using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CRM.Business.Entities;
using System.IO;

namespace CRM.Business.Views
{
    public class vEliminarContacto : BaseViews.BaseViewString
    {

        private const string SP_NAME = "EliminarContacto";



        public vEliminarContacto()
            : base(SP_NAME, true,null)
        {

        }

        public void EliminarContacto(int id)
        {
             base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@Contacto_ID", id));
            
            
        }
        
    }
}
