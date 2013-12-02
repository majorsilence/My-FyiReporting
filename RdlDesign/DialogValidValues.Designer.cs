using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class DialogValidValues : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DataGridTextBoxColumn dgtbLabel;
private DataGridTextBoxColumn dgtbValue;
private System.Windows.Forms.DataGridTableStyle dgTableStyle;
private System.Windows.Forms.DataGrid dgParms;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.Button bDelete;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogValidValues));
			this.dgParms = new System.Windows.Forms.DataGrid();
			this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.bDelete = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParms
			// 
			resources.ApplyResources(this.dgParms, "dgParms");
			this.dgParms.CaptionVisible = false;
			this.dgParms.DataMember = "";
			this.dgParms.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParms.Name = "dgParms";
			this.dgParms.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dgTableStyle});
			// 
			// dgTableStyle
			// 
			this.dgTableStyle.AllowSorting = false;
			this.dgTableStyle.DataGrid = this.dgParms;
			resources.ApplyResources(this.dgTableStyle, "dgTableStyle");
			this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			// 
			// bOK
			// 
			resources.ApplyResources(this.bOK, "bOK");
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Name = "bOK";
			// 
			// bCancel
			// 
			resources.ApplyResources(this.bCancel, "bCancel");
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Name = "bCancel";
			// 
			// bDelete
			// 
			resources.ApplyResources(this.bDelete, "bDelete");
			this.bDelete.Name = "bDelete";
			this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
			// 
			// DialogValidValues
			// 
			this.AcceptButton = this.bOK;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.bCancel;
			this.ControlBox = false;
			this.Controls.Add(this.bDelete);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.dgParms);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogValidValues";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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
