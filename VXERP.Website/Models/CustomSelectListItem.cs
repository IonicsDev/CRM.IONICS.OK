using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using System.Xml.Linq;

namespace VXERP.Website.Models
{
    public class CustomSelectItem : SelectListItem
    {
        public bool Enabled { get; set; }
    }

    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString MyDropDownList(this HtmlHelper html, IEnumerable<CustomSelectItem> selectList)
        {
            var selectDoc = XDocument.Parse(html.DropDownList("", (IEnumerable<SelectListItem>)selectList).ToString());

            var options = from XElement el in selectDoc.Element("select").Descendants()
                          select el;

            foreach (var item in options)
            {
                var itemValue = item.Attribute("value");
                if (!selectList.Where(x => x.Value == itemValue.Value).Single().Enabled)
                    item.SetAttributeValue("disabled", "disabled");
            }

            // rebuild the control, resetting the options with the ones you modified
            selectDoc.Root.ReplaceNodes(options.ToArray());
            return MvcHtmlString.Create(selectDoc.ToString());
        }
    }
}