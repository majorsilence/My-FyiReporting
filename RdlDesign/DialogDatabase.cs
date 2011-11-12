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
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Xml;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for DialogDatabase.
	/// </summary>
	public class DialogDatabase : System.Windows.Forms.Form
	{
		RdlDesigner _rDesigner=null;
		static private readonly string SHARED_CONNECTION="Shared Data Source";
		string _StashConnection=null;

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TabPage DBConnection;
		private System.Windows.Forms.TabPage DBSql;
		private System.Windows.Forms.TabPage ReportType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
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

		private List<SqlColumn> _ColumnList=null;
		private string _TempFileName=null;
		private string _ResultReport="nothing";

		private readonly string _Schema2003 = 
"xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2003/10/reportdefinition\" xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\"";
		private readonly string _Schema2005 = 
"xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition\" xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\"";
		
		private string _TemplateChart=" some junk";
		private string _TemplateMatrix=" some junk";
		private string _TemplateTable= @"<?xml version='1.0' encoding='UTF-8'?>
<Report |schema| > 
	<Description>|description|</Description>
	<Author>|author|</Author>
	|orientation|
	<DataSources>
		<DataSource Name='DS1'>
			|connectionproperties|
		</DataSource>
	</DataSources>
	<Width>7.5in</Width>
	<TopMargin>.25in</TopMargin>
	<LeftMargin>.25in</LeftMargin>
	<RightMargin>.25in</RightMargin>
	<BottomMargin>.25in</BottomMargin>
	|reportparameters|
	<DataSets>
		<DataSet Name='Data'>
			<Query>
				<DataSourceName>DS1</DataSourceName>
				<CommandText>|sqltext|</CommandText>
				|queryparameters|
			</Query>
			<Fields>
			|sqlfields|
			</Fields>
		</DataSet>
	</DataSets>
	<PageHeader>
		<Height>.5in</Height>
|ifdef reportname|
		<ReportItems>
			<Textbox><Top>.1in</Top><Left>.1in</Left><Width>6in</Width><Height>.25in</Height><Value>|reportnameasis|</Value><Style><FontSize>15pt</FontSize><FontWeight>Bold</FontWeight></Style></Textbox> 
		</ReportItems>
|endif|
        <PrintOnFirstPage>true</PrintOnFirstPage>
        <PrintOnLastPage>true</PrintOnLastPage>
	</PageHeader>
	<Body>
		<ReportItems>
			<Table>
				<DataSetName>Data</DataSetName>
				<NoRows>Query returned no rows!</NoRows>
				<Style><BorderStyle><Default>Solid</Default></BorderStyle></Style>
				<TableColumns>
					|tablecolumns|
				</TableColumns>
				<Header>
					<TableRows>
						<TableRow>
							<Height>12pt</Height>
							<TableCells>|tableheaders|</TableCells>
						</TableRow>
					</TableRows>
					<RepeatOnNewPage>true</RepeatOnNewPage>
				</Header>
|ifdef grouping|
				<TableGroups>
				<TableGroup>
				<Header>
					<TableRows>
						<TableRow>
							<Height>12pt</Height>
							<TableCells>
								<TableCell>
									<ColSpan>|columncount|</ColSpan>
									<ReportItems><Textbox><Value>=Fields.|groupbycolumn|.Value</Value><Style><PaddingLeft>2pt</PaddingLeft><BorderStyle><Default>Solid</Default></BorderStyle><FontWeight>Bold</FontWeight></Style></Textbox></ReportItems>
								</TableCell>
							</TableCells>
						</TableRow>
					</TableRows>
					<RepeatOnNewPage>true</RepeatOnNewPage>
				</Header>
				<Footer>
					<TableRows>
						<TableRow>
							<Height>12pt</Height>
							<TableCells>|gtablefooters|</TableCells>
						</TableRow>
					</TableRows>
				</Footer>
		<Grouping Name='|groupbycolumn|Group'><GroupExpressions><GroupExpression>=Fields!|groupbycolumn|.Value</GroupExpression></GroupExpressions></Grouping>
		</TableGroup>
		</TableGroups>
|endif|
				<Details>
					<TableRows>
						<TableRow>
							<Height>12pt</Height>
							<TableCells>|tablevalues|</TableCells>
						</TableRow>
					</TableRows>
				</Details>
|ifdef footers|
				<Footer>
					<TableRows>
						<TableRow>
							<Height>12pt</Height>
							<TableCells>|tablefooters|</TableCells>
						</TableRow>
					</TableRows>
				</Footer>
|endif|
			</Table>
		</ReportItems>
		<Height>|bodyheight|</Height>
	</Body>
	<PageFooter>
		<Height>14pt</Height>
		<ReportItems>
			<Textbox><Top>1pt</Top><Left>10pt</Left><Height>12pt</Height><Width>3in</Width>
				<Value>=Globals!PageNumber + ' of ' + Globals!TotalPages</Value>
				<Style><FontSize>10pt</FontSize><FontWeight>Normal</FontWeight></Style>
			</Textbox> 	
		</ReportItems>
        <PrintOnFirstPage>true</PrintOnFirstPage>
        <PrintOnLastPage>true</PrintOnLastPage>
	</PageFooter>
</Report>";
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
		private string _TemplateList= @"<?xml version='1.0' encoding='UTF-8'?>
<Report |schema| > 
	<Description>|description|</Description>
	<Author>|author|</Author>
	|orientation|
	<DataSources>
		<DataSource Name='DS1'>
			|connectionproperties|
		</DataSource>
	</DataSources>
	<Width>7.5in</Width>
	<TopMargin>.25in</TopMargin>
	<LeftMargin>.25in</LeftMargin>
	<RightMargin>.25in</RightMargin>
	<BottomMargin>.25in</BottomMargin>
	|reportparameters|
	<DataSets>
		<DataSet Name='Data'>
			<Query>
				<DataSourceName>DS1</DataSourceName>
				<CommandText>|sqltext|</CommandText>
				|queryparameters|
			</Query>
			<Fields>
				|sqlfields|
			</Fields>
		</DataSet>
	</DataSets>
	<PageHeader>
		<Height>.5in</Height>
		<ReportItems>
|ifdef reportname|
			<Textbox><Top>.02in</Top><Left>.1in</Left><Width>6in</Width><Height>.25in</Height><Value>|reportname|</Value><Style><FontSize>15pt</FontSize><FontWeight>Bold</FontWeight></Style></Textbox> 
|endif|
			|listheaders|
		</ReportItems>
        <PrintOnFirstPage>true</PrintOnFirstPage>
        <PrintOnLastPage>true</PrintOnLastPage>
	</PageHeader>
	<Body><Height>25pt</Height>
		<ReportItems>
			<List>
				<DataSetName>Data</DataSetName>
				<Height>24pt</Height>
				<NoRows>Query returned no rows!</NoRows>
				<ReportItems>
					|listvalues|
				</ReportItems>
				<Width>|listwidth|</Width>
			</List>
		</ReportItems>
		</Body>
	<PageFooter>
		<Height>14pt</Height>
		<ReportItems>
			<Textbox><Top>1pt</Top><Left>10pt</Left><Height>12pt</Height><Width>3in</Width>
				<Value>=Globals!PageNumber + ' of ' + Globals!TotalPages</Value>
				<Style><FontSize>10pt</FontSize><FontWeight>Normal</FontWeight></Style>
			</Textbox> 	
		</ReportItems>
        <PrintOnFirstPage>true</PrintOnFirstPage>
        <PrintOnLastPage>true</PrintOnLastPage>
	</PageFooter>
		</Report>";

		public DialogDatabase(RdlDesigner rDesigner)
		{
			_rDesigner = rDesigner;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			string[] items = RdlEngineConfig.GetProviders();
			cbConnectionTypes.Items.Add(SHARED_CONNECTION);
			cbConnectionTypes.Items.AddRange(items);
			cbConnectionTypes.SelectedIndex = 1;
			cbOrientation.SelectedIndex = 0;
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvTablesColumns = new System.Windows.Forms.TreeView();
            this.bMove = new System.Windows.Forms.Button();
            this.tbSQL = new System.Windows.Forms.TextBox();
            this.tcDialog.SuspendLayout();
            this.ReportType.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.DBConnection.SuspendLayout();
            this.ReportParameters.SuspendLayout();
            this.DBSql.SuspendLayout();
            this.panel2.SuspendLayout();
            this.TabularGroup.SuspendLayout();
            this.ReportSyntax.SuspendLayout();
            this.ReportPreview.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.rdlViewer1.ShowFindPanel = false;
            this.rdlViewer1.ShowParameterPanel = true;
            this.rdlViewer1.Size = new System.Drawing.Size(520, 300);
            this.rdlViewer1.SourceFile = null;
            this.rdlViewer1.SourceRdl = null;
            this.rdlViewer1.TabIndex = 0;
            this.rdlViewer1.Text = "rdlViewer1";
            this.rdlViewer1.UseTrueMargins = true;
            this.rdlViewer1.Zoom = 0.5969851F;
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
            // bMove
            // 
            this.bMove.Location = new System.Drawing.Point(3, 3);
            this.bMove.Name = "bMove";
            this.bMove.Size = new System.Drawing.Size(32, 23);
            this.bMove.TabIndex = 5;
            this.bMove.Text = ">>";
            this.bMove.Click += new System.EventHandler(this.bMove_Click);
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
            this.tcDialog.ResumeLayout(false);
            this.ReportType.ResumeLayout(false);
            this.ReportType.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.DBConnection.ResumeLayout(false);
            this.DBConnection.PerformLayout();
            this.ReportParameters.ResumeLayout(false);
            this.ReportParameters.PerformLayout();
            this.DBSql.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.TabularGroup.ResumeLayout(false);
            this.ReportSyntax.ResumeLayout(false);
            this.ReportSyntax.PerformLayout();
            this.ReportPreview.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public string ResultReport
		{
			get {return _ResultReport;}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (!DoReportSyntax(false))
				return;
			DialogResult = DialogResult.OK;
			_ResultReport = tbReportSyntax.Text;
			this.Close();		
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TabControl tc = (TabControl) sender;
			string tag = (string) tc.TabPages[tc.SelectedIndex].Tag;
			switch (tag)
			{
				case "type":	// nothing to do here
					break;
				case "connect":	// nothing to do here
					break;
				case "sql":		// obtain table and column information
					DoSqlSchema();
					break;
				case "group":	// obtain group by information using connection & sql
					DoGrouping();
					break;
				case "syntax":	// obtain report using connection, sql, 
					DoReportSyntax(false);
					break;
				case "preview":	// run report using generated report syntax
					DoReportPreview();
					break;
				default:
					break;
			}
		}

		// Fill out tvTablesColumns
		private void DoSqlSchema()
		{
			// TODO be more efficient and remember schema info;
			//   need to mark changes to connections
			if (tvTablesColumns.Nodes.Count > 0)
				return;

			// suppress redraw until tree view is complete
			tvTablesColumns.BeginUpdate();
			
			// Get the schema information
			List<SqlSchemaInfo> si = DesignerUtility.GetSchemaInfo(GetDataProvider(), GetDataConnection());
			TreeNode ndRoot = new TreeNode("Tables");
			tvTablesColumns.Nodes.Add(ndRoot);
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

			// Now do parameters
			if (lbParameters.Items.Count > 0)
			{
				ndRoot = new TreeNode("Parameters");
				tvTablesColumns.Nodes.Add(ndRoot);
				foreach(ReportParm rp in lbParameters.Items)
				{
					string paramName;

					// force the name to start with @
					if (rp.Name[0] == '@')
						paramName = rp.Name;
					else
						paramName = "@" + rp.Name;

					// Add the node to the tree
					TreeNode aRoot = new TreeNode(paramName);
					ndRoot.Nodes.Add(aRoot);
				}
			}

			tvTablesColumns.EndUpdate();
		}

		private void DoGrouping()
		{
			if (cbColumnList.Items.Count > 0)		// We already have the columns?
				return;

			if (_ColumnList == null)
				_ColumnList = DesignerUtility.GetSqlColumns(GetDataProvider(), GetDataConnection(), tbSQL.Text, this.lbParameters.Items);

			foreach (SqlColumn sq in _ColumnList)
			{
				cbColumnList.Items.Add(sq);
				clbSubtotal.Items.Add(sq);
			}

			SqlColumn sqc = new SqlColumn();
			sqc.Name="";
			cbColumnList.Items.Add(sqc);
			return;
		}

		private bool DoReportSyntax(bool UseFullSharedDSName)
		{
			string template;
 
			if (rbList.Checked)
				template = _TemplateList;
			else if (rbTable.Checked)
				template = _TemplateTable;
			else if (rbMatrix.Checked)
				template = _TemplateMatrix;
			else if (rbChart.Checked)
				template = _TemplateChart;
			else
				template = _TemplateTable;	// default to table- should never reach
			
			if (_ColumnList == null)
				_ColumnList = DesignerUtility.GetSqlColumns(GetDataProvider(), GetDataConnection(), tbSQL.Text, this.lbParameters.Items);

			if (_ColumnList.Count == 0)		// can only happen by an error
				return false;

			string[] parts = template.Split('|');
			StringBuilder sb = new StringBuilder(template.Length);
			decimal left=0m; 
			decimal width;
			decimal bodyHeight=0;
			string name;
			int skip=0;					// skip is used to allow nesting of ifdef 
			string args;
			string canGrow;
			string align;
			// handle the group by column
			string gbcolumn;
			if (this.cbColumnList.Text.Length > 0)
				gbcolumn = GetFieldName(this.cbColumnList.Text);
			else
				gbcolumn = null;
			
			CultureInfo cinfo = new CultureInfo("", false );

			foreach (string p in parts)
			{
				// Handle conditional special
				if (p.Substring(0, 5) == "ifdef")
				{
					args = p.Substring(6);
					switch (args)
					{
						case "reportname":
							if (tbReportName.Text.Length == 0)
								skip++;
							break;
						case "description":
							if (tbReportDescription.Text.Length == 0)
								skip++;
							break;
						case "author":
							if (tbReportAuthor.Text.Length == 0)
								skip++;
							break;
						case "grouping":
							if (gbcolumn == null)
								skip++;
							break;
						case "footers":
							if (!this.ckbGrandTotal.Checked)
								skip++;
							else if (this.clbSubtotal.CheckedItems.Count <= 0)
								skip++;
							break;
						default:
							throw new Exception(String.Format("Unknown ifdef element {0} specified in template.", args));
					}
					continue;
				}

				// if skipping lines (due to ifdef) then go to next endif
				if (skip > 0 && p != "endif")
					continue;

				switch (p)
				{
					case "endif":
						if (skip > 0)
							skip--;
						break;
					case "schema":
						if (this.rbSchema2003.Checked)
							sb.Append(_Schema2003);
						else if (this.rbSchema2005.Checked)
							sb.Append(_Schema2005);
						break;
					case "reportname":	 
						sb.Append(tbReportName.Text.Replace('\'', '_'));
						break;
					case "reportnameasis":
						sb.Append(tbReportName.Text);
						break;
					case "description":
						sb.Append(tbReportDescription.Text);
						break;
					case "author":
						sb.Append(tbReportAuthor.Text);
						break;
					case "connectionproperties":
						if (this.cbConnectionTypes.Text == SHARED_CONNECTION)
						{
							string file = this.tbConnection.Text;
                            if (!UseFullSharedDSName)
							    file = Path.GetFileNameWithoutExtension(file);      // when we save report we use qualified name
							sb.AppendFormat("<DataSourceReference>{0}</DataSourceReference>", file);
						}
						else
							sb.AppendFormat("<ConnectionProperties><DataProvider>{0}</DataProvider><ConnectString>{1}</ConnectString></ConnectionProperties>",
								GetDataProvider(), GetDataConnection());
						break;
					case "dataprovider":
						sb.Append(GetDataProvider());
						break;
					case "connectstring":
						sb.Append(tbConnection.Text);
						break;
					case "columncount":
						sb.Append(_ColumnList.Count);
						break;
					case "orientation":
						if (this.cbOrientation.SelectedIndex == 0)
						{	// Portrait is first in the list
							sb.Append("<PageHeight>11in</PageHeight><PageWidth>8.5in</PageWidth>");
						}
						else
						{
							sb.Append("<PageHeight>8.5in</PageHeight><PageWidth>11in</PageWidth>");
						}
						break;
					case "groupbycolumn":
						sb.Append(gbcolumn);
						break;
					case "reportparameters":
						DoReportSyntaxParameters(cinfo, sb);
						break;
					case "queryparameters":
						DoReportSyntaxQParameters(cinfo, sb, tbSQL.Text);
						break;
					case "sqltext":
						sb.Append(tbSQL.Text.Replace("<", "&lt;"));
						break;
					case "sqlfields":
						foreach (SqlColumn sq in _ColumnList)
						{
							name = GetFieldName(sq.Name);
							string type = sq.DataType.FullName;
							if (this.rbSchemaNo.Checked)
								sb.AppendFormat(cinfo, "<Field Name='{0}'><DataField>{1}</DataField><TypeName>{2}</TypeName></Field>", name, sq.Name, type);
							else
								sb.AppendFormat(cinfo, "<Field Name='{0}'><DataField>{1}</DataField><rd:TypeName>{2}</rd:TypeName></Field>", name, sq.Name, type);
						}
						break;
					case "listheaders":
						left = .0m; 
						foreach (SqlColumn sq in _ColumnList)
						{
							name = sq.Name;
							width = name.Length / 8m;
							if (width < 1)
								width = 1;
							sb.AppendFormat(cinfo, @"
		<Textbox><Top>.3in</Top><Left>{0}in</Left><Width>{1}in</Width><Height>.2in</Height><Value>{2}</Value>
			<Style><FontWeight>Bold</FontWeight><BorderStyle><Bottom>Solid</Bottom></BorderStyle>
				<BorderWidth><Bottom>3pt</Bottom></BorderWidth></Style>
		</Textbox>", 
								left,
								width,
								name);
							left += width;
						}
						break;
					case "listvalues":
						left = .0m; 
						foreach (SqlColumn sq in _ColumnList)
						{
							name = GetFieldName(sq.Name);
							DoAlignAndCanGrow(sq.DataType, out canGrow, out align);
							width = name.Length / 8m;
							if (width < 1)
								width = 1;
							sb.AppendFormat(cinfo, @"
		<Textbox Name='{2}'><Top>.1in</Top><Left>{0}in</Left><Width>{1}in</Width><Height>.25in</Height><Value>=Fields!{2}.Value</Value><CanGrow>{3}</CanGrow><Style>{4}</Style></Textbox>", 
								left, width, name, canGrow, align);
							left += width;
						}
						bodyHeight = .4m;
						break;
					case "listwidth":		// in template list width must follow something that sets left
						sb.AppendFormat(cinfo, "{0}in", left);
						break;
					case "tableheaders":
						// the group by column is always the first one in the table
						if (gbcolumn != null)
						{
							bodyHeight += 12m;
							sb.AppendFormat(cinfo, @"
							<TableCell>
								<ReportItems><Textbox><Value>{0}</Value><Style><TextAlign>Center</TextAlign><BorderStyle><Default>Solid</Default></BorderStyle><FontWeight>Bold</FontWeight></Style></Textbox></ReportItems>
							</TableCell>",
								this.cbColumnList.Text);
						}
						bodyHeight += 12m;
						foreach (SqlColumn sq in _ColumnList)
						{
							name = sq.Name;
							if (name == this.cbColumnList.Text)
								continue;
							sb.AppendFormat(cinfo, @"
							<TableCell>
								<ReportItems><Textbox><Value>{0}</Value><Style><TextAlign>Center</TextAlign><BorderStyle><Default>Solid</Default></BorderStyle><FontWeight>Bold</FontWeight></Style></Textbox></ReportItems>
							</TableCell>",
								name);
						}
						break;
					case "tablecolumns":
						if (gbcolumn != null)
						{
							bodyHeight += 12m;
							width = gbcolumn.Length / 8m;		// TODO should really use data value
							if (width < 1)
								width = 1;
							sb.AppendFormat(cinfo, @"<TableColumn><Width>{0}in</Width></TableColumn>", width);
						}
						bodyHeight += 12m;
						foreach (SqlColumn sq in _ColumnList)
						{
							name = GetFieldName(sq.Name);
							if (name == gbcolumn)
								continue;
							width = name.Length / 8m;		// TODO should really use data value
							if (width < 1)
								width = 1;
							sb.AppendFormat(cinfo, @"<TableColumn><Width>{0}in</Width></TableColumn>", width);
						}
						break;
					case "tablevalues":
						bodyHeight += 12m;
						if (gbcolumn != null)
						{
							sb.Append(@"<TableCell>
								<ReportItems><Textbox><Value></Value><Style><BorderStyle><Default>None</Default><Left>Solid</Left></BorderStyle></Style></Textbox></ReportItems>
							</TableCell>");
						}
						foreach (SqlColumn sq in _ColumnList)
						{
							name = GetFieldName(sq.Name);
							if (name == gbcolumn)
								continue;
							DoAlignAndCanGrow(sq.DataType, out canGrow, out align);
							sb.AppendFormat(cinfo, @"
							<TableCell>
								<ReportItems><Textbox Name='{0}'><Value>=Fields!{0}.Value</Value><CanGrow>{1}</CanGrow><Style><BorderStyle><Default>Solid</Default></BorderStyle>{2}</Style></Textbox></ReportItems>
							</TableCell>",
								name, canGrow, align);
						}
						break;
					case "gtablefooters":
					case "tablefooters":
						bodyHeight += 12m;
						canGrow="false";
						align="";
						string nameprefix = p == "gtablefooters"? "gf": "tf";
						if (gbcolumn != null)	// handle group by column first
						{
							int i = clbSubtotal.FindStringExact(this.cbColumnList.Text);
							SqlColumn sq= i < 0? null: (SqlColumn) clbSubtotal.Items[i];
							if (i >= 0 && clbSubtotal.GetItemChecked(i))
							{
								string funct = DesignerUtility.IsNumeric(sq.DataType)? "Sum": "Count";

								DoAlignAndCanGrow(((object) 0).GetType(), out canGrow, out align);
								sb.AppendFormat(cinfo, @"
							<TableCell>
								<ReportItems><Textbox Name='{4}_{0}'><Value>={1}(Fields!{0}.Value)</Value><CanGrow>{2}</CanGrow><Style><BorderStyle><Default>Solid</Default></BorderStyle>{3}</Style></Textbox></ReportItems>
							</TableCell>",
									gbcolumn, funct, canGrow, align, nameprefix);
							}
							else
							{
								sb.AppendFormat(cinfo, "<TableCell><ReportItems><Textbox><Value></Value><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></Textbox></ReportItems></TableCell>");
							}
						}
						for (int i=0; i < this.clbSubtotal.Items.Count; i++)
						{
							SqlColumn sq = (SqlColumn) clbSubtotal.Items[i];
							name = GetFieldName(sq.Name);
							if (name == gbcolumn)
								continue;
							if (clbSubtotal.GetItemChecked(i))
							{
								string funct = DesignerUtility.IsNumeric(sq.DataType)? "Sum": "Count";

								DoAlignAndCanGrow(((object) 0).GetType(), out canGrow, out align);
								sb.AppendFormat(cinfo, @"
							<TableCell>
								<ReportItems><Textbox Name='{4}_{0}'><Value>={1}(Fields!{0}.Value)</Value><CanGrow>{2}</CanGrow><Style><BorderStyle><Default>Solid</Default></BorderStyle>{3}</Style></Textbox></ReportItems>
							</TableCell>",
									name, funct, canGrow, align, nameprefix);
							}
							else
							{
								sb.AppendFormat(cinfo, "<TableCell><ReportItems><Textbox><Value></Value><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></Textbox></ReportItems></TableCell>");
							}
						}
						break;
					case "bodyheight":	// Note: this must follow the table definition
						sb.AppendFormat(cinfo, "{0}pt", bodyHeight);						
						break;
					default:
						sb.Append(p);
						break;
				}
			}

			try
			{
				tbReportSyntax.Text = DesignerUtility.FormatXml(sb.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Internal Error");
				tbReportSyntax.Text = sb.ToString();
			}
			return true;
		}

		private string GetFieldName(string sqlName)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in sqlName)
			{
				if (Char.IsLetterOrDigit(c) || c == '_')
					sb.Append(c);
				else 
					sb.Append('_');
			}
			return sb.ToString();
		}

		private void DoAlignAndCanGrow(Type t, out string canGrow, out string align)
		{
			string st = t.ToString();	
			switch (st)
			{
				case "System.String":
					canGrow="true";
					align="<PaddingLeft>2pt</PaddingLeft>";
					break;
				case "System.Int16":
				case "System.Int32":
				case "System.Single":
				case "System.Double":
				case "System.Decimal":
					canGrow="false";
					align="<PaddingRight>2pt</PaddingRight><TextAlign>Right</TextAlign>";
					break;
				default:
					canGrow="false";
					align="<PaddingLeft>2pt</PaddingLeft>";
					break;
			}
			return;
		}


		private void DoReportSyntaxParameters(CultureInfo cinfo, StringBuilder sb)
		{
			if (this.lbParameters.Items.Count <= 0)
				return;

			sb.Append("<ReportParameters>");
			foreach (ReportParm rp in lbParameters.Items)
			{
				sb.AppendFormat(cinfo, "<ReportParameter Name=\"{0}\">", rp.Name);
				sb.AppendFormat(cinfo, "<DataType>{0}</DataType>", rp.DataType);
				sb.AppendFormat(cinfo, "<Nullable>{0}</Nullable>", rp.AllowNull.ToString());
				if (rp.DefaultValue != null && rp.DefaultValue.Count > 0)
				{
					sb.AppendFormat(cinfo, "<DefaultValue><Values>");
					foreach (string dv in rp.DefaultValue)
					{
						sb.AppendFormat(cinfo, "<Value>{0}</Value>", XmlUtil.XmlAnsi(dv));
					}
					sb.AppendFormat(cinfo, "</Values></DefaultValue>");
				}
				sb.AppendFormat(cinfo, "<AllowBlank>{0}</AllowBlank>", rp.AllowBlank);
				if (rp.Prompt != null && rp.Prompt.Length > 0)
					sb.AppendFormat(cinfo, "<Prompt>{0}</Prompt>", rp.Prompt);
				if (rp.ValidValues != null && rp.ValidValues.Count > 0)
				{
					sb.Append("<ValidValues><ParameterValues>");
					foreach (ParameterValueItem pvi in rp.ValidValues)
					{
						sb.Append("<ParameterValue>");
						sb.AppendFormat(cinfo, "<Value>{0}</Value>", XmlUtil.XmlAnsi(pvi.Value));
						if (pvi.Label != null)
							sb.AppendFormat(cinfo, "<Label>{0}</Label>", XmlUtil.XmlAnsi(pvi.Label));
						sb.Append("</ParameterValue>");
					}
					sb.Append("</ParameterValues></ValidValues>");
				}
				sb.Append("</ReportParameter>");
			}
			sb.Append("</ReportParameters>");
		}

		private void DoReportSyntaxQParameters(CultureInfo cinfo, StringBuilder sb, string sql)
		{
			if (this.lbParameters.Items.Count <= 0)
				return;

			bool bFirst=true;
			foreach (ReportParm rp in lbParameters.Items)
			{
				// force the name to start with @
				string paramName;
				if (rp.Name[0] == '@')
					paramName = rp.Name;
				else
					paramName = "@" + rp.Name;

				// Only create a query parameter if parameter is used in the query
				if (sql.IndexOf(paramName) >= 0)
				{
					if (bFirst)
					{	// Only put out queryparameters if we actually have one
						sb.Append("<QueryParameters>");
						bFirst=false;
					}
					sb.AppendFormat(cinfo, "<QueryParameter Name=\"{0}\">", rp.Name);
					sb.AppendFormat(cinfo, "<Value>=Parameters!{0}</Value>",rp.Name);
					sb.Append("</QueryParameter>");
				}
			}
			if (!bFirst)
				sb.Append("</QueryParameters>");
		}

		private bool DoReportPreview()
		{
			if (!DoReportSyntax(true))
				return false;

            rdlViewer1.GetDataSourceReferencePassword = _rDesigner.SharedDatasetPassword; 
            rdlViewer1.SourceRdl = tbReportSyntax.Text;
			return true;
		}

		private string GetDataProvider()
		{
			string cType = cbConnectionTypes.Text;
			_StashConnection = null;
			if (cType == SHARED_CONNECTION)
			{
                if (!DesignerUtility.GetSharedConnectionInfo(_rDesigner, tbConnection.Text, out cType, out _StashConnection))
                    return null;
			}
			else
			{
				_StashConnection = tbConnection.Text;
			}
			return cType;
		}

		private string GetDataConnection()
		{	// GetDataProvider must be called first to ensure the DataConnection is correct.
			return _StashConnection;
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
			List<SqlColumn> tColumns = DesignerUtility.GetSqlColumns(GetDataProvider(), GetDataConnection(), sql, null);
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

		private void tbSQL_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))	// only accept text
				e.Effect = DragDropEffects.Copy;
		}

		private void tbSQL_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
				tbSQL.SelectedText = (string) e.Data.GetData(DataFormats.Text);
		}

		private void tvTablesColumns_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode node = tvTablesColumns.GetNodeAt(e.X, e.Y);
			if (node == null || node.Parent == null)
				return; 

			string dragText;
			if (tbSQL.Text == "")
			{
				if (node.Parent.Parent == null)
				{	// select table; generate full select for table
					tvTablesColumns_ExpandTable(node);	// make sure we've obtained the columns

					dragText="SELECT ";
					TreeNode next = node.FirstNode;
					while (true)
					{
                        dragText += DesignerUtility.NormalizeSqlName(next.Text);
						next = next.NextNode;
						if (next == null)
							break;
						dragText += ", ";
					}
                    dragText += (" FROM " + DesignerUtility.NormalizeSqlName(node.Text));
				}
				else
				{	// select column; generate select of that column	
                    dragText = "SELECT " + DesignerUtility.NormalizeSqlName(node.Text) + " FROM " + DesignerUtility.NormalizeSqlName(node.Parent.Text);
				}
			}
			else
				dragText = node.Text;

			tvTablesColumns.DoDragDrop(dragText, DragDropEffects.Copy);
		}


		private void DialogDatabase_Closed(object sender, System.EventArgs e)
		{
			if (_TempFileName != null)
				File.Delete(_TempFileName);
		}

		private void tbSQL_TextChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
			_ColumnList=null;			// get rid of any column list as well
			cbColumnList.Items.Clear();	// and clear out other places where columns show
			cbColumnList.Text="";
			clbSubtotal.Items.Clear();
		}

		private void tbReportName_TextChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
		}

		private void tbReportDescription_TextChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
		}

		private void tbReportAuthor_TextChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
		}

		private void rbTable_CheckedChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax

			if (rbTable.Checked)
			{
				TabularGroup.Enabled = true;
			}
			else
			{
				TabularGroup.Enabled = false;
			}
		}

		private void rbList_CheckedChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
		}

		private void rbMatrix_CheckedChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
		}

		private void rbChart_CheckedChanged(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";	// when SQL changes get rid of report syntax
		}

		private void bAdd_Click(object sender, System.EventArgs e)
		{
			ReportParm rp = new ReportParm("newparm");
			int cur = this.lbParameters.Items.Add(rp);
			lbParameters.SelectedIndex = cur;
			this.tbParmName.Focus();
		}

		private void bRemove_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;
			lbParameters.Items.RemoveAt(cur);
			if (lbParameters.Items.Count <= 0)
				return;
			cur--;
			if (cur < 0)
				cur = 0;
			lbParameters.SelectedIndex = cur;
		}

		private void lbParameters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			tbParmName.Text = rp.Name;
			cbParmType.Text = rp.DataType;
			tbParmPrompt.Text = rp.Prompt;
			tbParmDefaultValue.Text = rp.DefaultValueDisplay; 			
			ckbParmAllowBlank.Checked = rp.AllowBlank;
			tbParmValidValues.Text = rp.ValidValuesDisplay;
			ckbParmAllowNull.Checked = rp.AllowNull;
		}

		private void lbParameters_MoveItem(int curloc, int newloc)
		{
			ReportParm rp = lbParameters.Items[curloc] as ReportParm;
			if (rp == null)
				return;

			lbParameters.BeginUpdate();
			lbParameters.Items.RemoveAt(curloc);
			lbParameters.Items.Insert(newloc, rp);
			lbParameters.SelectedIndex = newloc;
			lbParameters.EndUpdate();
		}

		private void tbParmName_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			if (rp.Name == tbParmName.Text)
				return;

			rp.Name = tbParmName.Text;
			// text doesn't change in listbox; force change by removing and re-adding item
			lbParameters_MoveItem(cur, cur);
		}

		private void cbParmType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.DataType = cbParmType.Text;
		}

		private void tbParmPrompt_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.Prompt = tbParmPrompt.Text;
		}

		private void ckbParmAllowNull_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.AllowNull = ckbParmAllowNull.Checked;
		}

		private void ckbParmAllowBlank_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			rp.AllowBlank = ckbParmAllowBlank.Checked;
		}

		private void bParmUp_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur <= 0)
				return;
		
			lbParameters_MoveItem(cur, cur-1);
		}

		private void bParmDown_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur+1 >= lbParameters.Items.Count)
				return;
		
			lbParameters_MoveItem(cur, cur+1);
		}

		private void tbParmDefaultValue_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			if (tbParmDefaultValue.Text.Length > 0)
			{
				if (rp.DefaultValue == null)
					rp.DefaultValue = new List<string>();
				else
					rp.DefaultValue.Clear();
				rp.DefaultValue.Add(tbParmDefaultValue.Text);
			}
			else
				rp.DefaultValue = null;
		
		}

		private void tbConnection_TextChanged(object sender, System.EventArgs e)
		{
			tvTablesColumns.Nodes.Clear();
		}

		private void emptyReportSyntax(object sender, System.EventArgs e)
		{
			tbReportSyntax.Text = "";		// need to generate another report
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

		private void bValidValues_Click(object sender, System.EventArgs e)
		{
			int cur = lbParameters.SelectedIndex;
			if (cur < 0)
				return;

			ReportParm rp = lbParameters.Items[cur] as ReportParm;
			if (rp == null)
				return;

			DialogValidValues dvv = new DialogValidValues(rp.ValidValues);
            try
            {
                if (dvv.ShowDialog() != DialogResult.OK)
                    return;
                rp.ValidValues = dvv.ValidValues;
                this.tbParmValidValues.Text = rp.ValidValuesDisplay;
            }
            finally
            {
                dvv.Dispose();
            }
		}

		private void cbConnectionTypes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cbConnectionTypes.Text == SHARED_CONNECTION)
			{
				this.lConnection.Text = "Shared Data Source File:";
				bShared.Visible = true;
			}
			else
			{
				this.lConnection.Text = "Connection:";
				bShared.Visible = false;
			}

			if (cbConnectionTypes.Text == "ODBC")
			{
				lODBC.Visible = cbOdbcNames.Visible = true;
				DesignerUtility.FillOdbcNames(cbOdbcNames);
			}
			else
			{
				lODBC.Visible = cbOdbcNames.Visible = false;
			}
