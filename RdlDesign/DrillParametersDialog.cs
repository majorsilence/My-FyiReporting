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
using System.Text;
using System.IO;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Drillthrough reports; pick report and specify parameters
	/// </summary>
	internal class DrillParametersDialog : System.Windows.Forms.Form
	{
		private string _DrillReport;
		private DataTable _DataTable;
		private DataGridTextBoxColumn dgtbName;
		private DataGridTextBoxColumn dgtbValue;
		private DataGridTextBoxColumn dgtbOmit;
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bFile;
		private System.Windows.Forms.TextBox tbReportFile;
		private System.Windows.Forms.DataGrid dgParms;
		private System.Windows.Forms.Button bRefreshParms;
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.Button bCancel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal DrillParametersDialog(string report, List<DrillParameter> parameters)
		{
			_DrillReport = report;
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues(parameters);
		}

        private void InitValues(List<DrillParameter> parameters)
		{
			this.tbReportFile.Text = _DrillReport;

			// Initialize the DataGrid columns
			dgtbName = new DataGridTextBoxColumn();
			dgtbValue = new DataGridTextBoxColumn();
			dgtbOmit = new DataGridTextBoxColumn();

			this.dgTableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] {
															this.dgtbName,
															this.dgtbValue,
															this.dgtbOmit});
			// 
			// dgtbFE
			// 
			dgtbName.HeaderText = "Parameter Name";
			dgtbName.MappingName = "ParameterName";
			dgtbName.Width = 75;
			// 
			// dgtbValue
			// 
			this.dgtbValue.HeaderText = "Value";
			this.dgtbValue.MappingName = "Value";
			this.dgtbValue.Width = 75;
			// 
			// dgtbOmit
			// 
			this.dgtbOmit.HeaderText = "Omit";
			this.dgtbOmit.MappingName = "Omit";
			this.dgtbOmit.Width = 75;

			// Initialize the DataTable
			_DataTable = new DataTable();	  
			
			_DataTable.Columns.Add(new DataColumn("ParameterName", typeof(string)));
			_DataTable.Columns.Add(new DataColumn("Value", typeof(string)));
			_DataTable.Columns.Add(new DataColumn("Omit", typeof(string)));

			string[] rowValues = new string[3];

			if (parameters != null)
			foreach (DrillParameter dp in parameters)
			{
				rowValues[0] = dp.ParameterName;
				rowValues[1] = dp.ParameterValue;
				rowValues[2] = dp.ParameterOmit;

				_DataTable.Rows.Add(rowValues);
			}
			// Don't allow new rows; do this by creating a DataView over the DataTable
