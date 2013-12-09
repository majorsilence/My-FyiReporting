using RdlMapFile.Resources;
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
using System.Globalization;
using System.Xml;

namespace fyiReporting.RdlMapFile
{
    /// <summary>
    /// PropertyReportItem - The ReportItem Properties
    /// </summary>
    [DefaultProperty("Text")]
    internal class PropertyText : PropertyBase
    {

        internal PropertyText(DesignXmlDraw d):base(d)
        {
        }

        [LocalizedCategory("Text")]
		[LocalizedDisplayName("Text_Value")]
		[LocalizedDescription("Text_Value")]
        public string Value
        {
            get {return GetTextValue("Value"); }
            set{SetTextValue("Value", value); }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
		[LocalizedCategory("Text")]
		[LocalizedDisplayName("Text_Location")]
		[LocalizedDescription("Text_Location")]
        public Point Location
        {
            get 
            {
                XmlNode v = Draw.SelectedItem;
                return Draw.GetTextPoint(v, true);
            }
            set 
            {
                string l = string.Format("{0},{1}", value.X, value.Y);
                SetTextValue("Location", l); 
            }
        }

		[LocalizedCategory("Text")]
		[LocalizedDisplayName("Text_Color")]
		[LocalizedDescription("Text_Color")]
        public Color Color
        {
            get
            {
                XmlNode cn = Draw.SelectedItem;
                return Draw.GetTextColor(cn);
            }

            set
            {
                string sc = value.Name;
                SetTextValue("Color", sc);
            }
        }

		[LocalizedCategory("Text")]
		[LocalizedDisplayName("Text_Font")]
		[LocalizedDescription("Text_Font")]
        public Font Font
        {
            get
            {
                XmlNode v = Draw.SelectedItem;
                return Draw.GetTextFont(v);
            }
            set
            {
                string family = value.FontFamily.GetName(0);
                string fs = string.Format(NumberFormatInfo.InvariantInfo, "{0}", value.SizeInPoints);

                Draw.StartUndoGroup(Strings.PropertyText_Undo_FontChange);
                XmlNode xn = Draw.SelectedItem;
                foreach (XmlNode n in Draw.SelectedList)
                {
                    if (xn.Name != n.Name)
                        continue;
                    
                    Draw.SetElement(n, "FontFamily", family);
                    Draw.SetElement(n, "FontSize", fs);
                    Draw.SetElement(n, "FontWeight", value.Bold? "Bold": "Normal");
                    Draw.SetElement(n, "FontStyle", value.Italic? "Italic": "Normal");
                    if (value.Underline)
                        Draw.SetElement(n, "TextDecoration", "Underline");
                    else if (value.Strikeout)
                        Draw.SetElement(n, "TextDecoration", "LineThrough");
                    else
                        Draw.SetElement(n, "TextDecoration", "None");
                }
                Draw.EndUndoGroup();

                Draw.SignalXmlChanged();
                Draw.Invalidate();

            }
        }
    }
}
