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
MenuItem menuFSep1;
MenuItem menuFSep2;
MenuItem menuFSep3;
MenuItem menuFSep4;
MenuItem menuNew;
MenuItem menuOpen;
MenuItem menuClose;
MenuItem menuSave;
MenuItem menuSaveAs;
MenuItem menuExport;
MenuItem menuExportCsv;
MenuItem menuExportExcel;
MenuItem menuExportRtf;
MenuItem menuExportXml;
MenuItem menuExportPdf;
MenuItem menuExportTif;
MenuItem menuExportHtml;
MenuItem menuExportMHtml;
MenuItem menuPrint;
MenuItem menuRecentFile;
MenuItem menuExit;
MenuItem menuEdit;
MenuItem menuEditUndo;
MenuItem menuEditRedo;
MenuItem menuEditCut;
MenuItem menuEditCopy;
MenuItem menuEditPaste;
MenuItem menuEditDelete;
MenuItem menuEditSelectAll;
MenuItem menuEditFind;
MenuItem menuEditFindNext;
MenuItem menuEditReplace;
MenuItem menuEditGoto;
MenuItem menuEditFormatXml;
MenuItem menuView;
MenuItem menuViewDesigner;
MenuItem menuViewRDL;
MenuItem menuViewPreview;
MenuItem menuViewBrowser;
MenuItem menuViewProperties;
MenuItem menuData;
MenuItem menuDataSources;
MenuItem menuDataSets;
MenuItem menuEmbeddedImages;
MenuItem menuNewDataSourceRef;
MenuItem menuFormatAlign;
MenuItem menuFormatAlignL;
MenuItem menuFormatAlignC;
MenuItem menuFormatAlignR;
MenuItem menuFormatAlignT;
MenuItem menuFormatAlignM;
MenuItem menuFormatAlignB;
MenuItem menuFormatSize;
MenuItem menuFormatSizeW;
MenuItem menuFormatSizeH;
MenuItem menuFormatSizeB;
MenuItem menuFormatHorz;
MenuItem menuFormatHorzE;
MenuItem menuFormatHorzI;
MenuItem menuFormatHorzD;
MenuItem menuFormatHorzZ;
MenuItem menuFormatVert;
MenuItem menuFormatVertE;
MenuItem menuFormatVertI;
MenuItem menuFormatVertD;
MenuItem menuFormatVertZ;
MenuItem menuFormatPaddingLeft;
MenuItem menuFormatPaddingLeftI;
MenuItem menuFormatPaddingLeftD;
MenuItem menuFormatPaddingLeftZ;
MenuItem menuFormatPaddingRight;
MenuItem menuFormatPaddingRightI;
MenuItem menuFormatPaddingRightD;
MenuItem menuFormatPaddingRightZ;
MenuItem menuFormatPaddingTop;
MenuItem menuFormatPaddingTopI;
MenuItem menuFormatPaddingTopD;
MenuItem menuFormatPaddingTopZ;
MenuItem menuFormatPaddingBottom;
MenuItem menuFormatPaddingBottomI;
MenuItem menuFormatPaddingBottomD;
MenuItem menuFormatPaddingBottomZ;
MenuItem menuFormat;
MenuItem menuTools;
MenuItem menuToolsValidateSchema;
MenuItem menuToolsProcess;
MenuItem menuToolsOptions;
MenuItem menuCascade;
MenuItem menuTileH;
MenuItem menuTileV;
MenuItem menuTile;
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
MenuItem menuCloseAll;

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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RdlDesigner";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "fyiReporting Designer";
            this.ResumeLayout(false);

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
	}
}
