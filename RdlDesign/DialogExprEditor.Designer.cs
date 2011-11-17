using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    public partial class DialogExprEditor : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DesignXmlDraw _Draw;
private SplitContainer splitContainer1;
private Button bCopy;
private Label lOp;
private TextBox tbExpr;
private Label lExpr;
private TreeView tvOp;
private Panel panel1;
private Button bCancel;
private Button bOK;
private System.ComponentModel.Container components = null;

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
