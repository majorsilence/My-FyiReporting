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
private System.ComponentModel.Container components = null;
private MDIChild printChild=null;
private DialogValidateRdl _ValidateRdl=null;
private DockStyle _PropertiesLocation = DockStyle.Right;
private System.Windows.Forms.StatusBar mainSB;
private StatusBarPanel statusPrimary;
private StatusBarPanel statusSelected;
private StatusBarPanel statusPosition;
private System.Windows.Forms.ToolBar mainTB;
private PropertyCtl mainProperties;
private Splitter mainSp;
internal System.Windows.Forms.TabControl mainTC;
private SimpleToggle ctlBold=null;
private SimpleToggle ctlItalic=null;
private SimpleToggle ctlUnderline=null;
private SimpleToggle ctlLAlign = null;
private SimpleToggle ctlRAlign = null;
private SimpleToggle ctlCAlign = null;
private ComboBox ctlFont = null;
private ComboBox ctlFontSize=null;
private ColorPicker ctlForeColor=null;
private ColorPicker ctlBackColor = null;
private Button ctlNew=null;
private Button ctlOpen=null;
private Button ctlSave=null;
private Button ctlCut=null;
private Button ctlCopy=null;
private Button ctlUndo=null;
private Button ctlPaste=null;
private Button ctlPrint=null;
private Button ctlPdf=null;
private Button ctlTif = null;
private Button ctlXml = null;
private Button ctlHtml=null;
private Button ctlMht=null;
private Button ctlCsv = null;
private Button ctlRtf = null;
private Button ctlExcel = null;
private ComboBox ctlZoom = null;
private SimpleToggle ctlInsertCurrent=null;
private SimpleToggle ctlInsertTextbox=null;
private SimpleToggle ctlInsertChart=null;
private SimpleToggle ctlInsertRectangle=null;
private SimpleToggle ctlInsertTable=null;
private SimpleToggle ctlInsertMatrix=null;
private SimpleToggle ctlInsertList=null;
private SimpleToggle ctlInsertLine=null;
private SimpleToggle ctlInsertImage=null;
private SimpleToggle ctlInsertSubreport=null;
private SimpleToggle ctlSelectTool = null;
private System.Windows.Forms.TextBox ctlEditTextbox=null;
private System.Windows.Forms.Label ctlEditLabel=null;
private System.Windows.Forms.Button bTable;
private System.Windows.Forms.Button bLine;
private System.Windows.Forms.Button bImage;
private System.Windows.Forms.Button bRectangle;
private System.Windows.Forms.Button bSubreport;
private System.Windows.Forms.Button bList;
private System.Windows.Forms.Button bChart;
private System.Windows.Forms.Button bText;
private System.Windows.Forms.Button bMatrix;
private System.Windows.Forms.Button bPrint;
private System.Windows.Forms.Button bSave;
private System.Windows.Forms.Button bOpen;
private System.Windows.Forms.Button bPaste;
private System.Windows.Forms.Button bCopy;
private System.Windows.Forms.Button bCut;
private System.Windows.Forms.Button bNew;
private System.Windows.Forms.Button bUndo;
private System.Windows.Forms.Button bPdf;
private System.Windows.Forms.Button bXml;
private System.Windows.Forms.Button bHtml;
private System.Windows.Forms.Button bMht;
private Button bCsv;
private Button bCAlign;
private Button bRAlign;
private Button bLAlign;
private Button bRtf;
private Button bSelectTool;
private Button bExcel;
private Button bTif;


		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdlDesigner));
            this.bExcel = new System.Windows.Forms.Button();
            this.bSelectTool = new System.Windows.Forms.Button();
            this.bRtf = new System.Windows.Forms.Button();
            this.bLAlign = new System.Windows.Forms.Button();
            this.bRAlign = new System.Windows.Forms.Button();
            this.bCAlign = new System.Windows.Forms.Button();
            this.bCsv = new System.Windows.Forms.Button();
            this.bMht = new System.Windows.Forms.Button();
            this.bHtml = new System.Windows.Forms.Button();
            this.bXml = new System.Windows.Forms.Button();
            this.bPdf = new System.Windows.Forms.Button();
            this.bUndo = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.bCut = new System.Windows.Forms.Button();
            this.bCopy = new System.Windows.Forms.Button();
            this.bPaste = new System.Windows.Forms.Button();
            this.bOpen = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bPrint = new System.Windows.Forms.Button();
            this.bMatrix = new System.Windows.Forms.Button();
            this.bText = new System.Windows.Forms.Button();
            this.bChart = new System.Windows.Forms.Button();
            this.bList = new System.Windows.Forms.Button();
            this.bSubreport = new System.Windows.Forms.Button();
            this.bRectangle = new System.Windows.Forms.Button();
            this.bImage = new System.Windows.Forms.Button();
            this.bLine = new System.Windows.Forms.Button();
            this.bTable = new System.Windows.Forms.Button();
            this.bTif = new System.Windows.Forms.Button();
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
            this.tIFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bExcel
            // 
            this.bExcel.Image = ((System.Drawing.Image)(resources.GetObject("bExcel.Image")));
            this.bExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bExcel.Location = new System.Drawing.Point(410, 61);
            this.bExcel.Name = "bExcel";
            this.bExcel.Size = new System.Drawing.Size(75, 23);
            this.bExcel.TabIndex = 27;
            this.bExcel.Text = "XLS";
            this.bExcel.Visible = false;
            // 
            // bSelectTool
            // 
            this.bSelectTool.Image = ((System.Drawing.Image)(resources.GetObject("bSelectTool.Image")));
            this.bSelectTool.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bSelectTool.Location = new System.Drawing.Point(410, 32);
            this.bSelectTool.Name = "bSelectTool";
            this.bSelectTool.Size = new System.Drawing.Size(75, 23);
            this.bSelectTool.TabIndex = 26;
            this.bSelectTool.Text = "SelectTool";
            this.bSelectTool.Visible = false;
            // 
            // bRtf
            // 
            this.bRtf.Image = ((System.Drawing.Image)(resources.GetObject("bRtf.Image")));
            this.bRtf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bRtf.Location = new System.Drawing.Point(504, 418);
            this.bRtf.Name = "bRtf";
            this.bRtf.Size = new System.Drawing.Size(75, 23);
            this.bRtf.TabIndex = 25;
            this.bRtf.Text = "RTF";
            this.bRtf.Visible = false;
            // 
            // bLAlign
            // 
            this.bLAlign.Image = ((System.Drawing.Image)(resources.GetObject("bLAlign.Image")));
            this.bLAlign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bLAlign.Location = new System.Drawing.Point(504, 389);
            this.bLAlign.Name = "bLAlign";
            this.bLAlign.Size = new System.Drawing.Size(75, 23);
            this.bLAlign.TabIndex = 24;
            this.bLAlign.Text = "L Align";
            this.bLAlign.Visible = false;
            // 
            // bRAlign
            // 
            this.bRAlign.Image = ((System.Drawing.Image)(resources.GetObject("bRAlign.Image")));
            this.bRAlign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bRAlign.Location = new System.Drawing.Point(608, 389);
            this.bRAlign.Name = "bRAlign";
            this.bRAlign.Size = new System.Drawing.Size(75, 23);
            this.bRAlign.TabIndex = 23;
            this.bRAlign.Text = "R Align";
            this.bRAlign.Visible = false;
            // 
            // bCAlign
            // 
            this.bCAlign.Image = ((System.Drawing.Image)(resources.GetObject("bCAlign.Image")));
            this.bCAlign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCAlign.Location = new System.Drawing.Point(608, 418);
            this.bCAlign.Name = "bCAlign";
            this.bCAlign.Size = new System.Drawing.Size(75, 23);
            this.bCAlign.TabIndex = 22;
            this.bCAlign.Text = "C Align";
            this.bCAlign.Visible = false;
            // 
            // bCsv
            // 
            this.bCsv.Image = global::fyiReporting.RdlDesign.Properties.Resources.csv;
            this.bCsv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCsv.Location = new System.Drawing.Point(608, 360);
            this.bCsv.Name = "bCsv";
            this.bCsv.Size = new System.Drawing.Size(75, 23);
            this.bCsv.TabIndex = 21;
            this.bCsv.Text = "CSV";
            this.bCsv.Visible = false;
            // 
            // bMht
            // 
            this.bMht.Image = ((System.Drawing.Image)(resources.GetObject("bMht.Image")));
            this.bMht.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMht.Location = new System.Drawing.Point(504, 360);
            this.bMht.Name = "bMht";
            this.bMht.Size = new System.Drawing.Size(75, 23);
            this.bMht.TabIndex = 20;
            this.bMht.Text = "HTML";
            this.bMht.Visible = false;
            // 
            // bHtml
            // 
            this.bHtml.Image = ((System.Drawing.Image)(resources.GetObject("bHtml.Image")));
            this.bHtml.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bHtml.Location = new System.Drawing.Point(608, 320);
            this.bHtml.Name = "bHtml";
            this.bHtml.Size = new System.Drawing.Size(75, 23);
            this.bHtml.TabIndex = 19;
            this.bHtml.Text = "HTML";
            this.bHtml.Visible = false;
            // 
            // bXml
            // 
            this.bXml.Image = ((System.Drawing.Image)(resources.GetObject("bXml.Image")));
            this.bXml.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bXml.Location = new System.Drawing.Point(504, 288);
            this.bXml.Name = "bXml";
            this.bXml.Size = new System.Drawing.Size(75, 23);
            this.bXml.TabIndex = 18;
            this.bXml.Text = "XML";
            this.bXml.Visible = false;
            // 
            // bPdf
            // 
            this.bPdf.Image = ((System.Drawing.Image)(resources.GetObject("bPdf.Image")));
            this.bPdf.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bPdf.Location = new System.Drawing.Point(504, 320);
            this.bPdf.Name = "bPdf";
            this.bPdf.Size = new System.Drawing.Size(75, 23);
            this.bPdf.TabIndex = 17;
            this.bPdf.Text = "PDF";
            this.bPdf.Visible = false;
            // 
            // bUndo
            // 
            this.bUndo.Image = ((System.Drawing.Image)(resources.GetObject("bUndo.Image")));
            this.bUndo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bUndo.Location = new System.Drawing.Point(504, 256);
            this.bUndo.Name = "bUndo";
            this.bUndo.Size = new System.Drawing.Size(75, 23);
            this.bUndo.TabIndex = 16;
            this.bUndo.Text = "Undo";
            this.bUndo.Visible = false;
            // 
            // bNew
            // 
            this.bNew.Image = ((System.Drawing.Image)(resources.GetObject("bNew.Image")));
            this.bNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bNew.Location = new System.Drawing.Point(504, 224);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(75, 23);
            this.bNew.TabIndex = 15;
            this.bNew.Text = "New";
            this.bNew.Visible = false;
            // 
            // bCut
            // 
            this.bCut.Image = ((System.Drawing.Image)(resources.GetObject("bCut.Image")));
            this.bCut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCut.Location = new System.Drawing.Point(504, 32);
            this.bCut.Name = "bCut";
            this.bCut.Size = new System.Drawing.Size(75, 23);
            this.bCut.TabIndex = 14;
            this.bCut.Text = "Cut";
            this.bCut.Visible = false;
            // 
            // bCopy
            // 
            this.bCopy.Image = ((System.Drawing.Image)(resources.GetObject("bCopy.Image")));
            this.bCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bCopy.Location = new System.Drawing.Point(504, 64);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(75, 23);
            this.bCopy.TabIndex = 13;
            this.bCopy.Text = "Copy";
            this.bCopy.Visible = false;
            // 
            // bPaste
            // 
            this.bPaste.Image = ((System.Drawing.Image)(resources.GetObject("bPaste.Image")));
            this.bPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bPaste.Location = new System.Drawing.Point(504, 96);
            this.bPaste.Name = "bPaste";
            this.bPaste.Size = new System.Drawing.Size(75, 23);
            this.bPaste.TabIndex = 12;
            this.bPaste.Text = "Paste";
            this.bPaste.Visible = false;
            // 
            // bOpen
            // 
            this.bOpen.Image = ((System.Drawing.Image)(resources.GetObject("bOpen.Image")));
            this.bOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bOpen.Location = new System.Drawing.Point(504, 128);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(75, 23);
            this.bOpen.TabIndex = 11;
            this.bOpen.Text = "Open";
            this.bOpen.Visible = false;
            // 
            // bSave
            // 
            this.bSave.Image = ((System.Drawing.Image)(resources.GetObject("bSave.Image")));
            this.bSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bSave.Location = new System.Drawing.Point(504, 160);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 10;
            this.bSave.Text = "Save";
            this.bSave.Visible = false;
            // 
            // bPrint
            // 
            this.bPrint.Image = ((System.Drawing.Image)(resources.GetObject("bPrint.Image")));
            this.bPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bPrint.Location = new System.Drawing.Point(504, 192);
            this.bPrint.Name = "bPrint";
            this.bPrint.Size = new System.Drawing.Size(75, 23);
            this.bPrint.TabIndex = 9;
            this.bPrint.Text = "Print";
            this.bPrint.Visible = false;
            // 
            // bMatrix
            // 
            this.bMatrix.Image = ((System.Drawing.Image)(resources.GetObject("bMatrix.Image")));
            this.bMatrix.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMatrix.Location = new System.Drawing.Point(608, 64);
            this.bMatrix.Name = "bMatrix";
            this.bMatrix.Size = new System.Drawing.Size(75, 23);
            this.bMatrix.TabIndex = 8;
            this.bMatrix.Text = "Matrix";
            this.bMatrix.Visible = false;
            // 
            // bText
            // 
            this.bText.Image = ((System.Drawing.Image)(resources.GetObject("bText.Image")));
            this.bText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bText.Location = new System.Drawing.Point(608, 96);
            this.bText.Name = "bText";
            this.bText.Size = new System.Drawing.Size(75, 23);
            this.bText.TabIndex = 7;
            this.bText.Text = "Text";
            this.bText.Visible = false;
            // 
            // bChart
            // 
            this.bChart.Image = ((System.Drawing.Image)(resources.GetObject("bChart.Image")));
            this.bChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bChart.Location = new System.Drawing.Point(608, 128);
            this.bChart.Name = "bChart";
            this.bChart.Size = new System.Drawing.Size(75, 23);
            this.bChart.TabIndex = 6;
            this.bChart.Text = "Chart";
            this.bChart.Visible = false;
            // 
            // bList
            // 
            this.bList.Image = ((System.Drawing.Image)(resources.GetObject("bList.Image")));
            this.bList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bList.Location = new System.Drawing.Point(608, 160);
            this.bList.Name = "bList";
            this.bList.Size = new System.Drawing.Size(75, 23);
            this.bList.TabIndex = 5;
            this.bList.Text = "List";
            this.bList.Visible = false;
            // 
            // bSubreport
            // 
            this.bSubreport.Image = ((System.Drawing.Image)(resources.GetObject("bSubreport.Image")));
            this.bSubreport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bSubreport.Location = new System.Drawing.Point(608, 192);
            this.bSubreport.Name = "bSubreport";
            this.bSubreport.Size = new System.Drawing.Size(75, 23);
            this.bSubreport.TabIndex = 4;
            this.bSubreport.Text = "Subreport";
            this.bSubreport.Visible = false;
            // 
            // bRectangle
            // 
            this.bRectangle.Image = ((System.Drawing.Image)(resources.GetObject("bRectangle.Image")));
            this.bRectangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bRectangle.Location = new System.Drawing.Point(608, 224);
            this.bRectangle.Name = "bRectangle";
            this.bRectangle.Size = new System.Drawing.Size(75, 23);
            this.bRectangle.TabIndex = 3;
            this.bRectangle.Text = "Rect";
            this.bRectangle.Visible = false;
            // 
            // bImage
            // 
            this.bImage.Image = ((System.Drawing.Image)(resources.GetObject("bImage.Image")));
            this.bImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bImage.Location = new System.Drawing.Point(608, 256);
            this.bImage.Name = "bImage";
            this.bImage.Size = new System.Drawing.Size(75, 23);
            this.bImage.TabIndex = 2;
            this.bImage.Text = "Image";
            this.bImage.Visible = false;
            // 
            // bLine
            // 
            this.bLine.Image = ((System.Drawing.Image)(resources.GetObject("bLine.Image")));
            this.bLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bLine.Location = new System.Drawing.Point(608, 288);
            this.bLine.Name = "bLine";
            this.bLine.Size = new System.Drawing.Size(75, 23);
            this.bLine.TabIndex = 1;
            this.bLine.Text = "Line";
            this.bLine.Visible = false;
            // 
            // bTable
            // 
            this.bTable.Image = ((System.Drawing.Image)(resources.GetObject("bTable.Image")));
            this.bTable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bTable.Location = new System.Drawing.Point(608, 32);
            this.bTable.Name = "bTable";
            this.bTable.Size = new System.Drawing.Size(75, 23);
            this.bTable.TabIndex = 0;
            this.bTable.Text = "Table";
            this.bTable.Visible = false;
            // 
            // bTif
            // 
            this.bTif.Image = ((System.Drawing.Image)(resources.GetObject("bTif.Image")));
            this.bTif.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bTif.Location = new System.Drawing.Point(410, 96);
            this.bTif.Name = "bTif";
            this.bTif.Size = new System.Drawing.Size(75, 23);
            this.bTif.TabIndex = 28;
            this.bTif.Text = "TIF";
            this.bTif.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(712, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuFile_Popup);
            // 
            // newReportToolStripMenuItem
            // 
            this.newReportToolStripMenuItem.Name = "newReportToolStripMenuItem";
            this.newReportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newReportToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.newReportToolStripMenuItem.Text = "&New Report...";
            this.newReportToolStripMenuItem.Click += new System.EventHandler(this.menuFileNewReport_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.menuFileClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.menuFileSaveAs_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Enabled = false;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.menuFilePrint_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pDFToolStripMenuItem,
            this.tIFFToolStripMenuItem,
            this.cSVToolStripMenuItem,
            this.excelToolStripMenuItem,
            this.rTFDOCToolStripMenuItem,
            this.xMLToolStripMenuItem,
            this.webPageHTMLToolStripMenuItem,
            this.webArchiveSingleFileMHTToolStripMenuItem});
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // pDFToolStripMenuItem
            // 
            this.pDFToolStripMenuItem.Name = "pDFToolStripMenuItem";
            this.pDFToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.pDFToolStripMenuItem.Text = "PDF...";
            // 
            // tIFFToolStripMenuItem
            // 
            this.tIFFToolStripMenuItem.Name = "tIFFToolStripMenuItem";
            this.tIFFToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.tIFFToolStripMenuItem.Text = "TIF...";
            // 
            // cSVToolStripMenuItem
            // 
            this.cSVToolStripMenuItem.Name = "cSVToolStripMenuItem";
            this.cSVToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.cSVToolStripMenuItem.Text = "CSV...";
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.excelToolStripMenuItem.Text = "Excel";
            // 
            // rTFDOCToolStripMenuItem
            // 
            this.rTFDOCToolStripMenuItem.Name = "rTFDOCToolStripMenuItem";
            this.rTFDOCToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.rTFDOCToolStripMenuItem.Text = "RTF, DOC";
            // 
            // xMLToolStripMenuItem
            // 
            this.xMLToolStripMenuItem.Name = "xMLToolStripMenuItem";
            this.xMLToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.xMLToolStripMenuItem.Text = "XML...";
            // 
            // webPageHTMLToolStripMenuItem
            // 
            this.webPageHTMLToolStripMenuItem.Name = "webPageHTMLToolStripMenuItem";
            this.webPageHTMLToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.webPageHTMLToolStripMenuItem.Text = "Web Page, HTML...";
            // 
            // webArchiveSingleFileMHTToolStripMenuItem
            // 
            this.webArchiveSingleFileMHTToolStripMenuItem.Name = "webArchiveSingleFileMHTToolStripMenuItem";
            this.webArchiveSingleFileMHTToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.webArchiveSingleFileMHTToolStripMenuItem.Text = "Web Archive, single file MHT...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Enabled = false;
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent &Files";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(185, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
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
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.menuEditUndo_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.menuEditRedo_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(167, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.menuEditCut_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.menuEditCopy_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.menuEditPaste_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.menuEditDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(167, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.menuEditSelectAll_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(167, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.findToolStripMenuItem.Text = "&Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.menuEditFind_Click);
            // 
            // findNextToolStripMenuItem
            // 
            this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
            this.findNextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.findNextToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.findNextToolStripMenuItem.Text = "Find Next";
            this.findNextToolStripMenuItem.Click += new System.EventHandler(this.menuEditFindNext_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.replaceToolStripMenuItem.Text = "&Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.menuEditReplace_Click);
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.goToToolStripMenuItem.Text = "&Go To...";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.menuEditGoto_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(167, 6);
            // 
            // formatXMLToolStripMenuItem
            // 
            this.formatXMLToolStripMenuItem.Name = "formatXMLToolStripMenuItem";
            this.formatXMLToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.formatXMLToolStripMenuItem.Text = "Format XM&L";
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
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // designerToolStripMenuItem
            // 
            this.designerToolStripMenuItem.Name = "designerToolStripMenuItem";
            this.designerToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.designerToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.designerToolStripMenuItem.Text = "Designer";
            this.designerToolStripMenuItem.Click += new System.EventHandler(this.menuViewDesigner_Click);
            // 
            // rDLTextToolStripMenuItem
            // 
            this.rDLTextToolStripMenuItem.Name = "rDLTextToolStripMenuItem";
            this.rDLTextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F7)));
            this.rDLTextToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.rDLTextToolStripMenuItem.Text = "RDL Text";
            this.rDLTextToolStripMenuItem.Click += new System.EventHandler(this.menuViewRDL_Click);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.previewToolStripMenuItem.Text = "Preview";
            this.previewToolStripMenuItem.Click += new System.EventHandler(this.menuViewPreview_Click);
            // 
            // showReportInBrowserToolStripMenuItem
            // 
            this.showReportInBrowserToolStripMenuItem.Name = "showReportInBrowserToolStripMenuItem";
            this.showReportInBrowserToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.showReportInBrowserToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.showReportInBrowserToolStripMenuItem.Text = "Show Report in Browser";
            this.showReportInBrowserToolStripMenuItem.Click += new System.EventHandler(this.menuViewBrowser_Click);
            // 
            // propertiesWindowsToolStripMenuItem
            // 
            this.propertiesWindowsToolStripMenuItem.Name = "propertiesWindowsToolStripMenuItem";
            this.propertiesWindowsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.propertiesWindowsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.propertiesWindowsToolStripMenuItem.Text = "Properties Windows";
            this.propertiesWindowsToolStripMenuItem.Click += new System.EventHandler(this.menuEditProperties_Click);
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
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "&Data";
            // 
            // dataSetsToolStripMenuItem
            // 
            this.dataSetsToolStripMenuItem.Name = "dataSetsToolStripMenuItem";
            this.dataSetsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.dataSetsToolStripMenuItem.Text = "&Data Sets";
            // 
            // dataSourcesToolStripMenuItem1
            // 
            this.dataSourcesToolStripMenuItem1.Name = "dataSourcesToolStripMenuItem1";
            this.dataSourcesToolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
            this.dataSourcesToolStripMenuItem1.Text = "Data &Sources";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(219, 6);
            // 
            // embeddedImagesToolStripMenuItem
            // 
            this.embeddedImagesToolStripMenuItem.Name = "embeddedImagesToolStripMenuItem";
            this.embeddedImagesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.embeddedImagesToolStripMenuItem.Text = "&Embedded Images...";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(219, 6);
            // 
            // createSharedDataSourceToolStripMenuItem
            // 
            this.createSharedDataSourceToolStripMenuItem.Name = "createSharedDataSourceToolStripMenuItem";
            this.createSharedDataSourceToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.createSharedDataSourceToolStripMenuItem.Text = "&Create Shared Data Source...";
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
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.formatToolStripMenuItem.Text = "F&ormat";
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
            this.alignToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.alignToolStripMenuItem.Text = "&Align";
            // 
            // leftsToolStripMenuItem
            // 
            this.leftsToolStripMenuItem.Name = "leftsToolStripMenuItem";
            this.leftsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.leftsToolStripMenuItem.Text = "&Lefts";
            this.leftsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignL_Click);
            // 
            // centersToolStripMenuItem
            // 
            this.centersToolStripMenuItem.Name = "centersToolStripMenuItem";
            this.centersToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.centersToolStripMenuItem.Text = "&Centers";
            this.centersToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignC_Click);
            // 
            // rightsToolStripMenuItem
            // 
            this.rightsToolStripMenuItem.Name = "rightsToolStripMenuItem";
            this.rightsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.rightsToolStripMenuItem.Text = "&Rights";
            this.rightsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignR_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(116, 6);
            // 
            // topsToolStripMenuItem
            // 
            this.topsToolStripMenuItem.Name = "topsToolStripMenuItem";
            this.topsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.topsToolStripMenuItem.Text = "&Tops";
            this.topsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignT_Click);
            // 
            // middlesToolStripMenuItem
            // 
            this.middlesToolStripMenuItem.Name = "middlesToolStripMenuItem";
            this.middlesToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.middlesToolStripMenuItem.Text = "&Middles";
            this.middlesToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignM_Click);
            // 
            // bottomsToolStripMenuItem
            // 
            this.bottomsToolStripMenuItem.Name = "bottomsToolStripMenuItem";
            this.bottomsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.bottomsToolStripMenuItem.Text = "&Bottoms";
            this.bottomsToolStripMenuItem.Click += new System.EventHandler(this.menuFormatAlignB_Click);
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.widthToolStripMenuItem,
            this.heightToolStripMenuItem,
            this.bothToolStripMenuItem});
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.sizeToolStripMenuItem.Text = "&Size";
            // 
            // widthToolStripMenuItem
            // 
            this.widthToolStripMenuItem.Name = "widthToolStripMenuItem";
            this.widthToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.widthToolStripMenuItem.Text = "&Width";
            this.widthToolStripMenuItem.Click += new System.EventHandler(this.menuFormatSizeW_Click);
            // 
            // heightToolStripMenuItem
            // 
            this.heightToolStripMenuItem.Name = "heightToolStripMenuItem";
            this.heightToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.heightToolStripMenuItem.Text = "&Height";
            this.heightToolStripMenuItem.Click += new System.EventHandler(this.menuFormatSizeH_Click);
            // 
            // bothToolStripMenuItem
            // 
            this.bothToolStripMenuItem.Name = "bothToolStripMenuItem";
            this.bothToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.bothToolStripMenuItem.Text = "&Both";
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
            this.horizontalSpacingToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.horizontalSpacingToolStripMenuItem.Text = "&Horizontal Spacing";
            // 
            // makeEqualToolStripMenuItem
            // 
            this.makeEqualToolStripMenuItem.Name = "makeEqualToolStripMenuItem";
            this.makeEqualToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.makeEqualToolStripMenuItem.Text = "&Make Equal";
            this.makeEqualToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzE_Click);
            // 
            // increaseToolStripMenuItem
            // 
            this.increaseToolStripMenuItem.Name = "increaseToolStripMenuItem";
            this.increaseToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.increaseToolStripMenuItem.Text = "&Increase";
            this.increaseToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzI_Click);
            // 
            // decreaseToolStripMenuItem
            // 
            this.decreaseToolStripMenuItem.Name = "decreaseToolStripMenuItem";
            this.decreaseToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.decreaseToolStripMenuItem.Text = "&Decrease";
            this.decreaseToolStripMenuItem.Click += new System.EventHandler(this.menuFormatHorzD_Click);
            // 
            // zeroToolStripMenuItem
            // 
            this.zeroToolStripMenuItem.Name = "zeroToolStripMenuItem";
            this.zeroToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.zeroToolStripMenuItem.Text = "&Zero";
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
            this.verticalSpacingToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.verticalSpacingToolStripMenuItem.Text = "Vertical Spacing";
            // 
            // makeEqualToolStripMenuItem1
            // 
            this.makeEqualToolStripMenuItem1.Name = "makeEqualToolStripMenuItem1";
            this.makeEqualToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.makeEqualToolStripMenuItem1.Text = "&Make Equal";
            this.makeEqualToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertE_Click);
            // 
            // increaseToolStripMenuItem1
            // 
            this.increaseToolStripMenuItem1.Name = "increaseToolStripMenuItem1";
            this.increaseToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.increaseToolStripMenuItem1.Text = "&Increase";
            this.increaseToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertI_Click);
            // 
            // decreaseToolStripMenuItem1
            // 
            this.decreaseToolStripMenuItem1.Name = "decreaseToolStripMenuItem1";
            this.decreaseToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.decreaseToolStripMenuItem1.Text = "&Decrease";
            this.decreaseToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertD_Click);
            // 
            // zeroToolStripMenuItem1
            // 
            this.zeroToolStripMenuItem1.Name = "zeroToolStripMenuItem1";
            this.zeroToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.zeroToolStripMenuItem1.Text = "&Zero";
            this.zeroToolStripMenuItem1.Click += new System.EventHandler(this.menuFormatVertZ_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(171, 6);
            // 
            // paddingLeftToolStripMenuItem
            // 
            this.paddingLeftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem2,
            this.decreaseToolStripMenuItem2,
            this.zeroToolStripMenuItem2});
            this.paddingLeftToolStripMenuItem.Name = "paddingLeftToolStripMenuItem";
            this.paddingLeftToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.paddingLeftToolStripMenuItem.Text = "Padding &Left";
            // 
            // increaseToolStripMenuItem2
            // 
            this.increaseToolStripMenuItem2.Name = "increaseToolStripMenuItem2";
            this.increaseToolStripMenuItem2.Size = new System.Drawing.Size(121, 22);
            this.increaseToolStripMenuItem2.Text = "&Increase";
            this.increaseToolStripMenuItem2.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // decreaseToolStripMenuItem2
            // 
            this.decreaseToolStripMenuItem2.Name = "decreaseToolStripMenuItem2";
            this.decreaseToolStripMenuItem2.Size = new System.Drawing.Size(121, 22);
            this.decreaseToolStripMenuItem2.Text = "&Decrease";
            this.decreaseToolStripMenuItem2.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // zeroToolStripMenuItem2
            // 
            this.zeroToolStripMenuItem2.Name = "zeroToolStripMenuItem2";
            this.zeroToolStripMenuItem2.Size = new System.Drawing.Size(121, 22);
            this.zeroToolStripMenuItem2.Text = "&Zero";
            this.zeroToolStripMenuItem2.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // paddingRightToolStripMenuItem
            // 
            this.paddingRightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem3,
            this.decreaseToolStripMenuItem3,
            this.zeroToolStripMenuItem3});
            this.paddingRightToolStripMenuItem.Name = "paddingRightToolStripMenuItem";
            this.paddingRightToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.paddingRightToolStripMenuItem.Text = "Padding &Right";
            // 
            // increaseToolStripMenuItem3
            // 
            this.increaseToolStripMenuItem3.Name = "increaseToolStripMenuItem3";
            this.increaseToolStripMenuItem3.Size = new System.Drawing.Size(121, 22);
            this.increaseToolStripMenuItem3.Text = "&Increase";
            this.increaseToolStripMenuItem3.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // decreaseToolStripMenuItem3
            // 
            this.decreaseToolStripMenuItem3.Name = "decreaseToolStripMenuItem3";
            this.decreaseToolStripMenuItem3.Size = new System.Drawing.Size(121, 22);
            this.decreaseToolStripMenuItem3.Text = "&Decrease";
            this.decreaseToolStripMenuItem3.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // zeroToolStripMenuItem3
            // 
            this.zeroToolStripMenuItem3.Name = "zeroToolStripMenuItem3";
            this.zeroToolStripMenuItem3.Size = new System.Drawing.Size(121, 22);
            this.zeroToolStripMenuItem3.Text = "&Zero";
            this.zeroToolStripMenuItem3.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // paddintTopToolStripMenuItem
            // 
            this.paddintTopToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem4,
            this.decreaseToolStripMenuItem4,
            this.zeroToolStripMenuItem4});
            this.paddintTopToolStripMenuItem.Name = "paddintTopToolStripMenuItem";
            this.paddintTopToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.paddintTopToolStripMenuItem.Text = "Padding &Top";
            // 
            // increaseToolStripMenuItem4
            // 
            this.increaseToolStripMenuItem4.Name = "increaseToolStripMenuItem4";
            this.increaseToolStripMenuItem4.Size = new System.Drawing.Size(121, 22);
            this.increaseToolStripMenuItem4.Text = "&Increase";
            this.increaseToolStripMenuItem4.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // decreaseToolStripMenuItem4
            // 
            this.decreaseToolStripMenuItem4.Name = "decreaseToolStripMenuItem4";
            this.decreaseToolStripMenuItem4.Size = new System.Drawing.Size(121, 22);
            this.decreaseToolStripMenuItem4.Text = "&Decrease";
            this.decreaseToolStripMenuItem4.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // zeroToolStripMenuItem4
            // 
            this.zeroToolStripMenuItem4.Name = "zeroToolStripMenuItem4";
            this.zeroToolStripMenuItem4.Size = new System.Drawing.Size(121, 22);
            this.zeroToolStripMenuItem4.Text = "&Zero";
            this.zeroToolStripMenuItem4.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // paddingBottomToolStripMenuItem
            // 
            this.paddingBottomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem5,
            this.decreaseToolStripMenuItem5,
            this.zeroToolStripMenuItem5});
            this.paddingBottomToolStripMenuItem.Name = "paddingBottomToolStripMenuItem";
            this.paddingBottomToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.paddingBottomToolStripMenuItem.Text = "Padding &Bottom";
            // 
            // increaseToolStripMenuItem5
            // 
            this.increaseToolStripMenuItem5.Name = "increaseToolStripMenuItem5";
            this.increaseToolStripMenuItem5.Size = new System.Drawing.Size(121, 22);
            this.increaseToolStripMenuItem5.Text = "&Increase";
            this.increaseToolStripMenuItem5.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // decreaseToolStripMenuItem5
            // 
            this.decreaseToolStripMenuItem5.Name = "decreaseToolStripMenuItem5";
            this.decreaseToolStripMenuItem5.Size = new System.Drawing.Size(121, 22);
            this.decreaseToolStripMenuItem5.Text = "&Decrease";
            this.decreaseToolStripMenuItem5.Click += new System.EventHandler(this.menuFormatPadding_Click);
            // 
            // zeroToolStripMenuItem5
            // 
            this.zeroToolStripMenuItem5.Name = "zeroToolStripMenuItem5";
            this.zeroToolStripMenuItem5.Size = new System.Drawing.Size(121, 22);
            this.zeroToolStripMenuItem5.Text = "&Zero";
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
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            this.toolsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuTools_Popup);
            // 
            // validateRDLToolStripMenuItem
            // 
            this.validateRDLToolStripMenuItem.Name = "validateRDLToolStripMenuItem";
            this.validateRDLToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.validateRDLToolStripMenuItem.Text = "&Validate RDL";
            this.validateRDLToolStripMenuItem.Click += new System.EventHandler(this.menuToolsValidateSchema_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(176, 6);
            // 
            // startDesktopServerToolStripMenuItem
            // 
            this.startDesktopServerToolStripMenuItem.Name = "startDesktopServerToolStripMenuItem";
            this.startDesktopServerToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.startDesktopServerToolStripMenuItem.Text = "&Start Desktop Server";
            this.startDesktopServerToolStripMenuItem.Click += new System.EventHandler(this.menuToolsProcess_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(176, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.menuToolsOptions_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileToolStripMenuItem,
            this.closeAllToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "&Window";
            this.windowToolStripMenuItem.DropDownOpening += new System.EventHandler(this.menuWnd_Popup);
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.J)));
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.cascadeToolStripMenuItem.Text = "&Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.menuWndCascade_Click);
            // 
            // tileToolStripMenuItem
            // 
            this.tileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontalToolStripMenuItem,
            this.verticallyToolStripMenuItem});
            this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
            this.tileToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.tileToolStripMenuItem.Text = "&Tile";
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.K)));
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.horizontalToolStripMenuItem.Text = "&Horizontally";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.menuWndTileH_Click);
            // 
            // verticallyToolStripMenuItem
            // 
            this.verticallyToolStripMenuItem.Name = "verticallyToolStripMenuItem";
            this.verticallyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.L)));
            this.verticallyToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.verticallyToolStripMenuItem.Text = "&Vertically";
            this.verticallyToolStripMenuItem.Click += new System.EventHandler(this.menuWndTileV_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.W)));
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.closeAllToolStripMenuItem.Text = "Close &All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.menuWndCloseAll_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.supportToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.helpToolStripMenuItem1.Text = "&Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.menuHelpHelp_Click);
            // 
            // supportToolStripMenuItem
            // 
            this.supportToolStripMenuItem.Name = "supportToolStripMenuItem";
            this.supportToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.supportToolStripMenuItem.Text = "&Support";
            this.supportToolStripMenuItem.Click += new System.EventHandler(this.menuHelpSupport_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // RdlDesigner
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 470);
            this.Controls.Add(this.bTif);
            this.Controls.Add(this.bExcel);
            this.Controls.Add(this.bSelectTool);
            this.Controls.Add(this.bRtf);
            this.Controls.Add(this.bLAlign);
            this.Controls.Add(this.bRAlign);
            this.Controls.Add(this.bCAlign);
            this.Controls.Add(this.bCsv);
            this.Controls.Add(this.bMht);
            this.Controls.Add(this.bHtml);
            this.Controls.Add(this.bXml);
            this.Controls.Add(this.bPdf);
            this.Controls.Add(this.bUndo);
            this.Controls.Add(this.bNew);
            this.Controls.Add(this.bCut);
            this.Controls.Add(this.bCopy);
            this.Controls.Add(this.bPaste);
            this.Controls.Add(this.bOpen);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bPrint);
            this.Controls.Add(this.bMatrix);
            this.Controls.Add(this.bText);
            this.Controls.Add(this.bChart);
            this.Controls.Add(this.bList);
            this.Controls.Add(this.bSubreport);
            this.Controls.Add(this.bRectangle);
            this.Controls.Add(this.bImage);
            this.Controls.Add(this.bLine);
            this.Controls.Add(this.bTable);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RdlDesigner";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "fyiReporting Designer";
            this.Load += new System.EventHandler(this.RdlDesigner_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
	}
}