#if DEBUG
			// this is only for ease of testing
			switch (cbConnectionTypes.Text)
			{
				case "SQL":
					tbConnection.Text = "Server=(local)\\VSDotNet;DataBase=Northwind;Integrated Security=SSPI;Connect Timeout=5";
					break;
				case "ODBC":
					tbConnection.Text = "dsn=world;UID=user;PWD=pswd;";
					break;
				case "Oracle":
					tbConnection.Text = "User Id=SYSTEM;Password=tiger;Data Source=orcl";
					break;
				case "Firebird.NET":
					tbConnection.Text = @"Dialect=3;User Id=SYSDBA;Database=C:\Program Files\Firebird\Firebird_1_5\examples\employee.fdb;Data Source=localhost;Password=masterkey";
					break;
				case "MySQL.NET":
					tbConnection.Text = "database=world;user id=user;password=pswd;";
					break;
				case "iAnywhere.NET":
					tbConnection.Text = "Data Source=ASA 9.0 Sample;UID=DBA;PWD=SQL";
					break;
				default:
					tbConnection.Text = "";
					break;
			}
#endif
		}

		private void cbOdbcNames_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string name = "dsn=" + cbOdbcNames.Text + ";";
			this.tbConnection.Text = name;
		}

		private void bTestConnection_Click(object sender, System.EventArgs e)
		{
			string cType = GetDataProvider();
			if (cType == null)
				return;

			if (DesignerUtility.TestConnection(cType, GetDataConnection()))
				MessageBox.Show("Connection successful!", "Test Connection");
		}

		private void DBConnection_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!DesignerUtility.TestConnection(this.GetDataConnection(), GetDataConnection()))
				e.Cancel = true;
		}

		private void bShared_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Data source reference files (*.dsr)|*.dsr" +
                "|All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			if (tbConnection.Text.Length > 0)
				ofd.FileName = tbConnection.Text;
			else
				ofd.FileName = "*.dsr";

			ofd.Title = "Specify Data Source Reference File Name";
			ofd.CheckFileExists = true;
			ofd.DefaultExt = "dsr";
			ofd.AddExtension = true;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    tbConnection.Text = ofd.FileName;
            }
            finally
            {
                ofd.Dispose();
            }
		}
	}
}
