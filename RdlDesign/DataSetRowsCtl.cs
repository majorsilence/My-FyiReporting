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
using System.Globalization;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Control supports the properties for DataSet/Rows elements.  This is an extension to 
	/// the RDL specification allowing data to be defined within a report.
	/// </summary>
	internal class DataSetRowsCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private DataSetValues _dsv;
		private XmlNode _dsNode;
		private DataTable _DataTable;

		private System.Windows.Forms.Button bDelete;
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.Button bUp;
		private System.Windows.Forms.Button bDown;
		private System.Windows.Forms.CheckBox chkRowsFile;
		private System.Windows.Forms.Button bRowsFile;
		private System.Windows.Forms.DataGrid dgRows;
		private System.Windows.Forms.TextBox tbRowsFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bLoad;
		private System.Windows.Forms.Button bClear;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal DataSetRowsCtl(DesignXmlDraw dxDraw, XmlNode dsNode, DataSetValues dsv)
		{
			_Draw = dxDraw;
			_dsv = dsv;
			_dsNode = dsNode;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			CreateDataTable();		// create data table based on the existing fields

			XmlNode rows = _Draw.GetNamedChildNode(_dsNode, "Rows");
			if (rows == null)
				rows = _Draw.GetNamedChildNode(_dsNode, "fyi:Rows");
			string file=null;
			if (rows != null)
			{
				file = _Draw.GetElementAttribute(rows, "File", null);
				PopulateRows(rows);
			}

			this.dgRows.DataSource = _DataTable;
			if (file != null)
			{
				tbRowsFile.Text = file;
				this.chkRowsFile.Checked = true;
			}

			chkRowsFile_CheckedChanged(this, new EventArgs());
		}

		private void CreateDataTable()
		{
			_DataTable = new DataTable();
			dgTableStyle.GridColumnStyles.Clear();	// reset the grid column styles

			foreach (DataRow dr in _dsv.Fields.Rows)
			{
				if (dr[0] == DBNull.Value)
					continue;
				if (dr[2] == DBNull.Value)
				{}
				else if (((string) dr[2]).Length > 0)
					continue;
				string name = (string) dr[0];
				DataGridTextBoxColumn dgc = new DataGridTextBoxColumn();
				dgTableStyle.GridColumnStyles.Add(dgc);
				dgc.HeaderText = name;
				dgc.MappingName = name;
				dgc.Width = 75;
                string type = dr["TypeName"] as string;
                Type t = type == null || type.Length == 0? typeof(string): 
                    fyiReporting.RDL.DataType.GetStyleType(type);
				_DataTable.Columns.Add(new DataColumn(name,t));
			}
		}

		private void PopulateRows(XmlNode rows)
		{
			object[] rowValues = new object[_DataTable.Columns.Count];

            bool bSkipMsg = false;
            foreach (XmlNode rNode in rows.ChildNodes)
			{
				if (rNode.Name != "Row")
					continue;
				int col=0;
				bool bBuiltRow=false;	// if all columns will be null we won't add the row
				foreach (DataColumn dc in _DataTable.Columns)
				{
					XmlNode dNode = _Draw.GetNamedChildNode(rNode, dc.ColumnName);
					if (dNode != null)
						bBuiltRow = true;

                    if (dNode == null)
                        rowValues[col] = null;
                    else if (dc.DataType == typeof(string))
                        rowValues[col] = dNode.InnerText;
                    else
                    {
                        object box;
                        try
                        {
                            if (dc.DataType == typeof(int))
                                box = Convert.ToInt32(dNode.InnerText, NumberFormatInfo.InvariantInfo);
                            else if (dc.DataType == typeof(decimal))
                                box = Convert.ToDecimal(dNode.InnerText, NumberFormatInfo.InvariantInfo);
                            else if (dc.DataType == typeof(long))
                                box = Convert.ToInt64(dNode.InnerText, NumberFormatInfo.InvariantInfo);
                            else if (DesignerUtility.IsNumeric(dc.DataType))    // catch all numeric
                                box = Convert.ToDouble(dNode.InnerText, NumberFormatInfo.InvariantInfo);
                            else if (dc.DataType == typeof(DateTime))
                            {
                                box = Convert.ToDateTime(dNode.InnerText,
                                    System.Globalization.DateTimeFormatInfo.InvariantInfo);
                            }
                            else
                            {
                                box = dNode.InnerText;
                            }
                            rowValues[col] = box;
                        }
                        catch (Exception e)
                        {
                            if (!bSkipMsg)
                            {
                                if (MessageBox.Show(string.Format("Unable to convert {1} to {0}: {2}",
                                        dc.DataType.ToString(), dNode.InnerText, e.Message) + Environment.NewLine + "Do you want to see any more errors?",
                                        "Error Reading Data Rows", MessageBoxButtons.YesNo) == DialogResult.No)
                                    bSkipMsg = true;
                            }
                            rowValues[col] = dNode.InnerText;
                        }
                    }
					col++;
				}
				if (bBuiltRow)
					_DataTable.Rows.Add(rowValues);
			}
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
			this.dgRows = new System.Windows.Forms.DataGrid();
			this.dgTableStyle = new System.Windows.Forms.DataGridTableStyle();
			this.bDelete = new System.Windows.Forms.Button();
			this.bUp = new System.Windows.Forms.Button();
			this.bDown = new System.Windows.Forms.Button();
			this.chkRowsFile = new System.Windows.Forms.CheckBox();
			this.tbRowsFile = new System.Windows.Forms.TextBox();
			this.bRowsFile = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.bLoad = new System.Windows.Forms.Button();
			this.bClear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgRows)).BeginInit();
			this.SuspendLayout();
			// 
			// dgRows
			// 
			this.dgRows.CaptionVisible = false;
			this.dgRows.DataMember = "";
			this.dgRows.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgRows.Location = new System.Drawing.Point(8, 48);
			this.dgRows.Name = "dgRows";
			this.dgRows.Size = new System.Drawing.Size(376, 200);
			this.dgRows.TabIndex = 2;
			this.dgRows.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																							   this.dgTableStyle});
			// 
			// dgTableStyle
			// 
			this.dgTableStyle.AllowSorting = false;
			this.dgTableStyle.DataGrid = this.dgRows;
			this.dgTableStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgTableStyle.MappingName = "";
			// 
			// bDelete
			// 
			this.bDelete.Location = new System.Drawing.Point(392, 48);
			this.bDelete.Name = "bDelete";
			this.bDelete.Size = new System.Drawing.Size(48, 23);
			this.bDelete.TabIndex = 1;
			this.bDelete.Text = "Delete";
			this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
			// 
			// bUp
			// 
			this.bUp.Location = new System.Drawing.Point(392, 80);
			this.bUp.Name = "bUp";
			this.bUp.Size = new System.Drawing.Size(48, 23);
			this.bUp.TabIndex = 3;
			this.bUp.Text = "Up";
			this.bUp.Click += new System.EventHandler(this.bUp_Click);
			// 
			// bDown
			// 
			this.bDown.Location = new System.Drawing.Point(392, 112);
			this.bDown.Name = "bDown";
			this.bDown.Size = new System.Drawing.Size(48, 23);
			this.bDown.TabIndex = 4;
			this.bDown.Text = "Down";
			this.bDown.Click += new System.EventHandler(this.bDown_Click);
			// 
			// chkRowsFile
			// 
			this.chkRowsFile.Location = new System.Drawing.Point(8, 8);
			this.chkRowsFile.Name = "chkRowsFile";
			this.chkRowsFile.Size = new System.Drawing.Size(136, 24);
			this.chkRowsFile.TabIndex = 5;
			this.chkRowsFile.Text = "Use XML file for data";
			this.chkRowsFile.CheckedChanged += new System.EventHandler(this.chkRowsFile_CheckedChanged);
			// 
			// tbRowsFile
			// 
			this.tbRowsFile.Location = new System.Drawing.Point(144, 8);
			this.tbRowsFile.Name = "tbRowsFile";
			this.tbRowsFile.Size = new System.Drawing.Size(240, 20);
			this.tbRowsFile.TabIndex = 6;
			this.tbRowsFile.Text = "";
			// 
			// bRowsFile
			// 
			this.bRowsFile.Location = new System.Drawing.Point(392, 8);
			this.bRowsFile.Name = "bRowsFile";
			this.bRowsFile.Size = new System.Drawing.Size(24, 23);
			this.bRowsFile.TabIndex = 7;
			this.bRowsFile.Text = "...";
			this.bRowsFile.Click += new System.EventHandler(this.bRowsFile_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 256);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(368, 23);
			this.label1.TabIndex = 8;
			this.label1.Text = "Warning: this panel supports an extension to the RDL specification.  This informa" +
				"tion will be ignored in RDL processors other than in fyiReporting.";
			// 
			// bLoad
			// 
			this.bLoad.Location = new System.Drawing.Point(392, 184);
			this.bLoad.Name = "bLoad";
			this.bLoad.Size = new System.Drawing.Size(48, 48);
			this.bLoad.TabIndex = 9;
			this.bLoad.Text = "Load From SQL";
			this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
			// 
			// bClear
			// 
			this.bClear.Location = new System.Drawing.Point(392, 141);
			this.bClear.Name = "bClear";
			this.bClear.Size = new System.Drawing.Size(48, 23);
			this.bClear.TabIndex = 10;
			this.bClear.Text = "Clear";
			this.bClear.Click += new System.EventHandler(this.bClear_Click);
			// 
			// DataSetRowsCtl
			// 
			this.Controls.Add(this.bClear);
			this.Controls.Add(this.bLoad);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bRowsFile);
			this.Controls.Add(this.tbRowsFile);
			this.Controls.Add(this.chkRowsFile);
			this.Controls.Add(this.bDown);
			this.Controls.Add(this.bUp);
			this.Controls.Add(this.bDelete);
			this.Controls.Add(this.dgRows);
			this.Name = "DataSetRowsCtl";
			this.Size = new System.Drawing.Size(488, 304);
			this.VisibleChanged += new System.EventHandler(this.DataSetRowsCtl_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.dgRows)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public bool IsValid()
		{
			if (this.chkRowsFile.Checked && this.tbRowsFile.Text.Length == 0)
			{
				MessageBox.Show("File name required when 'Use XML file for data checked'");
				return false;
			}
			return true;
		}

		public void Apply()
		{
			// Remove the old row
			XmlNode rows = _Draw.GetNamedChildNode(this._dsNode, "Rows");
			if (rows == null)
				rows =  _Draw.GetNamedChildNode(this._dsNode, "fyi:Rows");
			if (rows != null)
				_dsNode.RemoveChild(rows);
			// different result if we just want the file
			if (this.chkRowsFile.Checked)
			{
				rows = _Draw.GetCreateNamedChildNode(_dsNode, "fyi:Rows");
				_Draw.SetElementAttribute(rows, "File", this.tbRowsFile.Text);
			}
			else
			{
				rows = GetXmlData();
				if (rows.HasChildNodes)
					_dsNode.AppendChild(rows);
			}
		}

		private void bDelete_Click(object sender, System.EventArgs e)
		{
			this._DataTable.Rows.RemoveAt(this.dgRows.CurrentRowIndex);
		}

		private void bUp_Click(object sender, System.EventArgs e)
		{
			int cr = dgRows.CurrentRowIndex;
			if (cr <= 0)		// already at the top
				return;
			
			SwapRow(_DataTable.Rows[cr-1], _DataTable.Rows[cr]);
			dgRows.CurrentRowIndex = cr-1;
		}

		private void bDown_Click(object sender, System.EventArgs e)
		{
			int cr = dgRows.CurrentRowIndex;
			if (cr < 0)			// invalid index
				return;
			if (cr + 1 >= _DataTable.Rows.Count)
				return;			// already at end
			
			SwapRow(_DataTable.Rows[cr+1], _DataTable.Rows[cr]);
			dgRows.CurrentRowIndex = cr+1;
		}

		private void SwapRow(DataRow tdr, DataRow fdr)
		{
			// Loop thru all the columns in a row and swap the data
			for (int ci=0; ci < _DataTable.Columns.Count; ci++)
			{
				object save = tdr[ci];
				tdr[ci] = fdr[ci];
				fdr[ci] = save;
			}
			return;
		}

		private void chkRowsFile_CheckedChanged(object sender, System.EventArgs e)
		{
			this.tbRowsFile.Enabled = chkRowsFile.Checked;
			this.bRowsFile.Enabled = chkRowsFile.Checked;

			this.bDelete.Enabled = !chkRowsFile.Checked;
			this.bUp.Enabled = !chkRowsFile.Checked;
			this.bDown.Enabled = !chkRowsFile.Checked;
			this.dgRows.Enabled = !chkRowsFile.Checked;
		}

		private void bRowsFile_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "XML files (*.xml)|*.xml" +
                "|All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			ofd.FileName = "*.xml";

			ofd.Title = "Specify XML File Name";
			ofd.DefaultExt = "xml";
			ofd.AddExtension = true;

            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileName(ofd.FileName);

                    this.tbRowsFile.Text = file;
                }
            }
            finally
            {
                ofd.Dispose();
            }
		}

		private bool DidFieldsChange()
		{
			int col=0;
			foreach (DataRow dr in _dsv.Fields.Rows)
			{
				if (col >= _DataTable.Columns.Count)
					return true;

				if (dr[0] == DBNull.Value)
					continue;
				if (dr[2] == DBNull.Value)
				{}
				else if (((string) dr[2]).Length > 0)
					continue;
				string name = (string) (dr[1] == DBNull.Value? dr[0]: dr[1]);
				if (_DataTable.Columns[col].ColumnName != name)
					return true;
				col++;
			}

			if (col == _DataTable.Columns.Count)
				return false;
			else
				return true;
		}

		private XmlNode GetXmlData()
		{
			XmlDocumentFragment fDoc = _Draw.ReportDocument.CreateDocumentFragment();

			XmlNode rows = _Draw.CreateElement(fDoc, "fyi:Rows", null);
			foreach (DataRow dr in _DataTable.Rows)
			{
				XmlNode row = _Draw.CreateElement(rows, "Row", null);
				bool bRowBuilt=false;
				foreach (DataColumn dc in _DataTable.Columns)
				{
					if (dr[dc] == DBNull.Value)
						continue;
                    string val;
                    if (dc.DataType == typeof(DateTime))
                    {
                        val = Convert.ToString(dr[dc], 
                            System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    }
                    else
                    {
                        val = Convert.ToString(dr[dc], NumberFormatInfo.InvariantInfo);
                    }
					if (val == null)
						continue;
					_Draw.CreateElement(row, dc.ColumnName, val);
					bRowBuilt = true;	// we've populated at least one column; so keep row
				}
				if (!bRowBuilt)
					rows.RemoveChild(row);
			}
			return rows;
		}

		private void DataSetRowsCtl_VisibleChanged(object sender, System.EventArgs e)
		{
			if (!DidFieldsChange())		// did the structure of the fields change
				return;

			// Need to reset the data; this assumes that some of the data rows are similar
			XmlNode rows = GetXmlData();	// get old data
			
			CreateDataTable();				// this recreates the datatable

			PopulateRows(rows);				// repopulate the datatable

			this.dgRows.DataSource = _DataTable;	// this recreates the datatable so reset grid
		}

		private void bClear_Click(object sender, System.EventArgs e)
		{
			this._DataTable.Rows.Clear();
		}

		private void bLoad_Click(object sender, System.EventArgs e)
		{
			// Load the data from the SQL; we append the data to what already exists
			try
			{
				// Obtain the connection information
				XmlNode rNode = _Draw.GetReportNode();
				XmlNode dsNode = _Draw.GetNamedChildNode(rNode, "DataSources");
				if (dsNode == null)
					return;
				XmlNode datasource=null;
				foreach (XmlNode dNode in dsNode)
				{	
					if (dNode.Name != "DataSource")
						continue;
					XmlAttribute nAttr = dNode.Attributes["Name"];
					if (nAttr == null)	// shouldn't really happen
						continue;
					if (nAttr.Value != _dsv.DataSourceName)
						continue;
					datasource = dNode;
					break;
				}
				if (datasource == null)
				{
					MessageBox.Show(string.Format("Datasource '{0}' not found.", _dsv.DataSourceName), "Load Failed");
					return;
				}
                // get the connection information
                string connection = "";
                string dataProvider = "";

                string dataSourceReference = _Draw.GetElementValue(datasource, "DataSourceReference", null);
                if (dataSourceReference != null)
                {
                    //  This is not very pretty code since it is assuming the structure of the windows parenting.
                    //    But there isn't any other way to get this information from here.
                    Control p = _Draw;
                    MDIChild mc = null;
                    while (p != null && !(p is RdlDesigner))
                    {
                        if (p is MDIChild)
                            mc = (MDIChild)p;

                        p = p.Parent;
                    }
                    if (p == null || mc == null || mc.SourceFile == null)
                    {
                        MessageBox.Show("Unable to locate DataSource Shared file.  Try saving report first");
                        return;
                    }
                    string filename = Path.GetDirectoryName(mc.SourceFile) + Path.DirectorySeparatorChar + dataSourceReference;
                    if (!DesignerUtility.GetSharedConnectionInfo((RdlDesigner) p, filename, out dataProvider, out connection))
                        return;
                }
                else
                {
                    XmlNode cpNode = DesignXmlDraw.FindNextInHierarchy(datasource, "ConnectionProperties", "ConnectString");
                    connection = cpNode == null ? "" : cpNode.InnerText;

                    XmlNode datap = DesignXmlDraw.FindNextInHierarchy(datasource, "ConnectionProperties", "DataProvider");
                    dataProvider = datap == null ? "" : datap.InnerText;
                }
				// Populate the data table				
				DesignerUtility.GetSqlData(dataProvider, connection, _dsv.CommandText, null, _DataTable);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Load Failed");
			}
		}
	}
}
