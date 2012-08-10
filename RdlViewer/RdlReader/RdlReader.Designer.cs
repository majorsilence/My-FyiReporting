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
MenuItem menuFSep1;
MenuItem menuFSep2;
MenuItem menuFSep3;
MenuItem menuOpen;
MenuItem menuClose;
MenuItem menuSaveAs;
MenuItem menuPrint;
MenuItem menuRecentFile;
MenuItem menuExit;
MenuItem menuFile;
MenuItem menuPLZoomTo;
MenuItem menuPLActualSize;
MenuItem menuPLFitPage;
MenuItem menuPLFitWidth;
MenuItem menuPLSinglePage;
MenuItem menuPLContinuous;
MenuItem menuPLFacing;
MenuItem menuPLContinuousFacing;
MenuItem menuPL;
MenuItem menuView;
MenuItem menuCascade;
MenuItem menuTileH;
MenuItem menuTileV;
MenuItem menuTile;
MenuItem menuCloseAll;
MenuItem menuWindow;
MenuItem menuFind;
MenuItem menuSelection;
MenuItem menuCopy;
MenuItem menuEdit;
MainMenu menuMain;

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdlReader));
            this.SuspendLayout();
            // 
            // RdlReader
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 470);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RdlReader";
            this.Text = "fyiReporting Reader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
