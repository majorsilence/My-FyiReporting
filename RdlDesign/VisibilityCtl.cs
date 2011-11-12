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
	internal class VisibilityCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		// flags for controlling whether syntax changed for a particular property
        private bool fHidden, fToggle;
        private System.Windows.Forms.GroupBox grpBoxVisibility;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbHidden;
        private System.Windows.Forms.ComboBox cbToggle;
        private System.Windows.Forms.Button bHidden;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal VisibilityCtl(DesignXmlDraw dxDraw, List<XmlNode> reportItems)
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
			fHidden = fToggle = false;
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
            this.grpBoxVisibility.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxVisibility
            // 
            this.grpBoxVisibility.Controls.Add(this.bHidden);
            this.grpBoxVisibility.Controls.Add(this.cbToggle);
            this.grpBoxVisibility.Controls.Add(this.tbHidden);
            this.grpBoxVisibility.Controls.Add(this.label3);
            this.grpBoxVisibility.Controls.Add(this.label2);
            this.grpBoxVisibility.Location = new System.Drawing.Point(3, 3);
            this.grpBoxVisibility.Name = "grpBoxVisibility";
            this.grpBoxVisibility.Size = new System.Drawing.Size(432, 80);
            this.grpBoxVisibility.TabIndex = 1;
            this.grpBoxVisibility.TabStop = false;
            this.grpBoxVisibility.Text = "Visibility";
            // 
            // bHidden
            // 
            this.bHidden.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cbToggle.Size = new System.Drawing.Size(224, 21);
            this.cbToggle.TabIndex = 2;
            this.cbToggle.SelectedIndexChanged += new System.EventHandler(this.cbToggle_SelectedIndexChanged);
            this.cbToggle.TextChanged += new System.EventHandler(this.cbToggle_SelectedIndexChanged);
            // 
            // tbHidden
            // 
            this.tbHidden.Location = new System.Drawing.Point(168, 24);
            this.tbHidden.Name = "tbHidden";
            this.tbHidden.Size = new System.Drawing.Size(224, 20);
            this.tbHidden.TabIndex = 0;
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
            // VisibilityCtl
            // 
            this.Controls.Add(this.grpBoxVisibility);
            this.Name = "VisibilityCtl";
            this.Size = new System.Drawing.Size(454, 104);
            this.grpBoxVisibility.ResumeLayout(false);
            this.grpBoxVisibility.PerformLayout();
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
			fHidden = fToggle = false;
		}

		private void ApplyChanges(XmlNode rNode)
		{
			if (fHidden || fToggle)
			{
				XmlNode visNode = _Draw.SetElement(rNode, "Visibility", null);

                if (fHidden)
                {
                    if (tbHidden.Text.Length == 0)
                        _Draw.RemoveElement(visNode, "Hidden");
                    else
                        _Draw.SetElement(visNode, "Hidden", tbHidden.Text);
                }
				if (fToggle)
					_Draw.SetElement(visNode, "ToggleItem", this.cbToggle.Text); 
			}
		}

		private void tbHidden_TextChanged(object sender, System.EventArgs e)
		{
			fHidden = true;
		}

		private void cbToggle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fToggle = true;
		}

		private void bExpr_Click(object sender, System.EventArgs e)
		{
			Button b = sender as Button;
			if (b == null)
				return;
			Control c = null;
			switch (b.Tag as string)
			{
				case "visibility":
					c = tbHidden;
					break;
			}

			if (c == null)
				return;

			XmlNode sNode = _ReportItems[0];

            using (DialogExprEditor ee = new DialogExprEditor(_Draw, c.Text, sNode))
            {
                DialogResult dr = ee.ShowDialog();
                if (dr == DialogResult.OK)
                    c.Text = ee.Expression;
                return;
            }
		}

	}
}
