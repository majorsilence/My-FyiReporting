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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using fyiReporting.RdlDesign.Resources;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for DialogAbout.
    /// </summary>
    public partial class DialogValidateRdl 
    {
        private readonly string SCHEMA2003 = "http://schemas.microsoft.com/sqlserver/reporting/2003/10/reportdefinition";
        private readonly string SCHEMA2003NAME = "http://schemas.microsoft.com/sqlserver/reporting/2003/10/reportdefinition/ReportDefinition.xsd";
        private readonly string SCHEMA2005 = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition";
        private readonly string SCHEMA2005NAME = "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition/ReportDefinition.xsd";
        static internal readonly string MSDESIGNERSCHEMA = "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner";
        static internal readonly string DESIGNERSCHEMA = "http://www.fyireporting.com/schemas";

        private int _ValidationErrorCount;
        private int _ValidationWarningCount;

        internal DialogValidateRdl(RdlDesigner designer)
        {
            _RdlDesigner = designer;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            return;
        }

        private void bValidate_Click(object sender, System.EventArgs e)
        {
            MDIChild mc = _RdlDesigner.ActiveMdiChild as MDIChild;
			if (mc == null || mc.DesignTab != DesignTabs.Edit)
            {
                MessageBox.Show(Strings.DialogValidateRdl_ShowC_SelectRDLTab);
                return;
            }
            string syntax = mc.SourceRdl;
            bool bNone = true;
            bool b2005 = true;
            Cursor saveCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            StringReader sr = null;
            XmlTextReader tr = null;
            XmlReader vr = null;

            try
            {
                // Find the namespace information in the <Report> element.
                //   We could be more precise and really parse it but it doesn't really help
                //   since we don't know the name and location of where the actual .xsd file is
                //   in the general case.  (e.g. xmlns="..." doesn't contain name of the .xsd file.
                int ir = syntax.IndexOf("<Report");
                if (ir >= 0)
                {
                    int er = syntax.IndexOf(">", ir);
                    if (er >= 0)
                    {
                        if (syntax.IndexOf("xmlns", ir, er - ir) >= 0)
                        {
                            bNone = false;
                            if (syntax.IndexOf("2005", ir, er - ir) < 0)
                                b2005 = false;
                        }
                    }
                }

                _ValidationErrorCount = 0;
                _ValidationWarningCount = 0;
                this.lbSchemaErrors.Items.Clear();
                sr = new StringReader(syntax);
                tr = new XmlTextReader(sr);
                XmlReaderSettings xrs = new XmlReaderSettings();
                xrs.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
                xrs.ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes |
                    XmlSchemaValidationFlags.ProcessIdentityConstraints |
                    XmlSchemaValidationFlags.ProcessSchemaLocation |
                    XmlSchemaValidationFlags.ProcessInlineSchema;

                // add any schemas needed
                if (!bNone)
                {
                    if (b2005)
                        xrs.Schemas.Add(SCHEMA2005, SCHEMA2005NAME);
                    else
                        xrs.Schemas.Add(SCHEMA2003, SCHEMA2003NAME);
                }
                // we always use the designer schema
                string designerSchema = string.Format("file://{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Designer.xsd");
                xrs.Schemas.Add(DESIGNERSCHEMA, designerSchema);

                vr = XmlReader.Create(tr, xrs);

                while (vr.Read()) ;

                this.lbSchemaErrors.Items.Add(string.Format("Validation completed with {0} warnings and {1} errors.", _ValidationWarningCount, _ValidationErrorCount));
            }
            catch (Exception ex)
            {
                this.lbSchemaErrors.Items.Add(ex.Message + "  Processing terminated.");
            }
            finally
            {
                Cursor.Current = saveCursor;
                if (sr != null)
                    sr.Close();
                if (tr != null)
                    tr.Close();
                if (vr != null)
                    vr.Close();
            }
        }

        public void ValidationHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
                this._ValidationErrorCount++;
            else
                this._ValidationWarningCount++;

            this.lbSchemaErrors.Items.Add(string.Format("{0}: {1} ({2}, {3})",
                args.Severity, args.Message, args.Exception.LineNumber, args.Exception.LinePosition));
        }

        private void lbSchemaErrors_DoubleClick(object sender, System.EventArgs e)
        {
            RdlEditPreview rep = _RdlDesigner.GetEditor();

            if (rep == null || this.lbSchemaErrors.SelectedIndex < 0)
                return;
            try
            {
                // line numbers are reported as (line#, character offset) e.g. (110, 32)  
                string v = this.lbSchemaErrors.Items[lbSchemaErrors.SelectedIndex] as string;
                int li = v.LastIndexOf("(");
                if (li < 0)
                    return;
                v = v.Substring(li + 1);
                li = v.IndexOf(",");	// find the
                v = v.Substring(0, li);

                int nLine = Int32.Parse(v);
                rep.Goto(this, nLine);
                this.BringToFront();
            }
#if DEBUG
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);	// developer might care about this error??
            }
#else
			catch 
			{}		// user doesn't really care if something went wrong
#endif

        }

        private void bClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void DialogValidateRdl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this._RdlDesigner.ValidateSchemaClosing();
        }
    }

}
