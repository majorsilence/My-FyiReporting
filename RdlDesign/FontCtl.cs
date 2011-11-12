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
using System.Globalization;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// FontCtl
	/// </summary>
	internal class FontCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		private bool fHorzAlign, fFormat, fDirection, fWritingMode, fTextDecoration;
		private bool fColor, fVerticalAlign, fFontStyle, fFontWeight, fFontSize, fFontFamily;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label lFont;
		private System.Windows.Forms.Button bFont;
		private System.Windows.Forms.ComboBox cbHorzAlign;
		private System.Windows.Forms.ComboBox cbFormat;
		private System.Windows.Forms.ComboBox cbDirection;
		private System.Windows.Forms.ComboBox cbWritingMode;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbTextDecoration;
		private System.Windows.Forms.Button bColor;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cbColor;
		private System.Windows.Forms.ComboBox cbVerticalAlign;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbFontStyle;
		private System.Windows.Forms.ComboBox cbFontWeight;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cbFontSize;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cbFontFamily;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button bFamily;
		private System.Windows.Forms.Button bStyle;
		private System.Windows.Forms.Button bColorEx;
		private System.Windows.Forms.Button bSize;
		private System.Windows.Forms.Button bWeight;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button bAlignment;
		private System.Windows.Forms.Button bDirection;
		private System.Windows.Forms.Button bVertical;
		private System.Windows.Forms.Button bWrtMode;
		private System.Windows.Forms.Button bFormat;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private string[] _names;

        internal FontCtl(DesignXmlDraw dxDraw, string[] names, List<XmlNode> styles)
		{
			_ReportItems = styles;
			_Draw = dxDraw;
            _names = names;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitTextStyles();
		}

		private void InitTextStyles()
		{
            cbColor.Items.AddRange(StaticLists.ColorList);
            cbFormat.Items.AddRange(StaticLists.FormatList);

			XmlNode sNode = _ReportItems[0];

            if (_names != null)
            {
                sNode = _Draw.FindCreateNextInHierarchy(sNode, _names);
            }

			sNode = _Draw.GetCreateNamedChildNode(sNode, "Style");

			string sFontStyle="Normal";
			string sFontFamily="Arial";
			string sFontWeight="Normal";
			string sFontSize="10pt";
			string sTextDecoration="None";
			string sHorzAlign="General";
			string sVerticalAlign="Top";
			string sColor="Black";
			string sFormat="";
			string sDirection="LTR";
			string sWritingMode="lr-tb";
			foreach (XmlNode lNode in sNode)
			{
				if (lNode.NodeType != XmlNodeType.Element)
					continue;
				switch (lNode.Name)
				{
					case "FontStyle":
						sFontStyle = lNode.InnerText;
						break;
					case "FontFamily":
						sFontFamily = lNode.InnerText;
						break;
					case "FontWeight":
						sFontWeight = lNode.InnerText;
						break;
					case "FontSize":
						sFontSize = lNode.InnerText;
						break;
					case "TextDecoration":
						sTextDecoration = lNode.InnerText;
						break;
					case "TextAlign":
						sHorzAlign = lNode.InnerText;
						break;
					case "VerticalAlign":
						sVerticalAlign = lNode.InnerText;
						break;
					case "Color":
						sColor = lNode.InnerText;
						break;
					case "Format":
						sFormat = lNode.InnerText;
						break;
					case "Direction":
						sDirection = lNode.InnerText;
						break;
					case "WritingMode":
						sWritingMode = lNode.InnerText;
						break;
				}
			}

			// Population Font Family dropdown
			foreach (FontFamily ff in FontFamily.Families)
			{
				cbFontFamily.Items.Add(ff.Name);
			}

			this.cbFontStyle.Text = sFontStyle;
			this.cbFontFamily.Text = sFontFamily;
			this.cbFontWeight.Text = sFontWeight;
			this.cbFontSize.Text = sFontSize;
			this.cbTextDecoration.Text = sTextDecoration;
			this.cbHorzAlign.Text = sHorzAlign;
			this.cbVerticalAlign.Text = sVerticalAlign;
			this.cbColor.Text = sColor;
			this.cbFormat.Text = sFormat;
			this.cbDirection.Text = sDirection;
			this.cbWritingMode.Text = sWritingMode;

            fHorzAlign = fFormat = fDirection = fWritingMode = fTextDecoration =
                fColor = fVerticalAlign = fFontStyle = fFontWeight = fFontSize = fFontFamily = false;

			return;
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lFont = new System.Windows.Forms.Label();
            this.bFont = new System.Windows.Forms.Button();
            this.cbVerticalAlign = new System.Windows.Forms.ComboBox();
            this.cbHorzAlign = new System.Windows.Forms.ComboBox();
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.cbWritingMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTextDecoration = new System.Windows.Forms.ComboBox();
            this.bColor = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cbColor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbFontStyle = new System.Windows.Forms.ComboBox();
            this.cbFontWeight = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbFontSize = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbFontFamily = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.bWeight = new System.Windows.Forms.Button();
            this.bSize = new System.Windows.Forms.Button();
            this.bColorEx = new System.Windows.Forms.Button();
            this.bStyle = new System.Windows.Forms.Button();
            this.bFamily = new System.Windows.Forms.Button();
            this.bAlignment = new System.Windows.Forms.Button();
            this.bDirection = new System.Windows.Forms.Button();
            this.bVertical = new System.Windows.Forms.Button();
            this.bWrtMode = new System.Windows.Forms.Button();
            this.bFormat = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Format";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(216, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Vertical";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Alignment";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "Direction";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(216, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 16);
            this.label8.TabIndex = 7;
            this.label8.Text = "Writing Mode";
            // 
            // lFont
            // 
            this.lFont.Location = new System.Drawing.Point(16, 16);
            this.lFont.Name = "lFont";
            this.lFont.Size = new System.Drawing.Size(40, 16);
            this.lFont.TabIndex = 8;
            this.lFont.Text = "Family";
            // 
            // bFont
            // 
            this.bFont.Location = new System.Drawing.Point(401, 20);
            this.bFont.Name = "bFont";
            this.bFont.Size = new System.Drawing.Size(22, 16);
            this.bFont.TabIndex = 4;
            this.bFont.Text = "...";
            this.bFont.Click += new System.EventHandler(this.bFont_Click);
            // 
            // cbVerticalAlign
            // 
            this.cbVerticalAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVerticalAlign.Items.AddRange(new object[] {
            "Top",
            "Middle",
            "Bottom"});
            this.cbVerticalAlign.Location = new System.Drawing.Point(304, 132);
            this.cbVerticalAlign.Name = "cbVerticalAlign";
            this.cbVerticalAlign.Size = new System.Drawing.Size(72, 21);
            this.cbVerticalAlign.TabIndex = 5;
            this.cbVerticalAlign.SelectedIndexChanged += new System.EventHandler(this.cbVerticalAlign_TextChanged);
            this.cbVerticalAlign.TextChanged += new System.EventHandler(this.cbVerticalAlign_TextChanged);
            // 
            // cbHorzAlign
            // 
            this.cbHorzAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHorzAlign.Items.AddRange(new object[] {
            "Left",
            "Center",
            "Right",
            "General"});
            this.cbHorzAlign.Location = new System.Drawing.Point(80, 132);
            this.cbHorzAlign.Name = "cbHorzAlign";
            this.cbHorzAlign.Size = new System.Drawing.Size(64, 21);
            this.cbHorzAlign.TabIndex = 3;
            this.cbHorzAlign.SelectedIndexChanged += new System.EventHandler(this.cbHorzAlign_TextChanged);
            this.cbHorzAlign.TextChanged += new System.EventHandler(this.cbHorzAlign_TextChanged);
            // 
            // cbFormat
            // 
            this.cbFormat.Location = new System.Drawing.Point(80, 196);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(296, 21);
            this.cbFormat.TabIndex = 11;
            this.cbFormat.TextChanged += new System.EventHandler(this.cbFormat_TextChanged);
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.Items.AddRange(new object[] {
            "LTR",
            "RTL"});
            this.cbDirection.Location = new System.Drawing.Point(80, 164);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(64, 21);
            this.cbDirection.TabIndex = 7;
            this.cbDirection.SelectedIndexChanged += new System.EventHandler(this.cbDirection_TextChanged);
            this.cbDirection.TextChanged += new System.EventHandler(this.cbDirection_TextChanged);
            // 
            // cbWritingMode
            // 
            this.cbWritingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWritingMode.Items.AddRange(new object[] {
            "lr-tb",
            "tb-rl"});
            this.cbWritingMode.Location = new System.Drawing.Point(304, 164);
            this.cbWritingMode.Name = "cbWritingMode";
            this.cbWritingMode.Size = new System.Drawing.Size(72, 21);
            this.cbWritingMode.TabIndex = 9;
            this.cbWritingMode.SelectedIndexChanged += new System.EventHandler(this.cbWritingMode_TextChanged);
            this.cbWritingMode.TextChanged += new System.EventHandler(this.cbWritingMode_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(224, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 19;
            this.label2.Text = "Decoration";
            // 
            // cbTextDecoration
            // 
            this.cbTextDecoration.Items.AddRange(new object[] {
            "None",
            "Underline",
            "Overline",
            "LineThrough"});
            this.cbTextDecoration.Location = new System.Drawing.Point(288, 80);
            this.cbTextDecoration.Name = "cbTextDecoration";
            this.cbTextDecoration.Size = new System.Drawing.Size(80, 21);
            this.cbTextDecoration.TabIndex = 12;
            this.cbTextDecoration.SelectedIndexChanged += new System.EventHandler(this.cbTextDecoration_TextChanged);
            this.cbTextDecoration.TextChanged += new System.EventHandler(this.cbTextDecoration_TextChanged);
            // 
            // bColor
            // 
            this.bColor.Location = new System.Drawing.Point(200, 82);
            this.bColor.Name = "bColor";
            this.bColor.Size = new System.Drawing.Size(22, 16);
            this.bColor.TabIndex = 11;
            this.bColor.Text = "...";
            this.bColor.Click += new System.EventHandler(this.bColor_Click);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 16);
            this.label9.TabIndex = 22;
            this.label9.Text = "Color";
            // 
            // cbColor
            // 
            this.cbColor.Location = new System.Drawing.Point(72, 80);
            this.cbColor.Name = "cbColor";
            this.cbColor.Size = new System.Drawing.Size(96, 21);
            this.cbColor.TabIndex = 9;
            this.cbColor.TextChanged += new System.EventHandler(this.cbColor_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Style";
            // 
            // cbFontStyle
            // 
            this.cbFontStyle.Items.AddRange(new object[] {
            "Normal",
            "Italic"});
            this.cbFontStyle.Location = new System.Drawing.Point(72, 48);
            this.cbFontStyle.Name = "cbFontStyle";
            this.cbFontStyle.Size = new System.Drawing.Size(96, 21);
            this.cbFontStyle.TabIndex = 5;
            this.cbFontStyle.TextChanged += new System.EventHandler(this.cbFontStyle_TextChanged);
            // 
            // cbFontWeight
            // 
            this.cbFontWeight.Items.AddRange(new object[] {
            "Lighter",
            "Normal",
            "Bold",
            "Bolder",
            "100",
            "200",
            "300",
            "400",
            "500",
            "600",
            "700",
            "800",
            "900"});
            this.cbFontWeight.Location = new System.Drawing.Point(270, 48);
            this.cbFontWeight.Name = "cbFontWeight";
            this.cbFontWeight.Size = new System.Drawing.Size(98, 21);
            this.cbFontWeight.TabIndex = 7;
            this.cbFontWeight.TextChanged += new System.EventHandler(this.cbFontWeight_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(224, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 16);
            this.label10.TabIndex = 27;
            this.label10.Text = "Weight";
            // 
            // cbFontSize
            // 
            this.cbFontSize.Items.AddRange(new object[] {
            "8pt",
            "9pt",
            "10pt",
            "11pt",
            "12pt",
            "14pt",
            "16pt",
            "18pt",
            "20pt",
            "22pt",
            "24pt",
            "26pt",
            "28pt",
            "36pt",
            "48pt",
            "72pt"});
            this.cbFontSize.Location = new System.Drawing.Point(270, 16);
            this.cbFontSize.Name = "cbFontSize";
            this.cbFontSize.Size = new System.Drawing.Size(98, 21);
            this.cbFontSize.TabIndex = 2;
            this.cbFontSize.TextChanged += new System.EventHandler(this.cbFontSize_TextChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(224, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 16);
            this.label11.TabIndex = 29;
            this.label11.Text = "Size";
            // 
            // cbFontFamily
            // 
            this.cbFontFamily.Items.AddRange(new object[] {
            "Arial"});
            this.cbFontFamily.Location = new System.Drawing.Point(72, 16);
            this.cbFontFamily.Name = "cbFontFamily";
            this.cbFontFamily.Size = new System.Drawing.Size(96, 21);
            this.cbFontFamily.TabIndex = 0;
            this.cbFontFamily.TextChanged += new System.EventHandler(this.cbFontFamily_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.bWeight);
            this.groupBox1.Controls.Add(this.bSize);
            this.groupBox1.Controls.Add(this.bColorEx);
            this.groupBox1.Controls.Add(this.bStyle);
            this.groupBox1.Controls.Add(this.bFamily);
            this.groupBox1.Controls.Add(this.lFont);
            this.groupBox1.Controls.Add(this.bFont);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbTextDecoration);
            this.groupBox1.Controls.Add(this.bColor);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbColor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbFontStyle);
            this.groupBox1.Controls.Add(this.cbFontWeight);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbFontSize);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cbFontFamily);
            this.groupBox1.Location = new System.Drawing.Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 112);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(376, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 16);
            this.button2.TabIndex = 13;
            this.button2.Tag = "decoration";
            this.button2.Text = "fx";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bWeight
            // 
            this.bWeight.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bWeight.Location = new System.Drawing.Point(376, 52);
            this.bWeight.Name = "bWeight";
            this.bWeight.Size = new System.Drawing.Size(22, 16);
            this.bWeight.TabIndex = 8;
            this.bWeight.Tag = "weight";
            this.bWeight.Text = "fx";
            this.bWeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bWeight.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bSize
            // 
            this.bSize.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSize.Location = new System.Drawing.Point(376, 20);
            this.bSize.Name = "bSize";
            this.bSize.Size = new System.Drawing.Size(22, 16);
            this.bSize.TabIndex = 3;
            this.bSize.Tag = "size";
            this.bSize.Text = "fx";
            this.bSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bSize.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bColorEx
            // 
            this.bColorEx.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bColorEx.Location = new System.Drawing.Point(176, 82);
            this.bColorEx.Name = "bColorEx";
            this.bColorEx.Size = new System.Drawing.Size(22, 16);
            this.bColorEx.TabIndex = 10;
            this.bColorEx.Tag = "color";
            this.bColorEx.Text = "fx";
            this.bColorEx.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bColorEx.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bStyle
            // 
            this.bStyle.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bStyle.Location = new System.Drawing.Point(176, 52);
            this.bStyle.Name = "bStyle";
            this.bStyle.Size = new System.Drawing.Size(22, 16);
            this.bStyle.TabIndex = 6;
            this.bStyle.Tag = "style";
            this.bStyle.Text = "fx";
            this.bStyle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bStyle.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bFamily
            // 
            this.bFamily.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bFamily.Location = new System.Drawing.Point(176, 20);
            this.bFamily.Name = "bFamily";
            this.bFamily.Size = new System.Drawing.Size(22, 16);
            this.bFamily.TabIndex = 1;
            this.bFamily.Tag = "family";
            this.bFamily.Text = "fx";
            this.bFamily.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bFamily.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bAlignment
            // 
            this.bAlignment.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bAlignment.Location = new System.Drawing.Point(152, 134);
            this.bAlignment.Name = "bAlignment";
            this.bAlignment.Size = new System.Drawing.Size(22, 16);
            this.bAlignment.TabIndex = 4;
            this.bAlignment.Tag = "halign";
            this.bAlignment.Text = "fx";
            this.bAlignment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bAlignment.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bDirection
            // 
            this.bDirection.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bDirection.Location = new System.Drawing.Point(152, 166);
            this.bDirection.Name = "bDirection";
            this.bDirection.Size = new System.Drawing.Size(22, 16);
            this.bDirection.TabIndex = 8;
            this.bDirection.Tag = "direction";
            this.bDirection.Text = "fx";
            this.bDirection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bDirection.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bVertical
            // 
            this.bVertical.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bVertical.Location = new System.Drawing.Point(384, 134);
            this.bVertical.Name = "bVertical";
            this.bVertical.Size = new System.Drawing.Size(22, 16);
            this.bVertical.TabIndex = 6;
            this.bVertical.Tag = "valign";
            this.bVertical.Text = "fx";
            this.bVertical.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bVertical.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bWrtMode
            // 
            this.bWrtMode.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bWrtMode.Location = new System.Drawing.Point(384, 166);
            this.bWrtMode.Name = "bWrtMode";
            this.bWrtMode.Size = new System.Drawing.Size(22, 16);
            this.bWrtMode.TabIndex = 10;
            this.bWrtMode.Tag = "writing";
            this.bWrtMode.Text = "fx";
            this.bWrtMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bWrtMode.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bFormat
            // 
            this.bFormat.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bFormat.Location = new System.Drawing.Point(384, 198);
            this.bFormat.Name = "bFormat";
            this.bFormat.Size = new System.Drawing.Size(22, 16);
            this.bFormat.TabIndex = 12;
            this.bFormat.Tag = "format";
            this.bFormat.Text = "fx";
            this.bFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bFormat.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // FontCtl
            // 
            this.Controls.Add(this.bFormat);
            this.Controls.Add(this.bWrtMode);
            this.Controls.Add(this.bVertical);
            this.Controls.Add(this.bDirection);
            this.Controls.Add(this.bAlignment);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbWritingMode);
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.cbFormat);
            this.Controls.Add(this.cbHorzAlign);
            this.Controls.Add(this.cbVerticalAlign);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Name = "FontCtl";
            this.Size = new System.Drawing.Size(456, 236);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public bool IsValid()
		{
			if (fFontSize)
			{
				try 
				{
					if (!this.cbFontSize.Text.Trim().StartsWith("="))
						DesignerUtility.ValidateSize(this.cbFontSize.Text, false, false);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Invalid Font Size");
					return false;
				}

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

			fHorzAlign = fFormat = fDirection = fWritingMode = fTextDecoration =
				fColor = fVerticalAlign = fFontStyle = fFontWeight = fFontSize = fFontFamily = false;
		}

		public void ApplyChanges(XmlNode node)
		{
            if (_names != null)
            {
                node = _Draw.FindCreateNextInHierarchy(node, _names);
            }

            XmlNode sNode = _Draw.GetCreateNamedChildNode(node, "Style");

			if (fFontStyle)
				_Draw.SetElement(sNode, "FontStyle", cbFontStyle.Text);
			if (fFontFamily)
				_Draw.SetElement(sNode, "FontFamily", cbFontFamily.Text);
			if (fFontWeight)
				_Draw.SetElement(sNode, "FontWeight", cbFontWeight.Text);

			if (fFontSize)
			{
				float size=10;
				size = DesignXmlDraw.GetSize(cbFontSize.Text);
				if (size <= 0)
				{
					size = DesignXmlDraw.GetSize(cbFontSize.Text+"pt");	// Try assuming pt
					if (size <= 0)	// still no good
						size = 10;	// just set default value
				}
				string rs = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.#}pt", size);

				_Draw.SetElement(sNode, "FontSize", rs);	// force to string
			}
			if (fTextDecoration)
				_Draw.SetElement(sNode, "TextDecoration", cbTextDecoration.Text);    
			if (fHorzAlign)
				_Draw.SetElement(sNode, "TextAlign", cbHorzAlign.Text);
			if (fVerticalAlign)
				_Draw.SetElement(sNode, "VerticalAlign", cbVerticalAlign.Text);
			if (fColor)
				_Draw.SetElement(sNode, "Color", cbColor.Text);
			if (fFormat)
			{
				if (cbFormat.Text.Length == 0)		// Don't put out a format if no format value
					_Draw.RemoveElement(sNode, "Format");
				else
					_Draw.SetElement(sNode, "Format", cbFormat.Text);
			}
			if (fDirection)
				_Draw.SetElement(sNode, "Direction", cbDirection.Text);
			if (fWritingMode)
				_Draw.SetElement(sNode, "WritingMode", cbWritingMode.Text);
			
			return;
		}

		private void bFont_Click(object sender, System.EventArgs e)
		{
			FontDialog fd = new FontDialog();
			fd.ShowColor = true;

			// STYLE
			System.Drawing.FontStyle fs = 0;
			if (cbFontStyle.Text == "Italic")
				fs |= System.Drawing.FontStyle.Italic;

			if (cbTextDecoration.Text == "Underline")
				fs |= FontStyle.Underline;
			else if (cbTextDecoration.Text == "LineThrough")
				fs |= FontStyle.Strikeout;

			// WEIGHT
			switch (cbFontWeight.Text)
			{
				case "Bold":
				case "Bolder":
				case "500":
				case "600":
				case "700":
				case "800":
				case "900":
					fs |= System.Drawing.FontStyle.Bold;
					break;
				default:
					break;
			}
			float size=10;
			size = DesignXmlDraw.GetSize(cbFontSize.Text);
			if (size <= 0)
			{
				size = DesignXmlDraw.GetSize(cbFontSize.Text+"pt");	// Try assuming pt
				if (size <= 0)	// still no good
					size = 10;	// just set default value
			}
			Font drawFont = new Font(cbFontFamily.Text, size, fs);	// si.FontSize already in points


			fd.Font = drawFont;
			fd.Color = 
				DesignerUtility.ColorFromHtml(cbColor.Text, System.Drawing.Color.Black);
            try
            {
                DialogResult dr = fd.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    drawFont.Dispose();
                    return;
                }

                // Apply all the font info
                cbFontWeight.Text = fd.Font.Bold ? "Bold" : "Normal";
                cbFontStyle.Text = fd.Font.Italic ? "Italic" : "Normal";
                cbFontFamily.Text = fd.Font.FontFamily.Name;
                cbFontSize.Text = fd.Font.Size.ToString() + "pt";
                cbColor.Text = ColorTranslator.ToHtml(fd.Color);
                if (fd.Font.Underline)
                    this.cbTextDecoration.Text = "Underline";
                else if (fd.Font.Strikeout)
                    this.cbTextDecoration.Text = "LineThrough";
                else
                    this.cbTextDecoration.Text = "None";
                drawFont.Dispose();
            }
            finally
            {
                fd.Dispose();
            }
			return;
		}

		private void bColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog cd = new ColorDialog();
			cd.AnyColor = true;
			cd.FullOpen = true;
			
			cd.CustomColors = RdlDesigner.GetCustomColors();
			cd.Color = 
				DesignerUtility.ColorFromHtml(cbColor.Text, System.Drawing.Color.Black);
            try
            {
                if (cd.ShowDialog() != DialogResult.OK)
                    return;

                RdlDesigner.SetCustomColors(cd.CustomColors);
                if (sender == this.bColor)
                    cbColor.Text = ColorTranslator.ToHtml(cd.Color);
            }
            finally
            {
                cd.Dispose();
            }
			return;
		}

		private void cbFontFamily_TextChanged(object sender, System.EventArgs e)
		{
			fFontFamily = true;
		}

		private void cbFontSize_TextChanged(object sender, System.EventArgs e)
		{
			fFontSize = true;
		}

		private void cbFontStyle_TextChanged(object sender, System.EventArgs e)
		{
			fFontStyle = true;
		}

		private void cbFontWeight_TextChanged(object sender, System.EventArgs e)
		{
			fFontWeight = true;
		}

		private void cbColor_TextChanged(object sender, System.EventArgs e)
		{
			fColor = true;
		}

		private void cbTextDecoration_TextChanged(object sender, System.EventArgs e)
		{
			fTextDecoration = true;
		}

		private void cbHorzAlign_TextChanged(object sender, System.EventArgs e)
		{
			fHorzAlign = true;
		}

		private void cbVerticalAlign_TextChanged(object sender, System.EventArgs e)
		{
			fVerticalAlign = true;
		}

		private void cbDirection_TextChanged(object sender, System.EventArgs e)
		{
			fDirection = true;
		}

		private void cbWritingMode_TextChanged(object sender, System.EventArgs e)
		{
			fWritingMode = true;
		}

		private void cbFormat_TextChanged(object sender, System.EventArgs e)
		{
			fFormat = true;
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
				case "family":
					c = cbFontFamily;
					break;
				case "style":
					c = cbFontStyle;
					break;
				case "color":
					c = cbColor;
					bColor = true;
					break;
				case "size":
					c = cbFontSize;
					break;
				case "weight":
					c = cbFontWeight;
					break;
				case "decoration":
					c = cbTextDecoration;
					break;
				case "halign":
					c = cbHorzAlign;
					break;
				case "valign":
					c = cbVerticalAlign;
					break;
				case "direction":
					c = cbDirection;
					break;
				case "writing":
					c = cbWritingMode;
					break;
				case "format":
					c = cbFormat;
					break;
			}

			if (c == null)
				return;

			XmlNode sNode = _ReportItems[0];

			DialogExprEditor ee = new DialogExprEditor(_Draw, c.Text, sNode, bColor);
            try
            {
                DialogResult dr = ee.ShowDialog();
                if (dr == DialogResult.OK)
                    c.Text = ee.Expression;
            }
            finally
            {
                ee.Dispose();
            }
			return;
		}

	}
}
