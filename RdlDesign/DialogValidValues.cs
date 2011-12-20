/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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
    internal partial class DialogValidValues 
    {
        private DataTable _DataTable;

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
                    string val = (string)dr[0];

                    if (val.Length <= 0)
                        continue;

                    string label;
                    if (dr[1] == DBNull.Value)
                        label = null;
                    else
                        label = (string)dr[1];

                    ParameterValueItem pvi = new ParameterValueItem();
                    pvi.Value = val;
                    pvi.Label = label;
                    list.Add(pvi);
                }
                return list.Count > 0 ? list : null;
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

        private void bDelete_Click(object sender, System.EventArgs e)
        {
            this._DataTable.Rows.RemoveAt(this.dgParms.CurrentRowIndex);
        }


    }
}
