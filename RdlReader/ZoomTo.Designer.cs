using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Majorsilence.Reporting.RdlReader
{
    public partial class ZoomTo : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.Label label1;
private System.Windows.Forms.ComboBox cbMagnify;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private RdlViewer.RdlViewer _Viewer;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZoomTo));
            this.DoubleBuffered = true;
			this.label1 = new System.Windows.Forms.Label();
			this.cbMagnify = new System.Windows.Forms.ComboBox();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// cbMagnify
			// 
			resources.ApplyResources(this.cbMagnify, "cbMagnify");
			this.cbMagnify.Items.AddRange(new object[] {
            resources.GetString("cbMagnify.Items"),
            resources.GetString("cbMagnify.Items1"),
            resources.GetString("cbMagnify.Items2"),
            resources.GetString("cbMagnify.Items3"),
            resources.GetString("cbMagnify.Items4"),
            resources.GetString("cbMagnify.Items5"),
            resources.GetString("cbMagnify.Items6"),
            resources.GetString("cbMagnify.Items7"),
            resources.GetString("cbMagnify.Items8"),
            resources.GetString("cbMagnify.Items9"),
            resources.GetString("cbMagnify.Items10")});
			this.cbMagnify.Name = "cbMagnify";
			// 
			// bOK
			// 
			resources.ApplyResources(this.bOK, "bOK");
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Name = "bOK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			resources.ApplyResources(this.bCancel, "bCancel");
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Name = "bCancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// ZoomTo
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.cbMagnify);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ZoomTo";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
		#endregion

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
	}
}
