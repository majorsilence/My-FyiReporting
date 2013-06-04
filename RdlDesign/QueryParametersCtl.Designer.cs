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
            this.dgParms = new System.Windows.Forms.DataGridView();
            this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dgtbName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgtbValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
            this.SuspendLayout();
            // 
            // dgParms
            // 
            this.dgParms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgParms.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgParms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgtbName,
            this.dgtbValue});
            this.dgParms.Location = new System.Drawing.Point(8, 8);
            this.dgParms.Name = "dgParms";
            this.dgParms.Size = new System.Drawing.Size(384, 168);
            this.dgParms.TabIndex = 2;
            // 
            // dgTableStyle
            // 
            this.dgTableStyle.DataGrid = null;
            this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // dgtbName
            // 
            this.dgtbName.DataPropertyName = "Name";
            this.dgtbName.HeaderText = "Parameter Name";
            this.dgtbName.Name = "dgtbName";
            // 
            // dgtbValue
            // 
            this.dgtbValue.DataPropertyName = "Value";
            this.dgtbValue.HeaderText = "Value";
            this.dgtbValue.Name = "dgtbValue";
            // 
            // QueryParametersCtl
            // 
            this.Controls.Add(this.dgParms);
            this.Name = "QueryParametersCtl";
            this.Size = new System.Drawing.Size(464, 304);
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