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
using System.Text;
using System.IO;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Filters specification: used for DataRegions (List, Chart, Table, Matrix), DataSets, group instances
	/// </summary>
	internal class SubreportCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private XmlNode _Subreport;
		private DataTable _DataTable;
		private DataGridTextBoxColumn dgtbName;
		private DataGridTextBoxColumn dgtbValue;
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bFile;
		private System.Windows.Forms.TextBox tbReportFile;
		private System.Windows.Forms.TextBox tbNoRows;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkMergeTrans;
		private System.Windows.Forms.DataGrid dgParms;
		private System.Windows.Forms.Button bRefreshParms;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal SubreportCtl(DesignXmlDraw dxDraw, XmlNode subReport)
		{
			_Draw = dxDraw;
			_Subreport =subReport;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			this.tbReportFile.Text = _Draw.GetElementValue(_Subreport, "ReportName", "");
			this.tbNoRows.Text = _Draw.GetElementValue(_Subreport, "NoRows", "");
			this.chkMergeTrans.Checked = _Draw.GetElementValue(_Subreport, "MergeTransactions", "false").ToLower() == "true";

			// Initialize the DataGrid columns
			dgtbName = new DataGridTextBoxColumn();
			dgtbValue = new DataGridTextBoxColumn();

			this.dgTableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] {
															this.dgtbName,
															this.dgtbValue});
			// 
			// dgtbFE
			// 
			dgtbName.HeaderText = "Parameter Name";
			dgtbName.MappingName = "ParameterName";
			dgtbName.Width = 75;
			// Get the parent's dataset name
//			string dataSetName = _Draw.GetDataSetNameValue(_FilterParent);
//
//			string[] fields = _Draw.GetFields(dataSetName, true);
//			if (fields != null)
//				dgtbFE.CB.Items.AddRange(fields);
			// 
			// dgtbValue
			// 
			this.dgtbValue.HeaderText = "Value";
			this.dgtbValue.MappingName = "Value";
			this.dgtbValue.Width = 75;
//			string[] parms = _Draw.GetReportParameters(true);
//			if (parms != null)
//				dgtbFV.CB.Items.AddRange(parms);

			// Initialize the DataTable
			_DataTable = new DataTable();
			_DataTable.Columns.Add(new DataColumn("ParameterName", typeof(string)));
			_DataTable.Columns.Add(new DataColumn("Value", typeof(string)));

			string[] rowValues = new string[2];
			XmlNode parameters = _Draw.GetNamedChildNode(_Subreport, "Parameters");

			if (parameters != null)
			foreach (XmlNode pNode in parameters.ChildNodes)
			{
				if (pNode.NodeType != XmlNodeType.Element || 
						pNode.Name != "Parameter")
					continue;
				rowValues[0] = _Draw.GetElementAttribute(pNode, "Name", "");
				rowValues[1] = _Draw.GetElementValue(pNode, "Value", "");

				_DataTable.Rows.Add(rowValues);
			}
			// Don't allow users to add their own rows
