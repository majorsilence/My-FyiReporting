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
using System.Reflection;
using System.Windows.Forms;
using RdlMapFile.Resources;

namespace fyiReporting.RdlMapFile
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

            tbLicense.Text = Strings.DialogAbout_About;
	        lVersion.Text = string.Format(Strings.DialogAbout_Version,  Assembly.GetExecutingAssembly().GetName().Version);
	        lVMVersion.Text = string.Format(".NET {0}", Environment.Version);
        }

        private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs ea)
        {
            var lnk = (LinkLabel)sender;
            lnk.Links[lnk.Links.IndexOf(ea.Link)].Visited = true;
            System.Diagnostics.Process.Start(lnk.Tag.ToString());
        }
    }
}
