using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vSecuenciamiento : BaseViews.BaseView
    {

        private const string SP_NAME = "Ver_Secuenciamiento";



        public vSecuenciamiento()
            : base(SP_NAME, true,null)
        {

        }

        public DataTable Get_Dinamico(){

            DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@Proceso", 1), new System.Data.SqlClient.SqlParameter("@Cg_Celda", "F1"));

            return datos;
        }

         public DataTable Get_Estatico(){

             DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@Proceso", 21), new System.Data.SqlClient.SqlParameter("@Cg_Celda", "F1"));

            return datos;
        }

         public DataTable Get_DinamicoSegundaPlanta()
         {

             DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@Proceso", 1), new System.Data.SqlClient.SqlParameter("@Cg_Celda", "F2"));

             return datos;
         }

         public DataTable Get_EstaticoSegundaPlanta()
         {

             DataTable datos = base.GetViewModel_SP(new System.Data.SqlClient.SqlParameter("@Proceso", 21), new System.Data.SqlClient.SqlParameter("@Cg_Celda", "F2"));

             return datos;
         }



        
    }
}
