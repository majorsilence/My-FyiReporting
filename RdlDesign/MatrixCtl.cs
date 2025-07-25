/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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

namespace Majorsilence.Reporting.RdlDesign
{
	/// <summary>
	/// Summary description for ReportCtl.
	/// </summary>
	internal class MatrixCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		bool fDataSet, fPBBefore, fPBAfter, fNoRows, fCellDataElementOutput, fCellDataElementName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbDataSet;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkPBBefore;
		private System.Windows.Forms.CheckBox chkPBAfter;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbNoRows;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkCellContents;
		private System.Windows.Forms.TextBox tbCellDataElementName;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public MatrixCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
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
			this.chkCellContents.Checked = _Draw.GetElementValue(riNode, "CellDataElementOutput", "Output")=="Output"?true:false;
			this.tbCellDataElementName.Text =  _Draw.GetElementValue(riNode, "CellDataElementName", "Cell");

			fNoRows = fDataSet = fPBBefore = fPBAfter = fCellDataElementOutput = fCellDataElementName = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixCtl));
			this.label2 = new System.Windows.Forms.Label();
			this.cbDataSet = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkPBAfter = new System.Windows.Forms.CheckBox();
			this.chkPBBefore = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbNoRows = new System.Windows.Forms.TextBox();
			this.tbCellDataElementName = new System.Windows.Forms.TextBox();
			this.chkCellContents = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// cbDataSet
			// 
			resources.ApplyResources(this.cbDataSet, "cbDataSet");
			this.cbDataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataSet.Name = "cbDataSet";
			this.cbDataSet.SelectedIndexChanged += new System.EventHandler(this.cbDataSet_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.chkPBAfter);
			this.groupBox1.Controls.Add(this.chkPBBefore);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// chkPBAfter
			// 
			resources.ApplyResources(this.chkPBAfter, "chkPBAfter");
			this.chkPBAfter.Name = "chkPBAfter";
			this.chkPBAfter.CheckedChanged += new System.EventHandler(this.chkPBAfter_CheckedChanged);
			// 
			// chkPBBefore
			// 
			resources.ApplyResources(this.chkPBBefore, "chkPBBefore");
			this.chkPBBefore.Name = "chkPBBefore";
			this.chkPBBefore.CheckedChanged += new System.EventHandler(this.chkPBBefore_CheckedChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tbNoRows
			// 
			resources.ApplyResources(this.tbNoRows, "tbNoRows");
			this.tbNoRows.Name = "tbNoRows";
			this.tbNoRows.TextChanged += new System.EventHandler(this.tbNoRows_TextChanged);
			// 
			// tbCellDataElementName
			// 
			resources.ApplyResources(this.tbCellDataElementName, "tbCellDataElementName");
			this.tbCellDataElementName.Name = "tbCellDataElementName";
			this.tbCellDataElementName.TextChanged += new System.EventHandler(this.tbCellDataElementName_TextChanged);
			// 
			// chkCellContents
			// 
			resources.ApplyResources(this.chkCellContents, "chkCellContents");
			this.chkCellContents.Name = "chkCellContents";
			this.chkCellContents.CheckedChanged += new System.EventHandler(this.chkCellContents_CheckedChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.tbCellDataElementName);
			this.groupBox2.Controls.Add(this.chkCellContents);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// MatrixCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.tbNoRows);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cbDataSet);
			this.Controls.Add(this.label2);
			this.Name = "MatrixCtl";
			this.groupBox1.ResumeLayout(false);
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
			// take information in control and apply to all the style nodes
			//  Only change information that has been marked as modified;
			//   this way when group is selected it is possible to change just
			//   the items you want and keep the rest the same.
				
			foreach (XmlNode riNode in this._ReportItems)
				ApplyChanges(riNode);

			// No more changes
			fNoRows = fDataSet = fPBBefore = fPBAfter= fCellDataElementOutput = fCellDataElementName = false;
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
			if (fCellDataElementOutput)
				_Draw.SetElement(node, "CellDataElementOutput", this.chkCellContents.Checked? "Output":"NoOutput");
			if (fCellDataElementName)
			{
				if (this.tbCellDataElementName.Text.Length > 0)
					_Draw.SetElement(node, "CellDataElementName", this.tbCellDataElementName.Text);
				else
					_Draw.RemoveElement(node, "CellDataElementName");
			}
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

		private void tbCellDataElementName_TextChanged(object sender, System.EventArgs e)
		{
			fCellDataElementName = true;
		}

		private void chkCellContents_CheckedChanged(object sender, System.EventArgs e)
		{
			this.fCellDataElementOutput = true;
		}
	}
}
