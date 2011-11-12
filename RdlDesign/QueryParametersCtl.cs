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
	/// QueryParametersCtl provides values for the DataSet Query QueryParameters rdl elements
	/// </summary>
	internal class QueryParametersCtl : System.Windows.Forms.UserControl, IProperty
	{
		private DesignXmlDraw _Draw;
		private DataSetValues _dsv;
		private DataGridTextBoxColumn dgtbName;
		private DataGridTextBoxColumn dgtbValue;
		private System.Windows.Forms.DataGridTableStyle dgTableStyle;
		private System.Windows.Forms.DataGrid dgParms;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal QueryParametersCtl(DesignXmlDraw dxDraw, DataSetValues dsv)
		{
			_Draw = dxDraw;
			_dsv = dsv;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
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
			dgtbName.MappingName = "Name";
			dgtbName.Width = 75;
			// 
			// dgtbValue
			// 
			this.dgtbValue.HeaderText = "Value";
			this.dgtbValue.MappingName = "Value";
			this.dgtbValue.Width = 75;
//			string[] parms = _Draw.GetReportParameters(true);
//			if (parms != null)
//				dgtbFV.CB.Items.AddRange(parms);

			// Initialize the DataGrid
			this.dgParms.DataSource = _dsv.QueryParameters;

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
			// SubreportCtl
			// 
			this.Controls.Add(this.dgParms);
			this.Name = "SubreportCtl";
			this.Size = new System.Drawing.Size(464, 304);
			((System.ComponentModel.ISupportInitialize)(this.dgParms)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
 

		public bool IsValid()
		{
			return true;
		}

		public void Apply()
		{
			return;			// the apply is done as part of the DataSetsCtl
		}

	}
}
