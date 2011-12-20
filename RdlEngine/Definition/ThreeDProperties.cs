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
	/// ThreeDProperties definition and processing.
	///</summary>
	[Serializable]
	internal class ThreeDProperties : ReportLink
	{
		bool _Enabled;		// Whether or not a chart is displayed in 3D. Default is False (2D).
		ThreeDPropertiesProjectionModeEnum _ProjectionMode;		// The projection mode used for the 3D rendering
		int _Rotation;		// Rotation angle
							//  Applies only for Perspective projection
		int _Inclination;	// Inclination angle
							//  Applies only for Perspective projection
		int _Perspective;	// Represents the percent of perspective
							//  Applies only for Perspective projection
		int _HeightRatio;	// Ratio between height and width
		int _DepthRatio;	// Ration between depth and width
		ThreeDPropertiesShadingEnum _Shading;	// Default: None
		int _GapDepth;		// Percent depth gap between 3D bars and columns
		int _WallThickness;	// Percent thickness of outer walls
		ThreeDPropertiesDrawingStyleEnum _DrawingStyle;	// Determines shape of chart data displayed Default: cube
							//		Only applies to bar and column chart types.
		bool _Clustered;	// Determines if data series are clustered
							// (displayed along distinct rows). Only
							// applies to bar and column chart types.  Defaults to false.		
	
		internal ThreeDProperties(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Enabled=false;
			_ProjectionMode=ThreeDPropertiesProjectionModeEnum.Perspective;
			_Rotation=0;
			_Inclination=0;
			_Perspective=0;
			_HeightRatio=0;
			_DepthRatio=0;
			_Shading=ThreeDPropertiesShadingEnum.None;
			_GapDepth=0;
			_WallThickness=0;
			_DrawingStyle=ThreeDPropertiesDrawingStyleEnum.Cube;
			_Clustered=false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Enabled":
						_Enabled = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "ProjectionMode":
						_ProjectionMode = ThreeDPropertiesProjectionMode.GetStyle(xNodeLoop.InnerText);
						break;
					case "Rotation":
						_Rotation = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "Inclination":
						_Inclination = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "Perspective":
						_Perspective = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "HeightRatio":
						_HeightRatio = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "DepthRatio":
						_DepthRatio = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "Shading":
						_Shading = ThreeDPropertiesShading.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "GapDepth":
						_GapDepth = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "WallThickness":
						_WallThickness = XmlUtil.Integer(xNodeLoop.InnerText);
						break;
					case "DrawingStyle":
						_DrawingStyle = ThreeDPropertiesDrawingStyle.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Clustered":
						_Clustered = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			return;
		}

		internal bool Enabled
		{
			get { return  _Enabled; }
			set {  _Enabled = value; }
		}

		internal ThreeDPropertiesProjectionModeEnum ProjectionMode
		{
			get { return  _ProjectionMode; }
			set {  _ProjectionMode = value; }
		}

		internal int Rotation
		{
			get { return  _Rotation; }
			set {  _Rotation = value; }
		}

		internal int Inclination
		{
			get { return  _Inclination; }
			set {  _Inclination = value; }
		}

		internal int Perspective
		{
			get { return  _Perspective; }
			set {  _Perspective = value; }
		}

		internal int HeightRatio
		{
			get { return  _HeightRatio; }
			set {  _HeightRatio = value; }
		}

		internal int DepthRatio
		{
			get { return  _DepthRatio; }
			set {  _DepthRatio = value; }
		}

		internal ThreeDPropertiesShadingEnum Shading
		{
			get { return  _Shading; }
			set {  _Shading = value; }
		}

		internal int GapDepth
		{
			get { return  _GapDepth; }
			set {  _GapDepth = value; }
		}

		internal int WallThickness
		{
			get { return  _WallThickness; }
			set {  _WallThickness = value; }
		}

		internal ThreeDPropertiesDrawingStyleEnum DrawingStyle
		{
			get { return  _DrawingStyle; }
			set {  _DrawingStyle = value; }
		}

		internal bool Clustered
		{
			get { return  _Clustered; }
			set {  _Clustered = value; }
		}
	}
}
