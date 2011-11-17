using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    public partial class DialogValidateRdl : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private RdlDesigner _RdlDesigner;
private System.Windows.Forms.Button bClose;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.Button bValidate;
private System.Windows.Forms.ListBox lbSchemaErrors;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.bClose = new System.Windows.Forms.Button();
			this.lbSchemaErrors = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.bValidate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// bClose
			// 
			this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bClose.CausesValidation = false;
			this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bClose.Location = new System.Drawing.Point(413, 234);
			this.bClose.Name = "bClose";
			this.bClose.TabIndex = 3;
			this.bClose.Text = "Close";
			this.bClose.Click += new System.EventHandler(this.bClose_Click);
			// 
			// lbSchemaErrors
			// 
			this.lbSchemaErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbSchemaErrors.HorizontalScrollbar = true;
			this.lbSchemaErrors.Location = new System.Drawing.Point(9, 54);
			this.lbSchemaErrors.Name = "lbSchemaErrors";
			this.lbSchemaErrors.Size = new System.Drawing.Size(484, 173);
			this.lbSchemaErrors.TabIndex = 2;
			this.lbSchemaErrors.DoubleClick += new System.EventHandler(this.lbSchemaErrors_DoubleClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(485, 38);
			this.label1.TabIndex = 0;
			this.label1.Text = "Press Validate button to do schema validation.  Double Click on a line to scroll " +
				"to the line of the error.   Note: schema validation does not reliably indicate w" +
				"hether or not report will run in this or other products that support RDL.";
			// 
			// bValidate
			// 
			this.bValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bValidate.Location = new System.Drawing.Point(328, 234);
			this.bValidate.Name = "bValidate";
			this.bValidate.TabIndex = 1;
			this.bValidate.Text = "Validate";
			this.bValidate.Click += new System.EventHandler(this.bValidate_Click);
			// 
			// DialogValidateRdl
			// 
			this.AcceptButton = this.bValidate;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bClose;
			this.ClientSize = new System.Drawing.Size(503, 261);
			this.Controls.Add(this.bValidate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbSchemaErrors);
			this.Controls.Add(this.bClose);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogValidateRdl";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Validate RDL Syntax";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DialogValidateRdl_Closing);
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
