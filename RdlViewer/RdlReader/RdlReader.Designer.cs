using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlReader
{
	public partial class RdlReader : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.ComponentModel.Container components = null;
private MDIChild printChild=null;


		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdlReader));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectionToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.actualSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fitPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fitWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.pageLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.singlePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.continuousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.facingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.continuousFacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.horizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.MdiWindowListItem = this.windowToolStripMenuItem;
			this.menuStrip1.Name = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveAsToolStripMenuItem,
            this.printToolStripMenuItem,
            this.toolStripSeparator2,
            this.recentFilesToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			// 
			// openToolStripMenuItem
			// 
			resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// closeToolStripMenuItem
			// 
			resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.menuFileClose_Click);
			// 
			// toolStripSeparator1
			// 
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
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
			// toolStripSeparator2
			// 
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			// 
			// recentFilesToolStripMenuItem
			// 
			resources.ApplyResources(this.recentFilesToolStripMenuItem, "recentFilesToolStripMenuItem");
			this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
			// 
			// toolStripSeparator3
			// 
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			// 
			// exitToolStripMenuItem
			// 
			resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.menuFileExit_Click);
			// 
			// editToolStripMenuItem
			// 
			resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectionToolToolStripMenuItem,
            this.toolStripSeparator4,
            this.copyToolStripMenuItem,
            this.toolStripSeparator5,
            this.findToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			// 
			// selectionToolToolStripMenuItem
			// 
			resources.ApplyResources(this.selectionToolToolStripMenuItem, "selectionToolToolStripMenuItem");
			this.selectionToolToolStripMenuItem.Name = "selectionToolToolStripMenuItem";
			this.selectionToolToolStripMenuItem.Click += new System.EventHandler(this.menuSelection_Click);
			// 
			// toolStripSeparator4
			// 
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			// 
			// copyToolStripMenuItem
			// 
			resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// toolStripSeparator5
			// 
			resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			// 
			// findToolStripMenuItem
			// 
			resources.ApplyResources(this.findToolStripMenuItem, "findToolStripMenuItem");
			this.findToolStripMenuItem.Name = "findToolStripMenuItem";
			this.findToolStripMenuItem.Click += new System.EventHandler(this.menuFind_Click);
			// 
			// viewToolStripMenuItem
			// 
			resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToToolStripMenuItem,
            this.actualSizeToolStripMenuItem,
            this.fitPageToolStripMenuItem,
            this.fitWidthToolStripMenuItem,
            this.toolStripSeparator6,
            this.pageLayoutToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			// 
			// zoomToToolStripMenuItem
			// 
			resources.ApplyResources(this.zoomToToolStripMenuItem, "zoomToToolStripMenuItem");
			this.zoomToToolStripMenuItem.Name = "zoomToToolStripMenuItem";
			this.zoomToToolStripMenuItem.Click += new System.EventHandler(this.menuPLZoomTo_Click);
			// 
			// actualSizeToolStripMenuItem
			// 
			resources.ApplyResources(this.actualSizeToolStripMenuItem, "actualSizeToolStripMenuItem");
			this.actualSizeToolStripMenuItem.Name = "actualSizeToolStripMenuItem";
			this.actualSizeToolStripMenuItem.Click += new System.EventHandler(this.menuPLActualSize_Click);
			// 
			// fitPageToolStripMenuItem
			// 
			resources.ApplyResources(this.fitPageToolStripMenuItem, "fitPageToolStripMenuItem");
			this.fitPageToolStripMenuItem.Name = "fitPageToolStripMenuItem";
			this.fitPageToolStripMenuItem.Click += new System.EventHandler(this.menuPLFitPage_Click);
			// 
			// fitWidthToolStripMenuItem
			// 
			resources.ApplyResources(this.fitWidthToolStripMenuItem, "fitWidthToolStripMenuItem");
			this.fitWidthToolStripMenuItem.Name = "fitWidthToolStripMenuItem";
			this.fitWidthToolStripMenuItem.Click += new System.EventHandler(this.menuPLFitWidth_Click);
			// 
			// toolStripSeparator6
			// 
			resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			// 
			// pageLayoutToolStripMenuItem
			// 
			resources.ApplyResources(this.pageLayoutToolStripMenuItem, "pageLayoutToolStripMenuItem");
			this.pageLayoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singlePageToolStripMenuItem,
            this.continuousToolStripMenuItem,
            this.facingToolStripMenuItem,
            this.continuousFacingToolStripMenuItem});
			this.pageLayoutToolStripMenuItem.Name = "pageLayoutToolStripMenuItem";
			// 
			// singlePageToolStripMenuItem
			// 
			resources.ApplyResources(this.singlePageToolStripMenuItem, "singlePageToolStripMenuItem");
			this.singlePageToolStripMenuItem.Name = "singlePageToolStripMenuItem";
			this.singlePageToolStripMenuItem.Click += new System.EventHandler(this.menuPLSinglePage_Click);
			// 
			// continuousToolStripMenuItem
			// 
			resources.ApplyResources(this.continuousToolStripMenuItem, "continuousToolStripMenuItem");
			this.continuousToolStripMenuItem.Name = "continuousToolStripMenuItem";
			this.continuousToolStripMenuItem.Click += new System.EventHandler(this.menuPLContinuous_Click);
			// 
			// facingToolStripMenuItem
			// 
			resources.ApplyResources(this.facingToolStripMenuItem, "facingToolStripMenuItem");
			this.facingToolStripMenuItem.Name = "facingToolStripMenuItem";
			this.facingToolStripMenuItem.Click += new System.EventHandler(this.menuPLFacing_Click);
			// 
			// continuousFacingToolStripMenuItem
			// 
			resources.ApplyResources(this.continuousFacingToolStripMenuItem, "continuousFacingToolStripMenuItem");
			this.continuousFacingToolStripMenuItem.Name = "continuousFacingToolStripMenuItem";
			this.continuousFacingToolStripMenuItem.Click += new System.EventHandler(this.menuPLContinuousFacing_Click);
			// 
			// windowToolStripMenuItem
			// 
			resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
			this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileToolStripMenuItem,
            this.closeAllToolStripMenuItem});
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			// 
			// cascadeToolStripMenuItem
			// 
			resources.ApplyResources(this.cascadeToolStripMenuItem, "cascadeToolStripMenuItem");
			this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
			this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.menuWndCascade_Click);
			// 
			// tileToolStripMenuItem
			// 
			resources.ApplyResources(this.tileToolStripMenuItem, "tileToolStripMenuItem");
			this.tileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontallyToolStripMenuItem,
            this.verticallyToolStripMenuItem});
			this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
			// 
			// horizontallyToolStripMenuItem
			// 
			resources.ApplyResources(this.horizontallyToolStripMenuItem, "horizontallyToolStripMenuItem");
			this.horizontallyToolStripMenuItem.Name = "horizontallyToolStripMenuItem";
			this.horizontallyToolStripMenuItem.Click += new System.EventHandler(this.menuWndTileH_Click);
			// 
			// verticallyToolStripMenuItem
			// 
			resources.ApplyResources(this.verticallyToolStripMenuItem, "verticallyToolStripMenuItem");
			this.verticallyToolStripMenuItem.Name = "verticallyToolStripMenuItem";
			this.verticallyToolStripMenuItem.Click += new System.EventHandler(this.menuWndTileV_Click);
			// 
			// closeAllToolStripMenuItem
			// 
			resources.ApplyResources(this.closeAllToolStripMenuItem, "closeAllToolStripMenuItem");
			this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
			this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.menuWndCloseAll_Click);
			// 
			// helpToolStripMenuItem
			// 
			resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			// 
			// aboutToolStripMenuItem
			// 
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.menuHelpAbout_Click);
			// 
			// toolStrip1
			// 
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonSave,
            this.toolStripButtonPrint});
			this.toolStrip1.Name = "toolStrip1";
			// 
			// toolStripButtonOpen
			// 
			resources.ApplyResources(this.toolStripButtonOpen, "toolStripButtonOpen");
			this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpen.Image = global::RdlReader.Properties.Resources.document_open;
			this.toolStripButtonOpen.Name = "toolStripButtonOpen";
			this.toolStripButtonOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// toolStripButtonSave
			// 
			resources.ApplyResources(this.toolStripButtonSave, "toolStripButtonSave");
			this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSave.Image = global::RdlReader.Properties.Resources.document_save;
			this.toolStripButtonSave.Name = "toolStripButtonSave";
			this.toolStripButtonSave.Click += new System.EventHandler(this.menuFileSaveAs_Click);
			// 
			// toolStripButtonPrint
			// 
			resources.ApplyResources(this.toolStripButtonPrint, "toolStripButtonPrint");
			this.toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonPrint.Image = global::RdlReader.Properties.Resources.document_print;
			this.toolStripButtonPrint.Name = "toolStripButtonPrint";
			this.toolStripButtonPrint.Click += new System.EventHandler(this.menuFilePrint_Click);
			// 
			// RdlReader
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "RdlReader";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
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
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem recentFilesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem selectionToolToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem zoomToToolStripMenuItem;
        private ToolStripMenuItem actualSizeToolStripMenuItem;
        private ToolStripMenuItem fitPageToolStripMenuItem;
        private ToolStripMenuItem fitWidthToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem singlePageToolStripMenuItem;
        private ToolStripMenuItem continuousToolStripMenuItem;
        private ToolStripMenuItem facingToolStripMenuItem;
        private ToolStripMenuItem pageLayoutToolStripMenuItem;
        private ToolStripMenuItem continuousFacingToolStripMenuItem;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem cascadeToolStripMenuItem;
        private ToolStripMenuItem tileToolStripMenuItem;
        private ToolStripMenuItem horizontallyToolStripMenuItem;
        private ToolStripMenuItem verticallyToolStripMenuItem;
        private ToolStripMenuItem closeAllToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonPrint;
        private ToolStripButton toolStripButtonOpen;
        private ToolStripButton toolStripButtonSave;
	}
}
