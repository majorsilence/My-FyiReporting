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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyTextbox - The Textbox Properties
    /// </summary>
    [DefaultProperty("Value")]
    internal class PropertyTextbox : PropertyReportItem
    {
        public PropertyTextbox(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }

        [LocalizedCategory("Textbox")]
		[LocalizedDisplayName("Textbox_Value")]
		[LocalizedDescription("Textbox_Value")]
        public PropertyExpr Value
        {
            get { return new PropertyExpr(this.GetValue("Value", "")); }
            set
            {
                this.SetValue("Value", value.Expression);
            }
        }

        [LocalizedCategory("Style")]
		[LocalizedDisplayName("Textbox_Appearance")]
		[LocalizedDescription("Textbox_Appearance")]
        public PropertyAppearance Appearance
        {
            get { return new PropertyAppearance(this); }
        }

        [LocalizedCategory("Textbox")]
		[LocalizedDisplayName("Textbox_CanGrow")]
		[LocalizedDescription("Textbox_CanGrow")]
        public bool CanGrow
        {
            get { return this.GetValue("CanGrow", "false").ToLower() == "true"; }
            set
            {
                this.SetValue("CanGrow", value? "true": "false");
            }
        }

        [LocalizedCategory("Textbox")]
		[LocalizedDisplayName("Textbox_CanShrink")]
		[LocalizedDescription("Textbox_CanShrink")]
        public bool CanShrink
        {
            get { return this.GetValue("CanShrink", "false").ToLower() == "true"; }
            set
            {
                this.SetValue("CanShrink", value ? "true" : "false");
            }
        }

        [LocalizedCategory("Textbox")]
		[LocalizedDisplayName("Textbox_HideDuplicates")]
		[LocalizedDescription("Textbox_HideDuplicates")]
		[TypeConverter(typeof(HideDuplicatesConverter))]
        public string HideDuplicates
        {
            get { return this.GetValue("HideDuplicates", ""); }
            set
            {
                if (value == "")
                    this.RemoveValue("HideDuplicates");
                else
                    this.SetValue("HideDuplicates", value);
            }
        }

        [LocalizedCategory("XML")]
		[LocalizedDisplayName("Textbox_DataElementStyle")]
		[LocalizedDescription("Textbox_DataElementStyle")]
        public DataElementStyleEnum DataElementStyle
        {
            get
            {
                string v = GetValue("DataElementStyle", "Auto");
                return fyiReporting.RDL.DataElementStyle.GetStyle(v);
            }
            set
            {
                SetValue("DataElementStyle", value.ToString());
            }
        }

        internal class HideDuplicatesConverter : StringConverter
        {

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                // returning false here means the property will
                // have a drop down and a value that can be manually
                // entered.      
                return false;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                if (context == null)
                    return base.GetStandardValues(context);

                PropertyTextbox pt = context.Instance as PropertyTextbox;
                if (pt == null)
                    return base.GetStandardValues(context);

                // Populate with the list of datasets and group names
                ArrayList ar = new ArrayList();
                ar.Add("");         // add an empty string to the collection
                object[] dsn = pt.Draw.DataSetNames;
                if (dsn != null)
                    ar.AddRange(dsn);
                object[] grps = pt.Draw.GroupingNames;
                if (grps != null)
                    ar.AddRange(grps);

                StandardValuesCollection svc = new StandardValuesCollection(ar);
                
                return svc;
            }
        }

    }
}
