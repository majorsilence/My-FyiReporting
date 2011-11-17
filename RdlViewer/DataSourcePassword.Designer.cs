using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlViewer
{
    public partial class DataSourcePassword : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.Label label1;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.TextBox tbPassPhrase;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.tbPassPhrase = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please enter the pass phrase for shared data sources";
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(112, 80);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 2;
			this.bOK.Text = "OK";
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(208, 80);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 3;
			this.bCancel.Text = "Cancel";
			// 
			// tbPassPhrase
			// 
			this.tbPassPhrase.Location = new System.Drawing.Point(16, 40);
			this.tbPassPhrase.Name = "tbPassPhrase";
			this.tbPassPhrase.PasswordChar = '*';
			this.tbPassPhrase.Size = new System.Drawing.Size(272, 20);
			this.tbPassPhrase.TabIndex = 1;
			this.tbPassPhrase.Text = "";
			// 
			// DataSourcePassword
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(306, 112);
			this.Controls.Add(this.tbPassPhrase);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DataSourcePassword";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Shared Data Sources";
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
