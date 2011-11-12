using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using fyiReporting.RDL;
using fyiReporting.RdlViewer;

namespace DataTests
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
        private fyiReporting.RdlViewer.RdlViewer rdlViewer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button bSetReport;
		private System.Windows.Forms.Button bManualDataTable;
		private System.Windows.Forms.Button bXMLData;
		private System.Windows.Forms.Button bIDataReader;
		private System.Windows.Forms.TextBox tbConnectionString;
		private System.Windows.Forms.Button bDataSource;
		private System.Windows.Forms.Button bEnumerable;
		private System.Windows.Forms.Button bParameters;
		private System.Windows.Forms.Button bSetParm;
        private TextBox tbParms;
        private Button bOpen;
        private CheckBox chkFind;
        private Button bPageUp;
        private Button bPageDown;
        private string _DataSourceReferencePassword = null;
        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


            InitMenuItems();
			//
			// Handle the Hyperlink event.
			//
            this.rdlViewer.Hyperlink += new RdlViewer.HyperlinkEventHandler(rdlViewer_Hyperlink);
            this.rdlViewer.SubreportDataRetrieval +=new EventHandler<SubreportDataRetrievalEventArgs>(rdlViewer_SubreportDataRetrieval);
            this.rdlViewer.GetDataSourceReferencePassword = new fyiReporting.RDL.NeedPassword(this.GetPassword); 
        }

        void InitMenuItems()
        {
            MenuItem menuResources = new MenuItem("&Resources", new EventHandler(this.menuResources_Click));

            // Create file menu and add array of sub-menu items
            MenuItem menuTools = new MenuItem("&Tools");
            menuTools.MenuItems.AddRange(
                new MenuItem[] { menuResources });

            MainMenu menuMain = new MainMenu(new MenuItem[]{menuTools});
            this.Menu = menuMain;
        }

        void rdlViewer_Hyperlink(object source, HyperlinkEventArgs e)
        {
            if (MessageBox.Show(string.Format("Do you wish to invoke Hyperlink {0}", e.Hyperlink), "Hyperlink", MessageBoxButtons.YesNo)
                == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        void rdlViewer_SubreportDataRetrieval(object sender, SubreportDataRetrievalEventArgs e)
        {
            int ids = 0;
            foreach (fyiReporting.RDL.DataSet ds in e.Report.DataSets)
                ids++;
            MessageBox.Show(string.Format("Subreport Data Retrieval: {0} datasets", ids));
        }
        
        string GetPassword()
        {
            if (_DataSourceReferencePassword != null)
                return _DataSourceReferencePassword;

            DataSourcePassword dlg = new DataSourcePassword();
            if (dlg.ShowDialog() == DialogResult.OK)
                _DataSourceReferencePassword = dlg.PassPhrase;

            return _DataSourceReferencePassword;
        }

        private void menuResources_Click(object sender, EventArgs e)
        {
            if (this.rdlViewer.PageCurrent > 1)
                rdlViewer.PageCurrent -= 1; 


            //MessageBox.Show("test");
            return;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.rdlViewer = new fyiReporting.RdlViewer.RdlViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bPageUp = new System.Windows.Forms.Button();
            this.chkFind = new System.Windows.Forms.CheckBox();
            this.bOpen = new System.Windows.Forms.Button();
            this.tbParms = new System.Windows.Forms.TextBox();
            this.bSetParm = new System.Windows.Forms.Button();
            this.bParameters = new System.Windows.Forms.Button();
            this.bEnumerable = new System.Windows.Forms.Button();
            this.bDataSource = new System.Windows.Forms.Button();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.bIDataReader = new System.Windows.Forms.Button();
            this.bXMLData = new System.Windows.Forms.Button();
            this.bManualDataTable = new System.Windows.Forms.Button();
            this.bSetReport = new System.Windows.Forms.Button();
            this.bPageDown = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdlViewer
            // 
            this.rdlViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.rdlViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdlViewer.Folder = null;
            this.rdlViewer.HighlightAll = false;
            this.rdlViewer.HighlightAllColor = System.Drawing.Color.Fuchsia;
            this.rdlViewer.HighlightCaseSensitive = false;
            this.rdlViewer.HighlightItemColor = System.Drawing.Color.Aqua;
            this.rdlViewer.HighlightPageItem = null;
            this.rdlViewer.HighlightText = null;
            this.rdlViewer.Location = new System.Drawing.Point(0, 0);
            this.rdlViewer.Name = "rdlViewer";
            this.rdlViewer.PageCurrent = 1;
            this.rdlViewer.Parameters = null;
            this.rdlViewer.ReportName = null;
            this.rdlViewer.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous;
            this.rdlViewer.ShowFindPanel = false;
            this.rdlViewer.ShowParameterPanel = false;
            this.rdlViewer.Size = new System.Drawing.Size(768, 362);
            this.rdlViewer.SourceFile = null;
            this.rdlViewer.SourceRdl = null;
            this.rdlViewer.TabIndex = 0;
            this.rdlViewer.Text = "rdlViewer1";
            this.rdlViewer.UseTrueMargins = true;
            this.rdlViewer.Zoom = 0.8913237F;
            this.rdlViewer.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
            this.rdlViewer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rdlViewer_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bPageDown);
            this.panel1.Controls.Add(this.bPageUp);
            this.panel1.Controls.Add(this.chkFind);
            this.panel1.Controls.Add(this.bOpen);
            this.panel1.Controls.Add(this.tbParms);
            this.panel1.Controls.Add(this.bSetParm);
            this.panel1.Controls.Add(this.bParameters);
            this.panel1.Controls.Add(this.bEnumerable);
            this.panel1.Controls.Add(this.bDataSource);
            this.panel1.Controls.Add(this.tbConnectionString);
            this.panel1.Controls.Add(this.bIDataReader);
            this.panel1.Controls.Add(this.bXMLData);
            this.panel1.Controls.Add(this.bManualDataTable);
            this.panel1.Controls.Add(this.bSetReport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 362);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(768, 65);
            this.panel1.TabIndex = 2;
            // 
            // bPageUp
            // 
            this.bPageUp.Location = new System.Drawing.Point(691, 9);
            this.bPageUp.Name = "bPageUp";
            this.bPageUp.Size = new System.Drawing.Size(20, 23);
            this.bPageUp.TabIndex = 13;
            this.bPageUp.Text = "<";
            this.bPageUp.UseVisualStyleBackColor = true;
            this.bPageUp.Click += new System.EventHandler(this.bPageUp_Click);
            // 
            // chkFind
            // 
            this.chkFind.AutoSize = true;
            this.chkFind.Location = new System.Drawing.Point(590, 37);
            this.chkFind.Name = "chkFind";
            this.chkFind.Size = new System.Drawing.Size(46, 17);
            this.chkFind.TabIndex = 12;
            this.chkFind.Text = "Find";
            this.chkFind.UseVisualStyleBackColor = true;
            this.chkFind.CheckedChanged += new System.EventHandler(this.chkFind_CheckedChanged);
            // 
            // bOpen
            // 
            this.bOpen.Location = new System.Drawing.Point(590, 8);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(57, 23);
            this.bOpen.TabIndex = 11;
            this.bOpen.Text = "Open";
            this.bOpen.UseVisualStyleBackColor = true;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // tbParms
            // 
            this.tbParms.Location = new System.Drawing.Point(410, 37);
            this.tbParms.Name = "tbParms";
            this.tbParms.Size = new System.Drawing.Size(166, 20);
            this.tbParms.TabIndex = 10;
            this.tbParms.Text = "Name=%";
            // 
            // bSetParm
            // 
            this.bSetParm.Location = new System.Drawing.Point(499, 9);
            this.bSetParm.Name = "bSetParm";
            this.bSetParm.Size = new System.Drawing.Size(77, 23);
            this.bSetParm.TabIndex = 9;
            this.bSetParm.Text = "Set Parm";
            this.bSetParm.Click += new System.EventHandler(this.bSetParm_Click);
            // 
            // bParameters
            // 
            this.bParameters.Location = new System.Drawing.Point(401, 8);
            this.bParameters.Name = "bParameters";
            this.bParameters.Size = new System.Drawing.Size(93, 23);
            this.bParameters.TabIndex = 8;
            this.bParameters.Text = "Parameter Rep";
            this.bParameters.Click += new System.EventHandler(this.bParameters_Click);
            // 
            // bEnumerable
            // 
            this.bEnumerable.Location = new System.Drawing.Point(310, 7);
            this.bEnumerable.Name = "bEnumerable";
            this.bEnumerable.Size = new System.Drawing.Size(77, 23);
            this.bEnumerable.TabIndex = 7;
            this.bEnumerable.Text = "IEnumerable";
            this.bEnumerable.Click += new System.EventHandler(this.bEnumerable_Click);
            // 
            // bDataSource
            // 
            this.bDataSource.Location = new System.Drawing.Point(13, 38);
            this.bDataSource.Name = "bDataSource";
            this.bDataSource.Size = new System.Drawing.Size(75, 23);
            this.bDataSource.TabIndex = 6;
            this.bDataSource.Text = "Data Source";
            this.bDataSource.Click += new System.EventHandler(this.bDataSource_Click);
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(188, 38);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(199, 20);
            this.tbConnectionString.TabIndex = 5;
            this.tbConnectionString.Text = "Server=(local)\\VSDotNet;DataBase=Northwind;Integrated Security=SSPI;Connect Timeo" +
                "ut=5";
            // 
            // bIDataReader
            // 
            this.bIDataReader.Location = new System.Drawing.Point(102, 38);
            this.bIDataReader.Name = "bIDataReader";
            this.bIDataReader.Size = new System.Drawing.Size(80, 23);
            this.bIDataReader.TabIndex = 4;
            this.bIDataReader.Text = "IDataReader";
            this.bIDataReader.Click += new System.EventHandler(this.bIDataReader_Click);
            // 
            // bXMLData
            // 
            this.bXMLData.Location = new System.Drawing.Point(223, 7);
            this.bXMLData.Name = "bXMLData";
            this.bXMLData.Size = new System.Drawing.Size(75, 23);
            this.bXMLData.TabIndex = 3;
            this.bXMLData.Text = "XML Data";
            this.bXMLData.Click += new System.EventHandler(this.bXMLData_Click);
            // 
            // bManualDataTable
            // 
            this.bManualDataTable.Location = new System.Drawing.Point(101, 7);
            this.bManualDataTable.Name = "bManualDataTable";
            this.bManualDataTable.Size = new System.Drawing.Size(110, 23);
            this.bManualDataTable.TabIndex = 2;
            this.bManualDataTable.Text = "Manual Data Table";
            this.bManualDataTable.Click += new System.EventHandler(this.ManualDataTable_Click);
            // 
            // bSetReport
            // 
            this.bSetReport.Location = new System.Drawing.Point(14, 7);
            this.bSetReport.Name = "bSetReport";
            this.bSetReport.Size = new System.Drawing.Size(75, 23);
            this.bSetReport.TabIndex = 1;
            this.bSetReport.Text = "Set Report";
            this.bSetReport.Click += new System.EventHandler(this.bSetReport_Click);
            // 
            // bPageDown
            // 
            this.bPageDown.Location = new System.Drawing.Point(717, 9);
            this.bPageDown.Name = "bPageDown";
            this.bPageDown.Size = new System.Drawing.Size(20, 23);
            this.bPageDown.TabIndex = 14;
            this.bPageDown.Text = ">";
            this.bPageDown.UseVisualStyleBackColor = true;
            this.bPageDown.Click += new System.EventHandler(this.bPageDown_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(768, 427);
            this.Controls.Add(this.rdlViewer);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Data Tests";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void bSetReport_Click(object sender, System.EventArgs e)
		{
			this.rdlViewer.SourceRdl = @"<?xml version='1.0' encoding='utf-8'?>
<Report>
  <DataElementStyle>ElementNormal</DataElementStyle>
  <RightMargin>0.25in</RightMargin>
  <Body>
    <ReportItems>
      <List Name='list1'>
        <DataInstanceName>Row</DataInstanceName>
		<NoRows>There are no rows</NoRows>
        <Style />
        <DataSetName>Data</DataSetName>
        <ReportItems>
          <Textbox Name='Phone'>
            <Style>
              <PaddingLeft>2pt</PaddingLeft>
              <PaddingBottom>2pt</PaddingBottom>
              <PaddingTop>2pt</PaddingTop>
              <PaddingRight>2pt</PaddingRight>
            </Style>
            <ZIndex>1</ZIndex>
            <CanGrow>false</CanGrow>
            <Value>=Fields!Phone.Value</Value>
            <Left>1.5in</Left>
            <Width>1in</Width>
            <Height>14pt</Height>
          </Textbox>
          <Textbox Name='ContactName'>
            <Style>
              <PaddingLeft>2pt</PaddingLeft>
              <PaddingBottom>2pt</PaddingBottom>
              <PaddingTop>2pt</PaddingTop>
              <PaddingRight>2pt</PaddingRight>
            </Style>
            <Width>1.5in</Width>
            <Height>14pt</Height>
            <CanGrow>false</CanGrow>
            <Value>=Fields!ContactName.Value</Value>
          </Textbox>
        </ReportItems>
      </List>
    </ReportItems>
    <Style />
    <Height>0.25in</Height>
    <ColumnSpacing>0.25in</ColumnSpacing>
    <Columns>3</Columns>
  </Body>
  <TopMargin>0.5in</TopMargin>
  <DataSources>
    <DataSource Name='DS1'>
      <ConnectionProperties>
        <DataProvider>SQL</DataProvider>
		<ConnectString></ConnectString>
      </ConnectionProperties>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name='Data'>
      <Query>
        <DataSourceName>DS1</DataSourceName>
        <CommandText>SELECT ContactName, Phone FROM Customers ORDER BY 1</CommandText>
      </Query>
      <Fields>
        <Field Name='ContactName'>
          <DataField>ContactName</DataField>
          <TypeName>String</TypeName>
        </Field>
        <Field Name='Phone'>
          <DataField>Phone</DataField>
          <TypeName>String</TypeName>
        </Field>
      </Fields>
    </DataSet>
  </DataSets>
  <LeftMargin>0.25in</LeftMargin>
  <BottomMargin>0.5in</BottomMargin>
  <Width>2.5in</Width>
  <PageHeader>
    <Height>0pt</Height>
  </PageHeader>
  <PageFooter>
    <Height>0pt</Height>
  </PageFooter>
</Report>";
		}

		private void ManualDataTable_Click(object sender, System.EventArgs e)
		{
			if (this.rdlViewer.SourceRdl == null)
			{
				MessageBox.Show("Hit the Set Report button first!");
				return;
			}

			// Create a DataTable and manually populate it.
			DataTable dt = new DataTable();
			// The column names need to match the Field DataField element
			dt.Columns.Add(new DataColumn("ContactName", typeof(string))); 
			dt.Columns.Add(new DataColumn("Phone", typeof(string)));
			// Create some data and add it to the data table
			string[] rowValues = new string[2];
			rowValues[0] = "Lily";
			rowValues[1] = "617-555-1234";
			dt.Rows.Add(rowValues);
			rowValues[0] = "Daisy";
			rowValues[1] = "617-555-8324";
			dt.Rows.Add(rowValues);

			// Tell the report to use the data
			Report rpt = this.rdlViewer.Report;			// Get the report
			fyiReporting.RDL.DataSet ds = rpt.DataSets["Data"];		// get the data set
			ds.SetData(dt);					// set the data for the dataset
			rdlViewer.Rebuild();			// force report to get rebuilt
		}

		private void bXMLData_Click(object sender, System.EventArgs e)
		{
			if (this.rdlViewer.SourceRdl == null)
			{
				MessageBox.Show("Hit the Set Report button first!");
				return;
			}

			string xmlData = @"<?xml version='1.0' encoding='UTF-8'?>
<Rows>
<Row><ContactName>Alejandra Camino</ContactName><Phone>(91) 745 6200</Phone></Row>
<Row><ContactName>Alexander Feuer</ContactName><Phone>0342-023176</Phone></Row>
<Row><ContactName>Ana Trujillo</ContactName><Phone>(5) 555-4729</Phone></Row>
<Row><ContactName>Anabela Domingues</ContactName><Phone>(11) 555-2167</Phone></Row>
<Row><ContactName>Andr&#233; Fonseca</ContactName><Phone>(11) 555-9482</Phone></Row>
</Rows>";

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlData);

			// Tell the report to use the data
			Report rpt = this.rdlViewer.Report;			// Get the report
			fyiReporting.RDL.DataSet ds = rpt.DataSets["Data"];		// get the data set
			ds.SetData(doc);
			rdlViewer.Rebuild();			// force report to get rebuilt
		}

		private void bIDataReader_Click(object sender, System.EventArgs e)
		{
			if (this.rdlViewer.SourceRdl == null)
			{
				MessageBox.Show("Hit the Set Report button first!");
				return;
			}
			Cursor saveCursor=Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			IDbConnection cnSQL=null;
			IDbCommand cmSQL=null;
			IDataReader dr=null;	   
			try
			{
				cnSQL = new SqlConnection(this.tbConnectionString.Text);
				cnSQL.Open();
				cmSQL = new SqlCommand("SELECT ContactName, Phone FROM Customers ORDER BY 1", 
										(SqlConnection) cnSQL);
				dr = cmSQL.ExecuteReader(CommandBehavior.SequentialAccess);

				// Tell the report to use the data
				Report rpt = this.rdlViewer.Report;			// Get the report
				fyiReporting.RDL.DataSet ds = rpt.DataSets["Data"];		// get the data set
				ds.SetData(dr);
				rdlViewer.Rebuild();	// force report to get rebuilt
			}
			catch (SqlException sqle)
			{
				MessageBox.Show(sqle.Message, "SQL Error");
			}
			catch (Exception ge)
			{
				MessageBox.Show(ge.Message, "Error");
			}
			finally
			{
				if (cnSQL != null)
				{
					cnSQL.Close();
					cnSQL.Dispose();
					if (cmSQL != null)
					{
						cmSQL.Dispose();
						if (dr != null)
							dr.Close();
					}
				}
				Cursor.Current = saveCursor;
			}
		}

		private void bDataSource_Click(object sender, System.EventArgs e)
		{
			if (this.rdlViewer.SourceRdl == null)
			{
				MessageBox.Show("Hit the Set Report button first!");
				return;
			}
			Cursor saveCursor=Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			IDbConnection cnSQL=null;
			try
			{
				cnSQL = new SqlConnection(this.tbConnectionString.Text);
				cnSQL.Open();

				// Tell the report to use this connection
				Report rpt = this.rdlViewer.Report;			// Get the report
				fyiReporting.RDL.DataSource ds = rpt.DataSources["DS1"];		// get the data source
				ds.UserConnection = cnSQL;
				// reset call to set user data (if made)
				fyiReporting.RDL.DataSet dts = rpt.DataSets["Data"];		// get the data set
				dts.SetData((XmlDocument) null);		// this will clear out user data (if any); otherwise userdata would override

				rdlViewer.Rebuild();			// force report to get rebuilt
				ds.UserConnection = null;		// clear it out
			}
			catch (SqlException sqle)
			{
				MessageBox.Show(sqle.Message, "SQL Error");
			}
			catch (Exception ge)
			{
				MessageBox.Show(ge.Message, "Error");
			}
			finally
			{
				if (cnSQL != null)
				{
					cnSQL.Close();
					cnSQL.Dispose();
				}
				Cursor.Current = saveCursor;
			}
		}

		private void bEnumerable_Click(object sender, System.EventArgs e)
		{
			if (this.rdlViewer.SourceRdl == null)
			{
				MessageBox.Show("Hit the Set Report button first!");
				return;
			}
			try
			{
				ArrayList ar = new ArrayList();
				ar.Add(new Contact("Contact Object", "(555) 712 1234"));
				ar.Add(new Contact("Alejandra Camino", "(91) 745 6200"));
				ar.Add(new Contact("Alexander Feuer", "0342-023176"));
				ar.Add(new Contact("Ana Trujillo", "(5) 555-4729"));
				ar.Add(new Contact("Anabela Domingues", "(11) 555-2167"));

				// Tell the report to use this IEnumerable
				Report rpt = this.rdlViewer.Report;			// Get the report
				// reset call to set user data (if made)
				fyiReporting.RDL.DataSet dts = rpt.DataSets["Data"];		// get the data set
				dts.SetData(ar);		// reset the data

				rdlViewer.Rebuild();			// force report to get rebuilt
			}
			catch (Exception ge)
			{
				MessageBox.Show(ge.Message, "Error");
			}
		
		}

		private void bParameters_Click(object sender, System.EventArgs e)
		{
			this.rdlViewer.SourceRdl = @"<?xml version='1.0' encoding='utf-8'?>
<Report>
  <DataElementStyle>ElementNormal</DataElementStyle>
  <RightMargin>0.25in</RightMargin>
  <ReportParameters>
	<ReportParameter Name='Name'>
		<DataType>String</DataType>
		<DefaultValue>
			<Values><Value>%</Value></Values>
		</DefaultValue>
	</ReportParameter>
  </ReportParameters>
  <Body>
    <ReportItems>
          <Textbox Name='ParmTB'>
            <Style>
              <PaddingLeft>2pt</PaddingLeft>
              <PaddingBottom>2pt</PaddingBottom>
              <PaddingTop>2pt</PaddingTop>
              <PaddingRight>2pt</PaddingRight>
            </Style>
            <ZIndex>1</ZIndex>
            <CanGrow>false</CanGrow>
            <Value>=Parameters!Name.Value</Value>
            <Left>0in</Left>
            <Width>2in</Width>
            <Height>14pt</Height>
          </Textbox>
      <List Name='list1'>
        <DataInstanceName>Row</DataInstanceName>
		<NoRows>There are no rows</NoRows>
        <Style />
        <DataSetName>Data</DataSetName>
        <ReportItems>
          <Textbox Name='Phone'>
            <Style>
              <PaddingLeft>2pt</PaddingLeft>
              <PaddingBottom>2pt</PaddingBottom>
              <PaddingTop>2pt</PaddingTop>
              <PaddingRight>2pt</PaddingRight>
            </Style>
            <ZIndex>1</ZIndex>
            <CanGrow>false</CanGrow>
            <Value>=Fields!Phone.Value</Value>
            <Left>1.5in</Left>
            <Width>1in</Width>
            <Height>14pt</Height>
          </Textbox>
          <Textbox Name='ContactName'>
            <Style>
              <PaddingLeft>2pt</PaddingLeft>
              <PaddingBottom>2pt</PaddingBottom>
              <PaddingTop>2pt</PaddingTop>
              <PaddingRight>2pt</PaddingRight>
            </Style>
            <Width>1.5in</Width>
            <Height>14pt</Height>
            <CanGrow>false</CanGrow>
            <Value>=Fields!ContactName.Value</Value>
          </Textbox>
        </ReportItems>
        <Top>1in</Top>
      </List>
    </ReportItems>
    <Style />
    <Height>0.25in</Height>
    <ColumnSpacing>0.25in</ColumnSpacing>
    <Columns>3</Columns>
  </Body>
  <TopMargin>0.5in</TopMargin>
  <DataSources>
    <DataSource Name='DS1'>
      <ConnectionProperties>
        <DataProvider>SQL</DataProvider>
		<ConnectString>Server=(local)\VSDotNet;DataBase=Northwind;Integrated Security=SSPI;Connect Timeout=5</ConnectString>
      </ConnectionProperties>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name='Data'>
      <Query>
        <DataSourceName>DS1</DataSourceName>
        <CommandText>SELECT ContactName, Phone FROM Customers WHERE ContactName LIKE @Contact ORDER BY 1</CommandText>
		<QueryParameters>
			<QueryParameter Name='Contact'>
				<Value>=Parameters!Name.Value</Value>
			</QueryParameter>
		</QueryParameters>
      </Query>
      <Fields>
        <Field Name='ContactName'>
          <DataField>ContactName</DataField>
          <TypeName>String</TypeName>
        </Field>
        <Field Name='Phone'>
          <DataField>Phone</DataField>
          <TypeName>String</TypeName>
        </Field>
      </Fields>
    </DataSet>
  </DataSets>
  <LeftMargin>0.25in</LeftMargin>
  <BottomMargin>0.5in</BottomMargin>
  <Width>2.5in</Width>
  <PageHeader>
    <Height>0pt</Height>
  </PageHeader>
  <PageFooter>
    <Height>0pt</Height>
  </PageFooter>
</Report>";
           // this.rdlViewer.ShowParameterPanel = true;
            this.rdlViewer.Parameters = this.tbParms.Text;
            rdlViewer.Rebuild();			// force report to get rebuilt

		}

		private void bSetParm_Click(object sender, System.EventArgs e)
		{
			this.rdlViewer.Parameters = this.tbParms.Text;
			this.rdlViewer.Rebuild();
		}

        private void bOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Report files (*.rdl)|*.rdl|" +
                "All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                this.rdlViewer.SourceFile = ofd.FileName;
            }

        }

        private void rdlViewer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int) Keys.Escape)
                this.Close();
        }

        private void chkFind_CheckedChanged(object sender, EventArgs e)
        {
            rdlViewer.ShowFindPanel = chkFind.Checked;
        }

        private void bPageUp_Click(object sender, EventArgs e)
        {
            //if (rdlViewer.ZoomMode != ZoomEnum.FitPage)
            //    rdlViewer.ZoomMode = ZoomEnum.FitPage;
            if (this.rdlViewer.PageCurrent > 1)
                rdlViewer.PageCurrent -= 1; 

        }

        private void bPageDown_Click(object sender, EventArgs e)
        {
            if (rdlViewer.PageCurrent < rdlViewer.PageCount)
                rdlViewer.PageCurrent += 1; 
        }
	}
	class Contact
	{
		public string ContactName;
		string _Phone;

		public Contact (string contact, string phone)
		{
			ContactName = contact;
			_Phone = phone;
		}

        public string Phone
        {
            get { return _Phone; }
        }
	}
}
