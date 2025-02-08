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
using System.Collections.Generic;
using System.Xml;

namespace Majorsilence.Reporting.RdlDesign
{
    /// <summary>
    /// PropertyRectangle - The Rectangle specific Properties
    /// </summary>
    internal class PropertyRectangle : PropertyReportItem
    {
        public PropertyRectangle(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }

        [LocalizedCategory("Rectangle")]
		[LocalizedDisplayName("Rectangle_PageBreakAtStart")]
		[LocalizedDescription("Rectangle_PageBreakAtStart")]
        public bool PageBreakAtStart
        {
            get { return Draw.GetElementValue(Node, "PageBreakAtStart", "false").ToLower() == "true"; }
            set
            {
                SetValue("PageBreakAtStart", value ? "true" : "false");
            }
        }

        [LocalizedCategory("Rectangle")]
		[LocalizedDisplayName("Rectangle_PageBreakAtEnd")]
		[LocalizedDescription("Rectangle_PageBreakAtEnd")]
        public bool PageBreakAtEnd
        {
            get { return Draw.GetElementValue(Node, "PageBreakAtEnd", "false").ToLower() == "true"; }
            set
            {
                SetValue("PageBreakAtEnd", value ? "true" : "false");
            }
        }
    }
}
