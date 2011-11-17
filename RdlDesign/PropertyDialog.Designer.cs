using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class PropertyDialog : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DesignXmlDraw _Draw;
private System.Windows.Forms.Panel panel1;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bApply;
private System.Windows.Forms.TabControl tcProps;
private System.Windows.Forms.Button bDelete;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
            this.panel1 = new System.Windows.Forms.Panel();
            this.bDelete = new System.Windows.Forms.Button();
            this.bApply = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tcProps = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.CausesValidation = false;
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bApply);
            this.panel1.Controls.Add(this.bOK);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(458, 40);
            this.panel1.TabIndex = 1;
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(8, 8);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 3;
            this.bDelete.Text = "Delete";
            this.bDelete.Visible = false;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bApply
            // 
            this.bApply.Location = new System.Drawing.Point(376, 8);
            this.bApply.Name = "bApply";
            this.bApply.Size = new System.Drawing.Size(75, 23);
            this.bApply.TabIndex = 2;
            this.bApply.Text = "Apply";
            this.bApply.Click += new System.EventHandler(this.bApply_Click);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(216, 8);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 0;
            this.bOK.Text = "OK";
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.CausesValidation = false;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(296, 8);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            // 
            // tcProps
            // 
            this.tcProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcProps.Location = new System.Drawing.Point(0, 0);
            this.tcProps.Multiline = true;
            this.tcProps.Name = "tcProps";
            this.tcProps.SelectedIndex = 0;
            this.tcProps.Size = new System.Drawing.Size(458, 326);
            this.tcProps.TabIndex = 0;
            // 
            // PropertyDialog
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(458, 366);
            this.Controls.Add(this.tcProps);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertyDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PropertyDialog_Closing);
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
