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
using System.Reflection;

namespace fyiReporting.RdlReader
{
    /// <summary>
    /// Summary description for DialogAbout.
    /// </summary>
    public partial class DialogAbout 
    {
        public DialogAbout()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            tbLicense.Text = @"RDL Reader displays reports defined using the Report Definition Language Specification.
Copyright (C) 2004-2008  fyiReporting Software, LLC
Copyright (C) 2012 Peter Gill <peter@majorsilence.com>

This file is part of the My-FyiReporting RDL project.
	
Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

For additional information, email peter@majorsilence.com or visit
the website https://github.com/majorsilence/My-FyiReporting.";

            lVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return;
        }

        private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs ea)
        {
            try
            {
                LinkLabel lnk = (LinkLabel)sender;
                lnk.Links[lnk.Links.IndexOf(ea.Link)].Visited = true;
                System.Diagnostics.Process.Start(lnk.Tag.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
