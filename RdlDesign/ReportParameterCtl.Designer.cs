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
            this.bParmDown = new System.Windows.Forms.Button();
            this.bParmUp = new System.Windows.Forms.Button();
            this.gbPropertyEdit = new System.Windows.Forms.GroupBox();
            this.ckbParmMultiValue = new System.Windows.Forms.CheckBox();
            this.gbValidValues = new System.Windows.Forms.GroupBox();
            this.bValidValues = new System.Windows.Forms.Button();
            this.lDisplayField = new System.Windows.Forms.Label();
            this.cbValidDisplayField = new System.Windows.Forms.ComboBox();
            this.lValidValuesField = new System.Windows.Forms.Label();
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
            this.gbDefaultValues = new System.Windows.Forms.GroupBox();
            this.tbParmDefaultValue = new System.Windows.Forms.TextBox();
            this.bDefaultValues = new System.Windows.Forms.Button();
            this.lDefaultValueFields = new System.Windows.Forms.Label();
            this.cbDefaultValueField = new System.Windows.Forms.ComboBox();
            this.cbDefaultDataSets = new System.Windows.Forms.ComboBox();
            this.rbDefaultValues = new System.Windows.Forms.RadioButton();
            this.rbDefaultDataSetName = new System.Windows.Forms.RadioButton();
            this.gbPropertyEdit.SuspendLayout();
            this.gbValidValues.SuspendLayout();
            this.gbDefaultValues.SuspendLayout();
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
            // gbPropertyEdit
            // 
            this.gbPropertyEdit.Controls.Add(this.ckbParmMultiValue);
            this.gbPropertyEdit.Controls.Add(this.gbValidValues);
            this.gbPropertyEdit.Controls.Add(this.ckbParmAllowBlank);
            this.gbPropertyEdit.Controls.Add(this.ckbParmAllowNull);
            this.gbPropertyEdit.Controls.Add(this.tbParmPrompt);
            this.gbPropertyEdit.Controls.Add(this.lParmPrompt);
            this.gbPropertyEdit.Controls.Add(this.cbParmType);
            this.gbPropertyEdit.Controls.Add(this.lParmType);
            this.gbPropertyEdit.Controls.Add(this.tbParmName);
            this.gbPropertyEdit.Controls.Add(this.lParmName);
            this.gbPropertyEdit.Controls.Add(this.gbDefaultValues);
            this.gbPropertyEdit.Enabled = false;
            this.gbPropertyEdit.Location = new System.Drawing.Point(163, -4);
            this.gbPropertyEdit.Margin = new System.Windows.Forms.Padding(1);
            this.gbPropertyEdit.Name = "gbPropertyEdit";
            this.gbPropertyEdit.Padding = new System.Windows.Forms.Padding(1);
            this.gbPropertyEdit.Size = new System.Drawing.Size(443, 303);
            this.gbPropertyEdit.TabIndex = 49;
            this.gbPropertyEdit.TabStop = false;
            // 
            // ckbParmMultiValue
            // 
            this.ckbParmMultiValue.Location = new System.Drawing.Point(236, 63);
            this.ckbParmMultiValue.Name = "ckbParmMultiValue";
            this.ckbParmMultiValue.Size = new System.Drawing.Size(79, 24);
            this.ckbParmMultiValue.TabIndex = 57;
            this.ckbParmMultiValue.Text = "MultiValue";
            this.ckbParmMultiValue.CheckedChanged += new System.EventHandler(this.ckbParmMultiValue_CheckedChanged);
            // 
            // gbValidValues
            // 
            this.gbValidValues.Controls.Add(this.bValidValues);
            this.gbValidValues.Controls.Add(this.lDisplayField);
            this.gbValidValues.Controls.Add(this.cbValidDisplayField);
            this.gbValidValues.Controls.Add(this.lValidValuesField);
            this.gbValidValues.Controls.Add(this.cbValidFields);
            this.gbValidValues.Controls.Add(this.cbValidDataSets);
            this.gbValidValues.Controls.Add(this.rbValues);
            this.gbValidValues.Controls.Add(this.rbDataSet);
            this.gbValidValues.Controls.Add(this.tbParmValidValues);
            this.gbValidValues.Location = new System.Drawing.Point(4, 187);
            this.gbValidValues.Name = "gbValidValues";
            this.gbValidValues.Size = new System.Drawing.Size(431, 108);
            this.gbValidValues.TabIndex = 53;
            this.gbValidValues.TabStop = false;
            this.gbValidValues.Text = "Valid Values";
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
            // lDisplayField
            // 
            this.lDisplayField.Location = new System.Drawing.Point(223, 86);
            this.lDisplayField.Name = "lDisplayField";
            this.lDisplayField.Size = new System.Drawing.Size(72, 16);
            this.lDisplayField.TabIndex = 37;
            this.lDisplayField.Text = "Display Field";
            this.lDisplayField.Visible = false;
            // 
            // cbValidDisplayField
            // 
            this.cbValidDisplayField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidDisplayField.Location = new System.Drawing.Point(301, 83);
            this.cbValidDisplayField.Name = "cbValidDisplayField";
            this.cbValidDisplayField.Size = new System.Drawing.Size(124, 21);
            this.cbValidDisplayField.TabIndex = 6;
            this.cbValidDisplayField.Visible = false;
            this.cbValidDisplayField.SelectedIndexChanged += new System.EventHandler(this.cbValidDisplayField_SelectedIndexChanged);
            // 
            // lValidValuesField
            // 
            this.lValidValuesField.Location = new System.Drawing.Point(226, 47);
            this.lValidValuesField.Name = "lValidValuesField";
            this.lValidValuesField.Size = new System.Drawing.Size(66, 18);
            this.lValidValuesField.TabIndex = 35;
            this.lValidValuesField.Text = "Value Field";
            this.lValidValuesField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lValidValuesField.Visible = false;
            // 
            // cbValidFields
            // 
            this.cbValidFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidFields.Location = new System.Drawing.Point(301, 46);
            this.cbValidFields.Name = "cbValidFields";
            this.cbValidFields.Size = new System.Drawing.Size(124, 21);
            this.cbValidFields.TabIndex = 5;
            this.cbValidFields.Visible = false;
            this.cbValidFields.SelectedIndexChanged += new System.EventHandler(this.cbValidFields_SelectedIndexChanged);
            // 
            // cbValidDataSets
            // 
            this.cbValidDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidDataSets.Location = new System.Drawing.Point(108, 46);
            this.cbValidDataSets.Name = "cbValidDataSets";
            this.cbValidDataSets.Size = new System.Drawing.Size(112, 21);
            this.cbValidDataSets.TabIndex = 4;
            this.cbValidDataSets.Visible = false;
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
            this.rbDataSet.Visible = false;
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
            this.ckbParmAllowBlank.Location = new System.Drawing.Point(82, 63);
            this.ckbParmAllowBlank.Name = "ckbParmAllowBlank";
            this.ckbParmAllowBlank.Size = new System.Drawing.Size(148, 24);
            this.ckbParmAllowBlank.TabIndex = 51;
            this.ckbParmAllowBlank.Text = "Allow blank (strings only)";
            this.ckbParmAllowBlank.CheckedChanged += new System.EventHandler(this.ckbParmAllowBlank_CheckedChanged);
            // 
            // ckbParmAllowNull
            // 
            this.ckbParmAllowNull.Location = new System.Drawing.Point(4, 63);
            this.ckbParmAllowNull.Name = "ckbParmAllowNull";
            this.ckbParmAllowNull.Size = new System.Drawing.Size(72, 24);
            this.ckbParmAllowNull.TabIndex = 50;
            this.ckbParmAllowNull.Text = "Allow null";
            this.ckbParmAllowNull.CheckedChanged += new System.EventHandler(this.ckbParmAllowNull_CheckedChanged);
            // 
            // tbParmPrompt
            // 
            this.tbParmPrompt.Location = new System.Drawing.Point(55, 37);
            this.tbParmPrompt.Name = "tbParmPrompt";
            this.tbParmPrompt.Size = new System.Drawing.Size(260, 20);
            this.tbParmPrompt.TabIndex = 49;
            this.tbParmPrompt.TextChanged += new System.EventHandler(this.tbParmPrompt_TextChanged);
            // 
            // lParmPrompt
            // 
            this.lParmPrompt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lParmPrompt.Location = new System.Drawing.Point(4, 38);
            this.lParmPrompt.Name = "lParmPrompt";
            this.lParmPrompt.Size = new System.Drawing.Size(48, 19);
            this.lParmPrompt.TabIndex = 56;
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
            this.cbParmType.Location = new System.Drawing.Point(228, 10);
            this.cbParmType.Name = "cbParmType";
            this.cbParmType.Size = new System.Drawing.Size(87, 21);
            this.cbParmType.TabIndex = 48;
            this.cbParmType.SelectedIndexChanged += new System.EventHandler(this.cbParmType_SelectedIndexChanged);
            // 
            // lParmType
            // 
            this.lParmType.Location = new System.Drawing.Point(157, 10);
            this.lParmType.Name = "lParmType";
            this.lParmType.Size = new System.Drawing.Size(56, 21);
            this.lParmType.TabIndex = 55;
            this.lParmType.Text = "Datatype";
            this.lParmType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbParmName
            // 
            this.tbParmName.Location = new System.Drawing.Point(55, 11);
            this.tbParmName.Name = "tbParmName";
            this.tbParmName.Size = new System.Drawing.Size(96, 20);
            this.tbParmName.TabIndex = 47;
            this.tbParmName.TextChanged += new System.EventHandler(this.tbParmName_TextChanged);
            // 
            // lParmName
            // 
            this.lParmName.Location = new System.Drawing.Point(4, 12);
            this.lParmName.Name = "lParmName";
            this.lParmName.Size = new System.Drawing.Size(45, 19);
            this.lParmName.TabIndex = 54;
            this.lParmName.Text = "Name";
            this.lParmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbDefaultValues
            // 
            this.gbDefaultValues.Controls.Add(this.tbParmDefaultValue);
            this.gbDefaultValues.Controls.Add(this.bDefaultValues);
            this.gbDefaultValues.Controls.Add(this.lDefaultValueFields);
            this.gbDefaultValues.Controls.Add(this.cbDefaultValueField);
            this.gbDefaultValues.Controls.Add(this.cbDefaultDataSets);
            this.gbDefaultValues.Controls.Add(this.rbDefaultValues);
            this.gbDefaultValues.Controls.Add(this.rbDefaultDataSetName);
            this.gbDefaultValues.Location = new System.Drawing.Point(4, 93);
            this.gbDefaultValues.Name = "gbDefaultValues";
            this.gbDefaultValues.Size = new System.Drawing.Size(431, 88);
            this.gbDefaultValues.TabIndex = 52;
            this.gbDefaultValues.TabStop = false;
            this.gbDefaultValues.Text = "Default Values";
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
            // lDefaultValueFields
            // 
            this.lDefaultValueFields.Location = new System.Drawing.Point(226, 46);
            this.lDefaultValueFields.Name = "lDefaultValueFields";
            this.lDefaultValueFields.Size = new System.Drawing.Size(67, 21);
            this.lDefaultValueFields.TabIndex = 35;
            this.lDefaultValueFields.Text = "Value Field";
            this.lDefaultValueFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lDefaultValueFields.Visible = false;
            // 
            // cbDefaultValueField
            // 
            this.cbDefaultValueField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultValueField.Location = new System.Drawing.Point(301, 46);
            this.cbDefaultValueField.Name = "cbDefaultValueField";
            this.cbDefaultValueField.Size = new System.Drawing.Size(124, 21);
            this.cbDefaultValueField.TabIndex = 5;
            this.cbDefaultValueField.Visible = false;
            this.cbDefaultValueField.SelectedIndexChanged += new System.EventHandler(this.cbDefaultValueField_SelectedIndexChanged);
            // 
            // cbDefaultDataSets
            // 
            this.cbDefaultDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultDataSets.Location = new System.Drawing.Point(108, 46);
            this.cbDefaultDataSets.Name = "cbDefaultDataSets";
            this.cbDefaultDataSets.Size = new System.Drawing.Size(112, 21);
            this.cbDefaultDataSets.TabIndex = 4;
            this.cbDefaultDataSets.Visible = false;
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
            this.rbDefaultDataSetName.Visible = false;
            this.rbDefaultDataSetName.CheckedChanged += new System.EventHandler(this.rbDefaultDataSetName_CheckedChanged);
            // 
            // ReportParameterCtl
            // 
            this.Controls.Add(this.gbPropertyEdit);
            this.Controls.Add(this.bParmDown);
            this.Controls.Add(this.bParmUp);
            this.Controls.Add(this.lbParameters);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.bRemove);
            this.Name = "ReportParameterCtl";
            this.Size = new System.Drawing.Size(614, 305);
            this.gbPropertyEdit.ResumeLayout(false);
            this.gbPropertyEdit.PerformLayout();
            this.gbValidValues.ResumeLayout(false);
            this.gbValidValues.PerformLayout();
            this.gbDefaultValues.ResumeLayout(false);
            this.gbDefaultValues.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        internal System.Windows.Forms.ListBox lbParameters;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.Button bParmDown;
        private System.Windows.Forms.Button bParmUp;
        private System.Windows.Forms.GroupBox gbPropertyEdit;
        private System.Windows.Forms.CheckBox ckbParmMultiValue;
        private System.Windows.Forms.GroupBox gbValidValues;
        private System.Windows.Forms.Button bValidValues;
        private System.Windows.Forms.Label lDisplayField;
        private System.Windows.Forms.ComboBox cbValidDisplayField;
        private System.Windows.Forms.Label lValidValuesField;
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
        private System.Windows.Forms.GroupBox gbDefaultValues;
        private System.Windows.Forms.TextBox tbParmDefaultValue;
        private System.Windows.Forms.Button bDefaultValues;
        private System.Windows.Forms.Label lDefaultValueFields;
        private System.Windows.Forms.ComboBox cbDefaultValueField;
        private System.Windows.Forms.ComboBox cbDefaultDataSets;
        private System.Windows.Forms.RadioButton rbDefaultValues;
        private System.Windows.Forms.RadioButton rbDefaultDataSetName;

    }
}