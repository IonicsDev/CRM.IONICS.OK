using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vPersonasEnEspera : BaseViews.BaseView
    {

        private const string VIEW_NAME = "v_PersonasEnEspera";



        public vPersonasEnEspera()
            : base(VIEW_NAME)
        {

        }


        public vPersonasEnEspera GetById(int id)
        {
            vPersonasEnEspera ret = null;

            foreach (DataRow row in base.GetByID(id).Rows)
            {
                ret = new vPersonasEnEspera();
                ret.Id = (int)row["ID"];
            }

            return ret;
        }

        

        
    }
}
