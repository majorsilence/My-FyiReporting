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
	/// Represents the report item for a line.
	///</summary>
	[Serializable]
	internal class Line : ReportItem
	{
		// Line has no additional elements/attributes beyond ReportItem
		internal Line(ReportDefn r, ReportLink p, XmlNode xNode) : base(r,p,xNode)
		{
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				// nothing beyond reportitem for now
				if (!ReportItemElement(xNodeLoop))	// try at ReportItem level
				{					
					// don't know this element - log it
					OwnerReport.rl.LogError(4, "Unknown Line element " + xNodeLoop.Name + " ignored.");
				}
			}
		}
		override internal void Run(IPresent ip, Row row)
		{
			ip.Line(this, row);
		}

		override internal void RunPage(Pages pgs, Row row)
		{
			Report r = pgs.Report;
            bool bHidden = IsHidden(r, row);

			SetPagePositionBegin(pgs);
			PageLine pl = new PageLine();
            SetPagePositionAndStyle(r, pl, row);
            if (!bHidden)
			    pgs.CurrentPage.AddObject(pl);
			SetPagePositionEnd(pgs, pl.Y);
		}

		internal float GetX2(Report rpt)
		{
			float x2=GetOffsetCalc(rpt)+LeftCalc(rpt);
			if (Width != null)
				x2 += Width.Points;
			return x2;
		}		

		internal int iX2
		{
			get 
			{
				int x2=0;
				if (Left != null)
					x2 = Left.Size;
				if (Width != null)
					x2 += Width.Size;
				return x2;
			}
		}

        internal int iY2
        {
            get
            {
                int y2 = 0;
                if (Top != null)
                    y2 = Top.Size;
                if (Height != null)
                    y2 += Height.Size;
                return y2;
            }
        }

        internal float Y2
        {
            get
            {
                float y2 = 0;
                if (Top != null)
                    y2 = Top.Points;
                if (Height != null)
                    y2 += Height.Points;
                return y2;
            }
        }
    }
}
