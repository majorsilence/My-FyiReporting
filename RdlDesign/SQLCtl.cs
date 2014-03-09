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
using System.Text;
using System.Xml;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for ReportCtl.
    /// </summary>
    internal partial class SQLCtl
    {

        string _DataSource;
        DataTable _QueryParameters;

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
			this.tbSQL.Text = sql.Replace("\n", Environment.NewLine);

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
                        bView = true;
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
                string pName = (string)dr[0];
                if (pName.Length == 0)
                    continue;
                if (qpRoot == null)
                {
                    qpRoot = new TreeNode("Query Parameters");
                    tvTablesColumns.Nodes.Add(qpRoot);
                }
                if (pName[0] != '@')
                {
                    pName = "@" + pName;
                }
                // Add the node to the tree
                TreeNode aRoot = new TreeNode(pName);
                qpRoot.Nodes.Add(aRoot);
            }

            tvTablesColumns.EndUpdate();
        }

        internal string SQL
        {
            get { return tbSQL.Text; }
            set { tbSQL.Text = value; }
        }

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
            bool bFirstTime = true;
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
