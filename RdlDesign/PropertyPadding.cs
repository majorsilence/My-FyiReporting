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
    [TypeConverter(typeof(PropertyPaddingConverter))]
    internal class PropertyPadding : IReportItem
    {
        PropertyReportItem _pri;
        string[] _names;
        string[] _subitems;

        internal PropertyPadding(PropertyReportItem pri)
        {
            _pri = pri;
            _names = null;
            _subitems = new string[] { "Style", "" };
        }

        internal PropertyPadding(PropertyReportItem ri, params string[] names)
        {
            _pri = ri;
            _names = names;

            // now build the array used to get/set values
            _subitems = new string[names.Length + 2];
            int i = 0;
            foreach (string s in names)
                _subitems[i++] = s;

            _subitems[i++] = "Style";
        }
        
        [RefreshProperties(RefreshProperties.Repaint)]
        public PropertyExpr Left
        {
            get
            {
                return new PropertyExpr(GetStyleValue("PaddingLeft", "0pt"));
            }
            set
            {
                SetPadding("PaddingLeft", value);
            }
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public PropertyExpr Right
        {
            get
            {
                return new PropertyExpr(GetStyleValue("PaddingRight", "0pt"));
            }
            set
            {
                SetPadding("PaddingRight", value);
            }
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public PropertyExpr Top
        {
            get
            {
                return new PropertyExpr(GetStyleValue("PaddingTop", "0pt"));
            }
            set
            {
                SetPadding("PaddingTop", value);
            }
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public PropertyExpr Bottom
        {
            get
            {
                return new PropertyExpr(GetStyleValue("PaddingBottom", "0pt"));
            }
            set
            {
                SetPadding("PaddingBottom", value);
            }
        }

        void SetPadding(string l, PropertyExpr pe)
        {
            if (!_pri.IsExpression(pe.Expression))
                DesignerUtility.ValidateSize(pe.Expression, true, false);
            SetStyleValue(l, pe.Expression);
        }

        private string GetStyleValue(string l1, string def)
        {
            _subitems[_subitems.Length - 1] = l1;
            return _pri.GetWithList(def, _subitems);
        }

        private void SetStyleValue(string l1, string val)
        {
            _subitems[_subitems.Length - 1] = l1;
            _pri.SetWithList(val, _subitems);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", Left, Top, Right, Bottom);
        }

        #region IReportItem Members
        public PropertyReportItem GetPRI()
        {
            return _pri;
        }

        #endregion
    }

    internal class PropertyPaddingConverter : ExpandableObjectConverter
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
            if (destinationType == typeof(string) && value is PropertyPadding)
            {
                PropertyPadding pe = value as PropertyPadding;
                return pe.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}