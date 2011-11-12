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
    [TypeConverter(typeof(PropertyFiltersConverter)),
       Editor(typeof(PropertyFiltersUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyFilters : IReportItem
    {
        PropertyReportItem pri;

        internal PropertyFilters(PropertyReportItem ri)
        {
            pri = ri;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            XmlNode filters = pri.Draw.GetNamedChildNode(pri.Node, "Filters");

            if (filters == null)
                return "";

            foreach (XmlNode fNode in filters.ChildNodes)
            {
                if (fNode.NodeType != XmlNodeType.Element ||
                        fNode.Name != "Filter")
                    continue;
                if (sb.Length > 0)
                    sb.Append(" AND ");
                // Get the values
                XmlNode vNodes = pri.Draw.GetNamedChildNode(fNode, "FilterValues");
                StringBuilder vs = new StringBuilder();
                if (vNodes != null)
                {
                    foreach (XmlNode v in vNodes.ChildNodes)
                    {
                        if (v.InnerText.Length <= 0)
                            continue;
                        if (vs.Length != 0)
                            vs.Append(", ");
                        vs.Append(v.InnerText);
                    }
                }
                // Add the row
                sb.Append(pri.Draw.GetElementValue(fNode, "FilterExpression", ""));
                sb.AppendFormat(" {0} ", pri.Draw.GetElementValue(fNode, "Operator", ""));
                sb.Append(vs.ToString());
            }

            return sb.ToString();
        }
        #region IReportItem Members
        public PropertyReportItem GetPRI()
        {
            return this.pri;
        }

        #endregion
    }

    internal class PropertyFiltersConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyFilters))
                return true;
            
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyFilters)
            {
                PropertyFilters pb = value as PropertyFilters;
                return pb.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertyFiltersUIEditor : UITypeEditor
    {
        internal PropertyFiltersUIEditor()
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

            PropertyFilters pb = value as PropertyFilters;
            if (pb == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pri.DesignCtl, pri.Draw, pri.Nodes, SingleCtlTypeEnum.FiltersCtl, null))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyFilters(pri);
                }

                return base.EditValue(context, provider, value);
            }
        }
    }
}