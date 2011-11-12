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
    [DefaultPropertyAttribute("Text")]
    internal class PropertyBase
    {
   		private DesignXmlDraw _Draw;

        internal PropertyBase(DesignXmlDraw d)
        {
            _Draw = d;
        }

        internal DesignXmlDraw Draw
        {
            get { return _Draw; }
        }

        internal string GetTextValue(string l)
        {
            XmlNode v = Draw.SelectedItem;
            return Draw.GetElementValue(v, l, "");
        }

        internal void SetTextValue(string l, string v)
        {
            Draw.StartUndoGroup(l + " change");
            XmlNode xn = Draw.SelectedItem;
            foreach (XmlNode n in Draw.SelectedList)
            {
                if (xn.Name == n.Name)
                    Draw.SetElement(n, l, v);
            }
            Draw.EndUndoGroup();

            Draw.SignalXmlChanged();
            Draw.Invalidate();
        }
        internal void SetTextValue(string l, float f)
        {
            string fs = string.Format(NumberFormatInfo.InvariantInfo, "{0}", f);
            SetTextValue(l, fs);
        }

    }
}
