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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Printing;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for ReportCtl.
    /// </summary>
    internal partial class ReportCtl : System.Windows.Forms.UserControl, IProperty
    {
        internal ReportCtl(DesignXmlDraw dxDraw)
        {
            _Draw = dxDraw;
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Initialize form using the style node values
            InitValues();
        }

        private void InitValues()
        {
            XmlNode rNode = _Draw.GetReportNode();
            tbWidth.Text = _Draw.GetElementValue(rNode, "Width", "");
            tbReportAuthor.Text = _Draw.GetElementValue(rNode, "Author", "");
            tbReportDescription.Text = _Draw.GetElementValue(rNode, "Description", "");
            tbPageWidth.Text = _Draw.GetElementValue(rNode, "PageWidth", "8.5in");
            tbPageHeight.Text = _Draw.GetElementValue(rNode, "PageHeight", "11in");
            tbMarginLeft.Text = _Draw.GetElementValue(rNode, "LeftMargin", "");
            tbMarginRight.Text = _Draw.GetElementValue(rNode, "RightMargin", "");
            tbMarginBottom.Text = _Draw.GetElementValue(rNode, "BottomMargin", "");
            tbMarginTop.Text = _Draw.GetElementValue(rNode, "TopMargin", "");
            // Page header settings
            XmlNode phNode = _Draw.GetCreateNamedChildNode(rNode, "PageHeader");
            this.chkPHFirst.Checked = _Draw.GetElementValue(phNode, "PrintOnFirstPage", "true").ToLower() == "true" ? true : false;
            this.chkPHLast.Checked = _Draw.GetElementValue(phNode, "PrintOnLastPage", "true").ToLower() == "true" ? true : false;
            // Page footer settings
            XmlNode pfNode = _Draw.GetCreateNamedChildNode(rNode, "PageFooter");
            this.chkPFFirst.Checked = _Draw.GetElementValue(pfNode, "PrintOnFirstPage", "true").ToLower() == "true" ? true : false;
            this.chkPFLast.Checked = _Draw.GetElementValue(pfNode, "PrintOnLastPage", "true").ToLower() == "true" ? true : false;


            PrinterSettings settings = new PrinterSettings();
            cbPageSize.DisplayMember = "PaperName";


            int width = MeasurementTypeAsHundrethsOfAnInch(tbPageWidth.Text);
            int height = MeasurementTypeAsHundrethsOfAnInch(tbPageHeight.Text);
            int pageCount = settings.PaperSizes.Count;

            // This conversion may be better converted to mm instead of hundrethds of an inch
            int count = 0;
            bool sizeFound = false;
            foreach (PaperSize psize in settings.PaperSizes)
            {
                cbPageSize.Items.Add(psize);


                if ((psize.Width == width) &&
                    (psize.Height == height) && 
                    (sizeFound == false))
                {   
                    cbPageSize.SelectedIndex = count;
                    sizeFound = true;
                }
                count = count + 1;
            }
        }

        private int MeasurementTypeAsHundrethsOfAnInch(string value)
        {
            string measurementType = value.Trim().ToLower();
            string measurementValue="0";
            if (measurementType.Length>=2)
            {
                measurementValue = measurementType.Substring(0, measurementType.Length-2);
                measurementType = measurementType.Substring(measurementType.Length-2);
            }

            if (measurementType == "mm")
            {
                // metric.  Convert to imperial for now
                return (int) ((decimal.Parse(measurementValue) / 25.4m) * 100);
            }
            else
            {
                // assume imperial
                return (int)(decimal.Parse(measurementValue) * 100);
            }

            

        }


        private string GetPaperSizeAsInch(int paperSize)
        {
            // paperSize is in hundredths of an inch.
            return (paperSize / 100.0).ToString() + "in";
        }

        public bool IsValid()
        {
            return true;
        }

        public void Apply()
        {
            XmlNode rNode = _Draw.GetReportNode();
            _Draw.SetElement(rNode, "Width", DesignerUtility.MakeValidSize(tbWidth.Text, false));
            _Draw.SetElement(rNode, "Author", tbReportAuthor.Text);
            _Draw.SetElement(rNode, "Description", tbReportDescription.Text);
            _Draw.SetElement(rNode, "PageWidth", tbPageWidth.Text);
            _Draw.SetElement(rNode, "PageHeight", tbPageHeight.Text);
            if (tbMarginLeft.Text.Trim().Length > 0)
                _Draw.SetElement(rNode, "LeftMargin", tbMarginLeft.Text);
            else
                _Draw.RemoveElement(rNode, "LeftMargin");
            if (tbMarginRight.Text.Trim().Length > 0)
                _Draw.SetElement(rNode, "RightMargin", tbMarginRight.Text);
            else
                _Draw.RemoveElement(rNode, "RightMargin");
            if (tbMarginBottom.Text.Trim().Length > 0)
                _Draw.SetElement(rNode, "BottomMargin", tbMarginBottom.Text);
            else
                _Draw.RemoveElement(rNode, "BottomMargin");
            if (tbMarginTop.Text.Trim().Length > 0)
                _Draw.SetElement(rNode, "TopMargin", tbMarginTop.Text);
            else
                _Draw.RemoveElement(rNode, "TopMargin");
            // Page header settings
            XmlNode phNode = _Draw.GetCreateNamedChildNode(rNode, "PageHeader");
            _Draw.SetElement(phNode, "PrintOnFirstPage", this.chkPHFirst.Checked ? "true" : "false");
            _Draw.SetElement(phNode, "PrintOnLastPage", this.chkPHLast.Checked ? "true" : "false");
            // Page footer settings
            XmlNode pfNode = _Draw.GetCreateNamedChildNode(rNode, "PageFooter");
            _Draw.SetElement(pfNode, "PrintOnFirstPage", this.chkPFFirst.Checked ? "true" : "false");
            _Draw.SetElement(pfNode, "PrintOnLastPage", this.chkPFLast.Checked ? "true" : "false");

        }

        private void tbSize_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            try { DesignerUtility.ValidateSize(tb.Text, false, false); }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(string.Format("Size value of {0} is invalid.\r\n", tb.Text, ex.Message), tb.Tag + " Field Invalid");
            }
        }

        private void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPageSize.SelectedItem == null)
            {
                return;
            }

            if (cbPageSize.SelectedItem is PaperSize)
            {
                PaperSize size = (PaperSize)cbPageSize.SelectedItem;
                tbPageWidth.Text = GetPaperSizeAsInch(size.Width);
                tbPageHeight.Text = GetPaperSizeAsInch(size.Height);

            }

        }
    }
}
