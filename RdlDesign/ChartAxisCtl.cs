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
	/// Summary description for ChartCtl.
	/// </summary>
	internal class ChartAxisCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		// change flags
		bool fMonth, fVisible, fMajorTickMarks, fMargin,fReverse,fInterlaced;
		bool fMajorGLWidth,fMajorGLColor,fMajorGLStyle;
		bool fMinorGLWidth,fMinorGLColor,fMinorGLStyle;
		bool fMajorInterval, fMinorInterval,fMax,fMin;
		bool fMinorTickMarks,fScalar,fLogScale,fMajorGLShow, fMinorGLShow, fCanOmit;
		
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkMonth;
		private System.Windows.Forms.CheckBox chkVisible;
		private System.Windows.Forms.ComboBox cbMajorTickMarks;
		private System.Windows.Forms.CheckBox chkMargin;
		private System.Windows.Forms.CheckBox chkReverse;
		private System.Windows.Forms.CheckBox chkInterlaced;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tbMajorGLWidth;
		private System.Windows.Forms.Button bMajorGLColor;
		private System.Windows.Forms.ComboBox cbMajorGLColor;
		private System.Windows.Forms.ComboBox cbMajorGLStyle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox tbMinorGLWidth;
		private System.Windows.Forms.Button bMinorGLColor;
		private System.Windows.Forms.ComboBox cbMinorGLColor;
		private System.Windows.Forms.ComboBox cbMinorGLStyle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox tbMajorInterval;
		private System.Windows.Forms.TextBox tbMinorInterval;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbMax;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox tbMin;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cbMinorTickMarks;
		private System.Windows.Forms.CheckBox chkScalar;
		private System.Windows.Forms.CheckBox chkLogScale;
		private System.Windows.Forms.CheckBox chkMajorGLShow;
		private System.Windows.Forms.CheckBox chkMinorGLShow;
		private System.Windows.Forms.Button bMinorIntervalExpr;
		private System.Windows.Forms.Button bMajorIntervalExpr;
		private System.Windows.Forms.Button bMinExpr;
		private System.Windows.Forms.Button bMaxExpr;
        private CheckBox chkCanOmit;
        
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal ChartAxisCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
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
            cbMajorGLColor.Items.AddRange(StaticLists.ColorList);
            cbMinorGLColor.Items.AddRange(StaticLists.ColorList);

			XmlNode node = _ReportItems[0];
            chkMonth.Checked = _Draw.GetElementValue(node, "fyi:Month", "false").ToLower() == "true" ? true : false; //added checkbox for month category axis WP 12 may 2008
			chkVisible.Checked = _Draw.GetElementValue(node, "Visible", "false").ToLower() == "true"? true: false;
			chkMargin.Checked = _Draw.GetElementValue(node, "Margin", "false").ToLower() == "true"? true: false;
			chkReverse.Checked = _Draw.GetElementValue(node, "Reverse", "false").ToLower() == "true"? true: false;
			chkInterlaced.Checked = _Draw.GetElementValue(node, "Interlaced", "false").ToLower() == "true"? true: false;
			chkScalar.Checked = _Draw.GetElementValue(node, "Scalar", "false").ToLower() == "true"? true: false;
			chkLogScale.Checked = _Draw.GetElementValue(node, "LogScale", "false").ToLower() == "true"? true: false;
            chkCanOmit.Checked = _Draw.GetElementValue(node, "fyi:CanOmit", "false").ToLower() == "true" ? true : false;
            cbMajorTickMarks.Text = _Draw.GetElementValue(node, "MajorTickMarks", "None");
			cbMinorTickMarks.Text = _Draw.GetElementValue(node, "MinorTickMarks", "None");
			// Major Grid Lines
			InitGridLines(node, "MajorGridLines", chkMajorGLShow, cbMajorGLColor, cbMajorGLStyle, tbMajorGLWidth);
			// Minor Grid Lines
			InitGridLines(node, "MinorGridLines", chkMinorGLShow, cbMinorGLColor, cbMinorGLStyle, tbMinorGLWidth);

			tbMajorInterval.Text = _Draw.GetElementValue(node, "MajorInterval", "");
			tbMinorInterval.Text = _Draw.GetElementValue(node, "MinorInterval", "");
			tbMax.Text = _Draw.GetElementValue(node, "Max", "");
			tbMin.Text = _Draw.GetElementValue(node, "Min", "");

			    fMonth = fVisible = fMajorTickMarks = fMargin=fReverse=fInterlaced=
				fMajorGLWidth=fMajorGLColor=fMajorGLStyle=
				fMinorGLWidth=fMinorGLColor=fMinorGLStyle=
				fMajorInterval= fMinorInterval=fMax=fMin=
				fMinorTickMarks=fScalar=fLogScale=fMajorGLShow=fMinorGLShow=fCanOmit=false;
		}

		private void InitGridLines(XmlNode node, string type, CheckBox show, ComboBox color, ComboBox style, TextBox width)
		{
			XmlNode m = _Draw.GetNamedChildNode(node, type);
			if (m != null)
			{
				show.Checked = _Draw.GetElementValue(m, "ShowGridLines", "false").ToLower() == "true"? true: false;
				XmlNode st = _Draw.GetNamedChildNode(m, "Style");
				if (st != null)
				{
					XmlNode work = _Draw.GetNamedChildNode(st, "BorderColor");
					if (work != null)
						color.Text = _Draw.GetElementValue(work, "Default", "Black");
					work = _Draw.GetNamedChildNode(st, "BorderStyle");
					if (work != null)
						style.Text = _Draw.GetElementValue(work, "Default", "Solid");
					work = _Draw.GetNamedChildNode(st, "BorderWidth");
					if (work != null)
						width.Text = _Draw.GetElementValue(work, "Default", "1pt");
				}
			}
			if (color.Text.Length == 0)
				color.Text = "Black";
			if (style.Text.Length == 0)
				style.Text = "Solid";
			if (width.Text.Length == 0)
				width.Text = "1pt";
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
            this.cbMajorTickMarks = new System.Windows.Forms.ComboBox();
            this.cbMinorTickMarks = new System.Windows.Forms.ComboBox();
            this.chkVisible = new System.Windows.Forms.CheckBox();
            this.chkMargin = new System.Windows.Forms.CheckBox();
            this.chkReverse = new System.Windows.Forms.CheckBox();
            this.chkInterlaced = new System.Windows.Forms.CheckBox();
            this.chkScalar = new System.Windows.Forms.CheckBox();
            this.chkLogScale = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMajorGLShow = new System.Windows.Forms.CheckBox();
            this.tbMajorGLWidth = new System.Windows.Forms.TextBox();
            this.bMajorGLColor = new System.Windows.Forms.Button();
            this.cbMajorGLColor = new System.Windows.Forms.ComboBox();
            this.cbMajorGLStyle = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMinorGLShow = new System.Windows.Forms.CheckBox();
            this.tbMinorGLWidth = new System.Windows.Forms.TextBox();
            this.bMinorGLColor = new System.Windows.Forms.Button();
            this.cbMinorGLColor = new System.Windows.Forms.ComboBox();
            this.cbMinorGLStyle = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbMajorInterval = new System.Windows.Forms.TextBox();
            this.tbMinorInterval = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.bMinorIntervalExpr = new System.Windows.Forms.Button();
            this.bMajorIntervalExpr = new System.Windows.Forms.Button();
            this.bMinExpr = new System.Windows.Forms.Button();
            this.bMaxExpr = new System.Windows.Forms.Button();
            this.chkCanOmit = new System.Windows.Forms.CheckBox();
            this.chkMonth = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Major Tick Marks";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(224, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minor Tick Marks";
            // 
            // cbMajorTickMarks
            // 
            this.cbMajorTickMarks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMajorTickMarks.Items.AddRange(new object[] {
            "None",
            "Inside",
            "Outside",
            "Cross"});
            this.cbMajorTickMarks.Location = new System.Drawing.Point(128, 8);
            this.cbMajorTickMarks.Name = "cbMajorTickMarks";
            this.cbMajorTickMarks.Size = new System.Drawing.Size(80, 21);
            this.cbMajorTickMarks.TabIndex = 2;
            this.cbMajorTickMarks.SelectedIndexChanged += new System.EventHandler(this.cbMajorTickMarks_SelectedIndexChanged);
            // 
            // cbMinorTickMarks
            // 
            this.cbMinorTickMarks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMinorTickMarks.Items.AddRange(new object[] {
            "None",
            "Inside",
            "Outside",
            "Cross"});
            this.cbMinorTickMarks.Location = new System.Drawing.Point(336, 8);
            this.cbMinorTickMarks.Name = "cbMinorTickMarks";
            this.cbMinorTickMarks.Size = new System.Drawing.Size(80, 21);
            this.cbMinorTickMarks.TabIndex = 4;
            this.cbMinorTickMarks.SelectedIndexChanged += new System.EventHandler(this.cbMinorTickMarks_SelectedIndexChanged);
            // 
            // chkVisible
            // 
            this.chkVisible.Location = new System.Drawing.Point(24, 224);
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.Size = new System.Drawing.Size(88, 24);
            this.chkVisible.TabIndex = 19;
            this.chkVisible.Text = "Visible";
            this.chkVisible.CheckedChanged += new System.EventHandler(this.chkVisible_CheckedChanged);
            // 
            // chkMargin
            // 
            this.chkMargin.Location = new System.Drawing.Point(240, 224);
            this.chkMargin.Name = "chkMargin";
            this.chkMargin.Size = new System.Drawing.Size(60, 24);
            this.chkMargin.TabIndex = 21;
            this.chkMargin.Text = "Margin";
            this.chkMargin.CheckedChanged += new System.EventHandler(this.chkMargin_CheckedChanged);
            // 
            // chkReverse
            // 
            this.chkReverse.Location = new System.Drawing.Point(108, 248);
            this.chkReverse.Name = "chkReverse";
            this.chkReverse.Size = new System.Drawing.Size(120, 24);
            this.chkReverse.TabIndex = 23;
            this.chkReverse.Text = "Reverse Direction";
            this.chkReverse.CheckedChanged += new System.EventHandler(this.chkReverse_CheckedChanged);
            // 
            // chkInterlaced
            // 
            this.chkInterlaced.Location = new System.Drawing.Point(240, 248);
            this.chkInterlaced.Name = "chkInterlaced";
            this.chkInterlaced.Size = new System.Drawing.Size(88, 24);
            this.chkInterlaced.TabIndex = 23;
            this.chkInterlaced.Text = "Interlaced";
            this.chkInterlaced.CheckedChanged += new System.EventHandler(this.chkInterlaced_CheckedChanged);
            // 
            // chkScalar
            // 
            this.chkScalar.Location = new System.Drawing.Point(24, 248);
            this.chkScalar.Name = "chkScalar";
            this.chkScalar.Size = new System.Drawing.Size(72, 24);
            this.chkScalar.TabIndex = 22;
            this.chkScalar.Text = "Scalar";
            this.chkScalar.CheckedChanged += new System.EventHandler(this.chkScalar_CheckedChanged);
            // 
            // chkLogScale
            // 
            this.chkLogScale.Location = new System.Drawing.Point(108, 224);
            this.chkLogScale.Name = "chkLogScale";
            this.chkLogScale.Size = new System.Drawing.Size(120, 24);
            this.chkLogScale.TabIndex = 20;
            this.chkLogScale.Text = "Log Scale";
            this.chkLogScale.CheckedChanged += new System.EventHandler(this.chkLogScale_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMajorGLShow);
            this.groupBox1.Controls.Add(this.tbMajorGLWidth);
            this.groupBox1.Controls.Add(this.bMajorGLColor);
            this.groupBox1.Controls.Add(this.cbMajorGLColor);
            this.groupBox1.Controls.Add(this.cbMajorGLStyle);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(16, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 48);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Major Grid Lines";
            // 
            // chkMajorGLShow
            // 
            this.chkMajorGLShow.Location = new System.Drawing.Point(8, 14);
            this.chkMajorGLShow.Name = "chkMajorGLShow";
            this.chkMajorGLShow.Size = new System.Drawing.Size(56, 24);
            this.chkMajorGLShow.TabIndex = 0;
            this.chkMajorGLShow.Text = "Show";
            this.chkMajorGLShow.CheckedChanged += new System.EventHandler(this.chkMajorGLShow_CheckedChanged);
            // 
            // tbMajorGLWidth
            // 
            this.tbMajorGLWidth.Location = new System.Drawing.Point(352, 16);
            this.tbMajorGLWidth.Name = "tbMajorGLWidth";
            this.tbMajorGLWidth.Size = new System.Drawing.Size(40, 20);
            this.tbMajorGLWidth.TabIndex = 7;
            this.tbMajorGLWidth.TextChanged += new System.EventHandler(this.tbMajorGLWidth_TextChanged);
            // 
            // bMajorGLColor
            // 
            this.bMajorGLColor.Location = new System.Drawing.Point(288, 14);
            this.bMajorGLColor.Name = "bMajorGLColor";
            this.bMajorGLColor.Size = new System.Drawing.Size(24, 24);
            this.bMajorGLColor.TabIndex = 5;
            this.bMajorGLColor.Text = "...";
            this.bMajorGLColor.Click += new System.EventHandler(this.bMajorGLColor_Click);
            // 
            // cbMajorGLColor
            // 
            this.cbMajorGLColor.Location = new System.Drawing.Point(208, 16);
            this.cbMajorGLColor.Name = "cbMajorGLColor";
            this.cbMajorGLColor.Size = new System.Drawing.Size(72, 21);
            this.cbMajorGLColor.TabIndex = 4;
            this.cbMajorGLColor.SelectedIndexChanged += new System.EventHandler(this.cbMajorGLColor_SelectedIndexChanged);
            // 
            // cbMajorGLStyle
            // 
            this.cbMajorGLStyle.Items.AddRange(new object[] {
            "None",
            "Dotted",
            "Dashed",
            "Solid",
            "Double",
            "Groove",
            "Ridge",
            "Inset",
            "WindowInset",
            "Outset"});
            this.cbMajorGLStyle.Location = new System.Drawing.Point(96, 16);
            this.cbMajorGLStyle.Name = "cbMajorGLStyle";
            this.cbMajorGLStyle.Size = new System.Drawing.Size(72, 21);
            this.cbMajorGLStyle.TabIndex = 2;
            this.cbMajorGLStyle.SelectedIndexChanged += new System.EventHandler(this.cbMajorGLStyle_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(176, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "Color";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(320, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Width";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(64, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Style";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkMinorGLShow);
            this.groupBox2.Controls.Add(this.tbMinorGLWidth);
            this.groupBox2.Controls.Add(this.bMinorGLColor);
            this.groupBox2.Controls.Add(this.cbMinorGLColor);
            this.groupBox2.Controls.Add(this.cbMinorGLStyle);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(16, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 48);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Minor Grid Lines";
            // 
            // chkMinorGLShow
            // 
            this.chkMinorGLShow.Location = new System.Drawing.Point(8, 14);
            this.chkMinorGLShow.Name = "chkMinorGLShow";
            this.chkMinorGLShow.Size = new System.Drawing.Size(56, 24);
            this.chkMinorGLShow.TabIndex = 0;
            this.chkMinorGLShow.Text = "Show";
            this.chkMinorGLShow.CheckedChanged += new System.EventHandler(this.chkMinorGLShow_CheckedChanged);
            // 
            // tbMinorGLWidth
            // 
            this.tbMinorGLWidth.Location = new System.Drawing.Point(352, 16);
            this.tbMinorGLWidth.Name = "tbMinorGLWidth";
            this.tbMinorGLWidth.Size = new System.Drawing.Size(40, 20);
            this.tbMinorGLWidth.TabIndex = 7;
            this.tbMinorGLWidth.TextChanged += new System.EventHandler(this.tbMinorGLWidth_TextChanged);
            // 
            // bMinorGLColor
            // 
            this.bMinorGLColor.Location = new System.Drawing.Point(288, 14);
            this.bMinorGLColor.Name = "bMinorGLColor";
            this.bMinorGLColor.Size = new System.Drawing.Size(24, 24);
            this.bMinorGLColor.TabIndex = 5;
            this.bMinorGLColor.Text = "...";
            this.bMinorGLColor.Click += new System.EventHandler(this.bMinorGLColor_Click);
            // 
            // cbMinorGLColor
            // 
            this.cbMinorGLColor.Location = new System.Drawing.Point(208, 16);
            this.cbMinorGLColor.Name = "cbMinorGLColor";
            this.cbMinorGLColor.Size = new System.Drawing.Size(72, 21);
            this.cbMinorGLColor.TabIndex = 4;
            this.cbMinorGLColor.SelectedIndexChanged += new System.EventHandler(this.cbMinorGLColor_SelectedIndexChanged);
            // 
            // cbMinorGLStyle
            // 
            this.cbMinorGLStyle.Items.AddRange(new object[] {
            "None",
            "Dotted",
            "Dashed",
            "Solid",
            "Double",
            "Groove",
            "Ridge",
            "Inset",
            "WindowInset",
            "Outset"});
            this.cbMinorGLStyle.Location = new System.Drawing.Point(96, 16);
            this.cbMinorGLStyle.Name = "cbMinorGLStyle";
            this.cbMinorGLStyle.Size = new System.Drawing.Size(72, 21);
            this.cbMinorGLStyle.TabIndex = 2;
            this.cbMinorGLStyle.SelectedIndexChanged += new System.EventHandler(this.cbMinorGLStyle_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(176, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Color";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(320, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Width";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(64, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Style";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 16);
            this.label9.TabIndex = 7;
            this.label9.Text = "Major Interval";
            // 
            // tbMajorInterval
            // 
            this.tbMajorInterval.Location = new System.Drawing.Point(104, 152);
            this.tbMajorInterval.Name = "tbMajorInterval";
            this.tbMajorInterval.Size = new System.Drawing.Size(65, 20);
            this.tbMajorInterval.TabIndex = 8;
            this.tbMajorInterval.TextChanged += new System.EventHandler(this.tbMajorInterval_TextChanged);
            // 
            // tbMinorInterval
            // 
            this.tbMinorInterval.Location = new System.Drawing.Point(302, 152);
            this.tbMinorInterval.Name = "tbMinorInterval";
            this.tbMinorInterval.Size = new System.Drawing.Size(65, 20);
            this.tbMinorInterval.TabIndex = 11;
            this.tbMinorInterval.TextChanged += new System.EventHandler(this.tbMinorInterval_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(217, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 16);
            this.label10.TabIndex = 10;
            this.label10.Text = "Minor Interval";
            // 
            // tbMax
            // 
            this.tbMax.Location = new System.Drawing.Point(302, 182);
            this.tbMax.Name = "tbMax";
            this.tbMax.Size = new System.Drawing.Size(65, 20);
            this.tbMax.TabIndex = 17;
            this.tbMax.TextChanged += new System.EventHandler(this.tbMax_TextChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(216, 184);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 16);
            this.label11.TabIndex = 16;
            this.label11.Text = "Maximum Value";
            // 
            // tbMin
            // 
            this.tbMin.Location = new System.Drawing.Point(104, 182);
            this.tbMin.Name = "tbMin";
            this.tbMin.Size = new System.Drawing.Size(65, 20);
            this.tbMin.TabIndex = 14;
            this.tbMin.TextChanged += new System.EventHandler(this.tbMin_TextChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(16, 184);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 16);
            this.label12.TabIndex = 13;
            this.label12.Text = "Minimum Value";
            // 
            // bMinorIntervalExpr
            // 
            this.bMinorIntervalExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bMinorIntervalExpr.Location = new System.Drawing.Point(375, 154);
            this.bMinorIntervalExpr.Name = "bMinorIntervalExpr";
            this.bMinorIntervalExpr.Size = new System.Drawing.Size(22, 16);
            this.bMinorIntervalExpr.TabIndex = 12;
            this.bMinorIntervalExpr.Tag = "minorinterval";
            this.bMinorIntervalExpr.Text = "fx";
            this.bMinorIntervalExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMinorIntervalExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bMajorIntervalExpr
            // 
            this.bMajorIntervalExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bMajorIntervalExpr.Location = new System.Drawing.Point(177, 154);
            this.bMajorIntervalExpr.Name = "bMajorIntervalExpr";
            this.bMajorIntervalExpr.Size = new System.Drawing.Size(22, 16);
            this.bMajorIntervalExpr.TabIndex = 9;
            this.bMajorIntervalExpr.Tag = "majorinterval";
            this.bMajorIntervalExpr.Text = "fx";
            this.bMajorIntervalExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMajorIntervalExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bMinExpr
            // 
            this.bMinExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bMinExpr.Location = new System.Drawing.Point(177, 184);
            this.bMinExpr.Name = "bMinExpr";
            this.bMinExpr.Size = new System.Drawing.Size(22, 16);
            this.bMinExpr.TabIndex = 15;
            this.bMinExpr.Tag = "min";
            this.bMinExpr.Text = "fx";
            this.bMinExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMinExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bMaxExpr
            // 
            this.bMaxExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bMaxExpr.Location = new System.Drawing.Point(376, 184);
            this.bMaxExpr.Name = "bMaxExpr";
            this.bMaxExpr.Size = new System.Drawing.Size(22, 16);
            this.bMaxExpr.TabIndex = 18;
            this.bMaxExpr.Tag = "max";
            this.bMaxExpr.Text = "fx";
            this.bMaxExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMaxExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // chkCanOmit
            // 
            this.chkCanOmit.Location = new System.Drawing.Point(334, 224);
            this.chkCanOmit.Name = "chkCanOmit";
            this.chkCanOmit.Size = new System.Drawing.Size(93, 48);
            this.chkCanOmit.TabIndex = 24;
            this.chkCanOmit.Text = "Can Omit Values on Truncation";
            this.chkCanOmit.CheckedChanged += new System.EventHandler(this.chkCanOmit_CheckedChanged);
            // 
            // chkMonth
            // 
            this.chkMonth.Location = new System.Drawing.Point(24, 272);
            this.chkMonth.Name = "chkMonth";
            this.chkMonth.Size = new System.Drawing.Size(145, 24);
            this.chkMonth.TabIndex = 25;
            this.chkMonth.Text = "Month Category Scale";
            this.chkMonth.CheckedChanged += new System.EventHandler(this.chkMonth_CheckedChanged);
            // 
            // ChartAxisCtl
            // 
            this.Controls.Add(this.chkMonth);
            this.Controls.Add(this.chkCanOmit);
            this.Controls.Add(this.bMaxExpr);
            this.Controls.Add(this.bMinExpr);
            this.Controls.Add(this.bMajorIntervalExpr);
            this.Controls.Add(this.bMinorIntervalExpr);
            this.Controls.Add(this.tbMax);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbMin);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbMinorInterval);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkLogScale);
            this.Controls.Add(this.chkScalar);
            this.Controls.Add(this.chkInterlaced);
            this.Controls.Add(this.chkReverse);
            this.Controls.Add(this.chkMargin);
            this.Controls.Add(this.chkVisible);
            this.Controls.Add(this.cbMinorTickMarks);
            this.Controls.Add(this.cbMajorTickMarks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbMajorInterval);
            this.Controls.Add(this.label9);
            this.Name = "ChartAxisCtl";
            this.Size = new System.Drawing.Size(440, 303);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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

			    fMonth = fVisible = fMajorTickMarks = fMargin=fReverse=fInterlaced=
				fMajorGLWidth=fMajorGLColor=fMajorGLStyle=
				fMinorGLWidth=fMinorGLColor=fMinorGLStyle=
				fMajorInterval= fMinorInterval=fMax=fMin=
				fMinorTickMarks=fScalar=fLogScale=fMajorGLShow=fMinorGLShow=fCanOmit=false;
		}

		public void ApplyChanges(XmlNode node)
		{
            if (fMonth)
            {
                _Draw.SetElement(node, "fyi:Month", this.chkMonth.Checked? "true" : "false");
            }
			if (fVisible)
			{
				_Draw.SetElement(node, "Visible", this.chkVisible.Checked? "true": "false");
			}
			if (fMajorTickMarks)
			{
				_Draw.SetElement(node, "MajorTickMarks", this.cbMajorTickMarks.Text);
			}
			if (fMargin)
			{
				_Draw.SetElement(node, "Margin", this.chkMargin.Checked? "true": "false");
			}
			if (fReverse)
			{
				_Draw.SetElement(node, "Reverse", this.chkReverse.Checked? "true": "false");
			}
			if (fInterlaced)
			{
				_Draw.SetElement(node, "Interlaced", this.chkInterlaced.Checked? "true": "false");
			}
			if (fMajorGLShow || fMajorGLWidth || fMajorGLColor || fMajorGLStyle)
			{
				ApplyGridLines(node, "MajorGridLines", chkMajorGLShow, cbMajorGLColor, cbMajorGLStyle, tbMajorGLWidth);
			}
			if (fMinorGLShow || fMinorGLWidth || fMinorGLColor || fMinorGLStyle)
			{
				ApplyGridLines(node, "MinorGridLines", chkMinorGLShow, cbMinorGLColor, cbMinorGLStyle, tbMinorGLWidth);
			}
			if (fMajorInterval)
			{
				_Draw.SetElement(node, "MajorInterval", this.tbMajorInterval.Text);
			}
			if (fMinorInterval)
			{
				_Draw.SetElement(node, "MinorInterval", this.tbMinorInterval.Text);
			}
			if (fMax)
			{
				_Draw.SetElement(node, "Max", this.tbMax.Text);
			}
			if (fMin)
			{
				_Draw.SetElement(node, "Min", this.tbMin.Text);
			}
			if (fMinorTickMarks)
			{
				_Draw.SetElement(node, "MinorTickMarks", this.cbMinorTickMarks.Text);
			}
			if (fScalar)
			{
				_Draw.SetElement(node, "Scalar", this.chkScalar.Checked? "true": "false");
			}
			if (fLogScale)
			{
				_Draw.SetElement(node, "LogScale", this.chkLogScale.Checked? "true": "false");
			}
            if (fCanOmit)
            {
                _Draw.SetElement(node, "fyi:CanOmit", this.chkCanOmit.Checked ? "true" : "false");
            }
        }

		private void ApplyGridLines(XmlNode node, string type, CheckBox show, ComboBox color, ComboBox style, TextBox width)
		{
			XmlNode m = _Draw.GetNamedChildNode(node, type);
			if (m == null)
			{
				m = _Draw.CreateElement(node, type, null);
			}

			_Draw.SetElement(m, "ShowGridLines", show.Checked? "true": "false");
			XmlNode st = _Draw.GetNamedChildNode(m, "Style");
			if (st == null)
				st = _Draw.CreateElement(m, "Style", null);

			XmlNode work = _Draw.GetNamedChildNode(st, "BorderColor");
			if (work == null)
				work = _Draw.CreateElement(st, "BorderColor", null);
			_Draw.SetElement(work, "Default", color.Text);

			work = _Draw.GetNamedChildNode(st, "BorderStyle");
			if (work == null)
				work = _Draw.CreateElement(st, "BorderStyle", null);
			_Draw.SetElement(work, "Default", style.Text);
			
			work = _Draw.GetNamedChildNode(st, "BorderWidth");
			if (work == null)
				work = _Draw.CreateElement(st, "BorderWidth", null);
			_Draw.SetElement(work, "Default", width.Text);
		}

		private void cbMajorTickMarks_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMajorTickMarks = true;
		}

		private void cbMinorTickMarks_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMinorTickMarks = true;
		}

		private void cbMajorGLStyle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMajorGLStyle = true;
		}

		private void cbMajorGLColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMajorGLColor = true;
		}

		private void tbMajorGLWidth_TextChanged(object sender, System.EventArgs e)
		{
			fMajorGLWidth = true;
		}

		private void cbMinorGLStyle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMinorGLStyle = true;
		}

		private void cbMinorGLColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMinorGLColor = true;
		}

		private void tbMinorGLWidth_TextChanged(object sender, System.EventArgs e)
		{
			fMinorGLWidth = true;
		}

		private void tbMajorInterval_TextChanged(object sender, System.EventArgs e)
		{
			fMajorInterval = true;
		}

		private void tbMinorInterval_TextChanged(object sender, System.EventArgs e)
		{
			fMinorInterval = true;
		}

		private void tbMin_TextChanged(object sender, System.EventArgs e)
		{
			fMin = true;
		}

		private void tbMax_TextChanged(object sender, System.EventArgs e)
		{
			fMax = true;
		}

        private void chkMonth_CheckedChanged(object sender, System.EventArgs e)
        {
            fMonth = true;
        }

		private void chkVisible_CheckedChanged(object sender, System.EventArgs e)
		{
			fVisible = true;
		}

		private void chkLogScale_CheckedChanged(object sender, System.EventArgs e)
		{
			fLogScale = true;
		}

        private void chkCanOmit_CheckedChanged(object sender, System.EventArgs e)
        {
            fCanOmit = true;
        }

		private void chkMargin_CheckedChanged(object sender, System.EventArgs e)
		{
			fMargin = true;
		}

		private void chkScalar_CheckedChanged(object sender, System.EventArgs e)
		{
			fScalar = true;
		}

		private void chkReverse_CheckedChanged(object sender, System.EventArgs e)
		{
			fReverse = true;
		}

		private void chkInterlaced_CheckedChanged(object sender, System.EventArgs e)
		{
			fInterlaced = true;
		}

		private void chkMajorGLShow_CheckedChanged(object sender, System.EventArgs e)
		{
			fMajorGLShow = true;
		}

		private void chkMinorGLShow_CheckedChanged(object sender, System.EventArgs e)
		{
			fMinorGLShow = true;
		}

		private void bMajorGLColor_Click(object sender, System.EventArgs e)
		{
			SetColor(this.cbMajorGLColor);		
		}

		private void bMinorGLColor_Click(object sender, System.EventArgs e)
		{
			SetColor(this.cbMinorGLColor);		
		}

		private void SetColor(ComboBox cbColor)
		{
			ColorDialog cd = new ColorDialog();
			cd.AnyColor = true;
			cd.FullOpen = true;
			
			cd.CustomColors = RdlDesigner.GetCustomColors();
			cd.Color = DesignerUtility.ColorFromHtml(cbColor.Text, System.Drawing.Color.Empty);

            try
            {
                if (cd.ShowDialog() != DialogResult.OK)
                    return;

                RdlDesigner.SetCustomColors(cd.CustomColors);
                cbColor.Text = ColorTranslator.ToHtml(cd.Color);
            }
            finally
            {
                cd.Dispose();
            }

			return;
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
				case "min":
					c = this.tbMin;
					break;
				case "max":
					c = this.tbMax;
					break;
				case "majorinterval":
					c = this.tbMajorInterval;
					break;
				case "minorinterval":
					c = this.tbMinorInterval;
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
