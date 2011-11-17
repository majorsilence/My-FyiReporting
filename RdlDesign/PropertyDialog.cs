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
using System.Xml;

namespace fyiReporting.RdlDesign
{
    internal enum PropertyTypeEnum
    {
        Report,
        DataSets,
        ReportItems,
        Grouping,
        ChartLegend,
        CategoryAxis,
        ValueAxis,
        ChartTitle,
        CategoryAxisTitle,
        ValueAxisTitle,
        TableGroup,
        ValueAxis2Title// 20022008 AJM GJL

    }

    /// <summary>
    /// Summary description for PropertyDialog.
    /// </summary>
    internal partial class PropertyDialog 
    {
        // design draw 
        private List<XmlNode> _Nodes;			// selected nodes
        private PropertyTypeEnum _Type;
        private bool _Changed = false;
        private bool _Delete = false;
        private XmlNode _TableColumn = null;	// when table this is the current table column
        private XmlNode _TableRow = null;		// when table this is the current table row
        private List<UserControl> _TabPanels = new List<UserControl>();		// list of IProperty controls

        internal PropertyDialog(DesignXmlDraw dxDraw, List<XmlNode> sNodes, PropertyTypeEnum type)
            : this(dxDraw, sNodes, type, null, null) { }

        internal PropertyDialog(DesignXmlDraw dxDraw, List<XmlNode> sNodes, PropertyTypeEnum type, XmlNode tcNode, XmlNode trNode)
        {
            this._Draw = dxDraw;
            this._Nodes = sNodes;
            this._Type = type;
            _TableColumn = tcNode;
            _TableRow = trNode;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //   Add the controls for the selected ReportItems
            switch (_Type)
            {
                case PropertyTypeEnum.Report:
                    BuildReportTabs();
                    break;
                case PropertyTypeEnum.DataSets:
                    BuildDataSetsTabs();
                    break;
                case PropertyTypeEnum.Grouping:
                    BuildGroupingTabs();
                    break;
                case PropertyTypeEnum.ChartLegend:
                    BuildChartLegendTabs();
                    break;
                case PropertyTypeEnum.CategoryAxis:
                case PropertyTypeEnum.ValueAxis:
                    BuildChartAxisTabs(type);
                    break;
                case PropertyTypeEnum.ChartTitle:
                case PropertyTypeEnum.CategoryAxisTitle:
                case PropertyTypeEnum.ValueAxisTitle:
                case PropertyTypeEnum.ValueAxis2Title:// 20022008 AJM GJL
                    BuildTitle(type);
                    break;
                case PropertyTypeEnum.ReportItems:
                default:
                    BuildReportItemTabs();
                    break;
            }
        }

        internal bool Changed
        {
            get { return _Changed; }
        }

        internal bool Delete
        {
            get { return _Delete; }
        }

        private void BuildReportTabs()
        {
            this.Text = "Report Properties";

            ReportCtl rc = new ReportCtl(_Draw);
            AddTab("Report", rc);

            ReportParameterCtl pc = new ReportParameterCtl(_Draw);
            AddTab("Parameters", pc);

            ReportXmlCtl xc = new ReportXmlCtl(_Draw);
            AddTab("XML Rendering", xc);

            BodyCtl bc = new BodyCtl(_Draw);
            AddTab("Body", bc);

            CodeCtl cc = new CodeCtl(_Draw);
            AddTab("Code", cc);

            ModulesClassesCtl mc = new ModulesClassesCtl(_Draw);
            AddTab("Modules/Classes", mc);
            return;
        }

        private void BuildDataSetsTabs()
        {
            bDelete.Visible = true;

            this.Text = "DataSet";

            XmlNode aNode;
            if (_Nodes != null && _Nodes.Count > 0)
                aNode = _Nodes[0];
            else
                aNode = null;

            DataSetsCtl dsc = new DataSetsCtl(_Draw, aNode);
            AddTab("DataSet", dsc);

            QueryParametersCtl qp = new QueryParametersCtl(_Draw, dsc.DSV);
            AddTab("Query Parameters", qp);

            FiltersCtl fc = new FiltersCtl(_Draw, aNode);
            AddTab("Filters", fc);

            DataSetRowsCtl dsrc = new DataSetRowsCtl(_Draw, aNode, dsc.DSV);
            AddTab("Data", dsrc);
            return;
        }

