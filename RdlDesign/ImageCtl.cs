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
	internal class ImageCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		bool fSource, fValue, fSizing, fMIMEType;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbExternal;
		private System.Windows.Forms.RadioButton rbDatabase;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbSizing;
		private System.Windows.Forms.ComboBox cbValueEmbedded;
		private System.Windows.Forms.ComboBox cbMIMEType;
		private System.Windows.Forms.ComboBox cbValueDatabase;
		private System.Windows.Forms.TextBox tbValueExternal;
		private System.Windows.Forms.Button bExternal;
		private System.Windows.Forms.RadioButton rbEmbedded;
		private System.Windows.Forms.Button bEmbedded;
		private System.Windows.Forms.Button bDatabaseExpr;
		private System.Windows.Forms.Button bMimeExpr;
		private System.Windows.Forms.Button bEmbeddedExpr;
		private System.Windows.Forms.Button bExternalExpr;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal ImageCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
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
			XmlNode iNode =  _ReportItems[0];

			// Populate the EmbeddedImage names
			cbValueEmbedded.Items.AddRange(_Draw.ReportNames.EmbeddedImageNames);
			string[] flds = _Draw.GetReportItemDataRegionFields(iNode, true);
			if (flds != null)
				this.cbValueDatabase.Items.AddRange(flds);

			string source = _Draw.GetElementValue(iNode, "Source", "Embedded");
			string val =  _Draw.GetElementValue(iNode, "Value", "");
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
			this.cbSizing.Text = _Draw.GetElementValue(iNode, "Sizing", "AutoSize");

			fSource = fValue = fSizing = fMIMEType = false;
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
			this.bEmbedded = new System.Windows.Forms.Button();
			this.bExternal = new System.Windows.Forms.Button();
			this.tbValueExternal = new System.Windows.Forms.TextBox();
			this.cbValueDatabase = new System.Windows.Forms.ComboBox();
			this.cbMIMEType = new System.Windows.Forms.ComboBox();
			this.cbValueEmbedded = new System.Windows.Forms.ComboBox();
			this.rbDatabase = new System.Windows.Forms.RadioButton();
			this.rbEmbedded = new System.Windows.Forms.RadioButton();
			this.rbExternal = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.cbSizing = new System.Windows.Forms.ComboBox();
			this.bDatabaseExpr = new System.Windows.Forms.Button();
			this.bMimeExpr = new System.Windows.Forms.Button();
			this.bEmbeddedExpr = new System.Windows.Forms.Button();
			this.bExternalExpr = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.bExternalExpr);
			this.groupBox1.Controls.Add(this.bEmbeddedExpr);
			this.groupBox1.Controls.Add(this.bMimeExpr);
			this.groupBox1.Controls.Add(this.bDatabaseExpr);
			this.groupBox1.Controls.Add(this.bEmbedded);
			this.groupBox1.Controls.Add(this.bExternal);
			this.groupBox1.Controls.Add(this.tbValueExternal);
			this.groupBox1.Controls.Add(this.cbValueDatabase);
			this.groupBox1.Controls.Add(this.cbMIMEType);
			this.groupBox1.Controls.Add(this.cbValueEmbedded);
			this.groupBox1.Controls.Add(this.rbDatabase);
			this.groupBox1.Controls.Add(this.rbEmbedded);
			this.groupBox1.Controls.Add(this.rbExternal);
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(408, 152);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Source";
			// 
			// bEmbedded
			// 
			this.bEmbedded.Location = new System.Drawing.Point(378, 58);
			this.bEmbedded.Name = "bEmbedded";
			this.bEmbedded.Size = new System.Drawing.Size(22, 16);
			this.bEmbedded.TabIndex = 8;
			this.bEmbedded.Text = "...";
			this.bEmbedded.Click += new System.EventHandler(this.bEmbedded_Click);
			// 
			// bExternal
			// 
			this.bExternal.Location = new System.Drawing.Point(378, 26);
			this.bExternal.Name = "bExternal";
			this.bExternal.Size = new System.Drawing.Size(22, 16);
			this.bExternal.TabIndex = 7;
			this.bExternal.Text = "...";
			this.bExternal.Click += new System.EventHandler(this.bExternal_Click);
			// 
			// tbValueExternal
			// 
			this.tbValueExternal.Location = new System.Drawing.Point(88, 24);
			this.tbValueExternal.Name = "tbValueExternal";
			this.tbValueExternal.Size = new System.Drawing.Size(256, 20);
			this.tbValueExternal.TabIndex = 6;
			this.tbValueExternal.Text = "";
			this.tbValueExternal.TextChanged += new System.EventHandler(this.Value_TextChanged);
			// 
			// cbValueDatabase
			// 
			this.cbValueDatabase.Location = new System.Drawing.Point(88, 120);
			this.cbValueDatabase.Name = "cbValueDatabase";
			this.cbValueDatabase.Size = new System.Drawing.Size(256, 21);
			this.cbValueDatabase.TabIndex = 5;
			this.cbValueDatabase.TextChanged += new System.EventHandler(this.Value_TextChanged);
			// 
			// cbMIMEType
			// 
			this.cbMIMEType.Items.AddRange(new object[] {
															"image/bmp",
															"image/jpeg",
															"image/gif",
															"image/png",
															"image/x-png"});
			this.cbMIMEType.Location = new System.Drawing.Point(88, 88);
			this.cbMIMEType.Name = "cbMIMEType";
			this.cbMIMEType.Size = new System.Drawing.Size(88, 21);
			this.cbMIMEType.TabIndex = 4;
			this.cbMIMEType.Text = "image/jpeg";
			this.cbMIMEType.SelectedIndexChanged += new System.EventHandler(this.cbMIMEType_SelectedIndexChanged);
			// 
			// cbValueEmbedded
			// 
			this.cbValueEmbedded.Location = new System.Drawing.Point(88, 56);
			this.cbValueEmbedded.Name = "cbValueEmbedded";
			this.cbValueEmbedded.Size = new System.Drawing.Size(256, 21);
			this.cbValueEmbedded.TabIndex = 3;
			this.cbValueEmbedded.TextChanged += new System.EventHandler(this.Value_TextChanged);
			// 
			// rbDatabase
			// 
			this.rbDatabase.Location = new System.Drawing.Point(8, 88);
			this.rbDatabase.Name = "rbDatabase";
			this.rbDatabase.Size = new System.Drawing.Size(80, 24);
			this.rbDatabase.TabIndex = 2;
			this.rbDatabase.Text = "Database";
			this.rbDatabase.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
			// 
			// rbEmbedded
			// 
			this.rbEmbedded.Location = new System.Drawing.Point(8, 56);
			this.rbEmbedded.Name = "rbEmbedded";
			this.rbEmbedded.Size = new System.Drawing.Size(80, 24);
			this.rbEmbedded.TabIndex = 1;
			this.rbEmbedded.Text = "Embedded";
			this.rbEmbedded.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
			// 
			// rbExternal
			// 
			this.rbExternal.Location = new System.Drawing.Point(8, 24);
			this.rbExternal.Name = "rbExternal";
			this.rbExternal.Size = new System.Drawing.Size(80, 24);
			this.rbExternal.TabIndex = 0;
			this.rbExternal.Text = "External";
			this.rbExternal.CheckedChanged += new System.EventHandler(this.rbSource_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 184);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Sizing";
			// 
			// cbSizing
			// 
			this.cbSizing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSizing.Items.AddRange(new object[] {
														  "AutoSize",
														  "Fit",
														  "FitProportional",
														  "Clip"});
			this.cbSizing.Location = new System.Drawing.Point(72, 184);
			this.cbSizing.Name = "cbSizing";
			this.cbSizing.Size = new System.Drawing.Size(96, 21);
			this.cbSizing.TabIndex = 2;
			this.cbSizing.SelectedIndexChanged += new System.EventHandler(this.cbSizing_SelectedIndexChanged);
			// 
			// bDatabaseExpr
			// 
			this.bDatabaseExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bDatabaseExpr.Location = new System.Drawing.Point(352, 122);
			this.bDatabaseExpr.Name = "bDatabaseExpr";
			this.bDatabaseExpr.Size = new System.Drawing.Size(22, 16);
			this.bDatabaseExpr.TabIndex = 9;
			this.bDatabaseExpr.Tag = "database";
			this.bDatabaseExpr.Text = "fx";
			this.bDatabaseExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bDatabaseExpr.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// bMimeExpr
			// 
			this.bMimeExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bMimeExpr.Location = new System.Drawing.Point(184, 90);
			this.bMimeExpr.Name = "bMimeExpr";
			this.bMimeExpr.Size = new System.Drawing.Size(22, 16);
			this.bMimeExpr.TabIndex = 10;
			this.bMimeExpr.Tag = "mime";
			this.bMimeExpr.Text = "fx";
			this.bMimeExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bMimeExpr.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// bEmbeddedExpr
			// 
			this.bEmbeddedExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bEmbeddedExpr.Location = new System.Drawing.Point(352, 58);
			this.bEmbeddedExpr.Name = "bEmbeddedExpr";
			this.bEmbeddedExpr.Size = new System.Drawing.Size(22, 16);
			this.bEmbeddedExpr.TabIndex = 11;
			this.bEmbeddedExpr.Tag = "embedded";
			this.bEmbeddedExpr.Text = "fx";
			this.bEmbeddedExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bEmbeddedExpr.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// bExternalExpr
			// 
			this.bExternalExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bExternalExpr.Location = new System.Drawing.Point(352, 26);
			this.bExternalExpr.Name = "bExternalExpr";
			this.bExternalExpr.Size = new System.Drawing.Size(22, 16);
			this.bExternalExpr.TabIndex = 12;
			this.bExternalExpr.Tag = "external";
			this.bExternalExpr.Text = "fx";
			this.bExternalExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bExternalExpr.Click += new System.EventHandler(this.bExpr_Click);
			// 
			// ImageCtl
			// 
			this.Controls.Add(this.cbSizing);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Name = "ImageCtl";
			this.Size = new System.Drawing.Size(472, 288);
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

			// No more changes
			fSource = fValue = fSizing = fMIMEType = false;
		}

		public void ApplyChanges(XmlNode node)
		{
			if (fSource || fValue || fMIMEType)
			{
				string source="";
				string val="";
				if (rbEmbedded.Checked)
				{
					val = cbValueEmbedded.Text;
					source = "Embedded";
				}
				else if (rbDatabase.Checked)
				{
					source = "Database";
					val = cbValueDatabase.Text;
					_Draw.SetElement(node, "MIMEType", this.cbMIMEType.Text);
				}
				else 
				{	// must be external
					source = "External";
					val = tbValueExternal.Text;
				}
				_Draw.SetElement(node, "Source", source);
				_Draw.SetElement(node, "Value", val);
			}
			if (fSizing)
				_Draw.SetElement(node, "Sizing", this.cbSizing.Text);
		}

		private void Value_TextChanged(object sender, System.EventArgs e)
		{
			fValue = true;
		}

		private void cbMIMEType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fMIMEType = true;
		}

		private void rbSource_CheckedChanged(object sender, System.EventArgs e)
		{
			fSource = true;
			this.cbValueDatabase.Enabled = this.cbMIMEType.Enabled = 
				this.bDatabaseExpr.Enabled = this.rbDatabase.Checked;
			this.cbValueEmbedded.Enabled = this.bEmbeddedExpr.Enabled = 
				this.bEmbedded.Enabled = this.rbEmbedded.Checked;
			this.tbValueExternal.Enabled = this.bExternalExpr.Enabled = 
				this.bExternal.Enabled = this.rbExternal.Checked;
		}

		private void cbSizing_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fSizing = true;
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
            }
            finally
            {
                dlgEI.Dispose();
            }
			// Populate the EmbeddedImage names
			cbValueEmbedded.Items.Clear();
			cbValueEmbedded.Items.AddRange(_Draw.ReportNames.EmbeddedImageNames);

		}
		private void bExpr_Click(object sender, System.EventArgs e)
		{
			Button b = sender as Button;
			if (b == null)
				return;
			Control c = null;
			switch (b.Tag as string)
			{
				case "external":
					c = tbValueExternal;
					break;
				case "embedded":
					c = cbValueEmbedded;
					break;
				case "mime":
					c = cbMIMEType;
					break;
				case "database":
					c = cbValueDatabase;
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
