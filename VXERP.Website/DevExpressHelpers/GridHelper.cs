using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System.Reflection;
using System.Web.UI;
using System.Collections;
using DevExpress.Web;
using CRM.Business.Views.BaseViews;
using CRM.Business.DAL;
using CRM.Business.Entities;
using CRM.Website.Controllers;
using CRM.Business.Views;
using CRM.Website.DevExpressHelpers;
using System.Data;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using System.Web.Routing;
using CRM.Website.Crosscutting;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;

namespace CRM.Website.DevExpressHelpers
{
    public class GridHelper : BaseController
    {
        public static bool IsEntity(Type type)
        {
            if (type == null)
                return false;
            
            if (type == typeof(Business.Entities.BaseEntities.BaseEntity))
                return true;
            
            return IsEntity(type.BaseType);
        }

        public static bool IsView(Type type)
        {
            if (type == null)
                return false;

            if (type == typeof(System.Data.EnumerableRowCollection))
                return true;

            return IsView(type.BaseType);
        }

        public static bool IsModel(Type type)
        {
            if (type == null)
                return false;

            if (type == typeof(Models.ViewModelBase))
                return true;

            return IsModel(type.BaseType);
        }

        /// <summary>
        /// A traves del tipo de un viewModel se obtienen las propiedades y se las agregada a Columns
        /// solo aquellas que no tengan el decorador Invisible y a las agregadas se les cambiara el nombre
        /// si existe otro decorador que lo indique
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="type"></param>
        public static void AddColumnsSetting(MVCxGridViewColumnCollection Columns, Type type)
        {
            Type modelType = null;

            if (IsEntity(type))
                modelType = type;
            else
                modelType = type.GetGenericArguments().Where(t => t.BaseType.IsEquivalentTo(typeof(Business.Entities.BaseEntities.BaseEntity)) || (t.BaseType.BaseType != null && t.BaseType.BaseType.IsEquivalentTo(typeof(Business.Entities.BaseEntities.BaseEntity)))).FirstOrDefault();

            if (modelType == null)
            {
           
                    return;
            }

            foreach (var property in modelType.GetProperties())
            {
                if (modelType.Name == "Novedad" && property.Name == "NombreTipoNovedad")
                    continue;
                // Si se configuro el modelo para descartar propiedades a agregar al gridview
                var attr = property.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name.Equals("InvisibleAttribute"));
                if (property.Name == "Id" || attr != null)
                    continue;

                //attr = property.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name.Equals("ColumnNameGridViewAttribute"));
                if (!IsPropertyForRendered(property.Name))
                    continue;

   
               

                Columns.Add(property.Name);
                var style = new DevExpress.Web.GridViewHeaderStyle();
               // style.BackColor = System.Drawing.Color.LightGray;
                Columns[property.Name].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                Columns[property.Name].HeaderStyle.Font.Bold = true;
                ((MVCxGridViewColumn)Columns[property.Name]).Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                // Si se configuro el modelo para cambiar el nombre de la columna agregada
                attr = property.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name.Equals("ColumnNameGridViewAttribute"));
                if (attr != null)
                {
                    var nameColumn = attr.NamedArguments.FirstOrDefault(a => a.MemberName.Equals("Name")).TypedValue.Value.ToString();
                    Columns[property.Name].Caption = nameColumn;
                }
            }
        }

        public static void AddColumnsSettingViewData(MVCxGridViewColumnCollection Columns, 
            IEnumerable<CRM.Business.Views.BaseViews.BaseView.DynObject> model)
        {
            if (model.Count() == 0)
                return;

            foreach (var property in  model.First().GetDynamicMemberNames())
            {
                if (property.ToUpper().Trim() == "ID" || property.ToUpper().Trim() == "ESTADO")
                    continue;

                Columns.Add(property);
                var style = new DevExpress.Web.GridViewHeaderStyle();
                // style.BackColor = System.Drawing.Color.LightGray;
                Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                Columns[property].HeaderStyle.Font.Bold = true;
                ((MVCxGridViewColumn)Columns[property]).Settings.AutoFilterCondition = AutoFilterCondition.Contains;

            }
        }

        public static void AddColumnsSettingViewDataContacto(MVCxGridViewColumnCollection Columns, 
            IEnumerable<CRM.Business.Views.BaseViews.BaseView.DynObject> model, HtmlHelper Html)
        {
            if (model.Count() == 0)
                return;

            foreach (var property in model.First().GetDynamicMemberNames())
            {
                if (property.ToUpper().Trim() == "ID" || property.ToUpper().Trim() == "CODIGOCLIENTE" || property.ToUpper().Trim() == "ESTADO" )
                    continue;

                if (property.ToUpper().Trim() == "CODCLIENTE")
                {
                    Columns.Add(column => {
                        column.Caption = property;
                        column.SetDataItemTemplateContent(container =>
                        {
                            Html.DevExpress().HyperLink(hyperlink =>
                            {
                                var visibleIndex = container.VisibleIndex;
                                var keyValue = container.KeyValue;
                                var lastName = DataBinder.Eval(container.DataItem, "CodCliente");
                                var codCliente = DataBinder.Eval(container.DataItem, "CodigoCliente");

                                hyperlink.Name = "hl" + keyValue.ToString();
                                hyperlink.Properties.Text = lastName.ToString();
                                hyperlink.NavigateUrl = DevExpressHelper.GetUrl(new { Controller = "Cliente", Action = "View", id = codCliente });
                            }).Render();
                        });

                    });
                    //var style = new DevExpress.Web.GridViewHeaderStyle();
                    // style.BackColor = System.Drawing.Color.LightGray;
                    Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                    Columns[property].HeaderStyle.Font.Bold = true;
                    ((MVCxGridViewColumn)Columns[property]).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
                else
                {
                    Columns.Add(property);
                    //var style = new DevExpress.Web.GridViewHeaderStyle();
                    // style.BackColor = System.Drawing.Color.LightGray;
                    Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                    Columns[property].HeaderStyle.Font.Bold = true;
                    ((MVCxGridViewColumn)Columns[property]).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }

                //Columns.Add(property);

            }
        }

        public static void AddCustomColumnsSettingViewData(MVCxGridViewColumnCollection Columns, IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model,
            HtmlHelper Html, bool IsAdminOperador)
        {
            if (model.Count() == 0)
                return;
            foreach (var property in model.First().GetDynamicMemberNames())
            {
                if (property.ToUpper().Trim() == "ID" || property.ToUpper().Trim() == "DEVOL" 
                    || property.ToUpper().Trim() == "ESTADO" || property.ToUpper().Trim() == "HEXACOLOR"
                    || property.ToUpper().Trim() == "DESCESTADO")
                    continue;
                
                if (property.ToUpper().Trim() == "FECHASOLICITADARECEPCION" || property.ToUpper().Trim() == "FECHARECEPCION"
                    || property.ToUpper().Trim() == "CONF_FECHA" || property.ToUpper().Trim() == "FE_RETIRO" || property.ToUpper().Trim() == "FECHAFINPRODUCCION")
                {
                    
                    Columns.Add(property).PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
                   

                /*    Columns(property).FieldName = "AdmissionDate";
                    Columns.Caption = "Admission Date";
                    Columns.EditFormSettings.Visible = DefaultBoolean.True;
                    column.ColumnType = MVCxGridViewColumnType.DateEdit;
                    var dateProperties = column.PropertiesEdit as DateEditProperties;
                    dateProperties.AllowMouseWheel = true;
                    dateProperties.AllowUserInput = true;
                    dateProperties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip; */
                }
                else
                {
                    if (property.ToUpper().Trim() == "FECHAAUTORIZADARECEPCION")
                    {
                        if (IsAdminOperador)
                        {
                            Columns.Add(column =>
                            {
                                column.FieldName = property;
                                column.Caption = "Fecha Autorizada Recepcion";
                                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                                int count = 0;
                                column.SetDataItemTemplateContent(container =>
                                {
                                    Html.DevExpress().DateEdit(settings =>
                                    {
                                        var visibleIndex = container.VisibleIndex;
                                        //var keyValue = container.KeyValue;
                                        var keyValue = DataBinder.Eval(container.DataItem, "ID");
                                        var fecha = DataBinder.Eval(container.DataItem, "FechaAutorizadaRecepcion");
                                        settings.Width = System.Web.UI.WebControls.Unit.Pixel(90);
                                        settings.Name = "date" + keyValue.ToString() + count;
                                        if (fecha.ToString() != string.Empty)
                                        settings.Date = Convert.ToDateTime(fecha);

                                        settings.Properties.DisplayFormatString = "d";
                                        settings.Properties.NullText = "dd/MM/yyyy";
                                        settings.Properties.EditFormat = EditFormat.Custom;
                                        settings.Properties.EditFormatString = "dd/MM/yyyy";
                                        settings.Properties.DisplayFormatString = "dd/MM/yyyy";
                                        
                                        settings.Properties.ClientSideEvents.DateChanged = String.Format("function (s, e) {{ UpdatePedido(s, {0}); }}",
                                            Convert.ToInt32(keyValue));
                                        count++;
                                    }).Render();
                                });
                            });
                            var style = new DevExpress.Web.GridViewHeaderStyle();
                            Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                            Columns[property].HeaderStyle.Font.Bold = true;
                        }
                        else
                        {
                            Columns.Add(property);
                        }
                    }
                    else
                    {
                        if (property.ToUpper().Trim() == "CANTIDADENTREGADA" || property.ToUpper().Trim() == "CANTIDADRECIBIDA")
                        {
                            Columns.Add(property).PropertiesEdit.DisplayFormatString = "N0";
                        }
                        else
                        {
                            Columns.Add(property);
                        }
                    }
                }
               
                Columns[property].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                Columns[property].HeaderStyle.Font.Bold = true;
            }
        }

        public static void AddColumnsSettingViewData(MVCxGridViewColumnCollection Columns, IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model)
        {
            if(model.Count() > 0)
            {
                foreach (var property in model.First().GetDynamicMemberNames())
                {
                    if (property.ToUpper().Trim() == "ID" || property.ToUpper().Trim() == "ESTADO")
                        continue;


                    if (property.ToUpper().Trim().Contains("FECHA"))
                    {
                        Columns.Add(column =>
                        {
                            column.FieldName = property;
                            column.Caption = property;
                            column.ColumnType = MVCxGridViewColumnType.DateEdit;
                        });
                    }
                    else
                    {
                        Columns.Add(property);
                    }

                    if (property.ToUpper().Trim() == "DESCRIPCION")
                        Columns[property].Width = 350;

                    var style = new DevExpress.Web.GridViewHeaderStyle();
                    // style.BackColor = System.Drawing.Color.LightGray;
                    Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                    Columns[property].HeaderStyle.Font.Bold = true;
                    ((MVCxGridViewColumn)Columns[property]).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                } 
            }
        }

        public static void AddColumnsSettingViewDataPDF(MVCxGridViewColumnCollection Columns, IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model, bool Export)
        {
            if (model.Count() > 0)
            {
                foreach (var property in model.First().GetDynamicMemberNames())
                {
                    if (property.ToUpper().Trim() == "ID")
                        continue;

                    if (Export)
                    {
                        if (!(property.Trim() == "Observaciones" || property.Trim() == "Cg_Cli"
                            || property.Trim() == "Des_Cli"))
                        {
                                Columns.Add(property);
                                var style = new DevExpress.Web.GridViewHeaderStyle();
                                // style.BackColor = System.Drawing.Color.LightGray;
                                Columns[property].HeaderStyle.BackColor = System.Drawing.Color.LightGray;
                                Columns[property].HeaderStyle.Font.Bold = true;
                                if (property.Trim() == "CantidadPedida")
                                    Columns[property].Caption = "Cant.\n Pedida";

                                if (property.Trim() == "CantidadRecibida")
                                    Columns[property].Caption = "Cant.\n Recibida";

                                if (property.Trim() == "UnidadFac")
                                    Columns[property].Caption = "Unidad\nFac";

                            ((MVCxGridViewColumn)Columns[property]).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene el custom attribute de la propiedad a traves del nombre
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static System.Reflection.CustomAttributeData GetCustomAttributeByAttibute(System.Reflection.PropertyInfo property, string attributeName)
        {
            var namePropertyEntityAttribute = property.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name.Equals(attributeName));

            return namePropertyEntityAttribute;
        }

        /// <summary>
        /// Obtiene el valor del parametro del atributo decorador
        /// de la propiedad
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attributeName"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static string GetValueCustomAttributeByAttibuteAndParameter(System.Reflection.PropertyInfo property, string attributeName, string parameterName)
        {
            //var namePropertyEntityAttribute = property.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name.Equals(attributeName));

            var namePropertyEntityAttribute = GetCustomAttributeByAttibute(property, attributeName);

            string valueParameterCustomAttribute = string.Empty;
            if (namePropertyEntityAttribute != null)
            {
                valueParameterCustomAttribute = namePropertyEntityAttribute.NamedArguments.FirstOrDefault(a => a.MemberName.Equals(parameterName)).TypedValue.Value.ToString();
            }

            return valueParameterCustomAttribute;
        }

        /// <summary>
        /// GetPropertyName<State>(x => x.Name);
        /// </summary>
        /// <typeparam name="Ent"></typeparam>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static string GetPropertyName<Ent>(Expression<Func<Ent, Object>> propertySelector)
        {
            var nombre = propertySelector.ToString();
            string[] sep = { "." };
            var arr = nombre.Split(sep, StringSplitOptions.None);
            var l = arr.Last();

            var ret = "";

            int i = 0;
            while (i < l.Length && (char.IsLetterOrDigit(l[i]) || l[i] == '_'))
            {
                ret += l[i].ToString();
                i++;
            }

            return ret;
        }

        public static string GetValueProperty(object model, string nameProperty)
        {
            try
            {
                foreach (var p in model.GetType().GetProperties())
                {
                    if (!p.Name.Equals(nameProperty))
                        continue;

                    MethodInfo methodInfo = model.GetType().GetMethod("get_" + nameProperty);
                    var value = (string)methodInfo.Invoke(model, null);
                    return value;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public static bool IsPropertyForRendered(string property)
        {
            try
            {
               

                return  true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static GridViewSettings GetEspecialSettingExport(IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model, 
            string controllerName)
        {
            GridViewSettings settings = new GridViewSettings();


            settings.Name = "grid_" + controllerName;
            settings.CallbackRouteValues = new { Controller = controllerName, Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);

            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;

            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;

            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;
            
            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.Position = System.Web.UI.WebControls.PagerPosition.TopAndBottom;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            AddColumnsSettingViewData(settings.Columns, model);

            //Fila de filtros
            settings.Settings.ShowFilterRow = true;
            //Icono del menu del filtrado
            settings.Settings.ShowFilterRowMenu = false;

            //Setear por defecto la condicion de filtrado
            //de todas las columnas a Contains
            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };


            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }

        public static GridViewSettings GetSettingExport(IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model, string controllerName)
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "grid_" + controllerName;
            settings.CallbackRouteValues = new { Controller = controllerName, Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;

            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.KeyFieldName = "ID";
            settings.Columns.Add("ID");
            AddColumnsSettingViewData(settings.Columns, model);
            //AddColumnsSettingViewData(settings.Columns, model, null);

            settings.Settings.ShowFilterRow = true;
            settings.Settings.ShowFilterRowMenu = false;

            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };

            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsExport.Landscape = true;
            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }

        public static GridViewSettings GetSettingExportPDF(IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model, string controllerName, bool Export)
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "grid_" + controllerName;
            settings.CallbackRouteValues = new { Controller = controllerName, Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;

            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.KeyFieldName = "ID";

            if(!Export)
                settings.Columns.Add("ID");

            AddColumnsSettingViewDataPDF(settings.Columns, model, Export);

            settings.Settings.ShowFilterRow = true;
            settings.Settings.ShowFilterRowMenu = false;

            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };

            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsExport.Landscape = true;
            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }


        public static GridViewSettings GetSettingExportWithoutId(IEnumerable<CRM.Business.Views.BaseViews.BaseViewString.DynObject> model, string controllerName)
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "grid_" + controllerName;
            settings.CallbackRouteValues = new { Controller = controllerName, Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;

            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            AddColumnsSettingViewData(settings.Columns, model);
            //AddColumnsSettingViewData(settings.Columns, model, null);

            settings.Settings.ShowFilterRow = true;
            settings.Settings.ShowFilterRowMenu = false;

            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };

            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }

         /// <summary>
        /// Seteo para el export
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static GridViewSettings GetSettingExport(IEnumerable<CRM.Business.Views.BaseViews.BaseView.DynObject> model , string controllerName)
        {
            GridViewSettings settings = new GridViewSettings();
         

            settings.Name = "grid_" + controllerName;
            settings.CallbackRouteValues = new { Controller = controllerName, Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;
            
            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.KeyFieldName = "ID";
            settings.Columns.Add("ID");
            AddColumnsSettingViewData(settings.Columns, model);



            //Fila de filtros
            settings.Settings.ShowFilterRow = true;
            //Icono del menu del filtrado
            settings.Settings.ShowFilterRowMenu = false;

            //Setear por defecto la condicion de filtrado
            //de todas las columnas a Contains
            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };


            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }

        /// <summary>
        /// Seteo para el export
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static GridViewSettings GetSettingExport(Type model, string controllerName)
        {
            GridViewSettings settings = new GridViewSettings();

            settings.Name = "grid_" + controllerName;
            settings.CallbackRouteValues = new { Controller = controllerName, Action = "GridViewAllPartial" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsSearchPanel.Visible = true;
            settings.SettingsBehavior.AllowFixedGroups = true;
            settings.SettingsBehavior.AllowDragDrop = true;
            settings.SettingsBehavior.AllowSort = true;

            settings.SettingsCookies.StoreFiltering = true;
            settings.SettingsCookies.StoreGroupingAndSorting = true;
            settings.SettingsCookies.StorePaging = true;
            settings.SettingsCookies.Enabled = true;
            settings.CustomJSProperties = (s, e) =>
            {
                MVCxGridView gridView = (MVCxGridView)s;
                e.Properties["cpClientLayout"] = gridView.SaveClientLayout();
            };

            settings.CommandColumn.Visible = true;
            settings.Settings.UseFixedTableLayout = true;
            settings.CommandColumn.Width = 50;

            settings.CommandColumn.ShowClearFilterButton = true;
            //CheckBox en cada fila y checkbox general
            settings.CommandColumn.ShowSelectCheckbox = true;
            settings.CommandColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            settings.CommandColumn.HeaderStyle.BackColor = System.Drawing.Color.Orange;

            settings.SettingsBehavior.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.KeyFieldName = "Id";
            settings.Columns.Add("Id");
            AddColumnsSetting(settings.Columns, model);

            //Fila de filtros
            settings.Settings.ShowFilterRow = true;
            //Icono del menu del filtrado
            settings.Settings.ShowFilterRowMenu = false;

            //Setear por defecto la condicion de filtrado
            //de todas las columnas a Contains
            settings.DataBound = (sender, e) =>
            {
                MVCxGridView gv = sender as MVCxGridView;
                gv.Visible = (gv.VisibleRowCount > 0);

                foreach (GridViewColumn column in gv.Columns)
                {
                    var dataColumn = column as GridViewDataColumn;
                    if (dataColumn != null)
                        dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
            };


            settings.PreRender = (s, e) =>
            {
                MVCxGridView grid = s as MVCxGridView;
                if (grid != null)
                    grid.ExpandAll();
            };

            settings.SettingsDetail.ExportMode = GridViewDetailExportMode.Expanded;
            return settings;
        }
        
        public static void SetCellData(object sender, ASPxGridViewTableDataCellEventArgs e, string controller)
        {
            MVCxGridView gv = sender as MVCxGridView;
            gv.Visible = (gv.VisibleRowCount > 0);

            //ListadoRepository listadoRepository = new ListadoRepository();
            //PropiedadNavegacionListadoRepository propiedadNavegacionRepository = new PropiedadNavegacionListadoRepository();

            Listado listado = AppSession.Listados.Where(x => x.Nombre.ToUpper().StartsWith(controller.ToUpper())).FirstOrDefault();//listadoRepository.GetFiltered(x => x.Nombre.ToUpper().StartsWith(controller.ToUpper())).FirstOrDefault();

            vClientes vClientes = new Business.Views.vClientes();
           
            foreach (GridViewColumn column in gv.Columns)
            {
                var dataColumn = column as GridViewDataColumn;

                if (dataColumn != null && listado != null)
                {
                    //if (propiedadNavegacionRepository.GetFiltered(x => x.Listado_Id == listado.Id && x.CampoFK.ToUpper().Equals(e.DataColumn.FieldName.ToUpper())).Any())
                    if (AppSession.PropiedadesNavegacion.Where(x => x.Listado_Id == listado.Id && x.CampoFK.ToUpper().Equals(e.DataColumn.FieldName.ToUpper())).Any())
                    {
                        if (e.DataColumn.FieldName.ToUpper() == "CODIGOCLIENTE")
                        {
                            int idCliente = Convert.ToInt32(e.CellValue);
                            DataTable data = vClientes.GetByID(idCliente);
                            if (data.Rows.Count > 0)
                            {
                                e.Cell.Text = "<a href='/Cliente/View/" + idCliente + "' title='Ver Cliente'>" + data.Rows[0]["Descripcion"].ToString() + "</a>";
                            }
                            else
                            {
                                e.Cell.Text = "Cliente Inexistente";
                            }
                        }

                        if (e.DataColumn.FieldName.ToUpper() == "PRO_COD")
                        {
                            vProductos vProductos = new Business.Views.vProductos();
                            vProductos.Datos = vProductos.GetEmpyViewModel();
                            string idProducto = e.CellValue.ToString();
                            DataTable data = vProductos.GetByID(idProducto);
                            if (data.Rows.Count > 0)
                            {
                                e.Cell.Text = "<a href='/Producto/View/" + idProducto + "' title='Ver Producto'>" + data.Rows[0]["Descripcion"].ToString() + "</a>";
                            }
                            else {
                                e.Cell.Text = "Producto Inexistente";
                            }
                        }
                    }
                }
            }
        }

        public static void SetHeaderColumnData(object sender, EventArgs e, string controller)
        {
            MVCxGridView gv = sender as MVCxGridView;
            gv.Visible = (gv.VisibleRowCount > 0);

            //CRM.Business.DAL.ListadoRepository listadoRepository = new CRM.Business.DAL.ListadoRepository();
            //CRM.Business.DAL.PropiedadNavegacionListadoRepository propiedadNavegacionRepository = new CRM.Business.DAL.PropiedadNavegacionListadoRepository();

            CRM.Business.Entities.Listado listado = AppSession.Listados.Where(x => x.Nombre.ToUpper().StartsWith(controller.ToUpper())).FirstOrDefault();
            //listadoRepository.GetFiltered(x => x.Nombre.ToUpper().StartsWith(controller.ToUpper())).FirstOrDefault();
            foreach (GridViewColumn column in gv.Columns)
            {
                var dataColumn = column as GridViewDataColumn;
                if (dataColumn != null && listado != null)
                {
                    dataColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;

                    //if (propiedadNavegacionRepository.GetFiltered(x => x.Listado_Id == listado.Id && x.CampoFK.ToUpper().Equals(dataColumn.FieldName.ToUpper())).Any())
                    if (AppSession.PropiedadesNavegacion.Where(x => x.Listado_Id == listado.Id && x.CampoFK.ToUpper().Equals(dataColumn.FieldName.ToUpper())).Any())
                    {
                        if (dataColumn.FieldName.ToUpper() == "CODIGOCLIENTE")
                        {
                            column.Caption = "Cliente";
                        }

                        if (dataColumn.FieldName.ToUpper() == "PRO_COD")
                        {
                            column.Caption = "Producto";
                        }
                    }
                }
            }
        }

        
    }

}

