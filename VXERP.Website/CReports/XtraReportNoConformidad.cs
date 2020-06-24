using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for XtraReportEventualidad
/// </summary>
public class XtraReportNoConformidad : DevExpress.XtraReports.UI.XtraReport
{
    private XRControlStyle xrControlStyle1;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private TopMarginBand topMarginBand1;
    private BottomMarginBand bottomMarginBand1;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private DetailBand detailBand1;
    private XRControlStyle Title;
    private XRControlStyle DetailCaption1;
    private XRControlStyle DetailData1;
    private XRControlStyle DetailCaption3;
    private XRControlStyle DetailData3;
    private XRControlStyle DetailData3_Odd;
    private XRControlStyle DetailCaptionBackground3;
    private XRControlStyle PageInfo;
    private XRLabel xrLabel4;
    private XRLine xrLine1;
    private XRPictureBox xrPictureBox1;
    private XRLine xrLine2;
    private XRLabel xrLabel11;
    private XRLabel xrLabel17;
    private XRLine xrLine7;
    private XRLabel xrLabel15;
    private XRLabel xrLabel9;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    private DevExpress.XtraReports.Parameters.Parameter Acontecimiento_Id;
    private XRLabel xrLabel5;
    private XRLabel xrLabel3;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private XRLabel xrLabel8;
    private SubBand sbAccionesInmediatas;
    private XRLabel xrLabel31;
    private XRLabel xrLabel12;
    private SubBand sbAccionesCorrectivas;
    private XRLabel xrLabel13;
    private XRLabel xrLabel14;
    private SubBand sbAccionesMejora;
    private XRLabel xrLabel16;
    private XRLabel xrLabel19;
    private SubBand sbAccionesOtras;
    private XRLabel xrLabel21;
    private XRLabel xrLabel22;
    private SubBand sbSinAcciones;
    private XRLabel xrLabel23;
    private XRLabel xrLabel24;
    private SubBand sbDescripcionAcciones;
    private XRLabel xrLabel25;
    private XRLabel xrLabel26;
    private XRLabel xrLabel18;
    private XRLabel xrLabel27;
    private PageFooterBand pfFirmas;
    private XRPictureBox xrPictureBox3;
    private XRPictureBox xrPictureBoxGerenteGeneral;
    private XRLabel xrLabel32;
    private DevExpress.XtraReports.Parameters.Parameter bFirmaGteGral;
    private XRLine xrLine3;
    private XRLine xrLine5;
    private XRLine xrLine4;
    private XRLine xrLine8;
    private XRLine xrLine6;
    private XRLine xrLine10;
    private XRLine xrLine9;
    private XRLine xrLine11;
    private XRLine xrLine14;
    private XRLine xrLine13;
    private XRLine xrLine12;
    private XRLine xrLine15;
    private XRLabel xrLabel10;
    private XRLine xrLine18;
    private XRLine xrLine17;
    private XRLine xrLine16;
    private XRLine xrLine21;
    private XRLine xrLine20;
    private XRLine xrLine19;
    private XRLine xrLine22;
    private XRLine xrLine26;
    private XRLine xrLine25;
    private XRLine xrLine24;
    private XRLine xrLine23;
    private XRLine xrLine30;
    private XRLine xrLine29;
    private XRLine xrLine28;
    private XRLine xrLine27;
    private XRLine xrLine34;
    private XRLine xrLine33;
    private XRLine xrLine32;
    private XRLine xrLine31;
    private XRLine xrLine38;
    private XRLine xrLine37;
    private XRLine xrLine36;
    private XRLine xrLine35;
    private SubBand sbFechaImplementacion;
    private XRLine xrLine42;
    private XRLine xrLine41;
    private XRLine xrLine39;
    private XRLine xrLine40;
    private XRLabel xrLabel34;
    private XRLabel xrLabel33;
    private SubBand SubBand1;
    private XRLine xrLine45;
    private XRLine xrLine44;
    private XRLine xrLine43;
    private XRLine xrLine46;
    private XRLine xrLine47;
    private XRLabel xrLabel36;
    private XRLabel xrLabel35;
    private SubBand sbNuevaAccion;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private XRLine xrLine51;
    private XRLine xrLine50;
    private XRLine xrLine49;
    private XRLine xrLine48;
    private SubBand SubBand2;
    private XRLabel xrLabel20;
    private XRLabel xrLabel30;
    private XRLine xrLine55;
    private XRLine xrLine54;
    private XRLine xrLine53;
    private XRLine xrLine52;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public XtraReportNoConformidad()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraReportNoConformidad));
            DevExpress.DataAccess.Sql.MasterDetailInfo masterDetailInfo1 = new DevExpress.DataAccess.Sql.MasterDetailInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo relationColumnInfo1 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.MasterDetailInfo masterDetailInfo2 = new DevExpress.DataAccess.Sql.MasterDetailInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo relationColumnInfo2 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.MasterDetailInfo masterDetailInfo3 = new DevExpress.DataAccess.Sql.MasterDetailInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo relationColumnInfo3 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.MasterDetailInfo masterDetailInfo4 = new DevExpress.DataAccess.Sql.MasterDetailInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo relationColumnInfo4 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.MasterDetailInfo masterDetailInfo5 = new DevExpress.DataAccess.Sql.MasterDetailInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo relationColumnInfo5 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.MasterDetailInfo masterDetailInfo6 = new DevExpress.DataAccess.Sql.MasterDetailInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo relationColumnInfo6 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLine11 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine10 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine6 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLine7 = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLine18 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine17 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine16 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbAccionesInmediatas = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine15 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine14 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine13 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine12 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbAccionesCorrectivas = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine22 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine21 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine20 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine19 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbAccionesMejora = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine26 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine25 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine24 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine23 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbAccionesOtras = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine30 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine29 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine28 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine27 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbSinAcciones = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine34 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine33 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine32 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine31 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbDescripcionAcciones = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine38 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine37 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine36 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine35 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbFechaImplementacion = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine42 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine41 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine39 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine40 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.SubBand1 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine47 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine46 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine45 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine44 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine43 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbNuevaAccion = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine51 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine50 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine49 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine48 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.SubBand2 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine55 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine54 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine53 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine52 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaptionBackground3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.Acontecimiento_Id = new DevExpress.XtraReports.Parameters.Parameter();
            this.pfFirmas = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPictureBoxGerenteGeneral = new DevExpress.XtraReports.UI.XRPictureBox();
            this.bFirmaGteGral = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.Name = "xrControlStyle1";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "EntitiesConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "ListAcontecimientos";
            queryParameter1.Name = "Id";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.Acontecimiento_Id]", typeof(int));
            queryParameter2.Name = "FirmaGerenteComercial";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("[Parameters.bFirmaGerenteComercial]", typeof(bool));
            customSqlQuery1.Parameters.Add(queryParameter1);
            customSqlQuery1.Parameters.Add(queryParameter2);
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            masterDetailInfo1.DetailQueryName = "SubTiposEventualidad";
            relationColumnInfo1.NestedKeyColumn = "Id";
            relationColumnInfo1.ParentKeyColumn = "SubTipoEventualidad_Id";
            masterDetailInfo1.KeyColumns.Add(relationColumnInfo1);
            masterDetailInfo1.MasterQueryName = "NoConformidad";
            masterDetailInfo2.DetailQueryName = "TiposEventualidad";
            relationColumnInfo2.NestedKeyColumn = "Id";
            relationColumnInfo2.ParentKeyColumn = "TipoEventualidad_Id";
            masterDetailInfo2.KeyColumns.Add(relationColumnInfo2);
            masterDetailInfo2.MasterQueryName = "SubTiposEventualidad";
            masterDetailInfo3.DetailQueryName = "Usuarios";
            relationColumnInfo3.NestedKeyColumn = "Id";
            relationColumnInfo3.ParentKeyColumn = "IdUsuario";
            masterDetailInfo3.KeyColumns.Add(relationColumnInfo3);
            masterDetailInfo3.MasterQueryName = "NoConformidad";
            masterDetailInfo4.DetailQueryName = "v_Clientes";
            relationColumnInfo4.NestedKeyColumn = "ID";
            relationColumnInfo4.ParentKeyColumn = "Cg_Clie";
            masterDetailInfo4.KeyColumns.Add(relationColumnInfo4);
            masterDetailInfo4.MasterQueryName = "NoConformidad";
            masterDetailInfo5.DetailQueryName = "ListPedidos";
            relationColumnInfo5.NestedKeyColumn = "ID";
            relationColumnInfo5.ParentKeyColumn = "Pedido_Id";
            masterDetailInfo5.KeyColumns.Add(relationColumnInfo5);
            masterDetailInfo5.MasterQueryName = "NoConformidad";
            masterDetailInfo6.DetailQueryName = "v_Remito";
            relationColumnInfo6.NestedKeyColumn = "Pedido";
            relationColumnInfo6.ParentKeyColumn = "Pedido_Id";
            masterDetailInfo6.KeyColumns.Add(relationColumnInfo6);
            masterDetailInfo6.MasterQueryName = "NoConformidad";
            this.sqlDataSource1.Relations.AddRange(new DevExpress.DataAccess.Sql.MasterDetailInfo[] {
            masterDetailInfo1,
            masterDetailInfo2,
            masterDetailInfo3,
            masterDetailInfo4,
            masterDetailInfo5,
            masterDetailInfo6});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.BorderColor = System.Drawing.Color.BlanchedAlmond;
            this.topMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine11,
            this.xrLine10,
            this.xrLine9,
            this.xrLine8,
            this.xrLine6,
            this.xrLine5,
            this.xrLine4,
            this.xrLine3,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel3,
            this.xrLabel9,
            this.xrLabel2,
            this.xrLabel1,
            this.xrLabel15,
            this.xrLabel17,
            this.xrLabel11,
            this.xrLine2,
            this.xrPictureBox1,
            this.xrLine1,
            this.xrLabel4});
            this.topMarginBand1.HeightF = 342F;
            this.topMarginBand1.Name = "topMarginBand1";
            this.topMarginBand1.StylePriority.UseBorderColor = false;
            // 
            // xrLine11
            // 
            this.xrLine11.ForeColor = System.Drawing.Color.Blue;
            this.xrLine11.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine11.LocationFloat = new DevExpress.Utils.PointFloat(0F, 251.5244F);
            this.xrLine11.Name = "xrLine11";
            this.xrLine11.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine11.StylePriority.UseForeColor = false;
            // 
            // xrLine10
            // 
            this.xrLine10.ForeColor = System.Drawing.Color.Blue;
            this.xrLine10.LocationFloat = new DevExpress.Utils.PointFloat(627.6712F, 52.3339F);
            this.xrLine10.Name = "xrLine10";
            this.xrLine10.SizeF = new System.Drawing.SizeF(118.329F, 2.083332F);
            this.xrLine10.StylePriority.UseForeColor = false;
            // 
            // xrLine9
            // 
            this.xrLine9.ForeColor = System.Drawing.Color.Blue;
            this.xrLine9.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(627.6712F, 1.589457E-05F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(2.083313F, 130.7732F);
            this.xrLine9.StylePriority.UseForeColor = false;
            // 
            // xrLine8
            // 
            this.xrLine8.ForeColor = System.Drawing.Color.Blue;
            this.xrLine8.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(1.99999F, 1.589457E-05F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine8.StylePriority.UseForeColor = false;
            // 
            // xrLine6
            // 
            this.xrLine6.ForeColor = System.Drawing.Color.Blue;
            this.xrLine6.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine6.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 161.9633F);
            this.xrLine6.Name = "xrLine6";
            this.xrLine6.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine6.StylePriority.UseForeColor = false;
            // 
            // xrLine5
            // 
            this.xrLine5.ForeColor = System.Drawing.Color.Blue;
            this.xrLine5.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(745.9167F, 2.083349F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(2.083313F, 339.4166F);
            this.xrLine5.StylePriority.UseForeColor = false;
            // 
            // xrLine4
            // 
            this.xrLine4.ForeColor = System.Drawing.Color.Blue;
            this.xrLine4.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(9.536743E-05F, 0F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(2.083336F, 341.5F);
            this.xrLine4.StylePriority.UseForeColor = false;
            // 
            // xrLine3
            // 
            this.xrLine3.ForeColor = System.Drawing.Color.Blue;
            this.xrLine3.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(99.43516F, 132.8565F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(2.083336F, 208.6434F);
            this.xrLine3.StylePriority.UseForeColor = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Raiz_Mejora_Cambio]")});
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 259.375F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(629.1575F, 72.625F);
            this.xrLabel8.Text = "xrLabel8";
            // 
            // xrLabel7
            // 
            this.xrLabel7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([TipoAcontecimiento]== \'No Conformidad\', \'Causas Raíz:\',\n\tIif([TipoAcontecimi" +
                    "ento]== \'Oportunidad de Mejora\', \'Origen de la Mejora\', \'Origen del Cambio\')\n )")});
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(21.45834F, 259.375F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(77.97687F, 35.35681F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Analisis del Evento:";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Fecha Apertura ]")});
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(647.4628F, 82.63435F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(82.29169F, 23F);
            this.xrLabel6.Text = "xrLabel6";
            this.xrLabel6.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrLabel6.Visible = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Descripcion]")});
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 174.448F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(626.1481F, 77.0764F);
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Origen]")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 132.8565F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(626.1482F, 17.64851F);
            this.xrLabel3.Text = "xrLabel3";
            // 
            // xrLabel9
            // 
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ListAcontecimientos].[Fecha Ocurrencia ]")});
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(647.4628F, 30.68517F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(82.29169F, 23F);
            this.xrLabel9.Text = "xrLabel9";
            this.xrLabel9.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // xrLabel2
            // 
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ListAcontecimientos].[ID]")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(328.4375F, 82.63434F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(227.0833F, 36.54166F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ListAcontecimientos].[TipoAcontecimiento]")});
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(229.5833F, 46.09267F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(383.5046F, 36.54167F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "xrLabel1";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(45.41663F, 132.8565F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(54.01853F, 17.6485F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "Origen:";
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(629.7545F, 61.80105F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(106.2455F, 20.8333F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Fecha Apertura";
            this.xrLabel17.Visible = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(636.7457F, 10.00001F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(109.2545F, 20.68516F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "Fecha Ocurrencia";
            // 
            // xrLine2
            // 
            this.xrLine2.ForeColor = System.Drawing.Color.Blue;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 130.7732F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine2.StylePriority.UseForeColor = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 6.070007F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(206.75F, 110.5926F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLine1
            // 
            this.xrLine1.ForeColor = System.Drawing.Color.Blue;
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(9.536743E-05F, 339.4167F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(748.0001F, 2.083344F);
            this.xrLine1.StylePriority.UseForeColor = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(21.45834F, 174.448F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(77.97687F, 14.52345F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Descripcion:";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine7,
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.bottomMarginBand1.HeightF = 49F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // xrLine7
            // 
            this.xrLine7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([AccionesOtras] == True, True , False)")});
            this.xrLine7.ForeColor = System.Drawing.Color.Blue;
            this.xrLine7.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine7.LocationFloat = new DevExpress.Utils.PointFloat(2.083365F, 9.999974F);
            this.xrLine7.Name = "xrLine7";
            this.xrLine7.SizeF = new System.Drawing.SizeF(743.9167F, 2.083334F);
            this.xrLine7.StylePriority.UseForeColor = false;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(586.125F, 25.24998F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(159.875F, 16.75002F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrPageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(2.083349F, 25.24999F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(97.35185F, 16.75002F);
            this.xrPageInfo1.StyleName = "PageInfo";
            this.xrPageInfo1.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine18,
            this.xrLine17,
            this.xrLine16,
            this.xrLabel10});
            this.detailBand1.HeightF = 45.375F;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.sbAccionesInmediatas,
            this.sbAccionesCorrectivas,
            this.sbAccionesMejora,
            this.sbAccionesOtras,
            this.sbSinAcciones,
            this.sbDescripcionAcciones,
            this.sbFechaImplementacion,
            this.SubBand1,
            this.sbNuevaAccion,
            this.SubBand2});
            // 
            // xrLine18
            // 
            this.xrLine18.ForeColor = System.Drawing.Color.Blue;
            this.xrLine18.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine18.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine18.Name = "xrLine18";
            this.xrLine18.SizeF = new System.Drawing.SizeF(2.083435F, 43.29166F);
            this.xrLine18.StylePriority.UseForeColor = false;
            // 
            // xrLine17
            // 
            this.xrLine17.ForeColor = System.Drawing.Color.Blue;
            this.xrLine17.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine17.LocationFloat = new DevExpress.Utils.PointFloat(0.0001827876F, 43.29166F);
            this.xrLine17.Name = "xrLine17";
            this.xrLine17.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine17.StylePriority.UseForeColor = false;
            // 
            // xrLine16
            // 
            this.xrLine16.ForeColor = System.Drawing.Color.Blue;
            this.xrLine16.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine16.LocationFloat = new DevExpress.Utils.PointFloat(745.9166F, 0F);
            this.xrLine16.Name = "xrLine16";
            this.xrLine16.SizeF = new System.Drawing.SizeF(2.083435F, 45.375F);
            this.xrLine16.StylePriority.UseForeColor = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(276.6667F, 4.124991F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(218.2084F, 31.25F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Acciones";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // sbAccionesInmediatas
            // 
            this.sbAccionesInmediatas.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine15,
            this.xrLine14,
            this.xrLine13,
            this.xrLine12,
            this.xrLabel31,
            this.xrLabel12});
            this.sbAccionesInmediatas.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([AccionesInmediatas] == True, True, False )")});
            this.sbAccionesInmediatas.HeightF = 82.78561F;
            this.sbAccionesInmediatas.Name = "sbAccionesInmediatas";
            this.sbAccionesInmediatas.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbAccionesInmediatas_BeforePrint);
            // 
            // xrLine15
            // 
            this.xrLine15.ForeColor = System.Drawing.Color.Blue;
            this.xrLine15.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine15.LocationFloat = new DevExpress.Utils.PointFloat(99.435F, 0F);
            this.xrLine15.Name = "xrLine15";
            this.xrLine15.SizeF = new System.Drawing.SizeF(2.083435F, 80.70227F);
            this.xrLine15.StylePriority.UseForeColor = false;
            // 
            // xrLine14
            // 
            this.xrLine14.ForeColor = System.Drawing.Color.Blue;
            this.xrLine14.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine14.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 0F);
            this.xrLine14.Name = "xrLine14";
            this.xrLine14.SizeF = new System.Drawing.SizeF(2.083435F, 80.70227F);
            this.xrLine14.StylePriority.UseForeColor = false;
            // 
            // xrLine13
            // 
            this.xrLine13.ForeColor = System.Drawing.Color.Blue;
            this.xrLine13.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine13.LocationFloat = new DevExpress.Utils.PointFloat(0F, 80.70227F);
            this.xrLine13.Name = "xrLine13";
            this.xrLine13.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine13.StylePriority.UseForeColor = false;
            // 
            // xrLine12
            // 
            this.xrLine12.ForeColor = System.Drawing.Color.Blue;
            this.xrLine12.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine12.LocationFloat = new DevExpress.Utils.PointFloat(745.9167F, 0F);
            this.xrLine12.Name = "xrLine12";
            this.xrLine12.SizeF = new System.Drawing.SizeF(2.083435F, 80.70227F);
            this.xrLine12.StylePriority.UseForeColor = false;
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(2.083429F, 0F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(97.3518F, 44.70231F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "Acciones Inmediatas:";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel12
            // 
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionAccionesInmediatas]")});
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 2.083333F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(629.1575F, 78.61893F);
            this.xrLabel12.Text = "xrLabel12";
            // 
            // sbAccionesCorrectivas
            // 
            this.sbAccionesCorrectivas.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine22,
            this.xrLine21,
            this.xrLine20,
            this.xrLine19,
            this.xrLabel13,
            this.xrLabel14});
            this.sbAccionesCorrectivas.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([AccionesCorrectivas] == True, True , False )")});
            this.sbAccionesCorrectivas.HeightF = 80.75269F;
            this.sbAccionesCorrectivas.Name = "sbAccionesCorrectivas";
            // 
            // xrLine22
            // 
            this.xrLine22.ForeColor = System.Drawing.Color.Blue;
            this.xrLine22.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine22.LocationFloat = new DevExpress.Utils.PointFloat(99.435F, 0F);
            this.xrLine22.Name = "xrLine22";
            this.xrLine22.SizeF = new System.Drawing.SizeF(2.083328F, 80.75269F);
            this.xrLine22.StylePriority.UseForeColor = false;
            // 
            // xrLine21
            // 
            this.xrLine21.ForeColor = System.Drawing.Color.Blue;
            this.xrLine21.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine21.LocationFloat = new DevExpress.Utils.PointFloat(0F, 78.66936F);
            this.xrLine21.Name = "xrLine21";
            this.xrLine21.SizeF = new System.Drawing.SizeF(748.0834F, 2.083334F);
            this.xrLine21.StylePriority.UseForeColor = false;
            // 
            // xrLine20
            // 
            this.xrLine20.ForeColor = System.Drawing.Color.Blue;
            this.xrLine20.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine20.LocationFloat = new DevExpress.Utils.PointFloat(0.0001827876F, 0F);
            this.xrLine20.Name = "xrLine20";
            this.xrLine20.SizeF = new System.Drawing.SizeF(2.083333F, 78.66936F);
            this.xrLine20.StylePriority.UseForeColor = false;
            // 
            // xrLine19
            // 
            this.xrLine19.ForeColor = System.Drawing.Color.Blue;
            this.xrLine19.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine19.LocationFloat = new DevExpress.Utils.PointFloat(745.9167F, 0F);
            this.xrLine19.Name = "xrLine19";
            this.xrLine19.SizeF = new System.Drawing.SizeF(2.083313F, 78.66936F);
            this.xrLine19.StylePriority.UseForeColor = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(2.083405F, 0F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(97.35181F, 40.08001F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Acciones Correctivas:";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel14
            // 
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionAccionesCorrectivas]")});
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(627.0743F, 78.66936F);
            this.xrLabel14.Text = "xrLabel14";
            // 
            // sbAccionesMejora
            // 
            this.sbAccionesMejora.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine26,
            this.xrLine25,
            this.xrLine24,
            this.xrLine23,
            this.xrLabel16,
            this.xrLabel19});
            this.sbAccionesMejora.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([AccionesMejora]== True, True , False )")});
            this.sbAccionesMejora.HeightF = 102.6437F;
            this.sbAccionesMejora.Name = "sbAccionesMejora";
            // 
            // xrLine26
            // 
            this.xrLine26.ForeColor = System.Drawing.Color.Blue;
            this.xrLine26.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine26.LocationFloat = new DevExpress.Utils.PointFloat(745.9134F, 0F);
            this.xrLine26.Name = "xrLine26";
            this.xrLine26.SizeF = new System.Drawing.SizeF(2.083374F, 100.5603F);
            this.xrLine26.StylePriority.UseForeColor = false;
            // 
            // xrLine25
            // 
            this.xrLine25.ForeColor = System.Drawing.Color.Blue;
            this.xrLine25.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine25.LocationFloat = new DevExpress.Utils.PointFloat(99.43156F, 0F);
            this.xrLine25.Name = "xrLine25";
            this.xrLine25.SizeF = new System.Drawing.SizeF(2.083435F, 100.5603F);
            this.xrLine25.StylePriority.UseForeColor = false;
            // 
            // xrLine24
            // 
            this.xrLine24.ForeColor = System.Drawing.Color.Blue;
            this.xrLine24.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine24.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine24.Name = "xrLine24";
            this.xrLine24.SizeF = new System.Drawing.SizeF(2.083435F, 100.5603F);
            this.xrLine24.StylePriority.UseForeColor = false;
            // 
            // xrLine23
            // 
            this.xrLine23.ForeColor = System.Drawing.Color.Blue;
            this.xrLine23.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine23.LocationFloat = new DevExpress.Utils.PointFloat(0F, 100.5603F);
            this.xrLine23.Name = "xrLine23";
            this.xrLine23.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine23.StylePriority.UseForeColor = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(2.083405F, 0F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(97.34816F, 31.25F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Acciones de Mejora:";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel19
            // 
            this.xrLabel19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionAccionesMejora]")});
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 0F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(629.1575F, 100.5603F);
            this.xrLabel19.Text = "xrLabel19";
            // 
            // sbAccionesOtras
            // 
            this.sbAccionesOtras.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine30,
            this.xrLine29,
            this.xrLine28,
            this.xrLine27,
            this.xrLabel21,
            this.xrLabel22});
            this.sbAccionesOtras.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([AccionesOtras] == true, true , false )")});
            this.sbAccionesOtras.HeightF = 67.29173F;
            this.sbAccionesOtras.Name = "sbAccionesOtras";
            // 
            // xrLine30
            // 
            this.xrLine30.ForeColor = System.Drawing.Color.Blue;
            this.xrLine30.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine30.LocationFloat = new DevExpress.Utils.PointFloat(0.0001827876F, 0F);
            this.xrLine30.Name = "xrLine30";
            this.xrLine30.SizeF = new System.Drawing.SizeF(2.083435F, 67.2917F);
            this.xrLine30.StylePriority.UseForeColor = false;
            // 
            // xrLine29
            // 
            this.xrLine29.ForeColor = System.Drawing.Color.Blue;
            this.xrLine29.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine29.LocationFloat = new DevExpress.Utils.PointFloat(1.996748F, 65.20834F);
            this.xrLine29.Name = "xrLine29";
            this.xrLine29.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine29.StylePriority.UseForeColor = false;
            // 
            // xrLine28
            // 
            this.xrLine28.ForeColor = System.Drawing.Color.Blue;
            this.xrLine28.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine28.LocationFloat = new DevExpress.Utils.PointFloat(99.43156F, 0F);
            this.xrLine28.Name = "xrLine28";
            this.xrLine28.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine28.StylePriority.UseForeColor = false;
            // 
            // xrLine27
            // 
            this.xrLine27.ForeColor = System.Drawing.Color.Blue;
            this.xrLine27.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine27.LocationFloat = new DevExpress.Utils.PointFloat(745.9134F, 0F);
            this.xrLine27.Name = "xrLine27";
            this.xrLine27.SizeF = new System.Drawing.SizeF(2.083374F, 67.2917F);
            this.xrLine27.StylePriority.UseForeColor = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(4.166857F, 0F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(95.26471F, 31.25F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "Otras Acciones:";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel22
            // 
            this.xrLabel22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionAccionesOtras]")});
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(111.9347F, 0F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(629.1573F, 65.20834F);
            this.xrLabel22.Text = "xrLabel22";
            // 
            // sbSinAcciones
            // 
            this.sbSinAcciones.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine34,
            this.xrLine33,
            this.xrLine32,
            this.xrLine31,
            this.xrLabel23,
            this.xrLabel24});
            this.sbSinAcciones.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([SinAcciones] == True, True , False )")});
            this.sbSinAcciones.HeightF = 67.29173F;
            this.sbSinAcciones.Name = "sbSinAcciones";
            // 
            // xrLine34
            // 
            this.xrLine34.ForeColor = System.Drawing.Color.Blue;
            this.xrLine34.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine34.LocationFloat = new DevExpress.Utils.PointFloat(0.000190258F, 0F);
            this.xrLine34.Name = "xrLine34";
            this.xrLine34.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine34.StylePriority.UseForeColor = false;
            // 
            // xrLine33
            // 
            this.xrLine33.ForeColor = System.Drawing.Color.Blue;
            this.xrLine33.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine33.LocationFloat = new DevExpress.Utils.PointFloat(0.0001827876F, 65.2084F);
            this.xrLine33.Name = "xrLine33";
            this.xrLine33.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine33.StylePriority.UseForeColor = false;
            // 
            // xrLine32
            // 
            this.xrLine32.ForeColor = System.Drawing.Color.Blue;
            this.xrLine32.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine32.LocationFloat = new DevExpress.Utils.PointFloat(745.9135F, 0F);
            this.xrLine32.Name = "xrLine32";
            this.xrLine32.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine32.StylePriority.UseForeColor = false;
            // 
            // xrLine31
            // 
            this.xrLine31.ForeColor = System.Drawing.Color.Blue;
            this.xrLine31.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine31.LocationFloat = new DevExpress.Utils.PointFloat(99.43156F, 0F);
            this.xrLine31.Name = "xrLine31";
            this.xrLine31.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine31.StylePriority.UseForeColor = false;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(4.080176F, 0F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(95.35139F, 31.25F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "Sin Acciones:";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel24
            // 
            this.xrLabel24.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionSinAcciones]")});
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(111.9348F, 0F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(629.1573F, 31.25F);
            this.xrLabel24.Text = "xrLabel24";
            // 
            // sbDescripcionAcciones
            // 
            this.sbDescripcionAcciones.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine38,
            this.xrLine37,
            this.xrLine36,
            this.xrLine35,
            this.xrLabel25,
            this.xrLabel26});
            this.sbDescripcionAcciones.HeightF = 67.29173F;
            this.sbDescripcionAcciones.Name = "sbDescripcionAcciones";
            // 
            // xrLine38
            // 
            this.xrLine38.ForeColor = System.Drawing.Color.Blue;
            this.xrLine38.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine38.LocationFloat = new DevExpress.Utils.PointFloat(1.996748F, 65.2084F);
            this.xrLine38.Name = "xrLine38";
            this.xrLine38.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine38.StylePriority.UseForeColor = false;
            // 
            // xrLine37
            // 
            this.xrLine37.ForeColor = System.Drawing.Color.Blue;
            this.xrLine37.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine37.LocationFloat = new DevExpress.Utils.PointFloat(746.0001F, 0F);
            this.xrLine37.Name = "xrLine37";
            this.xrLine37.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine37.StylePriority.UseForeColor = false;
            // 
            // xrLine36
            // 
            this.xrLine36.ForeColor = System.Drawing.Color.Blue;
            this.xrLine36.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine36.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine36.Name = "xrLine36";
            this.xrLine36.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine36.StylePriority.UseForeColor = false;
            // 
            // xrLine35
            // 
            this.xrLine35.ForeColor = System.Drawing.Color.Blue;
            this.xrLine35.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine35.LocationFloat = new DevExpress.Utils.PointFloat(99.43528F, 0F);
            this.xrLine35.Name = "xrLine35";
            this.xrLine35.SizeF = new System.Drawing.SizeF(2.083427F, 67.29173F);
            this.xrLine35.StylePriority.UseForeColor = false;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(4.080176F, 9.999974F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(93.26797F, 31.25F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "Descripcion Acciones:";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel26
            // 
            this.xrLabel26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionAcciones]")});
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(114.0182F, 9.999974F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(629.1573F, 55.20843F);
            this.xrLabel26.Text = "xrLabel26";
            // 
            // sbFechaImplementacion
            // 
            this.sbFechaImplementacion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine42,
            this.xrLine41,
            this.xrLine39,
            this.xrLine40,
            this.xrLabel34,
            this.xrLabel33});
            this.sbFechaImplementacion.HeightF = 31.00414F;
            this.sbFechaImplementacion.Name = "sbFechaImplementacion";
            // 
            // xrLine42
            // 
            this.xrLine42.ForeColor = System.Drawing.Color.Blue;
            this.xrLine42.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine42.LocationFloat = new DevExpress.Utils.PointFloat(745.9133F, 0F);
            this.xrLine42.Name = "xrLine42";
            this.xrLine42.SizeF = new System.Drawing.SizeF(2.083435F, 30.04796F);
            this.xrLine42.StylePriority.UseForeColor = false;
            // 
            // xrLine41
            // 
            this.xrLine41.ForeColor = System.Drawing.Color.Blue;
            this.xrLine41.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine41.LocationFloat = new DevExpress.Utils.PointFloat(193.1854F, 0F);
            this.xrLine41.Name = "xrLine41";
            this.xrLine41.SizeF = new System.Drawing.SizeF(2.083435F, 27.96462F);
            this.xrLine41.StylePriority.UseForeColor = false;
            // 
            // xrLine39
            // 
            this.xrLine39.ForeColor = System.Drawing.Color.Blue;
            this.xrLine39.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine39.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.083333F);
            this.xrLine39.Name = "xrLine39";
            this.xrLine39.SizeF = new System.Drawing.SizeF(2.083427F, 25.88129F);
            this.xrLine39.StylePriority.UseForeColor = false;
            // 
            // xrLine40
            // 
            this.xrLine40.ForeColor = System.Drawing.Color.Blue;
            this.xrLine40.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine40.LocationFloat = new DevExpress.Utils.PointFloat(0F, 27.96462F);
            this.xrLine40.Name = "xrLine40";
            this.xrLine40.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine40.StylePriority.UseForeColor = false;
            // 
            // xrLabel34
            // 
            this.xrLabel34.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FechaImplementacion]")});
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(207.3518F, 4.964638F);
            this.xrLabel34.Multiline = true;
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel34.Text = "xrLabel34";
            this.xrLabel34.TextFormatString = "{0:d}";
            // 
            // xrLabel33
            // 
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(2.083429F, 0F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(179.6436F, 27.96462F);
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "Fecha de Implementacion:";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // SubBand1
            // 
            this.SubBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine47,
            this.xrLabel36,
            this.xrLabel35,
            this.xrLine46,
            this.xrLine45,
            this.xrLine44,
            this.xrLine43,
            this.xrLabel27,
            this.xrLabel18});
            this.SubBand1.HeightF = 124.4874F;
            this.SubBand1.Name = "SubBand1";
            // 
            // xrLine47
            // 
            this.xrLine47.ForeColor = System.Drawing.Color.Blue;
            this.xrLine47.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine47.LocationFloat = new DevExpress.Utils.PointFloat(511.1216F, 0F);
            this.xrLine47.Name = "xrLine47";
            this.xrLine47.SizeF = new System.Drawing.SizeF(2.083435F, 46.66236F);
            this.xrLine47.StylePriority.UseForeColor = false;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(513.2051F, 9.999974F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(116.5494F, 34.05976F);
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "Fecha de Evaluación:";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel35
            // 
            this.xrLabel35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FechaEvaluacion]")});
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(636.0001F, 14F);
            this.xrLabel35.Multiline = true;
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel35.Text = "xrLabel35";
            this.xrLabel35.TextFormatString = "{0:dd/MM/yyyy}";
            // 
            // xrLine46
            // 
            this.xrLine46.ForeColor = System.Drawing.Color.Blue;
            this.xrLine46.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine46.LocationFloat = new DevExpress.Utils.PointFloat(0F, 44.05975F);
            this.xrLine46.Name = "xrLine46";
            this.xrLine46.SizeF = new System.Drawing.SizeF(746.0002F, 2.602608F);
            this.xrLine46.StylePriority.UseForeColor = false;
            // 
            // xrLine45
            // 
            this.xrLine45.ForeColor = System.Drawing.Color.Blue;
            this.xrLine45.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine45.LocationFloat = new DevExpress.Utils.PointFloat(745.9133F, 0F);
            this.xrLine45.Name = "xrLine45";
            this.xrLine45.SizeF = new System.Drawing.SizeF(2.083435F, 122.4041F);
            this.xrLine45.StylePriority.UseForeColor = false;
            // 
            // xrLine44
            // 
            this.xrLine44.ForeColor = System.Drawing.Color.Blue;
            this.xrLine44.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine44.LocationFloat = new DevExpress.Utils.PointFloat(0.0001907349F, 0F);
            this.xrLine44.Name = "xrLine44";
            this.xrLine44.SizeF = new System.Drawing.SizeF(2.083435F, 122.4041F);
            this.xrLine44.StylePriority.UseForeColor = false;
            // 
            // xrLine43
            // 
            this.xrLine43.ForeColor = System.Drawing.Color.Blue;
            this.xrLine43.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine43.LocationFloat = new DevExpress.Utils.PointFloat(1.996748F, 122.4041F);
            this.xrLine43.Name = "xrLine43";
            this.xrLine43.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine43.StylePriority.UseForeColor = false;
            // 
            // xrLabel27
            // 
            this.xrLabel27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DescripcionEvaluacion]")});
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 57.07906F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(733.1754F, 65.32501F);
            this.xrLabel27.Text = "xrLabel27";
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(49.18652F, 7.397366F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(461.9351F, 36.66239F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "Evaluación de la eficacia de la Acción tomada";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // sbNuevaAccion
            // 
            this.sbNuevaAccion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine51,
            this.xrLine50,
            this.xrLine49,
            this.xrLine48,
            this.xrLabel29,
            this.xrLabel28});
            this.sbNuevaAccion.HeightF = 98.44573F;
            this.sbNuevaAccion.Name = "sbNuevaAccion";
            // 
            // xrLine51
            // 
            this.xrLine51.ForeColor = System.Drawing.Color.Blue;
            this.xrLine51.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine51.LocationFloat = new DevExpress.Utils.PointFloat(1.996771F, 41.24997F);
            this.xrLine51.Name = "xrLine51";
            this.xrLine51.SizeF = new System.Drawing.SizeF(744.0033F, 2.083336F);
            this.xrLine51.StylePriority.UseForeColor = false;
            // 
            // xrLine50
            // 
            this.xrLine50.ForeColor = System.Drawing.Color.Blue;
            this.xrLine50.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine50.LocationFloat = new DevExpress.Utils.PointFloat(0.0001907349F, 96.3624F);
            this.xrLine50.Name = "xrLine50";
            this.xrLine50.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine50.StylePriority.UseForeColor = false;
            // 
            // xrLine49
            // 
            this.xrLine49.ForeColor = System.Drawing.Color.Blue;
            this.xrLine49.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine49.LocationFloat = new DevExpress.Utils.PointFloat(746.0001F, 0F);
            this.xrLine49.Name = "xrLine49";
            this.xrLine49.SizeF = new System.Drawing.SizeF(2.083435F, 98.44573F);
            this.xrLine49.StylePriority.UseForeColor = false;
            // 
            // xrLine48
            // 
            this.xrLine48.ForeColor = System.Drawing.Color.Blue;
            this.xrLine48.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine48.LocationFloat = new DevExpress.Utils.PointFloat(0.0001907349F, 0F);
            this.xrLine48.Name = "xrLine48";
            this.xrLine48.SizeF = new System.Drawing.SizeF(2.083435F, 98.44573F);
            this.xrLine48.StylePriority.UseForeColor = false;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(229.5833F, 10.00001F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(297.3518F, 31.25F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "Nueva Accion";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel28
            // 
            this.xrLabel28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NuevaAccion]")});
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(4.907942F, 43.33331F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(731.0921F, 53.02909F);
            this.xrLabel28.Text = "xrLabel28";
            // 
            // SubBand2
            // 
            this.SubBand2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine55,
            this.xrLine54,
            this.xrLine53,
            this.xrLine52,
            this.xrLabel20,
            this.xrLabel30});
            this.SubBand2.HeightF = 98.44573F;
            this.SubBand2.Name = "SubBand2";
            // 
            // xrLine55
            // 
            this.xrLine55.ForeColor = System.Drawing.Color.Blue;
            this.xrLine55.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine55.LocationFloat = new DevExpress.Utils.PointFloat(0.0001907349F, 0F);
            this.xrLine55.Name = "xrLine55";
            this.xrLine55.SizeF = new System.Drawing.SizeF(2.083435F, 98.0657F);
            this.xrLine55.StylePriority.UseForeColor = false;
            // 
            // xrLine54
            // 
            this.xrLine54.ForeColor = System.Drawing.Color.Blue;
            this.xrLine54.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine54.LocationFloat = new DevExpress.Utils.PointFloat(746.0002F, 0F);
            this.xrLine54.Name = "xrLine54";
            this.xrLine54.SizeF = new System.Drawing.SizeF(2.083435F, 98.0657F);
            this.xrLine54.StylePriority.UseForeColor = false;
            // 
            // xrLine53
            // 
            this.xrLine53.ForeColor = System.Drawing.Color.Blue;
            this.xrLine53.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine53.LocationFloat = new DevExpress.Utils.PointFloat(3.993487F, 31.25F);
            this.xrLine53.Name = "xrLine53";
            this.xrLine53.SizeF = new System.Drawing.SizeF(744.0033F, 2.083336F);
            this.xrLine53.StylePriority.UseForeColor = false;
            // 
            // xrLine52
            // 
            this.xrLine52.ForeColor = System.Drawing.Color.Blue;
            this.xrLine52.LineStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.xrLine52.LocationFloat = new DevExpress.Utils.PointFloat(0F, 95.98236F);
            this.xrLine52.Name = "xrLine52";
            this.xrLine52.SizeF = new System.Drawing.SizeF(746F, 2.083328F);
            this.xrLine52.StylePriority.UseForeColor = false;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(229.5833F, 0F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(303.185F, 31.25F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Obervaciones";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel30
            // 
            this.xrLabel30.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Observaciones]")});
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(3.240577F, 33.33333F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(734.8422F, 54.73239F);
            this.xrLabel30.Text = "xrLabel30";
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Tahoma", 14F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Title.Name = "Title";
            // 
            // DetailCaption1
            // 
            this.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.DetailCaption1.BorderColor = System.Drawing.Color.White;
            this.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption1.BorderWidth = 2F;
            this.DetailCaption1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.DetailCaption1.ForeColor = System.Drawing.Color.White;
            this.DetailCaption1.Name = "DetailCaption1";
            this.DetailCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData1
            // 
            this.DetailData1.BackColor = System.Drawing.Color.Transparent;
            this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData1.BorderWidth = 2F;
            this.DetailData1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData1.ForeColor = System.Drawing.Color.Black;
            this.DetailData1.Name = "DetailData1";
            this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailCaption3
            // 
            this.DetailCaption3.BackColor = System.Drawing.Color.Transparent;
            this.DetailCaption3.BorderColor = System.Drawing.Color.Transparent;
            this.DetailCaption3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailCaption3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.DetailCaption3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.DetailCaption3.Name = "DetailCaption3";
            this.DetailCaption3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3
            // 
            this.DetailData3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData3.ForeColor = System.Drawing.Color.Black;
            this.DetailData3.Name = "DetailData3";
            this.DetailData3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailCaptionBackground3
            // 
            this.DetailCaptionBackground3.BackColor = System.Drawing.Color.Transparent;
            this.DetailCaptionBackground3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.DetailCaptionBackground3.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.DetailCaptionBackground3.BorderWidth = 2F;
            this.DetailCaptionBackground3.Name = "DetailCaptionBackground3";
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // Acontecimiento_Id
            // 
            this.Acontecimiento_Id.Name = "Acontecimiento_Id";
            this.Acontecimiento_Id.Type = typeof(int);
            this.Acontecimiento_Id.ValueInfo = "0";
            // 
            // pfFirmas
            // 
            this.pfFirmas.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel32,
            this.xrPictureBox3,
            this.xrPictureBoxGerenteGeneral});
            this.pfFirmas.HeightF = 138.2097F;
            this.pfFirmas.Name = "pfFirmas";
            // 
            // xrLabel32
            // 
            this.xrLabel32.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif(?bFirmaGteGral == true, true , false)\n")});
            this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 98.48341F);
            this.xrLabel32.Multiline = true;
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(139.5833F, 31.29174F);
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "Ing. Daniel Alejo Perticaro\r\n       Gerente General";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox3
            // 
            this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(174.4167F, 9.999974F);
            this.xrPictureBox3.Name = "xrPictureBox3";
            this.xrPictureBox3.SizeF = new System.Drawing.SizeF(101.0417F, 104.1085F);
            // 
            // xrPictureBoxGerenteGeneral
            // 
            this.xrPictureBoxGerenteGeneral.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif(?bFirmaGteGral == true, true , false)")});
            this.xrPictureBoxGerenteGeneral.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBoxGerenteGeneral.ImageSource"));
            this.xrPictureBoxGerenteGeneral.LocationFloat = new DevExpress.Utils.PointFloat(32.91667F, 10.0001F);
            this.xrPictureBoxGerenteGeneral.Name = "xrPictureBoxGerenteGeneral";
            this.xrPictureBoxGerenteGeneral.SizeF = new System.Drawing.SizeF(105.2083F, 104.1084F);
            this.xrPictureBoxGerenteGeneral.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // bFirmaGteGral
            // 
            this.bFirmaGteGral.Description = "Muestra firma digital de gerente general";
            this.bFirmaGteGral.Name = "bFirmaGteGral";
            this.bFirmaGteGral.Type = typeof(bool);
            this.bFirmaGteGral.ValueInfo = "False";
            // 
            // XtraReportNoConformidad
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.bottomMarginBand1,
            this.detailBand1,
            this.pfFirmas});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "ListAcontecimientos";
            this.DataSource = this.sqlDataSource1;
            this.ExportOptions.Html.TableLayout = false;
            this.ExportOptions.Html.Title = "No conformidad";
            this.FilterString = "[Registro] = ?Acontecimiento_Id";
            this.Margins = new System.Drawing.Printing.Margins(32, 30, 342, 49);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Acontecimiento_Id,
            this.bFirmaGteGral});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailCaption3,
            this.DetailData3,
            this.DetailData3_Odd,
            this.DetailCaptionBackground3,
            this.PageInfo});
            this.Version = "19.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbAccionesInmediatas_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void sbAccionesInmediatas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }
}
