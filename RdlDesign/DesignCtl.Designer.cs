﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Majorsilence.Reporting.RdlDesign
{
	public partial class DesignCtl : System.Windows.Forms.UserControl
	{
		private ContextMenuStrip ContextMenuDefault;
		private System.ComponentModel.IContainer components;
		private ToolStripMenuItem MenuDefaultProperties;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem MenuDefaultCopy;
		private ToolStripMenuItem MenuDefaultPaste;
		private ToolStripMenuItem MenuDefaultDelete;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem MenuDefaultSelectAll;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem MenuDefaultInsert;
		private ToolStripMenuItem MenuInsertChart;
		private ToolStripMenuItem MenuInsertGrid;
		private ToolStripMenuItem MenuInsertImage;
		private ToolStripMenuItem MenuInsertLine;
		private ToolStripMenuItem MenuInsertList;
		private ToolStripMenuItem MenuInsertMatrix;
		private ToolStripMenuItem MenuInsertRectangle;
		private ToolStripMenuItem MenuInsertSubreport;
		private ToolStripMenuItem MenuInsertTable;
		private ToolStripMenuItem MenuInsertTextbox;
		private ContextMenuStrip ContextMenuChart;
		private ToolStripMenuItem MenuChartProperties;
		private ToolStripMenuItem MenuChartLegend;
		private ToolStripMenuItem MenuChartTitle;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem MenuChartInsertCategoryGrouping;
		private ToolStripMenuItem MenuChartEditCategoryGrouping;
		private ToolStripMenuItem MenuChartDeleteCategoryGrouping;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripMenuItem MenuChartCategoryAxis;
		private ToolStripMenuItem MenuChartCategoryAxisTitle;
		private ToolStripSeparator toolStripMenuItem6;
		private ToolStripMenuItem MenuChartInsertSeriesGrouping;
		private ToolStripMenuItem MenuChartEditSeriesGrouping;
		private ToolStripMenuItem MenuChartDeleteSeriesGrouping;
		private ToolStripSeparator toolStripMenuItem7;
		private ToolStripMenuItem MenuChartValueAxis;
		private ToolStripMenuItem MenuChartValueAxisTitle;
		private ToolStripMenuItem MenuChartValueAxisRightTitle;
		private ToolStripSeparator toolStripMenuItem8;
		private ToolStripMenuItem MenuChartCopy;
		private ToolStripMenuItem MenuChartPaste;
		private ToolStripMenuItem MenuChartDelete;
		private ToolStripSeparator toolStripMenuItem9;
		private ContextMenuStrip ContextMenuMatrix;
		private ToolStripMenuItem MenuMatrixProperties;
		private ToolStripMenuItem MenuMatrixMatrixProperties;
		private ToolStripSeparator toolStripMenuItem10;
		private ToolStripMenuItem MenuMatrixInsertColumnGroup;
		private ToolStripMenuItem MenuMatrixEditColumnGroup;
		private ToolStripMenuItem MenuMatrixDeleteColumnGroup;
		private ToolStripSeparator toolStripMenuItem11;
		private ToolStripMenuItem MenuMatrixInsertRowGroup;
		private ToolStripMenuItem MenuMatrixEditRowGroup;
		private ToolStripMenuItem MenuMatrixDeleteRowGroup;
		private ToolStripSeparator toolStripMenuItem12;
		private ToolStripMenuItem MenuMatrixDeleteMatrix;
		private ToolStripSeparator toolStripMenuItem13;
		private ToolStripMenuItem MenuMatrixCopy;
		private ToolStripMenuItem MenuMatrixPaste;
		private ToolStripMenuItem MenuMatrixDelete;
		private ToolStripSeparator toolStripMenuItem14;
		private ToolStripMenuItem MenuMatrixSelectAll;
		private ContextMenuStrip ContextMenuSubreport;
		private ToolStripMenuItem MenuSubreportProperties;
		private ToolStripMenuItem MenuSubreportOpen;
		private ToolStripSeparator toolStripMenuItem15;
		private ToolStripMenuItem MenuSubreportCopy;
		private ToolStripMenuItem MenuSubreportPaste;
		private ToolStripMenuItem MenuSubreportDelete;
		private ToolStripSeparator toolStripMenuItem16;
		private ToolStripMenuItem MenuSubreportSelectAll;
		private ContextMenuStrip ContextMenuGrid;
		private ToolStripMenuItem MenuGridProperties;
		private ToolStripMenuItem MenuGridGridProperties;
		private ToolStripMenuItem MenuGridReplaceCell;
		private ToolStripMenuItem MenuGridReplaceCellChart;
		private ToolStripMenuItem MenuGridReplaceCellImage;
		private ToolStripMenuItem MenuGridReplaceCellList;
		private ToolStripMenuItem MenuGridReplaceCellMatrix;
		private ToolStripMenuItem MenuGridReplaceCellRectangle;
		private ToolStripMenuItem MenuGridReplaceCellSubreport;
		private ToolStripMenuItem MenuGridReplaceCellTable;
		private ToolStripMenuItem MenuGridReplaceCellTextbox;
		private ToolStripSeparator toolStripMenuItem17;
		private ToolStripMenuItem MenuGridInsertColumnBefore;
		private ToolStripMenuItem MenuGridInsertColumnAfter;
		private ToolStripSeparator toolStripMenuItem18;
		private ToolStripMenuItem MenuGridInsertRowBefore;
		private ToolStripMenuItem MenuGridInsertRowAfter;
		private ToolStripSeparator toolStripMenuItem19;
		private ToolStripMenuItem MenuGridDeleteColumn;
		private ToolStripMenuItem MenuGridDeleteRow;
		private ToolStripMenuItem MenuGridDeleteGrid;
		private ToolStripSeparator toolStripMenuItem20;
		private ToolStripMenuItem MenuGridCopy;
		private ToolStripMenuItem MenuGridPaste;
		private ToolStripMenuItem MenuGridDelete;
		private ToolStripSeparator toolStripMenuItem21;
		private ToolStripMenuItem MenuGridSelectAll;
		private ContextMenuStrip ContextMenuTable;
		private ToolStripMenuItem MenuTableProperties;
		private ToolStripMenuItem MenuTableTableProperties;
		private ToolStripMenuItem MenuTableReplaceCell;
		private ToolStripMenuItem MenuTableReplaceCellChart;
		private ToolStripMenuItem MenuTableReplaceCellImage;
		private ToolStripMenuItem MenuTableReplaceCellList;
		private ToolStripMenuItem MenuTableReplaceCellMatrix;
		private ToolStripMenuItem MenuTableReplaceCellRectangle;
		private ToolStripMenuItem MenuTableReplaceCellSubreport;
		private ToolStripMenuItem MenuTableReplaceCellTable;
		private ToolStripMenuItem MenuTableReplaceCellTextbox;
		private ToolStripSeparator toolStripMenuItem22;
		private ToolStripMenuItem MenuTableInsertColumnBefore;
		private ToolStripMenuItem MenuTableInsertColumnAfter;
		private ToolStripSeparator toolStripMenuItem23;
		private ToolStripMenuItem MenuTableInsertRowBefore;
		private ToolStripMenuItem MenuTableInsertRowAfter;
		private ToolStripSeparator toolStripMenuItem24;
		private ToolStripMenuItem MenuTableInsertGroup;
		private ToolStripMenuItem MenuTableEditGroup;
		private ToolStripMenuItem MenuTableDeleteGroup;
		private ToolStripSeparator toolStripMenuItem25;
		private ToolStripMenuItem MenuTableDeleteColumn;
		private ToolStripMenuItem MenuTableDeleteRow;
		private ToolStripMenuItem MenuTableDeleteTable;
		private ToolStripSeparator toolStripMenuItem26;
		private ToolStripMenuItem MenuTableCopy;
		private ToolStripMenuItem MenuTablePaste;
		private ToolStripMenuItem MenuTableDelete;
		private ToolStripSeparator toolStripMenuItem27;
		private ToolStripMenuItem MenuTableSelectAll;
		private ToolStripMenuItem MenuChartSelectAll;

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignCtl));
            this.DoubleBuffered = true;
			this.ContextMenuDefault = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuDefaultProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuDefaultCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuDefaultPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuDefaultDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuDefaultSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuDefaultInsert = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertChart = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertImage = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertLine = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertList = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertMatrix = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertRectangle = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertSubreport = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertTable = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuInsertTextbox = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuChart = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuChartProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartLegend = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuChartInsertCategoryGrouping = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartEditCategoryGrouping = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartDeleteCategoryGrouping = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuChartCategoryAxis = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartCategoryAxisTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuChartInsertSeriesGrouping = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartEditSeriesGrouping = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartDeleteSeriesGrouping = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuChartValueAxis = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartValueAxisTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartValueAxisRightTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuChartCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuChartDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuChartSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuMatrix = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuMatrixProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixMatrixProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMatrixInsertColumnGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixEditColumnGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixDeleteColumnGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMatrixInsertRowGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixEditRowGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixDeleteRowGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMatrixDeleteMatrix = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMatrixCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMatrixDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMatrixSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuSubreport = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuSubreportProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuSubreportOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuSubreportCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuSubreportPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuSubreportDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuSubreportSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuGridProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridGridProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCell = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellChart = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellImage = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellList = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellMatrix = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellRectangle = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellSubreport = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellTable = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridReplaceCellTextbox = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGridInsertColumnBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridInsertColumnAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGridInsertRowBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridInsertRowAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGridDeleteColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridDeleteGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGridCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGridDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGridSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuTableProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableTableProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCell = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellChart = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellImage = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellList = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellMatrix = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellRectangle = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellSubreport = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellTable = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableReplaceCellTextbox = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuTableInsertColumnBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableInsertColumnAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuTableInsertRowBefore = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableInsertRowAfter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuTableInsertGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableEditGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableDeleteGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuTableDeleteColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableDeleteTable = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuTableCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTablePaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTableDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuTableSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ContextMenuDefault.SuspendLayout();
			this.ContextMenuChart.SuspendLayout();
			this.ContextMenuMatrix.SuspendLayout();
			this.ContextMenuSubreport.SuspendLayout();
			this.ContextMenuGrid.SuspendLayout();
			this.ContextMenuTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// ContextMenuDefault
			// 
			resources.ApplyResources(this.ContextMenuDefault, "ContextMenuDefault");
			this.ContextMenuDefault.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuDefaultProperties,
            this.toolStripMenuItem1,
            this.MenuDefaultCopy,
            this.MenuDefaultPaste,
            this.MenuDefaultDelete,
            this.toolStripMenuItem2,
            this.MenuDefaultSelectAll,
            this.toolStripMenuItem3,
            this.MenuDefaultInsert});
			this.ContextMenuDefault.Name = "ContextMenuDefault";
			this.ContextMenuDefault.Opened += new System.EventHandler(this.menuContext_Popup);
			// 
			// MenuDefaultProperties
			// 
			resources.ApplyResources(this.MenuDefaultProperties, "MenuDefaultProperties");
			this.MenuDefaultProperties.Name = "MenuDefaultProperties";
			this.MenuDefaultProperties.Click += new System.EventHandler(this.menuProperties_Click);
			// 
			// toolStripMenuItem1
			// 
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			// 
			// MenuDefaultCopy
			// 
			resources.ApplyResources(this.MenuDefaultCopy, "MenuDefaultCopy");
			this.MenuDefaultCopy.Name = "MenuDefaultCopy";
			this.MenuDefaultCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// MenuDefaultPaste
			// 
			resources.ApplyResources(this.MenuDefaultPaste, "MenuDefaultPaste");
			this.MenuDefaultPaste.Name = "MenuDefaultPaste";
			this.MenuDefaultPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// MenuDefaultDelete
			// 
			resources.ApplyResources(this.MenuDefaultDelete, "MenuDefaultDelete");
			this.MenuDefaultDelete.Name = "MenuDefaultDelete";
			this.MenuDefaultDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// toolStripMenuItem2
			// 
			resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			// 
			// MenuDefaultSelectAll
			// 
			resources.ApplyResources(this.MenuDefaultSelectAll, "MenuDefaultSelectAll");
			this.MenuDefaultSelectAll.Name = "MenuDefaultSelectAll";
			this.MenuDefaultSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
			// 
			// toolStripMenuItem3
			// 
			resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			// 
			// MenuDefaultInsert
			// 
			resources.ApplyResources(this.MenuDefaultInsert, "MenuDefaultInsert");
			this.MenuDefaultInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuInsertChart,
            this.MenuInsertGrid,
            this.MenuInsertImage,
            this.MenuInsertLine,
            this.MenuInsertList,
            this.MenuInsertMatrix,
            this.MenuInsertRectangle,
            this.MenuInsertSubreport,
            this.MenuInsertTable,
            this.MenuInsertTextbox});
			this.MenuDefaultInsert.Name = "MenuDefaultInsert";
			// 
			// MenuInsertChart
			// 
			resources.ApplyResources(this.MenuInsertChart, "MenuInsertChart");
			this.MenuInsertChart.Name = "MenuInsertChart";
			this.MenuInsertChart.Click += new System.EventHandler(this.menuInsertChart_Click);
			// 
			// MenuInsertGrid
			// 
			resources.ApplyResources(this.MenuInsertGrid, "MenuInsertGrid");
			this.MenuInsertGrid.Name = "MenuInsertGrid";
			this.MenuInsertGrid.Click += new System.EventHandler(this.menuInsertGrid_Click);
			// 
			// MenuInsertImage
			// 
			resources.ApplyResources(this.MenuInsertImage, "MenuInsertImage");
			this.MenuInsertImage.Name = "MenuInsertImage";
			this.MenuInsertImage.Click += new System.EventHandler(this.menuInsertImage_Click);
			// 
			// MenuInsertLine
			// 
			resources.ApplyResources(this.MenuInsertLine, "MenuInsertLine");
			this.MenuInsertLine.Name = "MenuInsertLine";
			this.MenuInsertLine.Click += new System.EventHandler(this.menuInsertLine_Click);
			// 
			// MenuInsertList
			// 
			resources.ApplyResources(this.MenuInsertList, "MenuInsertList");
			this.MenuInsertList.Name = "MenuInsertList";
			this.MenuInsertList.Click += new System.EventHandler(this.menuInsertList_Click);
			// 
			// MenuInsertMatrix
			// 
			resources.ApplyResources(this.MenuInsertMatrix, "MenuInsertMatrix");
			this.MenuInsertMatrix.Name = "MenuInsertMatrix";
			this.MenuInsertMatrix.Click += new System.EventHandler(this.menuInsertMatrix_Click);
			// 
			// MenuInsertRectangle
			// 
			resources.ApplyResources(this.MenuInsertRectangle, "MenuInsertRectangle");
			this.MenuInsertRectangle.Name = "MenuInsertRectangle";
			this.MenuInsertRectangle.Click += new System.EventHandler(this.menuInsertRectangle_Click);
			// 
			// MenuInsertSubreport
			// 
			resources.ApplyResources(this.MenuInsertSubreport, "MenuInsertSubreport");
			this.MenuInsertSubreport.Name = "MenuInsertSubreport";
			this.MenuInsertSubreport.Click += new System.EventHandler(this.menuInsertSubreport_Click);
			// 
			// MenuInsertTable
			// 
			resources.ApplyResources(this.MenuInsertTable, "MenuInsertTable");
			this.MenuInsertTable.Name = "MenuInsertTable";
			this.MenuInsertTable.Click += new System.EventHandler(this.menuInsertTable_Click);
			// 
			// MenuInsertTextbox
			// 
			resources.ApplyResources(this.MenuInsertTextbox, "MenuInsertTextbox");
			this.MenuInsertTextbox.Name = "MenuInsertTextbox";
			this.MenuInsertTextbox.Click += new System.EventHandler(this.menuInsertTextbox_Click);
			// 
			// ContextMenuChart
			// 
			resources.ApplyResources(this.ContextMenuChart, "ContextMenuChart");
			this.ContextMenuChart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuChartProperties,
            this.MenuChartLegend,
            this.MenuChartTitle,
            this.toolStripMenuItem4,
            this.MenuChartInsertCategoryGrouping,
            this.MenuChartEditCategoryGrouping,
            this.MenuChartDeleteCategoryGrouping,
            this.toolStripMenuItem5,
            this.MenuChartCategoryAxis,
            this.MenuChartCategoryAxisTitle,
            this.toolStripMenuItem6,
            this.MenuChartInsertSeriesGrouping,
            this.MenuChartEditSeriesGrouping,
            this.MenuChartDeleteSeriesGrouping,
            this.toolStripMenuItem7,
            this.MenuChartValueAxis,
            this.MenuChartValueAxisTitle,
            this.MenuChartValueAxisRightTitle,
            this.toolStripMenuItem8,
            this.MenuChartCopy,
            this.MenuChartPaste,
            this.MenuChartDelete,
            this.toolStripMenuItem9,
            this.MenuChartSelectAll});
			this.ContextMenuChart.Name = "ContextMenuChart";
			this.ContextMenuChart.Opened += new System.EventHandler(this.menuContext_Popup);
			// 
			// MenuChartProperties
			// 
			resources.ApplyResources(this.MenuChartProperties, "MenuChartProperties");
			this.MenuChartProperties.Name = "MenuChartProperties";
			this.MenuChartProperties.Click += new System.EventHandler(this.menuProperties_Click);
			// 
			// MenuChartLegend
			// 
			resources.ApplyResources(this.MenuChartLegend, "MenuChartLegend");
			this.MenuChartLegend.Name = "MenuChartLegend";
			this.MenuChartLegend.Click += new System.EventHandler(this.menuPropertiesLegend_Click);
			// 
			// MenuChartTitle
			// 
			resources.ApplyResources(this.MenuChartTitle, "MenuChartTitle");
			this.MenuChartTitle.Name = "MenuChartTitle";
			this.MenuChartTitle.Click += new System.EventHandler(this.menuPropertiesChartTitle_Click);
			// 
			// toolStripMenuItem4
			// 
			resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			// 
			// MenuChartInsertCategoryGrouping
			// 
			resources.ApplyResources(this.MenuChartInsertCategoryGrouping, "MenuChartInsertCategoryGrouping");
			this.MenuChartInsertCategoryGrouping.Name = "MenuChartInsertCategoryGrouping";
			this.MenuChartInsertCategoryGrouping.Click += new System.EventHandler(this.menuChartInsertCategoryGrouping_Click);
			// 
			// MenuChartEditCategoryGrouping
			// 
			resources.ApplyResources(this.MenuChartEditCategoryGrouping, "MenuChartEditCategoryGrouping");
			this.MenuChartEditCategoryGrouping.Name = "MenuChartEditCategoryGrouping";
			// 
			// MenuChartDeleteCategoryGrouping
			// 
			resources.ApplyResources(this.MenuChartDeleteCategoryGrouping, "MenuChartDeleteCategoryGrouping");
			this.MenuChartDeleteCategoryGrouping.Name = "MenuChartDeleteCategoryGrouping";
			// 
			// toolStripMenuItem5
			// 
			resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			// 
			// MenuChartCategoryAxis
			// 
			resources.ApplyResources(this.MenuChartCategoryAxis, "MenuChartCategoryAxis");
			this.MenuChartCategoryAxis.Name = "MenuChartCategoryAxis";
			this.MenuChartCategoryAxis.Click += new System.EventHandler(this.menuPropertiesCategoryAxis_Click);
			// 
			// MenuChartCategoryAxisTitle
			// 
			resources.ApplyResources(this.MenuChartCategoryAxisTitle, "MenuChartCategoryAxisTitle");
			this.MenuChartCategoryAxisTitle.Name = "MenuChartCategoryAxisTitle";
			this.MenuChartCategoryAxisTitle.Click += new System.EventHandler(this.menuPropertiesCategoryAxisTitle_Click);
			// 
			// toolStripMenuItem6
			// 
			resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			// 
			// MenuChartInsertSeriesGrouping
			// 
			resources.ApplyResources(this.MenuChartInsertSeriesGrouping, "MenuChartInsertSeriesGrouping");
			this.MenuChartInsertSeriesGrouping.Name = "MenuChartInsertSeriesGrouping";
			this.MenuChartInsertSeriesGrouping.Click += new System.EventHandler(this.menuChartInsertSeriesGrouping_Click);
			// 
			// MenuChartEditSeriesGrouping
			// 
			resources.ApplyResources(this.MenuChartEditSeriesGrouping, "MenuChartEditSeriesGrouping");
			this.MenuChartEditSeriesGrouping.Name = "MenuChartEditSeriesGrouping";
			// 
			// MenuChartDeleteSeriesGrouping
			// 
			resources.ApplyResources(this.MenuChartDeleteSeriesGrouping, "MenuChartDeleteSeriesGrouping");
			this.MenuChartDeleteSeriesGrouping.Name = "MenuChartDeleteSeriesGrouping";
			// 
			// toolStripMenuItem7
			// 
			resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			// 
			// MenuChartValueAxis
			// 
			resources.ApplyResources(this.MenuChartValueAxis, "MenuChartValueAxis");
			this.MenuChartValueAxis.Name = "MenuChartValueAxis";
			this.MenuChartValueAxis.Click += new System.EventHandler(this.menuPropertiesValueAxis_Click);
			// 
			// MenuChartValueAxisTitle
			// 
			resources.ApplyResources(this.MenuChartValueAxisTitle, "MenuChartValueAxisTitle");
			this.MenuChartValueAxisTitle.Name = "MenuChartValueAxisTitle";
			this.MenuChartValueAxisTitle.Click += new System.EventHandler(this.menuPropertiesValueAxisTitle_Click);
			// 
			// MenuChartValueAxisRightTitle
			// 
			resources.ApplyResources(this.MenuChartValueAxisRightTitle, "MenuChartValueAxisRightTitle");
			this.MenuChartValueAxisRightTitle.Name = "MenuChartValueAxisRightTitle";
			this.MenuChartValueAxisRightTitle.Click += new System.EventHandler(this.menuPropertiesValueAxis2Title_Click);
			// 
			// toolStripMenuItem8
			// 
			resources.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			// 
			// MenuChartCopy
			// 
			resources.ApplyResources(this.MenuChartCopy, "MenuChartCopy");
			this.MenuChartCopy.Name = "MenuChartCopy";
			this.MenuChartCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// MenuChartPaste
			// 
			resources.ApplyResources(this.MenuChartPaste, "MenuChartPaste");
			this.MenuChartPaste.Name = "MenuChartPaste";
			this.MenuChartPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// MenuChartDelete
			// 
			resources.ApplyResources(this.MenuChartDelete, "MenuChartDelete");
			this.MenuChartDelete.Name = "MenuChartDelete";
			this.MenuChartDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// toolStripMenuItem9
			// 
			resources.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			// 
			// MenuChartSelectAll
			// 
			resources.ApplyResources(this.MenuChartSelectAll, "MenuChartSelectAll");
			this.MenuChartSelectAll.Name = "MenuChartSelectAll";
			this.MenuChartSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
			// 
			// ContextMenuMatrix
			// 
			resources.ApplyResources(this.ContextMenuMatrix, "ContextMenuMatrix");
			this.ContextMenuMatrix.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMatrixProperties,
            this.MenuMatrixMatrixProperties,
            this.toolStripMenuItem10,
            this.MenuMatrixInsertColumnGroup,
            this.MenuMatrixEditColumnGroup,
            this.MenuMatrixDeleteColumnGroup,
            this.toolStripMenuItem11,
            this.MenuMatrixInsertRowGroup,
            this.MenuMatrixEditRowGroup,
            this.MenuMatrixDeleteRowGroup,
            this.toolStripMenuItem12,
            this.MenuMatrixDeleteMatrix,
            this.toolStripMenuItem13,
            this.MenuMatrixCopy,
            this.MenuMatrixPaste,
            this.MenuMatrixDelete,
            this.toolStripMenuItem14,
            this.MenuMatrixSelectAll});
			this.ContextMenuMatrix.Name = "ContextMenuMatrix";
			this.ContextMenuMatrix.Opened += new System.EventHandler(this.menuContext_Popup);
			// 
			// MenuMatrixProperties
			// 
			resources.ApplyResources(this.MenuMatrixProperties, "MenuMatrixProperties");
			this.MenuMatrixProperties.Name = "MenuMatrixProperties";
			this.MenuMatrixProperties.Click += new System.EventHandler(this.menuProperties_Click);
			// 
			// MenuMatrixMatrixProperties
			// 
			resources.ApplyResources(this.MenuMatrixMatrixProperties, "MenuMatrixMatrixProperties");
			this.MenuMatrixMatrixProperties.Name = "MenuMatrixMatrixProperties";
			this.MenuMatrixMatrixProperties.Click += new System.EventHandler(this.menuMatrixProperties_Click);
			// 
			// toolStripMenuItem10
			// 
			resources.ApplyResources(this.toolStripMenuItem10, "toolStripMenuItem10");
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			// 
			// MenuMatrixInsertColumnGroup
			// 
			resources.ApplyResources(this.MenuMatrixInsertColumnGroup, "MenuMatrixInsertColumnGroup");
			this.MenuMatrixInsertColumnGroup.Name = "MenuMatrixInsertColumnGroup";
			this.MenuMatrixInsertColumnGroup.Click += new System.EventHandler(this.menuMatrixInsertColumnGroup_Click);
			// 
			// MenuMatrixEditColumnGroup
			// 
			resources.ApplyResources(this.MenuMatrixEditColumnGroup, "MenuMatrixEditColumnGroup");
			this.MenuMatrixEditColumnGroup.Name = "MenuMatrixEditColumnGroup";
			// 
			// MenuMatrixDeleteColumnGroup
			// 
			resources.ApplyResources(this.MenuMatrixDeleteColumnGroup, "MenuMatrixDeleteColumnGroup");
			this.MenuMatrixDeleteColumnGroup.Name = "MenuMatrixDeleteColumnGroup";
			// 
			// toolStripMenuItem11
			// 
			resources.ApplyResources(this.toolStripMenuItem11, "toolStripMenuItem11");
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			// 
			// MenuMatrixInsertRowGroup
			// 
			resources.ApplyResources(this.MenuMatrixInsertRowGroup, "MenuMatrixInsertRowGroup");
			this.MenuMatrixInsertRowGroup.Name = "MenuMatrixInsertRowGroup";
			this.MenuMatrixInsertRowGroup.Click += new System.EventHandler(this.menuMatrixInsertRowGroup_Click);
			// 
			// MenuMatrixEditRowGroup
			// 
			resources.ApplyResources(this.MenuMatrixEditRowGroup, "MenuMatrixEditRowGroup");
			this.MenuMatrixEditRowGroup.Name = "MenuMatrixEditRowGroup";
			// 
			// MenuMatrixDeleteRowGroup
			// 
			resources.ApplyResources(this.MenuMatrixDeleteRowGroup, "MenuMatrixDeleteRowGroup");
			this.MenuMatrixDeleteRowGroup.Name = "MenuMatrixDeleteRowGroup";
			// 
			// toolStripMenuItem12
			// 
			resources.ApplyResources(this.toolStripMenuItem12, "toolStripMenuItem12");
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			// 
			// MenuMatrixDeleteMatrix
			// 
			resources.ApplyResources(this.MenuMatrixDeleteMatrix, "MenuMatrixDeleteMatrix");
			this.MenuMatrixDeleteMatrix.Name = "MenuMatrixDeleteMatrix";
			this.MenuMatrixDeleteMatrix.Click += new System.EventHandler(this.menuMatrixDelete_Click);
			// 
			// toolStripMenuItem13
			// 
			resources.ApplyResources(this.toolStripMenuItem13, "toolStripMenuItem13");
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			// 
			// MenuMatrixCopy
			// 
			resources.ApplyResources(this.MenuMatrixCopy, "MenuMatrixCopy");
			this.MenuMatrixCopy.Name = "MenuMatrixCopy";
			this.MenuMatrixCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// MenuMatrixPaste
			// 
			resources.ApplyResources(this.MenuMatrixPaste, "MenuMatrixPaste");
			this.MenuMatrixPaste.Name = "MenuMatrixPaste";
			this.MenuMatrixPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// MenuMatrixDelete
			// 
			resources.ApplyResources(this.MenuMatrixDelete, "MenuMatrixDelete");
			this.MenuMatrixDelete.Name = "MenuMatrixDelete";
			this.MenuMatrixDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// toolStripMenuItem14
			// 
			resources.ApplyResources(this.toolStripMenuItem14, "toolStripMenuItem14");
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			// 
			// MenuMatrixSelectAll
			// 
			resources.ApplyResources(this.MenuMatrixSelectAll, "MenuMatrixSelectAll");
			this.MenuMatrixSelectAll.Name = "MenuMatrixSelectAll";
			this.MenuMatrixSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
			// 
			// ContextMenuSubreport
			// 
			resources.ApplyResources(this.ContextMenuSubreport, "ContextMenuSubreport");
			this.ContextMenuSubreport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuSubreportProperties,
            this.MenuSubreportOpen,
            this.toolStripMenuItem15,
            this.MenuSubreportCopy,
            this.MenuSubreportPaste,
            this.MenuSubreportDelete,
            this.toolStripMenuItem16,
            this.MenuSubreportSelectAll});
			this.ContextMenuSubreport.Name = "ContextMenuSubreport";
			this.ContextMenuSubreport.Opened += new System.EventHandler(this.menuContext_Popup);
			// 
			// MenuSubreportProperties
			// 
			resources.ApplyResources(this.MenuSubreportProperties, "MenuSubreportProperties");
			this.MenuSubreportProperties.Name = "MenuSubreportProperties";
			this.MenuSubreportProperties.Click += new System.EventHandler(this.menuProperties_Click);
			// 
			// MenuSubreportOpen
			// 
			resources.ApplyResources(this.MenuSubreportOpen, "MenuSubreportOpen");
			this.MenuSubreportOpen.Name = "MenuSubreportOpen";
			this.MenuSubreportOpen.Click += new System.EventHandler(this.menuOpenSubreport_Click);
			// 
			// toolStripMenuItem15
			// 
			resources.ApplyResources(this.toolStripMenuItem15, "toolStripMenuItem15");
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			// 
			// MenuSubreportCopy
			// 
			resources.ApplyResources(this.MenuSubreportCopy, "MenuSubreportCopy");
			this.MenuSubreportCopy.Name = "MenuSubreportCopy";
			this.MenuSubreportCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// MenuSubreportPaste
			// 
			resources.ApplyResources(this.MenuSubreportPaste, "MenuSubreportPaste");
			this.MenuSubreportPaste.Name = "MenuSubreportPaste";
			this.MenuSubreportPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// MenuSubreportDelete
			// 
			resources.ApplyResources(this.MenuSubreportDelete, "MenuSubreportDelete");
			this.MenuSubreportDelete.Name = "MenuSubreportDelete";
			this.MenuSubreportDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// toolStripMenuItem16
			// 
			resources.ApplyResources(this.toolStripMenuItem16, "toolStripMenuItem16");
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			// 
			// MenuSubreportSelectAll
			// 
			resources.ApplyResources(this.MenuSubreportSelectAll, "MenuSubreportSelectAll");
			this.MenuSubreportSelectAll.Name = "MenuSubreportSelectAll";
			this.MenuSubreportSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
			// 
			// ContextMenuGrid
			// 
			resources.ApplyResources(this.ContextMenuGrid, "ContextMenuGrid");
			this.ContextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuGridProperties,
            this.MenuGridGridProperties,
            this.MenuGridReplaceCell,
            this.toolStripMenuItem17,
            this.MenuGridInsertColumnBefore,
            this.MenuGridInsertColumnAfter,
            this.toolStripMenuItem18,
            this.MenuGridInsertRowBefore,
            this.MenuGridInsertRowAfter,
            this.toolStripMenuItem19,
            this.MenuGridDeleteColumn,
            this.MenuGridDeleteRow,
            this.MenuGridDeleteGrid,
            this.toolStripMenuItem20,
            this.MenuGridCopy,
            this.MenuGridPaste,
            this.MenuGridDelete,
            this.toolStripMenuItem21,
            this.MenuGridSelectAll});
			this.ContextMenuGrid.Name = "ContextMenuGrid";
			this.ContextMenuGrid.Opened += new System.EventHandler(this.menuContext_Popup);
			// 
			// MenuGridProperties
			// 
			resources.ApplyResources(this.MenuGridProperties, "MenuGridProperties");
			this.MenuGridProperties.Name = "MenuGridProperties";
			this.MenuGridProperties.Click += new System.EventHandler(this.menuProperties_Click);
			// 
			// MenuGridGridProperties
			// 
			resources.ApplyResources(this.MenuGridGridProperties, "MenuGridGridProperties");
			this.MenuGridGridProperties.Name = "MenuGridGridProperties";
			this.MenuGridGridProperties.Click += new System.EventHandler(this.menuTableProperties_Click);
			// 
			// MenuGridReplaceCell
			// 
			resources.ApplyResources(this.MenuGridReplaceCell, "MenuGridReplaceCell");
			this.MenuGridReplaceCell.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuGridReplaceCellChart,
            this.MenuGridReplaceCellImage,
            this.MenuGridReplaceCellList,
            this.MenuGridReplaceCellMatrix,
            this.MenuGridReplaceCellRectangle,
            this.MenuGridReplaceCellSubreport,
            this.MenuGridReplaceCellTable,
            this.MenuGridReplaceCellTextbox});
			this.MenuGridReplaceCell.Name = "MenuGridReplaceCell";
			// 
			// MenuGridReplaceCellChart
			// 
			resources.ApplyResources(this.MenuGridReplaceCellChart, "MenuGridReplaceCellChart");
			this.MenuGridReplaceCellChart.Name = "MenuGridReplaceCellChart";
			this.MenuGridReplaceCellChart.Click += new System.EventHandler(this.menuInsertChart_Click);
			// 
			// MenuGridReplaceCellImage
			// 
			resources.ApplyResources(this.MenuGridReplaceCellImage, "MenuGridReplaceCellImage");
			this.MenuGridReplaceCellImage.Name = "MenuGridReplaceCellImage";
			this.MenuGridReplaceCellImage.Click += new System.EventHandler(this.menuInsertImage_Click);
			// 
			// MenuGridReplaceCellList
			// 
			resources.ApplyResources(this.MenuGridReplaceCellList, "MenuGridReplaceCellList");
			this.MenuGridReplaceCellList.Name = "MenuGridReplaceCellList";
			this.MenuGridReplaceCellList.Click += new System.EventHandler(this.menuInsertList_Click);
			// 
			// MenuGridReplaceCellMatrix
			// 
			resources.ApplyResources(this.MenuGridReplaceCellMatrix, "MenuGridReplaceCellMatrix");
			this.MenuGridReplaceCellMatrix.Name = "MenuGridReplaceCellMatrix";
			this.MenuGridReplaceCellMatrix.Click += new System.EventHandler(this.menuInsertMatrix_Click);
			// 
			// MenuGridReplaceCellRectangle
			// 
			resources.ApplyResources(this.MenuGridReplaceCellRectangle, "MenuGridReplaceCellRectangle");
			this.MenuGridReplaceCellRectangle.Name = "MenuGridReplaceCellRectangle";
			this.MenuGridReplaceCellRectangle.Click += new System.EventHandler(this.menuInsertRectangle_Click);
			// 
			// MenuGridReplaceCellSubreport
			// 
			resources.ApplyResources(this.MenuGridReplaceCellSubreport, "MenuGridReplaceCellSubreport");
			this.MenuGridReplaceCellSubreport.Name = "MenuGridReplaceCellSubreport";
			this.MenuGridReplaceCellSubreport.Click += new System.EventHandler(this.menuInsertSubreport_Click);
			// 
			// MenuGridReplaceCellTable
			// 
			resources.ApplyResources(this.MenuGridReplaceCellTable, "MenuGridReplaceCellTable");
			this.MenuGridReplaceCellTable.Name = "MenuGridReplaceCellTable";
			this.MenuGridReplaceCellTable.Click += new System.EventHandler(this.menuInsertTable_Click);
			// 
			// MenuGridReplaceCellTextbox
			// 
			resources.ApplyResources(this.MenuGridReplaceCellTextbox, "MenuGridReplaceCellTextbox");
			this.MenuGridReplaceCellTextbox.Name = "MenuGridReplaceCellTextbox";
			this.MenuGridReplaceCellTextbox.Click += new System.EventHandler(this.menuInsertTextbox_Click);
			// 
			// toolStripMenuItem17
			// 
			resources.ApplyResources(this.toolStripMenuItem17, "toolStripMenuItem17");
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			// 
			// MenuGridInsertColumnBefore
			// 
			resources.ApplyResources(this.MenuGridInsertColumnBefore, "MenuGridInsertColumnBefore");
			this.MenuGridInsertColumnBefore.Name = "MenuGridInsertColumnBefore";
			this.MenuGridInsertColumnBefore.Click += new System.EventHandler(this.menuTableInsertColumnBefore_Click);
			// 
			// MenuGridInsertColumnAfter
			// 
			resources.ApplyResources(this.MenuGridInsertColumnAfter, "MenuGridInsertColumnAfter");
			this.MenuGridInsertColumnAfter.Name = "MenuGridInsertColumnAfter";
			this.MenuGridInsertColumnAfter.Click += new System.EventHandler(this.menuTableInsertColumnAfter_Click);
			// 
			// toolStripMenuItem18
			// 
			resources.ApplyResources(this.toolStripMenuItem18, "toolStripMenuItem18");
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			// 
			// MenuGridInsertRowBefore
			// 
			resources.ApplyResources(this.MenuGridInsertRowBefore, "MenuGridInsertRowBefore");
			this.MenuGridInsertRowBefore.Name = "MenuGridInsertRowBefore";
			this.MenuGridInsertRowBefore.Click += new System.EventHandler(this.menuTableInsertRowBefore_Click);
			// 
			// MenuGridInsertRowAfter
			// 
			resources.ApplyResources(this.MenuGridInsertRowAfter, "MenuGridInsertRowAfter");
			this.MenuGridInsertRowAfter.Name = "MenuGridInsertRowAfter";
			this.MenuGridInsertRowAfter.Click += new System.EventHandler(this.menuTableInsertRowAfter_Click);
			// 
			// toolStripMenuItem19
			// 
			resources.ApplyResources(this.toolStripMenuItem19, "toolStripMenuItem19");
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			// 
			// MenuGridDeleteColumn
			// 
			resources.ApplyResources(this.MenuGridDeleteColumn, "MenuGridDeleteColumn");
			this.MenuGridDeleteColumn.Name = "MenuGridDeleteColumn";
			this.MenuGridDeleteColumn.Click += new System.EventHandler(this.menuTableDeleteColumn_Click);
			// 
			// MenuGridDeleteRow
			// 
			resources.ApplyResources(this.MenuGridDeleteRow, "MenuGridDeleteRow");
			this.MenuGridDeleteRow.Name = "MenuGridDeleteRow";
			this.MenuGridDeleteRow.Click += new System.EventHandler(this.menuTableDeleteRow_Click);
			// 
			// MenuGridDeleteGrid
			// 
			resources.ApplyResources(this.MenuGridDeleteGrid, "MenuGridDeleteGrid");
			this.MenuGridDeleteGrid.Name = "MenuGridDeleteGrid";
			this.MenuGridDeleteGrid.Click += new System.EventHandler(this.menuTableDelete_Click);
			// 
			// toolStripMenuItem20
			// 
			resources.ApplyResources(this.toolStripMenuItem20, "toolStripMenuItem20");
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			// 
			// MenuGridCopy
			// 
			resources.ApplyResources(this.MenuGridCopy, "MenuGridCopy");
			this.MenuGridCopy.Name = "MenuGridCopy";
			this.MenuGridCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// MenuGridPaste
			// 
			resources.ApplyResources(this.MenuGridPaste, "MenuGridPaste");
			this.MenuGridPaste.Name = "MenuGridPaste";
			this.MenuGridPaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// MenuGridDelete
			// 
			resources.ApplyResources(this.MenuGridDelete, "MenuGridDelete");
			this.MenuGridDelete.Name = "MenuGridDelete";
			this.MenuGridDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// toolStripMenuItem21
			// 
			resources.ApplyResources(this.toolStripMenuItem21, "toolStripMenuItem21");
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			// 
			// MenuGridSelectAll
			// 
			resources.ApplyResources(this.MenuGridSelectAll, "MenuGridSelectAll");
			this.MenuGridSelectAll.Name = "MenuGridSelectAll";
			this.MenuGridSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
			// 
			// ContextMenuTable
			// 
			resources.ApplyResources(this.ContextMenuTable, "ContextMenuTable");
			this.ContextMenuTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuTableProperties,
            this.MenuTableTableProperties,
            this.MenuTableReplaceCell,
            this.toolStripMenuItem22,
            this.MenuTableInsertColumnBefore,
            this.MenuTableInsertColumnAfter,
            this.toolStripMenuItem23,
            this.MenuTableInsertRowBefore,
            this.MenuTableInsertRowAfter,
            this.toolStripMenuItem24,
            this.MenuTableInsertGroup,
            this.MenuTableEditGroup,
            this.MenuTableDeleteGroup,
            this.toolStripMenuItem25,
            this.MenuTableDeleteColumn,
            this.MenuTableDeleteRow,
            this.MenuTableDeleteTable,
            this.toolStripMenuItem26,
            this.MenuTableCopy,
            this.MenuTablePaste,
            this.MenuTableDelete,
            this.toolStripMenuItem27,
            this.MenuTableSelectAll});
			this.ContextMenuTable.Name = "ContextMenuTable";
			this.ContextMenuTable.Opened += new System.EventHandler(this.menuContext_Popup);
			// 
			// MenuTableProperties
			// 
			resources.ApplyResources(this.MenuTableProperties, "MenuTableProperties");
			this.MenuTableProperties.Name = "MenuTableProperties";
			this.MenuTableProperties.Click += new System.EventHandler(this.menuProperties_Click);
			// 
			// MenuTableTableProperties
			// 
			resources.ApplyResources(this.MenuTableTableProperties, "MenuTableTableProperties");
			this.MenuTableTableProperties.Name = "MenuTableTableProperties";
			this.MenuTableTableProperties.Click += new System.EventHandler(this.menuTableProperties_Click);
			// 
			// MenuTableReplaceCell
			// 
			resources.ApplyResources(this.MenuTableReplaceCell, "MenuTableReplaceCell");
			this.MenuTableReplaceCell.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuTableReplaceCellChart,
            this.MenuTableReplaceCellImage,
            this.MenuTableReplaceCellList,
            this.MenuTableReplaceCellMatrix,
            this.MenuTableReplaceCellRectangle,
            this.MenuTableReplaceCellSubreport,
            this.MenuTableReplaceCellTable,
            this.MenuTableReplaceCellTextbox});
			this.MenuTableReplaceCell.Name = "MenuTableReplaceCell";
			// 
			// MenuTableReplaceCellChart
			// 
			resources.ApplyResources(this.MenuTableReplaceCellChart, "MenuTableReplaceCellChart");
			this.MenuTableReplaceCellChart.Name = "MenuTableReplaceCellChart";
			this.MenuTableReplaceCellChart.Click += new System.EventHandler(this.menuInsertChart_Click);
			// 
			// MenuTableReplaceCellImage
			// 
			resources.ApplyResources(this.MenuTableReplaceCellImage, "MenuTableReplaceCellImage");
			this.MenuTableReplaceCellImage.Name = "MenuTableReplaceCellImage";
			this.MenuTableReplaceCellImage.Click += new System.EventHandler(this.menuInsertImage_Click);
			// 
			// MenuTableReplaceCellList
			// 
			resources.ApplyResources(this.MenuTableReplaceCellList, "MenuTableReplaceCellList");
			this.MenuTableReplaceCellList.Name = "MenuTableReplaceCellList";
			this.MenuTableReplaceCellList.Click += new System.EventHandler(this.menuInsertList_Click);
			// 
			// MenuTableReplaceCellMatrix
			// 
			resources.ApplyResources(this.MenuTableReplaceCellMatrix, "MenuTableReplaceCellMatrix");
			this.MenuTableReplaceCellMatrix.Name = "MenuTableReplaceCellMatrix";
			this.MenuTableReplaceCellMatrix.Click += new System.EventHandler(this.menuInsertMatrix_Click);
			// 
			// MenuTableReplaceCellRectangle
			// 
			resources.ApplyResources(this.MenuTableReplaceCellRectangle, "MenuTableReplaceCellRectangle");
			this.MenuTableReplaceCellRectangle.Name = "MenuTableReplaceCellRectangle";
			this.MenuTableReplaceCellRectangle.Click += new System.EventHandler(this.menuInsertRectangle_Click);
			// 
			// MenuTableReplaceCellSubreport
			// 
			resources.ApplyResources(this.MenuTableReplaceCellSubreport, "MenuTableReplaceCellSubreport");
			this.MenuTableReplaceCellSubreport.Name = "MenuTableReplaceCellSubreport";
			this.MenuTableReplaceCellSubreport.Click += new System.EventHandler(this.menuInsertSubreport_Click);
			// 
			// MenuTableReplaceCellTable
			// 
			resources.ApplyResources(this.MenuTableReplaceCellTable, "MenuTableReplaceCellTable");
			this.MenuTableReplaceCellTable.Name = "MenuTableReplaceCellTable";
			this.MenuTableReplaceCellTable.Click += new System.EventHandler(this.menuInsertTable_Click);
			// 
			// MenuTableReplaceCellTextbox
			// 
			resources.ApplyResources(this.MenuTableReplaceCellTextbox, "MenuTableReplaceCellTextbox");
			this.MenuTableReplaceCellTextbox.Name = "MenuTableReplaceCellTextbox";
			this.MenuTableReplaceCellTextbox.Click += new System.EventHandler(this.menuInsertTextbox_Click);
			// 
			// toolStripMenuItem22
			// 
			resources.ApplyResources(this.toolStripMenuItem22, "toolStripMenuItem22");
			this.toolStripMenuItem22.Name = "toolStripMenuItem22";
			// 
			// MenuTableInsertColumnBefore
			// 
			resources.ApplyResources(this.MenuTableInsertColumnBefore, "MenuTableInsertColumnBefore");
			this.MenuTableInsertColumnBefore.Name = "MenuTableInsertColumnBefore";
			this.MenuTableInsertColumnBefore.Click += new System.EventHandler(this.menuTableInsertColumnBefore_Click);
			// 
			// MenuTableInsertColumnAfter
			// 
			resources.ApplyResources(this.MenuTableInsertColumnAfter, "MenuTableInsertColumnAfter");
			this.MenuTableInsertColumnAfter.Name = "MenuTableInsertColumnAfter";
			this.MenuTableInsertColumnAfter.Click += new System.EventHandler(this.menuTableInsertColumnAfter_Click);
			// 
			// toolStripMenuItem23
			// 
			resources.ApplyResources(this.toolStripMenuItem23, "toolStripMenuItem23");
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			// 
			// MenuTableInsertRowBefore
			// 
			resources.ApplyResources(this.MenuTableInsertRowBefore, "MenuTableInsertRowBefore");
			this.MenuTableInsertRowBefore.Name = "MenuTableInsertRowBefore";
			this.MenuTableInsertRowBefore.Click += new System.EventHandler(this.menuTableInsertRowBefore_Click);
			// 
			// MenuTableInsertRowAfter
			// 
			resources.ApplyResources(this.MenuTableInsertRowAfter, "MenuTableInsertRowAfter");
			this.MenuTableInsertRowAfter.Name = "MenuTableInsertRowAfter";
			this.MenuTableInsertRowAfter.Click += new System.EventHandler(this.menuTableInsertRowAfter_Click);
			// 
			// toolStripMenuItem24
			// 
			resources.ApplyResources(this.toolStripMenuItem24, "toolStripMenuItem24");
			this.toolStripMenuItem24.Name = "toolStripMenuItem24";
			// 
			// MenuTableInsertGroup
			// 
			resources.ApplyResources(this.MenuTableInsertGroup, "MenuTableInsertGroup");
			this.MenuTableInsertGroup.Name = "MenuTableInsertGroup";
			this.MenuTableInsertGroup.Click += new System.EventHandler(this.menuTableInsertGroup_Click);
			// 
			// MenuTableEditGroup
			// 
			resources.ApplyResources(this.MenuTableEditGroup, "MenuTableEditGroup");
			this.MenuTableEditGroup.Name = "MenuTableEditGroup";
			// 
			// MenuTableDeleteGroup
			// 
			resources.ApplyResources(this.MenuTableDeleteGroup, "MenuTableDeleteGroup");
			this.MenuTableDeleteGroup.Name = "MenuTableDeleteGroup";
			// 
			// toolStripMenuItem25
			// 
			resources.ApplyResources(this.toolStripMenuItem25, "toolStripMenuItem25");
			this.toolStripMenuItem25.Name = "toolStripMenuItem25";
			// 
			// MenuTableDeleteColumn
			// 
			resources.ApplyResources(this.MenuTableDeleteColumn, "MenuTableDeleteColumn");
			this.MenuTableDeleteColumn.Name = "MenuTableDeleteColumn";
			this.MenuTableDeleteColumn.Click += new System.EventHandler(this.menuTableDeleteColumn_Click);
			// 
			// MenuTableDeleteRow
			// 
			resources.ApplyResources(this.MenuTableDeleteRow, "MenuTableDeleteRow");
			this.MenuTableDeleteRow.Name = "MenuTableDeleteRow";
			this.MenuTableDeleteRow.Click += new System.EventHandler(this.menuTableDeleteRow_Click);
			// 
			// MenuTableDeleteTable
			// 
			resources.ApplyResources(this.MenuTableDeleteTable, "MenuTableDeleteTable");
			this.MenuTableDeleteTable.Name = "MenuTableDeleteTable";
			this.MenuTableDeleteTable.Click += new System.EventHandler(this.menuTableDelete_Click);
			// 
			// toolStripMenuItem26
			// 
			resources.ApplyResources(this.toolStripMenuItem26, "toolStripMenuItem26");
			this.toolStripMenuItem26.Name = "toolStripMenuItem26";
			// 
			// MenuTableCopy
			// 
			resources.ApplyResources(this.MenuTableCopy, "MenuTableCopy");
			this.MenuTableCopy.Name = "MenuTableCopy";
			this.MenuTableCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// MenuTablePaste
			// 
			resources.ApplyResources(this.MenuTablePaste, "MenuTablePaste");
			this.MenuTablePaste.Name = "MenuTablePaste";
			this.MenuTablePaste.Click += new System.EventHandler(this.menuPaste_Click);
			// 
			// MenuTableDelete
			// 
			resources.ApplyResources(this.MenuTableDelete, "MenuTableDelete");
			this.MenuTableDelete.Name = "MenuTableDelete";
			this.MenuTableDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// toolStripMenuItem27
			// 
			resources.ApplyResources(this.toolStripMenuItem27, "toolStripMenuItem27");
			this.toolStripMenuItem27.Name = "toolStripMenuItem27";
			// 
			// MenuTableSelectAll
			// 
			resources.ApplyResources(this.MenuTableSelectAll, "MenuTableSelectAll");
			this.MenuTableSelectAll.Name = "MenuTableSelectAll";
			this.MenuTableSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
			// 
			// DesignCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Name = "DesignCtl";
			this.ContextMenuDefault.ResumeLayout(false);
			this.ContextMenuChart.ResumeLayout(false);
			this.ContextMenuMatrix.ResumeLayout(false);
			this.ContextMenuSubreport.ResumeLayout(false);
			this.ContextMenuGrid.ResumeLayout(false);
			this.ContextMenuTable.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
