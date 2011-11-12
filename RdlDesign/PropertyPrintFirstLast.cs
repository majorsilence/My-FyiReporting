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
using System.Drawing.Design;
using System.Xml;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyExpr - 
    /// </summary>
    [TypeConverter(typeof(PropertyPrintFirstLastConverter))]
    internal class PropertyPrintFirstLast
    {
        PropertyReport _pr;
        XmlNode _parent;

        internal PropertyPrintFirstLast(PropertyReport pr, XmlNode phNode)
        {
            _pr = pr;
            _parent = phNode;
        }
        public string Height
        {
            get
            {
                return _pr.Draw.GetElementValue(_parent, "Height", "0pt");
            }
            set
            {
                string v = value;
                if (v.Length == 0)
                    v = "0pt";
                else
                    DesignerUtility.ValidateSize(v, true, false);

                SetProp("Height", v);
                _pr.DesignCtl.SetScrollControls();          // this will force ruler and scroll bars to be updated

            }
        }

        public bool PrintOnFirstPage
        {
            get
            {
                return _pr.Draw.GetElementValue(_parent, "PrintOnFirstPage", "false").ToLower() == "true" ? true : false;
            }
            set
            {
                SetPrint("PrintOnFirstPage", value);
            }
        }
        public bool PrintOnLastPage
        {
            get
            {
                return _pr.Draw.GetElementValue(_parent, "PrintOnLastPage", "false").ToLower() == "true" ? true : false;
            }
            set
            {
                SetPrint("PrintOnLastPage", value);
            }
        }

        void SetPrint(string l, bool b)
        {
            SetProp(l, b ? "true" : "false");
        }

        void SetProp(string l, string v)
        {
            _pr.DesignCtl.StartUndoGroup(l + " change");
            _pr.Draw.SetElement(_parent, l, v);
            _pr.DesignCtl.EndUndoGroup(true);
            _pr.DesignCtl.SignalReportChanged();
            _pr.Draw.Invalidate();
        }

    }

    internal class PropertyPrintFirstLastConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                         System.Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyPrintFirstLast)
            {
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}