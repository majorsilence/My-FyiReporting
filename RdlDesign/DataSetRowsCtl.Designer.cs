namespace fyiReporting.RdlDesign
{
	partial class DataSetRowsCtl
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
            this.dgRows = new System.Windows.Forms.DataGrid();
            this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.bDelete = new System.Windows.Forms.Button();
            this.bUp = new System.Windows.Forms.Button();
            this.bDown = new System.Windows.Forms.Button();
            this.chkRowsFile = new System.Windows.Forms.CheckBox();
            this.tbRowsFile = new System.Windows.Forms.TextBox();
            this.bRowsFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bLoad = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgRows)).BeginInit();
            this.SuspendLayout();
            // 
            // dgRows
            // 
            this.dgRows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgRows.CaptionVisible = false;
            this.dgRows.DataMember = "";
            this.dgRows.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgRows.Location = new System.Drawing.Point(8, 48);
            this.dgRows.Name = "dgRows";
            this.dgRows.Size = new System.Drawing.Size(416, 221);
            this.dgRows.TabIndex = 2;
            this.dgRows.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dgTableStyle});
            // 
            // dgTableStyle
            // 
            this.dgTableStyle.AllowSorting = false;
            this.dgTableStyle.DataGrid = this.dgRows;
            this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // bDelete
            // 
            this.bDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bDelete.Location = new System.Drawing.Point(430, 48);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(72, 23);
            this.bDelete.TabIndex = 1;
            this.bDelete.Text = "Delete";
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bUp
            // 
            this.bUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bUp.Location = new System.Drawing.Point(430, 77);
            this.bUp.Name = "bUp";
            this.bUp.Size = new System.Drawing.Size(72, 23);
            this.bUp.TabIndex = 3;
            this.bUp.Text = "Up";
            this.bUp.Click += new System.EventHandler(this.bUp_Click);
            // 
            // bDown
            // 
            this.bDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bDown.Location = new System.Drawing.Point(430, 106);
            this.bDown.Name = "bDown";
            this.bDown.Size = new System.Drawing.Size(72, 23);
            this.bDown.TabIndex = 4;
            this.bDown.Text = "Down";
            this.bDown.Click += new System.EventHandler(this.bDown_Click);
            // 
            // chkRowsFile
            // 
            this.chkRowsFile.Location = new System.Drawing.Point(8, 10);
            this.chkRowsFile.Name = "chkRowsFile";
            this.chkRowsFile.Size = new System.Drawing.Size(136, 20);
            this.chkRowsFile.TabIndex = 5;
            this.chkRowsFile.Text = "Use XML file for data";
            this.chkRowsFile.CheckedChanged += new System.EventHandler(this.chkRowsFile_CheckedChanged);
            // 
            // tbRowsFile
            // 
            this.tbRowsFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRowsFile.Location = new System.Drawing.Point(144, 10);
            this.tbRowsFile.Name = "tbRowsFile";
            this.tbRowsFile.Size = new System.Drawing.Size(240, 20);
            this.tbRowsFile.TabIndex = 6;
            // 
            // bRowsFile
            // 
            this.bRowsFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRowsFile.Location = new System.Drawing.Point(400, 9);
            this.bRowsFile.Name = "bRowsFile";
            this.bRowsFile.Size = new System.Drawing.Size(24, 23);
            this.bRowsFile.TabIndex = 7;
            this.bRowsFile.Text = "...";
            this.bRowsFile.Click += new System.EventHandler(this.bRowsFile_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(3, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(421, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Warning: this panel supports an extension to the RDL specification.  This informa" +
    "tion will be ignored in RDL processors other than in fyiReporting.";
            // 
            // bLoad
            // 
            this.bLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bLoad.Location = new System.Drawing.Point(430, 212);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(72, 36);
            this.bLoad.TabIndex = 9;
            this.bLoad.Text = "Load From SQL";
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bClear
            // 
            this.bClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClear.Location = new System.Drawing.Point(430, 135);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(72, 23);
            this.bClear.TabIndex = 10;
            this.bClear.Text = "Clear";
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // DataSetRowsCtl
            // 
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.bLoad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bRowsFile);
            this.Controls.Add(this.tbRowsFile);
            this.Controls.Add(this.chkRowsFile);
            this.Controls.Add(this.bDown);
            this.Controls.Add(this.bUp);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.dgRows);
            this.Name = "DataSetRowsCtl";
            this.Size = new System.Drawing.Size(505, 304);
            this.VisibleChanged += new System.EventHandler(this.DataSetRowsCtl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgRows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		private System.Windows.Forms.Button bDelete;
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.Button bUp;
		private System.Windows.Forms.Button bDown;
		private System.Windows.Forms.CheckBox chkRowsFile;
		private System.Windows.Forms.Button bRowsFile;
		private System.Windows.Forms.DataGrid dgRows;
		private System.Windows.Forms.TextBox tbRowsFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bLoad;
		private System.Windows.Forms.Button bClear;
	}
}