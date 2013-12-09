using System.ComponentModel;            // need this for the properties metadata
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
using System.Drawing;
using System.Xml;

namespace fyiReporting.RdlMapFile
{
    /// <summary>
    /// PropertyReportItem - The ReportItem Properties
    /// </summary>
    [DefaultProperty("Line")]
    internal class PropertyLine : PropertyBase
    {

        internal PropertyLine(DesignXmlDraw d):base(d)
        {
        }

        [LocalizedCategory("Line")]
		[LocalizedDisplayName("Line_P1")]
		[LocalizedDescription("Line_P1")]
        public Point P1
        {
            get 
            {
                XmlNode v = Draw.SelectedItem;
                Point[] pts = Draw.GetLineCoord(v, true);
                return pts[0];
            }
            set 
            {
                XmlNode v = Draw.SelectedItem;
                Point[] pts = Draw.GetLineCoord(v, true);
                string l = string.Format("{0},{1},{2}, {3}", value.X, value.Y, pts[1].X, pts[1].Y);
                SetTextValue("Points", l); 
            }
        }

		[LocalizedCategory("Line")]
		[LocalizedDisplayName("Line_P2")]
		[LocalizedDescription("Line_P2")]
        public Point P2
        {
            get
            {
                XmlNode v = Draw.SelectedItem;
                Point[] pts = Draw.GetLineCoord(v, true);
                return pts[1];
            }
            set
            {
                XmlNode v = Draw.SelectedItem;
                Point[] pts = Draw.GetLineCoord(v, true);
                string l = string.Format("{0},{1},{2}, {3}", pts[1].X, pts[1].Y, value.X, value.Y);
                SetTextValue("Points", l);
            }
        }

    }
}
