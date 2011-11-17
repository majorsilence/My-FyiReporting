using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlViewer
{
    public partial class DialogMessages : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.Button bOK;
private System.Windows.Forms.TextBox tbMessages;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.bOK = new System.Windows.Forms.Button();
			this.tbMessages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bOK.Location = new System.Drawing.Point(200, 208);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 0;
			this.bOK.Text = "OK";
			// 
			// tbMessages
			// 
			this.tbMessages.Location = new System.Drawing.Point(16, 16);
			this.tbMessages.Multiline = true;
			this.tbMessages.Name = "tbMessages";
			this.tbMessages.ReadOnly = true;
			this.tbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbMessages.Size = new System.Drawing.Size(448, 176);
			this.tbMessages.TabIndex = 9;
			this.tbMessages.Text = "";
			// 
			// DialogMessages
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bOK;
			this.ClientSize = new System.Drawing.Size(482, 240);
			this.Controls.Add(this.tbMessages);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogMessages";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Report Warnings";
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
