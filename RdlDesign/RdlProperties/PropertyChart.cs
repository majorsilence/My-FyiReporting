/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyChart - The Chart Properties
    /// </summary>
    internal class PropertyChart : PropertyDataRegion
    {
        internal PropertyChart(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }
        [LocalizedCategory("Chart"), 
        TypeConverter(typeof(ChartTypeConverter)),
       Description("Type of the chart.")]
        public string Type
        {
            get { return this.GetValue("Type", "Column"); }
            set
            {
                this.SetValue("Type", value);
            }
        }
        [LocalizedCategory("Chart"),
        TypeConverter(typeof(PaletteTypeConverter)),
       Description("Color palette for the chart.")]
        public string Palette
        {
            get { return this.GetValue("Palette", "Default"); }
            set
            {
                this.SetValue("Palette", value);
            }
        }
        [LocalizedCategory("Chart"),
       Description("Percentage width for bars and columns.")]
        public int PointWidth
        {
            get 
            {
                int pw = 100;
                try
                {
                    string spw = this.GetValue("PointWidth", "100");
                    pw = Convert.ToInt32(spw);
                }
                catch { }
                return pw;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("PointWidth must be greater than 0.");
                this.SetValue("PointWidth", value.ToString());
            }
        }
 
        [LocalizedCategory("Chart"),
      Description("Properties controlling the display of the chart data.")]
        public PropertyChartData ChartData
        {
            get { return new PropertyChartData(this);}
        }

        [LocalizedCategory("Chart"),
      TypeConverter(typeof(ChartTitleTypeConverter)),
      Description("Chart Title.")]
        public PropertyChartTitle Title
        {
            get { return new PropertyChartTitle(this); }
        }
        [LocalizedCategory("Chart"),
      TypeConverter(typeof(ChartLegendTypeConverter)),
      Description("Chart Legend.")]
        public PropertyChartLegend Legend
        {
            get { return new PropertyChartLegend(this); }
        }
        [LocalizedCategory("Chart"),
      TypeConverter(typeof(ChartAxisTypeConverter)),
      Description("CategoryAxis defines the category (X) axis.")]
        public PropertyChartAxis CategoryAxis
        {
            get { return new PropertyChartAxis(this, "CategoryAxis"); }
        }
        [LocalizedCategory("Chart"),
      TypeConverter(typeof(ChartAxisTypeConverter)),
      Description("ValueAxis defines the data (Y) axis.")]
        public PropertyChartAxis ValueAxis
        {
            get { return new PropertyChartAxis(this, "ValueAxis"); }
        }
    }

    #region ChartAxis
    [TypeConverter(typeof(ChartAxisTypeConverter)),
      Description("Properties controlling the display of an axis.")]
    internal class PropertyChartAxis : IReportItem
    {
        PropertyChart _pt;
        string _Axis;
        internal PropertyChartAxis(PropertyChart pt, string axis)
        {
            _pt = pt;
            _Axis = axis;
        }

        [Description("Is the axis visible?")]
        public bool Visible
        {
            get
            {
                string v = _pt.GetWithList("true", _Axis, "Axis", "Visible");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", _Axis, "Axis", "Visible");
            }
        }
        [Description("Is there axis margin?")]
        public bool Margin
        {
            get
            {
                string v = _pt.GetWithList("false", _Axis, "Axis", "Margin");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", _Axis, "Axis", "Margin");
            }
        }
        [Description("Major tick marks.")]
        public AxisTickMarksEnum MajorTickMarks
        {
            get
            {
                string v = _pt.GetWithList("None", _Axis, "Axis", "MajorTickMarks");
                return AxisTickMarks.GetStyle(v);
            }
            set
            {
                _pt.SetWithList(value.ToString(), _Axis, "Axis", "MajorTickMarks");
            }
        }
        [Description("Minor tick marks.")]
        public AxisTickMarksEnum MinorTickMarks
        {
            get
            {
                string v = _pt.GetWithList("None", _Axis, "Axis", "MinorTickMarks");
                return AxisTickMarks.GetStyle(v);
            }
            set
            {
                _pt.SetWithList(value.ToString(), _Axis, "Axis", "MinorTickMarks");
            }
        }
        [Description("Unit for major gridlines and tickmarks; omit for autodivision.")]
        public PropertyExpr MajorInterval
        {
            get
            {
                string v = _pt.GetWithList("", _Axis, "Axis", "MajorInterval");
                return new PropertyExpr(v);
            }
            set
            {
                _pt.SetWithList(value.Expression, _Axis, "Axis", "MajorInterval");
            }
        }
        [Description("Unit for minor gridlines and tickmarks; omit for autodivision.")]
        public PropertyExpr MinorInterval
        {
            get
            {
                string v = _pt.GetWithList("", _Axis, "Axis", "MinorInterval");
                return new PropertyExpr(v);
            }
            set
            {
                _pt.SetWithList(value.Expression, _Axis, "Axis", "MinorInterval");
            }
        }
        [Description("Value at which to cross the other axis.")]
        public PropertyExpr CrossAt
        {
            get
            {
                string v = _pt.GetWithList("", _Axis, "Axis", "CrossAt");
                return new PropertyExpr(v);
            }
            set
            {
                _pt.SetWithList(value.Expression, _Axis, "Axis", "CrossAt");
            }
        }
        [Description("Minimum value for the axis.  When omitted axis autoscales.")]
        public PropertyExpr Min
        {
            get
            {
                string v = _pt.GetWithList("", _Axis, "Axis", "Min");
                return new PropertyExpr(v);
            }
            set
            {
                _pt.SetWithList(value.Expression, _Axis, "Axis", "Min");
            }
        }
        [Description("Maximum value for the axis.  When omitted axis autoscales.")]
        public PropertyExpr Max
        {
            get
            {
                string v = _pt.GetWithList("", _Axis, "Axis", "Max");
                return new PropertyExpr(v);
            }
            set
            {
                _pt.SetWithList(value.Expression, _Axis, "Axis", "Max");
            }
        }
        [Description("Should axis be plotted normally (false) or should the direction be reversed?")]
        public bool Reverse
        {
            get
            {
                string v = _pt.GetWithList("false", _Axis, "Axis", "Reverse");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", _Axis, "Axis", "Reverse");
            }
        }
        [Description("When true strip lines are drawn every other grid line.")]
        public bool Interlaced
        {
            get
            {
                string v = _pt.GetWithList("false", _Axis, "Axis", "Interlaced");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", _Axis, "Axis", "Interlaced");
            }
        }
        [Description("Scalar")]
        public bool Scalar
        {
            get
            {
                string v = _pt.GetWithList("false", _Axis, "Axis", "Scalar");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", _Axis, "Axis", "Scalar");
            }
        }

        [Description("Scalar")]
        public bool LogScale
        {
            get
            {
                string v = _pt.GetWithList("false", _Axis, "Axis", "LogScale");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", _Axis, "Axis", "LogScale");
            }
        }

        [Description("Title for the axis")]
        public PropertyChartTitle Title
        {
            get { return new PropertyChartTitle(_pt, _Axis, "Axis"); }
        }

        [Description("Font, color, alignment, ... of the axis.")]
        public PropertyAppearance Appearance
        {
            get { return new PropertyAppearance(_pt, _Axis, "Axis"); }
        }

        [Description("Background of the axis.")]
        public PropertyBackground Background
        {
            get { return new PropertyBackground(_pt, _Axis, "Axis"); }
        }

        [Description("Border properties of the axis.")]
        public PropertyBorder Border
        {
            get { return new PropertyBorder(_pt, _Axis, "Axis"); }
        }

        [Description("Padding properties of the axis.")]
        public PropertyPadding Padding
        {
            get { return new PropertyPadding(_pt, _Axis, "Axis"); }
        }

        [Description("Major Grid Lines properties")]
        public PropertyChartGridLines MajorGridLines
        {
            get { return new PropertyChartGridLines(_pt, _Axis, "Axis", "MajorGridLines"); }
        }

        [Description("Minor Grid Lines properties")]
        public PropertyChartGridLines MinorGridLines
        {
            get { return new PropertyChartGridLines(_pt, _Axis, "Axis", "MinorGridLines"); }
        }

        public override string ToString()
        {
            if (!this.Visible)
                return "hidden";
            else
                return this.Title.Caption.Expression;
        }

        #region IReportItem Members

        public PropertyReportItem GetPRI()
        {
            return this._pt;
        }

        #endregion
    }
    internal class ChartAxisTypeConverter : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyChartAxis))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyChartAxis)
            {
                PropertyChartAxis pa = value as PropertyChartAxis;
                return pa.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }


    #endregion

    #region ChartData
    [LocalizedCategory("ChartData"),
       TypeConverter(typeof(ChartDataTypeConverter)),
     Description("Properties controlling the display of the chart data.")]
    [ReadOnly(true)]//Doesn't work with static rows so we have disabled it... 05122007 AJM & GJL
    internal class PropertyChartData : IReportItem
    {
        PropertyChart _pt;
        internal PropertyChartData(PropertyChart pt)
        {
            _pt = pt;
        }
        [Description("Appearance of the label when visible.")]
        public PropertyAppearance LabelAppearance
        {
            get
            {
                return new PropertyAppearance(_pt,
                                "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataLabel");
            }
        }

        [Description("Should data label be displayed?")]
        public bool LabelVisible
        {
            get
            {
                string cdata = _pt.GetWithList("false",
                    "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataLabel", "Visible");

                return cdata.ToLower() == "true";
            }
            set
            {
                _pt.SetWithList(value? "true": "false", "ChartData", "ChartSeries", 
                    "DataPoints", "DataPoint", "DataLabel", "Visible");
            }
        }

        [Description("Expression that is to be charted.")]
        public PropertyExpr DataValue
        {
            get
            {
                // Chart data-- this is a simplification of what is possible (TODO) 
                //        <ChartData>
                //          <ChartSeries>
                //            <DataPoints>
                //              <DataPoint>
                //                <DataValues>
                //                  <DataValue>
                //                    <Value>=Sum(Fields!Sales.Value)</Value>
                //                  </DataValue>
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

                string cdata = _pt.GetWithList("",
                    "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataValues", "DataValue", "Value");
                return new PropertyExpr(cdata);
            }
            set
            {
                _pt.SetWithList(value.Expression, "ChartData", "ChartSeries", "DataPoints", "DataPoint",
                    "DataValues", "DataValue", "Value");
            }
        }
        [Description("Y Coordinate for Scatter and Bubble charts.")]
        public PropertyExpr DataValue2
        {
            get
            {
                // Chart data-- this is a simplification of what is possible (TODO) 
                //        <ChartData>
                //          <ChartSeries>
                //            <DataPoints>
                //              <DataPoint>
                //                <DataValues>
                //                  <DataValue>
                //                    <Value>=Sum(Fields!Sales.Value)</Value>
                //                  </DataValue>
                //                  <DataValue>   ---- this is the second data value
                //                    <Value>=Sum(Fields!Sales.Value)</Value>
                //                  </DataValue>
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
                string type = _pt.Type.ToLowerInvariant();
                if (!(type == "scatter" || type == "bubble"))
                    return new PropertyExpr("");

                return new PropertyExpr(GetChartDataValue(2));
            }
            set
            {
                string type = _pt.Type.ToLowerInvariant();
                if (!(type == "scatter" || type == "bubble"))
                    throw new ArgumentException("Chart type must be 'Scatter' or 'Bubble' to set DataValue2."); ;
                SetChartDataValue(2, value.Expression);
            }
        }
        [Description("Bubble size in Bubble charts.")]
        public PropertyExpr DataValue3
        {
            get
            {
                // Chart data-- this is a simplification of what is possible 
                //        <ChartData>
                //          <ChartSeries>
                //            <DataPoints>
                //              <DataPoint>
                //                <DataValues>
                //                  <DataValue>
                //                    <Value>=Sum(Fields!Sales.Value)</Value>
                //                  </DataValue>
                //                  <DataValue>
                //                    <Value>=Sum(Fields!Sales.Value)</Value>
                //                  </DataValue>
                //                  <DataValue>   ---- this is the third data value-- bubble size
                //                    <Value>=Sum(Fields!Sales.Value)</Value>
                //                  </DataValue>
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
                string type = _pt.Type.ToLowerInvariant();
                if (type != "bubble")
                    return new PropertyExpr("");

                return new PropertyExpr(GetChartDataValue(3));
            }
            set
            {
                string type = _pt.Type.ToLowerInvariant();
                if (type != "bubble")
                    throw new ArgumentException("Chart type must be 'Bubble' to set DataValue3."); ;
                SetChartDataValue(3, value.Expression);
            }
        }
        private string GetChartDataValue(int i)
        {
            XmlNode dvs = DesignXmlDraw.FindNextInHierarchy(_pt.Node,
    "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataValues");
            XmlNode cnode;
            foreach (XmlNode dv in dvs.ChildNodes)
            {
                if (dv.Name != "DataValue")
                    continue;
                i--;
                cnode = DesignXmlDraw.FindNextInHierarchy(dv, "Value");
                if (cnode == null)
                    continue;
                if (i <= 0)
                    return cnode.InnerText;
            }
            return "";
        }
 
        private void SetChartDataValue(int i, string expr)
        {
            DesignXmlDraw dr = _pt.Draw;
            string expr1 = i == 1 ? expr : _pt.ChartData.DataValue.Expression;
            string expr2 = i == 2 ? expr : _pt.ChartData.DataValue2.Expression;
            string expr3 = i == 3 ? expr : _pt.ChartData.DataValue3.Expression;

            foreach (XmlNode node in _pt.Nodes)
            {
                XmlNode chartdata = dr.SetElement(node, "ChartData", null);
                XmlNode chartseries = dr.SetElement(chartdata, "ChartSeries", null);
                XmlNode datapoints = dr.SetElement(chartseries, "DataPoints", null);
                XmlNode datapoint = dr.SetElement(datapoints, "DataPoint", null);
                XmlNode datavalues = dr.SetElement(datapoint, "DataValues", null);
                dr.RemoveElementAll(datavalues, "DataValue");
                XmlNode datavalue = dr.SetElement(datavalues, "DataValue", null);
                dr.SetElement(datavalue, "Value", expr1);

                string type = _pt.Type.ToLowerInvariant();
                if (type == "scatter" || type == "bubble")
                {
                    datavalue = dr.CreateElement(datavalues, "DataValue", null);
                    dr.SetElement(datavalue, "Value", expr2);
                    if (type == "bubble")
                    {
                        datavalue = dr.CreateElement(datavalues, "DataValue", null);
                        dr.SetElement(datavalue, "Value", expr3);
                    }
                }
            }
        }
        
        public override string ToString()
        {
            return _pt.GetWithList("",
                "ChartData", "ChartSeries", "DataPoints", "DataPoint", "DataValues", "DataValue", "Value");
        }

#region IReportItem Members

        public PropertyReportItem GetPRI()
        {
            return this._pt;
        }

        #endregion
    }

    internal class ChartDataTypeConverter : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyChartData))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyChartData)
            {
                PropertyChartData pc = value as PropertyChartData;
                return pc.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }


