using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web.Mvc;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Website.DevExpressHelpers
{
    public class PivotGridHelper
    {
        static PivotGridSettings pivotGridSettings;
        const string olapConexionString = @"provider=MSOLAP;data source=IONICS-2K3\CUBOS;initial catalog=Ventas;cube name=Solutiion;";

        public static string OlapConexionString
        {
            get {return olapConexionString;}
        }

        public static PivotGridSettings PivotGridSettings
        {
            get
            {
                if (pivotGridSettings == null)
                    pivotGridSettings = CreatePivotGridSettings();
                return pivotGridSettings;
            }
        }
        public static PivotGridSettings PivotCustomCallbackSettings
        {
            get
            {
                PivotGridSettings settings = PivotGridSettings;
                settings.BeforeGetCallbackResult = (sender, e) =>
                {
                    var field = ((MVCxPivotGrid)sender).Fields["ProductName"];
                    object[] values = field.GetUniqueValues();
                    field.FilterValues.ValuesIncluded = values;
                };
                return settings;
            }
        }



        static PivotGridSettings CreatePivotGridSettings()
        {

            PivotGridSettings settings = new PivotGridSettings();
            settings.Name = "PivotGrid";
            settings.CallbackRouteValues = new { Controller = "Cubo", Action = "PivotGridPartial" };
            //settings.CustomActionRouteValues = new { Controller = "Home", Action = "PivotGridCustomCallback" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.OptionsChartDataSource.ProvideDataByColumns = false;
            settings.CustomizationFieldsLeft = 600;
            settings.CustomizationFieldsTop = 400;
            settings.OptionsView.HorizontalScrollBarMode = ScrollBarMode.Auto;
            settings.OptionsView.VerticalScrollingMode = PivotScrollingMode.Virtual;
            settings.OptionsView.HorizontalScrollingMode = PivotScrollingMode.Virtual;
            settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;

            settings.OptionsPager.RowsPerPage = 25;
            settings.OptionsPager.ColumnsPerPage = 15;
            settings.OptionsPager.Visible = false;
            settings.OptionsFilter.NativeCheckBoxes = false;
            settings.OptionsView.ShowColumnGrandTotals = true;

            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 0;
                field.Caption = "Cliente";
                field.FieldName = "[Clientes].[Cliente].[Cliente]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 1;
                field.Caption = "Provincia";
                field.FieldName = "[Provincia].[Provincia].[Provincia]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 2;
                field.Caption = "Producto";
                field.FieldName = "[Producto].[Producto].[Producto]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 3;
                field.Caption = "Unid Fact";
                field.FieldName = "[Unidad de Facturacion].[Unid Fac].[Unid Fac]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 4;
                field.Caption = "Categoría";
                field.FieldName = "[Categorías de Cliente].[Categoría].[Categoría]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 5;
                field.Caption = "Pais";
                field.FieldName = "[País].[Pais].[Pais]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 6;
                field.Caption = "Proceso";
                field.FieldName = "[Proceso].[Proceso].[Proceso]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.FilterArea;
                field.AreaIndex = 7;
                field.Caption = "Rubro";
                field.FieldName = "[Rubro].[Rubro].[Rubro]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.ColumnArea;
                field.AreaIndex = 0;
                field.Caption = "Unidad de Produccion";
                field.FieldName = "[Unidad de Produccion].[Unid Prod].[Unid Prod]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.AreaIndex = 0;
                field.Caption = "Año";
                field.FieldName = "[Fecha].[Jerarquía].[Año]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.AreaIndex = 1;
                field.Caption = "Mes";
                field.FieldName = "[Fecha].[Jerarquía].[Mes]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.RowArea;
                field.AreaIndex = 2;
                field.Caption = "Dia";
                field.FieldName = "[Fecha].[Jerarquía].[Dia]";
                field.SortMode = PivotSortMode.None;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.DataArea;
                field.AreaIndex = 0;
                field.Caption = "Importe";
                field.FieldName = "[Measures].[Importe Tot Pesos]";
                field.CellFormat.FormatString = "$#.##";
                field.CellFormat.FormatType = FormatType.Custom;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.DataArea;
                field.AreaIndex = 1;
                field.Caption = "Precio Unit";
                field.FieldName = "[Measures].[Precio Unit Pesos]";
                field.CellFormat.FormatString = "${0:f2}";
                field.CellFormat.FormatType = FormatType.Custom;
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.DataArea;
                field.AreaIndex = 2;
                field.Caption = "Vol Equivalente";
                field.FieldName = "[Measures].[Vol Equivalente]";
                field.CellFormat.FormatString = "{0:f2}";
                field.CellFormat.FormatType = FormatType.Custom;
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.DataArea;
                field.AreaIndex = 3;
                field.Caption = "Unid Fisicas";
                field.FieldName = "[Measures].[Unidades Fisicas]";
                field.CellFormat.FormatString = "{0:f2}";
                field.CellFormat.FormatType = FormatType.Custom;
                field.Visible = false;
            });
            settings.Fields.Add(field =>
            {
                field.Area = PivotArea.DataArea;
                field.AreaIndex = 4;
                field.Caption = "Precio Unit Promedio";
                field.FieldName = "[Measures].[Precio Unitario Promedio]";
                field.CellFormat.FormatString = "{0:f2}";
                field.CellFormat.FormatType = FormatType.Custom;
                field.Visible = false;
            });
            //settings.Name = "pivotGrid";
            //settings.CallbackRouteValues = new { Controller = "Home", Action = "PivotGridPartial" };
            //settings.CustomActionRouteValues = new { Controller = "Home", Action = "PivotGridCustomCallback" };
            //settings.OptionsView.HorizontalScrollBarMode = DevExpress.Web.ScrollBarMode.Auto;
            //settings.OptionsChartDataSource.ProvideDataByColumns = false;
            //settings.Width = new System.Web.UI.WebControls.Unit(90, System.Web.UI.WebControls.UnitType.Percentage);

            //settings.ClientSideEvents.BeginCallback = "OnBeforePivotGridCallback";
            //settings.ClientSideEvents.EndCallback = "UpdateChart";


            //settings.Groups.Add("Order Date");
            //settings.Fields.Add("Country", PivotArea.FilterArea);
            //settings.Fields.Add("City", PivotArea.FilterArea);
            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.ColumnArea;
            //    field.FieldName = "OrderDate";
            //    field.GroupInterval = PivotGroupInterval.DateYear;
            //    field.Caption = "Year";
            //    field.GroupIndex = 0;
            //});
            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.ColumnArea;
            //    field.FieldName = "OrderDate";
            //    field.GroupInterval = PivotGroupInterval.DateMonth;
            //    field.Caption = "Month";
            //    field.GroupIndex = 0;
            //    field.InnerGroupIndex = 1;
            //});

            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.DataArea;
            //    field.FieldName = "Quantity";
            //    field.Visible = false;
            //});
            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.DataArea;
            //    field.FieldName = "UnitPrice";
            //    field.Visible = false;
            //});
            //settings.Fields.Add(field =>
            //{
            //    field.Area = PivotArea.DataArea;
            //    field.FieldName = "Amount";
            //    field.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            //    field.UnboundExpression = "[UnitPrice]*[Quantity]";
            //    field.Visible = true;
            //});
            //settings.Fields.Add("ProductName", PivotArea.RowArea);
            //settings.PreRender = (sender, e) =>
            //{
            //    ((MVCxPivotGrid)sender).CollapseAll();
            //    var field = ((MVCxPivotGrid)sender).Fields["ProductName"];
            //    object[] values = field.GetUniqueValues();
            //    field.FilterValues.ValuesIncluded = values.Where(name => name.ToString().StartsWith("N")).ToArray();

            //};

            //settings.CustomFieldValueCells = ()

            settings.ClientSideEvents.EndCallback = "UpdateChart";
            return settings;
        }
    }
}