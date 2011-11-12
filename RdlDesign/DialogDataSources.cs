using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using System.IO;
using fyiReporting.RDL;


namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for DialogDataSourceRef.
	/// </summary>
	internal class DialogDataSources : System.Windows.Forms.Form
	{
		DesignXmlDraw _Draw;
        string _FileName;           // file name of open file; used to obtain directory
		private System.Windows.Forms.TextBox tbFilename;
		private System.Windows.Forms.Button bGetFilename;
		private System.Windows.Forms.ComboBox cbDataProvider;
		private System.Windows.Forms.TextBox tbConnection;
		private System.Windows.Forms.CheckBox ckbIntSecurity;
		private System.Windows.Forms.TextBox tbPrompt;
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Button bTestConnection;
		private System.Windows.Forms.ListBox lbDataSources;
		private System.Windows.Forms.Button bRemove;
		private System.Windows.Forms.Button bAdd;
		private System.Windows.Forms.CheckBox chkSharedDataSource;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lDataProvider;
		private System.Windows.Forms.Label lConnectionString;
		private System.Windows.Forms.Label lPrompt;
		private System.Windows.Forms.TextBox tbDSName;
		private System.Windows.Forms.Button bExprConnect;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal DialogDataSources(string filename, DesignXmlDraw draw)
		{
			_Draw = draw;
            _FileName = filename;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			InitValues();
		}

		private void InitValues()
		{
			// Populate the DataProviders
			cbDataProvider.Items.Clear();
			string[] items = RdlEngineConfig.GetProviders();
			cbDataProvider.Items.AddRange(items);

			//
			// Obtain the existing DataSets info
			//
			XmlNode rNode = _Draw.GetReportNode();
			XmlNode dsNode = _Draw.GetNamedChildNode(rNode, "DataSources");
			if (dsNode == null)
				return;
			foreach (XmlNode dNode in dsNode)
			{	
				if (dNode.Name != "DataSource")
					continue;
				XmlAttribute nAttr = dNode.Attributes["Name"];
				if (nAttr == null)	// shouldn't really happen
					continue;

				DataSourceValues dsv = new DataSourceValues(nAttr.Value);
				dsv.Node = dNode;

				dsv.DataSourceReference = _Draw.GetElementValue(dNode, "DataSourceReference", null);
				if (dsv.DataSourceReference == null)
				{	// this is not a data source reference
					dsv.bDataSourceReference = false;
					dsv.DataSourceReference = "";

					XmlNode cpNode = DesignXmlDraw.FindNextInHierarchy(dNode, "ConnectionProperties", "ConnectString");
					dsv.ConnectionString = cpNode == null? "": cpNode.InnerText;
				
					XmlNode datap = DesignXmlDraw.FindNextInHierarchy(dNode, "ConnectionProperties", "DataProvider");
					dsv.DataProvider = datap == null? "": datap.InnerText;
				
					XmlNode p = DesignXmlDraw.FindNextInHierarchy(dNode, "ConnectionProperties", "Prompt");
					dsv.Prompt = p == null? "": p.InnerText;
				}
				else
				{	// we have a data source reference
					dsv.bDataSourceReference = true;
					dsv.ConnectionString = "";
					dsv.DataProvider = "";
					dsv.Prompt = "";
				}

				this.lbDataSources.Items.Add(dsv);
			}
			if (lbDataSources.Items.Count > 0)
				lbDataSources.SelectedIndex = 0;
			else
				this.bOK.Enabled = false;		
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
			this.tbFilename = new System.Windows.Forms.TextBox();
			this.bGetFilename = new System.Windows.Forms.Button();
			this.lDataProvider = new System.Windows.Forms.Label();
			this.cbDataProvider = new System.Windows.Forms.ComboBox();
			this.lConnectionString = new System.Windows.Forms.Label();
			this.tbConnection = new System.Windows.Forms.TextBox();
			this.ckbIntSecurity = new System.Windows.Forms.CheckBox();
			this.lPrompt = new System.Windows.Forms.Label();
			this.tbPrompt = new System.Windows.Forms.TextBox();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.bTestConnection = new System.Windows.Forms.Button();
			this.lbDataSources = new System.Windows.Forms.ListBox();
			this.bRemove = new System.Windows.Forms.Button();
			this.bAdd = new System.Windows.Forms.Button();
			this.chkSharedDataSource = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbDSName = new System.Windows.Forms.TextBox();
			this.bExprConnect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbFilename
			// 
			this.tbFilename.Location = new System.Drawing.Point(192, 112);
			this.tbFilename.Name = "tbFilename";
			this.tbFilename.Size = new System.Drawing.Size(216, 20);
			this.tbFilename.TabIndex = 2;
			this.tbFilename.Text = "";
			this.tbFilename.TextChanged += new System.EventHandler(this.tbFilename_TextChanged);
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
			// lDataProvider
			// 
			this.lDataProvider.Location = new System.Drawing.Point(8, 152);
			this.lDataProvider.Name = "lDataProvider";
			this.lDataProvider.Size = new System.Drawing.Size(80, 23);
			this.lDataProvider.TabIndex = 7;
			this.lDataProvider.Text = "Data provider";
			// 
			// cbDataProvider
			// 
			this.cbDataProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataProvider.Items.AddRange(new object[] {
																"SQL",
																"ODBC",
																"OLEDB"});
			this.cbDataProvider.Location = new System.Drawing.Point(96, 152);
			this.cbDataProvider.Name = "cbDataProvider";
			this.cbDataProvider.Size = new System.Drawing.Size(144, 21);
			this.cbDataProvider.TabIndex = 4;
			this.cbDataProvider.SelectedIndexChanged += new System.EventHandler(this.cbDataProvider_SelectedIndexChanged);
			// 
			// lConnectionString
			// 
			this.lConnectionString.Location = new System.Drawing.Point(8, 192);
			this.lConnectionString.Name = "lConnectionString";
			this.lConnectionString.Size = new System.Drawing.Size(100, 16);
			this.lConnectionString.TabIndex = 10;
			this.lConnectionString.Text = "Connection string";
			// 
			// tbConnection
			// 
			this.tbConnection.Location = new System.Drawing.Point(16, 216);
			this.tbConnection.Multiline = true;
			this.tbConnection.Name = "tbConnection";
			this.tbConnection.Size = new System.Drawing.Size(424, 40);
			this.tbConnection.TabIndex = 6;
			this.tbConnection.Text = "";
			this.tbConnection.TextChanged += new System.EventHandler(this.tbConnection_TextChanged);
			// 
			// ckbIntSecurity
			// 
			this.ckbIntSecurity.Location = new System.Drawing.Point(280, 152);
			this.ckbIntSecurity.Name = "ckbIntSecurity";
			this.ckbIntSecurity.Size = new System.Drawing.Size(144, 24);
			this.ckbIntSecurity.TabIndex = 5;
			this.ckbIntSecurity.Text = "Use integrated security";
			this.ckbIntSecurity.CheckedChanged += new System.EventHandler(this.ckbIntSecurity_CheckedChanged);
			// 
			// lPrompt
			// 
			this.lPrompt.Location = new System.Drawing.Point(8, 272);
			this.lPrompt.Name = "lPrompt";
			this.lPrompt.Size = new System.Drawing.Size(432, 16);
			this.lPrompt.TabIndex = 12;
			this.lPrompt.Text = "(Optional) Enter the prompt displayed when asking for database credentials ";
			// 
			// tbPrompt
			// 
			this.tbPrompt.Location = new System.Drawing.Point(16, 296);
			this.tbPrompt.Name = "tbPrompt";
			this.tbPrompt.Size = new System.Drawing.Size(424, 20);
			this.tbPrompt.TabIndex = 7;
			this.tbPrompt.Text = "";
			this.tbPrompt.TextChanged += new System.EventHandler(this.tbPrompt_TextChanged);
			// 
			// bOK
			// 
			this.bOK.Location = new System.Drawing.Point(272, 344);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 9;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.CausesValidation = false;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(368, 344);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 10;
			this.bCancel.Text = "Cancel";
			// 
			// bTestConnection
			// 
			this.bTestConnection.Location = new System.Drawing.Point(16, 344);
			this.bTestConnection.Name = "bTestConnection";
			this.bTestConnection.Size = new System.Drawing.Size(96, 23);
			this.bTestConnection.TabIndex = 8;
			this.bTestConnection.Text = "Test Connection";
			this.bTestConnection.Click += new System.EventHandler(this.bTestConnection_Click);
			// 
			// lbDataSources
			// 
			this.lbDataSources.Location = new System.Drawing.Point(16, 8);
			this.lbDataSources.Name = "lbDataSources";
			this.lbDataSources.Size = new System.Drawing.Size(120, 82);
			this.lbDataSources.TabIndex = 11;
			this.lbDataSources.SelectedIndexChanged += new System.EventHandler(this.lbDataSources_SelectedIndexChanged);
			// 
			// bRemove
			// 
			this.bRemove.Location = new System.Drawing.Point(144, 40);
			this.bRemove.Name = "bRemove";
			this.bRemove.Size = new System.Drawing.Size(56, 23);
			this.bRemove.TabIndex = 20;
			this.bRemove.Text = "Remove";
			this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
			// 
			// bAdd
			// 
			this.bAdd.Location = new System.Drawing.Point(144, 8);
			this.bAdd.Name = "bAdd";
			this.bAdd.Size = new System.Drawing.Size(56, 23);
			this.bAdd.TabIndex = 19;
			this.bAdd.Text = "Add";
			this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
			// 
			// chkSharedDataSource
			// 
			this.chkSharedDataSource.Location = new System.Drawing.Point(8, 112);
			this.chkSharedDataSource.Name = "chkSharedDataSource";
			this.chkSharedDataSource.Size = new System.Drawing.Size(184, 16);
			this.chkSharedDataSource.TabIndex = 1;
			this.chkSharedDataSource.Text = "Shared Data Source Reference";
			this.chkSharedDataSource.CheckedChanged += new System.EventHandler(this.chkSharedDataSource_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(216, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 23);
			this.label1.TabIndex = 22;
			this.label1.Text = "Data Source Name";
			// 
			// tbDSName
			// 
			this.tbDSName.Location = new System.Drawing.Point(216, 24);
			this.tbDSName.Name = "tbDSName";
			this.tbDSName.Size = new System.Drawing.Size(216, 20);
			this.tbDSName.TabIndex = 0;
			this.tbDSName.Text = "";
			this.tbDSName.Validating += new System.ComponentModel.CancelEventHandler(this.tbDSName_Validating);
			this.tbDSName.TextChanged += new System.EventHandler(this.tbDSName_TextChanged);
			// 
			// bExprConnect
			// 
			this.bExprConnect.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bExprConnect.Location = new System.Drawing.Point(416, 192);
			this.bExprConnect.Name = "bExprConnect";
			this.bExprConnect.Size = new System.Drawing.Size(22, 16);
			this.bExprConnect.TabIndex = 23;
			this.bExprConnect.Tag = "pright";
			this.bExprConnect.Text = "fx";
			this.bExprConnect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bExprConnect.Click += new System.EventHandler(this.bExprConnect_Click);
			// 
			// DialogDataSources
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(456, 374);
			this.Controls.Add(this.bExprConnect);
			this.Controls.Add(this.tbDSName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chkSharedDataSource);
			this.Controls.Add(this.bRemove);
			this.Controls.Add(this.bAdd);
			this.Controls.Add(this.lbDataSources);
			this.Controls.Add(this.bTestConnection);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.tbPrompt);
			this.Controls.Add(this.lPrompt);
			this.Controls.Add(this.ckbIntSecurity);
			this.Controls.Add(this.tbConnection);
			this.Controls.Add(this.lConnectionString);
			this.Controls.Add(this.cbDataProvider);
			this.Controls.Add(this.lDataProvider);
			this.Controls.Add(this.bGetFilename);
			this.Controls.Add(this.tbFilename);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogDataSources";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DataSources in Report";
			this.ResumeLayout(false);

		}
		#endregion
		public void Apply()
		{
			XmlNode rNode = _Draw.GetReportNode();
			_Draw.RemoveElement(rNode, "DataSources");	// remove old DataSources
			if (this.lbDataSources.Items.Count <= 0)
				return;			// nothing in list?  all done

			XmlNode dsNode = _Draw.SetElement(rNode, "DataSources", null);
			foreach (DataSourceValues dsv in lbDataSources.Items)
			{
				if (dsv.Name == null || dsv.Name.Length <= 0)
					continue;					// shouldn't really happen
				XmlNode dNode = _Draw.CreateElement(dsNode, "DataSource", null);

				// Create the name attribute
				_Draw.SetElementAttribute(dNode, "Name", dsv.Name);

				if (dsv.bDataSourceReference)
				{
					_Draw.SetElement(dNode, "DataSourceReference", dsv.DataSourceReference);
					continue;
				}
				// must fill out the connection properties
				XmlNode cNode = _Draw.CreateElement(dNode, "ConnectionProperties", null);
				_Draw.SetElement(cNode, "DataProvider", dsv.DataProvider);
				_Draw.SetElement(cNode, "ConnectString", dsv.ConnectionString);
				_Draw.SetElement(cNode, "IntegratedSecurity", dsv.IntegratedSecurity? "true": "false");
				if (dsv.Prompt != null && dsv.Prompt.Length > 0)
					_Draw.SetElement(cNode, "Prompt", dsv.Prompt);
			}
		}

		private void bGetFilename_Click(object sender, System.EventArgs e)
		{ 
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Data source reference files (*.dsr)|*.dsr" +
                "|All files (*.*)|*.*";
			ofd.FilterIndex = 1;
			if (tbFilename.Text.Length > 0)
				ofd.FileName = tbFilename.Text;
			else
				ofd.FileName = "*.dsr";

			ofd.Title = "Specify Data Source Reference File Name";
			ofd.DefaultExt = "dsr";
			ofd.AddExtension = true;

            try
            {
                if (_FileName != null)
                    ofd.InitialDirectory = Path.GetDirectoryName(_FileName);
            }
            catch
            {
            }
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string dsr = DesignerUtility.RelativePathTo(
                            Path.GetDirectoryName(_FileName), Path.GetDirectoryName(ofd.FileName));

                        string f = Path.GetFileNameWithoutExtension(ofd.FileName);

                        tbFilename.Text = dsr == "" ? f : dsr + Path.DirectorySeparatorChar + f;
                    }
                    catch
                    {
                        tbFilename.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                    }
                }
            }
            finally
            {
                ofd.Dispose();
            }
		}

		private void tbFilename_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			dsv.DataSourceReference = tbFilename.Text;
			return;
		}

		private void bOK_Click(object sender, System.EventArgs e)
		{
			// Verify there are no duplicate names in the data sources
			Hashtable ht = new Hashtable(this.lbDataSources.Items.Count+1);
			foreach (DataSourceValues dsv in lbDataSources.Items)
			{
				if (dsv.Name == null || dsv.Name.Length == 0)
				{
					MessageBox.Show(this, "Name must be specified for all DataSources.", "Data Sources");
					return;
				}

				if (!ReportNames.IsNameValid(dsv.Name))
				{
					MessageBox.Show(this, 
						string.Format("Name '{0}' contains invalid characters.", dsv.Name), "Data Sources");
					return;
				}

				string name = (string) ht[dsv.Name];
				if (name != null)
				{
					MessageBox.Show(this, 
						string.Format("Each DataSource must have a unique name. '{0}' is repeated.", dsv.Name), "Data Sources");
					return;
				}
				ht.Add(dsv.Name, dsv.Name);
			}

			// apply the result
			Apply();
			DialogResult = DialogResult.OK;
		}

		private void bTestConnection_Click(object sender, System.EventArgs e)
		{
			if (DesignerUtility.TestConnection(this.cbDataProvider.Text, tbConnection.Text))
				MessageBox.Show("Connection succeeded!", "Test Connection");
		}

		private void tbDSName_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			if (dsv.Name == tbDSName.Text)
				return;

			dsv.Name = tbDSName.Text;
			// text doesn't change in listbox; force change by removing and re-adding item
			lbDataSources.BeginUpdate();
			lbDataSources.Items.RemoveAt(cur);
			lbDataSources.Items.Insert(cur, dsv);
			lbDataSources.SelectedIndex = cur;
			lbDataSources.EndUpdate();
		}

		private void chkSharedDataSource_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;
			dsv.bDataSourceReference = chkSharedDataSource.Checked;

			bool bEnableDataSourceRef = chkSharedDataSource.Checked;
			// shared data source fields
			tbFilename.Enabled = bEnableDataSourceRef;
			bGetFilename.Enabled = bEnableDataSourceRef;
			// in report data source
			cbDataProvider.Enabled = !bEnableDataSourceRef;
			tbConnection.Enabled = !bEnableDataSourceRef;
			ckbIntSecurity.Enabled = !bEnableDataSourceRef;
			tbPrompt.Enabled = !bEnableDataSourceRef;
			bTestConnection.Enabled = !bEnableDataSourceRef;
			lDataProvider.Enabled = !bEnableDataSourceRef;
			lConnectionString.Enabled = !bEnableDataSourceRef;
			lPrompt.Enabled = !bEnableDataSourceRef;
		}

		private void lbDataSources_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			tbDSName.Text = dsv.Name;
			cbDataProvider.Text = dsv.DataProvider;
			tbConnection.Text = dsv.ConnectionString;
			ckbIntSecurity.Checked = dsv.IntegratedSecurity;
			this.tbFilename.Text = dsv.DataSourceReference;
			tbPrompt.Text = dsv.Prompt;
			this.chkSharedDataSource.Checked = dsv.bDataSourceReference;
			chkSharedDataSource_CheckedChanged(this.chkSharedDataSource, e);	// force message
		}

		private void bAdd_Click(object sender, System.EventArgs e)
		{
			DataSourceValues dsv = new DataSourceValues("datasource");
			int cur = this.lbDataSources.Items.Add(dsv);

			lbDataSources.SelectedIndex = cur;

			this.tbDSName.Focus();
		}

		private void bRemove_Click(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;
			lbDataSources.Items.RemoveAt(cur);
			if (lbDataSources.Items.Count <= 0)
				return;
			cur--;
			if (cur < 0)
				cur = 0;
			lbDataSources.SelectedIndex = cur;
		}

		private void tbDSName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			if (!ReportNames.IsNameValid(tbDSName.Text))
			{
				e.Cancel = true;
				MessageBox.Show(this, 
					string.Format("Name '{0}' contains invalid characters.", tbDSName.Text), "Data Sources");
			}

		}

		private void tbConnection_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			dsv.ConnectionString = tbConnection.Text;
		}

		private void tbPrompt_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			dsv.Prompt = tbPrompt.Text;
		}

		private void cbDataProvider_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			dsv.DataProvider = cbDataProvider.Text;
		}

		private void ckbIntSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			int cur = lbDataSources.SelectedIndex;
			if (cur < 0)
				return;

			DataSourceValues dsv = lbDataSources.Items[cur] as DataSourceValues;
			if (dsv == null)
				return;

			dsv.IntegratedSecurity = ckbIntSecurity.Checked;
		}

		private void bExprConnect_Click(object sender, System.EventArgs e)
		{
			DialogExprEditor ee = new DialogExprEditor(_Draw, this.tbConnection.Text, null, false);
            try
            {
                DialogResult dr = ee.ShowDialog();
                if (dr == DialogResult.OK)
                    tbConnection.Text = ee.Expression;
            }
            finally
            {
                ee.Dispose();
            }
		}
	}

	class DataSourceValues
	{
		string _Name;
		bool _bDataSourceReference;
		string _DataSourceReference;
		string _DataProvider;
		string _ConnectionString;
		bool _IntegratedSecurity;
		string _Prompt;
		XmlNode _Node;

		internal DataSourceValues(string name)
		{
			_Name = name;
		}

		internal string Name
		{
			get {return _Name;}
			set {_Name = value;}
		}

		internal bool bDataSourceReference
		{
			get {return _bDataSourceReference;}
			set {_bDataSourceReference = value;}
		}

		internal string DataSourceReference
		{
			get {return _DataSourceReference;}
			set {_DataSourceReference = value;}
		}
	
		internal string DataProvider
		{
			get {return _DataProvider;}
			set {_DataProvider = value;}
		}

		internal string ConnectionString
		{
			get {return _ConnectionString;}
			set {_ConnectionString = value;}
		}

		internal bool IntegratedSecurity
		{
			get {return _IntegratedSecurity;}
			set {_IntegratedSecurity = value;}
		}

		internal string Prompt
		{
			get {return _Prompt;}
			set {_Prompt = value;}
		}

		internal XmlNode Node
		{
			get {return _Node;}
			set {_Node = value;}
		}

		override public string ToString()
		{
			return _Name;
		}
	}
}
