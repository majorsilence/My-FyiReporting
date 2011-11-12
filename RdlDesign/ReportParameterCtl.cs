/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Text;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for StyleCtl.
	/// </summary>
	internal class ReportParameterCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private System.Windows.Forms.Button bParmDown;
		private System.Windows.Forms.Button bParmUp;
		private System.Windows.Forms.TextBox tbParmValidValues;
		private System.Windows.Forms.CheckBox ckbParmAllowBlank;
		private System.Windows.Forms.TextBox tbParmPrompt;
		private System.Windows.Forms.Label lParmPrompt;
		private System.Windows.Forms.ComboBox cbParmType;
		private System.Windows.Forms.Label lParmType;
		private System.Windows.Forms.TextBox tbParmName;
		private System.Windows.Forms.Label lParmName;
		private System.Windows.Forms.Button bRemove;
		private System.Windows.Forms.Button bAdd;
		private System.Windows.Forms.ListBox lbParameters;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbDataSet;
		private System.Windows.Forms.RadioButton rbValues;
		private System.Windows.Forms.ComboBox cbValidDataSets;
		private System.Windows.Forms.ComboBox cbValidFields;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbValidDisplayField;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbDefaultValueField;
		private System.Windows.Forms.ComboBox cbDefaultDataSets;
		private System.Windows.Forms.RadioButton rbDefaultValues;
		private System.Windows.Forms.RadioButton rbDefaultDataSetName;
		private System.Windows.Forms.CheckBox ckbParmAllowNull;
		private System.Windows.Forms.Button bDefaultValues;
		private System.Windows.Forms.TextBox tbParmDefaultValue;
		private System.Windows.Forms.Button bValidValues;
        private CheckBox ckbParmMultiValue;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal ReportParameterCtl(DesignXmlDraw dxDraw)
		{
			_Draw = dxDraw;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			// Populate datasets combos
			object[] datasets = _Draw.DataSetNames;
			this.cbDefaultDataSets.Items.AddRange(datasets);
			this.cbValidDataSets.Items.AddRange(datasets);

			XmlNode rNode = _Draw.GetReportNode();
			XmlNode rpsNode = _Draw.GetNamedChildNode(rNode, "ReportParameters");
			if (rpsNode == null)
				return;
			foreach (XmlNode repNode in rpsNode)
			{	
				XmlAttribute nAttr = repNode.Attributes["Name"];
				if (nAttr == null)	// shouldn't really happen
					continue;
				ReportParm repParm = new ReportParm(nAttr.Value);
				repParm.DataType = _Draw.GetElementValue(repNode, "DataType", "String");
				// Get default values
				InitDefaultValues(repNode, repParm);

				string nullable  = _Draw.GetElementValue(repNode, "Nullable", "false");
				repParm.AllowNull = (nullable.ToLower() == "true");
				string allowBlank  = _Draw.GetElementValue(repNode, "AllowBlank", "false");
				repParm.AllowBlank = (allowBlank.ToLower() == "true");
                string mvalue = _Draw.GetElementValue(repNode, "MultiValue", "false");
                repParm.MultiValue = (mvalue.ToLower() == "true");
                repParm.Prompt = _Draw.GetElementValue(repNode, "Prompt", "");

				InitValidValues(repNode, repParm);

				this.lbParameters.Items.Add(repParm);
			}
			if (lbParameters.Items.Count > 0)
				lbParameters.SelectedIndex = 0;
		}

		void InitDefaultValues(XmlNode reportParameterNode, ReportParm repParm)
		{
			repParm.Default = true;
			XmlNode dfNode = _Draw.GetNamedChildNode(reportParameterNode, "DefaultValue");
			if (dfNode == null)
				return;

			XmlNode vNodes = _Draw.GetNamedChildNode(dfNode, "Values");
			if (vNodes != null)
			{
				List<string> al = new List<string>();
				foreach (XmlNode v in vNodes.ChildNodes)
				{
					if (v.InnerText.Length <= 0)
						continue;
					al.Add(v.InnerText);
				}
				if (al.Count >= 1)
					repParm.DefaultValue  = al;
			}
			XmlNode dsNodes = _Draw.GetNamedChildNode(dfNode, "DataSetReference");
			if (dsNodes != null)
			{
				repParm.Default = false;
				repParm.DefaultDSRDataSetName = _Draw.GetElementValue(dsNodes, "DataSetName", "");
				repParm.DefaultDSRValueField = _Draw.GetElementValue(dsNodes, "ValueField", "");
			}
		}

		void InitValidValues(XmlNode reportParameterNode, ReportParm repParm)
		{
			repParm.Valid = true;
			XmlNode vvsNode = _Draw.GetNamedChildNode(reportParameterNode, "ValidValues");
			if (vvsNode == null)
				return;

			XmlNode vNodes = _Draw.GetNamedChildNode(vvsNode, "ParameterValues");
			if (vNodes != null)
			{
                List<ParameterValueItem> pvs = new List<ParameterValueItem>();
				foreach (XmlNode v in vNodes.ChildNodes)
				{
					if (v.Name != "ParameterValue")
						continue;
					XmlNode pv = _Draw.GetNamedChildNode(v, "Value");
					if (pv == null)
						continue;
					if (pv == null || pv.InnerText.Length <= 0)
						continue;
					ParameterValueItem pvi = new ParameterValueItem();
					pvi.Value = pv.InnerText;
					pvi.Label = _Draw.GetElementValue(v, "Label", null);
					pvs.Add(pvi);
				}
				if (pvs.Count > 0)
				{
					repParm.ValidValues = pvs;
				}
			}
			XmlNode dsNodes = _Draw.GetNamedChildNode(vvsNode, "DataSetReference");
			if (dsNodes != null)
			{
				repParm.Valid = false;
				repParm.ValidValuesDSRDataSetName = _Draw.GetElementValue(dsNodes, "DataSetName", "");
				repParm.ValidValuesDSRValueField = _Draw.GetElementValue(dsNodes, "ValueField", "");
				repParm.ValidValuesDSRLabelField = _Draw.GetElementValue(dsNodes, "LabelField", "");
			}
		}

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
            this.bParmDown = new System.Windows.Forms.Button();
            this.bParmUp = new System.Windows.Forms.Button();
            this.tbParmValidValues = new System.Windows.Forms.TextBox();
            this.ckbParmAllowBlank = new System.Windows.Forms.CheckBox();
            this.tbParmPrompt = new System.Windows.Forms.TextBox();
            this.lParmPrompt = new System.Windows.Forms.Label();
            this.cbParmType = new System.Windows.Forms.ComboBox();
            this.lParmType = new System.Windows.Forms.Label();
            this.tbParmName = new System.Windows.Forms.TextBox();
            this.lParmName = new System.Windows.Forms.Label();
            this.bRemove = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.lbParameters = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bValidValues = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbValidDisplayField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbValidFields = new System.Windows.Forms.ComboBox();
            this.cbValidDataSets = new System.Windows.Forms.ComboBox();
            this.rbValues = new System.Windows.Forms.RadioButton();
            this.rbDataSet = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbParmDefaultValue = new System.Windows.Forms.TextBox();
            this.bDefaultValues = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDefaultValueField = new System.Windows.Forms.ComboBox();
            this.cbDefaultDataSets = new System.Windows.Forms.ComboBox();
            this.rbDefaultValues = new System.Windows.Forms.RadioButton();
            this.rbDefaultDataSetName = new System.Windows.Forms.RadioButton();
            this.ckbParmAllowNull = new System.Windows.Forms.CheckBox();
            this.ckbParmMultiValue = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bParmDown
            // 
            this.bParmDown.Font = new System.Drawing.Font("Wingdings", 7.25F, System.Drawing.FontStyle.Bold);
            this.bParmDown.Location = new System.Drawing.Point(128, 40);
            this.bParmDown.Name = "bParmDown";
            this.bParmDown.Size = new System.Drawing.Size(20, 24);
            this.bParmDown.TabIndex = 4;
            this.bParmDown.Text = "";
            this.bParmDown.Click += new System.EventHandler(this.bParmDown_Click);
            // 
            // bParmUp
            // 
            this.bParmUp.Font = new System.Drawing.Font("Wingdings", 7.25F, System.Drawing.FontStyle.Bold);
            this.bParmUp.Location = new System.Drawing.Point(128, 8);
            this.bParmUp.Name = "bParmUp";
            this.bParmUp.Size = new System.Drawing.Size(20, 24);
            this.bParmUp.TabIndex = 3;
            this.bParmUp.Text = "";
            this.bParmUp.Click += new System.EventHandler(this.bParmUp_Click);
            // 
            // tbParmValidValues
            // 
            this.tbParmValidValues.Location = new System.Drawing.Point(72, 16);
            this.tbParmValidValues.Name = "tbParmValidValues";
            this.tbParmValidValues.ReadOnly = true;
            this.tbParmValidValues.Size = new System.Drawing.Size(328, 20);
            this.tbParmValidValues.TabIndex = 1;
            // 
            // ckbParmAllowBlank
            // 
            this.ckbParmAllowBlank.Location = new System.Drawing.Point(222, 72);
            this.ckbParmAllowBlank.Name = "ckbParmAllowBlank";
            this.ckbParmAllowBlank.Size = new System.Drawing.Size(148, 24);
            this.ckbParmAllowBlank.TabIndex = 9;
            this.ckbParmAllowBlank.Text = "Allow blank (strings only)";
            this.ckbParmAllowBlank.CheckedChanged += new System.EventHandler(this.ckbParmAllowBlank_CheckedChanged);
            // 
            // tbParmPrompt
            // 
            this.tbParmPrompt.Location = new System.Drawing.Point(208, 40);
            this.tbParmPrompt.Name = "tbParmPrompt";
            this.tbParmPrompt.Size = new System.Drawing.Size(240, 20);
            this.tbParmPrompt.TabIndex = 7;
            this.tbParmPrompt.TextChanged += new System.EventHandler(this.tbParmPrompt_TextChanged);
            // 
            // lParmPrompt
            // 
            this.lParmPrompt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lParmPrompt.Location = new System.Drawing.Point(160, 40);
            this.lParmPrompt.Name = "lParmPrompt";
            this.lParmPrompt.Size = new System.Drawing.Size(48, 16);
            this.lParmPrompt.TabIndex = 23;
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
            this.cbParmType.Location = new System.Drawing.Point(368, 16);
            this.cbParmType.Name = "cbParmType";
            this.cbParmType.Size = new System.Drawing.Size(80, 21);
            this.cbParmType.TabIndex = 6;
            this.cbParmType.SelectedIndexChanged += new System.EventHandler(this.cbParmType_SelectedIndexChanged);
            // 
            // lParmType
            // 
            this.lParmType.Location = new System.Drawing.Point(304, 16);
            this.lParmType.Name = "lParmType";
            this.lParmType.Size = new System.Drawing.Size(56, 23);
            this.lParmType.TabIndex = 21;
            this.lParmType.Text = "Datatype";
            this.lParmType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbParmName
            // 
            this.tbParmName.Location = new System.Drawing.Point(208, 16);
            this.tbParmName.Name = "tbParmName";
            this.tbParmName.Size = new System.Drawing.Size(104, 20);
            this.tbParmName.TabIndex = 5;
            this.tbParmName.TextChanged += new System.EventHandler(this.tbParmName_TextChanged);
            // 
            // lParmName
            // 
            this.lParmName.Location = new System.Drawing.Point(160, 16);
            this.lParmName.Name = "lParmName";
            this.lParmName.Size = new System.Drawing.Size(40, 16);
            this.lParmName.TabIndex = 19;
            this.lParmName.Text = "Name";
            this.lParmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bRemove
            // 
            this.bRemove.Location = new System.Drawing.Point(68, 70);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(54, 23);
            this.bRemove.TabIndex = 2;
            this.bRemove.Text = "Remove";
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(8, 70);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(54, 23);
            this.bAdd.TabIndex = 1;
            this.bAdd.Text = "Add";
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // lbParameters
            // 
            this.lbParameters.Location = new System.Drawing.Point(8, 8);
            this.lbParameters.Name = "lbParameters";
            this.lbParameters.Size = new System.Drawing.Size(112, 56);
            this.lbParameters.TabIndex = 0;
            this.lbParameters.SelectedIndexChanged += new System.EventHandler(this.lbParameters_SelectedIndexChanged);
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
            this.groupBox1.Location = new System.Drawing.Point(8, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 96);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Valid Values";
            // 
            // bValidValues
            // 
            this.bValidValues.Location = new System.Drawing.Point(408, 16);
            this.bValidValues.Name = "bValidValues";
            this.bValidValues.Size = new System.Drawing.Size(24, 23);
            this.bValidValues.TabIndex = 2;
            this.bValidValues.Text = "...";
            this.bValidValues.Click += new System.EventHandler(this.bValidValues_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(240, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 37;
            this.label2.Text = "Display Field";
            // 
            // cbValidDisplayField
            // 
            this.cbValidDisplayField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidDisplayField.Location = new System.Drawing.Point(312, 72);
            this.cbValidDisplayField.Name = "cbValidDisplayField";
            this.cbValidDisplayField.Size = new System.Drawing.Size(112, 21);
            this.cbValidDisplayField.TabIndex = 6;
            this.cbValidDisplayField.SelectedIndexChanged += new System.EventHandler(this.cbValidDisplayField_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(240, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "Value Field";
            // 
            // cbValidFields
            // 
            this.cbValidFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidFields.Location = new System.Drawing.Point(312, 40);
            this.cbValidFields.Name = "cbValidFields";
            this.cbValidFields.Size = new System.Drawing.Size(112, 21);
            this.cbValidFields.TabIndex = 5;
            this.cbValidFields.SelectedIndexChanged += new System.EventHandler(this.cbValidFields_SelectedIndexChanged);
            // 
            // cbValidDataSets
            // 
            this.cbValidDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValidDataSets.Location = new System.Drawing.Point(112, 40);
            this.cbValidDataSets.Name = "cbValidDataSets";
            this.cbValidDataSets.Size = new System.Drawing.Size(112, 21);
            this.cbValidDataSets.TabIndex = 4;
            this.cbValidDataSets.SelectedIndexChanged += new System.EventHandler(this.cbValidDataSets_SelectedIndexChanged);
            // 
            // rbValues
            // 
            this.rbValues.Location = new System.Drawing.Point(8, 16);
            this.rbValues.Name = "rbValues";
            this.rbValues.Size = new System.Drawing.Size(64, 24);
            this.rbValues.TabIndex = 0;
            this.rbValues.Text = "Values";
            this.rbValues.CheckedChanged += new System.EventHandler(this.rbValues_CheckedChanged);
            // 
            // rbDataSet
            // 
            this.rbDataSet.Location = new System.Drawing.Point(8, 40);
            this.rbDataSet.Name = "rbDataSet";
            this.rbDataSet.Size = new System.Drawing.Size(104, 24);
            this.rbDataSet.TabIndex = 3;
            this.rbDataSet.Text = "Data Set Name";
            this.rbDataSet.CheckedChanged += new System.EventHandler(this.rbDataSet_CheckedChanged);
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
            this.groupBox2.Location = new System.Drawing.Point(8, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 72);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Default Values";
            // 
            // tbParmDefaultValue
            // 
            this.tbParmDefaultValue.Location = new System.Drawing.Point(72, 16);
            this.tbParmDefaultValue.Name = "tbParmDefaultValue";
            this.tbParmDefaultValue.ReadOnly = true;
            this.tbParmDefaultValue.Size = new System.Drawing.Size(328, 20);
            this.tbParmDefaultValue.TabIndex = 1;
            // 
            // bDefaultValues
            // 
            this.bDefaultValues.Location = new System.Drawing.Point(407, 16);
            this.bDefaultValues.Name = "bDefaultValues";
            this.bDefaultValues.Size = new System.Drawing.Size(23, 23);
            this.bDefaultValues.TabIndex = 2;
            this.bDefaultValues.Text = "...";
            this.bDefaultValues.Click += new System.EventHandler(this.bDefaultValues_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(240, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 23);
            this.label4.TabIndex = 35;
            this.label4.Text = "Value Field";
            // 
            // cbDefaultValueField
            // 
            this.cbDefaultValueField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultValueField.Location = new System.Drawing.Point(312, 40);
            this.cbDefaultValueField.Name = "cbDefaultValueField";
            this.cbDefaultValueField.Size = new System.Drawing.Size(112, 21);
            this.cbDefaultValueField.TabIndex = 5;
            this.cbDefaultValueField.SelectedIndexChanged += new System.EventHandler(this.cbDefaultValueField_SelectedIndexChanged);
            // 
            // cbDefaultDataSets
            // 
            this.cbDefaultDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultDataSets.Location = new System.Drawing.Point(112, 40);
            this.cbDefaultDataSets.Name = "cbDefaultDataSets";
            this.cbDefaultDataSets.Size = new System.Drawing.Size(112, 21);
            this.cbDefaultDataSets.TabIndex = 4;
            this.cbDefaultDataSets.SelectedIndexChanged += new System.EventHandler(this.cbDefaultDataSets_SelectedIndexChanged);
            // 
            // rbDefaultValues
            // 
            this.rbDefaultValues.Location = new System.Drawing.Point(8, 16);
            this.rbDefaultValues.Name = "rbDefaultValues";
            this.rbDefaultValues.Size = new System.Drawing.Size(64, 24);
            this.rbDefaultValues.TabIndex = 0;
            this.rbDefaultValues.Text = "Values";
            this.rbDefaultValues.CheckedChanged += new System.EventHandler(this.rbDefaultValues_CheckedChanged);
            // 
            // rbDefaultDataSetName
            // 
            this.rbDefaultDataSetName.Location = new System.Drawing.Point(8, 40);
            this.rbDefaultDataSetName.Name = "rbDefaultDataSetName";
            this.rbDefaultDataSetName.Size = new System.Drawing.Size(104, 24);
            this.rbDefaultDataSetName.TabIndex = 3;
            this.rbDefaultDataSetName.Text = "Data Set Name";
            this.rbDefaultDataSetName.CheckedChanged += new System.EventHandler(this.rbDefaultDataSetName_CheckedChanged);
            // 
            // ckbParmAllowNull
            // 
            this.ckbParmAllowNull.Location = new System.Drawing.Point(150, 72);
            this.ckbParmAllowNull.Name = "ckbParmAllowNull";
            this.ckbParmAllowNull.Size = new System.Drawing.Size(72, 24);
            this.ckbParmAllowNull.TabIndex = 8;
            this.ckbParmAllowNull.Text = "Allow null";
            this.ckbParmAllowNull.CheckedChanged += new System.EventHandler(this.ckbParmAllowNull_CheckedChanged);
            // 
            // ckbParmMultiValue
            // 
            this.ckbParmMultiValue.Location = new System.Drawing.Point(376, 72);
            this.ckbParmMultiValue.Name = "ckbParmMultiValue";
            this.ckbParmMultiValue.Size = new System.Drawing.Size(79, 24);
            this.ckbParmMultiValue.TabIndex = 24;
            this.ckbParmMultiValue.Text = "MultiValue";
            this.ckbParmMultiValue.CheckedChanged += new System.EventHandler(this.ckbParmMultiValue_CheckedChanged);
            // 
            // ReportParameterCtl
            // 
            this.Controls.Add(this.ckbParmMultiValue);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bParmDown);
            this.Controls.Add(this.bParmUp);
            this.Controls.Add(this.ckbParmAllowBlank);
            this.Controls.Add(this.ckbParmAllowNull);
            this.Controls.Add(this.tbParmPrompt);
            this.Controls.Add(this.lParmPrompt);
            this.Controls.Add(this.cbParmType);
            this.Controls.Add(this.lParmType);
            this.Controls.Add(this.tbParmName);
            this.Controls.Add(this.lParmName);
            this.Controls.Add(this.bRemove);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.lbParameters);
            this.Controls.Add(this.groupBox2);
            this.Name = "ReportParameterCtl";
            this.Size = new System.Drawing.Size(456, 296);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
 
		public bool IsValid()
		{
			return true;
		}

		public void Apply()
		{
			XmlNode rNode = _Draw.GetReportNode();
			_Draw.RemoveElement(rNode, "ReportParameters");	// remove old ReportParameters
			if (this.lbParameters.Items.Count <= 0)
				return;			// nothing in list?  all done

			XmlNode rpsNode = _Draw.SetElement(rNode, "ReportParameters", null);
			foreach (ReportParm repParm in lbParameters.Items)
			{
				if (repParm.Name == null || repParm.Name.Length <= 0)
					continue;	// shouldn't really happen
				XmlNode repNode = _Draw.CreateElement(rpsNode, "ReportParameter", null);
				// Create the name attribute
				_Draw.SetElementAttribute(repNode, "Name", repParm.Name);

				_Draw.SetElement(repNode, "DataType", repParm.DataType);
				// Handle default values
				ApplyDefaultValues(repNode, repParm);
				
				_Draw.SetElement(repNode, "Nullable", repParm.AllowNull? "true": "false");
				_Draw.SetElement(repNode, "AllowBlank", repParm.AllowBlank? "true": "false");
                _Draw.SetElement(repNode, "MultiValue", repParm.MultiValue ? "true" : "false");
                _Draw.SetElement(repNode, "Prompt", repParm.Prompt);

				// Handle ValidValues
				ApplyValidValues(repNode, repParm);
			}
		}

		private void ApplyDefaultValues(XmlNode repNode, ReportParm repParm)
		{
			_Draw.RemoveElement(repNode, "DefaultValue");
			if (repParm.Default)
			{
				if (repParm.DefaultValue == null || repParm.DefaultValue.Count == 0)
					return;

				XmlNode defNode = _Draw.GetCreateNamedChildNode(repNode, "DefaultValue");
				XmlNode vNodes = _Draw.GetCreateNamedChildNode(defNode, "Values");
				foreach (string dv in repParm.DefaultValue)
				{
					_Draw.CreateElement(vNodes, "Value", dv);
				}
			}
			else
			{
				if (repParm.DefaultDSRDataSetName == null || repParm.DefaultDSRDataSetName.Length == 0 ||
					repParm.DefaultDSRValueField == null || repParm.DefaultDSRValueField.Length == 0)
					return;
				XmlNode defNode = _Draw.GetCreateNamedChildNode(repNode, "DefaultValue");
				XmlNode dsrNode = _Draw.GetCreateNamedChildNode(defNode, "DataSetReference");
				_Draw.CreateElement(dsrNode, "DataSetName", repParm.DefaultDSRDataSetName);
				_Draw.CreateElement(dsrNode, "ValueField", repParm.DefaultDSRValueField);
			}
		}

		private void ApplyValidValues(XmlNode repNode, ReportParm repParm)
		{
			_Draw.RemoveElement(repNode, "ValidValues");
			if (repParm.Valid)
			{
				if (repParm.ValidValues == null || repParm.ValidValues.Count == 0)
					return;

				XmlNode vvNode = _Draw.GetCreateNamedChildNode(repNode, "ValidValues");
				XmlNode vNodes = _Draw.GetCreateNamedChildNode(vvNode, "ParameterValues");
				// put out the parameter values
				foreach (ParameterValueItem dvi in repParm.ValidValues)
				{
					XmlNode pvNode = _Draw.CreateElement(vNodes, "ParameterValue", null);
					_Draw.CreateElement(pvNode, "Value", dvi.Value);
					if (dvi.Label != null)
						_Draw.CreateElement(pvNode, "Label", dvi.Label);
				}
			}
			else
			{
				if (repParm.ValidValuesDSRDataSetName == null || repParm.ValidValuesDSRDataSetName.Length == 0 ||
					repParm.ValidValuesDSRValueField == null || repParm.ValidValuesDSRValueField.Length == 0)
					return;
				XmlNode defNode = _Draw.GetCreateNamedChildNode(repNode, "ValidValues");
				XmlNode dsrNode = _Draw.GetCreateNamedChildNode(defNode, "DataSetReference");
				_Draw.CreateElement(dsrNode, "DataSetName", repParm.ValidValuesDSRDataSetName);
				_Draw.CreateElement(dsrNode, "ValueField", repParm.ValidValuesDSRValueField);
				if (repParm.ValidValuesDSRLabelField != null && repParm.ValidValuesDSRLabelField.Length > 0)
					_Draw.CreateElement(dsrNode, "LabelField", repParm.ValidValuesDSRLabelField);
			}
		}

		private void bAdd_Click(object sender, System.EventArgs e)
		{
			ReportParm rp = new ReportParm("newparm");
			int cur = this.lbParameters.Items.Add(rp);
			lbParameters.SelectedIndex = cur;
			this.tbParmName.Focus();
		}

		private void bRemove_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;
			lbParameters.Items.RemoveAt(cur);
			if (lbParameters.Items.Count <= 0)
				return;
			cur--;
			if (cur < 0)
				cur = 0;
			lbParameters.SelectedIndex = cur;
		}

		private void lbParameters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			tbParmName.Text = rp.Name;
			cbParmType.Text = rp.DataType;
			tbParmPrompt.Text = rp.Prompt;
			ckbParmAllowBlank.Checked = rp.AllowBlank;
            ckbParmMultiValue.Checked = rp.MultiValue;
            ckbParmAllowNull.Checked = rp.AllowNull;
			// Handle default values
			if (rp.Default)
			{
				this.rbDefaultValues.Checked = true;
				tbParmDefaultValue.Text = rp.DefaultValueDisplay; 			

				tbParmDefaultValue.Enabled = bDefaultValues.Enabled = true;
				this.cbDefaultDataSets.Enabled = false;
				this.cbDefaultValueField.Enabled = false;
				this.cbDefaultDataSets.SelectedIndex = -1;
				this.cbDefaultValueField.SelectedIndex = -1;
			}
			else
			{
				this.rbDefaultDataSetName.Checked = true;
				this.cbDefaultDataSets.Text = rp.DefaultDSRDataSetName;
				this.cbDefaultValueField.Text = rp.DefaultDSRValueField;

				tbParmDefaultValue.Enabled = bDefaultValues.Enabled =false;
				tbParmDefaultValue.Text = "";
				this.cbDefaultDataSets.Enabled = true;
				this.cbDefaultValueField.Enabled = true;
			}
			// Handle Valid Values
			if (rp.Valid)
			{
				this.rbValues.Checked = true;
				tbParmValidValues.Text = rp.ValidValuesDisplay;

				tbParmValidValues.Enabled = bValidValues.Enabled = true;
				this.cbValidDataSets.Enabled =
					this.cbValidFields.Enabled =
					this.cbValidDisplayField.Enabled = false;
				this.cbValidDataSets.SelectedIndex   =
					this.cbValidFields.SelectedIndex =
					this.cbValidDisplayField.SelectedIndex = -1;
			}
			else
			{
				this.rbDataSet.Checked = true;
				this.cbValidDataSets.Text = rp.ValidValuesDSRDataSetName;
				this.cbValidFields.Text = rp.ValidValuesDSRValueField;
				this.cbValidDisplayField.Text = rp.ValidValuesDSRLabelField == null? "":rp.ValidValuesDSRLabelField;

				this.cbValidDataSets.Enabled =
						this.cbValidFields.Enabled =
						this.cbValidDisplayField.Enabled = true;
				tbParmValidValues.Enabled = bValidValues.Enabled = false;
				tbParmValidValues.Text = "";
			}
		}

		private void lbParameters_MoveItem(int curloc, int newloc)
		{
			ReportParm rp = lbParameters.Items[curloc] as ReportParm;
			if (rp == null)
				return;

			lbParameters.BeginUpdate();
			lbParameters.Items.RemoveAt(curloc);
			lbParameters.Items.Insert(newloc, rp);
			lbParameters.SelectedIndex = newloc;
			lbParameters.EndUpdate();
		}

		private void tbParmName_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			if (rp.Name == tbParmName.Text)
				return;

			rp.Name = tbParmName.Text;
			// text doesn't change in listbox; force change by removing and re-adding item
			lbParameters_MoveItem(cur, cur);
		}

		private void cbParmType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.DataType = cbParmType.Text;
		}

		private void tbParmPrompt_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.Prompt = tbParmPrompt.Text;
		}

		private void ckbParmAllowNull_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.AllowNull = ckbParmAllowNull.Checked;
		}

		private void ckbParmAllowBlank_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.AllowBlank = ckbParmAllowBlank.Checked;
		}

		private void bParmUp_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur <= 0)
				return;
		
			lbParameters_MoveItem(cur, cur-1);
		}

		private void bParmDown_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur+1 >= lbParameters.Items.Count)
				return;
		
			lbParameters_MoveItem(cur, cur+1);
		}

		private void cbDefaultDataSets_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.DefaultDSRDataSetName = cbDefaultDataSets.Text;

			// Populate the fields from the selected dataset
			this.cbDefaultValueField.Items.Clear();
			string[] fields = _Draw.GetFields(cbDefaultDataSets.Text, false);
            if (fields != null)
			    this.cbDefaultValueField.Items.AddRange(fields);
		}

		private void cbValidDataSets_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.ValidValuesDSRDataSetName = cbValidDataSets.Text;

			// Populate the fields from the selected dataset
			this.cbValidFields.Items.Clear();
			string[] fields = _Draw.GetFields(cbValidDataSets.Text, false);
            if (fields != null)
			    this.cbValidFields.Items.AddRange(fields);
			this.cbValidDisplayField.Items.Clear();
			this.cbValidDisplayField.Items.Add("");
            if (fields != null)
			    this.cbValidDisplayField.Items.AddRange(fields);
		}

		private void cbDefaultValueField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.DefaultDSRValueField = cbDefaultValueField.Text;
		}

		private void cbValidFields_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.ValidValuesDSRValueField = cbValidFields.Text;
		}

		private void cbValidDisplayField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.ValidValuesDSRLabelField = cbValidDisplayField.Text;
		}

		private void rbDefaultValues_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.Default = rbDefaultValues.Checked;

			tbParmDefaultValue.Enabled = bDefaultValues.Enabled = rbDefaultValues.Checked;
			this.cbDefaultDataSets.Enabled = 
				this.cbDefaultValueField.Enabled = !rbDefaultValues.Checked;
		}

		private void rbDefaultDataSetName_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.Default = !rbDefaultDataSetName.Checked;

			tbParmDefaultValue.Enabled = bDefaultValues.Enabled = !rbDefaultDataSetName.Checked;
			this.cbDefaultDataSets.Enabled = 
				this.cbDefaultValueField.Enabled = rbDefaultDataSetName.Checked;
		}

		private void rbValues_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			this.tbParmValidValues.Enabled = bValidValues.Enabled =  rbValues.Checked;
			rp.Valid = rbValues.Checked;

			this.cbValidDisplayField.Enabled = 
				this.cbValidFields.Enabled =
				this.cbValidDataSets.Enabled = !rbValues.Checked;
		}

		private void rbDataSet_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.Valid = !rbDataSet.Checked;
			this.tbParmValidValues.Enabled = bValidValues.Enabled = !rbDataSet.Checked;
			this.cbValidDisplayField.Enabled = 
				this.cbValidFields.Enabled =
				this.cbValidDataSets.Enabled = rbDataSet.Checked;
		}

		private void bDefaultValues_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

            using (DialogListOfStrings dlos = new DialogListOfStrings(rp.DefaultValue))
            {
                dlos.Text = "Default Values";
                if (dlos.ShowDialog() != DialogResult.OK)
                    return;
                rp.DefaultValue = dlos.ListOfStrings;
                this.tbParmDefaultValue.Text = rp.DefaultValueDisplay;
            }
		}

		private void bValidValues_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

            using (DialogValidValues dvv = new DialogValidValues(rp.ValidValues))
            {
                if (dvv.ShowDialog() != DialogResult.OK)
                    return;
                rp.ValidValues = dvv.ValidValues;
                this.tbParmValidValues.Text = rp.ValidValuesDisplay;
            }
		}

        private void ckbParmMultiValue_CheckedChanged(object sender, EventArgs e)
        {
            int cur = lbParameters.SelectedIndex;
            if (cur < 0)
                return;

            ReportParm rp = lbParameters.Items[cur] as ReportParm;
            if (rp == null)
                return;

            rp.MultiValue = ckbParmMultiValue.Checked;

        }

	}
}
