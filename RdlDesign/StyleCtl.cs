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
	/// Summary description for StyleCtl.
	/// </summary>
	internal class StyleCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		// flags for controlling whether syntax changed for a particular property
		private bool fPadLeft, fPadRight, fPadTop, fPadBottom;
		private bool fEndColor, fBackColor, fGradient, fDEName, fDEOutput;

		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox tbPadLeft;
		private System.Windows.Forms.TextBox tbPadRight;
		private System.Windows.Forms.TextBox tbPadTop;
		private System.Windows.Forms.GroupBox grpBoxPadding;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bBackColor;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ComboBox cbEndColor;
		private System.Windows.Forms.ComboBox cbBackColor;
		private System.Windows.Forms.Button bEndColor;
		private System.Windows.Forms.ComboBox cbGradient;
		private System.Windows.Forms.TextBox tbPadBottom;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbDEName;
		private System.Windows.Forms.ComboBox cbDEOutput;
		private System.Windows.Forms.GroupBox gbXML;
		private System.Windows.Forms.Button bValueExpr;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button bGradient;
		private System.Windows.Forms.Button bExprBackColor;
		private System.Windows.Forms.Button bExprEndColor;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal StyleCtl(DesignXmlDraw dxDraw, List<XmlNode> reportItems)
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
            cbEndColor.Items.AddRange(StaticLists.ColorList);
            cbBackColor.Items.AddRange(StaticLists.ColorList);

			XmlNode sNode = _Draw.GetNamedChildNode(node, "Style");

			// Handle padding
			tbPadLeft.Text = _Draw.GetElementValue(sNode, "PaddingLeft", "0pt");
			tbPadRight.Text = _Draw.GetElementValue(sNode, "PaddingRight", "0pt");
			tbPadTop.Text = _Draw.GetElementValue(sNode, "PaddingTop", "0pt");
			tbPadBottom.Text = _Draw.GetElementValue(sNode, "PaddingBottom", "0pt");

			this.cbBackColor.Text = _Draw.GetElementValue(sNode, "BackgroundColor", "");
			this.cbEndColor.Text = _Draw.GetElementValue(sNode, "BackgroundGradientEndColor", "");
			this.cbGradient.Text = _Draw.GetElementValue(sNode, "BackgroundGradientType", "None");
			this.tbDEName.Text = _Draw.GetElementValue(node, "DataElementName", "");
			this.cbDEOutput.Text = _Draw.GetElementValue(node, "DataElementOutput", "Auto");
			if (node.Name != "Chart")
			{   // only chart support gradients
				this.cbEndColor.Enabled = bExprEndColor.Enabled =
					cbGradient.Enabled = bGradient.Enabled = 
					this.bEndColor.Enabled = bExprEndColor.Enabled = false;
			}
			if (node.Name == "Line" || node.Name == "Image")
			{
				gbXML.Visible = false;
			}

			// nothing has changed now
			fPadLeft = fPadRight = fPadTop = fPadBottom =
				fEndColor = fBackColor = fGradient = fDEName = fDEOutput = false;
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
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPadLeft = new System.Windows.Forms.TextBox();
            this.tbPadRight = new System.Windows.Forms.TextBox();
            this.tbPadTop = new System.Windows.Forms.TextBox();
            this.tbPadBottom = new System.Windows.Forms.TextBox();
            this.grpBoxPadding = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bValueExpr = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bGradient = new System.Windows.Forms.Button();
            this.bExprBackColor = new System.Windows.Forms.Button();
            this.bExprEndColor = new System.Windows.Forms.Button();
            this.bEndColor = new System.Windows.Forms.Button();
            this.cbBackColor = new System.Windows.Forms.ComboBox();
            this.cbEndColor = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cbGradient = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bBackColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.gbXML = new System.Windows.Forms.GroupBox();
            this.cbDEOutput = new System.Windows.Forms.ComboBox();
            this.tbDEName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBoxPadding.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbXML.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(8, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 16);
            this.label11.TabIndex = 20;
            this.label11.Text = "Left";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(224, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 16);
            this.label12.TabIndex = 21;
            this.label12.Text = "Right";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 56);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 16);
            this.label13.TabIndex = 22;
            this.label13.Text = "Top";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(216, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 16);
            this.label14.TabIndex = 23;
            this.label14.Text = "Bottom";
            // 
            // tbPadLeft
            // 
            this.tbPadLeft.Location = new System.Drawing.Point(48, 24);
            this.tbPadLeft.Name = "tbPadLeft";
            this.tbPadLeft.Size = new System.Drawing.Size(128, 20);
            this.tbPadLeft.TabIndex = 0;
            this.tbPadLeft.TextChanged += new System.EventHandler(this.tbPadLeft_TextChanged);
            // 
            // tbPadRight
            // 
            this.tbPadRight.Location = new System.Drawing.Point(264, 24);
            this.tbPadRight.Name = "tbPadRight";
            this.tbPadRight.Size = new System.Drawing.Size(128, 20);
            this.tbPadRight.TabIndex = 2;
            this.tbPadRight.TextChanged += new System.EventHandler(this.tbPadRight_TextChanged);
            // 
            // tbPadTop
            // 
            this.tbPadTop.Location = new System.Drawing.Point(48, 56);
            this.tbPadTop.Name = "tbPadTop";
            this.tbPadTop.Size = new System.Drawing.Size(128, 20);
            this.tbPadTop.TabIndex = 4;
            this.tbPadTop.TextChanged += new System.EventHandler(this.tbPadTop_TextChanged);
            // 
            // tbPadBottom
            // 
            this.tbPadBottom.Location = new System.Drawing.Point(264, 56);
            this.tbPadBottom.Name = "tbPadBottom";
            this.tbPadBottom.Size = new System.Drawing.Size(128, 20);
            this.tbPadBottom.TabIndex = 6;
            this.tbPadBottom.TextChanged += new System.EventHandler(this.tbPadBottom_TextChanged);
            // 
            // grpBoxPadding
            // 
            this.grpBoxPadding.Controls.Add(this.button3);
            this.grpBoxPadding.Controls.Add(this.button2);
            this.grpBoxPadding.Controls.Add(this.button1);
            this.grpBoxPadding.Controls.Add(this.bValueExpr);
            this.grpBoxPadding.Controls.Add(this.label13);
            this.grpBoxPadding.Controls.Add(this.tbPadRight);
            this.grpBoxPadding.Controls.Add(this.label14);
            this.grpBoxPadding.Controls.Add(this.label11);
            this.grpBoxPadding.Controls.Add(this.tbPadBottom);
            this.grpBoxPadding.Controls.Add(this.label12);
            this.grpBoxPadding.Controls.Add(this.tbPadTop);
            this.grpBoxPadding.Controls.Add(this.tbPadLeft);
            this.grpBoxPadding.Location = new System.Drawing.Point(16, 96);
            this.grpBoxPadding.Name = "grpBoxPadding";
            this.grpBoxPadding.Size = new System.Drawing.Size(432, 88);
            this.grpBoxPadding.TabIndex = 1;
            this.grpBoxPadding.TabStop = false;
            this.grpBoxPadding.Text = "Padding";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(400, 26);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(22, 16);
            this.button3.TabIndex = 3;
            this.button3.Tag = "pright";
            this.button3.Text = "fx";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(400, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 16);
            this.button2.TabIndex = 7;
            this.button2.Tag = "pbottom";
            this.button2.Text = "fx";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(184, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 16);
            this.button1.TabIndex = 5;
            this.button1.Tag = "ptop";
            this.button1.Text = "fx";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bValueExpr
            // 
            this.bValueExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bValueExpr.Location = new System.Drawing.Point(184, 26);
            this.bValueExpr.Name = "bValueExpr";
            this.bValueExpr.Size = new System.Drawing.Size(22, 16);
            this.bValueExpr.TabIndex = 1;
            this.bValueExpr.Tag = "pleft";
            this.bValueExpr.Text = "fx";
            this.bValueExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bValueExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bGradient);
            this.groupBox1.Controls.Add(this.bExprBackColor);
            this.groupBox1.Controls.Add(this.bExprEndColor);
            this.groupBox1.Controls.Add(this.bEndColor);
            this.groupBox1.Controls.Add(this.cbBackColor);
            this.groupBox1.Controls.Add(this.cbEndColor);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cbGradient);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.bBackColor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background";
            // 
            // bGradient
            // 
            this.bGradient.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bGradient.Location = new System.Drawing.Point(253, 42);
            this.bGradient.Name = "bGradient";
            this.bGradient.Size = new System.Drawing.Size(22, 16);
            this.bGradient.TabIndex = 4;
            this.bGradient.Tag = "bgradient";
            this.bGradient.Text = "fx";
            this.bGradient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bGradient.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bExprBackColor
            // 
            this.bExprBackColor.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExprBackColor.Location = new System.Drawing.Point(102, 42);
            this.bExprBackColor.Name = "bExprBackColor";
            this.bExprBackColor.Size = new System.Drawing.Size(22, 16);
            this.bExprBackColor.TabIndex = 1;
            this.bExprBackColor.Tag = "bcolor";
            this.bExprBackColor.Text = "fx";
            this.bExprBackColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bExprBackColor.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bExprEndColor
            // 
            this.bExprEndColor.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExprEndColor.Location = new System.Drawing.Point(377, 42);
            this.bExprEndColor.Name = "bExprEndColor";
            this.bExprEndColor.Size = new System.Drawing.Size(22, 16);
            this.bExprEndColor.TabIndex = 6;
            this.bExprEndColor.Tag = "bendcolor";
            this.bExprEndColor.Text = "fx";
            this.bExprEndColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bExprEndColor.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bEndColor
            // 
            this.bEndColor.Location = new System.Drawing.Point(404, 42);
            this.bEndColor.Name = "bEndColor";
            this.bEndColor.Size = new System.Drawing.Size(22, 16);
            this.bEndColor.TabIndex = 7;
            this.bEndColor.Text = "...";
            this.bEndColor.Click += new System.EventHandler(this.bColor_Click);
            // 
            // cbBackColor
            // 
            this.cbBackColor.Location = new System.Drawing.Point(8, 40);
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.Size = new System.Drawing.Size(88, 21);
            this.cbBackColor.TabIndex = 0;
            this.cbBackColor.SelectedIndexChanged += new System.EventHandler(this.cbBackColor_SelectedIndexChanged);
            this.cbBackColor.TextChanged += new System.EventHandler(this.cbBackColor_SelectedIndexChanged);
            // 
            // cbEndColor
            // 
            this.cbEndColor.Location = new System.Drawing.Point(286, 40);
            this.cbEndColor.Name = "cbEndColor";
            this.cbEndColor.Size = new System.Drawing.Size(88, 21);
            this.cbEndColor.TabIndex = 5;
            this.cbEndColor.SelectedIndexChanged += new System.EventHandler(this.cbEndColor_SelectedIndexChanged);
            this.cbEndColor.TextChanged += new System.EventHandler(this.cbEndColor_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(286, 24);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 16);
            this.label15.TabIndex = 5;
            this.label15.Text = "End Color";
            // 
            // cbGradient
            // 
            this.cbGradient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGradient.Items.AddRange(new object[] {
            "None",
            "LeftRight",
            "TopBottom",
            "Center",
            "DiagonalLeft",
            "DiagonalRight",
            "HorizontalCenter",
            "VerticalCenter"});
            this.cbGradient.Location = new System.Drawing.Point(161, 40);
            this.cbGradient.Name = "cbGradient";
            this.cbGradient.Size = new System.Drawing.Size(88, 21);
            this.cbGradient.TabIndex = 3;
            this.cbGradient.SelectedIndexChanged += new System.EventHandler(this.cbGradient_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(161, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 3;
            this.label10.Text = "Gradient";
            // 
            // bBackColor
            // 
            this.bBackColor.Location = new System.Drawing.Point(128, 42);
            this.bBackColor.Name = "bBackColor";
            this.bBackColor.Size = new System.Drawing.Size(22, 16);
            this.bBackColor.TabIndex = 2;
            this.bBackColor.Text = "...";
            this.bBackColor.Click += new System.EventHandler(this.bColor_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Color";
            // 
            // gbXML
            // 
            this.gbXML.Controls.Add(this.cbDEOutput);
            this.gbXML.Controls.Add(this.tbDEName);
            this.gbXML.Controls.Add(this.label2);
            this.gbXML.Controls.Add(this.label1);
            this.gbXML.Location = new System.Drawing.Point(16, 200);
            this.gbXML.Name = "gbXML";
            this.gbXML.Size = new System.Drawing.Size(432, 80);
            this.gbXML.TabIndex = 24;
            this.gbXML.TabStop = false;
            this.gbXML.Text = "XML";
            // 
            // cbDEOutput
            // 
            this.cbDEOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDEOutput.Items.AddRange(new object[] {
            "Output",
            "NoOutput",
            "ContentsOnly",
            "Auto"});
            this.cbDEOutput.Location = new System.Drawing.Point(138, 48);
            this.cbDEOutput.Name = "cbDEOutput";
            this.cbDEOutput.Size = new System.Drawing.Size(117, 21);
            this.cbDEOutput.TabIndex = 3;
            this.cbDEOutput.SelectedIndexChanged += new System.EventHandler(this.cbDEOutput_SelectedIndexChanged);
            // 
            // tbDEName
            // 
            this.tbDEName.Location = new System.Drawing.Point(137, 20);
            this.tbDEName.Name = "tbDEName";
            this.tbDEName.Size = new System.Drawing.Size(279, 20);
            this.tbDEName.TabIndex = 2;
            this.tbDEName.Text = "textBox1";
            this.tbDEName.TextChanged += new System.EventHandler(this.tbDEName_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "DataElementOutput";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "DataElementName";
            // 
            // StyleCtl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpBoxPadding);
            this.Controls.Add(this.gbXML);
            this.Name = "StyleCtl";
            this.Size = new System.Drawing.Size(472, 312);
            this.grpBoxPadding.ResumeLayout(false);
            this.grpBoxPadding.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbXML.ResumeLayout(false);
            this.gbXML.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
     
		public bool IsValid()
		{
			string name="";
			try
			{
				if (fPadLeft && !this.tbPadLeft.Text.StartsWith("="))
				{
					name = "Left";
					DesignerUtility.ValidateSize(this.tbPadLeft.Text, true, false);
				}
				
				if (fPadRight && !this.tbPadRight.Text.StartsWith("="))
				{
					name = "Right";
					DesignerUtility.ValidateSize(this.tbPadRight.Text, true, false);
				}
				
				if (fPadTop && !this.tbPadTop.Text.StartsWith("="))
				{
					name = "Top";
					DesignerUtility.ValidateSize(this.tbPadTop.Text, true, false);
				}
				
				if (fPadBottom && !this.tbPadBottom.Text.StartsWith("="))
				{
					name = "Bottom";
					DesignerUtility.ValidateSize(this.tbPadBottom.Text, true, false);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, name + " Padding Invalid");
				return false;
			}
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
			fPadLeft = fPadRight = fPadTop = fPadBottom =
				fEndColor = fBackColor = fGradient = fDEName = fDEOutput = false;
		}

		private void bFont_Click(object sender, System.EventArgs e)
		{
            using (FontDialog fd = new FontDialog())
            {
                fd.ShowColor = true;
                if (fd.ShowDialog() != DialogResult.OK)
                    return;
            } 
            return;
		}

		private void bColor_Click(object sender, System.EventArgs e)
		{
            using (ColorDialog cd = new ColorDialog())
            {
                cd.AnyColor = true;
                cd.FullOpen = true;
                cd.CustomColors = RdlDesigner.GetCustomColors();

                if (cd.ShowDialog() != DialogResult.OK)
                    return;

                RdlDesigner.SetCustomColors(cd.CustomColors);
                if (sender == this.bEndColor)
                    cbEndColor.Text = ColorTranslator.ToHtml(cd.Color);
                else if (sender == this.bBackColor)
                    cbBackColor.Text = ColorTranslator.ToHtml(cd.Color);
            }
			return;
		}

		private void cbBackColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fBackColor = true;
		}

		private void cbGradient_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fGradient = true;
		}

		private void cbEndColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fEndColor = true;
		}

		private void tbPadLeft_TextChanged(object sender, System.EventArgs e)
		{
			fPadLeft = true;
		}

		private void tbPadRight_TextChanged(object sender, System.EventArgs e)
		{
			fPadRight = true;
		}

		private void tbPadTop_TextChanged(object sender, System.EventArgs e)
		{
			fPadTop = true;
		}

		private void tbPadBottom_TextChanged(object sender, System.EventArgs e)
		{
			fPadBottom = true;
		}
		
		private void ApplyChanges(XmlNode rNode)
		{
			XmlNode xNode = _Draw.GetNamedChildNode(rNode, "Style");

			if (fPadLeft)
			{ _Draw.SetElement(xNode, "PaddingLeft", tbPadLeft.Text); }
			if (fPadRight)
			{ _Draw.SetElement(xNode, "PaddingRight", tbPadRight.Text); }
			if (fPadTop)
			{ _Draw.SetElement(xNode, "PaddingTop", tbPadTop.Text); }
			if (fPadBottom)
			{ _Draw.SetElement(xNode, "PaddingBottom", tbPadBottom.Text); }
			if (fEndColor)
			{ _Draw.SetElement(xNode, "BackgroundGradientEndColor", cbEndColor.Text); }
			if (fBackColor)
			{ _Draw.SetElement(xNode, "BackgroundColor", cbBackColor.Text); }
			if (fGradient)
			{ _Draw.SetElement(xNode, "BackgroundGradientType", cbGradient.Text); }
			if (fDEName)
			{ _Draw.SetElement(rNode, "DataElementName", tbDEName.Text); }
			if (fDEOutput)
			{ _Draw.SetElement(rNode, "DataElementOutput", cbDEOutput.Text); }
		}

		private void cbDEOutput_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fDEOutput = true;
		}

		private void tbDEName_TextChanged(object sender, System.EventArgs e)
		{
			fDEName = true;
		}
		private void bExpr_Click(object sender, System.EventArgs e)
		{
			Button b = sender as Button;
			if (b == null)
				return;
			Control c = null;
			bool bColor=false;
			switch (b.Tag as string)
			{
				case "pleft":
					c = tbPadLeft;
					break;
				case "pright":
					c = tbPadRight;
					break;
				case "ptop":
					c = tbPadTop;
					break;
				case "pbottom":
					c = tbPadBottom;
					break;
				case "bcolor":
					c = cbBackColor;
					bColor = true;
					break;
				case "bgradient":
					c = cbGradient;
					break;
				case "bendcolor":
					c = cbEndColor;
					bColor = true;
					break;
			}

			if (c == null)
				return;

			XmlNode sNode = _ReportItems[0];

            using (DialogExprEditor ee = new DialogExprEditor(_Draw, c.Text, sNode, bColor))
            {
                DialogResult dr = ee.ShowDialog();
                if (dr == DialogResult.OK)
                    c.Text = ee.Expression;

            } 
            return;
		}

	}
}
