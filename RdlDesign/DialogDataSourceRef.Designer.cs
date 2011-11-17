using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    public partial class DialogDataSourceRef : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.Label label1;
private System.Windows.Forms.TextBox tbPassword;
private System.Windows.Forms.Label label2;
private System.Windows.Forms.TextBox tbFilename;
private System.Windows.Forms.Button bGetFilename;
private System.Windows.Forms.Label label3;
private System.Windows.Forms.ComboBox cbDataProvider;
private System.Windows.Forms.Label label4;
private System.Windows.Forms.TextBox tbConnection;
private System.Windows.Forms.CheckBox ckbIntSecurity;
private System.Windows.Forms.Label label5;
private System.Windows.Forms.TextBox tbPrompt;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.TextBox tbPassword2;
private System.Windows.Forms.Label label6;
private System.Windows.Forms.Button bTestConnection;
private System.Windows.Forms.ComboBox cbOdbcNames;
private System.Windows.Forms.Label lODBC;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbFilename = new System.Windows.Forms.TextBox();
			this.bGetFilename = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cbDataProvider = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbConnection = new System.Windows.Forms.TextBox();
			this.ckbIntSecurity = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbPrompt = new System.Windows.Forms.TextBox();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.tbPassword2 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.bTestConnection = new System.Windows.Forms.Button();
			this.cbOdbcNames = new System.Windows.Forms.ComboBox();
			this.lODBC = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(440, 32);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter (twice) a pass phrase used to encrypt the data source reference file.   You" +
				" should use the same password you\'ve used to create previous files, as only one " +
				"can be used.";
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(16, 56);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(184, 20);
			this.tbPassword.TabIndex = 0;
			this.tbPassword.Text = "";
			this.tbPassword.TextChanged += new System.EventHandler(this.validate_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(408, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Enter the file name that will hold the encrypted data source information";
			// 
			// tbFilename
			// 
			this.tbFilename.Location = new System.Drawing.Point(16, 112);
			this.tbFilename.Name = "tbFilename";
			this.tbFilename.Size = new System.Drawing.Size(392, 20);
			this.tbFilename.TabIndex = 2;
			this.tbFilename.Text = "";
			this.tbFilename.TextChanged += new System.EventHandler(this.validate_TextChanged);
			// 
			// bGetFilename
			// 
			this.bGetFilename.Location = new System.Drawing.Point(424, 112);
			this.bGetFilename.Name = "bGetFilename";
			this.bGetFilename.Size = new System.Drawing.Size(24, 23);
			this.bGetFilename.TabIndex = 3;
			this.bGetFilename.Text = "...";
			this.bGetFilename.Click += new System.EventHandler(this.bGetFilename_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 152);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 23);
			this.label3.TabIndex = 7;
			this.label3.Text = "Data provider";
			// 
			// cbDataProvider
			// 
			this.cbDataProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataProvider.Location = new System.Drawing.Point(80, 152);
			this.cbDataProvider.Name = "cbDataProvider";
			this.cbDataProvider.Size = new System.Drawing.Size(104, 21);
			this.cbDataProvider.TabIndex = 4;
			this.cbDataProvider.SelectedIndexChanged += new System.EventHandler(this.cbDataProvider_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 192);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Connection string";
			// 
			// tbConnection
			// 
			this.tbConnection.Location = new System.Drawing.Point(16, 216);
			this.tbConnection.Multiline = true;
			this.tbConnection.Name = "tbConnection";
			this.tbConnection.Size = new System.Drawing.Size(424, 40);
			this.tbConnection.TabIndex = 7;
			this.tbConnection.Text = "";
			this.tbConnection.TextChanged += new System.EventHandler(this.validate_TextChanged);
			// 
			// ckbIntSecurity
			// 
			this.ckbIntSecurity.Location = new System.Drawing.Point(296, 184);
			this.ckbIntSecurity.Name = "ckbIntSecurity";
			this.ckbIntSecurity.Size = new System.Drawing.Size(144, 24);
			this.ckbIntSecurity.TabIndex = 6;
			this.ckbIntSecurity.Text = "Use integrated security";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 272);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(432, 16);
			this.label5.TabIndex = 12;
			this.label5.Text = "(Optional) Enter the prompt displayed when asking for database credentials ";
			// 
			// tbPrompt
			// 
			this.tbPrompt.Location = new System.Drawing.Point(16, 296);
			this.tbPrompt.Name = "tbPrompt";
			this.tbPrompt.Size = new System.Drawing.Size(424, 20);
			this.tbPrompt.TabIndex = 8;
			this.tbPrompt.Text = "";
			// 
			// bOK
			// 
			this.bOK.Location = new System.Drawing.Point(272, 344);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 10;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(368, 344);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 11;
			this.bCancel.Text = "Cancel";
			// 
			// tbPassword2
			// 
			this.tbPassword2.Location = new System.Drawing.Point(256, 56);
			this.tbPassword2.Name = "tbPassword2";
			this.tbPassword2.PasswordChar = '*';
			this.tbPassword2.Size = new System.Drawing.Size(184, 20);
			this.tbPassword2.TabIndex = 1;
			this.tbPassword2.Text = "";
			this.tbPassword2.TextChanged += new System.EventHandler(this.validate_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(208, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 23);
			this.label6.TabIndex = 2;
			this.label6.Text = "Repeat";
			// 
			// bTestConnection
			// 
			this.bTestConnection.Location = new System.Drawing.Point(16, 344);
			this.bTestConnection.Name = "bTestConnection";
			this.bTestConnection.Size = new System.Drawing.Size(96, 23);
			this.bTestConnection.TabIndex = 9;
			this.bTestConnection.Text = "Test Connection";
			this.bTestConnection.Click += new System.EventHandler(this.bTestConnection_Click);
			// 
			// cbOdbcNames
			// 
			this.cbOdbcNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOdbcNames.Location = new System.Drawing.Point(296, 152);
			this.cbOdbcNames.Name = "cbOdbcNames";
			this.cbOdbcNames.Size = new System.Drawing.Size(152, 21);
			this.cbOdbcNames.Sorted = true;
			this.cbOdbcNames.TabIndex = 5;
			this.cbOdbcNames.SelectedIndexChanged += new System.EventHandler(this.cbOdbcNames_SelectedIndexChanged);
			// 
			// lODBC
			// 
			this.lODBC.Location = new System.Drawing.Point(184, 152);
			this.lODBC.Name = "lODBC";
			this.lODBC.Size = new System.Drawing.Size(112, 23);
			this.lODBC.TabIndex = 17;
			this.lODBC.Text = "ODBC Data Sources";
			// 
			// DialogDataSourceRef
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(456, 374);
			this.Controls.Add(this.cbOdbcNames);
			this.Controls.Add(this.lODBC);
			this.Controls.Add(this.bTestConnection);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbPassword2);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.tbPrompt);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.ckbIntSecurity);
			this.Controls.Add(this.tbConnection);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbDataProvider);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.bGetFilename);
			this.Controls.Add(this.tbFilename);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogDataSourceRef";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create Data Source Reference File";
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
