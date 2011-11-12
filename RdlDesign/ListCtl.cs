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
	internal class ListCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		bool fDataSet, fPBBefore, fPBAfter, fNoRows, fDataInstanceElementOutput, fDataInstanceName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbDataSet;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkPBBefore;
		private System.Windows.Forms.CheckBox chkPBAfter;
		private System.Windows.Forms.Button bGroups;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbNoRows;
		private System.Windows.Forms.TextBox tbDataInstanceName;
		private System.Windows.Forms.CheckBox chkXmlInstances;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal ListCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
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
			this.chkXmlInstances.Checked = _Draw.GetElementValue(riNode, "DataInstanceElementOutput", "Output")=="Output"?true:false;
			this.tbDataInstanceName.Text =  _Draw.GetElementValue(riNode, "DataInstanceName", "Item");

			fNoRows = fDataSet = fPBBefore = fPBAfter = fDataInstanceElementOutput = fDataInstanceName = false;
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
			this.bGroups = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbNoRows = new System.Windows.Forms.TextBox();
			this.tbDataInstanceName = new System.Windows.Forms.TextBox();
			this.chkXmlInstances = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "DataSet Name";
			// 
			// cbDataSet
			// 
			this.cbDataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataSet.Location = new System.Drawing.Point(120, 16);
			this.cbDataSet.Name = "cbDataSet";
			this.cbDataSet.Size = new System.Drawing.Size(304, 21);
			this.cbDataSet.TabIndex = 1;
			this.cbDataSet.SelectedIndexChanged += new System.EventHandler(this.cbDataSet_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkPBAfter);
			this.groupBox1.Controls.Add(this.chkPBBefore);
			this.groupBox1.Location = new System.Drawing.Point(24, 88);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 48);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Page Breaks";
			// 
			// chkPBAfter
			// 
			this.chkPBAfter.Location = new System.Drawing.Point(192, 16);
			this.chkPBAfter.Name = "chkPBAfter";
			this.chkPBAfter.Size = new System.Drawing.Size(128, 24);
			this.chkPBAfter.TabIndex = 1;
			this.chkPBAfter.Text = "Insert after List";
			this.chkPBAfter.CheckedChanged += new System.EventHandler(this.chkPBAfter_CheckedChanged);
			// 
			// chkPBBefore
			// 
			this.chkPBBefore.Location = new System.Drawing.Point(16, 16);
			this.chkPBBefore.Name = "chkPBBefore";
			this.chkPBBefore.Size = new System.Drawing.Size(128, 24);
			this.chkPBBefore.TabIndex = 0;
			this.chkPBBefore.Text = "Insert before List";
			this.chkPBBefore.CheckedChanged += new System.EventHandler(this.chkPBBefore_CheckedChanged);
			// 
			// bGroups
			// 
			this.bGroups.Location = new System.Drawing.Point(24, 240);
			this.bGroups.Name = "bGroups";
			this.bGroups.Size = new System.Drawing.Size(120, 23);
			this.bGroups.TabIndex = 6;
			this.bGroups.Text = "Group Expressions...";
			this.bGroups.Click += new System.EventHandler(this.bGroups_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "No rows message";
			// 
			// tbNoRows
			// 
			this.tbNoRows.Location = new System.Drawing.Point(120, 48);
			this.tbNoRows.Name = "tbNoRows";
			this.tbNoRows.Size = new System.Drawing.Size(304, 20);
			this.tbNoRows.TabIndex = 3;
			this.tbNoRows.Text = "textBox1";
			this.tbNoRows.TextChanged += new System.EventHandler(this.tbNoRows_TextChanged);
			// 
			// tbDataInstanceName
			// 
			this.tbDataInstanceName.Location = new System.Drawing.Point(146, 44);
			this.tbDataInstanceName.Name = "tbDataInstanceName";
			this.tbDataInstanceName.Size = new System.Drawing.Size(238, 20);
			this.tbDataInstanceName.TabIndex = 2;
			this.tbDataInstanceName.Text = "textBox1";
			this.tbDataInstanceName.TextChanged += new System.EventHandler(this.tbDataInstanceName_TextChanged);
			// 
			// chkXmlInstances
			// 
			this.chkXmlInstances.Location = new System.Drawing.Point(16, 16);
			this.chkXmlInstances.Name = "chkXmlInstances";
			this.chkXmlInstances.Size = new System.Drawing.Size(160, 24);
			this.chkXmlInstances.TabIndex = 0;
			this.chkXmlInstances.Text = "Render list instances";
			this.chkXmlInstances.CheckedChanged += new System.EventHandler(this.chkXmlInstances_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 16);
			this.label3.TabIndex = 1;
			this.label3.Text = "Data Instance Name";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.tbDataInstanceName);
			this.groupBox2.Controls.Add(this.chkXmlInstances);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new System.Drawing.Point(24, 152);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(400, 72);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "XML";
			// 
			// ListCtl
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.tbNoRows);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bGroups);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cbDataSet);
			this.Controls.Add(this.label2);
			this.Name = "ListCtl";
			this.Size = new System.Drawing.Size(472, 288);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

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
			fNoRows = fDataSet = fPBBefore = fPBAfter= fDataInstanceElementOutput = fDataInstanceName = false;
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
			if (fDataInstanceElementOutput)
				_Draw.SetElement(node, "DataInstanceElementOutput", this.chkXmlInstances.Checked? "Output":"NoOutput");
			if (fDataInstanceName)
			{
				if (this.tbDataInstanceName.Text.Length > 0)
					_Draw.SetElement(node, "DataInstanceName", this.tbDataInstanceName.Text);
				else
					_Draw.RemoveElement(node, "DataInstanceName");
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

		private void bGroups_Click(object sender, System.EventArgs e)
		{
			PropertyDialog pd = new PropertyDialog(_Draw, _ReportItems, PropertyTypeEnum.Grouping);
            try
            {
                DialogResult dr = pd.ShowDialog();
                if (pd.Changed || dr == DialogResult.OK)
                {
                    //				_DrawPanel.Invalidate();   TODO need to force change somehow?????
                }
            }
            finally
            {
                pd.Dispose();
            }
		}

		private void chkXmlInstances_CheckedChanged(object sender, System.EventArgs e)
		{
			fDataInstanceElementOutput = true;
		}

		private void tbDataInstanceName_TextChanged(object sender, System.EventArgs e)
		{
			fDataInstanceName = true;
		}
	}
}
