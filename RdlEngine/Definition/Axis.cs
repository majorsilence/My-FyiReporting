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
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Axis definition and processing.
	///</summary>
	[Serializable]
	internal class Axis : ReportLink
	{
		bool _Visible;	// Whether the axis labels are displayed. Defaults to false.
		Style _Style;	// Defines text style properties for the axis labels
		// and line style properties for the axis line.
		Title _Title;	// Defines a title for the First Y axis
        Title _Title2;  // 20022008 AJM GJL Defines a title for the Second Y axis
		bool _Margin;	// Indicates whether an axis margin will be
		//	created. The size of the margin is automatically
		//	generated based on the Scale and the number of
		//	data points. Defaults to false.
		AxisTickMarksEnum _MajorTickMarks; // None (Default)
		AxisTickMarksEnum _MinorTickMarks; // None (Default)
		ChartGridLines _MajorGridLines;	// Indicates major gridlines should be displayed for this axis.
		ChartGridLines _MinorGridLines;	// Indicates minor gridlines should be displayed for this axis.
		Expression _MajorInterval;		// Unit for major gridlines/tickmarks
		// If omitted, the axis is autodivided
		Expression _MinorInterval;		// Unit for minor gridlines/tickmarks
		// If omitted, the axis is autodivided
		bool _Reverse;	// If false (Default) the axis is plotted normally, if
		//  true its direction is reversed.
		int _CrossAt;	//  Value at which to cross the other axis
		// If omitted, uses the default behavior for the chart type.
		bool _Interlaced;	// If this property is true then strip lines are drawn
		//every other grid line interval for the axis. If grid
		//lines are not used for the axis then the axis� tick
		//marks or labels are used to determine the
		//interlaced strip lines interval.
		//Defaults to False.
		bool _Scalar;	// Indicates the values along this axis are scalar
		//values (i.e. numeric or date) which should be
		//displayed on the chart in a continuous axis.
		//Scalar cannot be true if the axis has more than
		//one grouping, if that grouping is static or has
		//more than one group expression or if the axis
		//values have a label.
		Expression _Min;		// Minimum value for the axis
		// If omitted, the axis autoscales
		Expression _Max;		// Maximum value for the axis
		// If omitted, the axis autoscales
		bool _LogScale;	// Whether the axis is logarithmic. Default is false.		
	    // If checked the axis shows gridlines at change of each month WP 12 May 2008.  Default is false
        bool _Month;

        
        bool _CanOmit=false;	// When display values don't fit, is it OK to drop some from display

		internal Axis(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Visible =false;
			_Style = null;
			_Title = null;
            _Title2 = null;// 20022008 AJM GJL
			_Margin = false;
			_MajorTickMarks = AxisTickMarksEnum.None;
			_MinorTickMarks = AxisTickMarksEnum.None;
			_MajorGridLines = null;
			_MinorGridLines = null;
			_MajorInterval = null;
			_MinorInterval =null;
			_Reverse = false;
			_CrossAt = 0;
			_Interlaced = false;
			_Scalar=false;
			_Min=null;
			_Max=null;
			_LogScale=false;
            _Month = false; //12052008 WP

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Visible":
						_Visible = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Style":
						_Style = new Style(r, this, xNodeLoop);
						break;
					case "Title":
						_Title = new Title(r, this, xNodeLoop);
						break;
                    // 20022008 AJM GJL - Second Y axis
                    case "Title2":
                    case "fyi:Title2":
                        _Title2 = new Title(r, this, xNodeLoop);
                       break;
					case "Margin":
						_Margin = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "MajorTickMarks":
						_MajorTickMarks = AxisTickMarks.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "MinorTickMarks":
						_MinorTickMarks = AxisTickMarks.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "MajorGridLines":
						_MajorGridLines = new ChartGridLines(r, this, xNodeLoop);
						break;
					case "MinorGridLines":
						_MinorGridLines = new ChartGridLines(r, this, xNodeLoop);
						break;
					case "MajorInterval":
						_MajorInterval = new Expression(OwnerReport, this, xNodeLoop, ExpressionType.Integer);
						OwnerReport.rl.LogError(4, "Axis element MajorInterval is currently ignored.");
						break;
					case "MinorInterval":
						_MinorInterval = new Expression(OwnerReport, this, xNodeLoop, ExpressionType.Integer);
						OwnerReport.rl.LogError(4, "Axis element MinorInterval is currently ignored.");
						break;
					case "Reverse":
						_Reverse = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "CrossAt":
						_CrossAt = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "Interlaced":
						_Interlaced = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Scalar":
						_Scalar = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Min":
						_Min = new Expression(OwnerReport, this, xNodeLoop, ExpressionType.Integer);
						break;
					case "Max":
						_Max = new Expression(OwnerReport, this, xNodeLoop, ExpressionType.Integer);
						break;
					case "LogScale":
						_LogScale = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
                    case "Month":
                    case "fyi:Month":
                        _Month = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
                        break;
                    case "fyi:CanOmit":
                        _CanOmit = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
                        break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Axis element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			if (_MajorInterval != null)
				_MajorInterval.FinalPass();
			if (_MinorInterval != null)
				_MinorInterval.FinalPass();
			if (_Max != null)
				_Max.FinalPass();
			if (_Min != null)
				_Min.FinalPass();
			if (_Style != null)
				_Style.FinalPass();
			if (_Title != null)
				_Title.FinalPass();
			// 20022008 AJM GJL - Second Y axis
            if (_Title2 != null)
                _Title2.FinalPass();
			if (_MajorGridLines != null)
				_MajorGridLines.FinalPass();
			if (_MinorGridLines != null)
				_MinorGridLines.FinalPass();
			return;
		}

		internal bool Visible
		{
			get { return  _Visible; }
			set {  _Visible = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}

		internal Title Title
		{
			get { return  _Title; }
			set {  _Title = value; }
		}
		// 20022008 AJM GJL - Second Y axis
        internal Title Title2
        {
            get { return _Title2; }
            set { _Title2 = value; }
        } 

		internal bool Margin
		{
			get { return  _Margin; }
			set {  _Margin = value; }
		}

		internal AxisTickMarksEnum MajorTickMarks
		{
			get { return  _MajorTickMarks; }
			set {  _MajorTickMarks = value; }
		}

		internal AxisTickMarksEnum MinorTickMarks
		{
			get { return  _MinorTickMarks; }
			set {  _MinorTickMarks = value; }
		}

		internal ChartGridLines MajorGridLines
		{
			get { return  _MajorGridLines; }
			set {  _MajorGridLines = value; }
		}

		internal ChartGridLines MinorGridLines
		{
			get { return  _MinorGridLines; }
			set {  _MinorGridLines = value; }
		}

		internal Expression MajorInterval
		{
			get { return  _MajorInterval; }
			set {  _MajorInterval = value; }
		}

		internal Expression MinorInterval
		{
			get { return  _MinorInterval; }
			set {  _MinorInterval = value; }
		}

		internal bool Reverse
		{
			get { return  _Reverse; }
			set {  _Reverse = value; }
		}

		internal int CrossAt
		{
			get { return  _CrossAt; }
			set {  _CrossAt = value; }
		}

		internal bool Interlaced
		{
			get { return  _Interlaced; }
			set {  _Interlaced = value; }
		}

		internal bool Scalar
		{
			get { return  _Scalar; }
			set {  _Scalar = value; }
		}

		internal Expression Min
		{
			get { return  _Min; }
			set {  _Min = value; }
		}

		internal Expression Max
		{
			get { return  _Max; }
			set {  _Max = value; }
		}
        //GJL 010308 --- Changed from int... We may want to start / stop a chart on a decimal
		internal double MaxEval(Report r, Row row)
		{
			if (_Max == null)
				return (double)int.MinValue;
			return  _Max.EvaluateDouble(r, row);
		}

		internal double MinEval(Report r, Row row)
		{
			if (_Min == null)
                return (double)int.MinValue;
			return  _Min.EvaluateDouble(r, row);
		}
        //WP 12 may 2008 -- set up month gridlines for report
        internal bool Month
        {
            get { return _Month; }
            set { _Month = value; }
		}

		internal bool LogScale
		{
			get { return  _LogScale; }
			set {  _LogScale = value; }
		}

        internal bool CanOmit
        {
            get { return _CanOmit; }
            set { _CanOmit = value; }
        }
    }	
}
