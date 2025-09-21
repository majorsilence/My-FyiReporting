
using System;
using System.Collections;
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
	internal class ReportXmlCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbDataTransform;
		private System.Windows.Forms.TextBox tbDataSchema;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbDataElementName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbElementStyle;
		private System.Windows.Forms.Button bOpenXsl;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal ReportXmlCtl(DesignXmlDraw dxDraw)
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
			tbDataTransform.Text = _Draw.GetElementValue(rNode, "DataTransform", "");
			tbDataSchema.Text = _Draw.GetElementValue(rNode, "DataSchema", "");
			tbDataElementName.Text = _Draw.GetElementValue(rNode, "DataElementName", "Report");
			cbElementStyle.Text = _Draw.GetElementValue(rNode, "DataElementStyle", "AttributeNormal");
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportXmlCtl));
			this.label1 = new System.Windows.Forms.Label();
			this.tbDataTransform = new System.Windows.Forms.TextBox();
			this.tbDataSchema = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbDataElementName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbElementStyle = new System.Windows.Forms.ComboBox();
			this.bOpenXsl = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tbDataTransform
			// 
			resources.ApplyResources(this.tbDataTransform, "tbDataTransform");
			this.tbDataTransform.Name = "tbDataTransform";
			// 
			// tbDataSchema
			// 
			resources.ApplyResources(this.tbDataSchema, "tbDataSchema");
			this.tbDataSchema.Name = "tbDataSchema";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// tbDataElementName
			// 
			resources.ApplyResources(this.tbDataElementName, "tbDataElementName");
			this.tbDataElementName.Name = "tbDataElementName";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// cbElementStyle
			// 
			resources.ApplyResources(this.cbElementStyle, "cbElementStyle");
			this.cbElementStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbElementStyle.Items.AddRange(new object[] {
            resources.GetString("cbElementStyle.Items"),
            resources.GetString("cbElementStyle.Items1")});
			this.cbElementStyle.Name = "cbElementStyle";
			// 
			// bOpenXsl
			// 
			resources.ApplyResources(this.bOpenXsl, "bOpenXsl");
			this.bOpenXsl.Name = "bOpenXsl";
			this.bOpenXsl.Click += new System.EventHandler(this.bOpenXsl_Click);
			// 
			// ReportXmlCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.bOpenXsl);
			this.Controls.Add(this.cbElementStyle);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbDataElementName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbDataSchema);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbDataTransform);
			this.Controls.Add(this.label1);
			this.Name = "ReportXmlCtl";
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
			XmlNode rNode = _Draw.GetReportNode();

			if (tbDataTransform.Text.Length > 0)
				_Draw.SetElement(rNode, "DataTransform", tbDataTransform.Text);
			else
				_Draw.RemoveElement(rNode, "DataTransform");
			
			if (tbDataSchema.Text.Length > 0)
				_Draw.SetElement(rNode, "DataSchema", tbDataSchema.Text);
			else
				_Draw.RemoveElement(rNode, "DataSchema");

			if (tbDataElementName.Text.Length > 0)
				_Draw.SetElement(rNode, "DataElementName", tbDataElementName.Text);
			else
				_Draw.RemoveElement(rNode, "DataElementName");

			_Draw.SetElement(rNode, "DataElementStyle", cbElementStyle.Text);
		}

		private void bOpenXsl_Click(object sender, System.EventArgs e)
		{
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = Strings.ReportXmlCtl_bOpenXsl_Click_XSLFilesFilter;
                ofd.FilterIndex = 1;
                ofd.FileName = "*.xsl";

                ofd.Title = Strings.ReportXmlCtl_bOpenXsl_Click_XslFilerTitle;
                //			ofd.DefaultExt = "xsl";
                //			ofd.AddExtension = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileName(ofd.FileName);

                    tbDataTransform.Text = file;
                }
            }
		}
	}
}
