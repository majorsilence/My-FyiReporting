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

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Filters specification: used for DataRegions (List, Chart, Table, Matrix), DataSets, group instances
	/// </summary>
	internal class FiltersCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private XmlNode _FilterParent;
        private DataGridViewTextBoxColumn dgtbFE;
        private DataGridViewComboBoxColumn dgtbOP;
        private DataGridViewTextBoxColumn dgtbFV;

		private System.Windows.Forms.Button bDelete;
		private System.Windows.Forms.DataGridView dgFilters;
		private System.Windows.Forms.Button bUp;
		private System.Windows.Forms.Button bDown;
		private System.Windows.Forms.Button bValueExpr;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal FiltersCtl(DesignXmlDraw dxDraw, XmlNode filterParent)
		{
			_Draw = dxDraw;
			_FilterParent = filterParent;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			// Initialize the DataGrid columns

            dgtbFE = new DataGridViewTextBoxColumn();
            dgtbOP = new DataGridViewComboBoxColumn(); 
            dgtbOP.Items.AddRange(new string[] 
                { "Equal", "Like", "NotEqual", "GreaterThan", "GreaterThanOrEqual", "LessThan",
                    "LessThanOrEqual", "TopN", "BottomN", "TopPercent", "BottomPercent", "In", "Between" });
            dgtbFV = new DataGridViewTextBoxColumn();

            dgFilters.Columns.Add(dgtbFE);
            dgFilters.Columns.Add(dgtbOP);
            dgFilters.Columns.Add(dgtbFV);
            // 
			// dgtbFE
			// 
			dgtbFE.HeaderText = "Filter Expression";
			dgtbFE.Width = 130;
			// Get the parent's dataset name
			//string dataSetName = _Draw.GetDataSetNameValue(_FilterParent);

            // unfortunately no way to make combo box editable
            //string[] fields = _Draw.GetFields(dataSetName, true);
            //if (fields != null)
            //    dgtbFE.Items.AddRange(fields);

            dgtbOP.HeaderText = "Operator";
			dgtbOP.Width = 100;
            dgtbOP.DropDownWidth = 140;
			// 
			// dgtbFV
			// 
			this.dgtbFV.HeaderText = "Value(s)";
			this.dgtbFV.Width = 130;
            //string[] parms = _Draw.GetReportParameters(true);
            //if (parms != null)
            //    dgtbFV.Items.AddRange(parms);

			XmlNode filters = _Draw.GetNamedChildNode(_FilterParent, "Filters");

			if (filters != null)
			foreach (XmlNode fNode in filters.ChildNodes)
			{
				if (fNode.NodeType != XmlNodeType.Element || 
						fNode.Name != "Filter")
					continue;
				// Get the values
				XmlNode vNodes = _Draw.GetNamedChildNode(fNode, "FilterValues");
				StringBuilder sb = new StringBuilder();
				if (vNodes != null)
				{
					foreach (XmlNode v in vNodes.ChildNodes)
					{
						if (v.InnerText.Length <= 0)
							continue;
						if (sb.Length != 0)
							sb.Append(", ");
						sb.Append(v.InnerText);
					}
				}
                // Add the row
                dgFilters.Rows.Add(_Draw.GetElementValue(fNode, "FilterExpression", ""),
                    _Draw.GetElementValue(fNode, "Operator", "Equal"),
                    sb.ToString());
			}

            if (dgFilters.Rows.Count == 0)
                dgFilters.Rows.Add("","Equal","");
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
            this.dgFilters = new System.Windows.Forms.DataGridView();
            this.bDelete = new System.Windows.Forms.Button();
            this.bUp = new System.Windows.Forms.Button();
            this.bDown = new System.Windows.Forms.Button();
            this.bValueExpr = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgFilters)).BeginInit();
            this.SuspendLayout();
            // 
            // dgFilters
            // 
            this.dgFilters.Location = new System.Drawing.Point(8, 8);
            this.dgFilters.Name = "dgFilters";
            this.dgFilters.Size = new System.Drawing.Size(376, 264);
            this.dgFilters.TabIndex = 2;
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(392, 40);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(48, 23);
            this.bDelete.TabIndex = 1;
            this.bDelete.Text = "Delete";
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bUp
            // 
            this.bUp.Location = new System.Drawing.Point(392, 71);
            this.bUp.Name = "bUp";
            this.bUp.Size = new System.Drawing.Size(48, 23);
            this.bUp.TabIndex = 3;
            this.bUp.Text = "Up";
            this.bUp.Click += new System.EventHandler(this.bUp_Click);
            // 
            // bDown
            // 
            this.bDown.Location = new System.Drawing.Point(392, 102);
            this.bDown.Name = "bDown";
            this.bDown.Size = new System.Drawing.Size(48, 23);
            this.bDown.TabIndex = 4;
            this.bDown.Text = "Down";
            this.bDown.Click += new System.EventHandler(this.bDown_Click);
            // 
            // bValueExpr
            // 
            this.bValueExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bValueExpr.Location = new System.Drawing.Point(392, 16);
            this.bValueExpr.Name = "bValueExpr";
            this.bValueExpr.Size = new System.Drawing.Size(22, 16);
            this.bValueExpr.TabIndex = 5;
            this.bValueExpr.Tag = "value";
            this.bValueExpr.Text = "fx";
            this.bValueExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bValueExpr.Click += new System.EventHandler(this.bValueExpr_Click);
            // 
            // FiltersCtl
            // 
            this.Controls.Add(this.bValueExpr);
            this.Controls.Add(this.bDown);
            this.Controls.Add(this.bUp);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.dgFilters);
            this.Name = "FiltersCtl";
            this.Size = new System.Drawing.Size(488, 304);
            ((System.ComponentModel.ISupportInitialize)(this.dgFilters)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
 		public bool IsValid()
		{
			return true;
		}

		public void Apply()
		{
			// Remove the old filters
			XmlNode filters = null;
			_Draw.RemoveElement(_FilterParent, "Filters");

			// Loop thru and add all the filters
			foreach (DataGridViewRow dr in this.dgFilters.Rows)
			{
				string fe = dr.Cells[0].Value as string;
                string op = dr.Cells[1].Value as string;
                string fv = dr.Cells[2].Value as string;
				if (fe == null || fe.Length <= 0 || 
                    op == null || op.Length <= 0 || 
                    fv == null || fv.Length <= 0)
					continue;
				if (filters == null)
					filters = _Draw.CreateElement(_FilterParent, "Filters", null);

				XmlNode fNode = _Draw.CreateElement(filters, "Filter", null);
				_Draw.CreateElement(fNode, "FilterExpression", fe);
				_Draw.CreateElement(fNode, "Operator", op);
				XmlNode fvNode = _Draw.CreateElement(fNode, "FilterValues", null);
				if (op == "In")
				{
					string[] vs = fv.Split(',');
					foreach (string v in vs)
						_Draw.CreateElement(fvNode, "FilterValue", v.Trim());
				}
				else if (op == "Between")
				{
					string[] vs = fv.Split(new char[] {','}, 2);
					foreach (string v in vs)
						_Draw.CreateElement(fvNode, "FilterValue", v.Trim());
				}
				else
				{
					_Draw.CreateElement(fvNode, "FilterValue", fv);
				}
			}
		}

		private void bDelete_Click(object sender, System.EventArgs e)
		{
            if (dgFilters.CurrentRow == null)
                return;

            if (!dgFilters.Rows[dgFilters.CurrentRow.Index].IsNewRow)   // can't delete the new row
                dgFilters.Rows.RemoveAt(this.dgFilters.CurrentRow.Index);
            else
            {   // just empty out the values
                DataGridViewRow dgrv = dgFilters.Rows[this.dgFilters.CurrentRow.Index];
                dgrv.Cells[0].Value = null;
                dgrv.Cells[1].Value = "Equal";
                dgrv.Cells[2].Value = null;
            }
		}

		private void bUp_Click(object sender, System.EventArgs e)
		{
            int cr = dgFilters.CurrentRow == null ? 0 : dgFilters.CurrentRow.Index;
            if (cr <= 0)		// already at the top
				return;

            SwapRow(dgFilters.Rows[cr - 1], dgFilters.Rows[cr]);

            dgFilters.CurrentCell = 
                dgFilters.Rows[cr-1].Cells[dgFilters.CurrentCell.ColumnIndex];
		}

		private void bDown_Click(object sender, System.EventArgs e)
		{
            int cr = dgFilters.CurrentRow == null ? 0 : dgFilters.CurrentRow.Index;
            if (cr < 0)			// invalid index
				return;
            if (cr + 1 >= dgFilters.Rows.Count)
                return;			// already at end
			
            SwapRow(dgFilters.Rows[cr+1], dgFilters.Rows[cr]);
            dgFilters.CurrentCell =
                dgFilters.Rows[cr + 1].Cells[dgFilters.CurrentCell.ColumnIndex];
		}

        private void SwapRow(DataGridViewRow tdr, DataGridViewRow fdr)
		{
			// column 1
			object save = tdr.Cells[0].Value;
            tdr.Cells[0].Value = fdr.Cells[0].Value;
            fdr.Cells[0].Value = save;
			// column 2
            save = tdr.Cells[1].Value;
            tdr.Cells[1].Value = fdr.Cells[1].Value;
            fdr.Cells[1].Value = save;
			// column 3
            save = tdr.Cells[2].Value;
            tdr.Cells[2].Value = fdr.Cells[2].Value;
            fdr.Cells[2].Value = save;
			return;
		}

		private void bValueExpr_Click(object sender, System.EventArgs e)
		{
            if (dgFilters.CurrentCell == null)
                dgFilters.Rows.Add("", "Equal", "");
            DataGridViewCell dgc = dgFilters.CurrentCell;
			int cc = dgc.ColumnIndex;
			string cv = dgc.Value as string;

			if (cc == 1)
			{	// This is the FilterOperator
				DialogFilterOperator fo = new DialogFilterOperator(cv);
                try
                {
                    DialogResult dlgr = fo.ShowDialog();
                    if (dlgr == DialogResult.OK)
                        dgc.Value = fo.Operator;
                }
                finally
                {
                    fo.Dispose();
                }
			}
			else
			{
				DialogExprEditor ee = new DialogExprEditor(_Draw, cv, _FilterParent, false);
                try
                {
                    DialogResult dlgr = ee.ShowDialog();
                    if (dlgr == DialogResult.OK)
                        dgc.Value = ee.Expression;
                }
                finally
                {
                    ee.Dispose();
                }
			}
		}
	}
}
