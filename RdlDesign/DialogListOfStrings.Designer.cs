using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    public partial class DialogListOfStrings : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.TextBox tbStrings;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.bOK = new System.Windows.Forms.Button();
			this.tbStrings = new System.Windows.Forms.TextBox();
			this.bCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(96, 192);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 0;
			this.bOK.Text = "OK";
			// 
			// tbStrings
			// 
			this.tbStrings.Location = new System.Drawing.Point(8, 40);
			this.tbStrings.Multiline = true;
			this.tbStrings.Name = "tbStrings";
			this.tbStrings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbStrings.Size = new System.Drawing.Size(256, 144);
			this.tbStrings.TabIndex = 9;
			this.tbStrings.Text = "";
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(192, 192);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 10;
			this.bCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(240, 23);
			this.label1.TabIndex = 11;
			this.label1.Text = "Enter separate values on multiple lines below";
			// 
			// DialogListOfStrings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(282, 224);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.tbStrings);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogListOfStrings";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