        private void BuildGroupingTabs()
        {
            XmlNode aNode = _Nodes[0];
            if (aNode.Name == "DynamicSeries")
            {
                this.Text = "Series Grouping";
            }
            else if (aNode.Name == "DynamicCategories")
            {
                this.Text = "Category Grouping";
            }
            else
            {
                this.Text = "Grouping and Sorting";
            }

            GroupingCtl gc = new GroupingCtl(_Draw, aNode);
            AddTab("Grouping", gc);

            SortingCtl sc = new SortingCtl(_Draw, aNode);
            AddTab("Sorting", sc);

            // We have to create a grouping here but will need to kill it if no definition follows it
            XmlNode gNode = _Draw.GetCreateNamedChildNode(aNode, "Grouping");

            FiltersCtl fc = new FiltersCtl(_Draw, gNode);
            AddTab("Filters", fc);





            return;
        }

        private void BuildReportItemTabs()
        {
            XmlNode aNode = _Nodes[0];

            // Determine if all nodes are the same type
            string type = aNode.Name;
            if (type == "CustomReportItem")
            {
                // For customReportItems we use the type that is a parameter
                string t = _Draw.GetElementValue(aNode, "Type", "");
                if (t.Length > 0)
                    type = t;
            }

            foreach (XmlNode pNode in this._Nodes)
            {
                // For customReportItems we use the type that is a parameter
                string t = pNode.Name;
                if (t == "CustomReportItem")
                {
                    t = _Draw.GetElementValue(aNode, "Type", "");
                    if (t.Length == 0)       // Shouldn't happen
                        t = pNode.Name;
                }
                if (t != type)
                    type = "";			// Not all nodes have the same type
            }

            EnsureStyle();	// Make sure we have Style nodes for all the report items

            if (_Nodes.Count > 1)
                this.Text = "Group Selection Properties";
            else
            {
                string name = _Draw.GetElementAttribute(aNode, "Name", "");
                this.Text = string.Format("{0} {1} Properties", type.Replace("fyi:", ""), name);
            }

            // Create all the tabs
            if (type == "Textbox")
            {
                StyleTextCtl stc = new StyleTextCtl(_Draw, this._Nodes);
                AddTab("Text", stc);
            }
            else if (type == "List")
            {
                ListCtl lc = new ListCtl(_Draw, this._Nodes);
                AddTab("List", lc);
                if (_Nodes.Count == 1)
                {
                    XmlNode l = _Nodes[0];
                    FiltersCtl fc = new FiltersCtl(_Draw, l);
                    AddTab("Filters", fc);
                    SortingCtl srtc = new SortingCtl(_Draw, l);
                    AddTab("Sorting", srtc);
                }
            }
            else if (type == "Chart")
            {
                ChartCtl cc = new ChartCtl(_Draw, this._Nodes);
                AddTab("Chart", cc);

                // 05122007 AJM & GJL Create a new StaticSeriesCtl tab
                StaticSeriesCtl ssc = new StaticSeriesCtl(_Draw, this._Nodes);
                if (ssc.ShowMe)
                {
                    //If the chart has static series, then show the StaticSeriesCtl GJL
                    AddTab("Static Series", ssc);
                }


                if (_Nodes.Count == 1)
                {
                    FiltersCtl fc = new FiltersCtl(_Draw, _Nodes[0]);
                    AddTab("Filters", fc);
                }
            }
            else if (type == "Image")
            {
                ImageCtl imgc = new ImageCtl(_Draw, this._Nodes);
                AddTab("Image", imgc);
            }
            else if (type == "Table")
            {
                XmlNode table = _Nodes[0];
                TableCtl tc = new TableCtl(_Draw, this._Nodes);
                AddTab("Table", tc);
                FiltersCtl fc = new FiltersCtl(_Draw, table);
                AddTab("Filters", fc);
                XmlNode details = _Draw.GetNamedChildNode(table, "Details");
                if (details != null)
                {	// if no details then we don't need details sorting
                    GroupingCtl grpc = new GroupingCtl(_Draw, details);
                    AddTab("Grouping", grpc);
                    SortingCtl srtc = new SortingCtl(_Draw, details);
                    AddTab("Sorting", srtc);
                }
                if (_TableColumn != null)
                {
                    TableColumnCtl tcc = new TableColumnCtl(_Draw, _TableColumn);
                    AddTab("Table Column", tcc);
                }
                if (_TableRow != null)
                {
                    TableRowCtl trc = new TableRowCtl(_Draw, _TableRow);
                    AddTab("Table Row", trc);
                }
            }
            else if (type == "fyi:Grid")
            {
                GridCtl gc = new GridCtl(_Draw, this._Nodes);
                AddTab("Grid", gc);
            }
            else if (type == "Matrix")
            {
                XmlNode matrix = _Nodes[0];
                MatrixCtl mc = new MatrixCtl(_Draw, this._Nodes);
                AddTab("Matrix", mc);
                FiltersCtl fc = new FiltersCtl(_Draw, matrix);
                AddTab("Filters", fc);
            }
            else if (type == "Subreport" && _Nodes.Count == 1)
            {
                XmlNode subreport = _Nodes[0];
                SubreportCtl src = new SubreportCtl(_Draw, subreport);
                AddTab("Subreport", src);
            }
            else if (aNode.Name == "CustomReportItem")
            {
                XmlNode cri = _Nodes[0];
                CustomReportItemCtl cric = new CustomReportItemCtl(_Draw, _Nodes);
                AddTab(type, cric);
            }

            // Position tab
            PositionCtl pc = new PositionCtl(_Draw, this._Nodes);
            AddTab("Name/Position", pc);

            // Border tab
            StyleBorderCtl bc = new StyleBorderCtl(_Draw, null, this._Nodes);
            AddTab("Border", bc);

            if (!(type == "Line" || type == "Subreport"))
            {
                // Style tab
                StyleCtl sc = new StyleCtl(_Draw, this._Nodes);
                AddTab("Style", sc);

                // Interactivity tab
                InteractivityCtl ic = new InteractivityCtl(_Draw, this._Nodes);
                AddTab("Interactivity", ic);
            }
        }

