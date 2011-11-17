using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class DialogNewMatrix : System.Windows.Forms.Form
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
private System.Windows.Forms.CheckedListBox lbMatrixColumns;
private System.Windows.Forms.Button bColumnUp;
private System.Windows.Forms.Button bColumnDown;
private System.Windows.Forms.Button bColumn;
private System.Windows.Forms.Button bRowSelect;
private System.Windows.Forms.CheckedListBox lbMatrixRows;
private System.Windows.Forms.Button bColumnDelete;
private System.Windows.Forms.Button bRowDelete;
private System.Windows.Forms.Button bRowDown;
private System.Windows.Forms.Button bRowUp;
private System.Windows.Forms.Label label4;
private System.Windows.Forms.Label label5;
private System.Windows.Forms.ComboBox cbMatrixCell;
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
			this.lbMatrixColumns = new System.Windows.Forms.CheckedListBox();
			this.bColumnUp = new System.Windows.Forms.Button();
			this.bColumnDown = new System.Windows.Forms.Button();
			this.bColumn = new System.Windows.Forms.Button();
			this.bRowSelect = new System.Windows.Forms.Button();
			this.lbMatrixRows = new System.Windows.Forms.CheckedListBox();
			this.bColumnDelete = new System.Windows.Forms.Button();
			this.bRowDelete = new System.Windows.Forms.Button();
			this.bRowDown = new System.Windows.Forms.Button();
			this.bRowUp = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cbMatrixCell = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.Location = new System.Drawing.Point(272, 368);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 13;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(368, 368);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 14;
			this.bCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 11;
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
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.TabIndex = 13;
			this.label2.Text = "DataSet Fields";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(224, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(184, 23);
			this.label3.TabIndex = 14;
			this.label3.Text = "Matrix Columns (check to subtotal)";
			// 
			// lbFields
			// 
			this.lbFields.Location = new System.Drawing.Point(16, 80);
			this.lbFields.Name = "lbFields";
			this.lbFields.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.lbFields.Size = new System.Drawing.Size(152, 225);
			this.lbFields.TabIndex = 1;
			// 
			// lbMatrixColumns
			// 
			this.lbMatrixColumns.Location = new System.Drawing.Point(232, 80);
			this.lbMatrixColumns.Name = "lbMatrixColumns";
			this.lbMatrixColumns.Size = new System.Drawing.Size(152, 94);
			this.lbMatrixColumns.TabIndex = 3;
			// 
			// bColumnUp
			// 
			this.bColumnUp.Location = new System.Drawing.Point(392, 80);
			this.bColumnUp.Name = "bColumnUp";
			this.bColumnUp.Size = new System.Drawing.Size(48, 24);
			this.bColumnUp.TabIndex = 4;
			this.bColumnUp.Text = "Up";
			this.bColumnUp.Click += new System.EventHandler(this.bColumnUp_Click);
			// 
			// bColumnDown
			// 
			this.bColumnDown.Location = new System.Drawing.Point(392, 112);
			this.bColumnDown.Name = "bColumnDown";
			this.bColumnDown.Size = new System.Drawing.Size(48, 24);
			this.bColumnDown.TabIndex = 5;
			this.bColumnDown.Text = "Down";
			this.bColumnDown.Click += new System.EventHandler(this.bColumnDown_Click);
			// 
			// bColumn
			// 
			this.bColumn.Location = new System.Drawing.Point(184, 88);
			this.bColumn.Name = "bColumn";
			this.bColumn.Size = new System.Drawing.Size(32, 24);
			this.bColumn.TabIndex = 2;
			this.bColumn.Text = ">";
			this.bColumn.Click += new System.EventHandler(this.bColumn_Click);
			// 
			// bRowSelect
			// 
			this.bRowSelect.Location = new System.Drawing.Point(184, 216);
			this.bRowSelect.Name = "bRowSelect";
			this.bRowSelect.Size = new System.Drawing.Size(32, 24);
			this.bRowSelect.TabIndex = 7;
			this.bRowSelect.Text = ">";
			this.bRowSelect.Click += new System.EventHandler(this.bRow_Click);
			// 
			// lbMatrixRows
			// 
			this.lbMatrixRows.Location = new System.Drawing.Point(232, 208);
			this.lbMatrixRows.Name = "lbMatrixRows";
			this.lbMatrixRows.Size = new System.Drawing.Size(152, 94);
			this.lbMatrixRows.TabIndex = 8;
			// 
			// bColumnDelete
			// 
			this.bColumnDelete.Location = new System.Drawing.Point(392, 144);
			this.bColumnDelete.Name = "bColumnDelete";
			this.bColumnDelete.Size = new System.Drawing.Size(48, 24);
			this.bColumnDelete.TabIndex = 6;
			this.bColumnDelete.Text = "Delete";
			this.bColumnDelete.Click += new System.EventHandler(this.bColumnDelete_Click);
			// 
			// bRowDelete
			// 
			this.bRowDelete.Location = new System.Drawing.Point(392, 272);
			this.bRowDelete.Name = "bRowDelete";
			this.bRowDelete.Size = new System.Drawing.Size(48, 24);
			this.bRowDelete.TabIndex = 11;
			this.bRowDelete.Text = "Delete";
			this.bRowDelete.Click += new System.EventHandler(this.bRowDelete_Click);
			// 
			// bRowDown
			// 
			this.bRowDown.Location = new System.Drawing.Point(392, 240);
			this.bRowDown.Name = "bRowDown";
			this.bRowDown.Size = new System.Drawing.Size(48, 24);
			this.bRowDown.TabIndex = 10;
			this.bRowDown.Text = "Down";
			this.bRowDown.Click += new System.EventHandler(this.bRowDown_Click);
			// 
			// bRowUp
			// 
			this.bRowUp.Location = new System.Drawing.Point(392, 208);
			this.bRowUp.Name = "bRowUp";
			this.bRowUp.Size = new System.Drawing.Size(48, 24);
			this.bRowUp.TabIndex = 9;
			this.bRowUp.Text = "Up";
			this.bRowUp.Click += new System.EventHandler(this.bRowUp_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(224, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(184, 23);
			this.label4.TabIndex = 31;
			this.label4.Text = "Matrix Rows (check to subtotal)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 320);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 23);
			this.label5.TabIndex = 32;
			this.label5.Text = "Matrix Cell Expression";
			// 
			// cbMatrixCell
			// 
			this.cbMatrixCell.Location = new System.Drawing.Point(16, 336);
			this.cbMatrixCell.Name = "cbMatrixCell";
			this.cbMatrixCell.Size = new System.Drawing.Size(368, 21);
			this.cbMatrixCell.TabIndex = 12;
			this.cbMatrixCell.TextChanged += new System.EventHandler(this.cbMatrixCell_TextChanged);
			this.cbMatrixCell.Enter += new System.EventHandler(this.cbMatrixCell_Enter);
			// 
			// DialogNewMatrix
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(456, 400);
			this.Controls.Add(this.cbMatrixCell);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.bRowDelete);
			this.Controls.Add(this.bRowDown);
			this.Controls.Add(this.bRowUp);
			this.Controls.Add(this.bColumnDelete);
			this.Controls.Add(this.lbMatrixRows);
			this.Controls.Add(this.bRowSelect);
			this.Controls.Add(this.bColumn);
			this.Controls.Add(this.bColumnDown);
			this.Controls.Add(this.bColumnUp);
			this.Controls.Add(this.lbMatrixColumns);
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
			this.Name = "DialogNewMatrix";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Matrix";
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
