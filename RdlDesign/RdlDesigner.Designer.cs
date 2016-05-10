using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
	public partial class RdlDesigner : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		Timer _IpcTimer = null;
private MDIChild printChild=null;
private DialogValidateRdl _ValidateRdl=null;
private DockStyle _PropertiesLocation = DockStyle.Right;

     

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdlDesigner));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pDFOldStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tIFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dOCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rTFDOCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.webPageHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.webArchiveSingleFileMHTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.formatXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.designerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rDLTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showReportInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.matrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rectangleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.subReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataSetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataSourcesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.embeddedImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.createSharedDataSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.alignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.centersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.topsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.middlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bottomsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.widthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.heightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bothToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.horizontalSpacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.makeEqualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verticalSpacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.makeEqualToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.paddingLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.paddingRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.paddintTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.paddingBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.increaseToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.decreaseToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.zeroToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.validateRDLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.startDesktopServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.supportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainTB = new System.Windows.Forms.ToolStrip();
			this.newToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.openToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.cutToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.undoToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.textboxToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.chartToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.tableToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.listToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.imageToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.matrixToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.subreportToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.rectangleToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.lineToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.fxToolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.ctlEditTextbox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.boldToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.italiacToolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.underlineToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.leftAlignToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.centerAlignToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.rightAlignToolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.fontToolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.fontSizeToolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.printToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.zoomToolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
			this.selectToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.pdfToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.htmlToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.excelToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.XmlToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.MhtToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.CsvToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.RtfToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.TifToolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.mainTC = new System.Windows.Forms.TabControl();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.foreColorPicker1 = new fyiReporting.RdlDesign.ColorPicker();
			this.backColorPicker1 = new fyiReporting.RdlDesign.ColorPicker();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusSelected = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusPosition = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainSP = new System.Windows.Forms.Splitter();
			this.ContextMenuTB = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuTBClose = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTBSave = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuTBCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
			this.mainProperties = new fyiReporting.RdlDesign.PropertyCtl();
			this.menuStrip1.SuspendLayout();
			this.mainTB.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.ContextMenuTB.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.insertToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Name = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newReportToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.printToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.recentFilesToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuFile_Popup);
			// 
			// newReportToolStripMenuItem
			// 
			this.newReportToolStripMenuItem.Name = "newReportToolStripMenuItem";
			resources.ApplyResources(this.newReportToolStripMenuItem, "newReportToolStripMenuItem");
			this.newReportToolStripMenuItem.Click += new System.EventHandler(this.menuFileNewReport_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
			this.openToolStripMenuItem.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.menuFileClose_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// saveToolStripMenuItem
			// 
			resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.menuFileSave_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.menuFileSaveAs_Click);
			// 
			// printToolStripMenuItem
			// 
			resources.ApplyResources(this.printToolStripMenuItem, "printToolStripMenuItem");
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.Click += new System.EventHandler(this.menuFilePrint_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pDFToolStripMenuItem,
            this.pDFOldStyleToolStripMenuItem,
            this.tIFFToolStripMenuItem,
            this.cSVToolStripMenuItem,
            this.excelToolStripMenuItem,
            this.dOCToolStripMenuItem,
            this.rTFDOCToolStripMenuItem,
            this.xMLToolStripMenuItem,
            this.webPageHTMLToolStripMenuItem,
            this.webArchiveSingleFileMHTToolStripMenuItem});
			resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			// 
			// pDFToolStripMenuItem
			// 
			this.pDFToolStripMenuItem.Name = "pDFToolStripMenuItem";
			resources.ApplyResources(this.pDFToolStripMenuItem, "pDFToolStripMenuItem");
			this.pDFToolStripMenuItem.Click += new System.EventHandler(this.pDFToolStripMenuItem_Click);
			// 
			// pDFOldStyleToolStripMenuItem
			// 
			this.pDFOldStyleToolStripMenuItem.Name = "pDFOldStyleToolStripMenuItem";
			resources.ApplyResources(this.pDFOldStyleToolStripMenuItem, "pDFOldStyleToolStripMenuItem");
			this.pDFOldStyleToolStripMenuItem.Click += new System.EventHandler(this.pDFOldStyleToolStripMenuItem_Click);
			// 
			// tIFFToolStripMenuItem
			// 
			this.tIFFToolStripMenuItem.Name = "tIFFToolStripMenuItem";
			resources.ApplyResources(this.tIFFToolStripMenuItem, "tIFFToolStripMenuItem");
			this.tIFFToolStripMenuItem.Click += new System.EventHandler(this.tIFFToolStripMenuItem_Click);
			// 
			// cSVToolStripMenuItem
			// 
			this.cSVToolStripMenuItem.Name = "cSVToolStripMenuItem";
			resources.ApplyResources(this.cSVToolStripMenuItem, "cSVToolStripMenuItem");
			this.cSVToolStripMenuItem.Click += new System.EventHandler(this.cSVToolStripMenuItem_Click);
			// 
			// excelToolStripMenuItem
			// 
			this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
			resources.ApplyResources(this.excelToolStripMenuItem, "excelToolStripMenuItem");
			this.excelToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
			// 
			// dOCToolStripMenuItem
			// 
			this.dOCToolStripMenuItem.Name = "dOCToolStripMenuItem";
			resources.ApplyResources(this.dOCToolStripMenuItem, "dOCToolStripMenuItem");
			this.dOCToolStripMenuItem.Click += new System.EventHandler(this.dOCToolStripMenuItem_Click);
			// 
			// rTFDOCToolStripMenuItem
			// 
			this.rTFDOCToolStripMenuItem.Name = "rTFDOCToolStripMenuItem";
			resources.ApplyResources(this.rTFDOCToolStripMenuItem, "rTFDOCToolStripMenuItem");
			this.rTFDOCToolStripMenuItem.Click += new System.EventHandler(this.rTFDOCToolStripMenuItem_Click);
			// 
			// xMLToolStripMenuItem
			// 
			this.xMLToolStripMenuItem.Name = "xMLToolStripMenuItem";
			resources.ApplyResources(this.xMLToolStripMenuItem, "xMLToolStripMenuItem");
			this.xMLToolStripMenuItem.Click += new System.EventHandler(this.xMLToolStripMenuItem_Click);
			// 
			// webPageHTMLToolStripMenuItem
			// 
			this.webPageHTMLToolStripMenuItem.Name = "webPageHTMLToolStripMenuItem";
			resources.ApplyResources(this.webPageHTMLToolStripMenuItem, "webPageHTMLToolStripMenuItem");
			this.webPageHTMLToolStripMenuItem.Click += new System.EventHandler(this.webPageHTMLToolStripMenuItem_Click);
			// 
			// webArchiveSingleFileMHTToolStripMenuItem
			// 
			this.webArchiveSingleFileMHTToolStripMenuItem.Name = "webArchiveSingleFileMHTToolStripMenuItem";
			resources.ApplyResources(this.webArchiveSingleFileMHTToolStripMenuItem, "webArchiveSingleFileMHTToolStripMenuItem");
			this.webArchiveSingleFileMHTToolStripMenuItem.Click += new System.EventHandler(this.webArchiveSingleFileMHTToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// recentFilesToolStripMenuItem
			// 
			resources.ApplyResources(this.recentFilesToolStripMenuItem, "recentFilesToolStripMenuItem");
			this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator4,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator5,
            this.selectAllToolStripMenuItem,
            this.toolStripSeparator6,
            this.findToolStripMenuItem,
            this.findNextToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.goToToolStripMenuItem,
            this.toolStripSeparator7,
            this.formatXMLToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
			this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuEdit_Popup);
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.menuEditUndo_Click);
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
			this.redoToolStripMenuItem.Click += new System.EventHandler(this.menuEditRedo_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.menuEditCut_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.menuEditCopy_Click);
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.menuEditPaste_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.menuEditDelete_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.menuEditSelectAll_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			// 
			// findToolStripMenuItem
			// 
			this.findToolStripMenuItem.Name = "findToolStripMenuItem";
			resources.ApplyResources(this.findToolStripMenuItem, "findToolStripMenuItem");
			this.findToolStripMenuItem.Click += new System.EventHandler(this.menuEditFind_Click);
			// 
			// findNextToolStripMenuItem
			// 
			this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
			resources.ApplyResources(this.findNextToolStripMenuItem, "findNextToolStripMenuItem");
			this.findNextToolStripMenuItem.Click += new System.EventHandler(this.menuEditFindNext_Click);
			// 
			// replaceToolStripMenuItem
			// 
			this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
			resources.ApplyResources(this.replaceToolStripMenuItem, "replaceToolStripMenuItem");
			this.replaceToolStripMenuItem.Click += new System.EventHandler(this.menuEditReplace_Click);
			// 
			// goToToolStripMenuItem
			// 
			this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
			resources.ApplyResources(this.goToToolStripMenuItem, "goToToolStripMenuItem");
			this.goToToolStripMenuItem.Click += new System.EventHandler(this.menuEditGoto_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
			// 
			// formatXMLToolStripMenuItem
			// 
			this.formatXMLToolStripMenuItem.Name = "formatXMLToolStripMenuItem";
			resources.ApplyResources(this.formatXMLToolStripMenuItem, "formatXMLToolStripMenuItem");
			this.formatXMLToolStripMenuItem.Click += new System.EventHandler(this.menuEdit_FormatXml);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.designerToolStripMenuItem,
            this.rDLTextToolStripMenuItem,
            this.previewToolStripMenuItem,
            this.showReportInBrowserToolStripMenuItem,
            this.propertiesWindowsToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
			this.viewToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuView_Popup);
			// 
			// designerToolStripMenuItem
			// 
			this.designerToolStripMenuItem.Name = "designerToolStripMenuItem";
			resources.ApplyResources(this.designerToolStripMenuItem, "designerToolStripMenuItem");
			this.designerToolStripMenuItem.Click += new System.EventHandler(this.menuViewDesigner_Click);
			// 
			// rDLTextToolStripMenuItem
			// 
			this.rDLTextToolStripMenuItem.Name = "rDLTextToolStripMenuItem";
			resources.ApplyResources(this.rDLTextToolStripMenuItem, "rDLTextToolStripMenuItem");
			this.rDLTextToolStripMenuItem.Click += new System.EventHandler(this.menuViewRDL_Click);
			// 
			// previewToolStripMenuItem
			// 
			this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
			resources.ApplyResources(this.previewToolStripMenuItem, "previewToolStripMenuItem");
			this.previewToolStripMenuItem.Click += new System.EventHandler(this.menuViewPreview_Click);
			// 
			// showReportInBrowserToolStripMenuItem
			// 
			this.showReportInBrowserToolStripMenuItem.Name = "showReportInBrowserToolStripMenuItem";
			resources.ApplyResources(this.showReportInBrowserToolStripMenuItem, "showReportInBrowserToolStripMenuItem");
			this.showReportInBrowserToolStripMenuItem.Click += new System.EventHandler(this.menuViewBrowser_Click);
			// 
			// propertiesWindowsToolStripMenuItem
			// 
			this.propertiesWindowsToolStripMenuItem.Name = "propertiesWindowsToolStripMenuItem";
			resources.ApplyResources(this.propertiesWindowsToolStripMenuItem, "propertiesWindowsToolStripMenuItem");
			this.propertiesWindowsToolStripMenuItem.Click += new System.EventHandler(this.menuEditProperties_Click);
			// 
			// insertToolStripMenuItem
			// 
			this.insertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chartToolStripMenuItem,
            this.gridToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.listToolStripMenuItem,
            this.matrixToolStripMenuItem,
            this.rectangleToolStripMenuItem,
            this.subReportToolStripMenuItem,
            this.tableToolStripMenuItem,
            this.textboxToolStripMenuItem});
			this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
			resources.ApplyResources(this.insertToolStripMenuItem, "insertToolStripMenuItem");
			// 
			// chartToolStripMenuItem
			// 
			this.chartToolStripMenuItem.Name = "chartToolStripMenuItem";
			resources.ApplyResources(this.chartToolStripMenuItem, "chartToolStripMenuItem");
			this.chartToolStripMenuItem.Tag = "Chart";
			this.chartToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// gridToolStripMenuItem
			// 
			this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
			resources.ApplyResources(this.gridToolStripMenuItem, "gridToolStripMenuItem");
			this.gridToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// imageToolStripMenuItem
			// 
			this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
			resources.ApplyResources(this.imageToolStripMenuItem, "imageToolStripMenuItem");
			this.imageToolStripMenuItem.Tag = "Image";
			this.imageToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// lineToolStripMenuItem
			// 
			this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
			resources.ApplyResources(this.lineToolStripMenuItem, "lineToolStripMenuItem");
			this.lineToolStripMenuItem.Tag = "Line";
			this.lineToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// listToolStripMenuItem
			// 
			this.listToolStripMenuItem.Name = "listToolStripMenuItem";
			resources.ApplyResources(this.listToolStripMenuItem, "listToolStripMenuItem");
			this.listToolStripMenuItem.Tag = "List";
			this.listToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// matrixToolStripMenuItem
			// 
			this.matrixToolStripMenuItem.Name = "matrixToolStripMenuItem";
			resources.ApplyResources(this.matrixToolStripMenuItem, "matrixToolStripMenuItem");
			this.matrixToolStripMenuItem.Tag = "Matrix";
			this.matrixToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// rectangleToolStripMenuItem
			// 
			this.rectangleToolStripMenuItem.Name = "rectangleToolStripMenuItem";
			resources.ApplyResources(this.rectangleToolStripMenuItem, "rectangleToolStripMenuItem");
			this.rectangleToolStripMenuItem.Tag = "Rectangle";
			this.rectangleToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// subReportToolStripMenuItem
			// 
			this.subReportToolStripMenuItem.Name = "subReportToolStripMenuItem";
			resources.ApplyResources(this.subReportToolStripMenuItem, "subReportToolStripMenuItem");
			this.subReportToolStripMenuItem.Tag = "Subreport";
			this.subReportToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// tableToolStripMenuItem
			// 
			this.tableToolStripMenuItem.Name = "tableToolStripMenuItem";
			resources.ApplyResources(this.tableToolStripMenuItem, "tableToolStripMenuItem");
			this.tableToolStripMenuItem.Tag = "Table";
			this.tableToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// textboxToolStripMenuItem
			// 
			this.textboxToolStripMenuItem.Name = "textboxToolStripMenuItem";
			resources.ApplyResources(this.textboxToolStripMenuItem, "textboxToolStripMenuItem");
			this.textboxToolStripMenuItem.Tag = "Textbox";
			this.textboxToolStripMenuItem.Click += new System.EventHandler(this.InsertToolStripMenuItem_Click);
			// 
			// dataToolStripMenuItem
			// 
			this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataSetsToolStripMenuItem,
            this.dataSourcesToolStripMenuItem1,
            this.toolStripSeparator8,
            this.embeddedImagesToolStripMenuItem,
            this.toolStripSeparator9,
            this.createSharedDataSourceToolStripMenuItem});
			this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
			resources.ApplyResources(this.dataToolStripMenuItem, "dataToolStripMenuItem");
			this.dataToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuData_Popup);
			// 
			// dataSetsToolStripMenuItem
			// 
			this.dataSetsToolStripMenuItem.Name = "dataSetsToolStripMenuItem";
			resources.ApplyResources(this.dataSetsToolStripMenuItem, "dataSetsToolStripMenuItem");
			// 
			// dataSourcesToolStripMenuItem1
			// 
			this.dataSourcesToolStripMenuItem1.Name = "dataSourcesToolStripMenuItem1";
			resources.ApplyResources(this.dataSourcesToolStripMenuItem1, "dataSourcesToolStripMenuItem1");
			this.dataSourcesToolStripMenuItem1.Click += new System.EventHandler(this.dataSourcesToolStripMenuItem1_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
			// 
			// embeddedImagesToolStripMenuItem
			// 
			this.embeddedImagesToolStripMenuItem.Name = "embeddedImagesToolStripMenuItem";
			resources.ApplyResources(this.embeddedImagesToolStripMenuItem, "embeddedImagesToolStripMenuItem");
			this.embeddedImagesToolStripMenuItem.Click += new System.EventHandler(this.embeddedImagesToolStripMenuItem_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
			// 
			// createSharedDataSourceToolStripMenuItem
			// 
			this.createSharedDataSourceToolStripMenuItem.Name = "createSharedDataSourceToolStripMenuItem";
			resources.ApplyResources(this.createSharedDataSourceToolStripMenuItem, "createSharedDataSourceToolStripMenuItem");
			this.createSharedDataSourceToolStripMenuItem.Click += new System.EventHandler(this.menuFileNewDataSourceRef_Click);
			// 
			// formatToolStripMenuItem
			// 
			this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alignToolStripMenuItem,
            this.sizeToolStripMenuItem,
            this.horizontalSpacingToolStripMenuItem,
            this.verticalSpacingToolStripMenuItem,
            this.toolStripSeparator11,
            this.paddingLeftToolStripMenuItem,
            this.paddingRightToolStripMenuItem,
            this.paddintTopToolStripMenuItem,
            this.paddingBottomToolStripMenuItem});
			this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
			resources.ApplyResources(this.formatToolStripMenuItem, "formatToolStripMenuItem");
			this.formatToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuFormat_Popup);
			// 
			// alignToolStripMenuItem
			// 
			this.alignToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftsToolStripMenuItem,
            this.centersToolStripMenuItem,
            this.rightsToolStripMenuItem,
            this.toolStripSeparator10,
            this.topsToolStripMenuItem,
            this.middlesToolStripMenuItem,
            this.bottomsToolStripMenuItem});
			this.alignToolStripMenuItem.Name = "alignToolStripMenuItem";
			resources.ApplyResources(this.alignToolStripMenuItem, "alignToolStripMenuItem");
			// 
			// leftsToolStripMenuItem
			// 
			this.leftsToolStripMenuItem.Name = "leftsToolStripMenuItem";
			resources.ApplyResources(this.leftsToolStripMenuItem, "leftsToolStripMenuItem");
			this.leftsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignL_Click);
			// 
			// centersToolStripMenuItem
			// 
			this.centersToolStripMenuItem.Name = "centersToolStripMenuItem";
			resources.ApplyResources(this.centersToolStripMenuItem, "centersToolStripMenuItem");
			this.centersToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignC_Click);
			// 
			// rightsToolStripMenuItem
			// 
			this.rightsToolStripMenuItem.Name = "rightsToolStripMenuItem";
			resources.ApplyResources(this.rightsToolStripMenuItem, "rightsToolStripMenuItem");
			this.rightsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignR_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
			// 
			// topsToolStripMenuItem
			// 
			this.topsToolStripMenuItem.Name = "topsToolStripMenuItem";
			resources.ApplyResources(this.topsToolStripMenuItem, "topsToolStripMenuItem");
			this.topsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignT_Click);
			// 
			// middlesToolStripMenuItem
			// 
			this.middlesToolStripMenuItem.Name = "middlesToolStripMenuItem";
			resources.ApplyResources(this.middlesToolStripMenuItem, "middlesToolStripMenuItem");
			this.middlesToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignM_Click);
			// 
			// bottomsToolStripMenuItem
			// 
			this.bottomsToolStripMenuItem.Name = "bottomsToolStripMenuItem";
			resources.ApplyResources(this.bottomsToolStripMenuItem, "bottomsToolStripMenuItem");
			this.bottomsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignB_Click);
			// 
			// sizeToolStripMenuItem
			// 
			this.sizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.widthToolStripMenuItem,
            this.heightToolStripMenuItem,
            this.bothToolStripMenuItem});
			this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
			resources.ApplyResources(this.sizeToolStripMenuItem, "sizeToolStripMenuItem");
			// 
			// widthToolStripMenuItem
			// 
			this.widthToolStripMenuItem.Name = "widthToolStripMenuItem";
			resources.ApplyResources(this.widthToolStripMenuItem, "widthToolStripMenuItem");
			this.widthToolStripMenuItem.Click += new System.EventHandler(this.menuFormatSizeW_Click);
			// 
			// heightToolStripMenuItem
			// 
			this.heightToolStripMenuItem.Name = "heightToolStripMenuItem";
			resources.ApplyResources(this.heightToolStripMenuItem, "heightToolStripMenuItem");
			this.heightToolStripMenuItem.Click += new System.EventHandler(this.menuFormatSizeH_Click);
			// 
			// bothToolStripMenuItem
			// 
			this.bothToolStripMenuItem.Name = "bothToolStripMenuItem";
			resources.ApplyResources(this.bothToolStripMenuItem, "bothToolStripMenuItem");
			this.bothToolStripMenuItem.Click += new System.EventHandler(this.menuFormatSizeB_Click);
			// 
			// horizontalSpacingToolStripMenuItem
			// 
			this.horizontalSpacingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.makeEqualToolStripMenuItem,
            this.increaseToolStripMenuItem,
            this.decreaseToolStripMenuItem,
            this.zeroToolStripMenuItem});
			this.horizontalSpacingToolStripMenuItem.Name = "horizontalSpacingToolStripMenuItem";
			resources.ApplyResources(this.horizontalSpacingToolStripMenuItem, "horizontalSpacingToolStripMenuItem");
			// 
			// makeEqualToolStripMenuItem
			// 
			this.makeEqualToolStripMenuItem.Name = "makeEqualToolStripMenuItem";
			resources.ApplyResources(this.makeEqualToolStripMenuItem, "makeEqualToolStripMenuItem");
			this.makeEqualToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzE_Click);
			// 
			// increaseToolStripMenuItem
			// 
			this.increaseToolStripMenuItem.Name = "increaseToolStripMenuItem";
			resources.ApplyResources(this.increaseToolStripMenuItem, "increaseToolStripMenuItem");
			this.increaseToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzI_Click);
			// 
			// decreaseToolStripMenuItem
			// 
			this.decreaseToolStripMenuItem.Name = "decreaseToolStripMenuItem";
			resources.ApplyResources(this.decreaseToolStripMenuItem, "decreaseToolStripMenuItem");
			this.decreaseToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzD_Click);
			// 
			// zeroToolStripMenuItem
			// 
			this.zeroToolStripMenuItem.Name = "zeroToolStripMenuItem";
			resources.ApplyResources(this.zeroToolStripMenuItem, "zeroToolStripMenuItem");
			this.zeroToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzZ_Click);
			// 
			// verticalSpacingToolStripMenuItem
			// 
			this.verticalSpacingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.makeEqualToolStripMenuItem1,
            this.increaseToolStripMenuItem1,
            this.decreaseToolStripMenuItem1,
            this.zeroToolStripMenuItem1});
			this.verticalSpacingToolStripMenuItem.Name = "verticalSpacingToolStripMenuItem";
			resources.ApplyResources(this.verticalSpacingToolStripMenuItem, "verticalSpacingToolStripMenuItem");
			// 
			// makeEqualToolStripMenuItem1
			// 
			this.makeEqualToolStripMenuItem1.Name = "makeEqualToolStripMenuItem1";
			resources.ApplyResources(this.makeEqualToolStripMenuItem1, "makeEqualToolStripMenuItem1");
			this.makeEqualToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertE_Click);
			// 
			// increaseToolStripMenuItem1
			// 
			this.increaseToolStripMenuItem1.Name = "increaseToolStripMenuItem1";
			resources.ApplyResources(this.increaseToolStripMenuItem1, "increaseToolStripMenuItem1");
			this.increaseToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertI_Click);
			// 
			// decreaseToolStripMenuItem1
			// 
			this.decreaseToolStripMenuItem1.Name = "decreaseToolStripMenuItem1";
			resources.ApplyResources(this.decreaseToolStripMenuItem1, "decreaseToolStripMenuItem1");
			this.decreaseToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertD_Click);
			// 
			// zeroToolStripMenuItem1
			// 
			this.zeroToolStripMenuItem1.Name = "zeroToolStripMenuItem1";
			resources.ApplyResources(this.zeroToolStripMenuItem1, "zeroToolStripMenuItem1");
			this.zeroToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertZ_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
			// 
			// paddingLeftToolStripMenuItem
			// 
			this.paddingLeftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem2,
            this.decreaseToolStripMenuItem2,
            this.zeroToolStripMenuItem2});
			this.paddingLeftToolStripMenuItem.Name = "paddingLeftToolStripMenuItem";
			resources.ApplyResources(this.paddingLeftToolStripMenuItem, "paddingLeftToolStripMenuItem");
			// 
			// increaseToolStripMenuItem2
			// 
			this.increaseToolStripMenuItem2.Name = "increaseToolStripMenuItem2";
			resources.ApplyResources(this.increaseToolStripMenuItem2, "increaseToolStripMenuItem2");
			this.increaseToolStripMenuItem2.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// decreaseToolStripMenuItem2
			// 
			this.decreaseToolStripMenuItem2.Name = "decreaseToolStripMenuItem2";
			resources.ApplyResources(this.decreaseToolStripMenuItem2, "decreaseToolStripMenuItem2");
			this.decreaseToolStripMenuItem2.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// zeroToolStripMenuItem2
			// 
			this.zeroToolStripMenuItem2.Name = "zeroToolStripMenuItem2";
			resources.ApplyResources(this.zeroToolStripMenuItem2, "zeroToolStripMenuItem2");
			this.zeroToolStripMenuItem2.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// paddingRightToolStripMenuItem
			// 
			this.paddingRightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem3,
            this.decreaseToolStripMenuItem3,
            this.zeroToolStripMenuItem3});
			this.paddingRightToolStripMenuItem.Name = "paddingRightToolStripMenuItem";
			resources.ApplyResources(this.paddingRightToolStripMenuItem, "paddingRightToolStripMenuItem");
			// 
			// increaseToolStripMenuItem3
			// 
			this.increaseToolStripMenuItem3.Name = "increaseToolStripMenuItem3";
			resources.ApplyResources(this.increaseToolStripMenuItem3, "increaseToolStripMenuItem3");
			this.increaseToolStripMenuItem3.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// decreaseToolStripMenuItem3
			// 
			this.decreaseToolStripMenuItem3.Name = "decreaseToolStripMenuItem3";
			resources.ApplyResources(this.decreaseToolStripMenuItem3, "decreaseToolStripMenuItem3");
			this.decreaseToolStripMenuItem3.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// zeroToolStripMenuItem3
			// 
			this.zeroToolStripMenuItem3.Name = "zeroToolStripMenuItem3";
			resources.ApplyResources(this.zeroToolStripMenuItem3, "zeroToolStripMenuItem3");
			this.zeroToolStripMenuItem3.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// paddintTopToolStripMenuItem
			// 
			this.paddintTopToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem4,
            this.decreaseToolStripMenuItem4,
            this.zeroToolStripMenuItem4});
			this.paddintTopToolStripMenuItem.Name = "paddintTopToolStripMenuItem";
			resources.ApplyResources(this.paddintTopToolStripMenuItem, "paddintTopToolStripMenuItem");
			// 
			// increaseToolStripMenuItem4
			// 
			this.increaseToolStripMenuItem4.Name = "increaseToolStripMenuItem4";
			resources.ApplyResources(this.increaseToolStripMenuItem4, "increaseToolStripMenuItem4");
			this.increaseToolStripMenuItem4.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// decreaseToolStripMenuItem4
			// 
			this.decreaseToolStripMenuItem4.Name = "decreaseToolStripMenuItem4";
			resources.ApplyResources(this.decreaseToolStripMenuItem4, "decreaseToolStripMenuItem4");
			this.decreaseToolStripMenuItem4.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// zeroToolStripMenuItem4
			// 
			this.zeroToolStripMenuItem4.Name = "zeroToolStripMenuItem4";
			resources.ApplyResources(this.zeroToolStripMenuItem4, "zeroToolStripMenuItem4");
			this.zeroToolStripMenuItem4.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// paddingBottomToolStripMenuItem
			// 
			this.paddingBottomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem5,
            this.decreaseToolStripMenuItem5,
            this.zeroToolStripMenuItem5});
			this.paddingBottomToolStripMenuItem.Name = "paddingBottomToolStripMenuItem";
			resources.ApplyResources(this.paddingBottomToolStripMenuItem, "paddingBottomToolStripMenuItem");
			// 
			// increaseToolStripMenuItem5
			// 
			this.increaseToolStripMenuItem5.Name = "increaseToolStripMenuItem5";
			resources.ApplyResources(this.increaseToolStripMenuItem5, "increaseToolStripMenuItem5");
			this.increaseToolStripMenuItem5.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// decreaseToolStripMenuItem5
			// 
			this.decreaseToolStripMenuItem5.Name = "decreaseToolStripMenuItem5";
			resources.ApplyResources(this.decreaseToolStripMenuItem5, "decreaseToolStripMenuItem5");
			this.decreaseToolStripMenuItem5.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// zeroToolStripMenuItem5
			// 
			this.zeroToolStripMenuItem5.Name = "zeroToolStripMenuItem5";
			resources.ApplyResources(this.zeroToolStripMenuItem5, "zeroToolStripMenuItem5");
			this.zeroToolStripMenuItem5.Click += new System.EventHandler(this.menuFormatPadding_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.validateRDLToolStripMenuItem,
            this.toolStripSeparator12,
            this.startDesktopServerToolStripMenuItem,
            this.toolStripSeparator13,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
			this.toolsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuTools_Popup);
			// 
			// validateRDLToolStripMenuItem
			// 
			this.validateRDLToolStripMenuItem.Name = "validateRDLToolStripMenuItem";
			resources.ApplyResources(this.validateRDLToolStripMenuItem, "validateRDLToolStripMenuItem");
			this.validateRDLToolStripMenuItem.Click += new System.EventHandler(this.menuToolsValidateSchema_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
			// 
			// startDesktopServerToolStripMenuItem
			// 
			this.startDesktopServerToolStripMenuItem.Name = "startDesktopServerToolStripMenuItem";
			resources.ApplyResources(this.startDesktopServerToolStripMenuItem, "startDesktopServerToolStripMenuItem");
			this.startDesktopServerToolStripMenuItem.Click += new System.EventHandler(this.menuToolsProcess_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
			this.optionsToolStripMenuItem.Click += new System.EventHandler(this.menuToolsOptions_Click);
			// 
			// windowToolStripMenuItem
			// 
			this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileToolStripMenuItem,
            this.closeAllToolStripMenuItem});
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
			this.windowToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuWnd_Popup);
			// 
			// cascadeToolStripMenuItem
			// 
			this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
			resources.ApplyResources(this.cascadeToolStripMenuItem, "cascadeToolStripMenuItem");
			this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.menuWndCascade_Click);
			// 
			// tileToolStripMenuItem
			// 
			this.tileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontalToolStripMenuItem,
            this.verticallyToolStripMenuItem});
			this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
			resources.ApplyResources(this.tileToolStripMenuItem, "tileToolStripMenuItem");
			// 
			// horizontalToolStripMenuItem
			// 
			this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
			resources.ApplyResources(this.horizontalToolStripMenuItem, "horizontalToolStripMenuItem");
			this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.menuWndTileH_Click);
			// 
			// verticallyToolStripMenuItem
			// 
			this.verticallyToolStripMenuItem.Name = "verticallyToolStripMenuItem";
			resources.ApplyResources(this.verticallyToolStripMenuItem, "verticallyToolStripMenuItem");
			this.verticallyToolStripMenuItem.Click += new System.EventHandler(this.menuWndTileV_Click);
			// 
			// closeAllToolStripMenuItem
			// 
			this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
			resources.ApplyResources(this.closeAllToolStripMenuItem, "closeAllToolStripMenuItem");
			this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.menuWndCloseAll_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.supportToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			// 
			// helpToolStripMenuItem1
			// 
			this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
			resources.ApplyResources(this.helpToolStripMenuItem1, "helpToolStripMenuItem1");
			this.helpToolStripMenuItem1.Click += new System.EventHandler(this.menuHelpHelp_Click);
			// 
			// supportToolStripMenuItem
			// 
			this.supportToolStripMenuItem.Name = "supportToolStripMenuItem";
			resources.ApplyResources(this.supportToolStripMenuItem, "supportToolStripMenuItem");
			this.supportToolStripMenuItem.Click += new System.EventHandler(this.menuHelpSupport_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.menuHelpAbout_Click);
			// 
			// mainTB
			// 
			this.mainTB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton1,
            this.openToolStripButton1,
            this.saveToolStripButton1,
            this.cutToolStripButton1,
            this.copyToolStripButton1,
            this.pasteToolStripButton1,
            this.undoToolStripButton1,
            this.textboxToolStripButton1,
            this.chartToolStripButton1,
            this.tableToolStripButton1,
            this.listToolStripButton1,
            this.imageToolStripButton1,
            this.matrixToolStripButton1,
            this.subreportToolStripButton1,
            this.rectangleToolStripButton1,
            this.lineToolStripButton1,
            this.fxToolStripLabel1,
            this.ctlEditTextbox});
			resources.ApplyResources(this.mainTB, "mainTB");
			this.mainTB.Name = "mainTB";
			// 
			// newToolStripButton1
			// 
			this.newToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.document_new;
			resources.ApplyResources(this.newToolStripButton1, "newToolStripButton1");
			this.newToolStripButton1.Name = "newToolStripButton1";
			this.newToolStripButton1.Tag = "New";
			this.newToolStripButton1.Click += new System.EventHandler(this.menuFileNewReport_Click);
			// 
			// openToolStripButton1
			// 
			this.openToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.document_open;
			resources.ApplyResources(this.openToolStripButton1, "openToolStripButton1");
			this.openToolStripButton1.Name = "openToolStripButton1";
			this.openToolStripButton1.Tag = "Open";
			this.openToolStripButton1.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// saveToolStripButton1
			// 
			this.saveToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.document_save;
			resources.ApplyResources(this.saveToolStripButton1, "saveToolStripButton1");
			this.saveToolStripButton1.Name = "saveToolStripButton1";
			this.saveToolStripButton1.Tag = "Save";
			this.saveToolStripButton1.Click += new System.EventHandler(this.menuFileSave_Click);
			// 
			// cutToolStripButton1
			// 
			this.cutToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cutToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.edit_cut;
			resources.ApplyResources(this.cutToolStripButton1, "cutToolStripButton1");
			this.cutToolStripButton1.Name = "cutToolStripButton1";
			this.cutToolStripButton1.Tag = "Cut";
			this.cutToolStripButton1.Click += new System.EventHandler(this.menuEditCut_Click);
			// 
			// copyToolStripButton1
			// 
			this.copyToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.copyToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.edit_copy;
			resources.ApplyResources(this.copyToolStripButton1, "copyToolStripButton1");
			this.copyToolStripButton1.Name = "copyToolStripButton1";
			this.copyToolStripButton1.Tag = "Copy";
			this.copyToolStripButton1.Click += new System.EventHandler(this.menuEditCopy_Click);
			// 
			// pasteToolStripButton1
			// 
			this.pasteToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pasteToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.edit_paste;
			resources.ApplyResources(this.pasteToolStripButton1, "pasteToolStripButton1");
			this.pasteToolStripButton1.Name = "pasteToolStripButton1";
			this.pasteToolStripButton1.Tag = "Paste";
			this.pasteToolStripButton1.Click += new System.EventHandler(this.menuEditPaste_Click);
			// 
			// undoToolStripButton1
			// 
			this.undoToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.undoToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.edit_undo;
			resources.ApplyResources(this.undoToolStripButton1, "undoToolStripButton1");
			this.undoToolStripButton1.Name = "undoToolStripButton1";
			this.undoToolStripButton1.Tag = "Undo";
			this.undoToolStripButton1.Click += new System.EventHandler(this.menuEditUndo_Click);
			// 
			// textboxToolStripButton1
			// 
			this.textboxToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.textboxToolStripButton1, "textboxToolStripButton1");
			this.textboxToolStripButton1.Name = "textboxToolStripButton1";
			this.textboxToolStripButton1.Tag = "Textbox";
			this.textboxToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// chartToolStripButton1
			// 
			this.chartToolStripButton1.CheckOnClick = true;
			this.chartToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.chart;
			resources.ApplyResources(this.chartToolStripButton1, "chartToolStripButton1");
			this.chartToolStripButton1.Name = "chartToolStripButton1";
			this.chartToolStripButton1.Tag = "Chart";
			this.chartToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// tableToolStripButton1
			// 
			this.tableToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.tableToolStripButton1, "tableToolStripButton1");
			this.tableToolStripButton1.Name = "tableToolStripButton1";
			this.tableToolStripButton1.Tag = "Table";
			this.tableToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// listToolStripButton1
			// 
			this.listToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.listToolStripButton1, "listToolStripButton1");
			this.listToolStripButton1.Name = "listToolStripButton1";
			this.listToolStripButton1.Tag = "List";
			this.listToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// imageToolStripButton1
			// 
			this.imageToolStripButton1.CheckOnClick = true;
			this.imageToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.Image;
			resources.ApplyResources(this.imageToolStripButton1, "imageToolStripButton1");
			this.imageToolStripButton1.Name = "imageToolStripButton1";
			this.imageToolStripButton1.Tag = "Image";
			this.imageToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// matrixToolStripButton1
			// 
			this.matrixToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.matrixToolStripButton1, "matrixToolStripButton1");
			this.matrixToolStripButton1.Name = "matrixToolStripButton1";
			this.matrixToolStripButton1.Tag = "Matrix";
			this.matrixToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// subreportToolStripButton1
			// 
			this.subreportToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.subreportToolStripButton1, "subreportToolStripButton1");
			this.subreportToolStripButton1.Name = "subreportToolStripButton1";
			this.subreportToolStripButton1.Tag = "Subreport";
			this.subreportToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// rectangleToolStripButton1
			// 
			this.rectangleToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.rectangleToolStripButton1, "rectangleToolStripButton1");
			this.rectangleToolStripButton1.Name = "rectangleToolStripButton1";
			this.rectangleToolStripButton1.Tag = "Rectangle";
			this.rectangleToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// lineToolStripButton1
			// 
			this.lineToolStripButton1.CheckOnClick = true;
			resources.ApplyResources(this.lineToolStripButton1, "lineToolStripButton1");
			this.lineToolStripButton1.Name = "lineToolStripButton1";
			this.lineToolStripButton1.Tag = "Line";
			this.lineToolStripButton1.Click += new System.EventHandler(this.Insert_Click);
			// 
			// fxToolStripLabel1
			// 
			resources.ApplyResources(this.fxToolStripLabel1, "fxToolStripLabel1");
			this.fxToolStripLabel1.Name = "fxToolStripLabel1";
			this.fxToolStripLabel1.Tag = "fx";
			this.fxToolStripLabel1.Click += new System.EventHandler(this.fxExpr_Click);
			this.fxToolStripLabel1.MouseEnter += new System.EventHandler(this.fxExpr_MouseEnter);
			this.fxToolStripLabel1.MouseLeave += new System.EventHandler(this.fxExpr_MouseLeave);
			// 
			// ctlEditTextbox
			// 
			this.ctlEditTextbox.Name = "ctlEditTextbox";
			resources.ApplyResources(this.ctlEditTextbox, "ctlEditTextbox");
			this.ctlEditTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditTextBox_KeyDown);
			this.ctlEditTextbox.Validated += new System.EventHandler(this.EditTextbox_Validated);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boldToolStripButton1,
            this.italiacToolStripButton1,
            this.underlineToolStripButton2,
            this.leftAlignToolStripButton2,
            this.centerAlignToolStripButton2,
            this.rightAlignToolStripButton3,
            this.fontToolStripComboBox1,
            this.fontSizeToolStripComboBox1,
            this.printToolStripButton2,
            this.zoomToolStripComboBox1,
            this.selectToolStripButton2,
            this.pdfToolStripButton2,
            this.htmlToolStripButton2,
            this.excelToolStripButton2,
            this.XmlToolStripButton2,
            this.MhtToolStripButton2,
            this.CsvToolStripButton2,
            this.RtfToolStripButton2,
            this.TifToolStripButton2});
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// boldToolStripButton1
			// 
			this.boldToolStripButton1.CheckOnClick = true;
			this.boldToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.boldToolStripButton1, "boldToolStripButton1");
			this.boldToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.format_text_bold;
			this.boldToolStripButton1.Name = "boldToolStripButton1";
			this.boldToolStripButton1.Tag = "bold";
			// 
			// italiacToolStripButton1
			// 
			this.italiacToolStripButton1.CheckOnClick = true;
			this.italiacToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.italiacToolStripButton1, "italiacToolStripButton1");
			this.italiacToolStripButton1.Image = global::fyiReporting.RdlDesign.Properties.Resources.format_text_italic;
			this.italiacToolStripButton1.Name = "italiacToolStripButton1";
			this.italiacToolStripButton1.Tag = "italic";
			this.italiacToolStripButton1.Click += new System.EventHandler(this.ctlItalic_Click);
			// 
			// underlineToolStripButton2
			// 
			this.underlineToolStripButton2.CheckOnClick = true;
			this.underlineToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.underlineToolStripButton2, "underlineToolStripButton2");
			this.underlineToolStripButton2.Image = global::fyiReporting.RdlDesign.Properties.Resources.format_text_underline;
			this.underlineToolStripButton2.Name = "underlineToolStripButton2";
			this.underlineToolStripButton2.Tag = "underline";
			this.underlineToolStripButton2.Click += new System.EventHandler(this.ctlUnderline_Click);
			// 
			// leftAlignToolStripButton2
			// 
			this.leftAlignToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.leftAlignToolStripButton2.Image = global::fyiReporting.RdlDesign.Properties.Resources.format_justify_left;
			resources.ApplyResources(this.leftAlignToolStripButton2, "leftAlignToolStripButton2");
			this.leftAlignToolStripButton2.Name = "leftAlignToolStripButton2";
			this.leftAlignToolStripButton2.Tag = "Left Align";
			this.leftAlignToolStripButton2.Click += new System.EventHandler(this.bottomsToolStripMenuItemutton_Click);
			// 
			// centerAlignToolStripButton2
			// 
			this.centerAlignToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.centerAlignToolStripButton2.Image = global::fyiReporting.RdlDesign.Properties.Resources.format_justify_center;
			resources.ApplyResources(this.centerAlignToolStripButton2, "centerAlignToolStripButton2");
			this.centerAlignToolStripButton2.Name = "centerAlignToolStripButton2";
			this.centerAlignToolStripButton2.Tag = "Center Align";
			this.centerAlignToolStripButton2.Click += new System.EventHandler(this.bottomsToolStripMenuItemutton_Click);
			// 
			// rightAlignToolStripButton3
			// 
			this.rightAlignToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.rightAlignToolStripButton3.Image = global::fyiReporting.RdlDesign.Properties.Resources.format_justify_right;
			resources.ApplyResources(this.rightAlignToolStripButton3, "rightAlignToolStripButton3");
			this.rightAlignToolStripButton3.Name = "rightAlignToolStripButton3";
			this.rightAlignToolStripButton3.Tag = "Right Align";
			this.rightAlignToolStripButton3.Click += new System.EventHandler(this.bottomsToolStripMenuItemutton_Click);
			// 
			// fontToolStripComboBox1
			// 
			this.fontToolStripComboBox1.Name = "fontToolStripComboBox1";
			resources.ApplyResources(this.fontToolStripComboBox1, "fontToolStripComboBox1");
			this.fontToolStripComboBox1.Tag = "Font";
			this.fontToolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.ctlFont_Change);
			this.fontToolStripComboBox1.Validated += new System.EventHandler(this.ctlFont_Change);
			// 
			// fontSizeToolStripComboBox1
			// 
			this.fontSizeToolStripComboBox1.Name = "fontSizeToolStripComboBox1";
			resources.ApplyResources(this.fontSizeToolStripComboBox1, "fontSizeToolStripComboBox1");
			this.fontSizeToolStripComboBox1.Tag = "Font Size";
			this.fontSizeToolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.ctlFontSize_Change);
			this.fontSizeToolStripComboBox1.Validated += new System.EventHandler(this.ctlFontSize_Change);
			// 
			// printToolStripButton2
			// 
			this.printToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.printToolStripButton2.Image = global::fyiReporting.RdlDesign.Properties.Resources.document_print;
			resources.ApplyResources(this.printToolStripButton2, "printToolStripButton2");
			this.printToolStripButton2.Name = "printToolStripButton2";
			this.printToolStripButton2.Tag = "Print";
			this.printToolStripButton2.Click += new System.EventHandler(this.menuFilePrint_Click);
			// 
			// zoomToolStripComboBox1
			// 
			this.zoomToolStripComboBox1.Name = "zoomToolStripComboBox1";
			resources.ApplyResources(this.zoomToolStripComboBox1, "zoomToolStripComboBox1");
			this.zoomToolStripComboBox1.Tag = "Zoom";
			this.zoomToolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.ctlZoom_Change);
			this.zoomToolStripComboBox1.Validated += new System.EventHandler(this.ctlZoom_Change);
			// 
			// selectToolStripButton2
			// 
			this.selectToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.selectToolStripButton2, "selectToolStripButton2");
			this.selectToolStripButton2.Name = "selectToolStripButton2";
			this.selectToolStripButton2.Tag = "Select Tool";
			this.selectToolStripButton2.Click += new System.EventHandler(this.ctlSelectTool_Click);
			// 
			// pdfToolStripButton2
			// 
			this.pdfToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.pdfToolStripButton2, "pdfToolStripButton2");
			this.pdfToolStripButton2.Name = "pdfToolStripButton2";
			this.pdfToolStripButton2.Tag = "PDF";
			this.pdfToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemPdf_Click);
			// 
			// htmlToolStripButton2
			// 
			this.htmlToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.htmlToolStripButton2, "htmlToolStripButton2");
			this.htmlToolStripButton2.Name = "htmlToolStripButton2";
			this.htmlToolStripButton2.Tag = "HTML";
			this.htmlToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemHtml_Click);
			// 
			// excelToolStripButton2
			// 
			this.excelToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.excelToolStripButton2, "excelToolStripButton2");
			this.excelToolStripButton2.Name = "excelToolStripButton2";
			this.excelToolStripButton2.Tag = "Excel";
			this.excelToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemExcel_Click);
			// 
			// XmlToolStripButton2
			// 
			this.XmlToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.XmlToolStripButton2, "XmlToolStripButton2");
			this.XmlToolStripButton2.Name = "XmlToolStripButton2";
			this.XmlToolStripButton2.Tag = "XML";
			this.XmlToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemXml_Click);
			// 
			// MhtToolStripButton2
			// 
			this.MhtToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.MhtToolStripButton2, "MhtToolStripButton2");
			this.MhtToolStripButton2.Name = "MhtToolStripButton2";
			this.MhtToolStripButton2.Tag = "MHT";
			this.MhtToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemMHtml_Click);
			// 
			// CsvToolStripButton2
			// 
			this.CsvToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.CsvToolStripButton2, "CsvToolStripButton2");
			this.CsvToolStripButton2.Name = "CsvToolStripButton2";
			this.CsvToolStripButton2.Tag = "CSV";
			this.CsvToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemCsv_Click);
			// 
			// RtfToolStripButton2
			// 
			this.RtfToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.RtfToolStripButton2, "RtfToolStripButton2");
			this.RtfToolStripButton2.Name = "RtfToolStripButton2";
			this.RtfToolStripButton2.Tag = "RTF";
			this.RtfToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemRtf_Click);
			// 
			// TifToolStripButton2
			// 
			this.TifToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.TifToolStripButton2, "TifToolStripButton2");
			this.TifToolStripButton2.Name = "TifToolStripButton2";
			this.TifToolStripButton2.Tag = "TIF";
			this.TifToolStripButton2.Click += new System.EventHandler(this.exportToolStripMenuItemTif_Click);
			// 
			// mainTC
			// 
			resources.ApplyResources(this.mainTC, "mainTC");
			this.mainTC.Name = "mainTC";
			this.mainTC.SelectedIndex = 0;
			this.mainTC.SelectedIndexChanged += new System.EventHandler(this.mainTC_SelectedIndexChanged);
			this.mainTC.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mainTC_MouseClick);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.foreColorPicker1);
			this.panel1.Controls.Add(this.backColorPicker1);
			this.panel1.Controls.Add(this.mainTC);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// foreColorPicker1
			// 
			this.foreColorPicker1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.foreColorPicker1.DropDownHeight = 1;
			this.foreColorPicker1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.foreColorPicker1, "foreColorPicker1");
			this.foreColorPicker1.FormattingEnabled = true;
			this.foreColorPicker1.Items.AddRange(new object[] {
            resources.GetString("foreColorPicker1.Items"),
            resources.GetString("foreColorPicker1.Items1"),
            resources.GetString("foreColorPicker1.Items2"),
            resources.GetString("foreColorPicker1.Items3"),
            resources.GetString("foreColorPicker1.Items4"),
            resources.GetString("foreColorPicker1.Items5"),
            resources.GetString("foreColorPicker1.Items6"),
            resources.GetString("foreColorPicker1.Items7"),
            resources.GetString("foreColorPicker1.Items8"),
            resources.GetString("foreColorPicker1.Items9"),
            resources.GetString("foreColorPicker1.Items10"),
            resources.GetString("foreColorPicker1.Items11"),
            resources.GetString("foreColorPicker1.Items12"),
            resources.GetString("foreColorPicker1.Items13"),
            resources.GetString("foreColorPicker1.Items14"),
            resources.GetString("foreColorPicker1.Items15"),
            resources.GetString("foreColorPicker1.Items16"),
            resources.GetString("foreColorPicker1.Items17"),
            resources.GetString("foreColorPicker1.Items18"),
            resources.GetString("foreColorPicker1.Items19"),
            resources.GetString("foreColorPicker1.Items20"),
            resources.GetString("foreColorPicker1.Items21"),
            resources.GetString("foreColorPicker1.Items22"),
            resources.GetString("foreColorPicker1.Items23"),
            resources.GetString("foreColorPicker1.Items24"),
            resources.GetString("foreColorPicker1.Items25"),
            resources.GetString("foreColorPicker1.Items26"),
            resources.GetString("foreColorPicker1.Items27"),
            resources.GetString("foreColorPicker1.Items28"),
            resources.GetString("foreColorPicker1.Items29"),
            resources.GetString("foreColorPicker1.Items30"),
            resources.GetString("foreColorPicker1.Items31"),
            resources.GetString("foreColorPicker1.Items32"),
            resources.GetString("foreColorPicker1.Items33"),
            resources.GetString("foreColorPicker1.Items34"),
            resources.GetString("foreColorPicker1.Items35"),
            resources.GetString("foreColorPicker1.Items36"),
            resources.GetString("foreColorPicker1.Items37"),
            resources.GetString("foreColorPicker1.Items38"),
            resources.GetString("foreColorPicker1.Items39"),
            resources.GetString("foreColorPicker1.Items40"),
            resources.GetString("foreColorPicker1.Items41"),
            resources.GetString("foreColorPicker1.Items42"),
            resources.GetString("foreColorPicker1.Items43"),
            resources.GetString("foreColorPicker1.Items44"),
            resources.GetString("foreColorPicker1.Items45"),
            resources.GetString("foreColorPicker1.Items46"),
            resources.GetString("foreColorPicker1.Items47"),
            resources.GetString("foreColorPicker1.Items48"),
            resources.GetString("foreColorPicker1.Items49"),
            resources.GetString("foreColorPicker1.Items50"),
            resources.GetString("foreColorPicker1.Items51"),
            resources.GetString("foreColorPicker1.Items52"),
            resources.GetString("foreColorPicker1.Items53"),
            resources.GetString("foreColorPicker1.Items54"),
            resources.GetString("foreColorPicker1.Items55"),
            resources.GetString("foreColorPicker1.Items56"),
            resources.GetString("foreColorPicker1.Items57"),
            resources.GetString("foreColorPicker1.Items58"),
            resources.GetString("foreColorPicker1.Items59"),
            resources.GetString("foreColorPicker1.Items60"),
            resources.GetString("foreColorPicker1.Items61"),
            resources.GetString("foreColorPicker1.Items62"),
            resources.GetString("foreColorPicker1.Items63"),
            resources.GetString("foreColorPicker1.Items64"),
            resources.GetString("foreColorPicker1.Items65"),
            resources.GetString("foreColorPicker1.Items66"),
            resources.GetString("foreColorPicker1.Items67"),
            resources.GetString("foreColorPicker1.Items68"),
            resources.GetString("foreColorPicker1.Items69"),
            resources.GetString("foreColorPicker1.Items70"),
            resources.GetString("foreColorPicker1.Items71"),
            resources.GetString("foreColorPicker1.Items72"),
            resources.GetString("foreColorPicker1.Items73"),
            resources.GetString("foreColorPicker1.Items74"),
            resources.GetString("foreColorPicker1.Items75"),
            resources.GetString("foreColorPicker1.Items76"),
            resources.GetString("foreColorPicker1.Items77"),
            resources.GetString("foreColorPicker1.Items78"),
            resources.GetString("foreColorPicker1.Items79"),
            resources.GetString("foreColorPicker1.Items80"),
            resources.GetString("foreColorPicker1.Items81"),
            resources.GetString("foreColorPicker1.Items82"),
            resources.GetString("foreColorPicker1.Items83"),
            resources.GetString("foreColorPicker1.Items84"),
            resources.GetString("foreColorPicker1.Items85"),
            resources.GetString("foreColorPicker1.Items86"),
            resources.GetString("foreColorPicker1.Items87"),
            resources.GetString("foreColorPicker1.Items88"),
            resources.GetString("foreColorPicker1.Items89"),
            resources.GetString("foreColorPicker1.Items90"),
            resources.GetString("foreColorPicker1.Items91"),
            resources.GetString("foreColorPicker1.Items92"),
            resources.GetString("foreColorPicker1.Items93"),
            resources.GetString("foreColorPicker1.Items94"),
            resources.GetString("foreColorPicker1.Items95"),
            resources.GetString("foreColorPicker1.Items96"),
            resources.GetString("foreColorPicker1.Items97"),
            resources.GetString("foreColorPicker1.Items98"),
            resources.GetString("foreColorPicker1.Items99"),
            resources.GetString("foreColorPicker1.Items100"),
            resources.GetString("foreColorPicker1.Items101"),
            resources.GetString("foreColorPicker1.Items102"),
            resources.GetString("foreColorPicker1.Items103"),
            resources.GetString("foreColorPicker1.Items104"),
            resources.GetString("foreColorPicker1.Items105"),
            resources.GetString("foreColorPicker1.Items106"),
            resources.GetString("foreColorPicker1.Items107"),
            resources.GetString("foreColorPicker1.Items108"),
            resources.GetString("foreColorPicker1.Items109"),
            resources.GetString("foreColorPicker1.Items110"),
            resources.GetString("foreColorPicker1.Items111"),
            resources.GetString("foreColorPicker1.Items112"),
            resources.GetString("foreColorPicker1.Items113"),
            resources.GetString("foreColorPicker1.Items114"),
            resources.GetString("foreColorPicker1.Items115"),
            resources.GetString("foreColorPicker1.Items116"),
            resources.GetString("foreColorPicker1.Items117"),
            resources.GetString("foreColorPicker1.Items118"),
            resources.GetString("foreColorPicker1.Items119"),
            resources.GetString("foreColorPicker1.Items120"),
            resources.GetString("foreColorPicker1.Items121"),
            resources.GetString("foreColorPicker1.Items122"),
            resources.GetString("foreColorPicker1.Items123"),
            resources.GetString("foreColorPicker1.Items124"),
            resources.GetString("foreColorPicker1.Items125"),
            resources.GetString("foreColorPicker1.Items126"),
            resources.GetString("foreColorPicker1.Items127"),
            resources.GetString("foreColorPicker1.Items128"),
            resources.GetString("foreColorPicker1.Items129"),
            resources.GetString("foreColorPicker1.Items130"),
            resources.GetString("foreColorPicker1.Items131"),
            resources.GetString("foreColorPicker1.Items132"),
            resources.GetString("foreColorPicker1.Items133"),
            resources.GetString("foreColorPicker1.Items134"),
            resources.GetString("foreColorPicker1.Items135"),
            resources.GetString("foreColorPicker1.Items136"),
            resources.GetString("foreColorPicker1.Items137"),
            resources.GetString("foreColorPicker1.Items138"),
            resources.GetString("foreColorPicker1.Items139"),
            resources.GetString("foreColorPicker1.Items140"),
            resources.GetString("foreColorPicker1.Items141"),
            resources.GetString("foreColorPicker1.Items142"),
            resources.GetString("foreColorPicker1.Items143"),
            resources.GetString("foreColorPicker1.Items144"),
            resources.GetString("foreColorPicker1.Items145"),
            resources.GetString("foreColorPicker1.Items146"),
            resources.GetString("foreColorPicker1.Items147"),
            resources.GetString("foreColorPicker1.Items148"),
            resources.GetString("foreColorPicker1.Items149"),
            resources.GetString("foreColorPicker1.Items150"),
            resources.GetString("foreColorPicker1.Items151"),
            resources.GetString("foreColorPicker1.Items152"),
            resources.GetString("foreColorPicker1.Items153"),
            resources.GetString("foreColorPicker1.Items154"),
            resources.GetString("foreColorPicker1.Items155"),
            resources.GetString("foreColorPicker1.Items156"),
            resources.GetString("foreColorPicker1.Items157"),
            resources.GetString("foreColorPicker1.Items158"),
            resources.GetString("foreColorPicker1.Items159"),
            resources.GetString("foreColorPicker1.Items160"),
            resources.GetString("foreColorPicker1.Items161"),
            resources.GetString("foreColorPicker1.Items162"),
            resources.GetString("foreColorPicker1.Items163"),
            resources.GetString("foreColorPicker1.Items164"),
            resources.GetString("foreColorPicker1.Items165"),
            resources.GetString("foreColorPicker1.Items166"),
            resources.GetString("foreColorPicker1.Items167"),
            resources.GetString("foreColorPicker1.Items168"),
            resources.GetString("foreColorPicker1.Items169"),
            resources.GetString("foreColorPicker1.Items170"),
            resources.GetString("foreColorPicker1.Items171"),
            resources.GetString("foreColorPicker1.Items172"),
            resources.GetString("foreColorPicker1.Items173"),
            resources.GetString("foreColorPicker1.Items174"),
            resources.GetString("foreColorPicker1.Items175"),
            resources.GetString("foreColorPicker1.Items176"),
            resources.GetString("foreColorPicker1.Items177"),
            resources.GetString("foreColorPicker1.Items178"),
            resources.GetString("foreColorPicker1.Items179"),
            resources.GetString("foreColorPicker1.Items180"),
            resources.GetString("foreColorPicker1.Items181"),
            resources.GetString("foreColorPicker1.Items182"),
            resources.GetString("foreColorPicker1.Items183"),
            resources.GetString("foreColorPicker1.Items184"),
            resources.GetString("foreColorPicker1.Items185"),
            resources.GetString("foreColorPicker1.Items186"),
            resources.GetString("foreColorPicker1.Items187"),
            resources.GetString("foreColorPicker1.Items188"),
            resources.GetString("foreColorPicker1.Items189"),
            resources.GetString("foreColorPicker1.Items190"),
            resources.GetString("foreColorPicker1.Items191"),
            resources.GetString("foreColorPicker1.Items192"),
            resources.GetString("foreColorPicker1.Items193"),
            resources.GetString("foreColorPicker1.Items194"),
            resources.GetString("foreColorPicker1.Items195"),
            resources.GetString("foreColorPicker1.Items196"),
            resources.GetString("foreColorPicker1.Items197"),
            resources.GetString("foreColorPicker1.Items198"),
            resources.GetString("foreColorPicker1.Items199"),
            resources.GetString("foreColorPicker1.Items200"),
            resources.GetString("foreColorPicker1.Items201"),
            resources.GetString("foreColorPicker1.Items202"),
            resources.GetString("foreColorPicker1.Items203"),
            resources.GetString("foreColorPicker1.Items204"),
            resources.GetString("foreColorPicker1.Items205"),
            resources.GetString("foreColorPicker1.Items206"),
            resources.GetString("foreColorPicker1.Items207"),
            resources.GetString("foreColorPicker1.Items208"),
            resources.GetString("foreColorPicker1.Items209"),
            resources.GetString("foreColorPicker1.Items210"),
            resources.GetString("foreColorPicker1.Items211"),
            resources.GetString("foreColorPicker1.Items212"),
            resources.GetString("foreColorPicker1.Items213"),
            resources.GetString("foreColorPicker1.Items214"),
            resources.GetString("foreColorPicker1.Items215"),
            resources.GetString("foreColorPicker1.Items216"),
            resources.GetString("foreColorPicker1.Items217"),
            resources.GetString("foreColorPicker1.Items218"),
            resources.GetString("foreColorPicker1.Items219"),
            resources.GetString("foreColorPicker1.Items220"),
            resources.GetString("foreColorPicker1.Items221"),
            resources.GetString("foreColorPicker1.Items222"),
            resources.GetString("foreColorPicker1.Items223"),
            resources.GetString("foreColorPicker1.Items224"),
            resources.GetString("foreColorPicker1.Items225"),
            resources.GetString("foreColorPicker1.Items226"),
            resources.GetString("foreColorPicker1.Items227"),
            resources.GetString("foreColorPicker1.Items228"),
            resources.GetString("foreColorPicker1.Items229"),
            resources.GetString("foreColorPicker1.Items230"),
            resources.GetString("foreColorPicker1.Items231"),
            resources.GetString("foreColorPicker1.Items232"),
            resources.GetString("foreColorPicker1.Items233"),
            resources.GetString("foreColorPicker1.Items234"),
            resources.GetString("foreColorPicker1.Items235"),
            resources.GetString("foreColorPicker1.Items236"),
            resources.GetString("foreColorPicker1.Items237"),
            resources.GetString("foreColorPicker1.Items238"),
            resources.GetString("foreColorPicker1.Items239"),
            resources.GetString("foreColorPicker1.Items240"),
            resources.GetString("foreColorPicker1.Items241"),
            resources.GetString("foreColorPicker1.Items242"),
            resources.GetString("foreColorPicker1.Items243"),
            resources.GetString("foreColorPicker1.Items244"),
            resources.GetString("foreColorPicker1.Items245"),
            resources.GetString("foreColorPicker1.Items246"),
            resources.GetString("foreColorPicker1.Items247"),
            resources.GetString("foreColorPicker1.Items248"),
            resources.GetString("foreColorPicker1.Items249"),
            resources.GetString("foreColorPicker1.Items250"),
            resources.GetString("foreColorPicker1.Items251"),
            resources.GetString("foreColorPicker1.Items252"),
            resources.GetString("foreColorPicker1.Items253"),
            resources.GetString("foreColorPicker1.Items254"),
            resources.GetString("foreColorPicker1.Items255"),
            resources.GetString("foreColorPicker1.Items256"),
            resources.GetString("foreColorPicker1.Items257"),
            resources.GetString("foreColorPicker1.Items258"),
            resources.GetString("foreColorPicker1.Items259"),
            resources.GetString("foreColorPicker1.Items260"),
            resources.GetString("foreColorPicker1.Items261"),
            resources.GetString("foreColorPicker1.Items262"),
            resources.GetString("foreColorPicker1.Items263"),
            resources.GetString("foreColorPicker1.Items264"),
            resources.GetString("foreColorPicker1.Items265"),
            resources.GetString("foreColorPicker1.Items266"),
            resources.GetString("foreColorPicker1.Items267"),
            resources.GetString("foreColorPicker1.Items268"),
            resources.GetString("foreColorPicker1.Items269"),
            resources.GetString("foreColorPicker1.Items270"),
            resources.GetString("foreColorPicker1.Items271"),
            resources.GetString("foreColorPicker1.Items272"),
            resources.GetString("foreColorPicker1.Items273"),
            resources.GetString("foreColorPicker1.Items274"),
            resources.GetString("foreColorPicker1.Items275"),
            resources.GetString("foreColorPicker1.Items276"),
            resources.GetString("foreColorPicker1.Items277"),
            resources.GetString("foreColorPicker1.Items278"),
            resources.GetString("foreColorPicker1.Items279"),
            resources.GetString("foreColorPicker1.Items280"),
            resources.GetString("foreColorPicker1.Items281"),
            resources.GetString("foreColorPicker1.Items282"),
            resources.GetString("foreColorPicker1.Items283"),
            resources.GetString("foreColorPicker1.Items284"),
            resources.GetString("foreColorPicker1.Items285"),
            resources.GetString("foreColorPicker1.Items286"),
            resources.GetString("foreColorPicker1.Items287"),
            resources.GetString("foreColorPicker1.Items288"),
            resources.GetString("foreColorPicker1.Items289"),
            resources.GetString("foreColorPicker1.Items290"),
            resources.GetString("foreColorPicker1.Items291"),
            resources.GetString("foreColorPicker1.Items292"),
            resources.GetString("foreColorPicker1.Items293"),
            resources.GetString("foreColorPicker1.Items294"),
            resources.GetString("foreColorPicker1.Items295"),
            resources.GetString("foreColorPicker1.Items296"),
            resources.GetString("foreColorPicker1.Items297"),
            resources.GetString("foreColorPicker1.Items298"),
            resources.GetString("foreColorPicker1.Items299"),
            resources.GetString("foreColorPicker1.Items300"),
            resources.GetString("foreColorPicker1.Items301"),
            resources.GetString("foreColorPicker1.Items302"),
            resources.GetString("foreColorPicker1.Items303"),
            resources.GetString("foreColorPicker1.Items304"),
            resources.GetString("foreColorPicker1.Items305"),
            resources.GetString("foreColorPicker1.Items306"),
            resources.GetString("foreColorPicker1.Items307"),
            resources.GetString("foreColorPicker1.Items308"),
            resources.GetString("foreColorPicker1.Items309"),
            resources.GetString("foreColorPicker1.Items310"),
            resources.GetString("foreColorPicker1.Items311"),
            resources.GetString("foreColorPicker1.Items312"),
            resources.GetString("foreColorPicker1.Items313"),
            resources.GetString("foreColorPicker1.Items314"),
            resources.GetString("foreColorPicker1.Items315"),
            resources.GetString("foreColorPicker1.Items316"),
            resources.GetString("foreColorPicker1.Items317"),
            resources.GetString("foreColorPicker1.Items318"),
            resources.GetString("foreColorPicker1.Items319"),
            resources.GetString("foreColorPicker1.Items320"),
            resources.GetString("foreColorPicker1.Items321"),
            resources.GetString("foreColorPicker1.Items322"),
            resources.GetString("foreColorPicker1.Items323"),
            resources.GetString("foreColorPicker1.Items324"),
            resources.GetString("foreColorPicker1.Items325"),
            resources.GetString("foreColorPicker1.Items326"),
            resources.GetString("foreColorPicker1.Items327"),
            resources.GetString("foreColorPicker1.Items328"),
            resources.GetString("foreColorPicker1.Items329"),
            resources.GetString("foreColorPicker1.Items330"),
            resources.GetString("foreColorPicker1.Items331"),
            resources.GetString("foreColorPicker1.Items332"),
            resources.GetString("foreColorPicker1.Items333"),
            resources.GetString("foreColorPicker1.Items334"),
            resources.GetString("foreColorPicker1.Items335"),
            resources.GetString("foreColorPicker1.Items336"),
            resources.GetString("foreColorPicker1.Items337"),
            resources.GetString("foreColorPicker1.Items338"),
            resources.GetString("foreColorPicker1.Items339"),
            resources.GetString("foreColorPicker1.Items340"),
            resources.GetString("foreColorPicker1.Items341"),
            resources.GetString("foreColorPicker1.Items342"),
            resources.GetString("foreColorPicker1.Items343"),
            resources.GetString("foreColorPicker1.Items344"),
            resources.GetString("foreColorPicker1.Items345"),
            resources.GetString("foreColorPicker1.Items346"),
            resources.GetString("foreColorPicker1.Items347"),
            resources.GetString("foreColorPicker1.Items348"),
            resources.GetString("foreColorPicker1.Items349"),
            resources.GetString("foreColorPicker1.Items350"),
            resources.GetString("foreColorPicker1.Items351"),
            resources.GetString("foreColorPicker1.Items352"),
            resources.GetString("foreColorPicker1.Items353"),
            resources.GetString("foreColorPicker1.Items354"),
            resources.GetString("foreColorPicker1.Items355"),
            resources.GetString("foreColorPicker1.Items356"),
            resources.GetString("foreColorPicker1.Items357"),
            resources.GetString("foreColorPicker1.Items358"),
            resources.GetString("foreColorPicker1.Items359"),
            resources.GetString("foreColorPicker1.Items360"),
            resources.GetString("foreColorPicker1.Items361"),
            resources.GetString("foreColorPicker1.Items362"),
            resources.GetString("foreColorPicker1.Items363"),
            resources.GetString("foreColorPicker1.Items364"),
            resources.GetString("foreColorPicker1.Items365"),
            resources.GetString("foreColorPicker1.Items366"),
            resources.GetString("foreColorPicker1.Items367"),
            resources.GetString("foreColorPicker1.Items368"),
            resources.GetString("foreColorPicker1.Items369"),
            resources.GetString("foreColorPicker1.Items370"),
            resources.GetString("foreColorPicker1.Items371"),
            resources.GetString("foreColorPicker1.Items372"),
            resources.GetString("foreColorPicker1.Items373"),
            resources.GetString("foreColorPicker1.Items374"),
            resources.GetString("foreColorPicker1.Items375"),
            resources.GetString("foreColorPicker1.Items376"),
            resources.GetString("foreColorPicker1.Items377"),
            resources.GetString("foreColorPicker1.Items378"),
            resources.GetString("foreColorPicker1.Items379"),
            resources.GetString("foreColorPicker1.Items380"),
            resources.GetString("foreColorPicker1.Items381"),
            resources.GetString("foreColorPicker1.Items382"),
            resources.GetString("foreColorPicker1.Items383"),
            resources.GetString("foreColorPicker1.Items384"),
            resources.GetString("foreColorPicker1.Items385"),
            resources.GetString("foreColorPicker1.Items386"),
            resources.GetString("foreColorPicker1.Items387"),
            resources.GetString("foreColorPicker1.Items388"),
            resources.GetString("foreColorPicker1.Items389"),
            resources.GetString("foreColorPicker1.Items390"),
            resources.GetString("foreColorPicker1.Items391"),
            resources.GetString("foreColorPicker1.Items392"),
            resources.GetString("foreColorPicker1.Items393"),
            resources.GetString("foreColorPicker1.Items394"),
            resources.GetString("foreColorPicker1.Items395"),
            resources.GetString("foreColorPicker1.Items396"),
            resources.GetString("foreColorPicker1.Items397"),
            resources.GetString("foreColorPicker1.Items398"),
            resources.GetString("foreColorPicker1.Items399"),
            resources.GetString("foreColorPicker1.Items400"),
            resources.GetString("foreColorPicker1.Items401"),
            resources.GetString("foreColorPicker1.Items402"),
            resources.GetString("foreColorPicker1.Items403"),
            resources.GetString("foreColorPicker1.Items404"),
            resources.GetString("foreColorPicker1.Items405"),
            resources.GetString("foreColorPicker1.Items406"),
            resources.GetString("foreColorPicker1.Items407"),
            resources.GetString("foreColorPicker1.Items408"),
            resources.GetString("foreColorPicker1.Items409"),
            resources.GetString("foreColorPicker1.Items410"),
            resources.GetString("foreColorPicker1.Items411"),
            resources.GetString("foreColorPicker1.Items412"),
            resources.GetString("foreColorPicker1.Items413"),
            resources.GetString("foreColorPicker1.Items414"),
            resources.GetString("foreColorPicker1.Items415"),
            resources.GetString("foreColorPicker1.Items416"),
            resources.GetString("foreColorPicker1.Items417"),
            resources.GetString("foreColorPicker1.Items418"),
            resources.GetString("foreColorPicker1.Items419"),
            resources.GetString("foreColorPicker1.Items420"),
            resources.GetString("foreColorPicker1.Items421"),
            resources.GetString("foreColorPicker1.Items422"),
            resources.GetString("foreColorPicker1.Items423"),
            resources.GetString("foreColorPicker1.Items424"),
            resources.GetString("foreColorPicker1.Items425"),
            resources.GetString("foreColorPicker1.Items426"),
            resources.GetString("foreColorPicker1.Items427"),
            resources.GetString("foreColorPicker1.Items428"),
            resources.GetString("foreColorPicker1.Items429"),
            resources.GetString("foreColorPicker1.Items430"),
            resources.GetString("foreColorPicker1.Items431"),
            resources.GetString("foreColorPicker1.Items432"),
            resources.GetString("foreColorPicker1.Items433"),
            resources.GetString("foreColorPicker1.Items434"),
            resources.GetString("foreColorPicker1.Items435"),
            resources.GetString("foreColorPicker1.Items436"),
            resources.GetString("foreColorPicker1.Items437"),
            resources.GetString("foreColorPicker1.Items438"),
            resources.GetString("foreColorPicker1.Items439"),
            resources.GetString("foreColorPicker1.Items440"),
            resources.GetString("foreColorPicker1.Items441"),
            resources.GetString("foreColorPicker1.Items442"),
            resources.GetString("foreColorPicker1.Items443"),
            resources.GetString("foreColorPicker1.Items444"),
            resources.GetString("foreColorPicker1.Items445"),
            resources.GetString("foreColorPicker1.Items446"),
            resources.GetString("foreColorPicker1.Items447"),
            resources.GetString("foreColorPicker1.Items448"),
            resources.GetString("foreColorPicker1.Items449"),
            resources.GetString("foreColorPicker1.Items450"),
            resources.GetString("foreColorPicker1.Items451"),
            resources.GetString("foreColorPicker1.Items452"),
            resources.GetString("foreColorPicker1.Items453"),
            resources.GetString("foreColorPicker1.Items454"),
            resources.GetString("foreColorPicker1.Items455"),
            resources.GetString("foreColorPicker1.Items456"),
            resources.GetString("foreColorPicker1.Items457"),
            resources.GetString("foreColorPicker1.Items458"),
            resources.GetString("foreColorPicker1.Items459"),
            resources.GetString("foreColorPicker1.Items460"),
            resources.GetString("foreColorPicker1.Items461"),
            resources.GetString("foreColorPicker1.Items462"),
            resources.GetString("foreColorPicker1.Items463"),
            resources.GetString("foreColorPicker1.Items464"),
            resources.GetString("foreColorPicker1.Items465"),
            resources.GetString("foreColorPicker1.Items466"),
            resources.GetString("foreColorPicker1.Items467"),
            resources.GetString("foreColorPicker1.Items468"),
            resources.GetString("foreColorPicker1.Items469"),
            resources.GetString("foreColorPicker1.Items470"),
            resources.GetString("foreColorPicker1.Items471"),
            resources.GetString("foreColorPicker1.Items472"),
            resources.GetString("foreColorPicker1.Items473"),
            resources.GetString("foreColorPicker1.Items474"),
            resources.GetString("foreColorPicker1.Items475"),
            resources.GetString("foreColorPicker1.Items476"),
            resources.GetString("foreColorPicker1.Items477"),
            resources.GetString("foreColorPicker1.Items478"),
            resources.GetString("foreColorPicker1.Items479"),
            resources.GetString("foreColorPicker1.Items480"),
            resources.GetString("foreColorPicker1.Items481"),
            resources.GetString("foreColorPicker1.Items482"),
            resources.GetString("foreColorPicker1.Items483"),
            resources.GetString("foreColorPicker1.Items484"),
            resources.GetString("foreColorPicker1.Items485"),
            resources.GetString("foreColorPicker1.Items486"),
            resources.GetString("foreColorPicker1.Items487"),
            resources.GetString("foreColorPicker1.Items488"),
            resources.GetString("foreColorPicker1.Items489"),
            resources.GetString("foreColorPicker1.Items490"),
            resources.GetString("foreColorPicker1.Items491"),
            resources.GetString("foreColorPicker1.Items492"),
            resources.GetString("foreColorPicker1.Items493"),
            resources.GetString("foreColorPicker1.Items494"),
            resources.GetString("foreColorPicker1.Items495"),
            resources.GetString("foreColorPicker1.Items496"),
            resources.GetString("foreColorPicker1.Items497"),
            resources.GetString("foreColorPicker1.Items498"),
            resources.GetString("foreColorPicker1.Items499"),
            resources.GetString("foreColorPicker1.Items500"),
            resources.GetString("foreColorPicker1.Items501"),
            resources.GetString("foreColorPicker1.Items502"),
            resources.GetString("foreColorPicker1.Items503"),
            resources.GetString("foreColorPicker1.Items504"),
            resources.GetString("foreColorPicker1.Items505"),
            resources.GetString("foreColorPicker1.Items506"),
            resources.GetString("foreColorPicker1.Items507"),
            resources.GetString("foreColorPicker1.Items508"),
            resources.GetString("foreColorPicker1.Items509"),
            resources.GetString("foreColorPicker1.Items510"),
            resources.GetString("foreColorPicker1.Items511"),
            resources.GetString("foreColorPicker1.Items512"),
            resources.GetString("foreColorPicker1.Items513"),
            resources.GetString("foreColorPicker1.Items514"),
            resources.GetString("foreColorPicker1.Items515"),
            resources.GetString("foreColorPicker1.Items516"),
            resources.GetString("foreColorPicker1.Items517"),
            resources.GetString("foreColorPicker1.Items518"),
            resources.GetString("foreColorPicker1.Items519"),
            resources.GetString("foreColorPicker1.Items520"),
            resources.GetString("foreColorPicker1.Items521"),
            resources.GetString("foreColorPicker1.Items522"),
            resources.GetString("foreColorPicker1.Items523"),
            resources.GetString("foreColorPicker1.Items524"),
            resources.GetString("foreColorPicker1.Items525"),
            resources.GetString("foreColorPicker1.Items526"),
            resources.GetString("foreColorPicker1.Items527"),
            resources.GetString("foreColorPicker1.Items528"),
            resources.GetString("foreColorPicker1.Items529"),
            resources.GetString("foreColorPicker1.Items530"),
            resources.GetString("foreColorPicker1.Items531"),
            resources.GetString("foreColorPicker1.Items532"),
            resources.GetString("foreColorPicker1.Items533"),
            resources.GetString("foreColorPicker1.Items534"),
            resources.GetString("foreColorPicker1.Items535"),
            resources.GetString("foreColorPicker1.Items536"),
            resources.GetString("foreColorPicker1.Items537"),
            resources.GetString("foreColorPicker1.Items538"),
            resources.GetString("foreColorPicker1.Items539"),
            resources.GetString("foreColorPicker1.Items540"),
            resources.GetString("foreColorPicker1.Items541"),
            resources.GetString("foreColorPicker1.Items542"),
            resources.GetString("foreColorPicker1.Items543"),
            resources.GetString("foreColorPicker1.Items544"),
            resources.GetString("foreColorPicker1.Items545"),
            resources.GetString("foreColorPicker1.Items546"),
            resources.GetString("foreColorPicker1.Items547"),
            resources.GetString("foreColorPicker1.Items548"),
            resources.GetString("foreColorPicker1.Items549"),
            resources.GetString("foreColorPicker1.Items550"),
            resources.GetString("foreColorPicker1.Items551"),
            resources.GetString("foreColorPicker1.Items552"),
            resources.GetString("foreColorPicker1.Items553"),
            resources.GetString("foreColorPicker1.Items554"),
            resources.GetString("foreColorPicker1.Items555"),
            resources.GetString("foreColorPicker1.Items556"),
            resources.GetString("foreColorPicker1.Items557"),
            resources.GetString("foreColorPicker1.Items558"),
            resources.GetString("foreColorPicker1.Items559"),
            resources.GetString("foreColorPicker1.Items560"),
            resources.GetString("foreColorPicker1.Items561"),
            resources.GetString("foreColorPicker1.Items562"),
            resources.GetString("foreColorPicker1.Items563"),
            resources.GetString("foreColorPicker1.Items564"),
            resources.GetString("foreColorPicker1.Items565"),
            resources.GetString("foreColorPicker1.Items566"),
            resources.GetString("foreColorPicker1.Items567"),
            resources.GetString("foreColorPicker1.Items568"),
            resources.GetString("foreColorPicker1.Items569"),
            resources.GetString("foreColorPicker1.Items570"),
            resources.GetString("foreColorPicker1.Items571"),
            resources.GetString("foreColorPicker1.Items572"),
            resources.GetString("foreColorPicker1.Items573"),
            resources.GetString("foreColorPicker1.Items574"),
            resources.GetString("foreColorPicker1.Items575"),
            resources.GetString("foreColorPicker1.Items576"),
            resources.GetString("foreColorPicker1.Items577"),
            resources.GetString("foreColorPicker1.Items578"),
            resources.GetString("foreColorPicker1.Items579"),
            resources.GetString("foreColorPicker1.Items580"),
            resources.GetString("foreColorPicker1.Items581"),
            resources.GetString("foreColorPicker1.Items582"),
            resources.GetString("foreColorPicker1.Items583"),
            resources.GetString("foreColorPicker1.Items584"),
            resources.GetString("foreColorPicker1.Items585"),
            resources.GetString("foreColorPicker1.Items586"),
            resources.GetString("foreColorPicker1.Items587"),
            resources.GetString("foreColorPicker1.Items588"),
            resources.GetString("foreColorPicker1.Items589"),
            resources.GetString("foreColorPicker1.Items590"),
            resources.GetString("foreColorPicker1.Items591"),
            resources.GetString("foreColorPicker1.Items592"),
            resources.GetString("foreColorPicker1.Items593"),
            resources.GetString("foreColorPicker1.Items594"),
            resources.GetString("foreColorPicker1.Items595"),
            resources.GetString("foreColorPicker1.Items596"),
            resources.GetString("foreColorPicker1.Items597"),
            resources.GetString("foreColorPicker1.Items598"),
            resources.GetString("foreColorPicker1.Items599"),
            resources.GetString("foreColorPicker1.Items600"),
            resources.GetString("foreColorPicker1.Items601"),
            resources.GetString("foreColorPicker1.Items602"),
            resources.GetString("foreColorPicker1.Items603"),
            resources.GetString("foreColorPicker1.Items604"),
            resources.GetString("foreColorPicker1.Items605"),
            resources.GetString("foreColorPicker1.Items606"),
            resources.GetString("foreColorPicker1.Items607"),
            resources.GetString("foreColorPicker1.Items608"),
            resources.GetString("foreColorPicker1.Items609"),
            resources.GetString("foreColorPicker1.Items610"),
            resources.GetString("foreColorPicker1.Items611"),
            resources.GetString("foreColorPicker1.Items612"),
            resources.GetString("foreColorPicker1.Items613"),
            resources.GetString("foreColorPicker1.Items614"),
            resources.GetString("foreColorPicker1.Items615"),
            resources.GetString("foreColorPicker1.Items616"),
            resources.GetString("foreColorPicker1.Items617"),
            resources.GetString("foreColorPicker1.Items618"),
            resources.GetString("foreColorPicker1.Items619"),
            resources.GetString("foreColorPicker1.Items620"),
            resources.GetString("foreColorPicker1.Items621"),
            resources.GetString("foreColorPicker1.Items622"),
            resources.GetString("foreColorPicker1.Items623"),
            resources.GetString("foreColorPicker1.Items624"),
            resources.GetString("foreColorPicker1.Items625"),
            resources.GetString("foreColorPicker1.Items626"),
            resources.GetString("foreColorPicker1.Items627"),
            resources.GetString("foreColorPicker1.Items628"),
            resources.GetString("foreColorPicker1.Items629"),
            resources.GetString("foreColorPicker1.Items630"),
            resources.GetString("foreColorPicker1.Items631"),
            resources.GetString("foreColorPicker1.Items632"),
            resources.GetString("foreColorPicker1.Items633"),
            resources.GetString("foreColorPicker1.Items634"),
            resources.GetString("foreColorPicker1.Items635"),
            resources.GetString("foreColorPicker1.Items636"),
            resources.GetString("foreColorPicker1.Items637"),
            resources.GetString("foreColorPicker1.Items638"),
            resources.GetString("foreColorPicker1.Items639"),
            resources.GetString("foreColorPicker1.Items640"),
            resources.GetString("foreColorPicker1.Items641"),
            resources.GetString("foreColorPicker1.Items642"),
            resources.GetString("foreColorPicker1.Items643"),
            resources.GetString("foreColorPicker1.Items644"),
            resources.GetString("foreColorPicker1.Items645"),
            resources.GetString("foreColorPicker1.Items646"),
            resources.GetString("foreColorPicker1.Items647"),
            resources.GetString("foreColorPicker1.Items648"),
            resources.GetString("foreColorPicker1.Items649"),
            resources.GetString("foreColorPicker1.Items650"),
            resources.GetString("foreColorPicker1.Items651"),
            resources.GetString("foreColorPicker1.Items652"),
            resources.GetString("foreColorPicker1.Items653"),
            resources.GetString("foreColorPicker1.Items654"),
            resources.GetString("foreColorPicker1.Items655"),
            resources.GetString("foreColorPicker1.Items656"),
            resources.GetString("foreColorPicker1.Items657"),
            resources.GetString("foreColorPicker1.Items658"),
            resources.GetString("foreColorPicker1.Items659"),
            resources.GetString("foreColorPicker1.Items660"),
            resources.GetString("foreColorPicker1.Items661"),
            resources.GetString("foreColorPicker1.Items662"),
            resources.GetString("foreColorPicker1.Items663"),
            resources.GetString("foreColorPicker1.Items664"),
            resources.GetString("foreColorPicker1.Items665"),
            resources.GetString("foreColorPicker1.Items666"),
            resources.GetString("foreColorPicker1.Items667"),
            resources.GetString("foreColorPicker1.Items668"),
            resources.GetString("foreColorPicker1.Items669"),
            resources.GetString("foreColorPicker1.Items670"),
            resources.GetString("foreColorPicker1.Items671"),
            resources.GetString("foreColorPicker1.Items672"),
            resources.GetString("foreColorPicker1.Items673"),
            resources.GetString("foreColorPicker1.Items674"),
            resources.GetString("foreColorPicker1.Items675"),
            resources.GetString("foreColorPicker1.Items676"),
            resources.GetString("foreColorPicker1.Items677"),
            resources.GetString("foreColorPicker1.Items678"),
            resources.GetString("foreColorPicker1.Items679"),
            resources.GetString("foreColorPicker1.Items680"),
            resources.GetString("foreColorPicker1.Items681"),
            resources.GetString("foreColorPicker1.Items682"),
            resources.GetString("foreColorPicker1.Items683"),
            resources.GetString("foreColorPicker1.Items684"),
            resources.GetString("foreColorPicker1.Items685"),
            resources.GetString("foreColorPicker1.Items686"),
            resources.GetString("foreColorPicker1.Items687"),
            resources.GetString("foreColorPicker1.Items688"),
            resources.GetString("foreColorPicker1.Items689"),
            resources.GetString("foreColorPicker1.Items690"),
            resources.GetString("foreColorPicker1.Items691"),
            resources.GetString("foreColorPicker1.Items692"),
            resources.GetString("foreColorPicker1.Items693"),
            resources.GetString("foreColorPicker1.Items694"),
            resources.GetString("foreColorPicker1.Items695"),
            resources.GetString("foreColorPicker1.Items696"),
            resources.GetString("foreColorPicker1.Items697"),
            resources.GetString("foreColorPicker1.Items698"),
            resources.GetString("foreColorPicker1.Items699")});
			this.foreColorPicker1.Name = "foreColorPicker1";
			this.foreColorPicker1.Tag = "Fore Color";
			this.foreColorPicker1.SelectedValueChanged += new System.EventHandler(this.ctlForeColor_Change);
			this.foreColorPicker1.Validated += new System.EventHandler(this.ctlForeColor_Change);
			// 
			// backColorPicker1
			// 
			this.backColorPicker1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.backColorPicker1.DropDownHeight = 1;
			this.backColorPicker1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.backColorPicker1, "backColorPicker1");
			this.backColorPicker1.FormattingEnabled = true;
			this.backColorPicker1.Items.AddRange(new object[] {
            resources.GetString("backColorPicker1.Items"),
            resources.GetString("backColorPicker1.Items1"),
            resources.GetString("backColorPicker1.Items2"),
            resources.GetString("backColorPicker1.Items3"),
            resources.GetString("backColorPicker1.Items4"),
            resources.GetString("backColorPicker1.Items5"),
            resources.GetString("backColorPicker1.Items6"),
            resources.GetString("backColorPicker1.Items7"),
            resources.GetString("backColorPicker1.Items8"),
            resources.GetString("backColorPicker1.Items9"),
            resources.GetString("backColorPicker1.Items10"),
            resources.GetString("backColorPicker1.Items11"),
            resources.GetString("backColorPicker1.Items12"),
            resources.GetString("backColorPicker1.Items13"),
            resources.GetString("backColorPicker1.Items14"),
            resources.GetString("backColorPicker1.Items15"),
            resources.GetString("backColorPicker1.Items16"),
            resources.GetString("backColorPicker1.Items17"),
            resources.GetString("backColorPicker1.Items18"),
            resources.GetString("backColorPicker1.Items19"),
            resources.GetString("backColorPicker1.Items20"),
            resources.GetString("backColorPicker1.Items21"),
            resources.GetString("backColorPicker1.Items22"),
            resources.GetString("backColorPicker1.Items23"),
            resources.GetString("backColorPicker1.Items24"),
            resources.GetString("backColorPicker1.Items25"),
            resources.GetString("backColorPicker1.Items26"),
            resources.GetString("backColorPicker1.Items27"),
            resources.GetString("backColorPicker1.Items28"),
            resources.GetString("backColorPicker1.Items29"),
            resources.GetString("backColorPicker1.Items30"),
            resources.GetString("backColorPicker1.Items31"),
            resources.GetString("backColorPicker1.Items32"),
            resources.GetString("backColorPicker1.Items33"),
            resources.GetString("backColorPicker1.Items34"),
            resources.GetString("backColorPicker1.Items35"),
            resources.GetString("backColorPicker1.Items36"),
            resources.GetString("backColorPicker1.Items37"),
            resources.GetString("backColorPicker1.Items38"),
            resources.GetString("backColorPicker1.Items39"),
            resources.GetString("backColorPicker1.Items40"),
            resources.GetString("backColorPicker1.Items41"),
            resources.GetString("backColorPicker1.Items42"),
            resources.GetString("backColorPicker1.Items43"),
            resources.GetString("backColorPicker1.Items44"),
            resources.GetString("backColorPicker1.Items45"),
            resources.GetString("backColorPicker1.Items46"),
            resources.GetString("backColorPicker1.Items47"),
            resources.GetString("backColorPicker1.Items48"),
            resources.GetString("backColorPicker1.Items49"),
            resources.GetString("backColorPicker1.Items50"),
            resources.GetString("backColorPicker1.Items51"),
            resources.GetString("backColorPicker1.Items52"),
            resources.GetString("backColorPicker1.Items53"),
            resources.GetString("backColorPicker1.Items54"),
            resources.GetString("backColorPicker1.Items55"),
            resources.GetString("backColorPicker1.Items56"),
            resources.GetString("backColorPicker1.Items57"),
            resources.GetString("backColorPicker1.Items58"),
            resources.GetString("backColorPicker1.Items59"),
            resources.GetString("backColorPicker1.Items60"),
            resources.GetString("backColorPicker1.Items61"),
            resources.GetString("backColorPicker1.Items62"),
            resources.GetString("backColorPicker1.Items63"),
            resources.GetString("backColorPicker1.Items64"),
            resources.GetString("backColorPicker1.Items65"),
            resources.GetString("backColorPicker1.Items66"),
            resources.GetString("backColorPicker1.Items67"),
            resources.GetString("backColorPicker1.Items68"),
            resources.GetString("backColorPicker1.Items69"),
            resources.GetString("backColorPicker1.Items70"),
            resources.GetString("backColorPicker1.Items71"),
            resources.GetString("backColorPicker1.Items72"),
            resources.GetString("backColorPicker1.Items73"),
            resources.GetString("backColorPicker1.Items74"),
            resources.GetString("backColorPicker1.Items75"),
            resources.GetString("backColorPicker1.Items76"),
            resources.GetString("backColorPicker1.Items77"),
            resources.GetString("backColorPicker1.Items78"),
            resources.GetString("backColorPicker1.Items79"),
            resources.GetString("backColorPicker1.Items80"),
            resources.GetString("backColorPicker1.Items81"),
            resources.GetString("backColorPicker1.Items82"),
            resources.GetString("backColorPicker1.Items83"),
            resources.GetString("backColorPicker1.Items84"),
            resources.GetString("backColorPicker1.Items85"),
            resources.GetString("backColorPicker1.Items86"),
            resources.GetString("backColorPicker1.Items87"),
            resources.GetString("backColorPicker1.Items88"),
            resources.GetString("backColorPicker1.Items89"),
            resources.GetString("backColorPicker1.Items90"),
            resources.GetString("backColorPicker1.Items91"),
            resources.GetString("backColorPicker1.Items92"),
            resources.GetString("backColorPicker1.Items93"),
            resources.GetString("backColorPicker1.Items94"),
            resources.GetString("backColorPicker1.Items95"),
            resources.GetString("backColorPicker1.Items96"),
            resources.GetString("backColorPicker1.Items97"),
            resources.GetString("backColorPicker1.Items98"),
            resources.GetString("backColorPicker1.Items99"),
            resources.GetString("backColorPicker1.Items100"),
            resources.GetString("backColorPicker1.Items101"),
            resources.GetString("backColorPicker1.Items102"),
            resources.GetString("backColorPicker1.Items103"),
            resources.GetString("backColorPicker1.Items104"),
            resources.GetString("backColorPicker1.Items105"),
            resources.GetString("backColorPicker1.Items106"),
            resources.GetString("backColorPicker1.Items107"),
            resources.GetString("backColorPicker1.Items108"),
            resources.GetString("backColorPicker1.Items109"),
            resources.GetString("backColorPicker1.Items110"),
            resources.GetString("backColorPicker1.Items111"),
            resources.GetString("backColorPicker1.Items112"),
            resources.GetString("backColorPicker1.Items113"),
            resources.GetString("backColorPicker1.Items114"),
            resources.GetString("backColorPicker1.Items115"),
            resources.GetString("backColorPicker1.Items116"),
            resources.GetString("backColorPicker1.Items117"),
            resources.GetString("backColorPicker1.Items118"),
            resources.GetString("backColorPicker1.Items119"),
            resources.GetString("backColorPicker1.Items120"),
            resources.GetString("backColorPicker1.Items121"),
            resources.GetString("backColorPicker1.Items122"),
            resources.GetString("backColorPicker1.Items123"),
            resources.GetString("backColorPicker1.Items124"),
            resources.GetString("backColorPicker1.Items125"),
            resources.GetString("backColorPicker1.Items126"),
            resources.GetString("backColorPicker1.Items127"),
            resources.GetString("backColorPicker1.Items128"),
            resources.GetString("backColorPicker1.Items129"),
            resources.GetString("backColorPicker1.Items130"),
            resources.GetString("backColorPicker1.Items131"),
            resources.GetString("backColorPicker1.Items132"),
            resources.GetString("backColorPicker1.Items133"),
            resources.GetString("backColorPicker1.Items134"),
            resources.GetString("backColorPicker1.Items135"),
            resources.GetString("backColorPicker1.Items136"),
            resources.GetString("backColorPicker1.Items137"),
            resources.GetString("backColorPicker1.Items138"),
            resources.GetString("backColorPicker1.Items139"),
            resources.GetString("backColorPicker1.Items140"),
            resources.GetString("backColorPicker1.Items141"),
            resources.GetString("backColorPicker1.Items142"),
            resources.GetString("backColorPicker1.Items143"),
            resources.GetString("backColorPicker1.Items144"),
            resources.GetString("backColorPicker1.Items145"),
            resources.GetString("backColorPicker1.Items146"),
            resources.GetString("backColorPicker1.Items147"),
            resources.GetString("backColorPicker1.Items148"),
            resources.GetString("backColorPicker1.Items149"),
            resources.GetString("backColorPicker1.Items150"),
            resources.GetString("backColorPicker1.Items151"),
            resources.GetString("backColorPicker1.Items152"),
            resources.GetString("backColorPicker1.Items153"),
            resources.GetString("backColorPicker1.Items154"),
            resources.GetString("backColorPicker1.Items155"),
            resources.GetString("backColorPicker1.Items156"),
            resources.GetString("backColorPicker1.Items157"),
            resources.GetString("backColorPicker1.Items158"),
            resources.GetString("backColorPicker1.Items159"),
            resources.GetString("backColorPicker1.Items160"),
            resources.GetString("backColorPicker1.Items161"),
            resources.GetString("backColorPicker1.Items162"),
            resources.GetString("backColorPicker1.Items163"),
            resources.GetString("backColorPicker1.Items164"),
            resources.GetString("backColorPicker1.Items165"),
            resources.GetString("backColorPicker1.Items166"),
            resources.GetString("backColorPicker1.Items167"),
            resources.GetString("backColorPicker1.Items168"),
            resources.GetString("backColorPicker1.Items169"),
            resources.GetString("backColorPicker1.Items170"),
            resources.GetString("backColorPicker1.Items171"),
            resources.GetString("backColorPicker1.Items172"),
            resources.GetString("backColorPicker1.Items173"),
            resources.GetString("backColorPicker1.Items174"),
            resources.GetString("backColorPicker1.Items175"),
            resources.GetString("backColorPicker1.Items176"),
            resources.GetString("backColorPicker1.Items177"),
            resources.GetString("backColorPicker1.Items178"),
            resources.GetString("backColorPicker1.Items179"),
            resources.GetString("backColorPicker1.Items180"),
            resources.GetString("backColorPicker1.Items181"),
            resources.GetString("backColorPicker1.Items182"),
            resources.GetString("backColorPicker1.Items183"),
            resources.GetString("backColorPicker1.Items184"),
            resources.GetString("backColorPicker1.Items185"),
            resources.GetString("backColorPicker1.Items186"),
            resources.GetString("backColorPicker1.Items187"),
            resources.GetString("backColorPicker1.Items188"),
            resources.GetString("backColorPicker1.Items189"),
            resources.GetString("backColorPicker1.Items190"),
            resources.GetString("backColorPicker1.Items191"),
            resources.GetString("backColorPicker1.Items192"),
            resources.GetString("backColorPicker1.Items193"),
            resources.GetString("backColorPicker1.Items194"),
            resources.GetString("backColorPicker1.Items195"),
            resources.GetString("backColorPicker1.Items196"),
            resources.GetString("backColorPicker1.Items197"),
            resources.GetString("backColorPicker1.Items198"),
            resources.GetString("backColorPicker1.Items199"),
            resources.GetString("backColorPicker1.Items200"),
            resources.GetString("backColorPicker1.Items201"),
            resources.GetString("backColorPicker1.Items202"),
            resources.GetString("backColorPicker1.Items203"),
            resources.GetString("backColorPicker1.Items204"),
            resources.GetString("backColorPicker1.Items205"),
            resources.GetString("backColorPicker1.Items206"),
            resources.GetString("backColorPicker1.Items207"),
            resources.GetString("backColorPicker1.Items208"),
            resources.GetString("backColorPicker1.Items209"),
            resources.GetString("backColorPicker1.Items210"),
            resources.GetString("backColorPicker1.Items211"),
            resources.GetString("backColorPicker1.Items212"),
            resources.GetString("backColorPicker1.Items213"),
            resources.GetString("backColorPicker1.Items214"),
            resources.GetString("backColorPicker1.Items215"),
            resources.GetString("backColorPicker1.Items216"),
            resources.GetString("backColorPicker1.Items217"),
            resources.GetString("backColorPicker1.Items218"),
            resources.GetString("backColorPicker1.Items219"),
            resources.GetString("backColorPicker1.Items220"),
            resources.GetString("backColorPicker1.Items221"),
            resources.GetString("backColorPicker1.Items222"),
            resources.GetString("backColorPicker1.Items223"),
            resources.GetString("backColorPicker1.Items224"),
            resources.GetString("backColorPicker1.Items225"),
            resources.GetString("backColorPicker1.Items226"),
            resources.GetString("backColorPicker1.Items227"),
            resources.GetString("backColorPicker1.Items228"),
            resources.GetString("backColorPicker1.Items229"),
            resources.GetString("backColorPicker1.Items230"),
            resources.GetString("backColorPicker1.Items231"),
            resources.GetString("backColorPicker1.Items232"),
            resources.GetString("backColorPicker1.Items233"),
            resources.GetString("backColorPicker1.Items234"),
            resources.GetString("backColorPicker1.Items235"),
            resources.GetString("backColorPicker1.Items236"),
            resources.GetString("backColorPicker1.Items237"),
            resources.GetString("backColorPicker1.Items238"),
            resources.GetString("backColorPicker1.Items239"),
            resources.GetString("backColorPicker1.Items240"),
            resources.GetString("backColorPicker1.Items241"),
            resources.GetString("backColorPicker1.Items242"),
            resources.GetString("backColorPicker1.Items243"),
            resources.GetString("backColorPicker1.Items244"),
            resources.GetString("backColorPicker1.Items245"),
            resources.GetString("backColorPicker1.Items246"),
            resources.GetString("backColorPicker1.Items247"),
            resources.GetString("backColorPicker1.Items248"),
            resources.GetString("backColorPicker1.Items249"),
            resources.GetString("backColorPicker1.Items250"),
            resources.GetString("backColorPicker1.Items251"),
            resources.GetString("backColorPicker1.Items252"),
            resources.GetString("backColorPicker1.Items253"),
            resources.GetString("backColorPicker1.Items254"),
            resources.GetString("backColorPicker1.Items255"),
            resources.GetString("backColorPicker1.Items256"),
            resources.GetString("backColorPicker1.Items257"),
            resources.GetString("backColorPicker1.Items258"),
            resources.GetString("backColorPicker1.Items259"),
            resources.GetString("backColorPicker1.Items260"),
            resources.GetString("backColorPicker1.Items261"),
            resources.GetString("backColorPicker1.Items262"),
            resources.GetString("backColorPicker1.Items263"),
            resources.GetString("backColorPicker1.Items264"),
            resources.GetString("backColorPicker1.Items265"),
            resources.GetString("backColorPicker1.Items266"),
            resources.GetString("backColorPicker1.Items267"),
            resources.GetString("backColorPicker1.Items268"),
            resources.GetString("backColorPicker1.Items269"),
            resources.GetString("backColorPicker1.Items270"),
            resources.GetString("backColorPicker1.Items271"),
            resources.GetString("backColorPicker1.Items272"),
            resources.GetString("backColorPicker1.Items273"),
            resources.GetString("backColorPicker1.Items274"),
            resources.GetString("backColorPicker1.Items275"),
            resources.GetString("backColorPicker1.Items276"),
            resources.GetString("backColorPicker1.Items277"),
            resources.GetString("backColorPicker1.Items278"),
            resources.GetString("backColorPicker1.Items279"),
            resources.GetString("backColorPicker1.Items280"),
            resources.GetString("backColorPicker1.Items281"),
            resources.GetString("backColorPicker1.Items282"),
            resources.GetString("backColorPicker1.Items283"),
            resources.GetString("backColorPicker1.Items284"),
            resources.GetString("backColorPicker1.Items285"),
            resources.GetString("backColorPicker1.Items286"),
            resources.GetString("backColorPicker1.Items287"),
            resources.GetString("backColorPicker1.Items288"),
            resources.GetString("backColorPicker1.Items289"),
            resources.GetString("backColorPicker1.Items290"),
            resources.GetString("backColorPicker1.Items291"),
            resources.GetString("backColorPicker1.Items292"),
            resources.GetString("backColorPicker1.Items293"),
            resources.GetString("backColorPicker1.Items294"),
            resources.GetString("backColorPicker1.Items295"),
            resources.GetString("backColorPicker1.Items296"),
            resources.GetString("backColorPicker1.Items297"),
            resources.GetString("backColorPicker1.Items298"),
            resources.GetString("backColorPicker1.Items299"),
            resources.GetString("backColorPicker1.Items300"),
            resources.GetString("backColorPicker1.Items301"),
            resources.GetString("backColorPicker1.Items302"),
            resources.GetString("backColorPicker1.Items303"),
            resources.GetString("backColorPicker1.Items304"),
            resources.GetString("backColorPicker1.Items305"),
            resources.GetString("backColorPicker1.Items306"),
            resources.GetString("backColorPicker1.Items307"),
            resources.GetString("backColorPicker1.Items308"),
            resources.GetString("backColorPicker1.Items309"),
            resources.GetString("backColorPicker1.Items310"),
            resources.GetString("backColorPicker1.Items311"),
            resources.GetString("backColorPicker1.Items312"),
            resources.GetString("backColorPicker1.Items313"),
            resources.GetString("backColorPicker1.Items314"),
            resources.GetString("backColorPicker1.Items315"),
            resources.GetString("backColorPicker1.Items316"),
            resources.GetString("backColorPicker1.Items317"),
            resources.GetString("backColorPicker1.Items318"),
            resources.GetString("backColorPicker1.Items319"),
            resources.GetString("backColorPicker1.Items320"),
            resources.GetString("backColorPicker1.Items321"),
            resources.GetString("backColorPicker1.Items322"),
            resources.GetString("backColorPicker1.Items323"),
            resources.GetString("backColorPicker1.Items324"),
            resources.GetString("backColorPicker1.Items325"),
            resources.GetString("backColorPicker1.Items326"),
            resources.GetString("backColorPicker1.Items327"),
            resources.GetString("backColorPicker1.Items328"),
            resources.GetString("backColorPicker1.Items329"),
            resources.GetString("backColorPicker1.Items330"),
            resources.GetString("backColorPicker1.Items331"),
            resources.GetString("backColorPicker1.Items332"),
            resources.GetString("backColorPicker1.Items333"),
            resources.GetString("backColorPicker1.Items334"),
            resources.GetString("backColorPicker1.Items335"),
            resources.GetString("backColorPicker1.Items336"),
            resources.GetString("backColorPicker1.Items337"),
            resources.GetString("backColorPicker1.Items338"),
            resources.GetString("backColorPicker1.Items339"),
            resources.GetString("backColorPicker1.Items340"),
            resources.GetString("backColorPicker1.Items341"),
            resources.GetString("backColorPicker1.Items342"),
            resources.GetString("backColorPicker1.Items343"),
            resources.GetString("backColorPicker1.Items344"),
            resources.GetString("backColorPicker1.Items345"),
            resources.GetString("backColorPicker1.Items346"),
            resources.GetString("backColorPicker1.Items347"),
            resources.GetString("backColorPicker1.Items348"),
            resources.GetString("backColorPicker1.Items349"),
            resources.GetString("backColorPicker1.Items350"),
            resources.GetString("backColorPicker1.Items351"),
            resources.GetString("backColorPicker1.Items352"),
            resources.GetString("backColorPicker1.Items353"),
            resources.GetString("backColorPicker1.Items354"),
            resources.GetString("backColorPicker1.Items355"),
            resources.GetString("backColorPicker1.Items356"),
            resources.GetString("backColorPicker1.Items357"),
            resources.GetString("backColorPicker1.Items358"),
            resources.GetString("backColorPicker1.Items359"),
            resources.GetString("backColorPicker1.Items360"),
            resources.GetString("backColorPicker1.Items361"),
            resources.GetString("backColorPicker1.Items362"),
            resources.GetString("backColorPicker1.Items363"),
            resources.GetString("backColorPicker1.Items364"),
            resources.GetString("backColorPicker1.Items365"),
            resources.GetString("backColorPicker1.Items366"),
            resources.GetString("backColorPicker1.Items367"),
            resources.GetString("backColorPicker1.Items368"),
            resources.GetString("backColorPicker1.Items369"),
            resources.GetString("backColorPicker1.Items370"),
            resources.GetString("backColorPicker1.Items371"),
            resources.GetString("backColorPicker1.Items372"),
            resources.GetString("backColorPicker1.Items373"),
            resources.GetString("backColorPicker1.Items374"),
            resources.GetString("backColorPicker1.Items375"),
            resources.GetString("backColorPicker1.Items376"),
            resources.GetString("backColorPicker1.Items377"),
            resources.GetString("backColorPicker1.Items378"),
            resources.GetString("backColorPicker1.Items379"),
            resources.GetString("backColorPicker1.Items380"),
            resources.GetString("backColorPicker1.Items381"),
            resources.GetString("backColorPicker1.Items382"),
            resources.GetString("backColorPicker1.Items383"),
            resources.GetString("backColorPicker1.Items384"),
            resources.GetString("backColorPicker1.Items385"),
            resources.GetString("backColorPicker1.Items386"),
            resources.GetString("backColorPicker1.Items387"),
            resources.GetString("backColorPicker1.Items388"),
            resources.GetString("backColorPicker1.Items389"),
            resources.GetString("backColorPicker1.Items390"),
            resources.GetString("backColorPicker1.Items391"),
            resources.GetString("backColorPicker1.Items392"),
            resources.GetString("backColorPicker1.Items393"),
            resources.GetString("backColorPicker1.Items394"),
            resources.GetString("backColorPicker1.Items395"),
            resources.GetString("backColorPicker1.Items396"),
            resources.GetString("backColorPicker1.Items397"),
            resources.GetString("backColorPicker1.Items398"),
            resources.GetString("backColorPicker1.Items399"),
            resources.GetString("backColorPicker1.Items400"),
            resources.GetString("backColorPicker1.Items401"),
            resources.GetString("backColorPicker1.Items402"),
            resources.GetString("backColorPicker1.Items403"),
            resources.GetString("backColorPicker1.Items404"),
            resources.GetString("backColorPicker1.Items405"),
            resources.GetString("backColorPicker1.Items406"),
            resources.GetString("backColorPicker1.Items407"),
            resources.GetString("backColorPicker1.Items408"),
            resources.GetString("backColorPicker1.Items409"),
            resources.GetString("backColorPicker1.Items410"),
            resources.GetString("backColorPicker1.Items411"),
            resources.GetString("backColorPicker1.Items412"),
            resources.GetString("backColorPicker1.Items413"),
            resources.GetString("backColorPicker1.Items414"),
            resources.GetString("backColorPicker1.Items415"),
            resources.GetString("backColorPicker1.Items416"),
            resources.GetString("backColorPicker1.Items417"),
            resources.GetString("backColorPicker1.Items418"),
            resources.GetString("backColorPicker1.Items419"),
            resources.GetString("backColorPicker1.Items420"),
            resources.GetString("backColorPicker1.Items421"),
            resources.GetString("backColorPicker1.Items422"),
            resources.GetString("backColorPicker1.Items423"),
            resources.GetString("backColorPicker1.Items424"),
            resources.GetString("backColorPicker1.Items425"),
            resources.GetString("backColorPicker1.Items426"),
            resources.GetString("backColorPicker1.Items427"),
            resources.GetString("backColorPicker1.Items428"),
            resources.GetString("backColorPicker1.Items429"),
            resources.GetString("backColorPicker1.Items430"),
            resources.GetString("backColorPicker1.Items431"),
            resources.GetString("backColorPicker1.Items432"),
            resources.GetString("backColorPicker1.Items433"),
            resources.GetString("backColorPicker1.Items434"),
            resources.GetString("backColorPicker1.Items435"),
            resources.GetString("backColorPicker1.Items436"),
            resources.GetString("backColorPicker1.Items437"),
            resources.GetString("backColorPicker1.Items438"),
            resources.GetString("backColorPicker1.Items439"),
            resources.GetString("backColorPicker1.Items440"),
            resources.GetString("backColorPicker1.Items441"),
            resources.GetString("backColorPicker1.Items442"),
            resources.GetString("backColorPicker1.Items443"),
            resources.GetString("backColorPicker1.Items444"),
            resources.GetString("backColorPicker1.Items445"),
            resources.GetString("backColorPicker1.Items446"),
            resources.GetString("backColorPicker1.Items447"),
            resources.GetString("backColorPicker1.Items448"),
            resources.GetString("backColorPicker1.Items449"),
            resources.GetString("backColorPicker1.Items450"),
            resources.GetString("backColorPicker1.Items451"),
            resources.GetString("backColorPicker1.Items452"),
            resources.GetString("backColorPicker1.Items453"),
            resources.GetString("backColorPicker1.Items454"),
            resources.GetString("backColorPicker1.Items455"),
            resources.GetString("backColorPicker1.Items456"),
            resources.GetString("backColorPicker1.Items457"),
            resources.GetString("backColorPicker1.Items458"),
            resources.GetString("backColorPicker1.Items459"),
            resources.GetString("backColorPicker1.Items460"),
            resources.GetString("backColorPicker1.Items461"),
            resources.GetString("backColorPicker1.Items462"),
            resources.GetString("backColorPicker1.Items463"),
            resources.GetString("backColorPicker1.Items464"),
            resources.GetString("backColorPicker1.Items465"),
            resources.GetString("backColorPicker1.Items466"),
            resources.GetString("backColorPicker1.Items467"),
            resources.GetString("backColorPicker1.Items468"),
            resources.GetString("backColorPicker1.Items469"),
            resources.GetString("backColorPicker1.Items470"),
            resources.GetString("backColorPicker1.Items471"),
            resources.GetString("backColorPicker1.Items472"),
            resources.GetString("backColorPicker1.Items473"),
            resources.GetString("backColorPicker1.Items474"),
            resources.GetString("backColorPicker1.Items475"),
            resources.GetString("backColorPicker1.Items476"),
            resources.GetString("backColorPicker1.Items477"),
            resources.GetString("backColorPicker1.Items478"),
            resources.GetString("backColorPicker1.Items479"),
            resources.GetString("backColorPicker1.Items480"),
            resources.GetString("backColorPicker1.Items481"),
            resources.GetString("backColorPicker1.Items482"),
            resources.GetString("backColorPicker1.Items483"),
            resources.GetString("backColorPicker1.Items484"),
            resources.GetString("backColorPicker1.Items485"),
            resources.GetString("backColorPicker1.Items486"),
            resources.GetString("backColorPicker1.Items487"),
            resources.GetString("backColorPicker1.Items488"),
            resources.GetString("backColorPicker1.Items489"),
            resources.GetString("backColorPicker1.Items490"),
            resources.GetString("backColorPicker1.Items491"),
            resources.GetString("backColorPicker1.Items492"),
            resources.GetString("backColorPicker1.Items493"),
            resources.GetString("backColorPicker1.Items494"),
            resources.GetString("backColorPicker1.Items495"),
            resources.GetString("backColorPicker1.Items496"),
            resources.GetString("backColorPicker1.Items497"),
            resources.GetString("backColorPicker1.Items498"),
            resources.GetString("backColorPicker1.Items499"),
            resources.GetString("backColorPicker1.Items500"),
            resources.GetString("backColorPicker1.Items501"),
            resources.GetString("backColorPicker1.Items502"),
            resources.GetString("backColorPicker1.Items503"),
            resources.GetString("backColorPicker1.Items504"),
            resources.GetString("backColorPicker1.Items505"),
            resources.GetString("backColorPicker1.Items506"),
            resources.GetString("backColorPicker1.Items507"),
            resources.GetString("backColorPicker1.Items508"),
            resources.GetString("backColorPicker1.Items509"),
            resources.GetString("backColorPicker1.Items510"),
            resources.GetString("backColorPicker1.Items511"),
            resources.GetString("backColorPicker1.Items512"),
            resources.GetString("backColorPicker1.Items513"),
            resources.GetString("backColorPicker1.Items514"),
            resources.GetString("backColorPicker1.Items515"),
            resources.GetString("backColorPicker1.Items516"),
            resources.GetString("backColorPicker1.Items517"),
            resources.GetString("backColorPicker1.Items518"),
            resources.GetString("backColorPicker1.Items519"),
            resources.GetString("backColorPicker1.Items520"),
            resources.GetString("backColorPicker1.Items521"),
            resources.GetString("backColorPicker1.Items522"),
            resources.GetString("backColorPicker1.Items523"),
            resources.GetString("backColorPicker1.Items524"),
            resources.GetString("backColorPicker1.Items525"),
            resources.GetString("backColorPicker1.Items526"),
            resources.GetString("backColorPicker1.Items527"),
            resources.GetString("backColorPicker1.Items528"),
            resources.GetString("backColorPicker1.Items529"),
            resources.GetString("backColorPicker1.Items530"),
            resources.GetString("backColorPicker1.Items531"),
            resources.GetString("backColorPicker1.Items532"),
            resources.GetString("backColorPicker1.Items533"),
            resources.GetString("backColorPicker1.Items534"),
            resources.GetString("backColorPicker1.Items535"),
            resources.GetString("backColorPicker1.Items536"),
            resources.GetString("backColorPicker1.Items537"),
            resources.GetString("backColorPicker1.Items538"),
            resources.GetString("backColorPicker1.Items539"),
            resources.GetString("backColorPicker1.Items540"),
            resources.GetString("backColorPicker1.Items541"),
            resources.GetString("backColorPicker1.Items542"),
            resources.GetString("backColorPicker1.Items543"),
            resources.GetString("backColorPicker1.Items544"),
            resources.GetString("backColorPicker1.Items545"),
            resources.GetString("backColorPicker1.Items546"),
            resources.GetString("backColorPicker1.Items547"),
            resources.GetString("backColorPicker1.Items548"),
            resources.GetString("backColorPicker1.Items549"),
            resources.GetString("backColorPicker1.Items550"),
            resources.GetString("backColorPicker1.Items551"),
            resources.GetString("backColorPicker1.Items552"),
            resources.GetString("backColorPicker1.Items553"),
            resources.GetString("backColorPicker1.Items554"),
            resources.GetString("backColorPicker1.Items555"),
            resources.GetString("backColorPicker1.Items556"),
            resources.GetString("backColorPicker1.Items557"),
            resources.GetString("backColorPicker1.Items558"),
            resources.GetString("backColorPicker1.Items559"),
            resources.GetString("backColorPicker1.Items560"),
            resources.GetString("backColorPicker1.Items561"),
            resources.GetString("backColorPicker1.Items562"),
            resources.GetString("backColorPicker1.Items563"),
            resources.GetString("backColorPicker1.Items564"),
            resources.GetString("backColorPicker1.Items565"),
            resources.GetString("backColorPicker1.Items566"),
            resources.GetString("backColorPicker1.Items567"),
            resources.GetString("backColorPicker1.Items568"),
            resources.GetString("backColorPicker1.Items569"),
            resources.GetString("backColorPicker1.Items570"),
            resources.GetString("backColorPicker1.Items571"),
            resources.GetString("backColorPicker1.Items572"),
            resources.GetString("backColorPicker1.Items573"),
            resources.GetString("backColorPicker1.Items574"),
            resources.GetString("backColorPicker1.Items575"),
            resources.GetString("backColorPicker1.Items576"),
            resources.GetString("backColorPicker1.Items577"),
            resources.GetString("backColorPicker1.Items578"),
            resources.GetString("backColorPicker1.Items579"),
            resources.GetString("backColorPicker1.Items580"),
            resources.GetString("backColorPicker1.Items581"),
            resources.GetString("backColorPicker1.Items582"),
            resources.GetString("backColorPicker1.Items583"),
            resources.GetString("backColorPicker1.Items584"),
            resources.GetString("backColorPicker1.Items585"),
            resources.GetString("backColorPicker1.Items586"),
            resources.GetString("backColorPicker1.Items587"),
            resources.GetString("backColorPicker1.Items588"),
            resources.GetString("backColorPicker1.Items589"),
            resources.GetString("backColorPicker1.Items590"),
            resources.GetString("backColorPicker1.Items591"),
            resources.GetString("backColorPicker1.Items592"),
            resources.GetString("backColorPicker1.Items593"),
            resources.GetString("backColorPicker1.Items594"),
            resources.GetString("backColorPicker1.Items595"),
            resources.GetString("backColorPicker1.Items596"),
            resources.GetString("backColorPicker1.Items597"),
            resources.GetString("backColorPicker1.Items598"),
            resources.GetString("backColorPicker1.Items599"),
            resources.GetString("backColorPicker1.Items600"),
            resources.GetString("backColorPicker1.Items601"),
            resources.GetString("backColorPicker1.Items602"),
            resources.GetString("backColorPicker1.Items603"),
            resources.GetString("backColorPicker1.Items604"),
            resources.GetString("backColorPicker1.Items605"),
            resources.GetString("backColorPicker1.Items606"),
            resources.GetString("backColorPicker1.Items607"),
            resources.GetString("backColorPicker1.Items608"),
            resources.GetString("backColorPicker1.Items609"),
            resources.GetString("backColorPicker1.Items610"),
            resources.GetString("backColorPicker1.Items611"),
            resources.GetString("backColorPicker1.Items612"),
            resources.GetString("backColorPicker1.Items613"),
            resources.GetString("backColorPicker1.Items614"),
            resources.GetString("backColorPicker1.Items615"),
            resources.GetString("backColorPicker1.Items616"),
            resources.GetString("backColorPicker1.Items617"),
            resources.GetString("backColorPicker1.Items618"),
            resources.GetString("backColorPicker1.Items619"),
            resources.GetString("backColorPicker1.Items620"),
            resources.GetString("backColorPicker1.Items621"),
            resources.GetString("backColorPicker1.Items622"),
            resources.GetString("backColorPicker1.Items623"),
            resources.GetString("backColorPicker1.Items624"),
            resources.GetString("backColorPicker1.Items625"),
            resources.GetString("backColorPicker1.Items626"),
            resources.GetString("backColorPicker1.Items627"),
            resources.GetString("backColorPicker1.Items628"),
            resources.GetString("backColorPicker1.Items629"),
            resources.GetString("backColorPicker1.Items630"),
            resources.GetString("backColorPicker1.Items631"),
            resources.GetString("backColorPicker1.Items632"),
            resources.GetString("backColorPicker1.Items633"),
            resources.GetString("backColorPicker1.Items634"),
            resources.GetString("backColorPicker1.Items635"),
            resources.GetString("backColorPicker1.Items636"),
            resources.GetString("backColorPicker1.Items637"),
            resources.GetString("backColorPicker1.Items638"),
            resources.GetString("backColorPicker1.Items639"),
            resources.GetString("backColorPicker1.Items640"),
            resources.GetString("backColorPicker1.Items641"),
            resources.GetString("backColorPicker1.Items642"),
            resources.GetString("backColorPicker1.Items643"),
            resources.GetString("backColorPicker1.Items644"),
            resources.GetString("backColorPicker1.Items645"),
            resources.GetString("backColorPicker1.Items646"),
            resources.GetString("backColorPicker1.Items647"),
            resources.GetString("backColorPicker1.Items648"),
            resources.GetString("backColorPicker1.Items649"),
            resources.GetString("backColorPicker1.Items650"),
            resources.GetString("backColorPicker1.Items651"),
            resources.GetString("backColorPicker1.Items652"),
            resources.GetString("backColorPicker1.Items653"),
            resources.GetString("backColorPicker1.Items654"),
            resources.GetString("backColorPicker1.Items655"),
            resources.GetString("backColorPicker1.Items656"),
            resources.GetString("backColorPicker1.Items657"),
            resources.GetString("backColorPicker1.Items658"),
            resources.GetString("backColorPicker1.Items659"),
            resources.GetString("backColorPicker1.Items660"),
            resources.GetString("backColorPicker1.Items661"),
            resources.GetString("backColorPicker1.Items662"),
            resources.GetString("backColorPicker1.Items663"),
            resources.GetString("backColorPicker1.Items664"),
            resources.GetString("backColorPicker1.Items665"),
            resources.GetString("backColorPicker1.Items666"),
            resources.GetString("backColorPicker1.Items667"),
            resources.GetString("backColorPicker1.Items668"),
            resources.GetString("backColorPicker1.Items669"),
            resources.GetString("backColorPicker1.Items670"),
            resources.GetString("backColorPicker1.Items671"),
            resources.GetString("backColorPicker1.Items672"),
            resources.GetString("backColorPicker1.Items673"),
            resources.GetString("backColorPicker1.Items674"),
            resources.GetString("backColorPicker1.Items675"),
            resources.GetString("backColorPicker1.Items676"),
            resources.GetString("backColorPicker1.Items677"),
            resources.GetString("backColorPicker1.Items678"),
            resources.GetString("backColorPicker1.Items679"),
            resources.GetString("backColorPicker1.Items680"),
            resources.GetString("backColorPicker1.Items681"),
            resources.GetString("backColorPicker1.Items682"),
            resources.GetString("backColorPicker1.Items683"),
            resources.GetString("backColorPicker1.Items684"),
            resources.GetString("backColorPicker1.Items685"),
            resources.GetString("backColorPicker1.Items686"),
            resources.GetString("backColorPicker1.Items687"),
            resources.GetString("backColorPicker1.Items688"),
            resources.GetString("backColorPicker1.Items689"),
            resources.GetString("backColorPicker1.Items690"),
            resources.GetString("backColorPicker1.Items691"),
            resources.GetString("backColorPicker1.Items692"),
            resources.GetString("backColorPicker1.Items693"),
            resources.GetString("backColorPicker1.Items694"),
            resources.GetString("backColorPicker1.Items695"),
            resources.GetString("backColorPicker1.Items696"),
            resources.GetString("backColorPicker1.Items697"),
            resources.GetString("backColorPicker1.Items698"),
            resources.GetString("backColorPicker1.Items699")});
			this.backColorPicker1.Name = "backColorPicker1";
			this.backColorPicker1.Tag = "Back Color";
			this.backColorPicker1.SelectedValueChanged += new System.EventHandler(this.ctlBackColor_Change);
			this.backColorPicker1.Validated += new System.EventHandler(this.ctlBackColor_Change);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusSelected,
            this.toolStripStatusLabel2,
            this.statusPosition});
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Name = "statusStrip1";
			// 
			// statusSelected
			// 
			this.statusSelected.Name = "statusSelected";
			resources.ApplyResources(this.statusSelected, "statusSelected");
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Gray;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
			// 
			// statusPosition
			// 
			this.statusPosition.Name = "statusPosition";
			resources.ApplyResources(this.statusPosition, "statusPosition");
			// 
			// mainSP
			// 
			resources.ApplyResources(this.mainSP, "mainSP");
			this.mainSP.Name = "mainSP";
			this.mainSP.TabStop = false;
			// 
			// ContextMenuTB
			// 
			this.ContextMenuTB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuTBClose,
            this.MenuTBSave,
            this.MenuTBCloseAllButThis});
			this.ContextMenuTB.Name = "ContextMenuTB";
			resources.ApplyResources(this.ContextMenuTB, "ContextMenuTB");
			// 
			// MenuTBClose
			// 
			this.MenuTBClose.Name = "MenuTBClose";
			resources.ApplyResources(this.MenuTBClose, "MenuTBClose");
			this.MenuTBClose.Click += new System.EventHandler(this.menuFileClose_Click);
			// 
			// MenuTBSave
			// 
			this.MenuTBSave.Name = "MenuTBSave";
			resources.ApplyResources(this.MenuTBSave, "MenuTBSave");
			this.MenuTBSave.Click += new System.EventHandler(this.menuFileSave_Click);
			// 
			// MenuTBCloseAllButThis
			// 
			this.MenuTBCloseAllButThis.Name = "MenuTBCloseAllButThis";
			resources.ApplyResources(this.MenuTBCloseAllButThis, "MenuTBCloseAllButThis");
			this.MenuTBCloseAllButThis.Click += new System.EventHandler(this.menuWndCloseAllButCurrent_Click);
			// 
			// mainProperties
			// 
			resources.ApplyResources(this.mainProperties, "mainProperties");
			this.mainProperties.Name = "mainProperties";
			// 
			// RdlDesigner
			// 
			this.AllowDrop = true;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.mainSP);
			this.Controls.Add(this.mainProperties);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.mainTB);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "RdlDesigner";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.RdlDesigner_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.RdlDesigner_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.RdlDesigner_DragEnter);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.mainTB.ResumeLayout(false);
			this.mainTB.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ContextMenuTB.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem dataToolStripMenuItem;
        private ToolStripMenuItem formatToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem newReportToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem pDFToolStripMenuItem;
        private ToolStripMenuItem tIFFToolStripMenuItem;
        private ToolStripMenuItem cSVToolStripMenuItem;
        private ToolStripMenuItem excelToolStripMenuItem;
        private ToolStripMenuItem rTFDOCToolStripMenuItem;
        private ToolStripMenuItem xMLToolStripMenuItem;
        private ToolStripMenuItem webPageHTMLToolStripMenuItem;
        private ToolStripMenuItem webArchiveSingleFileMHTToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem recentFilesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem findNextToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem goToToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem formatXMLToolStripMenuItem;
        private ToolStripMenuItem designerToolStripMenuItem;
        private ToolStripMenuItem rDLTextToolStripMenuItem;
        private ToolStripMenuItem previewToolStripMenuItem;
        private ToolStripMenuItem showReportInBrowserToolStripMenuItem;
        private ToolStripMenuItem propertiesWindowsToolStripMenuItem;
        private ToolStripMenuItem dataSetsToolStripMenuItem;
        private ToolStripMenuItem embeddedImagesToolStripMenuItem;
        private ToolStripMenuItem createSharedDataSourceToolStripMenuItem;
        private ToolStripMenuItem dataSourcesToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem alignToolStripMenuItem;
        private ToolStripMenuItem leftsToolStripMenuItem;
        private ToolStripMenuItem centersToolStripMenuItem;
        private ToolStripMenuItem rightsToolStripMenuItem;
        private ToolStripMenuItem topsToolStripMenuItem;
        private ToolStripMenuItem middlesToolStripMenuItem;
        private ToolStripMenuItem bottomsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem sizeToolStripMenuItem;
        private ToolStripMenuItem widthToolStripMenuItem;
        private ToolStripMenuItem heightToolStripMenuItem;
        private ToolStripMenuItem bothToolStripMenuItem;
        private ToolStripMenuItem horizontalSpacingToolStripMenuItem;
        private ToolStripMenuItem makeEqualToolStripMenuItem;
        private ToolStripMenuItem increaseToolStripMenuItem;
        private ToolStripMenuItem decreaseToolStripMenuItem;
        private ToolStripMenuItem zeroToolStripMenuItem;
        private ToolStripMenuItem verticalSpacingToolStripMenuItem;
        private ToolStripMenuItem makeEqualToolStripMenuItem1;
        private ToolStripMenuItem increaseToolStripMenuItem1;
        private ToolStripMenuItem decreaseToolStripMenuItem1;
        private ToolStripMenuItem zeroToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem paddingLeftToolStripMenuItem;
        private ToolStripMenuItem increaseToolStripMenuItem2;
        private ToolStripMenuItem decreaseToolStripMenuItem2;
        private ToolStripMenuItem zeroToolStripMenuItem2;
        private ToolStripMenuItem paddingRightToolStripMenuItem;
        private ToolStripMenuItem increaseToolStripMenuItem3;
        private ToolStripMenuItem decreaseToolStripMenuItem3;
        private ToolStripMenuItem zeroToolStripMenuItem3;
        private ToolStripMenuItem paddintTopToolStripMenuItem;
        private ToolStripMenuItem increaseToolStripMenuItem4;
        private ToolStripMenuItem decreaseToolStripMenuItem4;
        private ToolStripMenuItem zeroToolStripMenuItem4;
        private ToolStripMenuItem paddingBottomToolStripMenuItem;
        private ToolStripMenuItem increaseToolStripMenuItem5;
        private ToolStripMenuItem decreaseToolStripMenuItem5;
        private ToolStripMenuItem zeroToolStripMenuItem5;
        private ToolStripMenuItem validateRDLToolStripMenuItem;
        private ToolStripMenuItem startDesktopServerToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripMenuItem cascadeToolStripMenuItem;
        private ToolStripMenuItem tileToolStripMenuItem;
        private ToolStripMenuItem horizontalToolStripMenuItem;
        private ToolStripMenuItem verticallyToolStripMenuItem;
        private ToolStripMenuItem closeAllToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem1;
        private ToolStripMenuItem supportToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip mainTB;
        private ToolStripButton newToolStripButton1;
        private ToolStrip toolStrip1;
        private ToolStripButton openToolStripButton1;
        private ToolStripButton saveToolStripButton1;
        private ToolStripButton cutToolStripButton1;
        private ToolStripButton copyToolStripButton1;
        private ToolStripButton pasteToolStripButton1;
        private ToolStripButton undoToolStripButton1;
        private ToolStripButton textboxToolStripButton1;
        private ToolStripButton chartToolStripButton1;
        private ToolStripButton tableToolStripButton1;
        private ToolStripButton listToolStripButton1;
        private ToolStripButton imageToolStripButton1;
        private ToolStripButton matrixToolStripButton1;
        private ToolStripButton subreportToolStripButton1;
        private ToolStripButton rectangleToolStripButton1;
        private ToolStripButton lineToolStripButton1;
        private ToolStripLabel fxToolStripLabel1;
        private ToolStripTextBox ctlEditTextbox;
        private ToolStripButton boldToolStripButton1;
        private ToolStripButton italiacToolStripButton1;
        private ToolStripButton underlineToolStripButton2;
        private ToolStripButton leftAlignToolStripButton2;
        private ToolStripButton centerAlignToolStripButton2;
        private ToolStripButton rightAlignToolStripButton3;
        private ToolStripComboBox fontToolStripComboBox1;
        private ToolStripComboBox fontSizeToolStripComboBox1;
        private ColorPicker foreColorPicker1;
        private ColorPicker backColorPicker1;
        private ToolStripButton printToolStripButton2;
        private ToolStripComboBox zoomToolStripComboBox1;
        private ToolStripButton selectToolStripButton2;
        private TabControl mainTC;
        private ToolStripButton pdfToolStripButton2;
        private ToolStripButton htmlToolStripButton2;
        private ToolStripButton excelToolStripButton2;
        private ToolStripButton XmlToolStripButton2;
        private ToolStripButton MhtToolStripButton2;
        private ToolStripButton CsvToolStripButton2;
        private ToolStripButton RtfToolStripButton2;
        private ToolStripButton TifToolStripButton2;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private StatusStrip statusStrip1;
        private Splitter mainSP;
        private PropertyCtl mainProperties;
        private ToolStripStatusLabel statusSelected;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel statusPosition;
        private ToolStripMenuItem insertToolStripMenuItem;
        private ToolStripMenuItem chartToolStripMenuItem;
        private ToolStripMenuItem gridToolStripMenuItem;
        private ToolStripMenuItem imageToolStripMenuItem;
        private ToolStripMenuItem lineToolStripMenuItem;
        private ToolStripMenuItem listToolStripMenuItem;
        private ToolStripMenuItem matrixToolStripMenuItem;
        private ToolStripMenuItem rectangleToolStripMenuItem;
        private ToolStripMenuItem subReportToolStripMenuItem;
        private ToolStripMenuItem tableToolStripMenuItem;
		private ToolStripMenuItem textboxToolStripMenuItem;
        private ToolStripMenuItem pDFOldStyleToolStripMenuItem;
        private ToolStripMenuItem dOCToolStripMenuItem;
		private ContextMenuStrip ContextMenuTB;
		private IContainer components;
		private ToolStripMenuItem MenuTBClose;
		private ToolStripMenuItem MenuTBSave;
		private ToolStripMenuItem MenuTBCloseAllButThis;
	}
}
