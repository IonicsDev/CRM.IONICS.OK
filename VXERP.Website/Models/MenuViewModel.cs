using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Business.Entities;

namespace CRM.Website.Models
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
        }

        public MenuViewModel(int menuId, string descripcion, string url)
        {
            this.MenuId = menuId;
            this.Descripcion = descripcion;
            this.URL = url;
        }

        public int MenuId { get; set; }
        public string Descripcion { get; set; }


        public string URL { get; set; }
        public int? Orden { get; set; }
        public string Class { get; set; }
        public bool Visible { get; set; }


        public IEnumerable<MenuViewModel> Children { get; set; }

        public static IEnumerable<MenuViewModel> CreateVM(int? parentid, List<Modulo> source)
        {
            return from men in source
                   where men.Parent_Id == parentid
                   select new MenuViewModel()
                   {
                       MenuId = men.Id,
                       Descripcion = men.Descripcion,
                       Class = men.Class,
                       URL = men.URL,
                       Visible = men.Visible,
                       Children = CreateVM(men.Id, source)
                   };
        }
    }
}