//			DataView dv = new DataView(_DataTable);	// this has bad side effects
//			dv.AllowNew = false;
			this.dgParms.DataSource = _DataTable;

			DataGridTableStyle ts = dgParms.TableStyles[0];
			
			ts.GridColumnStyles[0].Width = 140;
			ts.GridColumnStyles[0].ReadOnly = true;
			ts.GridColumnStyles[1].Width = 140;
			ts.GridColumnStyles[2].Width = 70;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DrillParametersDialog));
			this.dgParms = new System.Windows.Forms.DataGrid();
			this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.label1 = new System.Windows.Forms.Label();
			this.tbReportFile = new System.Windows.Forms.TextBox();
			this.bFile = new System.Windows.Forms.Button();
			this.bRefreshParms = new System.Windows.Forms.Button();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParms
			// 
			this.dgParms.CaptionVisible = false;
			this.dgParms.DataMember = "";
			this.dgParms.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParms.Location = new System.Drawing.Point(8, 40);
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
			this.label1.Text = "Report name";
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
			// bRefreshParms
			// 
			this.bRefreshParms.Location = new System.Drawing.Point(400, 40);
			this.bRefreshParms.Name = "bRefreshParms";
			this.bRefreshParms.Size = new System.Drawing.Size(56, 23);
			this.bRefreshParms.TabIndex = 10;
			this.bRefreshParms.Text = "Refresh";
			this.bRefreshParms.Click += new System.EventHandler(this.bRefreshParms_Click);
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(288, 216);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 11;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.CausesValidation = false;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(376, 216);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 12;
			this.bCancel.Text = "Cancel";
			// 
			// DrillParametersDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(464, 248);
			this.ControlBox = false;
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bRefreshParms);
			this.Controls.Add(this.bFile);
			this.Controls.Add(this.tbReportFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dgParms);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DrillParametersDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Specify Drillthrough Report and Parameters";
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public string DrillthroughReport
		{
			get {return this._DrillReport;}
		}

        public List<DrillParameter> DrillParameters
		{
			get 
			{
                List<DrillParameter> parms = new List<DrillParameter>();

				// Loop thru and add all the filters
				foreach (DataRow dr in _DataTable.Rows)
				{
					if (dr[0] == DBNull.Value || dr[1] == DBNull.Value)
						continue;
					string name = (string) dr[0];
					string val = (string) dr[1];
					string omit = dr[2] == DBNull.Value? "false": (string) dr[2];
					if (name.Length <= 0 || val.Length <= 0)
						continue;
					DrillParameter dp = new DrillParameter(name, val, omit);
					parms.Add(dp);
				}
				if (parms.Count == 0)
					return null;
				return parms;
			}
		}

		private void bFile_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Report files (*.rdl)|*.rdl" +
                "|All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.FileName = "*.rdl";

			ofd.Title = "Specify Report File Name";
			ofd.DefaultExt = "rdl";
			ofd.AddExtension = true;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileNameWithoutExtension(ofd.FileName);

                    tbReportFile.Text = file;
                }
            }
            finally
            {
                ofd.Dispose();
            }
		}

		private void bRefreshParms_Click(object sender, System.EventArgs e)
		{
			// Obtain the source
			Cursor savec = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;	// this can take some time
			try
			{
				string filename="";
				if (tbReportFile.Text.Length > 0)
					filename = tbReportFile.Text + ".rdl";

				filename = GetFileNameWithPath(filename);

				string source = this.GetSource(filename);
				if (source == null)
					return;						// error: message already displayed

				// Compile the report
				Report report = this.GetReport(source, filename);
				if (report == null)
					return;					// error: message already displayed
			
				ICollection rps = report.UserReportParameters;
				string[] rowValues = new string[3];
				_DataTable.Rows.Clear();
				foreach (UserReportParameter rp in rps)
				{
					rowValues[0] = rp.Name;
					rowValues[1] = "";
					rowValues[2] = "false";

					_DataTable.Rows.Add(rowValues);
				}
				this.dgParms.Refresh();
				this.dgParms.Focus();
			}
			finally
			{
				Cursor.Current = savec;
			}
		}

		private string GetFileNameWithPath(string file)
		{	// todo: should prefix this with the path of the open file
			
			return file;
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
				{
					folder = Environment.CurrentDirectory;
				}
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

		private void DrillParametersDialog_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (DataRow dr in _DataTable.Rows)
			{
				if (dr[1] == DBNull.Value)
				{
					e.Cancel = true;
					break;
				}
				string val = (string) dr[1];
				if (val.Length <= 0)
				{
					e.Cancel = true;
					break;
				}
			}
			if (e.Cancel)
			{
				MessageBox.Show("Value must be specified for every parameter", this.Text);
			}
		}

		private void bOK_Click(object sender, System.EventArgs e)
		{
			CancelEventArgs ce = new CancelEventArgs();
			DrillParametersDialog_Validating(this, ce);
			if (ce.Cancel)
			{
				DialogResult = DialogResult.None;
				return;
			}
			DialogResult = DialogResult.OK;
		}

	}
	internal class DrillParameter
	{
		internal string ParameterName;
		internal string ParameterValue;
		internal string ParameterOmit;
		
		internal DrillParameter(string name, string pvalue, string omit)
		{
			ParameterName = name;
			ParameterValue = pvalue;
			ParameterOmit = omit;
		}
	}
}
