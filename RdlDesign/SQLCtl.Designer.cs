using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class SQLCtl : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		DesignXmlDraw _Draw;
private System.Windows.Forms.Panel panel1;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private SplitContainer splitContainer1;
private TreeView tvTablesColumns;
private TextBox tbSQL;
private Button bMove;
private System.ComponentModel.Container components = null;

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
	}
}
