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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// DialogListOfStrings: puts up a dialog that lets a user enter a list of strings
    /// </summary>
    public partial class DialogListOfStrings 
    {
        public DialogListOfStrings(List<string> list)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (list == null || list.Count == 0)
                return;

            // Populate textbox with the list of strings
            string[] sa = new string[list.Count];
            int l = 0;
            foreach (string v in list)
            {
                sa[l++] = v;
            }
            tbStrings.Lines = sa;

            return;
        }

        public List<string> ListOfStrings
        {
            get
            {
                if (this.tbStrings.Text.Length == 0)
                    return null;
                List<string> l = new List<string>();
                foreach (string v in tbStrings.Lines)
                {
                    if (v.Length > 0)
                        l.Add(v);
                }
                return l;
            }
        }

        private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs ea)
        {
            LinkLabel lnk = (LinkLabel)sender;
            lnk.Links[lnk.Links.IndexOf(ea.Link)].Visited = true;
            System.Diagnostics.Process.Start(lnk.Tag.ToString());
        }
    }

}
