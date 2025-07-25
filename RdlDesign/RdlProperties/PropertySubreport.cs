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
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;

namespace Majorsilence.Reporting.RdlDesign
{
    /// <summary>
    /// PropertySubreport - The Rectangle specific Properties
    /// </summary>
    
    internal class PropertySubreport : PropertyReportItem
    {
        public PropertySubreport(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }

        [LocalizedCategory("Subreport")]
		[LocalizedDisplayName("Subreport_ReportName")]
		[LocalizedDescription("Subreport_ReportName")]
        [Editor(typeof(PropertySubreportUIEditor), typeof(UITypeEditor))]
        public string ReportName
        {
            get { return Draw.GetElementValue(Node, "ReportName", ""); }
            set
            {
                SetValue("ReportName", value);
            }
        }

        [LocalizedCategory("Subreport")]
		[LocalizedDisplayName("Subreport_Parameters")]
		[LocalizedDescription("Subreport_Parameters")]
		[Editor(typeof(PropertySubreportParametersUIEditor), typeof(UITypeEditor))]
        public string Parameters
        {
            get 
            { 
                XmlNode pn = this.Draw.GetNamedChildNode(this.Node, "Parameters");
                return (pn == null || pn.ChildNodes == null || pn.ChildNodes.Count == 0) ? 
                    "none defined" :
                    string.Format("{0} defined", pn.ChildNodes.Count);
            }
        }

        [LocalizedCategory("Subreport")]
		[LocalizedDisplayName("Subreport_NoRows")]
		[LocalizedDescription("Subreport_NoRows")]
        public PropertyExpr NoRows
        {
            get { return new PropertyExpr(this.Draw.GetElementValue(this.Node, "NoRows", "")); }
            set
            {
                if (value.Expression == null || value.Expression.Length == 0)
                    this.RemoveValue("NoRows");
                else
                    this.SetValue("NoRows", value.Expression);
            }
        }

        [LocalizedCategory("Subreport")]
		[LocalizedDisplayName("Subreport_MergeTransactions")]
		[LocalizedDescription("Subreport_MergeTransactions")]
        public bool MergeTransactions
        {
            get { return string.Compare(this.Draw.GetElementValue(this.Node, "MergeTransactions", "true"), "true", true)==0; }
            set
            {
                this.SetValue("MergeTransactions", value? "true": "false");
            }
        }

    }

    internal class PropertySubreportUIEditor : UITypeEditor
    {
        public PropertySubreportUIEditor()
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
            PropertySubreport pr = context.Instance as PropertySubreport;
            if (pr == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pr.DesignCtl, pr.Draw, pr.Nodes, SingleCtlTypeEnum.SubreportCtl, null))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return pr.ReportName;
                }

                return base.EditValue(context, provider, value);
            }
        }
    }

    internal class PropertySubreportParametersUIEditor : UITypeEditor
    {
        public PropertySubreportParametersUIEditor()
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
            PropertySubreport pr = context.Instance as PropertySubreport;
            if (pr == null)
                return base.EditValue(context, provider, value);

            using (SingleCtlDialog scd = new SingleCtlDialog(pr.DesignCtl, pr.Draw, pr.Nodes, SingleCtlTypeEnum.SubreportCtl, null))
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return pr.Parameters;
                }

                return base.EditValue(context, provider, value);
            }
        }
    }

}
