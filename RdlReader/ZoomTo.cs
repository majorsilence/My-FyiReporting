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
using RdlReader.Resources;
using Majorsilence.Reporting.Rdl;
using Majorsilence.Reporting.RdlViewer;

namespace Majorsilence.Reporting.RdlReader
{
    /// <summary>
    /// Summary description for ZoomTo.
    /// </summary>
    public partial class ZoomTo 
    {
        public ZoomTo(RdlViewer.RdlViewer viewer)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            _Viewer = viewer;
            // set the intial value for magnification
            if (_Viewer.ZoomMode == ZoomEnum.FitPage)
                cbMagnify.Text = "Fit Page";
            else if (_Viewer.ZoomMode == ZoomEnum.FitWidth)
                cbMagnify.Text = "Fit Width";
            else if (_Viewer.Zoom == 1)
                cbMagnify.Text = "Actual Size";
            else
            {
                string formatted = string.Format("{0:#0.##}", _Viewer.Zoom * 100);
                if (formatted[formatted.Length - 1] == '.')
                    formatted = formatted.Substring(0, formatted.Length - 2);
                formatted = formatted + "%";
                cbMagnify.Text = formatted;
            }

        }

        private void bOK_Click(object sender, System.EventArgs e)
        {
            switch (cbMagnify.Text)
            {
                case "Fit Page":
                    _Viewer.ZoomMode = ZoomEnum.FitPage;
                    break;
                case "Actual Size":
                    _Viewer.Zoom = 1;
                    break;
                case "Fit Width":
                    _Viewer.ZoomMode = ZoomEnum.FitWidth;
                    break;
                default:
                    string z = cbMagnify.Text.Replace("%", "");
                    try
                    {
                        float zfactor = (float)(Convert.ToSingle(z) / 100.0);
                        _Viewer.Zoom = zfactor;
                    }
                    catch
                    {
                        MessageBox.Show(this, Strings.ZoomTo_ShowI_MagnificationLevelInvalid);
                        return;
                    }
                    break;
            }
            DialogResult = DialogResult.OK;
        }

        private void bCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
