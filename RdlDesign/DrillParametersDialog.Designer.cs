using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class DrillParametersDialog : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DataGridTextBoxColumn dgtbName;
private DataGridTextBoxColumn dgtbValue;
private DataGridTextBoxColumn dgtbOmit;
private System.Windows.Forms.DataGridTableStyle dgTableStyle;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.Button bFile;
private System.Windows.Forms.TextBox tbReportFile;
private System.Windows.Forms.DataGrid dgParms;
private System.Windows.Forms.Button bRefreshParms;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DrillParametersDialog));
			this.dgParms = new System.Windows.Forms.DataGrid();
			this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.label1 = new System.Windows.Forms.Label();
			this.tbReportFile = new System.Windows.Forms.TextBox();
			this.bFile = new System.Windows.Forms.Button();
			this.bRefreshParms = new System.Windows.Forms.Button();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParms
			// 
			this.dgParms.CaptionVisible = false;
			this.dgParms.DataMember = "";
			this.dgParms.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParms.Location = new System.Drawing.Point(8, 40);
			this.dgParms.Name = "dgParms";
			this.dgParms.Size = new System.Drawing.Size(384, 168);
			this.dgParms.TabIndex = 2;
			this.dgParms.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								this.dgTableStyle});
			// 
			// dgTableStyle
			// 
			this.dgTableStyle.AllowSorting = false;
			this.dgTableStyle.DataGrid = this.dgParms;
			this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgTableStyle.MappingName = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Report name";
			// 
			// tbReportFile
			// 
			this.tbReportFile.Location = new System.Drawing.Point(104, 8);
			this.tbReportFile.Name = "tbReportFile";
			this.tbReportFile.Size = new System.Drawing.Size(312, 20);
			this.tbReportFile.TabIndex = 4;
			this.tbReportFile.Text = "";
			// 
			// bFile
			// 
			this.bFile.Location = new System.Drawing.Point(424, 8);
			this.bFile.Name = "bFile";
			this.bFile.Size = new System.Drawing.Size(24, 23);
			this.bFile.TabIndex = 5;
			this.bFile.Text = "...";
			this.bFile.Click += new System.EventHandler(this.bFile_Click);
			// 
			// bRefreshParms
			// 
			this.bRefreshParms.Location = new System.Drawing.Point(400, 40);
			this.bRefreshParms.Name = "bRefreshParms";
			this.bRefreshParms.Size = new System.Drawing.Size(56, 23);
			this.bRefreshParms.TabIndex = 10;
			this.bRefreshParms.Text = "Refresh";
			this.bRefreshParms.Click += new System.EventHandler(this.bRefreshParms_Click);
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(288, 216);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 11;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.CausesValidation = false;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(376, 216);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 12;
			this.bCancel.Text = "Cancel";
			// 
			// DrillParametersDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(464, 248);
			this.ControlBox = false;
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bRefreshParms);
			this.Controls.Add(this.bFile);
			this.Controls.Add(this.tbReportFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dgParms);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DrillParametersDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Specify Drillthrough Report and Parameters";
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
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
