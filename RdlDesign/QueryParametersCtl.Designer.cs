namespace Majorsilence.Reporting.RdlDesign
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
            this.dgtbName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgtbValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bValueExpr = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
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
            // bValueExpr
            // 
            resources.ApplyResources(this.bValueExpr, "bValueExpr");
            this.bValueExpr.Name = "bValueExpr";
            this.bValueExpr.Tag = "value";
            this.bValueExpr.Click += new System.EventHandler(this.bValueExpr_Click);
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // QueryParametersCtl
            // 
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.bValueExpr);
            this.Controls.Add(this.dgParms);
            this.Name = "QueryParametersCtl";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
				
		private System.Windows.Forms.DataGridView dgParms;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgtbName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgtbValue;
        private System.Windows.Forms.Button bValueExpr;
        private System.Windows.Forms.Button bDelete;
    }
}