using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CRM.Business.Views
{
    public class vIndustrias : BaseViews.BaseView
    {

        private const string VIEW_NAME = "v_industrias";



        public vIndustrias()
            : base(VIEW_NAME)
        {

        }


        public vIndustrias GetById(int id)
        {
            vIndustrias ret = null;

            foreach (DataRow row in base.GetByID(id).Rows)
            {
                ret = new vIndustrias();
                ret.Id = (int)row["ID"];
            }

            return ret;
        }

        

        
    }
}
