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
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;
using System.Text.RegularExpressions;
using fyiReporting.RDL;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyChart - The Chart Properties
    /// </summary>
    internal class PropertyDataRegion : PropertyReportItem
    {
        internal PropertyDataRegion(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }
        [CategoryAttribute("DataRegion"), 
        DescriptionAttribute("Message to display if no rows available.")]
        public PropertyExpr NoRows
        {
            get { return new PropertyExpr(this.GetValue("NoRows", "")); }
            set
            {
                if (value.Expression == null || value.Expression.Length == 0)
                    this.RemoveValue("NoRows");
                else
                    this.SetValue("NoRows", value.Expression);
            }
        }
        [CategoryAttribute("DataRegion"),
        DescriptionAttribute("Keep data region on one page if possible.")]
        public bool KeepTogether
        {
            get { return this.GetValue("KeepTogether", "True").ToLower().Trim() == "true"; }
            set
            {
                this.SetValue("KeepTogether", value);
            }
        }
        [CategoryAttribute("DataRegion"), TypeConverter(typeof(DataSetsConverter)),
        DescriptionAttribute("Specifies which data set to use.")]
        public string DataSetName
        {
            get { return this.GetValue("DataSetName", ""); }
            set
            {
                if (value == null || value.Length == 0)
                    this.RemoveValue("DataSetName");
                else
                    this.SetValue("DataSetName", value);
            }
        }
        [CategoryAttribute("DataRegion"),
        DescriptionAttribute("Cause a page break before rendering.")]
        public bool PageBreakAtStart
        {
            get { return this.GetValue("PageBreakAtStart", "True").ToLower().Trim() == "true"; }
            set
            {
                this.SetValue("PageBreakAtStart", value);
            }
        }
        [CategoryAttribute("DataRegion"),
        DescriptionAttribute("Cause a page break after rendering.")]
        public bool PageBreakAtEnd
        {
            get { return this.GetValue("PageBreakAtEnd", "True").ToLower().Trim() == "true"; }
            set
            {
                this.SetValue("PageBreakAtEnd", value);
            }
        }
        [CategoryAttribute("DataRegion"),
                   DescriptionAttribute("Filters to apply to each row of data in data region.")]
        public PropertyFilters Filters
        {
            get
            {
                return new PropertyFilters(this);
            }
        }
        [CategoryAttribute("Style"),
                   DescriptionAttribute("Font, color, alignment, ... of NoRows text.")]
        public PropertyAppearance Appearance
        {
            get { return new PropertyAppearance(this); }
        }
    }
    #region DataSets
    internal class DataSetsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            PropertyReportItem pr = context.Instance as PropertyReportItem;
            if (pr == null)
                return base.GetStandardValues(context);

            // Populate with the list of datasets
            ArrayList ar = new ArrayList();
            ar.Add("");         // add an empty string to the collection
            object[] dsn = pr.Draw.DataSetNames;
            if (dsn != null)
                ar.AddRange(dsn);
            return new StandardValuesCollection(ar);
        }
    }
    #endregion

}