        private void BuildChartAxisTabs(PropertyTypeEnum type)
        {
            string propName;
            if (type == PropertyTypeEnum.CategoryAxis)
            {
                this.Text = "Chart Category (X) Axis";
                propName = "CategoryAxis";
            }
            else
            {
                this.Text = "Chart Value (Y) Axis";
                propName = "ValueAxis";
            }

            XmlNode cNode = _Nodes[0];
            XmlNode aNode = _Draw.GetCreateNamedChildNode(cNode, propName);
            XmlNode axNode = _Draw.GetCreateNamedChildNode(aNode, "Axis");

            // Now we replace the node array with a new one containing only the legend
            _Nodes = new List<XmlNode>();
            _Nodes.Add(axNode);

            EnsureStyle();	// Make sure we have Style nodes

            // Chart Axis
            ChartAxisCtl cac = new ChartAxisCtl(_Draw, this._Nodes);
            AddTab("Axis", cac);

            // Style Text
            StyleTextCtl stc = new StyleTextCtl(_Draw, this._Nodes);
            AddTab("Text", stc);

            // Border tab
            StyleBorderCtl bc = new StyleBorderCtl(_Draw, null, this._Nodes);
            AddTab("Border", bc);

            // Style tab
            StyleCtl sc = new StyleCtl(_Draw, this._Nodes);
            AddTab("Style", sc);
        }

        private void BuildChartLegendTabs()
        {
            this.Text = "Chart Legend Properties";

            XmlNode cNode = _Nodes[0];
            XmlNode lNode = _Draw.GetCreateNamedChildNode(cNode, "Legend");

            // Now we replace the node array with a new one containing only the legend
            _Nodes = new List<XmlNode>();
            _Nodes.Add(lNode);

            EnsureStyle();	// Make sure we have Style nodes

            // Chart Legend
            ChartLegendCtl clc = new ChartLegendCtl(_Draw, this._Nodes);
            AddTab("Legend", clc);

            // Style Text
            StyleTextCtl stc = new StyleTextCtl(_Draw, this._Nodes);
            AddTab("Text", stc);

            // Border tab
            StyleBorderCtl bc = new StyleBorderCtl(_Draw, null, this._Nodes);
            AddTab("Border", bc);

            // Style tab
            StyleCtl sc = new StyleCtl(_Draw, this._Nodes);
            AddTab("Style", sc);
        }

