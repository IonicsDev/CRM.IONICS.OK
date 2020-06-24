using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vEmpleadosEnIonics : BaseViews.BaseView
    {

        private const string VIEW_NAME = "v_EmpleadosEnIonics";



        public vEmpleadosEnIonics()
            : base(VIEW_NAME)
        {

        }


        public vEmpleadosEnIonics GetById(int id)
        {
            vEmpleadosEnIonics ret = null;

            foreach (DataRow row in base.GetByID(id).Rows)
            {
                ret = new vEmpleadosEnIonics();
                ret.Id = (int)row["ID"];
            }

            return ret;
        }

        

        
    }
}
