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
    [TypeConverter(typeof(PropertySortingConverter)),
       Editor(typeof(PropertySortingUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertySorting : IReportItem
    {
        PropertyReportItem pri;

        internal PropertySorting(PropertyReportItem ri)
        {
            pri = ri;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            XmlNode sorting = pri.Draw.GetNamedChildNode(pri.Node, "Sorting");

            if (sorting == null)
                return "";

            foreach (XmlNode sNode in sorting.ChildNodes)
            {
                if (sNode.NodeType != XmlNodeType.Element ||
                        sNode.Name != "SortBy")
                    continue;
                if (sb.Length > 0)
                    sb.Append(", ");
                // Get the values
                XmlNode vNodes = pri.Draw.GetNamedChildNode(sNode, "SortExpression");
                if (vNodes != null)
                {
                    sb.Append(vNodes.InnerText);
                    string dir = pri.Draw.GetElementValue(sNode, "Direction", "Ascending");
                    sb.Append(' ');
                    sb.Append(dir);
                }
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

    internal class PropertySortingConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertySorting))
                return true;
            
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertySorting)
            {
                PropertySorting pb = value as PropertySorting;
                return pb.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertySortingUIEditor : UITypeEditor
    {
        internal PropertySortingUIEditor()
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

            PropertySorting pb = value as PropertySorting;
            if (pb == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pri.DesignCtl, pri.Draw, pri.Nodes,
                SingleCtlTypeEnum.SortingCtl, null))
            {

                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertySorting(pri);
                }

                return base.EditValue(context, provider, value);
            }
        }
    }
}