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

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for StyleCtl.
	/// </summary>
	internal class DataSetsCtl : System.Windows.Forms.UserControl, IProperty
	{
		private bool _UseTypenameQualified=false;
		private DesignXmlDraw _Draw;
		private XmlNode _dsNode;
		private DataSetValues _dsv;
		private System.Windows.Forms.ComboBox cbDataSource;
		private System.Windows.Forms.TextBox tbDSName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbSQL;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.Button bDeleteField;
		private System.Windows.Forms.Button bEditSQL;
		private System.Windows.Forms.Label lDataSource;
		private System.Windows.Forms.Label lDataSetName;
		private System.Windows.Forms.DataGrid dgFields;
		private System.Windows.Forms.Button bRefresh;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown tbTimeout;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal DataSetsCtl(DesignXmlDraw dxDraw, XmlNode dsNode)
		{
			_Draw = dxDraw;
			_dsNode = dsNode;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		internal DataSetValues DSV
		{
			get {return _dsv;}
		}

		private void InitValues()
		{
			// Initialize the DataGrid columns
			DataGridTextBoxColumn dgtbName = new DataGridTextBoxColumn();
			DataGridTextBoxColumn dgtbQueryName = new DataGridTextBoxColumn();
			DataGridTextBoxColumn dgtbValue = new DataGridTextBoxColumn();
			DataGridTextBoxColumn dgtbTypeName = new DataGridTextBoxColumn();

			this.dataGridTableStyle1.GridColumnStyles.AddRange(new DataGridColumnStyle[] {
																					  dgtbName,
																					  dgtbQueryName,
																					  dgtbValue,
																					  dgtbTypeName});
			// dgtbName
			dgtbName.Format = "";
			dgtbName.FormatInfo = null;
			dgtbName.HeaderText = "Name";
			dgtbName.MappingName = "Name";
			dgtbName.Width = 75;

			// dgtbQueryName
			dgtbQueryName.Format = "";
			dgtbQueryName.FormatInfo = null;
			dgtbQueryName.HeaderText = "Query Column Name";
			dgtbQueryName.MappingName = "QueryName";
			dgtbQueryName.Width = 80;

			// dgtbValue
			// 
			dgtbValue.Format = "";
			dgtbValue.FormatInfo = null;
			dgtbValue.HeaderText = "Value";
			dgtbValue.MappingName = "Value";
			dgtbValue.Width = 175;

			// dgtbTypeName
			dgtbTypeName.Format = "";
			dgtbTypeName.FormatInfo = null;
			dgtbTypeName.HeaderText = "TypeName";
			dgtbTypeName.MappingName = "TypeName";
			dgtbTypeName.Width = 150;

			// cbDataSource
			cbDataSource.Items.AddRange(_Draw.DataSourceNames);

			//
			// Obtain the existing DataSet info
			//
			XmlNode dNode = this._dsNode;
			XmlAttribute nAttr = dNode.Attributes["Name"];

			_dsv = new DataSetValues(nAttr == null? "": nAttr.Value);
			_dsv.Node = dNode;
				
			XmlNode ctNode = DesignXmlDraw.FindNextInHierarchy(dNode, "Query", "CommandText");
			_dsv.CommandText = ctNode == null? "": ctNode.InnerText;
				
			XmlNode datasource = DesignXmlDraw.FindNextInHierarchy(dNode, "Query", "DataSourceName");
			_dsv.DataSourceName = datasource == null? "": datasource.InnerText;

			XmlNode timeout = DesignXmlDraw.FindNextInHierarchy(dNode, "Query", "Timeout");
			try
			{
				_dsv.Timeout = timeout == null? 0: Convert.ToInt32(timeout.InnerText);
			}
			catch		// we don't stop just because timeout isn't convertable
			{
				_dsv.Timeout = 0;
			}

			// Get QueryParameters; they are loaded here but used by the QueryParametersCtl
			_dsv.QueryParameters = new DataTable();
			_dsv.QueryParameters.Columns.Add(new DataColumn("Name", typeof(string)));
			_dsv.QueryParameters.Columns.Add(new DataColumn("Value", typeof(string)));
			XmlNode qpNode = DesignXmlDraw.FindNextInHierarchy(dNode, "Query", "QueryParameters");
			if (qpNode != null)
			{
				string[] rowValues = new string[2];
				foreach (XmlNode qNode in qpNode.ChildNodes)
				{
					if (qNode.Name != "QueryParameter")
						continue;
					XmlAttribute xAttr = qNode.Attributes["Name"];
					if (xAttr == null)
						continue;
					rowValues[0] = xAttr.Value;
					rowValues[1] = _Draw.GetElementValue(qNode, "Value", "");
					_dsv.QueryParameters.Rows.Add(rowValues);
				}
			}

			// Get Fields
			_dsv.Fields = new DataTable();
			_dsv.Fields.Columns.Add(new DataColumn("Name", typeof(string)));
			_dsv.Fields.Columns.Add(new DataColumn("QueryName", typeof(string)));
			_dsv.Fields.Columns.Add(new DataColumn("Value", typeof(string)));
			_dsv.Fields.Columns.Add(new DataColumn("TypeName", typeof(string)));

			XmlNode fsNode = _Draw.GetNamedChildNode(dNode, "Fields");
			if (fsNode != null)
			{
				string[] rowValues = new string[4];
				foreach (XmlNode fNode in fsNode.ChildNodes)
				{
					if (fNode.Name != "Field")
						continue;
					XmlAttribute xAttr = fNode.Attributes["Name"];
					if (xAttr == null)
						continue;
					rowValues[0] = xAttr.Value;
					rowValues[1] = _Draw.GetElementValue(fNode, "DataField", "");
					rowValues[2] = _Draw.GetElementValue(fNode, "Value", "");
					string typename=null;
					typename = _Draw.GetElementValue(fNode, "TypeName", null);
					if (typename == null)
					{
						typename = _Draw.GetElementValue(fNode, "rd:TypeName", null);
						if (typename != null)
							_UseTypenameQualified = true;	// we got it qualified so we'll generate qualified
					}
					rowValues[3] = typename==null?"":typename;

					_dsv.Fields.Rows.Add(rowValues);
				}
			}
			this.tbDSName.Text = _dsv.Name;
			this.tbSQL.Text = _dsv.CommandText;
			this.cbDataSource.Text = _dsv.DataSourceName;
			dgFields.DataSource = _dsv.Fields;
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
            this.cbDataSource = new System.Windows.Forms.ComboBox();
            this.lDataSource = new System.Windows.Forms.Label();
            this.tbDSName = new System.Windows.Forms.TextBox();
            this.lDataSetName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSQL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgFields = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.bDeleteField = new System.Windows.Forms.Button();
            this.bEditSQL = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTimeout = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // cbDataSource
            // 
            this.cbDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataSource.Location = new System.Drawing.Point(296, 8);
            this.cbDataSource.Name = "cbDataSource";
            this.cbDataSource.Size = new System.Drawing.Size(144, 21);
            this.cbDataSource.TabIndex = 1;
            this.cbDataSource.SelectedIndexChanged += new System.EventHandler(this.cbDataSource_SelectedIndexChanged);
            // 
            // lDataSource
            // 
            this.lDataSource.Location = new System.Drawing.Point(224, 8);
            this.lDataSource.Name = "lDataSource";
            this.lDataSource.Size = new System.Drawing.Size(72, 23);
            this.lDataSource.TabIndex = 21;
            this.lDataSource.Text = "Data Source";
            this.lDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDSName
            // 
            this.tbDSName.Location = new System.Drawing.Point(64, 8);
            this.tbDSName.Name = "tbDSName";
            this.tbDSName.Size = new System.Drawing.Size(144, 20);
            this.tbDSName.TabIndex = 0;
            this.tbDSName.TextChanged += new System.EventHandler(this.tbDSName_TextChanged);
            // 
            // lDataSetName
            // 
            this.lDataSetName.Location = new System.Drawing.Point(8, 8);
            this.lDataSetName.Name = "lDataSetName";
            this.lDataSetName.Size = new System.Drawing.Size(48, 16);
            this.lDataSetName.TabIndex = 19;
            this.lDataSetName.Text = "Name";
            this.lDataSetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "SQL Select";
            // 
            // tbSQL
            // 
            this.tbSQL.AcceptsReturn = true;
            this.tbSQL.AcceptsTab = true;
            this.tbSQL.Location = new System.Drawing.Point(8, 56);
            this.tbSQL.Multiline = true;
            this.tbSQL.Name = "tbSQL";
            this.tbSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSQL.Size = new System.Drawing.Size(376, 80);
            this.tbSQL.TabIndex = 5;
            this.tbSQL.Text = "textBox1";
            this.tbSQL.TextChanged += new System.EventHandler(this.tbSQL_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 25;
            this.label2.Text = "Fields";
            // 
            // dgFields
            // 
            this.dgFields.CaptionVisible = false;
            this.dgFields.DataMember = "";
            this.dgFields.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgFields.Location = new System.Drawing.Point(8, 152);
            this.dgFields.Name = "dgFields";
            this.dgFields.Size = new System.Drawing.Size(376, 104);
            this.dgFields.TabIndex = 8;
            this.dgFields.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.dgFields;
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // bDeleteField
            // 
            this.bDeleteField.Location = new System.Drawing.Point(392, 168);
            this.bDeleteField.Name = "bDeleteField";
            this.bDeleteField.Size = new System.Drawing.Size(48, 23);
            this.bDeleteField.TabIndex = 9;
            this.bDeleteField.Text = "Delete";
            this.bDeleteField.Click += new System.EventHandler(this.bDeleteField_Click);
            // 
            // bEditSQL
            // 
            this.bEditSQL.Location = new System.Drawing.Point(392, 64);
            this.bEditSQL.Name = "bEditSQL";
            this.bEditSQL.Size = new System.Drawing.Size(48, 23);
            this.bEditSQL.TabIndex = 6;
            this.bEditSQL.Text = "SQL...";
            this.bEditSQL.Click += new System.EventHandler(this.bEditSQL_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(392, 96);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(52, 32);
            this.bRefresh.TabIndex = 7;
            this.bRefresh.Text = "Refresh Fields";
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(224, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 27;
            this.label3.Text = "Timeout";
            // 
            // tbTimeout
            // 
            this.tbTimeout.Location = new System.Drawing.Point(296, 32);
            this.tbTimeout.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(104, 20);
            this.tbTimeout.TabIndex = 2;
            this.tbTimeout.ThousandsSeparator = true;
            this.tbTimeout.ValueChanged += new System.EventHandler(this.tbTimeout_ValueChanged);
            // 
            // DataSetsCtl
            // 
            this.Controls.Add(this.tbTimeout);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.bEditSQL);
            this.Controls.Add(this.bDeleteField);
            this.Controls.Add(this.dgFields);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSQL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDataSource);
            this.Controls.Add(this.lDataSource);
            this.Controls.Add(this.tbDSName);
            this.Controls.Add(this.lDataSetName);
            this.Name = "DataSetsCtl";
            this.Size = new System.Drawing.Size(448, 304);
            ((System.ComponentModel.ISupportInitialize)(this.dgFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public bool IsValid()
		{
			string nerr = _Draw.NameError(this._dsNode, this.tbDSName.Text);
			if (nerr != null)
			{
				MessageBox.Show(nerr, "Name");
				return false;
			}
			return true;
		}

		public void Apply()
		{
			XmlNode rNode = _Draw.GetReportNode();
			XmlNode dsNode = _Draw.GetNamedChildNode(rNode, "DataSets");

			XmlNode dNode = this._dsNode;
			// Create the name attribute
			_Draw.SetElementAttribute(dNode, "Name", _dsv.Name);

			_Draw.RemoveElement(dNode, "Query");	// get rid of old query
			XmlNode qNode = _Draw.CreateElement(dNode, "Query", null);
			_Draw.SetElement(qNode, "DataSourceName", _dsv.DataSourceName);
			if (_dsv.Timeout > 0)
				_Draw.SetElement(qNode, "Timeout", _dsv.Timeout.ToString());

			_Draw.SetElement(qNode, "CommandText", _dsv.CommandText);

			// Handle QueryParameters
			_Draw.RemoveElement(qNode, "QueryParameters");	// get rid of old QueryParameters
			XmlNode qpsNode = _Draw.CreateElement(qNode, "QueryParameters", null);
			foreach (DataRow dr in _dsv.QueryParameters.Rows)
			{
				if (dr[0] == DBNull.Value || dr[1] == null)
					continue;
				string name = (string) dr[0];
				if (name.Length <= 0)
					continue;
				XmlNode qpNode = _Draw.CreateElement(qpsNode, "QueryParameter", null);
				_Draw.SetElementAttribute(qpNode, "Name", name);
				_Draw.SetElement(qpNode, "Value", (string) dr[1]);	
			}
			if (!qpsNode.HasChildNodes)	// if no parameters we don't need to define them
				_Draw.RemoveElement(qNode, "QueryParameters");

			// Handle Fields
			_Draw.RemoveElement(dNode, "Fields");	// get rid of old Fields
			XmlNode fsNode = _Draw.CreateElement(dNode, "Fields", null);
			foreach (DataRow dr in _dsv.Fields.Rows)
			{
				if (dr[0] == DBNull.Value)
					continue;
				if (dr[1] == DBNull.Value && dr[2] == DBNull.Value)
					continue;
				XmlNode fNode = _Draw.CreateElement(fsNode, "Field", null);
				_Draw.SetElementAttribute(fNode, "Name", (string) dr[0]);
				if (dr[1] != DBNull.Value && 
					dr[1] is string &&
					(string) dr[1] != string.Empty)
					_Draw.SetElement(fNode, "DataField", (string) dr[1]);
				else if (dr[2] != DBNull.Value && 
					dr[2] is string &&
					(string) dr[2] != string.Empty)
					_Draw.SetElement(fNode, "Value", (string) dr[2]);
				else
					_Draw.SetElement(fNode, "DataField", (string) dr[0]);	// make datafield same as name

				// Handle typename if any
				if (dr[3] != DBNull.Value && 
					dr[3] is string &&
					(string) dr[3] != string.Empty)
				{
					_Draw.SetElement(fNode, _UseTypenameQualified? "rd:TypeName":"TypeName", (string) dr[3]);
				}
			}
		}

		private void tbDSName_TextChanged(object sender, System.EventArgs e)
		{
			_dsv.Name = tbDSName.Text;
		}

		private void cbDataSource_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_dsv.DataSourceName = cbDataSource.Text;
		}

		private void tbSQL_TextChanged(object sender, System.EventArgs e)
		{
			_dsv.CommandText = tbSQL.Text;
		}

		private void bDeleteField_Click(object sender, System.EventArgs e)
		{
            if (this.dgFields.CurrentRowIndex < 0)
                return; 
			_dsv.Fields.Rows.RemoveAt(this.dgFields.CurrentRowIndex);
		}

		private void bRefresh_Click(object sender, System.EventArgs e)
		{
			// Need to clear all the fields and then replace with the columns 
			//   of the SQL statement
			List<SqlColumn> cols = DesignerUtility.GetSqlColumns(_Draw, cbDataSource.Text, tbSQL.Text);
			if (cols == null || cols.Count <= 0)
				return;				// something didn't work right
			
			_dsv.Fields.Rows.Clear();
			string[] rowValues = new string[4];
			foreach (SqlColumn sc in cols)
			{
				rowValues[0] = sc.Name;
				rowValues[1] = sc.Name;
				rowValues[2] = "";
                rowValues[3] = sc.DataType.FullName;
				_dsv.Fields.Rows.Add(rowValues);
			}
		}

		private void bEditSQL_Click(object sender, System.EventArgs e)
		{
			SQLCtl sc = new SQLCtl(_Draw, cbDataSource.Text, this.tbSQL.Text, _dsv.QueryParameters);
            try
            {
                DialogResult dr = sc.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    tbSQL.Text = sc.SQL;
                }
            }
            finally
            {
                sc.Dispose();
            }
		}

		private void tbTimeout_ValueChanged(object sender, System.EventArgs e)
		{
			_dsv.Timeout = Convert.ToInt32(tbTimeout.Value);
		}
	}

	internal class DataSetValues
	{
		string _Name;
		string _DataSourceName;
		string _CommandText;
		int    _Timeout;
		DataTable _QueryParameters;  // of type DSQueryParameter
		DataTable _Fields;
		XmlNode _Node;

		internal DataSetValues(string name)
		{
			_Name = name;
		}

		internal string Name
		{
			get {return _Name;}
			set {_Name = value;}
		}

		internal string DataSourceName
		{
			get {return _DataSourceName;}
			set {_DataSourceName = value;}
		}

		internal string CommandText
		{
			get {return _CommandText;}
			set {_CommandText = value;}
		}

		internal int Timeout
		{
			get {return _Timeout;}
			set {_Timeout = value;}
		}

		internal DataTable QueryParameters
		{
			get {return _QueryParameters;}
			set {_QueryParameters = value;}
		}

		internal XmlNode Node
		{
			get {return _Node;}
			set {_Node = value;}
		}

		internal DataTable Fields
		{
			get {return _Fields;}
			set {_Fields = value;}
		}

		override public string ToString()
		{
			return _Name;
		}
	}
}
