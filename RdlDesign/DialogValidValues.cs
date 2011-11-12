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
	/// DialogValidValues allow user to provide ValidValues: Value and Label lists
	/// </summary>
	internal class DialogValidValues : System.Windows.Forms.Form
	{
		private DataTable _DataTable;
		private DataGridTextBoxColumn dgtbLabel;
		private DataGridTextBoxColumn dgtbValue;
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.DataGrid dgParms;
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bDelete;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal DialogValidValues(List<ParameterValueItem> list)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues(list);			
		}

        internal List<ParameterValueItem> ValidValues
		{
			get
			{
                List<ParameterValueItem> list = new List<ParameterValueItem>();
				foreach (DataRow dr in _DataTable.Rows)
				{
					if (dr[0] == DBNull.Value)
						continue;
					string val = (string) dr[0];

					if (val.Length <= 0)
						continue;

					string label;
					if (dr[1] == DBNull.Value)
						label = null;
					else
						label = (string) dr[1];

					ParameterValueItem pvi = new ParameterValueItem();
					pvi.Value = val;
					pvi.Label = label;
					list.Add(pvi);
				}
				return list.Count > 0? list: null;
			}
		}

        private void InitValues(List<ParameterValueItem> list)
		{
			// Initialize the DataGrid columns
			dgtbLabel = new DataGridTextBoxColumn();
			dgtbValue = new DataGridTextBoxColumn();

			this.dgTableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] {
															this.dgtbValue,
															this.dgtbLabel});
			// 
			// dgtbFE
			// 
			dgtbValue.HeaderText = "Value";
			dgtbValue.MappingName = "Value";
			dgtbValue.Width = 75;
			// 
			// dgtbValue
			// 
			this.dgtbLabel.HeaderText = "Label";
			this.dgtbLabel.MappingName = "Label";
			this.dgtbLabel.Width = 75;

			// Initialize the DataGrid
			//this.dgParms.DataSource = _dsv.QueryParameters;

			_DataTable = new DataTable();
			_DataTable.Columns.Add(new DataColumn("Value", typeof(string)));
			_DataTable.Columns.Add(new DataColumn("Label", typeof(string)));

			string[] rowValues = new string[2];
			if (list != null)
			foreach (ParameterValueItem pvi in list)
			{
				rowValues[0] = pvi.Value;
				rowValues[1] = pvi.Label;

				_DataTable.Rows.Add(rowValues);
			}

			this.dgParms.DataSource = _DataTable;

			////
			DataGridTableStyle ts = dgParms.TableStyles[0];
			ts.GridColumnStyles[0].Width = 140;
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
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.bDelete = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParms
			// 
			this.dgParms.CaptionVisible = false;
			this.dgParms.DataMember = "";
			this.dgParms.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParms.Location = new System.Drawing.Point(8, 8);
			this.dgParms.Name = "dgParms";
			this.dgParms.Size = new System.Drawing.Size(320, 168);
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
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(216, 192);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 3;
			this.bOK.Text = "OK";
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(312, 192);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 4;
			this.bCancel.Text = "Cancel";
			// 
			// bDelete
			// 
			this.bDelete.Location = new System.Drawing.Point(336, 16);
			this.bDelete.Name = "bDelete";
			this.bDelete.Size = new System.Drawing.Size(48, 23);
			this.bDelete.TabIndex = 5;
			this.bDelete.Text = "Delete";
			this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
			// 
			// DialogValidValues
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(392, 222);
			this.ControlBox = false;
			this.Controls.Add(this.bDelete);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.dgParms);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogValidValues";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Valid Values";
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void bDelete_Click(object sender, System.EventArgs e)
		{
			this._DataTable.Rows.RemoveAt(this.dgParms.CurrentRowIndex);
		}


	}
}
