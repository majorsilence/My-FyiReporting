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
using System.Text;
using System.Xml;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for ReportCtl.
	/// </summary>
	internal class SQLCtl : System.Windows.Forms.Form
	{
		DesignXmlDraw _Draw;
		string _DataSource;
        DataTable _QueryParameters;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.Button bCancel;
        private SplitContainer splitContainer1;
        private TreeView tvTablesColumns;
        private TextBox tbSQL;
        private Button bMove;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal SQLCtl(DesignXmlDraw dxDraw, string datasource, string sql, DataTable queryParameters)
		{
			_Draw = dxDraw;
			_DataSource = datasource;
			_QueryParameters = queryParameters;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues(sql);			
		}

		private void InitValues(string sql)
		{
			this.tbSQL.Text = sql;

			// Fill out the tables, columns and parameters

			// suppress redraw until tree view is complete
			tvTablesColumns.BeginUpdate();
			
			// Get the schema information
			List<SqlSchemaInfo> si = DesignerUtility.GetSchemaInfo(_Draw, _DataSource);
			if (si != null && si.Count > 0)
			{
				TreeNode ndRoot = new TreeNode("Tables");
				tvTablesColumns.Nodes.Add(ndRoot);
				if (si == null)		// Nothing to initialize
					return;
				bool bView = false;
				foreach (SqlSchemaInfo ssi in si)
				{
					if (!bView && ssi.Type == "VIEW")
					{	// Switch over to views
						ndRoot = new TreeNode("Views");
						tvTablesColumns.Nodes.Add(ndRoot);
						bView=true;
					}

					// Add the node to the tree
					TreeNode aRoot = new TreeNode(ssi.Name);
					ndRoot.Nodes.Add(aRoot);
					aRoot.Nodes.Add("");
				}
			}
			// Now do parameters
			TreeNode qpRoot = null;
			foreach (DataRow dr in _QueryParameters.Rows)
			{
				if (dr[0] == DBNull.Value || dr[1] == null)
					continue;
				string pName = (string) dr[0];
				if (pName.Length == 0)
					continue;
				if (qpRoot == null)
				{
					qpRoot = new TreeNode("Query Parameters");
					tvTablesColumns.Nodes.Add(qpRoot);
				}
				if (pName[0] == '@')
					pName = "@" + pName;
				// Add the node to the tree
				TreeNode aRoot = new TreeNode(pName);
				qpRoot.Nodes.Add(aRoot);
			}

			tvTablesColumns.EndUpdate();
		}

		internal string SQL
		{
			get {return tbSQL.Text;}
			set {tbSQL.Text = value;}
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvTablesColumns = new System.Windows.Forms.TreeView();
            this.tbSQL = new System.Windows.Forms.TextBox();
            this.bMove = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.bOK);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Location = new System.Drawing.Point(0, 215);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 40);
            this.panel1.TabIndex = 6;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(300, 8);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.CausesValidation = false;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(388, 8);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Cancel";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvTablesColumns);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbSQL);
            this.splitContainer1.Panel2.Controls.Add(this.bMove);
            this.splitContainer1.Size = new System.Drawing.Size(468, 215);
            this.splitContainer1.SplitterDistance = 123;
            this.splitContainer1.TabIndex = 9;
            // 
            // tvTablesColumns
            // 
            this.tvTablesColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTablesColumns.FullRowSelect = true;
            this.tvTablesColumns.Location = new System.Drawing.Point(0, 0);
            this.tvTablesColumns.Name = "tvTablesColumns";
            this.tvTablesColumns.Size = new System.Drawing.Size(123, 215);
            this.tvTablesColumns.TabIndex = 5;
            this.tvTablesColumns.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTablesColumns_BeforeExpand);
            // 
            // tbSQL
            // 
            this.tbSQL.AcceptsReturn = true;
            this.tbSQL.AcceptsTab = true;
            this.tbSQL.AllowDrop = true;
            this.tbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSQL.Location = new System.Drawing.Point(37, 0);
            this.tbSQL.Multiline = true;
            this.tbSQL.Name = "tbSQL";
            this.tbSQL.Size = new System.Drawing.Size(299, 215);
            this.tbSQL.TabIndex = 10;
            // 
            // bMove
            // 
            this.bMove.Location = new System.Drawing.Point(3, 3);
            this.bMove.Name = "bMove";
            this.bMove.Size = new System.Drawing.Size(32, 23);
            this.bMove.TabIndex = 9;
            this.bMove.Text = ">>";
            this.bMove.Click += new System.EventHandler(this.bMove_Click);
            // 
            // SQLCtl
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(468, 255);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SQLCtl";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL Syntax Helper";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void bOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
		private void tvTablesColumns_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			tvTablesColumns_ExpandTable(e.Node);
		}

		private void tvTablesColumns_ExpandTable(TreeNode tNode)
		{
			if (tNode.Parent == null)	// Check for Tables or Views
				return;					

			if (tNode.FirstNode.Text != "")	// Have we already filled it out?
				return;

			// Need to obtain the column information for the requested table/view
			// suppress redraw until tree view is complete
			tvTablesColumns.BeginUpdate();

            string sql = "SELECT * FROM " + DesignerUtility.NormalizeSqlName(tNode.Text);
			List<SqlColumn> tColumns = DesignerUtility.GetSqlColumns(_Draw, _DataSource, sql);
			bool bFirstTime=true;
			foreach (SqlColumn sc in tColumns)
			{
				if (bFirstTime)
				{
					bFirstTime = false;
					tNode.FirstNode.Text = sc.Name;
				}
				else
					tNode.Nodes.Add(sc.Name);
			}

			tvTablesColumns.EndUpdate();
		}

		private void bMove_Click(object sender, System.EventArgs e)
		{
			if (tvTablesColumns.SelectedNode == null ||
				tvTablesColumns.SelectedNode.Parent == null)
				return;		// this is the Tables/Views node

			TreeNode node = tvTablesColumns.SelectedNode;
			string t = node.Text;
			if (tbSQL.Text == "")
			{
				if (node.Parent.Parent == null)
				{	// select table; generate full select for table
					tvTablesColumns_ExpandTable(node);	// make sure we've obtained the columns

					StringBuilder sb = new StringBuilder("SELECT ");
					TreeNode next = node.FirstNode;
					while (true)
					{
                        sb.Append(DesignerUtility.NormalizeSqlName(next.Text));
						next = next.NextNode;
						if (next == null)
							break;
						sb.Append(", ");
					}
					sb.Append(" FROM ");
                    sb.Append(DesignerUtility.NormalizeSqlName(node.Text));
					t = sb.ToString();
				}
				else
				{	// select column; generate select of that column	
                    t = "SELECT " + DesignerUtility.NormalizeSqlName(node.Text) + " FROM " + DesignerUtility.NormalizeSqlName(node.Parent.Text);
				}
			}

			tbSQL.SelectedText = t;
		}


	}
}
