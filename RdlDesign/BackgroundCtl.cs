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
	internal class BackgroundCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		// flags for controlling whether syntax changed for a particular property
		private bool fEndColor, fBackColor, fGradient, fBackImage;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button bBackColor;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ComboBox cbEndColor;
		private System.Windows.Forms.ComboBox cbBackColor;
		private System.Windows.Forms.Button bEndColor;
        private System.Windows.Forms.ComboBox cbGradient;
		private System.Windows.Forms.Button bGradient;
		private System.Windows.Forms.Button bExprBackColor;
        private System.Windows.Forms.Button bExprEndColor;
        private GroupBox groupBox2;
        private Button bExternalExpr;
        private Button bEmbeddedExpr;
        private Button bMimeExpr;
        private Button bDatabaseExpr;
        private Button bEmbedded;
        private Button bExternal;
        private TextBox tbValueExternal;
        private ComboBox cbValueDatabase;
        private ComboBox cbMIMEType;
        private ComboBox cbValueEmbedded;
        private RadioButton rbDatabase;
        private RadioButton rbEmbedded;
        private RadioButton rbExternal;
        private RadioButton rbNone;
        private ComboBox cbRepeat;
        private Label label1;
        private Button bRepeatExpr;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private string[] _names;

        internal BackgroundCtl(DesignXmlDraw dxDraw, string[] names, List<XmlNode> reportItems)
		{
			_ReportItems = reportItems;
			_Draw = dxDraw;
            _names = names;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues(_ReportItems[0]);			
		}

		private void InitValues(XmlNode node)
		{
            cbEndColor.Items.AddRange(StaticLists.ColorList);
            cbBackColor.Items.AddRange(StaticLists.ColorList);
            cbGradient.Items.AddRange(StaticLists.GradientList);

            if (_names != null)
            {
                node = _Draw.FindCreateNextInHierarchy(node, _names);
            }

            XmlNode sNode = _Draw.GetNamedChildNode(node, "Style");

			this.cbBackColor.Text = _Draw.GetElementValue(sNode, "BackgroundColor", "");
			this.cbEndColor.Text = _Draw.GetElementValue(sNode, "BackgroundGradientEndColor", "");
			this.cbGradient.Text = _Draw.GetElementValue(sNode, "BackgroundGradientType", "None");
			if (node.Name != "Chart")
			{   // only chart support gradients
				this.cbEndColor.Enabled = bExprEndColor.Enabled =
					cbGradient.Enabled = bGradient.Enabled = 
					this.bEndColor.Enabled = bExprEndColor.Enabled = false;
			}

            cbValueEmbedded.Items.AddRange(_Draw.ReportNames.EmbeddedImageNames);
            string[] flds = _Draw.GetReportItemDataRegionFields(node, true);
            if (flds != null)
                this.cbValueDatabase.Items.AddRange(flds);

            XmlNode iNode = _Draw.GetNamedChildNode(sNode, "BackgroundImage");
            if (iNode != null)
            {
                string source = _Draw.GetElementValue(iNode, "Source", "Embedded");
                string val = _Draw.GetElementValue(iNode, "Value", "");
                switch (source)
                {
                    case "Embedded":
                        this.rbEmbedded.Checked = true;
                        this.cbValueEmbedded.Text = val;
                        break;
                    case "Database":
                        this.rbDatabase.Checked = true;
                        this.cbValueDatabase.Text = val;
                        this.cbMIMEType.Text = _Draw.GetElementValue(iNode, "MIMEType", "image/png");
                        break;
                    case "External":
                    default:
                        this.rbExternal.Checked = true;
                        this.tbValueExternal.Text = val;
                        break;
                }
                this.cbRepeat.Text = _Draw.GetElementValue(iNode, "BackgroundRepeat", "Repeat");
            }
            else
                this.rbNone.Checked = true;

            rbSource_CheckedChanged(null, null);

			// nothing has changed now
			fEndColor = fBackColor = fGradient = fBackImage = false;
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bRepeatExpr = new System.Windows.Forms.Button();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.cbRepeat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bExternalExpr = new System.Windows.Forms.Button();
            this.bEmbeddedExpr = new System.Windows.Forms.Button();
            this.bMimeExpr = new System.Windows.Forms.Button();
            this.bDatabaseExpr = new System.Windows.Forms.Button();
            this.bEmbedded = new System.Windows.Forms.Button();
            this.bExternal = new System.Windows.Forms.Button();
            this.tbValueExternal = new System.Windows.Forms.TextBox();
            this.cbValueDatabase = new System.Windows.Forms.ComboBox();
            this.cbMIMEType = new System.Windows.Forms.ComboBox();
            this.cbValueEmbedded = new System.Windows.Forms.ComboBox();
            this.rbDatabase = new System.Windows.Forms.RadioButton();
            this.rbEmbedded = new System.Windows.Forms.RadioButton();
            this.rbExternal = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bRepeatExpr);
            this.groupBox2.Controls.Add(this.rbNone);
            this.groupBox2.Controls.Add(this.cbRepeat);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.bExternalExpr);
            this.groupBox2.Controls.Add(this.bEmbeddedExpr);
            this.groupBox2.Controls.Add(this.bMimeExpr);
            this.groupBox2.Controls.Add(this.bDatabaseExpr);
            this.groupBox2.Controls.Add(this.bEmbedded);
            this.groupBox2.Controls.Add(this.bExternal);
            this.groupBox2.Controls.Add(this.tbValueExternal);
            this.groupBox2.Controls.Add(this.cbValueDatabase);
            this.groupBox2.Controls.Add(this.cbMIMEType);
            this.groupBox2.Controls.Add(this.cbValueEmbedded);
            this.groupBox2.Controls.Add(this.rbDatabase);
            this.groupBox2.Controls.Add(this.rbEmbedded);
            this.groupBox2.Controls.Add(this.rbExternal);
            this.groupBox2.Location = new System.Drawing.Point(16, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 219);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Background Image Source";
            // 
            // bRepeatExpr
            // 
            this.bRepeatExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bRepeatExpr.Location = new System.Drawing.Point(213, 184);
            this.bRepeatExpr.Name = "bRepeatExpr";
            this.bRepeatExpr.Size = new System.Drawing.Size(22, 16);
            this.bRepeatExpr.TabIndex = 16;
            this.bRepeatExpr.Tag = "repeat";
            this.bRepeatExpr.Text = "fx";
            this.bRepeatExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbNone
            // 
            this.rbNone.Location = new System.Drawing.Point(6, 25);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(80, 24);
            this.rbNone.TabIndex = 15;
            this.rbNone.Text = "None";
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
            // 
            // cbRepeat
            // 
            this.cbRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepeat.Items.AddRange(new object[] {
            "Repeat",
            "NoRepeat",
            "RepeatX",
            "RepeatY"});
            this.cbRepeat.Location = new System.Drawing.Point(87, 184);
            this.cbRepeat.Name = "cbRepeat";
            this.cbRepeat.Size = new System.Drawing.Size(120, 21);
            this.cbRepeat.TabIndex = 14;
            this.cbRepeat.SelectedIndexChanged += new System.EventHandler(this.BackImage_Changed);
            this.cbRepeat.TextChanged += new System.EventHandler(this.BackImage_Changed);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Repeat";
            // 
            // bExternalExpr
            // 
            this.bExternalExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExternalExpr.Location = new System.Drawing.Point(376, 56);
            this.bExternalExpr.Name = "bExternalExpr";
            this.bExternalExpr.Size = new System.Drawing.Size(22, 16);
            this.bExternalExpr.TabIndex = 12;
            this.bExternalExpr.Tag = "external";
            this.bExternalExpr.Text = "fx";
            this.bExternalExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bExternalExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bEmbeddedExpr
            // 
            this.bEmbeddedExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bEmbeddedExpr.Location = new System.Drawing.Point(376, 88);
            this.bEmbeddedExpr.Name = "bEmbeddedExpr";
            this.bEmbeddedExpr.Size = new System.Drawing.Size(22, 16);
            this.bEmbeddedExpr.TabIndex = 11;
            this.bEmbeddedExpr.Tag = "embedded";
            this.bEmbeddedExpr.Text = "fx";
            this.bEmbeddedExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bEmbeddedExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bMimeExpr
            // 
            this.bMimeExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bMimeExpr.Location = new System.Drawing.Point(182, 121);
            this.bMimeExpr.Name = "bMimeExpr";
            this.bMimeExpr.Size = new System.Drawing.Size(22, 16);
            this.bMimeExpr.TabIndex = 10;
            this.bMimeExpr.Tag = "mime";
            this.bMimeExpr.Text = "fx";
            this.bMimeExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bMimeExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bDatabaseExpr
            // 
            this.bDatabaseExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bDatabaseExpr.Location = new System.Drawing.Point(376, 152);
            this.bDatabaseExpr.Name = "bDatabaseExpr";
            this.bDatabaseExpr.Size = new System.Drawing.Size(22, 16);
            this.bDatabaseExpr.TabIndex = 9;
            this.bDatabaseExpr.Tag = "database";
            this.bDatabaseExpr.Text = "fx";
            this.bDatabaseExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bDatabaseExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // bEmbedded
            // 
            this.bEmbedded.Location = new System.Drawing.Point(402, 88);
            this.bEmbedded.Name = "bEmbedded";
            this.bEmbedded.Size = new System.Drawing.Size(22, 16);
            this.bEmbedded.TabIndex = 8;
            this.bEmbedded.Text = "...";
            this.bEmbedded.Click += new System.EventHandler(this.bEmbedded_Click);
            // 
            // bExternal
            // 
            this.bExternal.Location = new System.Drawing.Point(402, 56);
            this.bExternal.Name = "bExternal";
            this.bExternal.Size = new System.Drawing.Size(22, 16);
            this.bExternal.TabIndex = 7;
            this.bExternal.Text = "...";
            this.bExternal.Click += new System.EventHandler(this.bExternal_Click);
            // 
            // tbValueExternal
            // 
            this.tbValueExternal.Location = new System.Drawing.Point(86, 55);
            this.tbValueExternal.Name = "tbValueExternal";
            this.tbValueExternal.Size = new System.Drawing.Size(284, 20);
            this.tbValueExternal.TabIndex = 6;
            this.tbValueExternal.TextChanged += new System.EventHandler(this.BackImage_Changed);
            // 
            // cbValueDatabase
            // 
            this.cbValueDatabase.Location = new System.Drawing.Point(86, 151);
            this.cbValueDatabase.Name = "cbValueDatabase";
            this.cbValueDatabase.Size = new System.Drawing.Size(284, 21);
            this.cbValueDatabase.TabIndex = 5;
            this.cbValueDatabase.SelectedIndexChanged += new System.EventHandler(this.BackImage_Changed);
            this.cbValueDatabase.TextChanged += new System.EventHandler(this.BackImage_Changed);
            // 
            // cbMIMEType
            // 
            this.cbMIMEType.Items.AddRange(new object[] {
            "image/bmp",
            "image/jpeg",
            "image/gif",
            "image/png",
            "image/x-png"});
            this.cbMIMEType.Location = new System.Drawing.Point(86, 119);
            this.cbMIMEType.Name = "cbMIMEType";
            this.cbMIMEType.Size = new System.Drawing.Size(88, 21);
            this.cbMIMEType.TabIndex = 4;
            this.cbMIMEType.Text = "image/jpeg";
            this.cbMIMEType.SelectedIndexChanged += new System.EventHandler(this.BackImage_Changed);
            this.cbMIMEType.TextChanged += new System.EventHandler(this.BackImage_Changed);
            // 
            // cbValueEmbedded
            // 
            this.cbValueEmbedded.Location = new System.Drawing.Point(86, 87);
            this.cbValueEmbedded.Name = "cbValueEmbedded";
            this.cbValueEmbedded.Size = new System.Drawing.Size(284, 21);
            this.cbValueEmbedded.TabIndex = 3;
            this.cbValueEmbedded.SelectedIndexChanged += new System.EventHandler(this.BackImage_Changed);
            this.cbValueEmbedded.TextChanged += new System.EventHandler(this.BackImage_Changed);
            // 
            // rbDatabase
            // 
            this.rbDatabase.Location = new System.Drawing.Point(6, 119);
            this.rbDatabase.Name = "rbDatabase";
            this.rbDatabase.Size = new System.Drawing.Size(80, 24);
            this.rbDatabase.TabIndex = 2;
            this.rbDatabase.Text = "Database";
            this.rbDatabase.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
            // 
            // rbEmbedded
            // 
            this.rbEmbedded.Location = new System.Drawing.Point(6, 87);
            this.rbEmbedded.Name = "rbEmbedded";
            this.rbEmbedded.Size = new System.Drawing.Size(80, 24);
            this.rbEmbedded.TabIndex = 1;
            this.rbEmbedded.Text = "Embedded";
            this.rbEmbedded.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
            // 
            // rbExternal
            // 
            this.rbExternal.Location = new System.Drawing.Point(6, 55);
            this.rbExternal.Name = "rbExternal";
            this.rbExternal.Size = new System.Drawing.Size(80, 24);
            this.rbExternal.TabIndex = 0;
            this.rbExternal.Text = "External";
            this.rbExternal.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
            // 
            // BackgroundCtl
            // 
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "BackgroundCtl";
            this.Size = new System.Drawing.Size(472, 351);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
            fEndColor = fBackColor = fGradient = fBackImage = false;
		}

		private void bColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog cd = new ColorDialog();
			cd.AnyColor = true;
			cd.FullOpen = true;
			cd.CustomColors = RdlDesigner.GetCustomColors();

            try
            {
                if (cd.ShowDialog() != DialogResult.OK)
                    return;

                RdlDesigner.SetCustomColors(cd.CustomColors);
                if (sender == this.bEndColor)
                    cbEndColor.Text = ColorTranslator.ToHtml(cd.Color);
                else if (sender == this.bBackColor)
                    cbBackColor.Text = ColorTranslator.ToHtml(cd.Color);
            }
            finally
            {
                cd.Dispose();
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

        private void BackImage_Changed(object sender, System.EventArgs e)
        {
            fBackImage = true;
        }

        private void rbSource_CheckedChanged(object sender, System.EventArgs e)
        {
            fBackImage = true;
            this.cbValueDatabase.Enabled = this.cbMIMEType.Enabled =
                this.bDatabaseExpr.Enabled = this.rbDatabase.Checked;
            this.cbValueEmbedded.Enabled = this.bEmbeddedExpr.Enabled =
                this.bEmbedded.Enabled = this.rbEmbedded.Checked;
            this.tbValueExternal.Enabled = this.bExternalExpr.Enabled =
                this.bExternal.Enabled = this.rbExternal.Checked;
        }
		
		private void ApplyChanges(XmlNode rNode)
		{
            if (_names != null)
            {
                rNode = _Draw.FindCreateNextInHierarchy(rNode, _names);
            }
            
            XmlNode xNode = _Draw.GetNamedChildNode(rNode, "Style");

			if (fEndColor)
			{ _Draw.SetElement(xNode, "BackgroundGradientEndColor", cbEndColor.Text); }
			if (fBackColor)
			{ _Draw.SetElement(xNode, "BackgroundColor", cbBackColor.Text); }
			if (fGradient)
			{ _Draw.SetElement(xNode, "BackgroundGradientType", cbGradient.Text); }
            if (fBackImage)
            {
                _Draw.RemoveElement(xNode, "BackgroundImage");
                if (!rbNone.Checked)
                {
                    XmlNode bi = _Draw.CreateElement(xNode, "BackgroundImage", null);
                    if (rbDatabase.Checked)
                    {
                        _Draw.SetElement(bi, "Source", "Database");
                        _Draw.SetElement(bi, "Value", cbValueDatabase.Text);
                        _Draw.SetElement(bi, "MIMEType", cbMIMEType.Text);
                    }
                    else if (rbExternal.Checked)
                    {
                        _Draw.SetElement(bi, "Source", "External");
                        _Draw.SetElement(bi, "Value", tbValueExternal.Text);
                    }
                    else if (rbEmbedded.Checked)
                    {
                        _Draw.SetElement(bi, "Source", "Embedded");
                        _Draw.SetElement(bi, "Value", cbValueEmbedded.Text);
                    }
                    _Draw.SetElement(bi, "BackgroundRepeat", cbRepeat.Text);
                }
            }
		}

        private void bExternal_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap Files (*.bmp)|*.bmp" +
                "|JPEG (*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif" +
                "|GIF (*.gif)|*.gif" +
                "|TIFF (*.tif;*.tiff)|*.tif;*.tiff" +
                "|PNG (*.png)|*.png" +
                "|All Picture Files|*.bmp;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.tif;*.tiff;*.png" +
                "|All files (*.*)|*.*";
            ofd.FilterIndex = 6;
            ofd.CheckFileExists = true;
            try
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    tbValueExternal.Text = ofd.FileName;
                }
            }
            finally
            {
                ofd.Dispose();
            }
        }

        private void bEmbedded_Click(object sender, System.EventArgs e)
        {
            DialogEmbeddedImages dlgEI = new DialogEmbeddedImages(this._Draw);
            dlgEI.StartPosition = FormStartPosition.CenterParent;
            try
            {
                DialogResult dr = dlgEI.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                // Populate the EmbeddedImage names
                cbValueEmbedded.Items.Clear();
                cbValueEmbedded.Items.AddRange(_Draw.ReportNames.EmbeddedImageNames);
            }
            finally
            {
                dlgEI.Dispose();
            }
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
                case "database":
                    c = cbValueDatabase;
                    break;
                case "embedded":
                    c = cbValueEmbedded;
                    break;
                case "external":
                    c = tbValueExternal;
                    break;
                case "repeat":
                    c = cbRepeat;
                    break;
                case "mime":
                    c = cbMIMEType;
                    break;
            }

			if (c == null)
				return;

			XmlNode sNode = _ReportItems[0];

			DialogExprEditor ee = new DialogExprEditor(_Draw, c.Text, sNode, bColor);
			DialogResult dr = ee.ShowDialog();
			if (dr == DialogResult.OK)
				c.Text = ee.Expression;
			return;
		}

	}
}
