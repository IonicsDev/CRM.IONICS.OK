using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vPersonasFueradePlanta : BaseViews.BaseView
    {

        private const string VIEW_NAME = "vPersonasFueradePlanta";



        public vPersonasFueradePlanta()
            : base(VIEW_NAME)
        {

        }


        public vPersonasFueradePlanta GetById(int id)
        {
            vPersonasFueradePlanta ret = null;

            foreach (DataRow row in base.GetByID(id).Rows)
            {
                ret = new vPersonasFueradePlanta();
                ret.Id = (int)row["ID"];
            }

            return ret;
        }

        

        
    }
}
