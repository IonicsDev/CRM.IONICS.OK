using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vPersonasEnPlanta : BaseViews.BaseView
    {

        private const string VIEW_NAME = "v_PersonasEnPlanta";



        public vPersonasEnPlanta()
            : base(VIEW_NAME)
        {

        }


        public vPersonasEnPlanta GetById(int id)
        {
            vPersonasEnPlanta ret = null;

            foreach (DataRow row in base.GetByID(id).Rows)
            {
                ret = new vPersonasEnPlanta();
                ret.Id = (int)row["ID"];
            }

            return ret;
        }

        

        
    }
}
