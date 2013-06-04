namespace fyiReporting.RdlDesign
{
    partial class ReportParameterCtl
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
            this.lbParameters = new System.Windows.Forms.ListBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.ckbParmMultiValue = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bValidValues = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbValidDisplayField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbValidFields = new System.Windows.Forms.ComboBox();
            this.cbValidDataSets = new System.Windows.Forms.ComboBox();
            this.rbValues = new System.Windows.Forms.RadioButton();
            this.rbDataSet = new System.Windows.Forms.RadioButton();
            this.tbParmValidValues = new System.Windows.Forms.TextBox();
            this.ckbParmAllowBlank = new System.Windows.Forms.CheckBox();
            this.ckbParmAllowNull = new System.Windows.Forms.CheckBox();
            this.tbParmPrompt = new System.Windows.Forms.TextBox();
            this.lParmPrompt = new System.Windows.Forms.Label();
            this.cbParmType = new System.Windows.Forms.ComboBox();
            this.lParmType = new System.Windows.Forms.Label();
            this.tbParmName = new System.Windows.Forms.TextBox();
            this.lParmName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbParmDefaultValue = new System.Windows.Forms.TextBox();
            this.bDefaultValues = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDefaultValueField = new System.Windows.Forms.ComboBox();
            this.cbDefaultDataSets = new System.Windows.Forms.ComboBox();
            this.rbDefaultValues = new System.Windows.Forms.RadioButton();
            this.rbDefaultDataSetName = new System.Windows.Forms.RadioButton();
            this.bParmDown = new System.Windows.Forms.Button();
            this.bParmUp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbParameters
            // 
            this.lbParameters.Location = new System.Drawing.Point(3, 3);
            this.lbParameters.Name = "lbParameters";
            this.lbParameters.Size = new System.Drawing.Size(126, 264);
            this.lbParameters.TabIndex = 26;
            this.lbParameters.SelectedIndexChanged += new System.EventHandler(this.lbParameters_SelectedIndexChanged);
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(3, 276);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(60, 23);
            this.bAdd.TabIndex = 27;
            this.bAdd.Text = "Add";
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bRemove
            // 
            this.bRemove.Location = new System.Drawing.Point(69, 276);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(60, 23);
            this.bRemove.TabIndex = 28;
            this.bRemove.Text = "Remove";
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // ckbParmMultiValue
            // 
            this.ckbParmMultiValue.Location = new System.Drawing.Point(406, 54);
            this.ckbParmMultiValue.Name = "ckbParmMultiValue";
            this.ckbParmMultiValue.Size = new System.Drawing.Size(79, 24);
            this.ckbParmMultiValue.TabIndex = 46;
            this.ckbParmMultiValue.Text = "MultiValue";
            this.ckbParmMultiValue.CheckedChanged += new System.EventHandler(this.ckbParmMultiValue_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bValidValues);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbValidDisplayField);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbValidFields);
            this.groupBox1.Controls.Add(this.cbValidDataSets);
            this.groupBox1.Controls.Add(this.rbValues);
            this.groupBox1.Controls.Add(this.rbDataSet);
            this.groupBox1.Controls.Add(this.tbParmValidValues);
            this.groupBox1.Location = new System.Drawing.Point(174, 178);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 121);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Valid Values";
            // 
            // bValidValues
            // 
            this.bValidValues.Location = new System.Drawing.Point(395, 16);
            this.bValidValues.Name = "bValidValues";
            this.bValidValues.Size = new System.Drawing.Size(30, 23);
            this.bValidValues.TabIndex = 2;
            this.bValidValues.Text = "...";
            this.bValidValues.Click += new System.EventHandler(this.bValidValues_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(223, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "Display Field";
            // 
            // cbValidDisplayField
            // 
            this.cbValidDisplayField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidDisplayField.Location = new System.Drawing.Point(301, 83);
            this.cbValidDisplayField.Name = "cbValidDisplayField";
            this.cbValidDisplayField.Size = new System.Drawing.Size(124, 21);
            this.cbValidDisplayField.TabIndex = 6;
            this.cbValidDisplayField.SelectedIndexChanged += new System.EventHandler(this.cbValidDisplayField_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(226, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 18);
            this.label1.TabIndex = 35;
            this.label1.Text = "Value Field";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbValidFields
            // 
            this.cbValidFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidFields.Location = new System.Drawing.Point(301, 46);
            this.cbValidFields.Name = "cbValidFields";
            this.cbValidFields.Size = new System.Drawing.Size(124, 21);
            this.cbValidFields.TabIndex = 5;
            this.cbValidFields.SelectedIndexChanged += new System.EventHandler(this.cbValidFields_SelectedIndexChanged);
            // 
            // cbValidDataSets
            // 
            this.cbValidDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidDataSets.Location = new System.Drawing.Point(108, 46);
            this.cbValidDataSets.Name = "cbValidDataSets";
            this.cbValidDataSets.Size = new System.Drawing.Size(112, 21);
            this.cbValidDataSets.TabIndex = 4;
            this.cbValidDataSets.SelectedIndexChanged += new System.EventHandler(this.cbValidDataSets_SelectedIndexChanged);
            // 
            // rbValues
            // 
            this.rbValues.Location = new System.Drawing.Point(6, 19);
            this.rbValues.Name = "rbValues";
            this.rbValues.Size = new System.Drawing.Size(64, 20);
            this.rbValues.TabIndex = 0;
            this.rbValues.Text = "Values";
            this.rbValues.CheckedChanged += new System.EventHandler(this.rbValues_CheckedChanged);
            // 
            // rbDataSet
            // 
            this.rbDataSet.Location = new System.Drawing.Point(6, 46);
            this.rbDataSet.Name = "rbDataSet";
            this.rbDataSet.Size = new System.Drawing.Size(100, 21);
            this.rbDataSet.TabIndex = 3;
            this.rbDataSet.Text = "Data Set Name";
            this.rbDataSet.CheckedChanged += new System.EventHandler(this.rbDataSet_CheckedChanged);
            // 
            // tbParmValidValues
            // 
            this.tbParmValidValues.Location = new System.Drawing.Point(76, 19);
            this.tbParmValidValues.Name = "tbParmValidValues";
            this.tbParmValidValues.ReadOnly = true;
            this.tbParmValidValues.Size = new System.Drawing.Size(313, 20);
            this.tbParmValidValues.TabIndex = 1;
            // 
            // ckbParmAllowBlank
            // 
            this.ckbParmAllowBlank.Location = new System.Drawing.Point(252, 54);
            this.ckbParmAllowBlank.Name = "ckbParmAllowBlank";
            this.ckbParmAllowBlank.Size = new System.Drawing.Size(148, 24);
            this.ckbParmAllowBlank.TabIndex = 40;
            this.ckbParmAllowBlank.Text = "Allow blank (strings only)";
            this.ckbParmAllowBlank.CheckedChanged += new System.EventHandler(this.ckbParmAllowBlank_CheckedChanged);
            // 
            // ckbParmAllowNull
            // 
            this.ckbParmAllowNull.Location = new System.Drawing.Point(174, 54);
            this.ckbParmAllowNull.Name = "ckbParmAllowNull";
            this.ckbParmAllowNull.Size = new System.Drawing.Size(72, 24);
            this.ckbParmAllowNull.TabIndex = 39;
            this.ckbParmAllowNull.Text = "Allow null";
            this.ckbParmAllowNull.CheckedChanged += new System.EventHandler(this.ckbParmAllowNull_CheckedChanged);
            // 
            // tbParmPrompt
            // 
            this.tbParmPrompt.Location = new System.Drawing.Point(225, 28);
            this.tbParmPrompt.Name = "tbParmPrompt";
            this.tbParmPrompt.Size = new System.Drawing.Size(260, 20);
            this.tbParmPrompt.TabIndex = 38;
            this.tbParmPrompt.TextChanged += new System.EventHandler(this.tbParmPrompt_TextChanged);
            // 
            // lParmPrompt
            // 
            this.lParmPrompt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lParmPrompt.Location = new System.Drawing.Point(177, 29);
            this.lParmPrompt.Name = "lParmPrompt";
            this.lParmPrompt.Size = new System.Drawing.Size(48, 19);
            this.lParmPrompt.TabIndex = 45;
            this.lParmPrompt.Text = "Prompt";
            this.lParmPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbParmType
            // 
            this.cbParmType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParmType.Items.AddRange(new object[] {
            "Boolean",
            "DateTime",
            "Integer",
            "Float",
            "String"});
            this.cbParmType.Location = new System.Drawing.Point(398, 1);
            this.cbParmType.Name = "cbParmType";
            this.cbParmType.Size = new System.Drawing.Size(87, 21);
            this.cbParmType.TabIndex = 37;
            this.cbParmType.SelectedIndexChanged += new System.EventHandler(this.cbParmType_SelectedIndexChanged);
            // 
            // lParmType
            // 
            this.lParmType.Location = new System.Drawing.Point(327, 1);
            this.lParmType.Name = "lParmType";
            this.lParmType.Size = new System.Drawing.Size(56, 21);
            this.lParmType.TabIndex = 44;
            this.lParmType.Text = "Datatype";
            this.lParmType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbParmName
            // 
            this.tbParmName.Location = new System.Drawing.Point(225, 2);
            this.tbParmName.Name = "tbParmName";
            this.tbParmName.Size = new System.Drawing.Size(96, 20);
            this.tbParmName.TabIndex = 36;
            this.tbParmName.TextChanged += new System.EventHandler(this.tbParmName_TextChanged);
            // 
            // lParmName
            // 
            this.lParmName.Location = new System.Drawing.Point(174, 3);
            this.lParmName.Name = "lParmName";
            this.lParmName.Size = new System.Drawing.Size(45, 19);
            this.lParmName.TabIndex = 43;
            this.lParmName.Text = "Name";
            this.lParmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbParmDefaultValue);
            this.groupBox2.Controls.Add(this.bDefaultValues);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbDefaultValueField);
            this.groupBox2.Controls.Add(this.cbDefaultDataSets);
            this.groupBox2.Controls.Add(this.rbDefaultValues);
            this.groupBox2.Controls.Add(this.rbDefaultDataSetName);
            this.groupBox2.Location = new System.Drawing.Point(174, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(431, 88);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Default Values";
            // 
            // tbParmDefaultValue
            // 
            this.tbParmDefaultValue.Location = new System.Drawing.Point(76, 19);
            this.tbParmDefaultValue.Name = "tbParmDefaultValue";
            this.tbParmDefaultValue.ReadOnly = true;
            this.tbParmDefaultValue.Size = new System.Drawing.Size(313, 20);
            this.tbParmDefaultValue.TabIndex = 1;
            // 
            // bDefaultValues
            // 
            this.bDefaultValues.Location = new System.Drawing.Point(395, 18);
            this.bDefaultValues.Name = "bDefaultValues";
            this.bDefaultValues.Size = new System.Drawing.Size(30, 23);
            this.bDefaultValues.TabIndex = 2;
            this.bDefaultValues.Text = "...";
            this.bDefaultValues.Click += new System.EventHandler(this.bDefaultValues_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(226, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 21);
            this.label4.TabIndex = 35;
            this.label4.Text = "Value Field";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbDefaultValueField
            // 
            this.cbDefaultValueField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultValueField.Location = new System.Drawing.Point(301, 46);
            this.cbDefaultValueField.Name = "cbDefaultValueField";
            this.cbDefaultValueField.Size = new System.Drawing.Size(124, 21);
            this.cbDefaultValueField.TabIndex = 5;
            this.cbDefaultValueField.SelectedIndexChanged += new System.EventHandler(this.cbDefaultValueField_SelectedIndexChanged);
            // 
            // cbDefaultDataSets
            // 
            this.cbDefaultDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultDataSets.Location = new System.Drawing.Point(108, 46);
            this.cbDefaultDataSets.Name = "cbDefaultDataSets";
            this.cbDefaultDataSets.Size = new System.Drawing.Size(112, 21);
            this.cbDefaultDataSets.TabIndex = 4;
            this.cbDefaultDataSets.SelectedIndexChanged += new System.EventHandler(this.cbDefaultDataSets_SelectedIndexChanged);
            // 
            // rbDefaultValues
            // 
            this.rbDefaultValues.Location = new System.Drawing.Point(6, 19);
            this.rbDefaultValues.Name = "rbDefaultValues";
            this.rbDefaultValues.Size = new System.Drawing.Size(64, 20);
            this.rbDefaultValues.TabIndex = 0;
            this.rbDefaultValues.Text = "Values";
            this.rbDefaultValues.CheckedChanged += new System.EventHandler(this.rbDefaultValues_CheckedChanged);
            // 
            // rbDefaultDataSetName
            // 
            this.rbDefaultDataSetName.Location = new System.Drawing.Point(6, 46);
            this.rbDefaultDataSetName.Name = "rbDefaultDataSetName";
            this.rbDefaultDataSetName.Size = new System.Drawing.Size(100, 21);
            this.rbDefaultDataSetName.TabIndex = 3;
            this.rbDefaultDataSetName.Text = "DataSet Name";
            this.rbDefaultDataSetName.CheckedChanged += new System.EventHandler(this.rbDefaultDataSetName_CheckedChanged);
            // 
            // bParmDown
            // 
            this.bParmDown.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.bParmDown.Location = new System.Drawing.Point(135, 43);
            this.bParmDown.Name = "bParmDown";
            this.bParmDown.Size = new System.Drawing.Size(24, 24);
            this.bParmDown.TabIndex = 48;
            this.bParmDown.Text = "";
            this.bParmDown.Click += new System.EventHandler(this.bParmDown_Click);
            // 
            // bParmUp
            // 
            this.bParmUp.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.bParmUp.Location = new System.Drawing.Point(135, 3);
            this.bParmUp.Name = "bParmUp";
            this.bParmUp.Size = new System.Drawing.Size(24, 24);
            this.bParmUp.TabIndex = 47;
            this.bParmUp.Text = "";
            this.bParmUp.Click += new System.EventHandler(this.bParmUp_Click);
            // 
            // ReportParameterCtl
            // 
            this.Controls.Add(this.bParmDown);
            this.Controls.Add(this.bParmUp);
            this.Controls.Add(this.ckbParmMultiValue);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ckbParmAllowBlank);
            this.Controls.Add(this.ckbParmAllowNull);
            this.Controls.Add(this.tbParmPrompt);
            this.Controls.Add(this.lParmPrompt);
            this.Controls.Add(this.cbParmType);
            this.Controls.Add(this.lParmType);
            this.Controls.Add(this.tbParmName);
            this.Controls.Add(this.lParmName);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lbParameters);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.bRemove);
            this.Name = "ReportParameterCtl";
            this.Size = new System.Drawing.Size(619, 308);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        internal System.Windows.Forms.ListBox lbParameters;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.CheckBox ckbParmMultiValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bValidValues;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbValidDisplayField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbValidFields;
        private System.Windows.Forms.ComboBox cbValidDataSets;
        private System.Windows.Forms.RadioButton rbValues;
        private System.Windows.Forms.RadioButton rbDataSet;
        private System.Windows.Forms.TextBox tbParmValidValues;
        private System.Windows.Forms.CheckBox ckbParmAllowBlank;
        private System.Windows.Forms.CheckBox ckbParmAllowNull;
        private System.Windows.Forms.TextBox tbParmPrompt;
        private System.Windows.Forms.Label lParmPrompt;
        private System.Windows.Forms.ComboBox cbParmType;
        private System.Windows.Forms.Label lParmType;
        private System.Windows.Forms.TextBox tbParmName;
        private System.Windows.Forms.Label lParmName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbParmDefaultValue;
        private System.Windows.Forms.Button bDefaultValues;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDefaultValueField;
        private System.Windows.Forms.ComboBox cbDefaultDataSets;
        private System.Windows.Forms.RadioButton rbDefaultValues;
        private System.Windows.Forms.RadioButton rbDefaultDataSetName;
        private System.Windows.Forms.Button bParmDown;
        private System.Windows.Forms.Button bParmUp;

    }
}