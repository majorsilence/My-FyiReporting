using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class DialogNewTable : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private DesignXmlDraw _Draw;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.ComboBox cbDataSets;
private System.Windows.Forms.Label label2;
private System.Windows.Forms.Label label3;
private System.Windows.Forms.ListBox lbFields;
private System.Windows.Forms.CheckedListBox lbTableColumns;
private System.Windows.Forms.Button bUp;
private System.Windows.Forms.Button bDown;
private System.Windows.Forms.Button bRight;
private System.Windows.Forms.Button bAllRight;
private System.Windows.Forms.Button bLeft;
private System.Windows.Forms.Button bAllLeft;
private System.Windows.Forms.Label label4;
private System.Windows.Forms.ComboBox cbGroupColumn;
private System.Windows.Forms.CheckBox chkGrandTotals;
private System.Windows.Forms.GroupBox groupBox1;
private System.Windows.Forms.RadioButton rbHorz;
private System.Windows.Forms.RadioButton rbVert;
private System.Windows.Forms.RadioButton rbVertComp;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cbDataSets = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lbFields = new System.Windows.Forms.ListBox();
			this.lbTableColumns = new System.Windows.Forms.CheckedListBox();
			this.bUp = new System.Windows.Forms.Button();
			this.bDown = new System.Windows.Forms.Button();
			this.bRight = new System.Windows.Forms.Button();
			this.bAllRight = new System.Windows.Forms.Button();
			this.bLeft = new System.Windows.Forms.Button();
			this.bAllLeft = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.cbGroupColumn = new System.Windows.Forms.ComboBox();
			this.chkGrandTotals = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbVert = new System.Windows.Forms.RadioButton();
			this.rbHorz = new System.Windows.Forms.RadioButton();
			this.rbVertComp = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.Location = new System.Drawing.Point(272, 312);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 12;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(368, 312);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 13;
			this.bCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "DataSet";
			// 
			// cbDataSets
			// 
			this.cbDataSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataSets.Location = new System.Drawing.Point(80, 16);
			this.cbDataSets.Name = "cbDataSets";
			this.cbDataSets.Size = new System.Drawing.Size(360, 21);
			this.cbDataSets.TabIndex = 0;
			this.cbDataSets.SelectedIndexChanged += new System.EventHandler(this.cbDataSets_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 88);
			this.label2.Name = "label2";
			this.label2.TabIndex = 9;
			this.label2.Text = "DataSet Fields";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(232, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(240, 23);
			this.label3.TabIndex = 10;
			this.label3.Text = "Table Columns (check totals when not Down)";
			// 
			// lbFields
			// 
			this.lbFields.Location = new System.Drawing.Point(16, 112);
			this.lbFields.Name = "lbFields";
			this.lbFields.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbFields.Size = new System.Drawing.Size(152, 134);
			this.lbFields.TabIndex = 2;
			// 
			// lbTableColumns
			// 
			this.lbTableColumns.Location = new System.Drawing.Point(232, 112);
			this.lbTableColumns.Name = "lbTableColumns";
			this.lbTableColumns.Size = new System.Drawing.Size(152, 139);
			this.lbTableColumns.TabIndex = 7;
			// 
			// bUp
			// 
			this.bUp.Location = new System.Drawing.Point(392, 120);
			this.bUp.Name = "bUp";
			this.bUp.Size = new System.Drawing.Size(48, 24);
			this.bUp.TabIndex = 8;
			this.bUp.Text = "Up";
			this.bUp.Click += new System.EventHandler(this.bUp_Click);
			// 
			// bDown
			// 
			this.bDown.Location = new System.Drawing.Point(392, 152);
			this.bDown.Name = "bDown";
			this.bDown.Size = new System.Drawing.Size(48, 24);
			this.bDown.TabIndex = 9;
			this.bDown.Text = "Down";
			this.bDown.Click += new System.EventHandler(this.bDown_Click);
			// 
			// bRight
			// 
			this.bRight.Location = new System.Drawing.Point(184, 120);
			this.bRight.Name = "bRight";
			this.bRight.Size = new System.Drawing.Size(32, 24);
			this.bRight.TabIndex = 3;
			this.bRight.Text = ">";
			this.bRight.Click += new System.EventHandler(this.bRight_Click);
			// 
			// bAllRight
			// 
			this.bAllRight.Location = new System.Drawing.Point(184, 152);
			this.bAllRight.Name = "bAllRight";
			this.bAllRight.Size = new System.Drawing.Size(32, 24);
			this.bAllRight.TabIndex = 4;
			this.bAllRight.Text = ">>";
			this.bAllRight.Click += new System.EventHandler(this.bAllRight_Click);
			// 
			// bLeft
			// 
			this.bLeft.Location = new System.Drawing.Point(184, 184);
			this.bLeft.Name = "bLeft";
			this.bLeft.Size = new System.Drawing.Size(32, 24);
			this.bLeft.TabIndex = 5;
			this.bLeft.Text = "<";
			this.bLeft.Click += new System.EventHandler(this.bLeft_Click);
			// 
			// bAllLeft
			// 
			this.bAllLeft.Location = new System.Drawing.Point(184, 216);
			this.bAllLeft.Name = "bAllLeft";
			this.bAllLeft.Size = new System.Drawing.Size(32, 24);
			this.bAllLeft.TabIndex = 6;
			this.bAllLeft.Text = "<<";
			this.bAllLeft.Click += new System.EventHandler(this.bAllLeft_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 264);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(216, 23);
			this.label4.TabIndex = 1;
			this.label4.Text = "Pick a column to group (create hierarchy)";
			// 
			// cbGroupColumn
			// 
			this.cbGroupColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbGroupColumn.Location = new System.Drawing.Point(16, 280);
			this.cbGroupColumn.Name = "cbGroupColumn";
			this.cbGroupColumn.Size = new System.Drawing.Size(168, 21);
			this.cbGroupColumn.TabIndex = 10;
			this.cbGroupColumn.Enter += new System.EventHandler(this.cbGroupColumn_Enter);
			// 
			// chkGrandTotals
			// 
			this.chkGrandTotals.Location = new System.Drawing.Point(232, 280);
			this.chkGrandTotals.Name = "chkGrandTotals";
			this.chkGrandTotals.Size = new System.Drawing.Size(168, 16);
			this.chkGrandTotals.TabIndex = 11;
			this.chkGrandTotals.Text = "Calculate Grand Totals";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbVertComp);
			this.groupBox1.Controls.Add(this.rbVert);
			this.groupBox1.Controls.Add(this.rbHorz);
			this.groupBox1.Location = new System.Drawing.Point(16, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(424, 40);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Arrange Fields";
			// 
			// rbVert
			// 
			this.rbVert.Location = new System.Drawing.Point(160, 16);
			this.rbVert.Name = "rbVert";
			this.rbVert.Size = new System.Drawing.Size(120, 16);
			this.rbVert.TabIndex = 1;
			this.rbVert.Text = "Down (row per field)";
			// 
			// rbHorz
			// 
			this.rbHorz.Location = new System.Drawing.Point(8, 16);
			this.rbHorz.Name = "rbHorz";
			this.rbHorz.Size = new System.Drawing.Size(160, 16);
			this.rbHorz.TabIndex = 0;
			this.rbHorz.Text = "Across (standard columns)";
			this.rbHorz.CheckedChanged += new System.EventHandler(this.rbHorz_CheckedChanged);
			// 
			// rbVertComp
			// 
			this.rbVertComp.Location = new System.Drawing.Point(296, 16);
			this.rbVertComp.Name = "rbVertComp";
			this.rbVertComp.Size = new System.Drawing.Size(112, 16);
			this.rbVertComp.TabIndex = 2;
			this.rbVertComp.Text = "Down (compress)";
			// 
			// DialogNewTable
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(456, 336);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chkGrandTotals);
			this.Controls.Add(this.cbGroupColumn);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.bAllLeft);
			this.Controls.Add(this.bLeft);
			this.Controls.Add(this.bAllRight);
			this.Controls.Add(this.bRight);
			this.Controls.Add(this.bDown);
			this.Controls.Add(this.bUp);
			this.Controls.Add(this.lbTableColumns);
			this.Controls.Add(this.lbFields);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbDataSets);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogNewTable";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Table";
			this.groupBox1.ResumeLayout(false);
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