#endregion
    #region ChartGridLines
    [TypeConverter(typeof(ChartGridLinesTypeConverter))]
    internal class PropertyChartGridLines : IReportItem
    {
        PropertyChart _pt;
        string[] _names;

        internal PropertyChartGridLines(PropertyChart pt, params string[] names)
        {
            _pt = pt;
            _names = names;
        }

        [RefreshProperties(RefreshProperties.Repaint), 
        Description("Determines if grid lines are shown.")]
        public bool ShowGridLines
        {
            get
            {
                List<string> l = new List<string>(_names);
                l.Add("ShowGridLines");
                string s = _pt.GetWithList("false", l.ToArray());
                return s == "true";
            }
            set
            {
                List<string> l = new List<string>(_names);
                l.Add("ShowGridLines");
                _pt.SetWithList(value?"true":"false", l.ToArray());
            }
        }
        [TypeConverter(typeof(ColorConverter)),
        Description("Line color")]
        public string Color
        {
            get
            {
                List<string> l = new List<string>(_names);
                l.Add("Style");
                l.Add("BorderColor");
                l.Add("Default");
                return _pt.GetWithList("Black", l.ToArray());
            }
            set
            {
                List<string> l = new List<string>(_names);
                l.Add("Style");
                l.Add("BorderColor");
                l.Add("Default");
                _pt.SetWithList(value, l.ToArray());
            }
        }

        [Description("Line style: Dotted, Dashed or Solid")]
        public LineStyleEnum LineStyle
        {
            get
            {
                List<string> l = new List<string>(_names);
                l.Add("Style");
                l.Add("BorderStyle");
                l.Add("Default");
                string s = _pt.GetWithList("Solid", l.ToArray());
                switch (s)
                {
                    case "Solid": return LineStyleEnum.Solid;
                    case "Dotted": return LineStyleEnum.Dotted;
                    case "Dashed": return LineStyleEnum.Dashed;
                    default: return LineStyleEnum.Solid;
                }
            }
            set
            {
                List<string> l = new List<string>(_names);
                l.Add("Style");
                l.Add("BorderStyle");
                l.Add("Default");
                _pt.SetWithList(value.ToString(), l.ToArray());
            }
        }

        [Description("Width of line")]
        public PropertyExpr Width
        {
            get
            {
                List<string> l = new List<string>(_names);
                l.Add("Style");
                l.Add("BorderWidth");
                l.Add("Default");
                return new PropertyExpr(_pt.GetWithList("1pt", l.ToArray()));
            }
            set
            {
                List<string> l = new List<string>(_names);
                l.Add("Style");
                l.Add("BorderWidth");
                l.Add("Default");
                _pt.SetWithList(value.Expression, l.ToArray());
            }
        }
        
        public override string ToString()
        {
            return this.ShowGridLines?"visible":"hidden";
        }

        #region IReportItem Members

        public PropertyReportItem GetPRI()
        {
            return this._pt;
        }

        #endregion
    }
    internal class ChartGridLinesTypeConverter : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyChartGridLines))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyChartGridLines)
            {
                PropertyChartGridLines pf = value as PropertyChartGridLines;
                return pf.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    #endregion

    #region ChartLegend
    [LocalizedCategory("ChartLegend"),
       TypeConverter(typeof(ChartLegendTypeConverter)),
      Description("Properties controlling the display of the chart legend.")]
    internal class PropertyChartLegend : IReportItem
    {
        PropertyChart _pt;
        internal PropertyChartLegend(PropertyChart pt)
        {
            _pt = pt;
        }

        [Description("Is the legend visible?")]
        public bool Visible
        {
            get
            {
                string v = _pt.GetWithList("true", "Legend", "Visible");
                return v == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", "Legend", "Visible");
            }
        }

        [Description("Position of the legend.")]
        public LegendPositionEnum Position
        {
            get
            {
                string v = _pt.GetWithList("RightTop", "Legend", "Position");
                return LegendPosition.GetStyle(v);
            }
            set
            {
                _pt.SetWithList(value.ToString(), "Legend", "Position");
            }
        }

        [Description("Layout of the legend.")]
        public LegendLayoutEnum Layout
        {
            get
            {
                string v = _pt.GetWithList("Column", "Legend", "Layout");
                return LegendLayout.GetStyle(v);
            }
            set
            {
                _pt.SetWithList(value.ToString(), "Legend", "Layout");
            }
        }

        [Description("Draw legend inside the plot area when true, otherwise outside.")]
        public bool InsidePlotArea
        {
            get
            {
                string v = _pt.GetWithList("true", "Legend", "InsidePlotArea");
                return v.ToLower() == "true";
            }
            set
            {
                _pt.SetWithList(value ? "true" : "false", "Legend", "InsidePlotArea");
            }
        }

        [Description("Font, color, alignment, ... of the legend.")]
        public PropertyAppearance Appearance
        {
            get { return new PropertyAppearance(_pt, "Legend"); }
        }

        [Description("Background of the legend.")]
        public PropertyBackground Background
        {
            get { return new PropertyBackground(_pt, "Legend"); }
        }

        [Description("Border properties of the legend.")]
        public PropertyBorder Border
        {
            get { return new PropertyBorder(_pt, "Legend"); }
        }

        [Description("Padding properties of the legend.")]
        public PropertyPadding Padding
        {
            get { return new PropertyPadding(_pt, "Legend"); }
        }

        public override string ToString()
        {
            //return _pt.GetWithList("", "Title", "Caption");
            if (!this.Visible)
                return "hidden";
            else
                return this.Position.ToString();
        }

        #region IReportItem Members

        public PropertyReportItem GetPRI()
        {
            return this._pt;
        }

        #endregion
    }
    internal class ChartLegendTypeConverter : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyChartLegend))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyChartLegend)
            {
                PropertyChartLegend pf = value as PropertyChartLegend;
                return pf.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }


#endregion
    #region ChartTitle
    [LocalizedCategory("ChartTitle"),
       TypeConverter(typeof(ChartTitleTypeConverter)),
      Description("Properties controlling the display of the chart title.")]
    internal class PropertyChartTitle : IReportItem
    {
        PropertyChart _pt;
        string[] _subitems;
        string[] _names;

        internal PropertyChartTitle(PropertyChart pt)
        {
            _pt = pt;
            _names = null;
            _subitems = new string[] { "Title"};
        }

        internal PropertyChartTitle(PropertyChart pt, params string[] names)
        {
            _pt = pt;
            _names = names;

            // now build the array used to get/set values
            _subitems = new string[names.Length + 1];
            int i = 0;
            foreach (string s in names)
                _subitems[i++] = s;

            _subitems[i++] = "Title";
        }

        [RefreshProperties(RefreshProperties.Repaint), 
        Description("The text of the title.")]
        public PropertyExpr Caption
        {
            get
            {
                List<string> l = new List<string>(_subitems);
                l.Add("Caption");
                return new PropertyExpr(_pt.GetWithList("", l.ToArray()));
            }
            set
            {
                List<string> l = new List<string>(_subitems);
                l.Add("Caption");
                _pt.SetWithList(value.Expression, l.ToArray());
            }
        }

        [Description("Font, color, alignment, ... of the caption.")]
        public PropertyAppearance Appearance
        {
            get { return new PropertyAppearance(_pt, _subitems); }
        }

        [Description("Background of the caption.")]
        public PropertyBackground Background
        {
            get { return new PropertyBackground(_pt, _subitems); }
        }

        [Description("Border properties of the caption.")]
        public PropertyBorder Border
        {
            get { return new PropertyBorder(_pt, _subitems); }
        }

        [Description("Padding properties of the caption.")]
        public PropertyPadding Padding
        {
            get { return new PropertyPadding(_pt, _subitems); }
        }

        public override string ToString()
        {
            return this.Caption.Expression;
        }

        #region IReportItem Members

        public PropertyReportItem GetPRI()
        {
            return this._pt;
        }

        #endregion
    }
    internal class ChartTitleTypeConverter : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyChartTitle))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyChartTitle)
            {
                PropertyChartTitle pf = value as PropertyChartTitle;
                return pf.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    #endregion
    #region ChartType
    internal class ChartTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] {
                                    "Column", "Bar", "Line", "Map", "Pie", "Area", "Doughnut", "Bubble", "Scatter"});
        }
    }
    #endregion
    #region PaletteType
    internal class PaletteTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] {
               "Default", "EarthTones", "Excel", "GrayScale", "Light", "Pastel", "SemiTransparent","Patterned","PatternedBlack","Custom"});
        }
    }
    #endregion

    internal enum LineStyleEnum {Solid, Dotted, Dashed };
}
