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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Our sample PropertyTab.  It is connected to an instance of FunkyButton
    /// and displayes each of its vertices as a different property.
    /// </summary>
    internal class PropertyTableTab : PropertyTab
    {
        internal PropertyReportItem _pri;   // the report item we're attached to

        public PropertyTableTab()
        {
        }

        /// <summary>
        /// This is the tooltip string that will be displayed
        /// for this tab button.
        /// </summary>
        public override string TabName
        {
            get
            {
                return "Table";
            }
        }

        /// <summary>
        /// The Image that will be displayed on the PropertyGrid toolbar.
        /// </summary>
        public override Bitmap Bitmap
        {
            get
            {
                // force this to 16x16 to work around a Beta2 PropertyGrid
                // issue.
                System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RdlDesigner));
                System.Drawing.Image i = ((System.Drawing.Image)(resources.GetObject("bTable.Image")));

                return new Bitmap(i, new Size(16, 16));
            }
        }


        /// <summary>
        /// The Table contains a PropertyReportItem
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool CanExtend(object o)
        {
            PropertyReportItem pri = o as PropertyReportItem;
            return !(pri == null || pri.TableNode == null);
        }

        /// <summary>
        /// Just call the other version...
        /// </summary>
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attrs)
        {
            return GetProperties(null, component, attrs);
        }


        /// <summary>
        /// Our main function.  We display the vertices of a FunkyButton as properties by creating
        /// PropertyDescriptors for each one.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="attrs"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attrs)
        {
            PropertyReportItem pri = component as PropertyReportItem;

            // we can get things besides PropertyReportItems here.  Since we want our Point types
            // to be expandable, the PropertyGrid will still ask this PropertyTab for the properties of
            // points, so we default to the standard way of getting properties from other types of objects.
            //
            if (pri == null)
            {

                TypeConverter tc = TypeDescriptor.GetConverter(component);
                if (tc != null)
                {
                    return tc.GetProperties(context, component, attrs);
                }
                else
                {
                    return TypeDescriptor.GetProperties(component, attrs);
                }
            }

            _pri = pri;
            XmlNode tnode = _pri.TableNode;

            // Get the collection of properties
            PropertyDescriptorCollection pdc = new PropertyDescriptorCollection(null);

            if (tnode == null) // return empty if no table
                return pdc;

            PropertyDescriptorCollection bProps =
                              TypeDescriptor.GetProperties(_pri, true);

            // For each property use a property descriptor of our own that is able to 
            // be globalized
            foreach (PropertyDescriptor p in bProps)
            {
                if (p.Category != "Table")
                    continue;
                pdc.Add(p);
            }
            return pdc;

        }
    }
}
