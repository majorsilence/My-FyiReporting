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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for BodyCtl.
	/// </summary>
	internal class BodyCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbHeight;
		private System.Windows.Forms.TextBox tbColumns;
		private System.Windows.Forms.TextBox tbColumnSpacing;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal BodyCtl(DesignXmlDraw dxDraw)
		{
			_Draw = dxDraw;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			XmlNode rNode = _Draw.GetReportNode();
			XmlNode bNode = _Draw.GetNamedChildNode(rNode, "Body");
			tbHeight.Text = _Draw.GetElementValue(bNode, "Height", "");
			tbColumns.Text = _Draw.GetElementValue(bNode, "Columns", "1");
			tbColumnSpacing.Text = _Draw.GetElementValue(bNode, "ColumnSpacing", "");
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbHeight = new System.Windows.Forms.TextBox();
			this.tbColumns = new System.Windows.Forms.TextBox();
			this.tbColumnSpacing = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Height";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Columns";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Column Spacing";
			// 
			// tbHeight
			// 
			this.tbHeight.Location = new System.Drawing.Point(104, 25);
			this.tbHeight.Name = "tbHeight";
			this.tbHeight.TabIndex = 0;
			this.tbHeight.Text = "";
			// 
			// tbColumns
			// 
			this.tbColumns.Location = new System.Drawing.Point(104, 61);
			this.tbColumns.Name = "tbColumns";
			this.tbColumns.TabIndex = 1;
			this.tbColumns.Text = "";
			// 
			// tbColumnSpacing
			// 
			this.tbColumnSpacing.Location = new System.Drawing.Point(104, 96);
			this.tbColumnSpacing.Name = "tbColumnSpacing";
			this.tbColumnSpacing.TabIndex = 2;
			this.tbColumnSpacing.Text = "";
			// 
			// BodyCtl
			// 
			this.Controls.Add(this.tbColumnSpacing);
			this.Controls.Add(this.tbColumns);
			this.Controls.Add(this.tbHeight);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "BodyCtl";
			this.Size = new System.Drawing.Size(472, 288);
			this.ResumeLayout(false);

		}
		#endregion
        
		public bool IsValid()
		{
			return true;
		}

		public void Apply()
		{
			XmlNode rNode = _Draw.GetReportNode();
			XmlNode bNode = _Draw.GetNamedChildNode(rNode, "Body");
			_Draw.SetElement(bNode, "Height", tbHeight.Text);
			_Draw.SetElement(bNode, "Columns", tbColumns.Text);
			if (tbColumnSpacing.Text.Length > 0)
				_Draw.SetElement(bNode, "ColumnSpacing", tbColumnSpacing.Text);
			else
				_Draw.RemoveElement(bNode, "ColumnSpacing");
		}
	}
}
