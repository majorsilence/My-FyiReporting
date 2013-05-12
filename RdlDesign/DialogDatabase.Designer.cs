using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    public partial class DialogDatabase : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		
private System.Windows.Forms.Button btnCancel;
private System.Windows.Forms.Panel panel1;
private System.Windows.Forms.Button btnOK;
private System.Windows.Forms.TabPage DBConnection;
private System.Windows.Forms.TabPage DBSql;
private System.Windows.Forms.TabPage ReportType;
private System.ComponentModel.Container components = null;
private System.Windows.Forms.GroupBox groupBox1;
private System.Windows.Forms.RadioButton rbTable;
private System.Windows.Forms.RadioButton rbList;
private System.Windows.Forms.RadioButton rbMatrix;
private System.Windows.Forms.RadioButton rbChart;
private System.Windows.Forms.TextBox tbConnection;
private System.Windows.Forms.TabPage ReportSyntax;
private System.Windows.Forms.TextBox tbReportSyntax;
private System.Windows.Forms.TabPage ReportPreview;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.Label label2;
private System.Windows.Forms.Label label3;
private System.Windows.Forms.TextBox tbReportName;
private System.Windows.Forms.TextBox tbReportDescription;
private System.Windows.Forms.TextBox tbReportAuthor;
private System.Windows.Forms.Panel panel2;
private fyiReporting.RdlViewer.RdlViewer rdlViewer1;
private System.Windows.Forms.TabPage ReportParameters;
private System.Windows.Forms.ListBox lbParameters;
private System.Windows.Forms.Button bAdd;
private System.Windows.Forms.Button bRemove;
private System.Windows.Forms.Label lParmName;
private System.Windows.Forms.TextBox tbParmName;
private System.Windows.Forms.Label lParmType;
private System.Windows.Forms.ComboBox cbParmType;
private System.Windows.Forms.Label lParmPrompt;
private System.Windows.Forms.TextBox tbParmPrompt;
private System.Windows.Forms.CheckBox ckbParmAllowBlank;
private System.Windows.Forms.Label lbParmValidValues;
private System.Windows.Forms.TextBox tbParmValidValues;
private System.Windows.Forms.Button bParmUp;
private System.Windows.Forms.Button bParmDown;
private System.Windows.Forms.CheckBox ckbParmAllowNull;
private System.Windows.Forms.TabControl tcDialog;
private System.Windows.Forms.Label lDefaultValue;
private System.Windows.Forms.TextBox tbParmDefaultValue;
private System.Windows.Forms.TabPage TabularGroup;
private System.Windows.Forms.ComboBox cbColumnList;
private System.Windows.Forms.Label label4;
private System.Windows.Forms.Label label5;
private System.Windows.Forms.CheckBox ckbGrandTotal;
private System.Windows.Forms.CheckedListBox clbSubtotal;
private System.Windows.Forms.Label label6;
private System.Windows.Forms.ComboBox cbOrientation;
private System.Windows.Forms.Button bValidValues;
private System.Windows.Forms.Label label7;
private System.Windows.Forms.ComboBox cbConnectionTypes;
private System.Windows.Forms.Label lODBC;
private System.Windows.Forms.ComboBox cbOdbcNames;
private System.Windows.Forms.Button bTestConnection;
private System.Windows.Forms.Label lConnection;
private System.Windows.Forms.Button bShared;
private System.Windows.Forms.GroupBox groupBox2;
private System.Windows.Forms.RadioButton rbSchemaNo;
private System.Windows.Forms.RadioButton rbSchema2003;
private System.Windows.Forms.RadioButton rbSchema2005;
private SplitContainer splitContainer1;
private TreeView tvTablesColumns;
private Button bMove;
private TextBox tbSQL;

		private void InitializeComponent()
		{
            this.tcDialog = new System.Windows.Forms.TabControl();
            this.ReportType = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbSchema2005 = new System.Windows.Forms.RadioButton();
            this.rbSchema2003 = new System.Windows.Forms.RadioButton();
            this.rbSchemaNo = new System.Windows.Forms.RadioButton();
            this.cbOrientation = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbReportAuthor = new System.Windows.Forms.TextBox();
            this.tbReportDescription = new System.Windows.Forms.TextBox();
            this.tbReportName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbChart = new System.Windows.Forms.RadioButton();
            this.rbMatrix = new System.Windows.Forms.RadioButton();
            this.rbList = new System.Windows.Forms.RadioButton();
            this.rbTable = new System.Windows.Forms.RadioButton();
            this.DBConnection = new System.Windows.Forms.TabPage();
            this.groupBoxSqlServer = new System.Windows.Forms.GroupBox();
            this.textBoxSqlPassword = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxSqlUser = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonDatabaseSearch = new System.Windows.Forms.Button();
            this.comboServerList = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonSearchSqlServers = new System.Windows.Forms.Button();
            this.comboDatabaseList = new System.Windows.Forms.ComboBox();
            this.buttonSqliteSelectDatabase = new System.Windows.Forms.Button();
            this.bShared = new System.Windows.Forms.Button();
            this.bTestConnection = new System.Windows.Forms.Button();
            this.cbOdbcNames = new System.Windows.Forms.ComboBox();
            this.lODBC = new System.Windows.Forms.Label();
            this.lConnection = new System.Windows.Forms.Label();
            this.cbConnectionTypes = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbConnection = new System.Windows.Forms.TextBox();
            this.ReportParameters = new System.Windows.Forms.TabPage();
            this.bValidValues = new System.Windows.Forms.Button();
            this.tbParmDefaultValue = new System.Windows.Forms.TextBox();
            this.lDefaultValue = new System.Windows.Forms.Label();
            this.bParmDown = new System.Windows.Forms.Button();
            this.bParmUp = new System.Windows.Forms.Button();
            this.tbParmValidValues = new System.Windows.Forms.TextBox();
            this.lbParmValidValues = new System.Windows.Forms.Label();
            this.ckbParmAllowBlank = new System.Windows.Forms.CheckBox();
            this.ckbParmAllowNull = new System.Windows.Forms.CheckBox();
            this.tbParmPrompt = new System.Windows.Forms.TextBox();
            this.lParmPrompt = new System.Windows.Forms.Label();
            this.cbParmType = new System.Windows.Forms.ComboBox();
            this.lParmType = new System.Windows.Forms.Label();
            this.tbParmName = new System.Windows.Forms.TextBox();
            this.lParmName = new System.Windows.Forms.Label();
            this.bRemove = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.lbParameters = new System.Windows.Forms.ListBox();
            this.DBSql = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvTablesColumns = new System.Windows.Forms.TreeView();
            this.tbSQL = new System.Windows.Forms.TextBox();
            this.bMove = new System.Windows.Forms.Button();
            this.TabularGroup = new System.Windows.Forms.TabPage();
            this.clbSubtotal = new System.Windows.Forms.CheckedListBox();
            this.ckbGrandTotal = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbColumnList = new System.Windows.Forms.ComboBox();
            this.ReportSyntax = new System.Windows.Forms.TabPage();
            this.tbReportSyntax = new System.Windows.Forms.TextBox();
            this.ReportPreview = new System.Windows.Forms.TabPage();
            this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.tcDialog.SuspendLayout();
            this.ReportType.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.DBConnection.SuspendLayout();
            this.groupBoxSqlServer.SuspendLayout();
            this.ReportParameters.SuspendLayout();
            this.DBSql.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.TabularGroup.SuspendLayout();
            this.ReportSyntax.SuspendLayout();
            this.ReportPreview.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDialog
            // 
            this.tcDialog.Controls.Add(this.ReportType);
            this.tcDialog.Controls.Add(this.DBConnection);
            this.tcDialog.Controls.Add(this.ReportParameters);
            this.tcDialog.Controls.Add(this.DBSql);
            this.tcDialog.Controls.Add(this.TabularGroup);
            this.tcDialog.Controls.Add(this.ReportSyntax);
            this.tcDialog.Controls.Add(this.ReportPreview);
            this.tcDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDialog.Location = new System.Drawing.Point(0, 0);
            this.tcDialog.Name = "tcDialog";
            this.tcDialog.SelectedIndex = 0;
            this.tcDialog.Size = new System.Drawing.Size(528, 326);
            this.tcDialog.TabIndex = 0;
            this.tcDialog.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // ReportType
            // 
            this.ReportType.Controls.Add(this.groupBox2);
            this.ReportType.Controls.Add(this.cbOrientation);
            this.ReportType.Controls.Add(this.label6);
            this.ReportType.Controls.Add(this.tbReportAuthor);
            this.ReportType.Controls.Add(this.tbReportDescription);
            this.ReportType.Controls.Add(this.tbReportName);
            this.ReportType.Controls.Add(this.label3);
            this.ReportType.Controls.Add(this.label2);
            this.ReportType.Controls.Add(this.label1);
            this.ReportType.Controls.Add(this.groupBox1);
            this.ReportType.Location = new System.Drawing.Point(4, 22);
            this.ReportType.Name = "ReportType";
            this.ReportType.Size = new System.Drawing.Size(520, 300);
            this.ReportType.TabIndex = 3;
            this.ReportType.Tag = "type";
            this.ReportType.Text = "Report Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbSchema2005);
            this.groupBox2.Controls.Add(this.rbSchema2003);
            this.groupBox2.Controls.Add(this.rbSchemaNo);
            this.groupBox2.Location = new System.Drawing.Point(16, 256);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 40);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RDL Schema";
            // 
            // rbSchema2005
            // 
            this.rbSchema2005.Checked = true;
            this.rbSchema2005.Location = new System.Drawing.Point(248, 16);
            this.rbSchema2005.Name = "rbSchema2005";
            this.rbSchema2005.Size = new System.Drawing.Size(104, 16);
            this.rbSchema2005.TabIndex = 2;
            this.rbSchema2005.TabStop = true;
            this.rbSchema2005.Text = "2005";
            // 
            // rbSchema2003
            // 
            this.rbSchema2003.Location = new System.Drawing.Point(120, 16);
            this.rbSchema2003.Name = "rbSchema2003";
            this.rbSchema2003.Size = new System.Drawing.Size(104, 16);
            this.rbSchema2003.TabIndex = 1;
            this.rbSchema2003.Text = "2003";
            // 
            // rbSchemaNo
            // 
            this.rbSchemaNo.Location = new System.Drawing.Point(8, 16);
            this.rbSchemaNo.Name = "rbSchemaNo";
            this.rbSchemaNo.Size = new System.Drawing.Size(104, 16);
            this.rbSchemaNo.TabIndex = 0;
            this.rbSchemaNo.Text = "None";
            // 
            // cbOrientation
            // 
            this.cbOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrientation.Items.AddRange(new object[] {
            "Portrait (8.5\" by 11\")",
            "Landscape (11\" by 8.5\")"});
            this.cbOrientation.Location = new System.Drawing.Point(96, 224);
            this.cbOrientation.Name = "cbOrientation";
            this.cbOrientation.Size = new System.Drawing.Size(168, 21);
            this.cbOrientation.TabIndex = 8;
            this.cbOrientation.SelectedIndexChanged += new System.EventHandler(this.emptyReportSyntax);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "Orientation:";
            // 
            // tbReportAuthor
            // 
            this.tbReportAuthor.Location = new System.Drawing.Point(96, 192);
            this.tbReportAuthor.Name = "tbReportAuthor";
            this.tbReportAuthor.Size = new System.Drawing.Size(304, 20);
            this.tbReportAuthor.TabIndex = 6;
            this.tbReportAuthor.TextChanged += new System.EventHandler(this.tbReportAuthor_TextChanged);
            // 
            // tbReportDescription
            // 
            this.tbReportDescription.Location = new System.Drawing.Point(96, 160);
            this.tbReportDescription.Name = "tbReportDescription";
            this.tbReportDescription.Size = new System.Drawing.Size(304, 20);
            this.tbReportDescription.TabIndex = 5;
            this.tbReportDescription.TextChanged += new System.EventHandler(this.tbReportDescription_TextChanged);
            // 
            // tbReportName
            // 
            this.tbReportName.Location = new System.Drawing.Point(96, 128);
            this.tbReportName.Name = "tbReportName";
            this.tbReportName.Size = new System.Drawing.Size(304, 20);
            this.tbReportName.TabIndex = 4;
            this.tbReportName.TextChanged += new System.EventHandler(this.tbReportName_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Author:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbChart);
            this.groupBox1.Controls.Add(this.rbMatrix);
            this.groupBox1.Controls.Add(this.rbList);
            this.groupBox1.Controls.Add(this.rbTable);
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 104);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Report Type";
            // 
            // rbChart
            // 
            this.rbChart.Location = new System.Drawing.Point(168, 56);
            this.rbChart.Name = "rbChart";
            this.rbChart.Size = new System.Drawing.Size(104, 24);
            this.rbChart.TabIndex = 3;
            this.rbChart.Text = "Chart";
            this.rbChart.Visible = false;
            this.rbChart.CheckedChanged += new System.EventHandler(this.rbChart_CheckedChanged);
            // 
            // rbMatrix
            // 
            this.rbMatrix.Location = new System.Drawing.Point(168, 24);
            this.rbMatrix.Name = "rbMatrix";
            this.rbMatrix.Size = new System.Drawing.Size(104, 24);
            this.rbMatrix.TabIndex = 2;
            this.rbMatrix.Text = "Matrix";
            this.rbMatrix.Visible = false;
            this.rbMatrix.CheckedChanged += new System.EventHandler(this.rbMatrix_CheckedChanged);
            // 
            // rbList
            // 
            this.rbList.Location = new System.Drawing.Point(32, 56);
            this.rbList.Name = "rbList";
            this.rbList.Size = new System.Drawing.Size(104, 24);
            this.rbList.TabIndex = 1;
            this.rbList.Text = "List";
            this.rbList.CheckedChanged += new System.EventHandler(this.rbList_CheckedChanged);
            // 
            // rbTable
            // 
            this.rbTable.Checked = true;
            this.rbTable.Location = new System.Drawing.Point(32, 24);
            this.rbTable.Name = "rbTable";
            this.rbTable.Size = new System.Drawing.Size(104, 24);
            this.rbTable.TabIndex = 0;
            this.rbTable.TabStop = true;
            this.rbTable.Text = "Table";
            this.rbTable.CheckedChanged += new System.EventHandler(this.rbTable_CheckedChanged);
            // 
            // DBConnection
            // 
            this.DBConnection.CausesValidation = false;
            this.DBConnection.Controls.Add(this.groupBoxSqlServer);
            this.DBConnection.Controls.Add(this.buttonSqliteSelectDatabase);
            this.DBConnection.Controls.Add(this.bShared);
            this.DBConnection.Controls.Add(this.bTestConnection);
            this.DBConnection.Controls.Add(this.cbOdbcNames);
            this.DBConnection.Controls.Add(this.lODBC);
            this.DBConnection.Controls.Add(this.lConnection);
            this.DBConnection.Controls.Add(this.cbConnectionTypes);
            this.DBConnection.Controls.Add(this.label7);
            this.DBConnection.Controls.Add(this.tbConnection);
            this.DBConnection.Location = new System.Drawing.Point(4, 22);
            this.DBConnection.Name = "DBConnection";
            this.DBConnection.Size = new System.Drawing.Size(520, 300);
            this.DBConnection.TabIndex = 0;
            this.DBConnection.Tag = "connect";
            this.DBConnection.Text = "Connection";
            this.DBConnection.Validating += new System.ComponentModel.CancelEventHandler(this.DBConnection_Validating);
            // 
            // groupBoxSqlServer
            // 
            this.groupBoxSqlServer.Controls.Add(this.textBoxSqlPassword);
            this.groupBoxSqlServer.Controls.Add(this.label11);
            this.groupBoxSqlServer.Controls.Add(this.textBoxSqlUser);
            this.groupBoxSqlServer.Controls.Add(this.label10);
            this.groupBoxSqlServer.Controls.Add(this.label8);
            this.groupBoxSqlServer.Controls.Add(this.buttonDatabaseSearch);
            this.groupBoxSqlServer.Controls.Add(this.comboServerList);
            this.groupBoxSqlServer.Controls.Add(this.label9);
            this.groupBoxSqlServer.Controls.Add(this.buttonSearchSqlServers);
            this.groupBoxSqlServer.Controls.Add(this.comboDatabaseList);
            this.groupBoxSqlServer.Location = new System.Drawing.Point(3, 147);
            this.groupBoxSqlServer.Name = "groupBoxSqlServer";
            this.groupBoxSqlServer.Size = new System.Drawing.Size(514, 140);
            this.groupBoxSqlServer.TabIndex = 14;
            this.groupBoxSqlServer.TabStop = false;
            this.groupBoxSqlServer.Text = "Sql Server";
            this.groupBoxSqlServer.Visible = false;
            // 
            // textBoxSqlPassword
            // 
            this.textBoxSqlPassword.Location = new System.Drawing.Point(306, 64);
            this.textBoxSqlPassword.Name = "textBoxSqlPassword";
            this.textBoxSqlPassword.PasswordChar = '*';
            this.textBoxSqlPassword.Size = new System.Drawing.Size(195, 20);
            this.textBoxSqlPassword.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(243, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 23);
            this.label11.TabIndex = 17;
            this.label11.Text = "password:";
            // 
            // textBoxSqlUser
            // 
            this.textBoxSqlUser.Location = new System.Drawing.Point(76, 63);
            this.textBoxSqlUser.Name = "textBoxSqlUser";
            this.textBoxSqlUser.Size = new System.Drawing.Size(161, 20);
            this.textBoxSqlUser.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(13, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 23);
            this.label10.TabIndex = 15;
            this.label10.Text = "Username:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Server:";
            // 
            // buttonDatabaseSearch
            // 
            this.buttonDatabaseSearch.Location = new System.Drawing.Point(372, 90);
            this.buttonDatabaseSearch.Name = "buttonDatabaseSearch";
            this.buttonDatabaseSearch.Size = new System.Drawing.Size(137, 23);
            this.buttonDatabaseSearch.TabIndex = 11;
            this.buttonDatabaseSearch.Text = "Search for Databases";
            this.buttonDatabaseSearch.UseVisualStyleBackColor = true;
            this.buttonDatabaseSearch.Click += new System.EventHandler(this.buttonDatabaseSearch_Click);
            // 
            // comboServerList
            // 
            this.comboServerList.FormattingEnabled = true;
            this.comboServerList.Location = new System.Drawing.Point(75, 21);
            this.comboServerList.Name = "comboServerList";
            this.comboServerList.Size = new System.Drawing.Size(291, 21);
            this.comboServerList.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Database:";
            // 
            // buttonSearchSqlServers
            // 
            this.buttonSearchSqlServers.Location = new System.Drawing.Point(372, 19);
            this.buttonSearchSqlServers.Name = "buttonSearchSqlServers";
            this.buttonSearchSqlServers.Size = new System.Drawing.Size(137, 23);
            this.buttonSearchSqlServers.TabIndex = 10;
            this.buttonSearchSqlServers.Text = "Search for Servers";
            this.buttonSearchSqlServers.UseVisualStyleBackColor = true;
            this.buttonSearchSqlServers.Click += new System.EventHandler(this.buttonSearchSqlServers_Click);
            // 
            // comboDatabaseList
            // 
            this.comboDatabaseList.FormattingEnabled = true;
            this.comboDatabaseList.Location = new System.Drawing.Point(76, 92);
            this.comboDatabaseList.Name = "comboDatabaseList";
            this.comboDatabaseList.Size = new System.Drawing.Size(290, 21);
            this.comboDatabaseList.TabIndex = 13;
            // 
            // buttonSqliteSelectDatabase
            // 
            this.buttonSqliteSelectDatabase.Location = new System.Drawing.Point(321, 104);
            this.buttonSqliteSelectDatabase.Name = "buttonSqliteSelectDatabase";
            this.buttonSqliteSelectDatabase.Size = new System.Drawing.Size(177, 23);
            this.buttonSqliteSelectDatabase.TabIndex = 7;
            this.buttonSqliteSelectDatabase.Text = "Select SQLite Database File";
            this.buttonSqliteSelectDatabase.UseVisualStyleBackColor = true;
            this.buttonSqliteSelectDatabase.Visible = false;
            this.buttonSqliteSelectDatabase.Click += new System.EventHandler(this.buttonSqliteSelectDatabase_Click);
            // 
            // bShared
            // 
            this.bShared.Location = new System.Drawing.Point(216, 48);
            this.bShared.Name = "bShared";
            this.bShared.Size = new System.Drawing.Size(24, 16);
            this.bShared.TabIndex = 6;
            this.bShared.Text = "...";
            this.bShared.Click += new System.EventHandler(this.bShared_Click);
            // 
            // bTestConnection
            // 
            this.bTestConnection.Location = new System.Drawing.Point(16, 104);
            this.bTestConnection.Name = "bTestConnection";
            this.bTestConnection.Size = new System.Drawing.Size(104, 23);
            this.bTestConnection.TabIndex = 3;
            this.bTestConnection.Text = "Test Connection";
            this.bTestConnection.Click += new System.EventHandler(this.bTestConnection_Click);
            // 
            // cbOdbcNames
            // 
            this.cbOdbcNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOdbcNames.Location = new System.Drawing.Point(352, 16);
            this.cbOdbcNames.Name = "cbOdbcNames";
            this.cbOdbcNames.Size = new System.Drawing.Size(152, 21);
            this.cbOdbcNames.Sorted = true;
            this.cbOdbcNames.TabIndex = 1;
            this.cbOdbcNames.SelectedIndexChanged += new System.EventHandler(this.cbOdbcNames_SelectedIndexChanged);
            // 
            // lODBC
            // 
            this.lODBC.Location = new System.Drawing.Point(240, 16);
            this.lODBC.Name = "lODBC";
            this.lODBC.Size = new System.Drawing.Size(112, 23);
            this.lODBC.TabIndex = 5;
            this.lODBC.Text = "ODBC Data Sources";
            // 
            // lConnection
            // 
            this.lConnection.Location = new System.Drawing.Point(16, 48);
            this.lConnection.Name = "lConnection";
            this.lConnection.Size = new System.Drawing.Size(184, 16);
            this.lConnection.TabIndex = 4;
            this.lConnection.Text = "Connection:";
            // 
            // cbConnectionTypes
            // 
            this.cbConnectionTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnectionTypes.Location = new System.Drawing.Point(112, 16);
            this.cbConnectionTypes.Name = "cbConnectionTypes";
            this.cbConnectionTypes.Size = new System.Drawing.Size(128, 21);
            this.cbConnectionTypes.TabIndex = 0;
            this.cbConnectionTypes.SelectedIndexChanged += new System.EventHandler(this.cbConnectionTypes_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 23);
            this.label7.TabIndex = 2;
            this.label7.Text = "Connection Type:";
            // 
            // tbConnection
            // 
            this.tbConnection.Location = new System.Drawing.Point(16, 72);
            this.tbConnection.Name = "tbConnection";
            this.tbConnection.Size = new System.Drawing.Size(488, 20);
            this.tbConnection.TabIndex = 2;
            this.tbConnection.Text = "Server=(local)\\VSDotNet;DataBase=Northwind;Integrated Security=SSPI;Connect Timeo" +
    "ut=5";
            this.tbConnection.TextChanged += new System.EventHandler(this.tbConnection_TextChanged);
            // 
            // ReportParameters
            // 
            this.ReportParameters.Controls.Add(this.bValidValues);
            this.ReportParameters.Controls.Add(this.tbParmDefaultValue);
            this.ReportParameters.Controls.Add(this.lDefaultValue);
            this.ReportParameters.Controls.Add(this.bParmDown);
            this.ReportParameters.Controls.Add(this.bParmUp);
            this.ReportParameters.Controls.Add(this.tbParmValidValues);
            this.ReportParameters.Controls.Add(this.lbParmValidValues);
            this.ReportParameters.Controls.Add(this.ckbParmAllowBlank);
            this.ReportParameters.Controls.Add(this.ckbParmAllowNull);
            this.ReportParameters.Controls.Add(this.tbParmPrompt);
            this.ReportParameters.Controls.Add(this.lParmPrompt);
            this.ReportParameters.Controls.Add(this.cbParmType);
            this.ReportParameters.Controls.Add(this.lParmType);
            this.ReportParameters.Controls.Add(this.tbParmName);
            this.ReportParameters.Controls.Add(this.lParmName);
            this.ReportParameters.Controls.Add(this.bRemove);
            this.ReportParameters.Controls.Add(this.bAdd);
            this.ReportParameters.Controls.Add(this.lbParameters);
            this.ReportParameters.Location = new System.Drawing.Point(4, 22);
            this.ReportParameters.Name = "ReportParameters";
            this.ReportParameters.Size = new System.Drawing.Size(520, 300);
            this.ReportParameters.TabIndex = 6;
            this.ReportParameters.Tag = "parameters";
            this.ReportParameters.Text = "Parameters";
            // 
            // bValidValues
            // 
            this.bValidValues.Location = new System.Drawing.Point(432, 152);
            this.bValidValues.Name = "bValidValues";
            this.bValidValues.Size = new System.Drawing.Size(24, 23);
            this.bValidValues.TabIndex = 16;
            this.bValidValues.Text = "...";
            this.bValidValues.Click += new System.EventHandler(this.bValidValues_Click);
            // 
            // tbParmDefaultValue
            // 
            this.tbParmDefaultValue.Location = new System.Drawing.Point(208, 200);
            this.tbParmDefaultValue.Name = "tbParmDefaultValue";
            this.tbParmDefaultValue.Size = new System.Drawing.Size(216, 20);
            this.tbParmDefaultValue.TabIndex = 10;
            this.tbParmDefaultValue.TextChanged += new System.EventHandler(this.tbParmDefaultValue_TextChanged);
            // 
            // lDefaultValue
            // 
            this.lDefaultValue.Location = new System.Drawing.Point(208, 184);
            this.lDefaultValue.Name = "lDefaultValue";
            this.lDefaultValue.Size = new System.Drawing.Size(100, 16);
            this.lDefaultValue.TabIndex = 15;
            this.lDefaultValue.Text = "Default Value";
            // 
            // bParmDown
            // 
            this.bParmDown.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.bParmDown.Location = new System.Drawing.Point(168, 64);
            this.bParmDown.Name = "bParmDown";
            this.bParmDown.Size = new System.Drawing.Size(24, 24);
            this.bParmDown.TabIndex = 14;
            this.bParmDown.Text = "";
            this.bParmDown.Click += new System.EventHandler(this.bParmDown_Click);
            // 
            // bParmUp
            // 
            this.bParmUp.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.bParmUp.Location = new System.Drawing.Point(168, 24);
            this.bParmUp.Name = "bParmUp";
            this.bParmUp.Size = new System.Drawing.Size(24, 24);
            this.bParmUp.TabIndex = 13;
            this.bParmUp.Text = "";
            this.bParmUp.Click += new System.EventHandler(this.bParmUp_Click);
            // 
            // tbParmValidValues
            // 
            this.tbParmValidValues.Location = new System.Drawing.Point(208, 152);
            this.tbParmValidValues.Name = "tbParmValidValues";
            this.tbParmValidValues.ReadOnly = true;
            this.tbParmValidValues.Size = new System.Drawing.Size(216, 20);
            this.tbParmValidValues.TabIndex = 9;
            // 
            // lbParmValidValues
            // 
            this.lbParmValidValues.Location = new System.Drawing.Point(208, 136);
            this.lbParmValidValues.Name = "lbParmValidValues";
            this.lbParmValidValues.Size = new System.Drawing.Size(100, 16);
            this.lbParmValidValues.TabIndex = 11;
            this.lbParmValidValues.Text = "Valid Values";
            // 
            // ckbParmAllowBlank
            // 
            this.ckbParmAllowBlank.Location = new System.Drawing.Point(288, 232);
            this.ckbParmAllowBlank.Name = "ckbParmAllowBlank";
            this.ckbParmAllowBlank.Size = new System.Drawing.Size(152, 24);
            this.ckbParmAllowBlank.TabIndex = 12;
            this.ckbParmAllowBlank.Text = "Allow blank (strings only)";
            this.ckbParmAllowBlank.CheckedChanged += new System.EventHandler(this.ckbParmAllowBlank_CheckedChanged);
            // 
            // ckbParmAllowNull
            // 
            this.ckbParmAllowNull.Location = new System.Drawing.Point(208, 232);
            this.ckbParmAllowNull.Name = "ckbParmAllowNull";
            this.ckbParmAllowNull.Size = new System.Drawing.Size(72, 24);
            this.ckbParmAllowNull.TabIndex = 11;
            this.ckbParmAllowNull.Text = "Allow null";
            this.ckbParmAllowNull.CheckedChanged += new System.EventHandler(this.ckbParmAllowNull_CheckedChanged);
            // 
            // tbParmPrompt
            // 
            this.tbParmPrompt.Location = new System.Drawing.Point(208, 104);
            this.tbParmPrompt.Name = "tbParmPrompt";
            this.tbParmPrompt.Size = new System.Drawing.Size(216, 20);
            this.tbParmPrompt.TabIndex = 8;
            this.tbParmPrompt.TextChanged += new System.EventHandler(this.tbParmPrompt_TextChanged);
            // 
            // lParmPrompt
            // 
            this.lParmPrompt.Location = new System.Drawing.Point(208, 88);
            this.lParmPrompt.Name = "lParmPrompt";
            this.lParmPrompt.Size = new System.Drawing.Size(48, 16);
            this.lParmPrompt.TabIndex = 7;
            this.lParmPrompt.Text = "Prompt";
            // 
            // cbParmType
            // 
            this.cbParmType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParmType.Items.AddRange(new object[] {
            "Boolean",
            "DateTime",
            "Integer",
            "Float",
            "String"});
            this.cbParmType.Location = new System.Drawing.Point(288, 56);
            this.cbParmType.Name = "cbParmType";
            this.cbParmType.Size = new System.Drawing.Size(80, 21);
            this.cbParmType.TabIndex = 6;
            this.cbParmType.SelectedIndexChanged += new System.EventHandler(this.cbParmType_SelectedIndexChanged);
            // 
            // lParmType
            // 
            this.lParmType.Location = new System.Drawing.Point(208, 56);
            this.lParmType.Name = "lParmType";
            this.lParmType.Size = new System.Drawing.Size(56, 23);
            this.lParmType.TabIndex = 5;
            this.lParmType.Text = "Datatype";
            // 
            // tbParmName
            // 
            this.tbParmName.Location = new System.Drawing.Point(288, 24);
            this.tbParmName.Name = "tbParmName";
            this.tbParmName.Size = new System.Drawing.Size(136, 20);
            this.tbParmName.TabIndex = 4;
            this.tbParmName.TextChanged += new System.EventHandler(this.tbParmName_TextChanged);
            // 
            // lParmName
            // 
            this.lParmName.Location = new System.Drawing.Point(208, 24);
            this.lParmName.Name = "lParmName";
            this.lParmName.Size = new System.Drawing.Size(48, 16);
            this.lParmName.TabIndex = 3;
            this.lParmName.Text = "Name";
            // 
            // bRemove
            // 
            this.bRemove.Location = new System.Drawing.Point(104, 264);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(56, 23);
            this.bRemove.TabIndex = 2;
            this.bRemove.Text = "Remove";
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(16, 264);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(56, 23);
            this.bAdd.TabIndex = 1;
            this.bAdd.Text = "Add";
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // lbParameters
            // 
            this.lbParameters.Location = new System.Drawing.Point(16, 16);
            this.lbParameters.Name = "lbParameters";
            this.lbParameters.Size = new System.Drawing.Size(144, 238);
            this.lbParameters.TabIndex = 0;
            this.lbParameters.SelectedIndexChanged += new System.EventHandler(this.lbParameters_SelectedIndexChanged);
            // 
            // DBSql
            // 
            this.DBSql.Controls.Add(this.panel2);
            this.DBSql.Location = new System.Drawing.Point(4, 22);
            this.DBSql.Name = "DBSql";
            this.DBSql.Size = new System.Drawing.Size(520, 300);
            this.DBSql.TabIndex = 1;
            this.DBSql.Tag = "sql";
            this.DBSql.Text = "SQL";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(520, 300);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer1.Size = new System.Drawing.Size(520, 300);
            this.splitContainer1.SplitterDistance = 173;
            this.splitContainer1.TabIndex = 5;
            // 
            // tvTablesColumns
            // 
            this.tvTablesColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTablesColumns.FullRowSelect = true;
            this.tvTablesColumns.Location = new System.Drawing.Point(0, 0);
            this.tvTablesColumns.Name = "tvTablesColumns";
            this.tvTablesColumns.Size = new System.Drawing.Size(173, 300);
            this.tvTablesColumns.TabIndex = 2;
            this.tvTablesColumns.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvTablesColumns_BeforeExpand);
            // 
            // tbSQL
            // 
            this.tbSQL.AllowDrop = true;
            this.tbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSQL.Location = new System.Drawing.Point(41, 3);
            this.tbSQL.Multiline = true;
            this.tbSQL.Name = "tbSQL";
            this.tbSQL.Size = new System.Drawing.Size(299, 294);
            this.tbSQL.TabIndex = 6;
            this.tbSQL.TextChanged += new System.EventHandler(this.tbSQL_TextChanged);
            this.tbSQL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSQL_KeyDown);
            // 
            // bMove
            // 
            this.bMove.Location = new System.Drawing.Point(3, 3);
            this.bMove.Name = "bMove";
            this.bMove.Size = new System.Drawing.Size(32, 23);
            this.bMove.TabIndex = 5;
            this.bMove.Text = ">>";
            this.bMove.Click += new System.EventHandler(this.bMove_Click);
            // 
            // TabularGroup
            // 
            this.TabularGroup.Controls.Add(this.clbSubtotal);
            this.TabularGroup.Controls.Add(this.ckbGrandTotal);
            this.TabularGroup.Controls.Add(this.label5);
            this.TabularGroup.Controls.Add(this.label4);
            this.TabularGroup.Controls.Add(this.cbColumnList);
            this.TabularGroup.Location = new System.Drawing.Point(4, 22);
            this.TabularGroup.Name = "TabularGroup";
            this.TabularGroup.Size = new System.Drawing.Size(520, 300);
            this.TabularGroup.TabIndex = 7;
            this.TabularGroup.Tag = "group";
            this.TabularGroup.Text = "Grouping";
            // 
            // clbSubtotal
            // 
            this.clbSubtotal.CheckOnClick = true;
            this.clbSubtotal.Location = new System.Drawing.Point(232, 32);
            this.clbSubtotal.Name = "clbSubtotal";
            this.clbSubtotal.Size = new System.Drawing.Size(192, 139);
            this.clbSubtotal.TabIndex = 5;
            this.clbSubtotal.SelectedIndexChanged += new System.EventHandler(this.emptyReportSyntax);
            // 
            // ckbGrandTotal
            // 
            this.ckbGrandTotal.Location = new System.Drawing.Point(16, 88);
            this.ckbGrandTotal.Name = "ckbGrandTotal";
            this.ckbGrandTotal.Size = new System.Drawing.Size(160, 24);
            this.ckbGrandTotal.TabIndex = 4;
            this.ckbGrandTotal.Text = "Calculate grand totals";
            this.ckbGrandTotal.CheckedChanged += new System.EventHandler(this.emptyReportSyntax);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(232, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Check columns you want to subtotal";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Pick a column to group (create hierarchy)";
            // 
            // cbColumnList
            // 
            this.cbColumnList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColumnList.Location = new System.Drawing.Point(16, 32);
            this.cbColumnList.Name = "cbColumnList";
            this.cbColumnList.Size = new System.Drawing.Size(200, 21);
            this.cbColumnList.TabIndex = 0;
            this.cbColumnList.SelectedIndexChanged += new System.EventHandler(this.emptyReportSyntax);
            // 
            // ReportSyntax
            // 
            this.ReportSyntax.Controls.Add(this.tbReportSyntax);
            this.ReportSyntax.Location = new System.Drawing.Point(4, 22);
            this.ReportSyntax.Name = "ReportSyntax";
            this.ReportSyntax.Size = new System.Drawing.Size(520, 300);
            this.ReportSyntax.TabIndex = 4;
            this.ReportSyntax.Tag = "syntax";
            this.ReportSyntax.Text = "Report Syntax";
            // 
            // tbReportSyntax
            // 
            this.tbReportSyntax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReportSyntax.Location = new System.Drawing.Point(0, 0);
            this.tbReportSyntax.Multiline = true;
            this.tbReportSyntax.Name = "tbReportSyntax";
            this.tbReportSyntax.ReadOnly = true;
            this.tbReportSyntax.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbReportSyntax.Size = new System.Drawing.Size(520, 300);
            this.tbReportSyntax.TabIndex = 0;
            this.tbReportSyntax.WordWrap = false;
            // 
            // ReportPreview
            // 
            this.ReportPreview.Controls.Add(this.rdlViewer1);
            this.ReportPreview.Location = new System.Drawing.Point(4, 22);
            this.ReportPreview.Name = "ReportPreview";
            this.ReportPreview.Size = new System.Drawing.Size(520, 300);
            this.ReportPreview.TabIndex = 5;
            this.ReportPreview.Tag = "preview";
            this.ReportPreview.Text = "Report Preview";
            // 
            // rdlViewer1
            // 
            this.rdlViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.rdlViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdlViewer1.Folder = null;
            this.rdlViewer1.HighlightAll = false;
            this.rdlViewer1.HighlightAllColor = System.Drawing.Color.Fuchsia;
            this.rdlViewer1.HighlightCaseSensitive = false;
            this.rdlViewer1.HighlightItemColor = System.Drawing.Color.Aqua;
            this.rdlViewer1.HighlightPageItem = null;
            this.rdlViewer1.HighlightText = null;
            this.rdlViewer1.Location = new System.Drawing.Point(0, 0);
            this.rdlViewer1.Name = "rdlViewer1";
            this.rdlViewer1.PageCurrent = 1;
            this.rdlViewer1.Parameters = null;
            this.rdlViewer1.ReportName = null;
            this.rdlViewer1.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous;
            this.rdlViewer1.SelectTool = false;
            this.rdlViewer1.ShowFindPanel = false;
            this.rdlViewer1.ShowParameterPanel = true;
            this.rdlViewer1.ShowWaitDialog = true;
            this.rdlViewer1.Size = new System.Drawing.Size(520, 300);
            this.rdlViewer1.SourceFile = null;
            this.rdlViewer1.SourceRdl = null;
            this.rdlViewer1.TabIndex = 0;
            this.rdlViewer1.UseTrueMargins = true;
            this.rdlViewer1.Zoom = 0.6181992F;
            this.rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(440, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(528, 40);
            this.panel1.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(344, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DialogDatabase
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(528, 366);
            this.Controls.Add(this.tcDialog);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogDatabase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Report from Database";
            this.Closed += new System.EventHandler(this.DialogDatabase_Closed);
            this.Load += new System.EventHandler(this.DialogDatabase_Load);
            this.tcDialog.ResumeLayout(false);
            this.ReportType.ResumeLayout(false);
            this.ReportType.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.DBConnection.ResumeLayout(false);
            this.DBConnection.PerformLayout();
            this.groupBoxSqlServer.ResumeLayout(false);
            this.groupBoxSqlServer.PerformLayout();
            this.ReportParameters.ResumeLayout(false);
            this.ReportParameters.PerformLayout();
            this.DBSql.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.TabularGroup.ResumeLayout(false);
            this.ReportSyntax.ResumeLayout(false);
            this.ReportSyntax.PerformLayout();
            this.ReportPreview.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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

        private Button buttonSqliteSelectDatabase;
        internal Button buttonSearchSqlServers;
        internal ComboBox comboServerList;
        internal Label label8;
        internal Button buttonDatabaseSearch;
        internal Label label9;
        internal ComboBox comboDatabaseList;
        private GroupBox groupBoxSqlServer;
        private Label label10;
        private TextBox textBoxSqlPassword;
        private Label label11;
        private TextBox textBoxSqlUser;
	}
}