//			DataView dv = new DataView(_DataTable);		// bad side effect
//			dv.AllowNew = false;
			this.dgParms.DataSource = _DataTable;

			DataGridTableStyle ts = dgParms.TableStyles[0];
			ts.GridColumnStyles[0].Width = 140;
			ts.GridColumnStyles[0].ReadOnly = true;
			ts.GridColumnStyles[1].Width = 140;
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
			this.dgParms = new System.Windows.Forms.DataGrid();
			this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.label1 = new System.Windows.Forms.Label();
			this.tbReportFile = new System.Windows.Forms.TextBox();
			this.bFile = new System.Windows.Forms.Button();
			this.tbNoRows = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkMergeTrans = new System.Windows.Forms.CheckBox();
			this.bRefreshParms = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParms
			// 
			this.dgParms.CaptionVisible = false;
			this.dgParms.DataMember = "";
			this.dgParms.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParms.Location = new System.Drawing.Point(8, 112);
			this.dgParms.Name = "dgParms";
			this.dgParms.Size = new System.Drawing.Size(384, 168);
			this.dgParms.TabIndex = 2;
			this.dgParms.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								this.dgTableStyle});
			// 
			// dgTableStyle
			// 
			this.dgTableStyle.AllowSorting = false;
			this.dgTableStyle.DataGrid = this.dgParms;
			this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgTableStyle.MappingName = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Subreport name";
			// 
			// tbReportFile
			// 
			this.tbReportFile.Location = new System.Drawing.Point(104, 8);
			this.tbReportFile.Name = "tbReportFile";
			this.tbReportFile.Size = new System.Drawing.Size(312, 20);
			this.tbReportFile.TabIndex = 4;
			this.tbReportFile.Text = "";
			// 
			// bFile
			// 
			this.bFile.Location = new System.Drawing.Point(424, 8);
			this.bFile.Name = "bFile";
			this.bFile.Size = new System.Drawing.Size(24, 23);
			this.bFile.TabIndex = 5;
			this.bFile.Text = "...";
			this.bFile.Click += new System.EventHandler(this.bFile_Click);
			// 
			// tbNoRows
			// 
			this.tbNoRows.Location = new System.Drawing.Point(104, 40);
			this.tbNoRows.Name = "tbNoRows";
			this.tbNoRows.Size = new System.Drawing.Size(312, 20);
			this.tbNoRows.TabIndex = 7;
			this.tbNoRows.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 8;
			this.label2.Text = "No rows message";
			// 
			// chkMergeTrans
			// 
			this.chkMergeTrans.Location = new System.Drawing.Point(8, 72);
			this.chkMergeTrans.Name = "chkMergeTrans";
			this.chkMergeTrans.Size = new System.Drawing.Size(376, 24);
			this.chkMergeTrans.TabIndex = 9;
			this.chkMergeTrans.Text = "Use same Data Source connections as parent report when possible";
			// 
			// bRefreshParms
			// 
			this.bRefreshParms.Location = new System.Drawing.Point(392, 120);
			this.bRefreshParms.Name = "bRefreshParms";
			this.bRefreshParms.Size = new System.Drawing.Size(56, 23);
			this.bRefreshParms.TabIndex = 10;
			this.bRefreshParms.Text = "Refresh";
			this.bRefreshParms.Click += new System.EventHandler(this.bRefreshParms_Click);
			// 
			// SubreportCtl
			// 
			this.Controls.Add(this.bRefreshParms);
			this.Controls.Add(this.chkMergeTrans);
			this.Controls.Add(this.tbNoRows);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.bFile);
			this.Controls.Add(this.tbReportFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dgParms);
			this.Name = "SubreportCtl";
			this.Size = new System.Drawing.Size(464, 304);
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
       
		public bool IsValid()
		{
			if (this.tbReportFile.Text.Length > 0)
				return true;
			MessageBox.Show("Subreport file must be specified.", "Subreport");
			return false;
		}

		public void Apply()
		{
			_Draw.SetElement(_Subreport, "ReportName", this.tbReportFile.Text);
			if (this.tbNoRows.Text.Trim().Length == 0)
				_Draw.RemoveElement(_Subreport, "NoRows");
			else
				_Draw.SetElement(_Subreport, "NoRows", tbNoRows.Text);

			_Draw.SetElement(_Subreport, "MergeTransactions", this.chkMergeTrans.Checked? "true": "false");

			// Remove the old filters
			XmlNode parms = _Draw.GetCreateNamedChildNode(_Subreport, "Parameters");
			while (parms.FirstChild != null)
			{
				parms.RemoveChild(parms.FirstChild);
			}
			// Loop thru and add all the filters
			foreach (DataRow dr in _DataTable.Rows)
			{
				if (dr[0] == DBNull.Value || dr[1] == DBNull.Value)
					continue;
				string name = (string) dr[0];
				string val = (string) dr[1];
				if (name.Length <= 0 || val.Length <= 0)
					continue;
				XmlNode pNode = _Draw.CreateElement(parms, "Parameter", null);
				_Draw.SetElementAttribute(pNode, "Name", name);
				_Draw.SetElement(pNode, "Value", val);
			}
			if (!parms.HasChildNodes)
				_Subreport.RemoveChild(parms);
		}

		private void bFile_Click(object sender, System.EventArgs e)
		{
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Report files (*.rdl)|*.rdl" +
                    "|All files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.FileName = "*.rdl";

                ofd.Title = "Specify Report File Name";
                ofd.DefaultExt = "rdl";
                ofd.AddExtension = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileNameWithoutExtension(ofd.FileName);

                    tbReportFile.Text = file;
                }
            }
		}

		private void bRefreshParms_Click(object sender, System.EventArgs e)
		{
			// Obtain the source
			string filename="";
			if (tbReportFile.Text.Length > 0)
				filename = tbReportFile.Text + ".rdl";

			string source = this.GetSource(filename);
			if (source == null)
				return;						// error: message already displayed

			// Compile the report
			Report report = this.GetReport(source, filename);
			if (report == null)
				return;					// error: message already displayed
			
			ICollection rps = report.UserReportParameters;
			string[] rowValues = new string[2];
			_DataTable.Rows.Clear();
			foreach (UserReportParameter rp in rps)
			{
				rowValues[0] = rp.Name;
				rowValues[1] = "";

				_DataTable.Rows.Add(rowValues);
			}
			this.dgParms.Refresh();
		}

		private string GetSource(string file)
		{
			StreamReader fs=null;
			string prog=null;
			try
			{
				fs = new StreamReader(file);
				prog = fs.ReadToEnd();
			}
			catch(Exception e)
			{
				prog = null;
				MessageBox.Show(e.Message, "Error reading report file");
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			return prog;
		}

		private Report GetReport(string prog, string file)
		{
			// Now parse the file
			RDLParser rdlp;
			Report r;
			try
			{
				rdlp =  new RDLParser(prog);
				string folder = Path.GetDirectoryName(file);
				if (folder == "")
					folder = Environment.CurrentDirectory;
				rdlp.Folder = folder;

				r = rdlp.Parse();
				if (r.ErrorMaxSeverity > 4) 
				{
					MessageBox.Show(string.Format("Report {0} has errors and cannot be processed.", "Report"));
					r = null;			// don't return when severe errors
				}
			}
			catch(Exception e)
			{
				r = null;
				MessageBox.Show(e.Message, "Report load failed");
			}
			return r;
		}

	}
}
