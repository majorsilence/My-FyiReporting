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
    /// PropertyAction - 
    /// </summary>
    [TypeConverter(typeof(PropertyBorderConverter)),
       Editor(typeof(PropertyBorderUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyBorder : IReportItem
    {
        PropertyReportItem pri;
        string[] _subitems;
        string[] _names;

        internal PropertyBorder(PropertyReportItem ri)
        {
            pri = ri;
            _names = null;
            _subitems = new string[] { "Style", "", "" };
        }

        internal PropertyBorder(PropertyReportItem ri, params string[] names)
        {
            pri = ri;
            _names = names;

            if (names == null)
            {
                _subitems = new string[] { "Style", "", "" };
            }
            else
            {
                // now build the array used to get/set values
                _subitems = new string[names.Length + 3];
                int i = 0;
                foreach (string s in names)
                    _subitems[i++] = s;

                _subitems[i++] = "Style";
            }
        }

        internal string[] Names
        {
            get { return _names; }
        }
        
        public override string ToString()
        {
            _subitems[_subitems.Length - 2] = "BorderStyle";
            _subitems[_subitems.Length - 1] = "Default";
            return pri.GetWithList("none", _subitems);
        }
        #region IReportItem Members
        public PropertyReportItem GetPRI()
        {
            return this.pri;
        }

        #endregion
    }

    internal class PropertyBorderConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyBorder))
                return true;
            
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyBorder)
            {
                PropertyBorder pb = value as PropertyBorder;
                return pb.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertyBorderUIEditor : UITypeEditor
    {
        internal PropertyBorderUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
                                        IServiceProvider provider,
                                        object value)
        {

            if ((context == null) || (provider == null))
                return base.EditValue(context, provider, value);

            // Access the Property Browser's UI display service
            IWindowsFormsEditorService editorService =
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService == null)
                return base.EditValue(context, provider, value);

            // Create an instance of the UI editor form
            IReportItem iri = context.Instance as IReportItem;
            if (iri == null)
                return base.EditValue(context, provider, value);
            PropertyReportItem pri = iri.GetPRI();

            PropertyBorder pb = value as PropertyBorder;
            if (pb == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pri.DesignCtl, pri.Draw, pri.Nodes, SingleCtlTypeEnum.BorderCtl, pb.Names))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyBorder(pri, pb.Names);
                }

                return base.EditValue(context, provider, value);
            }

        }
    }
}