        private void BuildTitle(PropertyTypeEnum type)
        {
            XmlNode cNode = _Nodes[0];
            _Nodes = new List<XmlNode>();		// replace with a new one
            if (type == PropertyTypeEnum.ChartTitle)
            {
                this.Text = "Chart Title";

                XmlNode lNode = _Draw.GetCreateNamedChildNode(cNode, "Title");
                _Nodes.Add(lNode);		// Working on the title		
            }
            else if (type == PropertyTypeEnum.CategoryAxisTitle)
            {
                this.Text = "Category (X) Axis Title";
                XmlNode caNode = _Draw.GetCreateNamedChildNode(cNode, "CategoryAxis");
                XmlNode aNode = _Draw.GetCreateNamedChildNode(caNode, "Axis");
                XmlNode tNode = _Draw.GetCreateNamedChildNode(aNode, "Title");
                _Nodes.Add(tNode);		// Working on the title		
            }
            // 20022008 AJM GJL
            else if (type == PropertyTypeEnum.ValueAxis2Title)
            {
                this.Text = "Value (Y) Axis (Right) Title";
                XmlNode caNode = _Draw.GetCreateNamedChildNode(cNode, "ValueAxis");
                XmlNode aNode = _Draw.GetCreateNamedChildNode(caNode, "Axis");
                XmlNode tNode = _Draw.GetCreateNamedChildNode(aNode, "fyi:Title2");
                _Nodes.Add(tNode);		// Working on the title		   
            }
            else
            {
                this.Text = "Value (Y) Axis Title";
                XmlNode caNode = _Draw.GetCreateNamedChildNode(cNode, "ValueAxis");
                XmlNode aNode = _Draw.GetCreateNamedChildNode(caNode, "Axis");
                XmlNode tNode = _Draw.GetCreateNamedChildNode(aNode, "Title");
                _Nodes.Add(tNode);		// Working on the title		
            }

            EnsureStyle();	// Make sure we have Style nodes

            // Style Text
            StyleTextCtl stc = new StyleTextCtl(_Draw, this._Nodes);
            AddTab("Text", stc);

            // Border tab
            StyleBorderCtl bc = new StyleBorderCtl(_Draw, null, this._Nodes);
            AddTab("Border", bc);

            // Style tab
            StyleCtl sc = new StyleCtl(_Draw, this._Nodes);
            AddTab("Style", sc);
        }

        private void EnsureStyle()
        {
            // Make sure we have Style nodes for all the nodes
            foreach (XmlNode pNode in this._Nodes)
            {
                XmlNode stNode = _Draw.GetCreateNamedChildNode(pNode, "Style");
            }
            return;
        }

        private void AddTab(string name, UserControl uc)
        {
            // Style tab
            TabPage tp = new TabPage();
            tp.Location = new System.Drawing.Point(4, 22);
            tp.Name = name + "1";
            tp.Size = new System.Drawing.Size(552, 284);
            tp.TabIndex = 1;
            tp.Text = name;

            _TabPanels.Add(uc);
            tp.Controls.Add(uc);

            uc.Dock = System.Windows.Forms.DockStyle.Fill;
            uc.Location = new System.Drawing.Point(0, 0);
            uc.Name = name + "1";
            uc.Size = new System.Drawing.Size(552, 284);
            uc.TabIndex = 0;

            tcProps.Controls.Add(tp);
        }

        private void bApply_Click(object sender, System.EventArgs e)
        {
            if (!IsValid())
                return;

            this._Changed = true;
            foreach (IProperty ip in _TabPanels)
            {
                ip.Apply();
            }
            this._Draw.Invalidate();		// Force screen to redraw
        }

        private void bOK_Click(object sender, System.EventArgs e)
        {
            if (!IsValid())
                return;

            bApply_Click(sender, e);	// Apply does all the work
            this.DialogResult = DialogResult.OK;
        }

        private bool IsValid()
        {
            int index = 0;
            foreach (IProperty ip in _TabPanels)
            {
                if (!ip.IsValid())
                {
                    tcProps.SelectedIndex = index;
                    return false;
                }
                index++;
            }
            return true;
        }

        private void PropertyDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_Type == PropertyTypeEnum.Grouping)
            {	// Need to check if grouping value is still required
                XmlNode aNode = _Nodes[0];

                // We have to create a grouping here but will need to kill it if no definition follows it
                XmlNode gNode = _Draw.GetNamedChildNode(aNode, "Grouping");
                if (gNode != null &&
                    _Draw.GetNamedChildNode(gNode, "GroupExpressions") == null)
                {	// Not a valid group if no GroupExpressions
                    aNode.RemoveChild(gNode);
                }
            }

        }

        private void bDelete_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show(this,
                    "Are you sure you want to delete this dataset?",
                    "DataSet",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _Delete = true;
                this.DialogResult = DialogResult.OK;
            }
        }
    }

    internal interface IProperty
    {
        void Apply();
        bool IsValid();
    }

}
