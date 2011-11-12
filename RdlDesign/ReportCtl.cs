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
	/// Summary description for ReportCtl.
	/// </summary>
	internal class ReportCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		// flags for controlling whether syntax changed for a particular property
		private System.Windows.Forms.TextBox tbReportAuthor;
		private System.Windows.Forms.TextBox tbReportDescription;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbPageWidth;
		private System.Windows.Forms.TextBox tbPageHeight;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox tbMarginLeft;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbMarginRight;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbMarginBottom;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tbMarginTop;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox tbWidth;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox chkPFFirst;
		private System.Windows.Forms.CheckBox chkPHFirst;
		private System.Windows.Forms.CheckBox chkPHLast;
		private System.Windows.Forms.CheckBox chkPFLast;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal ReportCtl(DesignXmlDraw dxDraw)
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
			tbWidth.Text = _Draw.GetElementValue(rNode, "Width", "");
			tbReportAuthor.Text = _Draw.GetElementValue(rNode, "Author", "");
			tbReportDescription.Text = _Draw.GetElementValue(rNode, "Description", "");
			tbPageWidth.Text = _Draw.GetElementValue(rNode, "PageWidth", "8.5in");
			tbPageHeight.Text = _Draw.GetElementValue(rNode, "PageHeight", "11in");
			tbMarginLeft.Text = _Draw.GetElementValue(rNode, "LeftMargin", "");
			tbMarginRight.Text = _Draw.GetElementValue(rNode, "RightMargin", "");
			tbMarginBottom.Text = _Draw.GetElementValue(rNode, "BottomMargin", "");
			tbMarginTop.Text = _Draw.GetElementValue(rNode, "TopMargin", "");
			// Page header settings
			XmlNode phNode = _Draw.GetCreateNamedChildNode(rNode, "PageHeader");
			this.chkPHFirst.Checked = _Draw.GetElementValue(phNode, "PrintOnFirstPage", "true").ToLower() == "true"? true: false;
			this.chkPHLast.Checked = _Draw.GetElementValue(phNode, "PrintOnLastPage", "true").ToLower() == "true"? true: false;
			// Page footer settings
			XmlNode pfNode = _Draw.GetCreateNamedChildNode(rNode, "PageFooter");
			this.chkPFFirst.Checked = _Draw.GetElementValue(pfNode, "PrintOnFirstPage", "true").ToLower() == "true"? true: false;
			this.chkPFLast.Checked = _Draw.GetElementValue(pfNode, "PrintOnLastPage", "true").ToLower() == "true"? true: false;
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
			this.tbReportAuthor = new System.Windows.Forms.TextBox();
			this.tbReportDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbPageHeight = new System.Windows.Forms.TextBox();
			this.tbPageWidth = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tbMarginBottom = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tbMarginTop = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbMarginRight = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbMarginLeft = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbWidth = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.chkPHLast = new System.Windows.Forms.CheckBox();
			this.chkPHFirst = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.chkPFLast = new System.Windows.Forms.CheckBox();
			this.chkPFFirst = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbReportAuthor
			// 
			this.tbReportAuthor.Location = new System.Drawing.Point(96, 9);
			this.tbReportAuthor.Name = "tbReportAuthor";
			this.tbReportAuthor.Size = new System.Drawing.Size(304, 20);
			this.tbReportAuthor.TabIndex = 0;
			this.tbReportAuthor.Text = "";
			// 
			// tbReportDescription
			// 
			this.tbReportDescription.Location = new System.Drawing.Point(96, 32);
			this.tbReportDescription.Name = "tbReportDescription";
			this.tbReportDescription.Size = new System.Drawing.Size(304, 20);
			this.tbReportDescription.TabIndex = 1;
			this.tbReportDescription.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Author:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "Description:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tbPageHeight);
			this.groupBox1.Controls.Add(this.tbPageWidth);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(16, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(384, 48);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Page";
			// 
			// tbPageHeight
			// 
			this.tbPageHeight.Location = new System.Drawing.Point(224, 16);
			this.tbPageHeight.Name = "tbPageHeight";
			this.tbPageHeight.Size = new System.Drawing.Size(72, 20);
			this.tbPageHeight.TabIndex = 1;
			this.tbPageHeight.Tag = "Page Height";
			this.tbPageHeight.Text = "";
			this.tbPageHeight.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// tbPageWidth
			// 
			this.tbPageWidth.Location = new System.Drawing.Point(72, 16);
			this.tbPageWidth.Name = "tbPageWidth";
			this.tbPageWidth.Size = new System.Drawing.Size(72, 20);
			this.tbPageWidth.TabIndex = 0;
			this.tbPageWidth.Tag = "Page Width";
			this.tbPageWidth.Text = "";
			this.tbPageWidth.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(164, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 16);
			this.label4.TabIndex = 2;
			this.label4.Text = "Height";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Width";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.tbMarginBottom);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.tbMarginTop);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.tbMarginRight);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.tbMarginLeft);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Location = new System.Drawing.Point(16, 136);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(384, 72);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Margins";
			// 
			// tbMarginBottom
			// 
			this.tbMarginBottom.Location = new System.Drawing.Point(224, 40);
			this.tbMarginBottom.Name = "tbMarginBottom";
			this.tbMarginBottom.Size = new System.Drawing.Size(72, 20);
			this.tbMarginBottom.TabIndex = 3;
			this.tbMarginBottom.Tag = "Bottom Margin";
			this.tbMarginBottom.Text = "";
			this.tbMarginBottom.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(168, 40);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "Bottom";
			// 
			// tbMarginTop
			// 
			this.tbMarginTop.Location = new System.Drawing.Point(72, 40);
			this.tbMarginTop.Name = "tbMarginTop";
			this.tbMarginTop.Size = new System.Drawing.Size(72, 20);
			this.tbMarginTop.TabIndex = 2;
			this.tbMarginTop.Tag = "Top Margin";
			this.tbMarginTop.Text = "";
			this.tbMarginTop.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 40);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 16);
			this.label8.TabIndex = 4;
			this.label8.Text = "Top";
			// 
			// tbMarginRight
			// 
			this.tbMarginRight.Location = new System.Drawing.Point(224, 16);
			this.tbMarginRight.Name = "tbMarginRight";
			this.tbMarginRight.Size = new System.Drawing.Size(72, 20);
			this.tbMarginRight.TabIndex = 1;
			this.tbMarginRight.Tag = "Right Margin";
			this.tbMarginRight.Text = "";
			this.tbMarginRight.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(168, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "Right";
			// 
			// tbMarginLeft
			// 
			this.tbMarginLeft.Location = new System.Drawing.Point(72, 16);
			this.tbMarginLeft.Name = "tbMarginLeft";
			this.tbMarginLeft.Size = new System.Drawing.Size(72, 20);
			this.tbMarginLeft.TabIndex = 0;
			this.tbMarginLeft.Tag = "Left Margin";
			this.tbMarginLeft.Text = "";
			this.tbMarginLeft.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Left";
			// 
			// tbWidth
			// 
			this.tbWidth.Location = new System.Drawing.Point(96, 56);
			this.tbWidth.Name = "tbWidth";
			this.tbWidth.Size = new System.Drawing.Size(72, 20);
			this.tbWidth.TabIndex = 2;
			this.tbWidth.Tag = "Width";
			this.tbWidth.Text = "";
			this.tbWidth.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 23);
			this.label9.TabIndex = 15;
			this.label9.Text = "Width";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.chkPHLast);
			this.groupBox3.Controls.Add(this.chkPHFirst);
			this.groupBox3.Location = new System.Drawing.Point(16, 216);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(184, 56);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Page Header";
			// 
			// chkPHLast
			// 
			this.chkPHLast.Location = new System.Drawing.Point(96, 16);
			this.chkPHLast.Name = "chkPHLast";
			this.chkPHLast.Size = new System.Drawing.Size(76, 28);
			this.chkPHLast.TabIndex = 2;
			this.chkPHLast.Text = "Print on last page";
			// 
			// chkPHFirst
			// 
			this.chkPHFirst.Location = new System.Drawing.Point(8, 16);
			this.chkPHFirst.Name = "chkPHFirst";
			this.chkPHFirst.Size = new System.Drawing.Size(76, 28);
			this.chkPHFirst.TabIndex = 1;
			this.chkPHFirst.Text = "Print on first page";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.chkPFLast);
			this.groupBox4.Controls.Add(this.chkPFFirst);
			this.groupBox4.Location = new System.Drawing.Point(216, 216);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(184, 56);
			this.groupBox4.TabIndex = 6;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Page Footer";
			// 
			// chkPFLast
			// 
			this.chkPFLast.Location = new System.Drawing.Point(96, 16);
			this.chkPFLast.Name = "chkPFLast";
			this.chkPFLast.Size = new System.Drawing.Size(76, 28);
			this.chkPFLast.TabIndex = 1;
			this.chkPFLast.Text = "Print on last page";
			// 
			// chkPFFirst
			// 
			this.chkPFFirst.Location = new System.Drawing.Point(8, 16);
			this.chkPFFirst.Name = "chkPFFirst";
			this.chkPFFirst.Size = new System.Drawing.Size(76, 28);
			this.chkPFFirst.TabIndex = 0;
			this.chkPFFirst.Text = "Print on first page";
			// 
			// ReportCtl
			// 
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.tbWidth);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.tbReportAuthor);
			this.Controls.Add(this.tbReportDescription);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Name = "ReportCtl";
			this.Size = new System.Drawing.Size(472, 288);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
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
			_Draw.SetElement(rNode, "Width", DesignerUtility.MakeValidSize(tbWidth.Text, false));
			_Draw.SetElement(rNode, "Author", tbReportAuthor.Text);
			_Draw.SetElement(rNode, "Description", tbReportDescription.Text);
			_Draw.SetElement(rNode, "PageWidth", tbPageWidth.Text);
			_Draw.SetElement(rNode, "PageHeight", tbPageHeight.Text);
			if (tbMarginLeft.Text.Trim().Length > 0)
				_Draw.SetElement(rNode, "LeftMargin", tbMarginLeft.Text);
			else
				_Draw.RemoveElement(rNode, "LeftMargin");
			if (tbMarginRight.Text.Trim().Length > 0)
				_Draw.SetElement(rNode, "RightMargin", tbMarginRight.Text);
			else
				_Draw.RemoveElement(rNode, "RightMargin");
			if (tbMarginBottom.Text.Trim().Length > 0)
				_Draw.SetElement(rNode, "BottomMargin", tbMarginBottom.Text);
			else
				_Draw.RemoveElement(rNode, "BottomMargin");
			if (tbMarginTop.Text.Trim().Length > 0)
				_Draw.SetElement(rNode, "TopMargin", tbMarginTop.Text);
			else
				_Draw.RemoveElement(rNode, "TopMargin");
			// Page header settings
			XmlNode phNode = _Draw.GetCreateNamedChildNode(rNode, "PageHeader");
			_Draw.SetElement(phNode, "PrintOnFirstPage", this.chkPHFirst.Checked?"true":"false");
			_Draw.SetElement(phNode, "PrintOnLastPage", this.chkPHLast.Checked?"true":"false");
			// Page footer settings
			XmlNode pfNode = _Draw.GetCreateNamedChildNode(rNode, "PageFooter");
			_Draw.SetElement(pfNode, "PrintOnFirstPage", this.chkPFFirst.Checked?"true":"false");
			_Draw.SetElement(pfNode, "PrintOnLastPage", this.chkPFLast.Checked?"true":"false");

		}

		private void tbSize_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			TextBox tb = sender as TextBox;
			if (tb == null)
				return;

			try {DesignerUtility.ValidateSize(tb.Text, false, false);}
			catch (Exception ex)
			{
				e.Cancel = true;
				MessageBox.Show(string.Format("Size value of {0} is invalid.\r\n", tb.Text, ex.Message), tb.Tag + " Field Invalid");
			}
		}
	}
}
