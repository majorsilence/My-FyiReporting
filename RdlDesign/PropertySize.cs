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
    [TypeConverter(typeof(PropertySizeConverter))]
    internal class PropertySize
    {
        PropertyReportItem _pri;
        string _h;
        string _w;

        internal PropertySize(PropertyReportItem pri, string h, string w)
        {
            _pri = pri;
            _h = h;
            _w = w;
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Height
        {
            get { return _h; }
            set 
            {
                DesignerUtility.ValidateSize(value, true, false);
                _h = value;
                _pri.SetValue("Height", value);
            }
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Width
        {
            get { return _w; }
            set 
            {
                DesignerUtility.ValidateSize(value, true, false);
                _w = value;
                _pri.SetValue("Width", value);
            }
        }
    }

    internal class PropertySizeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertySize)
            {
                PropertySize pe = value as PropertySize;
                return string.Format("({0}, {1})", pe.Height, pe.Width);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}