using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlMapFile
{
    public partial class DialogFindByKey : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DesignXmlDraw _Draw;
private Label label1;
private Button bOK;
private Button bCancel;
private ListBox lbKeyList;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lbKeyList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select keys for the polygons you wish to select";
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(182, 258);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(276, 258);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // lbKeyList
            // 
            this.lbKeyList.FormattingEnabled = true;
            this.lbKeyList.Location = new System.Drawing.Point(15, 38);
            this.lbKeyList.Name = "lbKeyList";
            this.lbKeyList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbKeyList.Size = new System.Drawing.Size(345, 212);
            this.lbKeyList.TabIndex = 4;
            // 
            // DialogFindByKey
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(372, 293);
            this.Controls.Add(this.lbKeyList);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogFindByKey";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find Polygons By Key";
            this.ResumeLayout(false);
            this.PerformLayout();

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
