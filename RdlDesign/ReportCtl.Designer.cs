using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Majorsilence.Reporting.RdlDesign
{
	internal partial class ReportCtl
	{
		#region Windows Form Designer generated code
		private DesignXmlDraw _Draw;
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
private ComboBox cbPageSize;
private Label label11;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportCtl));
            this.DoubleBuffered = true;
			this.tbReportAuthor = new System.Windows.Forms.TextBox();
			this.tbReportDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbPageSize = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
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
			resources.ApplyResources(this.tbReportAuthor, "tbReportAuthor");
			this.tbReportAuthor.Name = "tbReportAuthor";
			// 
			// tbReportDescription
			// 
			resources.ApplyResources(this.tbReportDescription, "tbReportDescription");
			this.tbReportDescription.Name = "tbReportDescription";
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
			this.groupBox1.Controls.Add(this.cbPageSize);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.tbPageHeight);
			this.groupBox1.Controls.Add(this.tbPageWidth);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// cbPageSize
			// 
			resources.ApplyResources(this.cbPageSize, "cbPageSize");
			this.cbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPageSize.FormattingEnabled = true;
			this.cbPageSize.Name = "cbPageSize";
			this.cbPageSize.SelectedIndexChanged += new System.EventHandler(this.cbPageSize_SelectedIndexChanged);
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// tbPageHeight
			// 
			resources.ApplyResources(this.tbPageHeight, "tbPageHeight");
			this.tbPageHeight.Name = "tbPageHeight";
			this.tbPageHeight.Tag = "Page Height";
			this.tbPageHeight.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// tbPageWidth
			// 
			resources.ApplyResources(this.tbPageWidth, "tbPageWidth");
			this.tbPageWidth.Name = "tbPageWidth";
			this.tbPageWidth.Tag = "Page Width";
			this.tbPageWidth.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.tbMarginBottom);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.tbMarginTop);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.tbMarginRight);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.tbMarginLeft);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// tbMarginBottom
			// 
			resources.ApplyResources(this.tbMarginBottom, "tbMarginBottom");
			this.tbMarginBottom.Name = "tbMarginBottom";
			this.tbMarginBottom.Tag = "Bottom Margin";
			this.tbMarginBottom.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// tbMarginTop
			// 
			resources.ApplyResources(this.tbMarginTop, "tbMarginTop");
			this.tbMarginTop.Name = "tbMarginTop";
			this.tbMarginTop.Tag = "Top Margin";
			this.tbMarginTop.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// tbMarginRight
			// 
			resources.ApplyResources(this.tbMarginRight, "tbMarginRight");
			this.tbMarginRight.Name = "tbMarginRight";
			this.tbMarginRight.Tag = "Right Margin";
			this.tbMarginRight.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// tbMarginLeft
			// 
			resources.ApplyResources(this.tbMarginLeft, "tbMarginLeft");
			this.tbMarginLeft.Name = "tbMarginLeft";
			this.tbMarginLeft.Tag = "Left Margin";
			this.tbMarginLeft.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// tbWidth
			// 
			resources.ApplyResources(this.tbWidth, "tbWidth");
			this.tbWidth.Name = "tbWidth";
			this.tbWidth.Tag = "Width";
			this.tbWidth.Validating += new System.ComponentModel.CancelEventHandler(this.tbSize_Validating);
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// groupBox3
			// 
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.chkPHLast);
			this.groupBox3.Controls.Add(this.chkPHFirst);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// chkPHLast
			// 
			resources.ApplyResources(this.chkPHLast, "chkPHLast");
			this.chkPHLast.Name = "chkPHLast";
			// 
			// chkPHFirst
			// 
			resources.ApplyResources(this.chkPHFirst, "chkPHFirst");
			this.chkPHFirst.Name = "chkPHFirst";
			// 
			// groupBox4
			// 
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.Controls.Add(this.chkPFLast);
			this.groupBox4.Controls.Add(this.chkPFFirst);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			// 
			// chkPFLast
			// 
			resources.ApplyResources(this.chkPFLast, "chkPFLast");
			this.chkPFLast.Name = "chkPFLast";
			// 
			// chkPFFirst
			// 
			resources.ApplyResources(this.chkPFFirst, "chkPFFirst");
			this.chkPFFirst.Name = "chkPFFirst";
			// 
			// ReportCtl
			// 
			resources.ApplyResources(this, "$this");
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
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

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
	}
}
