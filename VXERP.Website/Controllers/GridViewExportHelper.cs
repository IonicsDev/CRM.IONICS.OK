using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Data;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.Web;

namespace CRM.Website.Controllers
{
    public enum GridViewExportFormat { None, Pdf, Xls, Xlsx, Rtf, Csv }
    public delegate ActionResult GridViewExportMethod(GridViewSettings settings, object dataObject);

   
    public class GridViewExportHelper<TEntity>
        where TEntity : CRM.Business.Entities.BaseEntities.BaseEntity, new()
    {
        public static string controllerName { get; set; }

        static string ExcelDataAwareGridViewSettingsKey = "51172A18-2073-426B-A5CB-136347B3A79F";

        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateDataAwareExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Xls, GridViewExtension.ExportToXls },
                { GridViewExportFormat.Xlsx, GridViewExtension.ExportToXlsx },
                { GridViewExportFormat.Csv, GridViewExtension.ExportToCsv }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> exportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> ExportFormatsInfo
        {
            get
            {
                if (exportFormatsInfo == null)
                    exportFormatsInfo = CreateExportFormatsInfo();
                return exportFormatsInfo;
            }
        }

        static IDictionary Context { get { return System.Web.HttpContext.Current.Items; } }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> CreateExportFormatsInfo()
        {
            return new Dictionary<GridViewExportFormat, GridViewExportMethod> {
                { GridViewExportFormat.Pdf, GridViewExtension.ExportToPdf },
                {
                    GridViewExportFormat.Xls,
                    (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                { 
                    GridViewExportFormat.Xlsx,
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                { GridViewExportFormat.Rtf, GridViewExtension.ExportToRtf },
                {
                    GridViewExportFormat.Csv,
                    (settings, data) => GridViewExtension.ExportToCsv(settings, data, new CsvExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                }
            };
        }

        static Dictionary<GridViewExportFormat, GridViewExportMethod> dataAwareExportFormatsInfo;
        public static Dictionary<GridViewExportFormat, GridViewExportMethod> DataAwareExportFormatsInfo
        {
            get
            {
                if (dataAwareExportFormatsInfo == null)
                    dataAwareExportFormatsInfo = CreateDataAwareExportFormatsInfo();
                return dataAwareExportFormatsInfo;
            }
        }

        public static GridViewSettings ExcelDataAwareExportGridViewSettings
        {
            get
            {
                GridViewSettings settings = Context[ExcelDataAwareGridViewSettingsKey] as GridViewSettings;
                if (settings == null)
                    Context[ExcelDataAwareGridViewSettingsKey] = settings = CreateExcelDataAwareExportGridViewSettings();
                return settings;
            }
        }


        static GridViewSettings CreateExcelDataAwareExportGridViewSettings()
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

            settings.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;

            settings.Settings.ShowGroupPanel = true;
            settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            settings.Settings.VerticalScrollableHeight = 300;

            settings.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };

            settings.KeyFieldName = "Id";

            CRM.Website.DevExpressHelpers.GridHelper.AddColumnsSetting(settings.Columns, typeof(TEntity));

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
    }
}