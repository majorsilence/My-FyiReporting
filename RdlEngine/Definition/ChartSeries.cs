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
				// 20022008 AJM GJL - Added Second Y axis support
using System;
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Chart series definition and processing.
	///</summary>
	[Serializable]
	internal class ChartSeries : ReportLink
	{
        String _Colour;
        DataPoints _Datapoints;	// Data points within a series
		PlotTypeEnum _PlotType;		// Indicates whether the series should be plotted
								// as a line in a Column chart. If set to auto,
								// should be plotted per the primary chart type.
								// Auto (Default) | Line	
        String _YAxis;          //Indicates if the series uses the left or right axis. GJL 140208
        bool _NoMarker;          //Indicates if the series should not show its plot markers. GJL 300508
        String _LineSize;
	
		internal ChartSeries(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Datapoints=null;
			_PlotType=PlotTypeEnum.Auto;
            _YAxis = "Left";
            _NoMarker = false;
            _LineSize = "Regular";

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DataPoints":
						_Datapoints = new DataPoints(r, this, xNodeLoop);
						break;
					case "PlotType":
						_PlotType = fyiReporting.RDL.PlotType.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
                    case "YAxis":
                    case "fyi:YAxis":
                        _YAxis = xNodeLoop.InnerText;
                        break;
                    case "NoMarker":
                    case "fyi:NoMarker":
                        _NoMarker = Boolean.Parse(xNodeLoop.InnerText);
                        break;
                    case "LineSize":
                    case "fyi:LineSize":
                        _LineSize = xNodeLoop.InnerText;
                        break;
                    case "fyi:Color":
                    case "Color":
                    case "Colour":
                        _Colour = xNodeLoop.InnerText;
                        break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ChartSeries element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_Datapoints == null)
				OwnerReport.rl.LogError(8, "ChartSeries requires the DataPoints element.");
		}
		
		override internal void FinalPass()
		{
			if (_Datapoints != null)
				_Datapoints.FinalPass();
			return;
		}

		internal DataPoints Datapoints
		{
			get { return  _Datapoints; }
			set {  _Datapoints = value; }
		}

		internal PlotTypeEnum PlotType
		{
			get { return  _PlotType; }
			set {  _PlotType = value; }
		}

        internal String Colour
        {
            get { return _Colour; }
            set { _Colour = value; }
        }

        internal String YAxis
        {
            get { return _YAxis; }
            set { _YAxis = value; }
        }

        internal bool NoMarker
        {
            get { return _NoMarker; }
            set { _NoMarker = value; }
        }

        internal string LineSize
        {
            get { return _LineSize; }
            set { _LineSize = value; }
        }
	}
}
