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
	/// DataPoint definition and processing.
	///</summary>
	[Serializable]
	internal class DataPoint : ReportLink
	{
		DataValues _DataValues;	//Data value set for the Y axis.
		DataLabel _DataLabel;	// Indicates the values should be marked with data labels.
		Action _Action;			// Action to execute.
		Style _Style;			// Defines border and background style
								// properties for the data point.
		Marker _Marker;			// Defines marker properties. Markers do
								//	not apply to data points of pie, doughnut
								//	and any stacked chart types.
		string _DataElementName;	// The name to use for the data element for
									//	this data point.
									//	Default: Name of corresponding static
									//	series or category. If there is no static
									//	series or categories, �Value�
		DataElementOutputEnum _DataElementOutput;	// Indicates whether the data point should
									// appear in a data rendering.
	
		internal DataPoint(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_DataValues=null;
			_DataLabel=null;
			_Action=null;
			_Style=null;
			_Marker=null;
			_DataElementName=null;
			_DataElementOutput=DataElementOutputEnum.Output;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DataValues":
						_DataValues = new DataValues(r, this, xNodeLoop);
						break;
					case "DataLabel":
						_DataLabel = new DataLabel(r, this, xNodeLoop);
						break;
					case "Action":
						_Action = new Action(r, this, xNodeLoop);
						break;
					case "Style":
						_Style = new Style(r, this, xNodeLoop);
						break;
					case "Marker":
						_Marker = new Marker(r, this, xNodeLoop);
						break;
					case "DataElementName":
						_DataElementName = xNodeLoop.InnerText;
						break;
					case "DataElementOutput":
						_DataElementOutput = fyiReporting.RDL.DataElementOutput.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown DataPoint element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_DataValues == null)
				OwnerReport.rl.LogError(8, "DataPoint requires the DataValues element.");
		}
		
		override internal void FinalPass()
		{
			if (_DataValues != null)
				_DataValues.FinalPass();
			if (_DataLabel != null)
				_DataLabel.FinalPass();
			if (_Action != null)
				_Action.FinalPass();
			if (_Style != null)
				_Style.FinalPass();
			if (_Marker != null)
				_Marker.FinalPass();
			return;
		}


		internal DataValues DataValues
		{
			get { return  _DataValues; }
			set {  _DataValues = value; }
		}

		internal DataLabel DataLabel
		{
			get { return  _DataLabel; }
			set {  _DataLabel = value; }
		}

		internal Action Action
		{
			get { return  _Action; }
			set {  _Action = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}

		internal Marker Marker
		{
			get { return  _Marker; }
			set {  _Marker = value; }
		}

		internal string DataElementName
		{
			get { return  _DataElementName; }
			set {  _DataElementName = value; }
		}

		internal DataElementOutputEnum DataElementOutput
		{
			get { return  _DataElementOutput; }
			set {  _DataElementOutput = value; }
		}
	}
	
}
