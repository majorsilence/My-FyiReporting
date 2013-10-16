namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for StyleCtl.
    /// </summary>
    partial class DataSetsCtl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbTimeout = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bEditSQL = new System.Windows.Forms.Button();
            this.tbSQL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDataSource = new System.Windows.Forms.ComboBox();
            this.lDataSource = new System.Windows.Forms.Label();
            this.tbDSName = new System.Windows.Forms.TextBox();
            this.lDataSetName = new System.Windows.Forms.Label();
            this.bDeleteField = new System.Windows.Forms.Button();
            this.dgFields = new System.Windows.Forms.DataGridView();
            this.dgtbName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgtbQueryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgtbValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgtbTypeName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgFields)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = null;
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tbTimeout);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.bRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.bEditSQL);
            this.splitContainer1.Panel1.Controls.Add(this.tbSQL);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cbDataSource);
            this.splitContainer1.Panel1.Controls.Add(this.lDataSource);
            this.splitContainer1.Panel1.Controls.Add(this.tbDSName);
            this.splitContainer1.Panel1.Controls.Add(this.lDataSetName);
            this.splitContainer1.Panel1MinSize = 120;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bDeleteField);
            this.splitContainer1.Panel2.Controls.Add(this.dgFields);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(450, 300);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.TabIndex = 28;
            // 
            // tbTimeout
            // 
            this.tbTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTimeout.Location = new System.Drawing.Point(291, 24);
            this.tbTimeout.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(88, 20);
            this.tbTimeout.TabIndex = 30;
            this.tbTimeout.ThousandsSeparator = true;
            this.tbTimeout.Click += new System.EventHandler(this.tbTimeout_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(219, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 37;
            this.label3.Text = "Timeout";
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(385, 77);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(62, 34);
            this.bRefresh.TabIndex = 33;
            this.bRefresh.Text = "Refresh Fields";
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bEditSQL
            // 
            this.bEditSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bEditSQL.Location = new System.Drawing.Point(385, 48);
            this.bEditSQL.Name = "bEditSQL";
            this.bEditSQL.Size = new System.Drawing.Size(62, 23);
            this.bEditSQL.TabIndex = 32;
            this.bEditSQL.Text = "SQL...";
            this.bEditSQL.Click += new System.EventHandler(this.bEditSQL_Click);
            // 
            // tbSQL
            // 
            this.tbSQL.AcceptsReturn = true;
            this.tbSQL.AcceptsTab = true;
            this.tbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSQL.Location = new System.Drawing.Point(3, 48);
            this.tbSQL.Multiline = true;
            this.tbSQL.Name = "tbSQL";
            this.tbSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSQL.Size = new System.Drawing.Size(376, 69);
            this.tbSQL.TabIndex = 31;
            this.tbSQL.Text = "textBox1";
            this.tbSQL.TextChanged += new System.EventHandler(this.tbSQL_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 36;
            this.label1.Text = "SQL Select";
            // 
            // cbDataSource
            // 
            this.cbDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSource.Location = new System.Drawing.Point(291, 0);
            this.cbDataSource.Name = "cbDataSource";
            this.cbDataSource.Size = new System.Drawing.Size(156, 21);
            this.cbDataSource.TabIndex = 29;
            this.cbDataSource.SelectedIndexChanged += new System.EventHandler(this.cbDataSource_SelectedIndexChanged);
            // 
            // lDataSource
            // 
            this.lDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lDataSource.Location = new System.Drawing.Point(219, 0);
            this.lDataSource.Name = "lDataSource";
            this.lDataSource.Size = new System.Drawing.Size(72, 23);
            this.lDataSource.TabIndex = 35;
            this.lDataSource.Text = "Data Source";
            this.lDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDSName
            // 
            this.tbDSName.Location = new System.Drawing.Point(59, 0);
            this.tbDSName.Name = "tbDSName";
            this.tbDSName.Size = new System.Drawing.Size(144, 20);
            this.tbDSName.TabIndex = 28;
            this.tbDSName.TextChanged += new System.EventHandler(this.tbDSName_TextChanged);
            // 
            // lDataSetName
            // 
            this.lDataSetName.Location = new System.Drawing.Point(3, 0);
            this.lDataSetName.Name = "lDataSetName";
            this.lDataSetName.Size = new System.Drawing.Size(48, 16);
            this.lDataSetName.TabIndex = 34;
            this.lDataSetName.Text = "Name";
            this.lDataSetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bDeleteField
            // 
            this.bDeleteField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bDeleteField.Location = new System.Drawing.Point(385, 16);
            this.bDeleteField.Name = "bDeleteField";
            this.bDeleteField.Size = new System.Drawing.Size(62, 23);
            this.bDeleteField.TabIndex = 27;
            this.bDeleteField.Text = "Delete";
            this.bDeleteField.Click += new System.EventHandler(this.bDeleteField_Click);
            // 
            // dgFields
            // 
            this.dgFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgFields.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgtbName,
            this.dgtbQueryName,
            this.dgtbValue,
            this.dgtbTypeName});
            this.dgFields.Location = new System.Drawing.Point(3, 16);
            this.dgFields.Name = "dgFields";
            this.dgFields.Size = new System.Drawing.Size(376, 157);
            this.dgFields.TabIndex = 26;
            // 
            // dgtbName
            // 
            this.dgtbName.DataPropertyName = "Name";
            this.dgtbName.HeaderText = "Name";
            this.dgtbName.Name = "dgtbName";
            // 
            // dgtbQueryName
            // 
            this.dgtbQueryName.DataPropertyName = "QueryName";
            this.dgtbQueryName.HeaderText = "Query Column Name";
            this.dgtbQueryName.Name = "dgtbQueryName";
            // 
            // dgtbValue
            // 
            this.dgtbValue.DataPropertyName = "Value";
            this.dgtbValue.HeaderText = "Value";
            this.dgtbValue.Name = "dgtbValue";
            // 
            // dgtbTypeName
            // 
            this.dgtbTypeName.DataPropertyName = "TypeName";
            this.dgtbTypeName.HeaderText = "TypeName";
            this.dgtbTypeName.Items.AddRange(new object[] {
            "System.String",
            "System.Int16",
            "System.Int32",
            "System.Int64",
            "System.UInt16",
            "System.UInt32",
            "System.UInt64",
            "System.Single",
            "System.Double",
            "System.Decimal",
            "System.DateTime",
            "System.Char",
            "System.Boolean",
            "System.Byte"});
            this.dgtbTypeName.Name = "dgtbTypeName";
            this.dgtbTypeName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgtbTypeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "Fields";
            // 
            // DataSetsCtl
            // 
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "DataSetsCtl";
            this.Size = new System.Drawing.Size(450, 300);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgFields)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown tbTimeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bEditSQL;
        private System.Windows.Forms.TextBox tbSQL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDataSource;
        private System.Windows.Forms.Label lDataSource;
        private System.Windows.Forms.TextBox tbDSName;
        private System.Windows.Forms.Label lDataSetName;
        private System.Windows.Forms.Button bDeleteField;
        private System.Windows.Forms.DataGridView dgFields;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgtbName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgtbQueryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgtbValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgtbTypeName;
    }
}