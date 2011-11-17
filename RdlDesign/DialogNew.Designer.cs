using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    public partial class DialogNew : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.ListView listNewChoices;
private System.Windows.Forms.Button btnOK;
private System.Windows.Forms.Button btnCancel;
private System.ComponentModel.Container components = null;
private System.Windows.Forms.Panel panel1;

		private void InitializeComponent()
		{
			this.listNewChoices = new System.Windows.Forms.ListView();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listNewChoices
			// 
			this.listNewChoices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listNewChoices.HideSelection = false;
			this.listNewChoices.Location = new System.Drawing.Point(0, 0);
			this.listNewChoices.MultiSelect = false;
			this.listNewChoices.Name = "listNewChoices";
			this.listNewChoices.Size = new System.Drawing.Size(448, 366);
			this.listNewChoices.TabIndex = 0;
			this.listNewChoices.ItemActivate += new System.EventHandler(this.listNewChoices_ItemActivate);
			this.listNewChoices.SelectedIndexChanged += new System.EventHandler(this.listNewChoices_SelectedIndexChanged);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Enabled = false;
			this.btnOK.Location = new System.Drawing.Point(280, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(368, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 334);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(448, 32);
			this.panel1.TabIndex = 3;
			// 
			// DialogNew
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 366);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listNewChoices);
			this.Name = "DialogNew";
			this.Text = "New";
			this.panel1.ResumeLayout(false);
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
