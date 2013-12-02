namespace fyiReporting.RdlDesign
{
	partial class QueryParametersCtl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryParametersCtl));
			this.dgParms = new System.Windows.Forms.DataGridView();
			this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.dgtbName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgtbValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParms
			// 
			resources.ApplyResources(this.dgParms, "dgParms");
			this.dgParms.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgParms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgtbName,
            this.dgtbValue});
			this.dgParms.Name = "dgParms";
			// 
			// dgTableStyle
			// 
			this.dgTableStyle.DataGrid = null;
			resources.ApplyResources(this.dgTableStyle, "dgTableStyle");
			this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			// 
			// dgtbName
			// 
			this.dgtbName.DataPropertyName = "Name";
			resources.ApplyResources(this.dgtbName, "dgtbName");
			this.dgtbName.Name = "dgtbName";
			// 
			// dgtbValue
			// 
			this.dgtbValue.DataPropertyName = "Value";
			resources.ApplyResources(this.dgtbValue, "dgtbValue");
			this.dgtbValue.Name = "dgtbValue";
			// 
			// QueryParametersCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.dgParms);
			this.Name = "QueryParametersCtl";
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
				
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.DataGridView dgParms;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgtbName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgtbValue;
	}
}