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
using System.IO;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for StyleCtl.
	/// </summary>
	internal class InteractivityCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
        private List<DrillParameter> _DrillParameters;
		// flags for controlling whether syntax changed for a particular property
		private bool fBookmark, fAction, fHidden, fToggle;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox grpBoxVisibility;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbBookmark;
		private System.Windows.Forms.RadioButton rbHyperlink;
		private System.Windows.Forms.RadioButton rbBookmarkLink;
		private System.Windows.Forms.RadioButton rbDrillthrough;
		private System.Windows.Forms.TextBox tbHyperlink;
		private System.Windows.Forms.TextBox tbBookmarkLink;
		private System.Windows.Forms.TextBox tbDrillthrough;
		private System.Windows.Forms.Button bParameters;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbHidden;
		private System.Windows.Forms.ComboBox cbToggle;
		private System.Windows.Forms.RadioButton rbNoAction;
		private System.Windows.Forms.Button bDrillthrough;
		private System.Windows.Forms.Button bHidden;
		private System.Windows.Forms.Button bBookmarkLink;
		private System.Windows.Forms.Button bHyperlink;
		private System.Windows.Forms.Button bBookmark;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal InteractivityCtl(DesignXmlDraw dxDraw, List<XmlNode> reportItems)
		{
			_ReportItems = reportItems;
			_Draw = dxDraw;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues(_ReportItems[0]);			
		}

		private void InitValues(XmlNode node)
		{
			
			tbBookmark.Text = _Draw.GetElementValue(node, "Bookmark", "");
			// Handle Action definition
			XmlNode aNode = _Draw.GetNamedChildNode(node, "Action");
			if (aNode == null)
				rbNoAction.Checked = true;
			else
			{
				XmlNode vLink = _Draw.GetNamedChildNode(aNode, "Hyperlink");
				if (vLink != null)
				{	// Hyperlink specified
					rbHyperlink.Checked = true;
					tbHyperlink.Text = vLink.InnerText;
				}
				else
				{
					vLink = _Draw.GetNamedChildNode(aNode, "Drillthrough");
					if (vLink != null)
					{	// Drillthrough specified
						rbDrillthrough.Checked = true;
						tbDrillthrough.Text =  _Draw.GetElementValue(vLink, "ReportName", "");
                        _DrillParameters = new List<DrillParameter>();
						XmlNode pNodes = _Draw.GetNamedChildNode(vLink, "Parameters");
						if (pNodes != null)
						{
							foreach (XmlNode pNode in pNodes.ChildNodes)
							{
								if (pNode.Name != "Parameter")
									continue;
								string name = _Draw.GetElementAttribute(pNode, "Name", "");
								string pvalue = _Draw.GetElementValue(pNode, "Value", "");
								string omit = _Draw.GetElementValue(pNode, "Omit", "false");
								DrillParameter dp = new DrillParameter(name, pvalue, omit);
								_DrillParameters.Add(dp);
							}
						}
					}
					else
					{	
						vLink = _Draw.GetNamedChildNode(aNode, "BookmarkLink");
						if (vLink != null)
						{	// BookmarkLink specified
							rbBookmarkLink.Checked = true;
							this.tbBookmarkLink.Text = vLink.InnerText;
						}
					}
				}
			}
			
			// Handle Visiblity definition
			XmlNode visNode = _Draw.GetNamedChildNode(node, "Visibility");
			if (visNode != null)
			{
				XmlNode hNode = _Draw.GetNamedChildNode(node, "Visibility");
				this.tbHidden.Text = _Draw.GetElementValue(visNode, "Hidden", "");
				this.cbToggle.Text = _Draw.GetElementValue(visNode, "ToggleItem", "");
			}
			IEnumerable list = _Draw.GetReportItems("//Textbox");
			if (list != null)
			{
				foreach (XmlNode tNode in list)
				{
					XmlAttribute name = tNode.Attributes["Name"];
					if (name != null && name.Value != null && name.Value.Length > 0)
						cbToggle.Items.Add(name.Value);
				}
			}
			// nothing has changed now
			fBookmark = fAction = fHidden = fToggle = false;
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
			this.grpBoxVisibility = new System.Windows.Forms.GroupBox();
			this.bHidden = new System.Windows.Forms.Button();
			this.cbToggle = new System.Windows.Forms.ComboBox();
			this.tbHidden = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.bBookmarkLink = new System.Windows.Forms.Button();
			this.bHyperlink = new System.Windows.Forms.Button();
			this.rbNoAction = new System.Windows.Forms.RadioButton();
			this.bParameters = new System.Windows.Forms.Button();
			this.bDrillthrough = new System.Windows.Forms.Button();
			this.tbDrillthrough = new System.Windows.Forms.TextBox();
			this.tbBookmarkLink = new System.Windows.Forms.TextBox();
			this.tbHyperlink = new System.Windows.Forms.TextBox();
			this.rbDrillthrough = new System.Windows.Forms.RadioButton();
			this.rbBookmarkLink = new System.Windows.Forms.RadioButton();
			this.rbHyperlink = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.tbBookmark = new System.Windows.Forms.TextBox();
			this.bBookmark = new System.Windows.Forms.Button();
			this.grpBoxVisibility.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpBoxVisibility
			// 
			this.grpBoxVisibility.Controls.Add(this.bHidden);
			this.grpBoxVisibility.Controls.Add(this.cbToggle);
			this.grpBoxVisibility.Controls.Add(this.tbHidden);
			this.grpBoxVisibility.Controls.Add(this.label3);
			this.grpBoxVisibility.Controls.Add(this.label2);
			this.grpBoxVisibility.Location = new System.Drawing.Point(8, 152);
			this.grpBoxVisibility.Name = "grpBoxVisibility";
			this.grpBoxVisibility.Size = new System.Drawing.Size(432, 80);
			this.grpBoxVisibility.TabIndex = 1;
			this.grpBoxVisibility.TabStop = false;
			this.grpBoxVisibility.Text = "Visibility";
			// 
			// bHidden
			// 
			this.bHidden.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bHidden.Location = new System.Drawing.Point(400, 26);
			this.bHidden.Name = "bHidden";
			this.bHidden.Size = new System.Drawing.Size(22, 16);
			this.bHidden.TabIndex = 1;
			this.bHidden.Tag = "visibility";
			this.bHidden.Text = "fx";
			this.bHidden.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bHidden.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// cbToggle
			// 
			this.cbToggle.Location = new System.Drawing.Point(168, 48);
			this.cbToggle.Name = "cbToggle";
			this.cbToggle.Size = new System.Drawing.Size(152, 21);
			this.cbToggle.TabIndex = 2;
			this.cbToggle.TextChanged += new System.EventHandler(this.cbToggle_SelectedIndexChanged);
			this.cbToggle.SelectedIndexChanged += new System.EventHandler(this.cbToggle_SelectedIndexChanged);
			// 
			// tbHidden
			// 
			this.tbHidden.Location = new System.Drawing.Point(168, 24);
			this.tbHidden.Name = "tbHidden";
			this.tbHidden.Size = new System.Drawing.Size(224, 20);
			this.tbHidden.TabIndex = 0;
			this.tbHidden.Text = "";
			this.tbHidden.TextChanged += new System.EventHandler(this.tbHidden_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(152, 23);
			this.label3.TabIndex = 1;
			this.label3.Text = "Toggle Item (Textbox name)";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Hidden (initial visibility)";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.bBookmarkLink);
			this.groupBox1.Controls.Add(this.bHyperlink);
			this.groupBox1.Controls.Add(this.rbNoAction);
			this.groupBox1.Controls.Add(this.bParameters);
			this.groupBox1.Controls.Add(this.bDrillthrough);
			this.groupBox1.Controls.Add(this.tbDrillthrough);
			this.groupBox1.Controls.Add(this.tbBookmarkLink);
			this.groupBox1.Controls.Add(this.tbHyperlink);
			this.groupBox1.Controls.Add(this.rbDrillthrough);
			this.groupBox1.Controls.Add(this.rbBookmarkLink);
			this.groupBox1.Controls.Add(this.rbHyperlink);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(432, 136);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Action";
			// 
			// bBookmarkLink
			// 
			this.bBookmarkLink.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bBookmarkLink.Location = new System.Drawing.Point(400, 80);
			this.bBookmarkLink.Name = "bBookmarkLink";
			this.bBookmarkLink.Size = new System.Drawing.Size(22, 16);
			this.bBookmarkLink.TabIndex = 3;
			this.bBookmarkLink.Tag = "bookmarklink";
			this.bBookmarkLink.Text = "fx";
			this.bBookmarkLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bBookmarkLink.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// bHyperlink
			// 
			this.bHyperlink.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bHyperlink.Location = new System.Drawing.Point(400, 50);
			this.bHyperlink.Name = "bHyperlink";
			this.bHyperlink.Size = new System.Drawing.Size(22, 16);
			this.bHyperlink.TabIndex = 1;
			this.bHyperlink.Tag = "hyperlink";
			this.bHyperlink.Text = "fx";
			this.bHyperlink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bHyperlink.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// rbNoAction
			// 
			this.rbNoAction.Location = new System.Drawing.Point(16, 16);
			this.rbNoAction.Name = "rbNoAction";
			this.rbNoAction.TabIndex = 5;
			this.rbNoAction.Text = "None";
			this.rbNoAction.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// bParameters
			// 
			this.bParameters.Location = new System.Drawing.Point(344, 104);
			this.bParameters.Name = "bParameters";
			this.bParameters.Size = new System.Drawing.Size(80, 23);
			this.bParameters.TabIndex = 6;
			this.bParameters.Text = "Parameters...";
			this.bParameters.Click += new System.EventHandler(this.bParameters_Click);
			// 
			// bDrillthrough
			// 
			this.bDrillthrough.Location = new System.Drawing.Point(312, 104);
			this.bDrillthrough.Name = "bDrillthrough";
			this.bDrillthrough.Size = new System.Drawing.Size(24, 23);
			this.bDrillthrough.TabIndex = 5;
			this.bDrillthrough.Text = "...";
			this.bDrillthrough.Click += new System.EventHandler(this.bDrillthrough_Click);
			// 
			// tbDrillthrough
			// 
			this.tbDrillthrough.Location = new System.Drawing.Point(128, 104);
			this.tbDrillthrough.Name = "tbDrillthrough";
			this.tbDrillthrough.Size = new System.Drawing.Size(176, 20);
			this.tbDrillthrough.TabIndex = 4;
			this.tbDrillthrough.Text = "";
			this.tbDrillthrough.TextChanged += new System.EventHandler(this.tbAction_TextChanged);
			// 
			// tbBookmarkLink
			// 
			this.tbBookmarkLink.Location = new System.Drawing.Point(128, 76);
			this.tbBookmarkLink.Name = "tbBookmarkLink";
			this.tbBookmarkLink.Size = new System.Drawing.Size(264, 20);
			this.tbBookmarkLink.TabIndex = 2;
			this.tbBookmarkLink.Text = "";
			this.tbBookmarkLink.TextChanged += new System.EventHandler(this.tbAction_TextChanged);
			// 
			// tbHyperlink
			// 
			this.tbHyperlink.Location = new System.Drawing.Point(128, 47);
			this.tbHyperlink.Name = "tbHyperlink";
			this.tbHyperlink.Size = new System.Drawing.Size(264, 20);
			this.tbHyperlink.TabIndex = 0;
			this.tbHyperlink.Text = "";
			this.tbHyperlink.TextChanged += new System.EventHandler(this.tbAction_TextChanged);
			// 
			// rbDrillthrough
			// 
			this.rbDrillthrough.Location = new System.Drawing.Point(16, 102);
			this.rbDrillthrough.Name = "rbDrillthrough";
			this.rbDrillthrough.TabIndex = 2;
			this.rbDrillthrough.Text = "Drill Through";
			this.rbDrillthrough.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// rbBookmarkLink
			// 
			this.rbBookmarkLink.Location = new System.Drawing.Point(16, 74);
			this.rbBookmarkLink.Name = "rbBookmarkLink";
			this.rbBookmarkLink.TabIndex = 1;
			this.rbBookmarkLink.Text = "Bookmark Link";
			this.rbBookmarkLink.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// rbHyperlink
			// 
			this.rbHyperlink.Location = new System.Drawing.Point(16, 45);
			this.rbHyperlink.Name = "rbHyperlink";
			this.rbHyperlink.TabIndex = 0;
			this.rbHyperlink.Text = "Hyperlink";
			this.rbHyperlink.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 256);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Bookmark";
			// 
			// tbBookmark
			// 
			this.tbBookmark.Location = new System.Drawing.Point(88, 254);
			this.tbBookmark.Name = "tbBookmark";
			this.tbBookmark.Size = new System.Drawing.Size(312, 20);
			this.tbBookmark.TabIndex = 3;
			this.tbBookmark.Text = "";
			// 
			// bBookmark
			// 
			this.bBookmark.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bBookmark.Location = new System.Drawing.Point(408, 258);
			this.bBookmark.Name = "bBookmark";
			this.bBookmark.Size = new System.Drawing.Size(22, 16);
			this.bBookmark.TabIndex = 4;
			this.bBookmark.Tag = "bookmark";
			this.bBookmark.Text = "fx";
			this.bBookmark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bBookmark.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// InteractivityCtl
			// 
			this.Controls.Add(this.bBookmark);
			this.Controls.Add(this.tbBookmark);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grpBoxVisibility);
			this.Name = "InteractivityCtl";
			this.Size = new System.Drawing.Size(472, 288);
			this.grpBoxVisibility.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
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

			// nothing has changed now
			fBookmark = fAction = fHidden = fToggle = false;
		}

		private void ApplyChanges(XmlNode rNode)
		{
			if (fBookmark)
				_Draw.SetElement(rNode, "Bookmark", tbBookmark.Text);

			if (fHidden || fToggle)
			{
				XmlNode visNode = _Draw.SetElement(rNode, "Visibility", null);

				if (fHidden)
					_Draw.SetElement(visNode, "Hidden", tbHidden.Text); 
				if (fToggle)
					_Draw.SetElement(visNode, "ToggleItem", this.cbToggle.Text); 
			}

			if (fAction)
			{
				XmlNode aNode;
				if (this.rbHyperlink.Checked)
				{
					aNode = _Draw.SetElement(rNode, "Action", null);
					_Draw.RemoveElement(aNode, "Drillthrough");	
					_Draw.RemoveElement(aNode, "BookmarkLink");	
					_Draw.SetElement(aNode, "Hyperlink", tbHyperlink.Text); 
				}
				else if (this.rbDrillthrough.Checked)
				{
					aNode = _Draw.SetElement(rNode, "Action", null);
					_Draw.RemoveElement(aNode, "Hyperlink");	
					_Draw.RemoveElement(aNode, "BookmarkLink");	
					XmlNode dNode = _Draw.SetElement(aNode, "Drillthrough", null);
					_Draw.SetElement(dNode, "ReportName", tbDrillthrough.Text); 
					// Now do parameters
					_Draw.RemoveElement(dNode, "Parameters");	// Get rid of prior values
					if (this._DrillParameters != null && _DrillParameters.Count > 0)
					{
						XmlNode pNodes = _Draw.SetElement(dNode, "Parameters", null);
						foreach (DrillParameter dp in _DrillParameters)
						{
							XmlNode p = _Draw.SetElement(pNodes, "Parameter", null);
							_Draw.SetElementAttribute(p, "Name", dp.ParameterName);
							_Draw.SetElement(p, "Value", dp.ParameterValue);
							if (dp.ParameterOmit != null && dp.ParameterOmit.Length > 0)
								_Draw.SetElement(p, "Omit", dp.ParameterOmit);
						}
					}
				}
				else if (this.rbBookmarkLink.Checked)
				{
					aNode = _Draw.SetElement(rNode, "Action", null);
					_Draw.RemoveElement(aNode, "Drillthrough");	
					_Draw.RemoveElement(aNode, "Hyperlink");	
					_Draw.SetElement(aNode, "BookmarkLink", tbBookmarkLink.Text); 
				}
				else	// assume no action
				{
					_Draw.RemoveElement(rNode, "Action");	
				}
			}
		}

		private void tbBookmark_TextChanged(object sender, System.EventArgs e)
		{
			fBookmark = true;
		}

		private void rbAction_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbHyperlink.Checked)
			{
				tbHyperlink.Enabled = bHyperlink.Enabled = true;
				tbBookmarkLink.Enabled = bBookmarkLink.Enabled = false;
				tbDrillthrough.Enabled = false;
				bDrillthrough.Enabled = false;
				bParameters.Enabled = false;

			}
			else if (this.rbDrillthrough.Checked)
			{
				tbHyperlink.Enabled = bHyperlink.Enabled = false;
				tbBookmarkLink.Enabled = bBookmarkLink.Enabled = false;
				tbDrillthrough.Enabled = true;
				bDrillthrough.Enabled = true;
				bParameters.Enabled = true;
			}
			else if (this.rbBookmarkLink.Checked)
			{
				tbHyperlink.Enabled = bHyperlink.Enabled = false;
				tbBookmarkLink.Enabled = bBookmarkLink.Enabled = true;
				tbDrillthrough.Enabled = false;
				bDrillthrough.Enabled = false;
				bParameters.Enabled = false;
			}
			else	// assume no action
			{
				tbHyperlink.Enabled = bHyperlink.Enabled = false;
				tbBookmarkLink.Enabled = bBookmarkLink.Enabled = false;
				tbDrillthrough.Enabled = false;
				bDrillthrough.Enabled = false;
				bParameters.Enabled = false;
			}
			fAction = true;
		}

		private void tbAction_TextChanged(object sender, System.EventArgs e)
		{
			fAction = true;
		}

		private void tbHidden_TextChanged(object sender, System.EventArgs e)
		{
			fHidden = true;
		}

		private void cbToggle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fToggle = true;
		}

		private void bDrillthrough_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Report files (*.rdl)|*.rdl" +
                "|All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.FileName = "*.rdl";

			ofd.Title = "Specify Report File Name";
			ofd.DefaultExt = "rdl";
			ofd.AddExtension = true;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileNameWithoutExtension(ofd.FileName);

                    tbDrillthrough.Text = file;
                }
            }
            finally
            {
                ofd.Dispose();
            }
		}

		private void bParameters_Click(object sender, System.EventArgs e)
		{
			DrillParametersDialog dpd = new DrillParametersDialog(this.tbDrillthrough.Text, _DrillParameters);
            try
            {
                if (dpd.ShowDialog(this) != DialogResult.OK)
                    return;
                tbDrillthrough.Text = dpd.DrillthroughReport;
                _DrillParameters = dpd.DrillParameters;
                fAction = true;
            }
            finally
            {
                dpd.Dispose();
            }
		}

		private void bExpr_Click(object sender, System.EventArgs e)
		{
			Button b = sender as Button;
			if (b == null)
				return;
			Control c = null;
			switch (b.Tag as string)
			{
				case "bookmark":
					c = tbBookmark;
 					break;
				case "bookmarklink":
					c = tbBookmarkLink;
 					break;
				case "hyperlink":
					c = tbHyperlink;
                    break;
				case "visibility":
					c = tbHidden;
                    break;
			}

			if (c == null)
				return;

			XmlNode sNode = _ReportItems[0];

			DialogExprEditor ee = new DialogExprEditor(_Draw, c.Text, sNode);
            try
            {
                DialogResult dr = ee.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    c.Text = ee.Expression;
                    if ((string)(b.Tag) == "bookmark")
                        fBookmark = true;
                    else
                        fAction = true;
                }
            }
            finally
            {
                ee.Dispose();
            }
			return;
		}

	}
}
