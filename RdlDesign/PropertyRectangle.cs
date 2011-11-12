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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;
using System.Text.RegularExpressions;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyRectangle - The Rectangle specific Properties
    /// </summary>
    
    internal class PropertyRectangle : PropertyReportItem
    {
        internal PropertyRectangle(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }
        [CategoryAttribute("Rectangle"),
           DescriptionAttribute("Determines if report will start a new page at the top of the rectangle.")]
        public bool PageBreakAtStart
        {
            get { return this.Draw.GetElementValue(this.Node, "PageBreakAtStart", "false").ToLower() == "true" ? true : false; }
            set
            {
                this.SetValue("PageBreakAtStart", value ? "true" : "false");
            }
        }
        [CategoryAttribute("Rectangle"),
           DescriptionAttribute("Determines if report will start a new page after the bottom of the rectangle.")]
        public bool PageBreakAtEnd
        {
            get { return this.Draw.GetElementValue(this.Node, "PageBreakAtEnd", "false").ToLower() == "true" ? true : false; }
            set
            {
                this.SetValue("PageBreakAtEnd", value ? "true" : "false");
            }
        }

    }
}
