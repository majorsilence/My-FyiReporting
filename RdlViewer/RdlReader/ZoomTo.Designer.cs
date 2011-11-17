using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlReader
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
			this.label1 = new System.Windows.Forms.Label();
			this.cbMagnify = new System.Windows.Forms.ComboBox();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Magnification";
			// 
			// cbMagnify
			// 
			this.cbMagnify.Items.AddRange(new object[] {
														   "800%",
														   "400%",
														   "200%",
														   "150%",
														   "125%",
														   "100%",
														   "50%",
														   "25%",
														   "Fit Page",
														   "Actual Size",
														   "Fit Width"});
			this.cbMagnify.Location = new System.Drawing.Point(96, 16);
			this.cbMagnify.Name = "cbMagnify";
			this.cbMagnify.Size = new System.Drawing.Size(120, 21);
			this.cbMagnify.TabIndex = 2;
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(24, 64);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 1;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(136, 64);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 0;
			this.bCancel.Text = "Cancel";
			this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
			// 
			// ZoomTo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 102);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.cbMagnify);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ZoomTo";
			this.ShowInTaskbar = false;
			this.Text = "Zoom To";
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
