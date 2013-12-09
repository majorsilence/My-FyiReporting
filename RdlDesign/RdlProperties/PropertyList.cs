using fyiReporting.RDL;
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
using System;
using System.Collections.Generic;
using System.ComponentModel;            // need this for the properties metadata
using System.Globalization;
using System.Xml;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyList - The List Properties
    /// </summary>
    [TypeConverter(typeof(PropertyListConverter))]
    internal class PropertyList : PropertyDataRegion
    {
        internal PropertyList(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris)
            : base(d, dc, ris)
        {
        }

        [LocalizedCategory("List")]
		[LocalizedDisplayName("List_Grouping")]
		[LocalizedDescription("List_Grouping")]
        public PropertyGrouping Grouping
        {
            get
            {
                return new PropertyGrouping(this);
            }
        }

		[LocalizedCategory("List")]
		[LocalizedDisplayName("List_Sorting")]
		[LocalizedDescription("List_Sorting")]
        public PropertySorting Sorting
        {
            get
            {
                return new PropertySorting(this);
            }
        }

        #region XML
        [LocalizedCategory("XML")]
		[LocalizedDisplayName("List_DataInstanceName")]
		[LocalizedDescription("List_DataInstanceName")]
        public string DataInstanceName
        {
            get
            {
                return GetValue("DataInstanceName", "");
            }
            set
            {
                SetValue("DataInstanceName", value);
            }
        }

        [LocalizedCategory("XML")]
		[LocalizedDisplayName("List_DataInstanceElementOutput")]
		[LocalizedDescription("List_DataInstanceElementOutput")]
        public DataInstanceElementOutputEnum DataInstanceElementOutput
        {
            get
            {
                string v = GetValue("DataInstanceElementOutput", "Output");
                return fyiReporting.RDL.DataInstanceElementOutput.GetStyle(v);
            }
            set
            {
                SetValue("DataInstanceElementOutput", value.ToString());
            }
        }
        #endregion
    }

    internal class PropertyListConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyList)
            {
                PropertyList pe = value as PropertyList;
                return pe.Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

}
