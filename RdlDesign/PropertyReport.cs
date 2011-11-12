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
using System.Xml;
using System.Text.RegularExpressions;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// PropertyReportItem - The ReportItem Properties
    /// </summary>
    [DefaultPropertyAttribute("Author")]
    internal class PropertyReport
    {
   		private DesignXmlDraw _Draw;
        private DesignCtl _DesignCtl;

        internal PropertyReport(DesignXmlDraw d, DesignCtl dc)
        {
            _Draw = d;
            _DesignCtl = dc;
        }

        internal DesignXmlDraw Draw
        {
            get { return _Draw; }
        }

        internal DesignCtl DesignCtl
        {
            get { return _DesignCtl; }
        }

        XmlNode ReportNode
        {
            get { return _Draw.GetReportNode(); }
        }

        [CategoryAttribute("Report"),
           DescriptionAttribute("The author of the report")]
        public string Author
        {
            get {return GetReportValue("Author"); }
            set{SetReportValue("Author", value); }
        }
        [CategoryAttribute("Report"),
           DescriptionAttribute("The description of the report")]
        public string Description
        {
            get { return GetReportValue("Description"); }
            set { SetReportValue("Description", value); }
        }
        [CategoryAttribute("Report"),
           DescriptionAttribute("The width of the report.")]
        public string Width
        {
            get { return GetReportValue("Width"); }
            set
            {
                DesignerUtility.ValidateSize(value, false, false);
                SetReportValue("Width", value);
                DesignCtl.SetScrollControls();          // this will force ruler and scroll bars to be updated
            }
        }
        [CategoryAttribute("Report"),
           DescriptionAttribute("Parameters defined in the report.")]
        public PropertyReportParameters Parameters
        {
            get { return new PropertyReportParameters(this); }
        }
        [CategoryAttribute("Report"),
           DescriptionAttribute("Basic functions defined for use in the report.")]
        public PropertyReportCode Code
        {
            get { return new PropertyReportCode(this); }
        }
        [CategoryAttribute("Report"),
           DescriptionAttribute("Modules and instances of classes for use in the report.")]
        public PropertyReportModulesClasses ModulesClasses
        {
            get { return new PropertyReportModulesClasses(this); }
        }

        [CategoryAttribute("Report"),
           DescriptionAttribute("The width of the page.")]
        public string PageWidth
        {
            get { return GetReportValue("PageWidth"); }
            set
            {
                DesignerUtility.ValidateSize(value, false, false);
                SetReportValue("PageWidth", value);

                DesignCtl.SetScrollControls();          // this will force ruler and scroll bars to be updated
            }
        }
        [CategoryAttribute("Report"),
           DescriptionAttribute("The height of the page.")]
        public string PageHeight
        {
            get { return GetReportValue("PageHeight"); }
            set
            {
                DesignerUtility.ValidateSize(value, false, false);
                SetReportValue("PageHeight", value);
            }
        }
        [CategoryAttribute("Report"),
        DisplayName("Page Margins"),
   DescriptionAttribute("Page margins for the report.")]
        public PropertyMargin Margins
        {
            get
            {
                return new PropertyMargin(this);
            }
        }

        [CategoryAttribute("Report"),
   DescriptionAttribute("PageHeader options for the report.")]
        public PropertyPrintFirstLast PageHeader
        {
            get
            {
                XmlNode phNode = _Draw.GetCreateNamedChildNode(ReportNode, "PageHeader");
                return new PropertyPrintFirstLast(this, phNode);
            }
        }

        [CategoryAttribute("Report"),
   DescriptionAttribute("PageFooter options for the report.")]
        public PropertyPrintFirstLast PageFooter
        {
            get
            {
                XmlNode phNode = _Draw.GetCreateNamedChildNode(ReportNode, "PageFooter");
                return new PropertyPrintFirstLast(this, phNode);
            }
        }

        [CategoryAttribute("Body"),
   DescriptionAttribute("Height of the body region.")]
        public string BodyHeight
        {
            get
            {
                return GetBodyValue("Height", "");
            }
            set
            {
                DesignerUtility.ValidateSize(value, true, false);
                SetBodyValue("Height", value);
                DesignCtl.SetScrollControls();          // this will force ruler and scroll bars to be updated
            }
        }

        [CategoryAttribute("Body"),
   DescriptionAttribute("Number of columns in the body region.")]
        public int BodyColumns
        {
            get
            {
                string c = GetBodyValue("Columns", "1");
                try
                {
                    return Convert.ToInt32(c);
                }
                catch
                {
                    return 1;
                }
            }
            set
            {
                if (value < 1)
                    throw new ArgumentException("The number of columns in the body must be greater than 0.");
                SetBodyValue("Columns", value.ToString());
            }
        }

        [CategoryAttribute("Body"),
   DescriptionAttribute("Spacing between columns.")]
        public string BodyColumnSpacing
        {
            get
            {
                return GetBodyValue("ColumnSpacing", "");
            }
            set
            {
                if (value.Length == 0)
                {
                    RemoveBodyValue("ColumnSpacing");
                }
                else
                {
                    DesignerUtility.ValidateSize(value, true, false);
                    SetBodyValue("ColumnSpacing", value);
                }
            }
        }

        [CategoryAttribute("XML"),
   DescriptionAttribute("XSL file to use to transform XML after rendering."),
        Editor(typeof(FileUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DataTransform
        {
            get { return GetReportValue("DataTransform"); }
            set { SetReportValue("DataTransform", value); }
        }

        [CategoryAttribute("XML"),
   DescriptionAttribute("The schema or namespace to specify when rendering XML.")]
        public string DataSchema
        {
            get { return GetReportValue("DataSchema"); }
            set { SetReportValue("DataSchema", value); }
        }

        [CategoryAttribute("XML"),
   DescriptionAttribute("The top level element name used when rendering XML.")]
        public string DataElementName
        {
            get { return GetReportValue("DataElementName"); }
            set { SetReportValue("DataElementName", value); }
        }

        [CategoryAttribute("XML"),
       TypeConverter(typeof(ElementStyleConverter)),
   DescriptionAttribute("Element style is either Attribute or Element.")]
        public string DataElementStyle
        {
            get { return GetReportValue("DataElementStyle", "AttributeNormal"); }
            set { SetReportValue("DataElementStyle", value); }
        }


        string GetBodyValue(string l, string def)
        {
            XmlNode rNode = _Draw.GetReportNode();
            XmlNode bNode = _Draw.GetNamedChildNode(rNode, "Body");
            return _Draw.GetElementValue(bNode, l, def);
        }

        void SetBodyValue(string l, string v)
        {
            XmlNode rNode = _Draw.GetReportNode();
            XmlNode bNode = _Draw.GetNamedChildNode(rNode, "Body");
            _DesignCtl.StartUndoGroup("Body " + l + " change");
            _Draw.SetElement(bNode, l, v);
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        void RemoveBodyValue(string l)
        {
            XmlNode rNode = _Draw.GetReportNode();
            XmlNode bNode = _Draw.GetNamedChildNode(rNode, "Body");
            _DesignCtl.StartUndoGroup("Body " + l + " change");
            _Draw.RemoveElement(bNode, l);
            _DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }

        internal string GetReportValue(string l)
        {
            return GetReportValue(l, "");
        }

        internal string GetReportValue(string l, string def)
        {
            XmlNode rNode = _Draw.GetReportNode();
            return _Draw.GetElementValue(rNode, l, def);
        }

        internal void SetReportValue(string l, string v)
        {
            XmlNode rNode = _Draw.GetReportNode();
            
            _DesignCtl.StartUndoGroup(l + " change");
            _Draw.SetElement(rNode, l, v);
            _DesignCtl.EndUndoGroup(true);

            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();
        }
    }
#region Parameters
    [TypeConverter(typeof(PropertyReportParameterConverter)),
     Editor(typeof(PropertyReportParameterUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyReportParameters
    {
        PropertyReport pr;

        internal PropertyReportParameters(PropertyReport r)
        {
            pr = r;
        }
        
        public override string ToString()
        {
            XmlNode rNode = pr.Draw.GetReportNode();
            XmlNode rpsNode = pr.Draw.GetNamedChildNode(rNode, "ReportParameters");

            string s;
            if (rpsNode == null)
                s = "No parameters defined";
            else
            {
                // show the list of parameters
                StringBuilder sb = new StringBuilder();
                foreach (XmlNode repNode in rpsNode)
                {
                    XmlAttribute nAttr = repNode.Attributes["Name"];
                    if (nAttr == null)	// shouldn't really happen
                        continue;
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(nAttr.Value);
                }
                sb.Append(" defined");
                s = sb.ToString();
            }
            return s;
        }
    }
    internal class PropertyReportParameterConverter  : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyReportParameters))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyReportParameters)
            {
                PropertyReportParameters prp = value as PropertyReportParameters;
                return prp.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertyReportParameterUIEditor : UITypeEditor
    {
        internal PropertyReportParameterUIEditor()
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
            PropertyReport pr = context.Instance as PropertyReport;

            if (pr == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pr.DesignCtl, pr.Draw, null, SingleCtlTypeEnum.ReportParameterCtl, null))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyReportParameters(pr);
                }

                return base.EditValue(context, provider, value);
            }
        }
    }
#endregion
    #region Code
    [TypeConverter(typeof(PropertyReportCodeConverter)),
     Editor(typeof(PropertyReportCodeUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyReportCode
    {
        PropertyReport pr;

        internal PropertyReportCode(PropertyReport r)
        {
            pr = r;
        }

        public override string ToString()
        {
            XmlNode rNode = pr.Draw.GetReportNode();
            XmlNode cNode = pr.Draw.GetNamedChildNode(rNode, "Code");
            return cNode == null ? "None defined" : "Defined";
        }
    }
    internal class PropertyReportCodeConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyReportCode))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyReportCode)
            {
                PropertyReportCode prc = value as PropertyReportCode;
                return prc.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertyReportCodeUIEditor : UITypeEditor
    {
        internal PropertyReportCodeUIEditor()
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
            PropertyReport pr = context.Instance as PropertyReport;

            if (pr == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pr.DesignCtl, pr.Draw, null, SingleCtlTypeEnum.ReportCodeCtl, null))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyReportCode(pr);
                }

                return base.EditValue(context, provider, value);
            }
        }
    }
