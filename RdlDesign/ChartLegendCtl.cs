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
	/// Summary description for ChartCtl.
	/// </summary>
	internal class ChartLegendCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		bool fVisible, fLayout, fPosition, fInsidePlotArea;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbPosition;
		private System.Windows.Forms.ComboBox cbLayout;
		private System.Windows.Forms.CheckBox chkVisible;
		private System.Windows.Forms.CheckBox chkInsidePlotArea;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal ChartLegendCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
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
			XmlNode node = _ReportItems[0];

			this.cbPosition.Text = _Draw.GetElementValue(node, "Position", "RightTop");
			this.cbLayout.Text = _Draw.GetElementValue(node, "Layout", "Column");
			this.chkVisible.Checked = _Draw.GetElementValue(node, "Visible", "false").ToLower() == "true"? true: false;
			this.chkInsidePlotArea.Checked = _Draw.GetElementValue(node, "InsidePlotArea", "false").ToLower() == "true"? true: false;

			fVisible = fLayout = fPosition = fInsidePlotArea = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartLegendCtl));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbPosition = new System.Windows.Forms.ComboBox();
			this.cbLayout = new System.Windows.Forms.ComboBox();
			this.chkVisible = new System.Windows.Forms.CheckBox();
			this.chkInsidePlotArea = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// cbPosition
			// 
			resources.ApplyResources(this.cbPosition, "cbPosition");
			this.cbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPosition.Items.AddRange(new object[] {
            resources.GetString("cbPosition.Items"),
            resources.GetString("cbPosition.Items1"),
            resources.GetString("cbPosition.Items2"),
            resources.GetString("cbPosition.Items3"),
            resources.GetString("cbPosition.Items4"),
            resources.GetString("cbPosition.Items5"),
            resources.GetString("cbPosition.Items6"),
            resources.GetString("cbPosition.Items7"),
            resources.GetString("cbPosition.Items8"),
            resources.GetString("cbPosition.Items9"),
            resources.GetString("cbPosition.Items10"),
            resources.GetString("cbPosition.Items11")});
			this.cbPosition.Name = "cbPosition";
			this.cbPosition.SelectedIndexChanged += new System.EventHandler(this.cbPosition_SelectedIndexChanged);
			// 
			// cbLayout
			// 
			resources.ApplyResources(this.cbLayout, "cbLayout");
			this.cbLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLayout.Items.AddRange(new object[] {
            resources.GetString("cbLayout.Items"),
            resources.GetString("cbLayout.Items1"),
            resources.GetString("cbLayout.Items2")});
			this.cbLayout.Name = "cbLayout";
			this.cbLayout.SelectedIndexChanged += new System.EventHandler(this.cbLayout_SelectedIndexChanged);
			// 
			// chkVisible
			// 
			resources.ApplyResources(this.chkVisible, "chkVisible");
			this.chkVisible.Name = "chkVisible";
			this.chkVisible.CheckedChanged += new System.EventHandler(this.chkVisible_CheckedChanged);
			// 
			// chkInsidePlotArea
			// 
			resources.ApplyResources(this.chkInsidePlotArea, "chkInsidePlotArea");
			this.chkInsidePlotArea.Name = "chkInsidePlotArea";
			this.chkInsidePlotArea.CheckedChanged += new System.EventHandler(this.chkInsidePlotArea_CheckedChanged);
			// 
			// ChartLegendCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.chkInsidePlotArea);
			this.Controls.Add(this.chkVisible);
			this.Controls.Add(this.cbLayout);
			this.Controls.Add(this.cbPosition);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "ChartLegendCtl";
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
			fVisible = fLayout = fPosition = fInsidePlotArea = false;
		}

		public void ApplyChanges(XmlNode node)
		{
			if (fVisible)
			{
				_Draw.SetElement(node, "Visible", this.chkVisible.Checked? "true": "false");
			}
			if (fLayout)
			{
				_Draw.SetElement(node, "Layout", this.cbLayout.Text);
			}
			if (fPosition)
			{
				_Draw.SetElement(node, "Position", this.cbPosition.Text);
			}
			if (fInsidePlotArea)
			{
				_Draw.SetElement(node, "InsidePlotArea", this.chkInsidePlotArea.Checked? "true": "false");
			}
		}

		private void cbPosition_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fPosition = true;
		}

		private void cbLayout_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fLayout = true;
		}

		private void chkVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			fVisible = true;
		}

		private void chkInsidePlotArea_CheckedChanged(object sender, System.EventArgs e)
		{
			fInsidePlotArea = true;
		}

	}
}
