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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;
using System.Text.RegularExpressions;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fyiReporting.RdlMapFile
{
    /// <summary>
    /// PropertyReportItem - The ReportItem Properties
    /// </summary>
    [DefaultPropertyAttribute("Polygon")]
    internal class PropertyPolygon : PropertyBase
    {

        internal PropertyPolygon(DesignXmlDraw d):base(d)
        {
        }
        [CategoryAttribute("Polygon"),
           DescriptionAttribute("The fill color for the polygon")]
        public Color FillColor
        {
            get
            {
                string sc = GetTextValue("FillColor");
                if (sc == null)
                    return Color.Empty;
                Color c = Color.Empty;
			    try 
			    {
				    c = ColorTranslator.FromHtml(sc);
			    }
			    catch 
			    {       // if bad color just ignore and handle as empty color
			    }
    			return c;
            }
  
            set
            {
                string sc = value.Name;
                SetTextValue("FillColor", sc);
            }
        }

        [CategoryAttribute("Polygon"),
           DescriptionAttribute("The keys associated with this polygon")]
        public string[] Keys
        {
            get 
            {
                string k = GetTextValue("Keys");
                string[] keys = k.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return keys;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < value.Length; i++)
                {
                    if (i > 0)
                        sb.Append(',');
                    sb.Append(value[i]);
                }
                SetTextValue("Keys", sb.ToString()); 
            }
        }

            [CategoryAttribute("Polygon"),
               DescriptionAttribute("The Points of the polygon")]
        public Point[] Points
        {
            get 
            {
                XmlNode v = Draw.SelectedItem;
                return Draw.GetPolygon(v, true);
            }
            set 
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < value.Length; i++)
                {
                    if (i > 0)
                        sb.Append(',');
                    sb.AppendFormat("{0},{1}", value[i].X, value[i].Y);
                }
                SetTextValue("Points", sb.ToString()); 
            }
        }

    }
}
