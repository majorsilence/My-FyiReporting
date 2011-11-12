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

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for ReportCtl.
	/// </summary>
	internal class TableCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		bool fDataSet, fPBBefore, fPBAfter, fNoRows;
		bool fDetailElementName, fDetailCollectionName, fRenderDetails;
		bool fCheckRows;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbDataSet;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkPBBefore;
		private System.Windows.Forms.CheckBox chkPBAfter;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbNoRows;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkRenderDetails;
		private System.Windows.Forms.TextBox tbDetailElementName;
		private System.Windows.Forms.TextBox tbDetailCollectionName;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox chkDetails;
		private System.Windows.Forms.CheckBox chkHeaderRows;
		private System.Windows.Forms.CheckBox chkFooterRows;
        private CheckBox chkFooterRepeat;
        private CheckBox chkHeaderRepeat;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal TableCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
		{
			_ReportItems = ris;
			_Draw = dxDraw;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			XmlNode riNode = _ReportItems[0];

			tbNoRows.Text = _Draw.GetElementValue(riNode, "NoRows", "");
			cbDataSet.Items.AddRange(_Draw.DataSetNames);
			cbDataSet.Text = _Draw.GetDataSetNameValue(riNode);
			if (_Draw.GetReportItemDataRegionContainer(riNode) != null)
				cbDataSet.Enabled = false;
			chkPBBefore.Checked = _Draw.GetElementValue(riNode, "PageBreakAtStart", "false").ToLower()=="true"? true:false;
			chkPBAfter.Checked = _Draw.GetElementValue(riNode, "PageBreakAtEnd", "false").ToLower()=="true"? true:false;

			this.chkRenderDetails.Checked = _Draw.GetElementValue(riNode, "DetailDataElementOutput", "output").ToLower() == "output";
			this.tbDetailElementName.Text = _Draw.GetElementValue(riNode, "DetailDataElementName", "Details");
			this.tbDetailCollectionName.Text = _Draw.GetElementValue(riNode, "DetailDataCollectionName", "Details_Collection");

			this.chkDetails.Checked = _Draw.GetNamedChildNode(riNode, "Details") != null;
            XmlNode fNode = _Draw.GetNamedChildNode(riNode, "Footer");
			this.chkFooterRows.Checked = fNode != null;
            if (fNode != null)
            {
                chkFooterRepeat.Checked = _Draw.GetElementValue(fNode, "RepeatOnNewPage", "false").ToLower() == "true" ? true : false;
            }
            else
                chkFooterRepeat.Enabled = false;

            XmlNode hNode = _Draw.GetNamedChildNode(riNode, "Header");
            this.chkHeaderRows.Checked = hNode != null;
            if (hNode != null)
            {
                chkHeaderRepeat.Checked = _Draw.GetElementValue(hNode, "RepeatOnNewPage", "false").ToLower() == "true" ? true : false;
            }
            else
                chkHeaderRepeat.Enabled = false;

			fNoRows = fDataSet = fPBBefore = fPBAfter = 
				fDetailElementName = fDetailCollectionName = fRenderDetails =
				fCheckRows = false;
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbDataSet = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkPBAfter = new System.Windows.Forms.CheckBox();
            this.chkPBBefore = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbNoRows = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbDetailCollectionName = new System.Windows.Forms.TextBox();
            this.tbDetailElementName = new System.Windows.Forms.TextBox();
            this.chkRenderDetails = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkFooterRows = new System.Windows.Forms.CheckBox();
            this.chkHeaderRows = new System.Windows.Forms.CheckBox();
            this.chkDetails = new System.Windows.Forms.CheckBox();
            this.chkHeaderRepeat = new System.Windows.Forms.CheckBox();
            this.chkFooterRepeat = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "DataSet Name";
            // 
            // cbDataSet
            // 
            this.cbDataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSet.Location = new System.Drawing.Point(120, 16);
            this.cbDataSet.Name = "cbDataSet";
            this.cbDataSet.Size = new System.Drawing.Size(304, 21);
            this.cbDataSet.TabIndex = 0;
            this.cbDataSet.SelectedIndexChanged += new System.EventHandler(this.cbDataSet_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkPBAfter);
            this.groupBox1.Controls.Add(this.chkPBBefore);
            this.groupBox1.Location = new System.Drawing.Point(24, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 48);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Page Breaks";
            // 
            // chkPBAfter
            // 
            this.chkPBAfter.Location = new System.Drawing.Point(192, 16);
            this.chkPBAfter.Name = "chkPBAfter";
            this.chkPBAfter.Size = new System.Drawing.Size(128, 24);
            this.chkPBAfter.TabIndex = 1;
            this.chkPBAfter.Text = "Insert after Table";
            this.chkPBAfter.CheckedChanged += new System.EventHandler(this.chkPBAfter_CheckedChanged);
            // 
            // chkPBBefore
            // 
            this.chkPBBefore.Location = new System.Drawing.Point(16, 16);
            this.chkPBBefore.Name = "chkPBBefore";
            this.chkPBBefore.Size = new System.Drawing.Size(128, 24);
            this.chkPBBefore.TabIndex = 0;
            this.chkPBBefore.Text = "Insert before Table";
            this.chkPBBefore.CheckedChanged += new System.EventHandler(this.chkPBBefore_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "No rows message";
            // 
            // tbNoRows
            // 
            this.tbNoRows.Location = new System.Drawing.Point(120, 40);
            this.tbNoRows.Name = "tbNoRows";
            this.tbNoRows.Size = new System.Drawing.Size(304, 20);
            this.tbNoRows.TabIndex = 1;
            this.tbNoRows.Text = "textBox1";
            this.tbNoRows.TextChanged += new System.EventHandler(this.tbNoRows_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbDetailCollectionName);
            this.groupBox2.Controls.Add(this.tbDetailElementName);
            this.groupBox2.Controls.Add(this.chkRenderDetails);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(24, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 92);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "XML";
            // 
            // tbDetailCollectionName
            // 
            this.tbDetailCollectionName.Location = new System.Drawing.Point(160, 62);
            this.tbDetailCollectionName.Name = "tbDetailCollectionName";
            this.tbDetailCollectionName.Size = new System.Drawing.Size(224, 20);
            this.tbDetailCollectionName.TabIndex = 3;
            this.tbDetailCollectionName.Text = "textBox1";
            this.tbDetailCollectionName.TextChanged += new System.EventHandler(this.tbDetailCollectionName_TextChanged);
            // 
            // tbDetailElementName
            // 
            this.tbDetailElementName.Location = new System.Drawing.Point(160, 36);
            this.tbDetailElementName.Name = "tbDetailElementName";
            this.tbDetailElementName.Size = new System.Drawing.Size(224, 20);
            this.tbDetailElementName.TabIndex = 2;
            this.tbDetailElementName.Text = "textBox1";
            this.tbDetailElementName.TextChanged += new System.EventHandler(this.tbDetailElementName_TextChanged);
            // 
            // chkRenderDetails
            // 
            this.chkRenderDetails.Location = new System.Drawing.Point(16, 12);
            this.chkRenderDetails.Name = "chkRenderDetails";
            this.chkRenderDetails.Size = new System.Drawing.Size(160, 24);
            this.chkRenderDetails.TabIndex = 0;
            this.chkRenderDetails.Text = "Render Details in Output";
            this.chkRenderDetails.CheckedChanged += new System.EventHandler(this.chkRenderDetails_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Detail Collection Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Detail Element Name";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkFooterRepeat);
            this.groupBox3.Controls.Add(this.chkHeaderRepeat);
            this.groupBox3.Controls.Add(this.chkFooterRows);
            this.groupBox3.Controls.Add(this.chkHeaderRows);
            this.groupBox3.Controls.Add(this.chkDetails);
            this.groupBox3.Location = new System.Drawing.Point(24, 120);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(400, 64);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Include Table Rows";
            // 
            // chkFooterRows
            // 
            this.chkFooterRows.Location = new System.Drawing.Point(272, 13);
            this.chkFooterRows.Name = "chkFooterRows";
            this.chkFooterRows.Size = new System.Drawing.Size(104, 24);
            this.chkFooterRows.TabIndex = 2;
            this.chkFooterRows.Text = "Footer Rows";
            this.chkFooterRows.CheckedChanged += new System.EventHandler(this.chkRows_CheckedChanged);
            // 
            // chkHeaderRows
            // 
            this.chkHeaderRows.Location = new System.Drawing.Point(144, 13);
            this.chkHeaderRows.Name = "chkHeaderRows";
            this.chkHeaderRows.Size = new System.Drawing.Size(104, 24);
            this.chkHeaderRows.TabIndex = 1;
            this.chkHeaderRows.Text = "Header Rows";
            this.chkHeaderRows.CheckedChanged += new System.EventHandler(this.chkRows_CheckedChanged);
            // 
            // chkDetails
            // 
            this.chkDetails.Location = new System.Drawing.Point(16, 13);
            this.chkDetails.Name = "chkDetails";
            this.chkDetails.Size = new System.Drawing.Size(104, 24);
            this.chkDetails.TabIndex = 1;
            this.chkDetails.Text = "Detail Rows";
            this.chkDetails.CheckedChanged += new System.EventHandler(this.chkRows_CheckedChanged);
            // 
            // chkHeaderRepeat
            // 
            this.chkHeaderRepeat.Location = new System.Drawing.Point(144, 34);
            this.chkHeaderRepeat.Name = "chkHeaderRepeat";
            this.chkHeaderRepeat.Size = new System.Drawing.Size(122, 30);
            this.chkHeaderRepeat.TabIndex = 3;
            this.chkHeaderRepeat.Text = "Repeat header on new page";
            this.chkHeaderRepeat.CheckedChanged += new System.EventHandler(this.chkRows_CheckedChanged);
            // 
            // chkFooterRepeat
            // 
            this.chkFooterRepeat.Location = new System.Drawing.Point(272, 34);
            this.chkFooterRepeat.Name = "chkFooterRepeat";
            this.chkFooterRepeat.Size = new System.Drawing.Size(122, 30);
            this.chkFooterRepeat.TabIndex = 4;
            this.chkFooterRepeat.Text = "Repeat footer on new page";
            this.chkFooterRepeat.CheckedChanged += new System.EventHandler(this.chkRows_CheckedChanged);
            // 
            // TableCtl
            // 
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tbNoRows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbDataSet);
            this.Controls.Add(this.label2);
            this.Name = "TableCtl";
            this.Size = new System.Drawing.Size(472, 288);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
  
		public bool IsValid()
		{
			if (this.chkDetails.Checked || this.chkFooterRows.Checked || this.chkHeaderRows.Checked)
				return true;

			MessageBox.Show("Table must have at least one Header, Details or Footer row defined.", "Table");

			return false;
		}

		public void Apply()
		{
			// take information in control and apply to all the style nodes
			//  Only change information that has been marked as modified;
			//   this way when group is selected it is possible to change just
			//   the items you want and keep the rest the same.
				
			foreach (XmlNode riNode in this._ReportItems)
				ApplyChanges(riNode);

			// No more changes
			fNoRows = fDataSet = fPBBefore = fPBAfter = 
				fDetailElementName = fDetailCollectionName = fRenderDetails =
				fCheckRows = false;
		}

		public void ApplyChanges(XmlNode node)
		{
			if (fNoRows)
				_Draw.SetElement(node, "NoRows", this.tbNoRows.Text);
			if (fDataSet)
				_Draw.SetElement(node, "DataSetName", this.cbDataSet.Text);
			if (fPBBefore)
				_Draw.SetElement(node, "PageBreakAtStart", this.chkPBBefore.Checked? "true":"false");
			if (fPBAfter)
				_Draw.SetElement(node, "PageBreakAtEnd", this.chkPBAfter.Checked? "true":"false");
			if (fCheckRows)
			{
				if (this.chkDetails.Checked)
					CreateTableRow(node, "Details", false);
				else
					_Draw.RemoveElement(node, "Details");
				if (this.chkHeaderRows.Checked)
					CreateTableRow(node, "Header", chkHeaderRepeat.Checked);
				else
					_Draw.RemoveElement(node, "Header");
				if (this.chkFooterRows.Checked)
					CreateTableRow(node, "Footer", chkFooterRepeat.Checked);
				else
					_Draw.RemoveElement(node, "Footer");
			}
			if (fRenderDetails)
				_Draw.SetElement(node, "DetailDataElementOutput", this.chkRenderDetails.Checked? "Output":"NoOutput");
			if (this.fDetailElementName)
			{
				if (this.tbDetailElementName.Text.Length > 0)
					_Draw.SetElement(node, "DetailDataElementName", this.tbDetailElementName.Text);
				else
					_Draw.RemoveElement(node, "DetailDataElementName");
			}
			if (this.fDetailCollectionName)
			{
				if (this.tbDetailCollectionName.Text.Length > 0)
					_Draw.SetElement(node, "DetailDataCollectionName", this.tbDetailCollectionName.Text);
				else
					_Draw.RemoveElement(node, "DetailDataCollectionName");
			}
		}

		private void CreateTableRow(XmlNode tblNode, string elementName, bool bRepeatOnNewPage)
		{
			XmlNode node = _Draw.GetNamedChildNode(tblNode, elementName);
			if (node == null)
			{
				node = _Draw.CreateElement(tblNode, elementName, null);
				XmlNode tblRows = _Draw.CreateElement(node, "TableRows", null);
				_Draw.InsertTableRow(tblRows);
			}
            if (bRepeatOnNewPage)
                _Draw.SetElement(node, "RepeatOnNewPage", "true");
            else
                _Draw.RemoveElement(node, "RepeatOnNewPage");

			return;
		}

		private void cbDataSet_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fDataSet = true;
		}

		private void chkPBBefore_CheckedChanged(object sender, System.EventArgs e)
		{
			fPBBefore = true;
		}

		private void chkPBAfter_CheckedChanged(object sender, System.EventArgs e)
		{
			fPBAfter = true;
		}

		private void tbNoRows_TextChanged(object sender, System.EventArgs e)
		{
			fNoRows = true;
		}

		private void chkRows_CheckedChanged(object sender, System.EventArgs e)
		{
			this.fCheckRows = true;
            chkFooterRepeat.Enabled = chkFooterRows.Checked;
            chkHeaderRepeat.Enabled = chkHeaderRows.Checked;
		}

		private void chkRenderDetails_CheckedChanged(object sender, System.EventArgs e)
		{
			fRenderDetails = true;
		}

		private void tbDetailElementName_TextChanged(object sender, System.EventArgs e)
		{
			fDetailElementName = true;
		}

		private void tbDetailCollectionName_TextChanged(object sender, System.EventArgs e)
		{
			fDetailCollectionName = true;
		}

	}
}
