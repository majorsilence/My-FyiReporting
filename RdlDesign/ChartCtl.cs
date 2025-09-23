
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace Majorsilence.Reporting.RdlDesign
{
	/// <summary>
	/// Summary description for ChartCtl.
	/// </summary>
	internal class ChartCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		//AJM GJL 14082008 Adding more flags
        bool fChartType, fVector, fSubtype, fPalette, fRenderElement, fPercentWidth, ftooltip,ftooltipX;
        bool fNoRows, fDataSet, fPageBreakStart, fPageBreakEnd, tooltipYFormat, tooltipXFormat;
		bool fChartData;
        
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbChartType;
		private System.Windows.Forms.ComboBox cbSubType;
		private System.Windows.Forms.ComboBox cbPalette;
		private System.Windows.Forms.ComboBox cbRenderElement;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown tbPercentWidth;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbNoRows;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbDataSet;
		private System.Windows.Forms.CheckBox chkPageBreakStart;
		private System.Windows.Forms.CheckBox chkPageBreakEnd;
        private System.Windows.Forms.ComboBox cbChartData;
        private ComboBox cbDataLabel;
        private CheckBox chkDataLabel;
        private Button bDataLabelExpr;
		private System.Windows.Forms.Label lData1;
        private ComboBox cbChartData2;
        private Label lData2;
        private ComboBox cbChartData3;
        private Label lData3;
        private Button bDataExpr;
        private Button bDataExpr3;
        private Button bDataExpr2;
        private ComboBox cbVector;
        private Button btnVectorExp;
        private Label label8;
        private Button button1;
        private Button button2;
        private Button button3;
        private CheckBox chkToolTip;
        private CheckBox checkBox1;
        private TextBox txtYToolFormat;
        private TextBox txtXToolFormat;
        private Label label9;
        private Label label10;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal ChartCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
		{
			_ReportItems = ris;
			_Draw = dxDraw;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			XmlNode node = _ReportItems[0];

            string type = _Draw.GetElementValue(node, "Type", "Column");
            this.cbChartType.Text = type;
            type = type.ToLowerInvariant();

            lData2.Enabled = cbChartData2.Enabled = bDataExpr2.Enabled = (type == "scatter" || type == "bubble");
            lData3.Enabled = cbChartData3.Enabled = bDataExpr3.Enabled = (type == "bubble");
			//AJM GJL 14082008

            this.chkToolTip.Checked = _Draw.GetElementValue(node, "fyi:Tooltip", "false").ToLower() == "true" ? true : false;
            this.checkBox1.Checked = _Draw.GetElementValue(node, "fyi:TooltipX", "false").ToLower() == "true" ? true : false;

            this.txtXToolFormat.Text = _Draw.GetElementValue(node, "fyi:TooltipXFormat","");
            this.txtYToolFormat.Text = _Draw.GetElementValue(node, "fyi:TooltipYFormat","");

            this.cbVector.Text = _Draw.GetElementValue(node, "fyi:RenderAsVector", "False");
			this.cbSubType.Text = _Draw.GetElementValue(node, "Subtype", "Plain");
			this.cbPalette.Text = _Draw.GetElementValue(node, "Palette", "Default");
			this.cbRenderElement.Text = _Draw.GetElementValue(node, "ChartElementOutput", "Output");
			this.tbPercentWidth.Text = _Draw.GetElementValue(node, "PointWidth", "0");
			this.tbNoRows.Text = _Draw.GetElementValue(node, "NoRows", "");

			// Handle the dataset for this dataregion
			object[] dsNames = _Draw.DataSetNames;
			string defName="";
			if (dsNames != null && dsNames.Length > 0)
			{
				this.cbDataSet.Items.AddRange(_Draw.DataSetNames);
				defName = (string) dsNames[0];
			}
			cbDataSet.Text = _Draw.GetDataSetNameValue(node);
			if (_Draw.GetReportItemDataRegionContainer(node) != null)
				cbDataSet.Enabled = false;
			// page breaks
			this.chkPageBreakStart.Checked = _Draw.GetElementValue(node, "PageBreakAtStart", "false").ToLower() == "true"? true: false;
			this.chkPageBreakEnd.Checked = _Draw.GetElementValue(node, "PageBreakAtEnd", "false").ToLower() == "true"? true: false;

			// Chart data-- this is a simplification of what is possible (TODO) 
			string cdata=string.Empty;
            string cdata2 = string.Empty;
            string cdata3 = string.Empty;
//        <ChartData>
//          <ChartSeries>
//            <DataPoints>
//              <DataPoint>
            //<DataValues>
            //      <DataValue>
            //        <Value>=Sum(Fields!Sales.Value)</Value>
            //      </DataValue>
            //      <DataValue>
            //        <Value>=Fields!Year.Value</Value>         ----- only scatter and bubble
            //      </DataValue>
            //      <DataValue>
            //        <Value>=Sum(Fields!Sales.Value)</Value>   ----- only bubble
            //      </DataValue>
            //    </DataValues>
//                <DataLabel>
//                  <Style>
//                    <Format>c</Format>
//                  </Style>
//                </DataLabel>
//                <Marker />
//              </DataPoint>
//            </DataPoints>
//          </ChartSeries>
//        </ChartData>

          
            //Determine if we have a static series or not... We are not allowing this to be changed here. That decision is taken when creating the chart. 05122007GJL
            XmlNode ss = DesignXmlDraw.FindNextInHierarchy(node, "SeriesGroupings", "SeriesGrouping", "StaticSeries");
            bool StaticSeries = ss != null;    
                                  
            XmlNode dvs = DesignXmlDraw.FindNextInHierarchy(node,
                "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataValues");
            int iter = 0;
            XmlNode cnode;
            foreach (XmlNode dv in dvs.ChildNodes)
            {
                if (dv.Name != "DataValue")
                    continue;
                iter++;
                cnode = DesignXmlDraw.FindNextInHierarchy(dv, "Value");
                if (cnode == null)
                    continue;
                switch (iter)
                {
                    case 1:
                        cdata = cnode.InnerText;
                        break;
                    case 2:
                        cdata2 = cnode.InnerText;
                        break;
                    case 3:
                        cdata3 = cnode.InnerText;
                        break;
                    default:
                        break;
                }
            }
			this.cbChartData.Text = cdata;
            this.cbChartData2.Text = cdata2;
            this.cbChartData3.Text = cdata3;
 
            //If the chart doesn't have a static series then dont show the datalabel values. 05122007GJL
            if (!StaticSeries) 
            {     
                //GJL 131107 Added data labels
                XmlNode labelExprNode = DesignXmlDraw.FindNextInHierarchy(node,
                    "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataLabel", "Value");
                if (labelExprNode != null)
                    this.cbDataLabel.Text = labelExprNode.InnerText;
                XmlNode labelVisNode = DesignXmlDraw.FindNextInHierarchy(node,
                    "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataLabel", "Visible");
                if (labelVisNode != null)
                    this.chkDataLabel.Checked = labelVisNode.InnerText.ToUpper().Equals("TRUE");
            }

            chkDataLabel.Enabled = bDataLabelExpr.Enabled = cbDataLabel.Enabled = 
                bDataExpr.Enabled = cbChartData.Enabled = !StaticSeries; 
            // Don't allow the datalabel OR datavalues to be changed here if we have a static series GJL


			fChartType = fSubtype = fPalette = fRenderElement = fPercentWidth =
				fNoRows = fDataSet = fPageBreakStart = fPageBreakEnd = fChartData = false;           
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartCtl));
            this.DoubleBuffered = true;
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbChartType = new System.Windows.Forms.ComboBox();
			this.cbSubType = new System.Windows.Forms.ComboBox();
			this.cbPalette = new System.Windows.Forms.ComboBox();
			this.cbRenderElement = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbPercentWidth = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.tbNoRows = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cbDataSet = new System.Windows.Forms.ComboBox();
			this.chkPageBreakStart = new System.Windows.Forms.CheckBox();
			this.chkPageBreakEnd = new System.Windows.Forms.CheckBox();
			this.cbChartData = new System.Windows.Forms.ComboBox();
			this.cbDataLabel = new System.Windows.Forms.ComboBox();
			this.chkDataLabel = new System.Windows.Forms.CheckBox();
			this.bDataLabelExpr = new System.Windows.Forms.Button();
			this.lData1 = new System.Windows.Forms.Label();
			this.cbChartData2 = new System.Windows.Forms.ComboBox();
			this.lData2 = new System.Windows.Forms.Label();
			this.cbChartData3 = new System.Windows.Forms.ComboBox();
			this.lData3 = new System.Windows.Forms.Label();
			this.bDataExpr = new System.Windows.Forms.Button();
			this.bDataExpr3 = new System.Windows.Forms.Button();
			this.bDataExpr2 = new System.Windows.Forms.Button();
			this.cbVector = new System.Windows.Forms.ComboBox();
			this.btnVectorExp = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.chkToolTip = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.txtYToolFormat = new System.Windows.Forms.TextBox();
			this.txtXToolFormat = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.tbPercentWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// cbChartType
			// 
			resources.ApplyResources(this.cbChartType, "cbChartType");
			this.cbChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbChartType.Items.AddRange(new object[] {
            resources.GetString("cbChartType.Items"),
            resources.GetString("cbChartType.Items1"),
            resources.GetString("cbChartType.Items2"),
            resources.GetString("cbChartType.Items3"),
            resources.GetString("cbChartType.Items4"),
            resources.GetString("cbChartType.Items5"),
            resources.GetString("cbChartType.Items6"),
            resources.GetString("cbChartType.Items7"),
            resources.GetString("cbChartType.Items8")});
			this.cbChartType.Name = "cbChartType";
			this.cbChartType.SelectedIndexChanged += new System.EventHandler(this.cbChartType_SelectedIndexChanged);
			// 
			// cbSubType
			// 
			resources.ApplyResources(this.cbSubType, "cbSubType");
			this.cbSubType.Name = "cbSubType";
			this.cbSubType.SelectedIndexChanged += new System.EventHandler(this.cbSubType_SelectedIndexChanged);
			// 
			// cbPalette
			// 
			resources.ApplyResources(this.cbPalette, "cbPalette");
			this.cbPalette.Items.AddRange(new object[] {
            resources.GetString("cbPalette.Items"),
            resources.GetString("cbPalette.Items1"),
            resources.GetString("cbPalette.Items2"),
            resources.GetString("cbPalette.Items3"),
            resources.GetString("cbPalette.Items4"),
            resources.GetString("cbPalette.Items5"),
            resources.GetString("cbPalette.Items6"),
            resources.GetString("cbPalette.Items7"),
            resources.GetString("cbPalette.Items8"),
            resources.GetString("cbPalette.Items9")});
			this.cbPalette.Name = "cbPalette";
			this.cbPalette.SelectedIndexChanged += new System.EventHandler(this.cbPalette_SelectedIndexChanged);
			// 
			// cbRenderElement
			// 
			resources.ApplyResources(this.cbRenderElement, "cbRenderElement");
			this.cbRenderElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRenderElement.Items.AddRange(new object[] {
            resources.GetString("cbRenderElement.Items"),
            resources.GetString("cbRenderElement.Items1")});
			this.cbRenderElement.Name = "cbRenderElement";
			this.cbRenderElement.SelectedIndexChanged += new System.EventHandler(this.cbRenderElement_SelectedIndexChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// tbPercentWidth
			// 
			resources.ApplyResources(this.tbPercentWidth, "tbPercentWidth");
			this.tbPercentWidth.Name = "tbPercentWidth";
			this.tbPercentWidth.ValueChanged += new System.EventHandler(this.tbPercentWidth_ValueChanged);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// tbNoRows
			// 
			resources.ApplyResources(this.tbNoRows, "tbNoRows");
			this.tbNoRows.Name = "tbNoRows";
			this.tbNoRows.TextChanged += new System.EventHandler(this.tbNoRows_TextChanged);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// cbDataSet
			// 
			resources.ApplyResources(this.cbDataSet, "cbDataSet");
			this.cbDataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataSet.Name = "cbDataSet";
			this.cbDataSet.SelectedIndexChanged += new System.EventHandler(this.cbDataSet_SelectedIndexChanged);
			// 
			// chkPageBreakStart
			// 
			resources.ApplyResources(this.chkPageBreakStart, "chkPageBreakStart");
			this.chkPageBreakStart.Name = "chkPageBreakStart";
			this.chkPageBreakStart.CheckedChanged += new System.EventHandler(this.chkPageBreakStart_CheckedChanged);
			// 
			// chkPageBreakEnd
			// 
			resources.ApplyResources(this.chkPageBreakEnd, "chkPageBreakEnd");
			this.chkPageBreakEnd.Name = "chkPageBreakEnd";
			this.chkPageBreakEnd.CheckedChanged += new System.EventHandler(this.chkPageBreakEnd_CheckedChanged);
			// 
			// cbChartData
			// 
			resources.ApplyResources(this.cbChartData, "cbChartData");
			this.cbChartData.Name = "cbChartData";
			this.cbChartData.TextChanged += new System.EventHandler(this.cbChartData_Changed);
			// 
			// cbDataLabel
			// 
			resources.ApplyResources(this.cbDataLabel, "cbDataLabel");
			this.cbDataLabel.Name = "cbDataLabel";
			this.cbDataLabel.TextChanged += new System.EventHandler(this.cbChartData_Changed);
			// 
			// chkDataLabel
			// 
			resources.ApplyResources(this.chkDataLabel, "chkDataLabel");
			this.chkDataLabel.Name = "chkDataLabel";
			this.chkDataLabel.UseVisualStyleBackColor = true;
			this.chkDataLabel.CheckedChanged += new System.EventHandler(this.chkDataLabel_CheckedChanged);
			// 
			// bDataLabelExpr
			// 
			resources.ApplyResources(this.bDataLabelExpr, "bDataLabelExpr");
			this.bDataLabelExpr.Name = "bDataLabelExpr";
			this.bDataLabelExpr.UseVisualStyleBackColor = true;
			this.bDataLabelExpr.Click += new System.EventHandler(this.bDataLabelExpr_Click);
			// 
			// lData1
			// 
			resources.ApplyResources(this.lData1, "lData1");
			this.lData1.Name = "lData1";
			// 
			// cbChartData2
			// 
			resources.ApplyResources(this.cbChartData2, "cbChartData2");
			this.cbChartData2.Name = "cbChartData2";
			this.cbChartData2.TextChanged += new System.EventHandler(this.cbChartData_Changed);
			// 
			// lData2
			// 
			resources.ApplyResources(this.lData2, "lData2");
			this.lData2.Name = "lData2";
			// 
			// cbChartData3
			// 
			resources.ApplyResources(this.cbChartData3, "cbChartData3");
			this.cbChartData3.Name = "cbChartData3";
			this.cbChartData3.TextChanged += new System.EventHandler(this.cbChartData_Changed);
			// 
			// lData3
			// 
			resources.ApplyResources(this.lData3, "lData3");
			this.lData3.Name = "lData3";
			// 
			// bDataExpr
			// 
			resources.ApplyResources(this.bDataExpr, "bDataExpr");
			this.bDataExpr.Name = "bDataExpr";
			this.bDataExpr.Tag = "d1";
			this.bDataExpr.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// bDataExpr3
			// 
			resources.ApplyResources(this.bDataExpr3, "bDataExpr3");
			this.bDataExpr3.Name = "bDataExpr3";
			this.bDataExpr3.Tag = "d3";
			this.bDataExpr3.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// bDataExpr2
			// 
			resources.ApplyResources(this.bDataExpr2, "bDataExpr2");
			this.bDataExpr2.Name = "bDataExpr2";
			this.bDataExpr2.Tag = "d2";
			this.bDataExpr2.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// cbVector
			// 
			resources.ApplyResources(this.cbVector, "cbVector");
			this.cbVector.Items.AddRange(new object[] {
            resources.GetString("cbVector.Items"),
            resources.GetString("cbVector.Items1")});
			this.cbVector.Name = "cbVector";
			this.cbVector.SelectedIndexChanged += new System.EventHandler(this.cbVector_SelectedIndexChanged);
			// 
			// btnVectorExp
			// 
			resources.ApplyResources(this.btnVectorExp, "btnVectorExp");
			this.btnVectorExp.Name = "btnVectorExp";
			this.btnVectorExp.Tag = "d4";
			this.btnVectorExp.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// button1
			// 
			resources.ApplyResources(this.button1, "button1");
			this.button1.Name = "button1";
			this.button1.Tag = "d7";
			this.button1.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// button2
			// 
			resources.ApplyResources(this.button2, "button2");
			this.button2.Name = "button2";
			this.button2.Tag = "d5";
			this.button2.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// button3
			// 
			resources.ApplyResources(this.button3, "button3");
			this.button3.Name = "button3";
			this.button3.Tag = "d6";
			this.button3.Click += new System.EventHandler(this.bDataExpr_Click);
			// 
			// chkToolTip
			// 
			resources.ApplyResources(this.chkToolTip, "chkToolTip");
			this.chkToolTip.Name = "chkToolTip";
			this.chkToolTip.UseVisualStyleBackColor = true;
			this.chkToolTip.CheckedChanged += new System.EventHandler(this.chkToolTip_CheckedChanged);
			// 
			// checkBox1
			// 
			resources.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// txtYToolFormat
			// 
			resources.ApplyResources(this.txtYToolFormat, "txtYToolFormat");
			this.txtYToolFormat.Name = "txtYToolFormat";
			this.txtYToolFormat.TextChanged += new System.EventHandler(this.txtYToolFormat_TextChanged);
			// 
			// txtXToolFormat
			// 
			resources.ApplyResources(this.txtXToolFormat, "txtXToolFormat");
			this.txtXToolFormat.Name = "txtXToolFormat";
			this.txtXToolFormat.TextChanged += new System.EventHandler(this.txtXToolFormat_TextChanged);
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// ChartCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.cbDataLabel);
			this.Controls.Add(this.chkPageBreakStart);
			this.Controls.Add(this.chkDataLabel);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.txtXToolFormat);
			this.Controls.Add(this.txtYToolFormat);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.chkToolTip);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.btnVectorExp);
			this.Controls.Add(this.cbVector);
			this.Controls.Add(this.bDataExpr2);
			this.Controls.Add(this.bDataExpr3);
			this.Controls.Add(this.bDataExpr);
			this.Controls.Add(this.cbChartData3);
			this.Controls.Add(this.lData3);
			this.Controls.Add(this.cbChartData2);
			this.Controls.Add(this.lData2);
			this.Controls.Add(this.bDataLabelExpr);
			this.Controls.Add(this.cbChartData);
			this.Controls.Add(this.lData1);
			this.Controls.Add(this.chkPageBreakEnd);
			this.Controls.Add(this.cbDataSet);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.tbNoRows);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbPercentWidth);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cbRenderElement);
			this.Controls.Add(this.cbPalette);
			this.Controls.Add(this.cbSubType);
			this.Controls.Add(this.cbChartType);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "ChartCtl";
			((System.ComponentModel.ISupportInitialize)(this.tbPercentWidth)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public bool IsValid()
		{
			return true;
		}

		public void Apply()
		{
			// take information in control and apply to all the style nodes
			//  Only change information that has been marked as modified;
			//   this way when group is selected it is possible to change just
			//   the items you want and keep the rest the same.
				
			foreach (XmlNode riNode in this._ReportItems)
				ApplyChanges(riNode);

			// No more changes
			//AJM GJL 14082008
			fChartType = fVector = fSubtype = fPalette = fRenderElement = fPercentWidth =
                fNoRows = fDataSet = fPageBreakStart = fPageBreakEnd = fChartData = ftooltip = ftooltipX = ftooltip = ftooltipX = false;
		}

		public void ApplyChanges(XmlNode node)
		{
			if (fChartType)
			{
				_Draw.SetElement(node, "Type", this.cbChartType.Text);
			}
            if (ftooltip)//now controls the displaying of Y value
            {
                _Draw.SetElement(node, "fyi:Tooltip", this.chkToolTip.Checked.ToString());
            }
            if (ftooltipX)// controls the displaying of X value
            {
                _Draw.SetElement(node, "fyi:TooltipX", this.chkToolTip.Checked.ToString());
            }
            if (tooltipXFormat)
            {
                _Draw.SetElement(node, "fyi:TooltipXFormat", this.txtXToolFormat.Text);
            }
            if (tooltipYFormat)
            {
                _Draw.SetElement(node, "fyi:TooltipYFormat", this.txtYToolFormat.Text);
            }

            if (fVector) //AJM GJL 14082008
            {
                _Draw.SetElement(node, "fyi:RenderAsVector", this.cbVector.Text);
            }
			if (fSubtype)
			{
				_Draw.SetElement(node, "Subtype", this.cbSubType.Text);
			}
			if (fPalette)
			{
				_Draw.SetElement(node, "Palette", this.cbPalette.Text);
			}
			if (fRenderElement)
			{
				_Draw.SetElement(node, "ChartElementOutput", this.cbRenderElement.Text);
			}
			if (fPercentWidth)
			{
				_Draw.SetElement(node, "PointWidth", this.tbPercentWidth.Text);
			}
			if (fNoRows)
			{
				_Draw.SetElement(node, "NoRows", this.tbNoRows.Text);
			}
			if (fDataSet)
			{
				_Draw.SetElement(node, "DataSetName", this.cbDataSet.Text);
			}
			if (fPageBreakStart)
			{
				_Draw.SetElement(node, "PageBreakAtStart", this.chkPageBreakStart.Checked? "true": "false");
			}
			if (fPageBreakEnd)
			{
				_Draw.SetElement(node, "PageBreakAtEnd", this.chkPageBreakEnd.Checked? "true": "false");
			}
			if (fChartData)
			{
				//        <ChartData>
				//          <ChartSeries>
				//            <DataPoints>
				//              <DataPoint>
				//                <DataValues>
				//                  <DataValue>
				//                    <Value>=Sum(Fields!Sales.Value)</Value>
				//                  </DataValue>   --- you can have up to 3 DataValue elements
				//                </DataValues>
				//                <DataLabel>
				//                  <Style>
				//                    <Format>c</Format>
				//                  </Style>
				//                </DataLabel>
				//                <Marker />
				//              </DataPoint>
				//            </DataPoints>
				//          </ChartSeries>
				//        </ChartData>
				XmlNode chartdata = _Draw.SetElement(node, "ChartData", null);
				XmlNode chartseries = _Draw.SetElement(chartdata, "ChartSeries", null);
				XmlNode datapoints = _Draw.SetElement(chartseries, "DataPoints", null);
				XmlNode datapoint = _Draw.SetElement(datapoints, "DataPoint", null);
				XmlNode datavalues = _Draw.SetElement(datapoint, "DataValues", null);
                _Draw.RemoveElementAll(datavalues, "DataValue");
                XmlNode datalabel = _Draw.SetElement(datapoint, "DataLabel", null);
				XmlNode datavalue = _Draw.SetElement(datavalues, "DataValue", null);
				_Draw.SetElement(datavalue, "Value", this.cbChartData.Text);

                string type = cbChartType.Text.ToLowerInvariant();
                if (type == "scatter" || type == "bubble")
                {
                    datavalue = _Draw.CreateElement(datavalues, "DataValue", null);
                    _Draw.SetElement(datavalue, "Value", this.cbChartData2.Text);
                    if (type == "bubble")
                    {
                        datavalue = _Draw.CreateElement(datavalues, "DataValue", null);
                        _Draw.SetElement(datavalue, "Value", this.cbChartData3.Text);
                    }
                }
                _Draw.SetElement(datalabel, "Value", this.cbDataLabel.Text);
                _Draw.SetElement(datalabel, "Visible", this.chkDataLabel.Checked.ToString());
			}
		}

		private void cbChartType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fChartType = true;
			// Change the potential sub-types
			string savesub = cbSubType.Text;
			string[] subItems = new string [] {"Plain"};
            bool bEnableY = false;
            bool bEnableBubble = false;
			switch (cbChartType.Text)
			{
				case "Column":
					subItems = new string [] {"Plain", "Stacked", "PercentStacked"};
					break;
				case "Bar":
					subItems = new string [] {"Plain", "Stacked", "PercentStacked"};
					break;
				case "Line":
					subItems = new string [] {"Plain", "Smooth"};
					break;
				case "Pie":
					subItems = new string [] {"Plain", "Exploded"};
					break;
				case "Area":
					subItems = new string [] {"Plain", "Stacked"};
					break;
				case "Doughnut":
					break;
                case "Map":
                    subItems = RdlDesigner.MapSubtypes;
                    break;
				case "Scatter":
					subItems = new string [] {"Plain", "Line", "SmoothLine"};
                    bEnableY = true;
                    break;
				case "Stock":
                    break;
				case "Bubble":
                    bEnableY = bEnableBubble = true;
                    break;
				default:
					break;
			}
			cbSubType.Items.Clear();
			cbSubType.Items.AddRange(subItems);

            lData2.Enabled = cbChartData2.Enabled = bDataExpr2.Enabled = bEnableY;
            lData3.Enabled = cbChartData3.Enabled = bDataExpr3.Enabled = bEnableBubble;
            
            int i=0;
			foreach (string s in subItems)
			{
				if (s == savesub)
				{
					cbSubType.SelectedIndex = i;
					return;
				}
				i++;
			}
			// Didn't match old style
			cbSubType.SelectedIndex = 0;
		}

		//AJM GJL 14082008
        private void cbVector_SelectedIndexChanged(object sender, EventArgs e)
        {
            fVector = true;
        }

		private void cbSubType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fSubtype = true;
		}

		private void cbPalette_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fPalette = true;
		}

		private void cbRenderElement_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fRenderElement = true;
		}

		private void tbPercentWidth_ValueChanged(object sender, System.EventArgs e)
		{
			fPercentWidth = true;
		}

		private void tbNoRows_TextChanged(object sender, System.EventArgs e)
		{
			fNoRows = true;
		}

		private void cbDataSet_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fDataSet = true;
		}

		private void chkPageBreakStart_CheckedChanged(object sender, System.EventArgs e)
		{
			fPageBreakStart = true;
		}

		private void chkPageBreakEnd_CheckedChanged(object sender, System.EventArgs e)
		{
			fPageBreakEnd = true;
		}

		private void cbChartData_Changed(object sender, System.EventArgs e)
		{
			fChartData = true;		
		}
        private void bDataExpr_Click(object sender, System.EventArgs e)
        {
            Button bs = sender as Button;
            if (bs == null)
                return;
            Control ctl = null; 
            switch (bs.Tag as string)
            {
                case "d1":
                    ctl = cbChartData;
                    break;
                case "d2":
                    ctl = cbChartData2;
                    break;
                case "d3":
                    ctl = cbChartData3;
                    break;
				//AJM GJL 14082008
                case "d4":
                    ctl = cbVector;
                    fVector = true;
                    break;
                case "d5":
                    ctl = cbChartType;
                    fChartType = true;
                    break;
                case "d6":
                    ctl = cbPalette;
                    fPalette = true;
                    break;
                case "d7":
                    ctl = cbSubType;
                    fSubtype = true;
                    break;
                default:
                    return;
            }
            DialogExprEditor ee = new DialogExprEditor(_Draw, ctl.Text, _ReportItems[0], false);
            try
            {
                DialogResult dlgr = ee.ShowDialog();
                if (dlgr == DialogResult.OK)
                {
                    ctl.Text = ee.Expression;
                    fChartData = true;
                }
            }
            finally
            {
                ee.Dispose();
            }
        }

        private void chkDataLabel_CheckedChanged(object sender, EventArgs e)
        {
            cbDataLabel.Enabled = bDataLabelExpr.Enabled = chkDataLabel.Checked; 
        }

        private void bDataLabelExpr_Click(object sender, EventArgs e)
        {
            DialogExprEditor ee = new DialogExprEditor(_Draw, cbDataLabel.Text,_ReportItems[0] , false);
            try
            {
                if (ee.ShowDialog() == DialogResult.OK)
                {                 
                    cbDataLabel.Text = ee.Expression;
                }

            }
            finally
            {
                ee.Dispose();
            }
            return;
        }       

        private void chkToolTip_CheckedChanged(object sender, EventArgs e)
        {
            ftooltip = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ftooltipX = true;
        }

        private void txtYToolFormat_TextChanged(object sender, EventArgs e)
        {
            tooltipYFormat = true;
        }

        private void txtXToolFormat_TextChanged(object sender, EventArgs e)
        {
            tooltipXFormat = true;
        }

	}
}
