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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Text; 
using System.Reflection;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// DialogListOfStrings: puts up a dialog that lets a user enter a list of strings
	/// </summary>
	public class DialogExprEditor : System.Windows.Forms.Form
	{
        Type[] BASE_TYPES = new Type[] 
      { 
         typeof(string), 
         typeof(double), 
         typeof(Single), 
         typeof(decimal), 
         typeof(DateTime), 
         typeof(char), 
         typeof(bool), 
         typeof(int), 
         typeof(short), 
         typeof(long), 
         typeof(byte), 
         typeof(UInt16), 
         typeof(UInt32), 
         typeof(UInt64) 
      }; 

		private DesignXmlDraw _Draw;		// design draw 
        private bool _Color;				// true if color list should be displayed
        private SplitContainer splitContainer1;
        private Button bCopy;
        private Label lOp;
        private TextBox tbExpr;
        private Label lExpr;
        private TreeView tvOp;
        private Panel panel1;
        private Button bCancel;
        private Button bOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal DialogExprEditor(DesignXmlDraw dxDraw, string expr, XmlNode node) : 
			this(dxDraw, expr, node, false)
		{
		}

		internal DialogExprEditor(DesignXmlDraw dxDraw, string expr, XmlNode node, bool bColor)
		{
			_Draw = dxDraw;
			_Color = bColor;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			tbExpr.Text = expr;

			// Fill out the fields list 
			string[] fields = null;
			// Find the dataregion that contains the item (if any)
			for (XmlNode pNode = node; pNode != null; pNode = pNode.ParentNode)
			{
				if (pNode.Name == "List" ||
					pNode.Name == "Table" ||
					pNode.Name == "Matrix" ||
					pNode.Name == "Chart")
				{
					string dsname = _Draw.GetDataSetNameValue(pNode);
					if (dsname != null)	// found it
					{
						fields = _Draw.GetFields(dsname, true);
					}
				}
			}
			BuildTree(fields);

			return;
		}

		void BuildTree(string[] flds)
		{
			// suppress redraw until tree view is complete
			tvOp.BeginUpdate();
			//AJM GJL Adding Missing 'User' Menu
            // Handle the user
            TreeNode ndRoot = new TreeNode("User");
            tvOp.Nodes.Add(ndRoot);
            foreach (string item in StaticLists.UserList)
            {
                // Add the node to the tree
                TreeNode aRoot = new TreeNode(item.StartsWith("=") ? item.Substring(1) : item);
                ndRoot.Nodes.Add(aRoot);
            }

			// Handle the globals
			ndRoot = new TreeNode("Globals");
			tvOp.Nodes.Add(ndRoot);
			foreach (string item in StaticLists.GlobalList)
			{
				// Add the node to the tree
				TreeNode aRoot = new TreeNode(item.StartsWith("=")? item.Substring(1): item);
				ndRoot.Nodes.Add(aRoot);
			}

			// Fields - only when a dataset is specified
			if (flds != null && flds.Length > 0)
			{
				ndRoot = new TreeNode("Fields");
				tvOp.Nodes.Add(ndRoot);

				foreach (string f in flds)
				{	
					TreeNode aRoot = new TreeNode(f.StartsWith("=")? f.Substring(1): f);
					ndRoot.Nodes.Add(aRoot);
				}
			}

			// Report parameters
			InitReportParameters();

			// Handle the functions
			ndRoot = new TreeNode("Functions");
			tvOp.Nodes.Add(ndRoot);
			InitFunctions(ndRoot);

			// Aggregate functions
			ndRoot = new TreeNode("Aggregate Functions");
			tvOp.Nodes.Add(ndRoot);
			foreach (string item in StaticLists.AggrFunctionList)
			{
				// Add the node to the tree
				TreeNode aRoot = new TreeNode(item);
				ndRoot.Nodes.Add(aRoot);
			}

			// Operators
			ndRoot = new TreeNode("Operators");
			tvOp.Nodes.Add(ndRoot);
			foreach (string item in StaticLists.OperatorList)
			{
				// Add the node to the tree
				TreeNode aRoot = new TreeNode(item);
				ndRoot.Nodes.Add(aRoot);
			}

			// Colors (if requested)
			if (_Color)
			{
				ndRoot = new TreeNode("Colors");
				tvOp.Nodes.Add(ndRoot);
				foreach (string item in StaticLists.ColorList)
				{
					// Add the node to the tree
					TreeNode aRoot = new TreeNode(item);
					ndRoot.Nodes.Add(aRoot);
				}
			}


			tvOp.EndUpdate();

		}

		/// <summary>
		/// Populate tree view with the report parameters (if any)
		/// </summary>
		void InitReportParameters()
		{
			string[] ps = _Draw.GetReportParameters(true);
			
			if (ps == null || ps.Length == 0)
				return;

			TreeNode ndRoot = new TreeNode("Parameters");
			tvOp.Nodes.Add(ndRoot);

			foreach (string p in ps)
			{
				TreeNode aRoot = new TreeNode(p.StartsWith("=")?p.Substring(1): p);
				ndRoot.Nodes.Add(aRoot);
			}

			return;
		}

		void InitFunctions(TreeNode ndRoot)
		{
            List<string> ar = new List<string>();
			
			ar.AddRange(StaticLists.FunctionList);

			// Build list of methods in the  VBFunctions class
			fyiReporting.RDL.FontStyleEnum fsi = FontStyleEnum.Italic;	// just want a class from RdlEngine.dll assembly
			Assembly a = Assembly.GetAssembly(fsi.GetType());
			if (a == null)
				return;
			Type ft = a.GetType("fyiReporting.RDL.VBFunctions");	 
			BuildMethods(ar, ft, "");

			// build list of financial methods in Financial class
			ft = a.GetType("fyiReporting.RDL.Financial");
			BuildMethods(ar, ft, "Financial.");

			a = Assembly.GetAssembly("".GetType());
			ft = a.GetType("System.Math");
			BuildMethods(ar, ft, "Math.");

			ft = a.GetType("System.Convert");
			BuildMethods(ar, ft, "Convert.");

			ft = a.GetType("System.String");
			BuildMethods(ar, ft, "String.");

			ar.Sort();
			string previous="";
			foreach (string item in ar)
			{
				if (item != previous)	// don't add duplicates
				{
					// Add the node to the tree
					TreeNode aRoot = new TreeNode(item);
					ndRoot.Nodes.Add(aRoot);
				}
				previous = item;
			}

		}

        void BuildMethods(List<string> ar, Type ft, string prefix)
		{
			if (ft == null)
				return;
			MethodInfo[] mis = ft.GetMethods(BindingFlags.Static | BindingFlags.Public);
			foreach (MethodInfo mi in mis)
			{
				// Add the node to the tree
				string name = BuildMethodName(mi);
				if (name != null)
					ar.Add(prefix + name);
			}
		}

		string BuildMethodName(MethodInfo mi)
		{
			StringBuilder sb = new StringBuilder(mi.Name);
			sb.Append("(");
			ParameterInfo[] pis = mi.GetParameters();
			bool bFirst=true;
			foreach (ParameterInfo pi in pis)
			{
				if (!IsBaseType(pi.ParameterType))
					return null;
				if (bFirst)
					bFirst = false;
				else
					sb.Append(", ");
				sb.Append(pi.Name);
			}
			sb.Append(")");
			return sb.ToString();
		}

		// Determines if underlying type is a primitive
		bool IsBaseType(Type t)
		{
			foreach (Type bt in BASE_TYPES)
			{
				if (bt == t)
					return true;
			}

			return false;
		}

		public string Expression
		{
			get	{return tbExpr.Text; }
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panel1 = new System.Windows.Forms.Panel();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvOp = new System.Windows.Forms.TreeView();
            this.bCopy = new System.Windows.Forms.Button();
            this.lOp = new System.Windows.Forms.Label();
            this.tbExpr = new System.Windows.Forms.TextBox();
            this.lExpr = new System.Windows.Forms.Label();
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
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bOK);
            this.panel1.Location = new System.Drawing.Point(0, 208);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 40);
            this.panel1.TabIndex = 15;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(374, 9);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(293, 9);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "OK";
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
            this.splitContainer1.Panel1.Controls.Add(this.tvOp);
            this.splitContainer1.Panel1.Controls.Add(this.bCopy);
            this.splitContainer1.Panel1.Controls.Add(this.lOp);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbExpr);
            this.splitContainer1.Panel2.Controls.Add(this.lExpr);
            this.splitContainer1.Size = new System.Drawing.Size(463, 203);
            this.splitContainer1.SplitterDistance = 154;
            this.splitContainer1.TabIndex = 14;
            // 
            // tvOp
            // 
            this.tvOp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvOp.Location = new System.Drawing.Point(0, 29);
            this.tvOp.Name = "tvOp";
            this.tvOp.Size = new System.Drawing.Size(151, 171);
            this.tvOp.TabIndex = 1;
            // 
            // bCopy
            // 
            this.bCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCopy.Location = new System.Drawing.Point(119, 0);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(32, 23);
            this.bCopy.TabIndex = 2;
            this.bCopy.Text = ">>";
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // lOp
            // 
            this.lOp.Location = new System.Drawing.Point(0, 0);
            this.lOp.Name = "lOp";
            this.lOp.Size = new System.Drawing.Size(106, 23);
            this.lOp.TabIndex = 14;
            this.lOp.Text = "Select and hit \'>>\'";
            // 
            // tbExpr
            // 
            this.tbExpr.AcceptsReturn = true;
            this.tbExpr.AcceptsTab = true;
            this.tbExpr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExpr.Location = new System.Drawing.Point(6, 32);
            this.tbExpr.Multiline = true;
            this.tbExpr.Name = "tbExpr";
            this.tbExpr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbExpr.Size = new System.Drawing.Size(296, 168);
            this.tbExpr.TabIndex = 0;
            this.tbExpr.WordWrap = false;
            // 
            // lExpr
            // 
            this.lExpr.Location = new System.Drawing.Point(3, 3);
            this.lExpr.Name = "lExpr";
            this.lExpr.Size = new System.Drawing.Size(134, 20);
            this.lExpr.TabIndex = 13;
            this.lExpr.Text = "Expressions start with \'=\'";
            // 
            // DialogExprEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(463, 248);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogExprEditor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Expression";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void bCopy_Click(object sender, System.EventArgs e)
		{
			if (tvOp.SelectedNode == null ||
				tvOp.SelectedNode.Parent == null)
				return;		// this is the top level nodes (Fields, Parameters, ...)

			TreeNode node = tvOp.SelectedNode;
			string t = node.Text;
            if (tbExpr.Text.Length == 0)
                t = "=" + t;
			tbExpr.SelectedText = t;
		}

	}

}