#endregion
    #region ModulesClasses
    [TypeConverter(typeof(PropertyReportModulesClassesConverter)),
    Editor(typeof(PropertyReportModulesClassesUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyReportModulesClasses
    {
        PropertyReport pr;

        internal PropertyReportModulesClasses(PropertyReport r)
        {
            pr = r;
        }

        public override string ToString()
        {
            XmlNode rNode = pr.Draw.GetReportNode();
            StringBuilder sb = new StringBuilder();

            XmlNode cmsNode = pr.Draw.GetNamedChildNode(rNode, "CodeModules");
            sb.Append(cmsNode == null? "No Modules, ": "Modules, ");
 
   			XmlNode clsNode = pr.Draw.GetNamedChildNode(rNode, "Classes");
            sb.Append(clsNode == null? "No Classes": "Classes");

            sb.Append(" defined");
            return sb.ToString();
        }
    }
    internal class PropertyReportModulesClassesConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyReportModulesClasses))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyReportModulesClasses)
            {
                PropertyReportModulesClasses prm = value as PropertyReportModulesClasses;
                return prm.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertyReportModulesClassesUIEditor : UITypeEditor
    {
        internal PropertyReportModulesClassesUIEditor()
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
            PropertyReport pr = context.Instance as PropertyReport;

            if (pr == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pr.DesignCtl, pr.Draw, null, SingleCtlTypeEnum.ReportModulesClassesCtl, null))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyReportModulesClasses(pr);
                }

                return base.EditValue(context, provider, value);
            }
        }
    }
#endregion

    #region XSLFile
    internal class FileUIEditor : UITypeEditor
    {
        internal FileUIEditor()
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
            PropertyReport pr = context.Instance as PropertyReport;

            if (pr == null)
                return base.EditValue(context, provider, value);

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "XSL Files (*.xsl)|*.xsl" +
                    "|All files (*.*)|*.*";
                ofd.FilterIndex = 0;
                ofd.CheckFileExists = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return ofd.FileName;
                }

                return base.EditValue(context, provider, value);
            }
        }
    }
    #endregion
    #region ElementStyle
    internal class ElementStyleConverter : StringConverter
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
            return new StandardValuesCollection(new string[] {
                                    "AttributeNormal", "ElementNormal"});
        }
    }
    #endregion
}
