
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Majorsilence.Reporting.RdlDesign.Resources;

namespace Majorsilence.Reporting.RdlDesign
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InteractivityCtl));
            this.DoubleBuffered = true;
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
			resources.ApplyResources(this.grpBoxVisibility, "grpBoxVisibility");
			this.grpBoxVisibility.Controls.Add(this.bHidden);
			this.grpBoxVisibility.Controls.Add(this.cbToggle);
			this.grpBoxVisibility.Controls.Add(this.tbHidden);
			this.grpBoxVisibility.Controls.Add(this.label3);
			this.grpBoxVisibility.Controls.Add(this.label2);
			this.grpBoxVisibility.Name = "grpBoxVisibility";
			this.grpBoxVisibility.TabStop = false;
			// 
			// bHidden
			// 
			resources.ApplyResources(this.bHidden, "bHidden");
			this.bHidden.Name = "bHidden";
			this.bHidden.Tag = "visibility";
			this.bHidden.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// cbToggle
			// 
			resources.ApplyResources(this.cbToggle, "cbToggle");
			this.cbToggle.Name = "cbToggle";
			this.cbToggle.SelectedIndexChanged += new System.EventHandler(this.cbToggle_SelectedIndexChanged);
			this.cbToggle.TextChanged += new System.EventHandler(this.cbToggle_SelectedIndexChanged);
			// 
			// tbHidden
			// 
			resources.ApplyResources(this.tbHidden, "tbHidden");
			this.tbHidden.Name = "tbHidden";
			this.tbHidden.TextChanged += new System.EventHandler(this.tbHidden_TextChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
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
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// bBookmarkLink
			// 
			resources.ApplyResources(this.bBookmarkLink, "bBookmarkLink");
			this.bBookmarkLink.Name = "bBookmarkLink";
			this.bBookmarkLink.Tag = "bookmarklink";
			this.bBookmarkLink.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// bHyperlink
			// 
			resources.ApplyResources(this.bHyperlink, "bHyperlink");
			this.bHyperlink.Name = "bHyperlink";
			this.bHyperlink.Tag = "hyperlink";
			this.bHyperlink.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// rbNoAction
			// 
			resources.ApplyResources(this.rbNoAction, "rbNoAction");
			this.rbNoAction.Name = "rbNoAction";
			this.rbNoAction.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// bParameters
			// 
			resources.ApplyResources(this.bParameters, "bParameters");
			this.bParameters.Name = "bParameters";
			this.bParameters.Click += new System.EventHandler(this.bParameters_Click);
			// 
			// bDrillthrough
			// 
			resources.ApplyResources(this.bDrillthrough, "bDrillthrough");
			this.bDrillthrough.Name = "bDrillthrough";
			this.bDrillthrough.Click += new System.EventHandler(this.bDrillthrough_Click);
			// 
			// tbDrillthrough
			// 
			resources.ApplyResources(this.tbDrillthrough, "tbDrillthrough");
			this.tbDrillthrough.Name = "tbDrillthrough";
			this.tbDrillthrough.TextChanged += new System.EventHandler(this.tbAction_TextChanged);
			// 
			// tbBookmarkLink
			// 
			resources.ApplyResources(this.tbBookmarkLink, "tbBookmarkLink");
			this.tbBookmarkLink.Name = "tbBookmarkLink";
			this.tbBookmarkLink.TextChanged += new System.EventHandler(this.tbAction_TextChanged);
			// 
			// tbHyperlink
			// 
			resources.ApplyResources(this.tbHyperlink, "tbHyperlink");
			this.tbHyperlink.Name = "tbHyperlink";
			this.tbHyperlink.TextChanged += new System.EventHandler(this.tbAction_TextChanged);
			// 
			// rbDrillthrough
			// 
			resources.ApplyResources(this.rbDrillthrough, "rbDrillthrough");
			this.rbDrillthrough.Name = "rbDrillthrough";
			this.rbDrillthrough.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// rbBookmarkLink
			// 
			resources.ApplyResources(this.rbBookmarkLink, "rbBookmarkLink");
			this.rbBookmarkLink.Name = "rbBookmarkLink";
			this.rbBookmarkLink.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// rbHyperlink
			// 
			resources.ApplyResources(this.rbHyperlink, "rbHyperlink");
			this.rbHyperlink.Name = "rbHyperlink";
			this.rbHyperlink.CheckedChanged += new System.EventHandler(this.rbAction_CheckedChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tbBookmark
			// 
			resources.ApplyResources(this.tbBookmark, "tbBookmark");
			this.tbBookmark.Name = "tbBookmark";
			// 
			// bBookmark
			// 
			resources.ApplyResources(this.bBookmark, "bBookmark");
			this.bBookmark.Name = "bBookmark";
			this.bBookmark.Tag = "bookmark";
			this.bBookmark.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// InteractivityCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.bBookmark);
			this.Controls.Add(this.tbBookmark);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grpBoxVisibility);
			this.Name = "InteractivityCtl";
			this.grpBoxVisibility.ResumeLayout(false);
			this.grpBoxVisibility.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
			ofd.Filter = Strings.InteractivityCtl_bDrillthrough_Click_ReportFilesFilter;
			ofd.FilterIndex = 1;
			ofd.FileName = "*.rdl";

			ofd.Title = Strings.InteractivityCtl_bDrillthrough_Click_ReportFilesTitle;
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
