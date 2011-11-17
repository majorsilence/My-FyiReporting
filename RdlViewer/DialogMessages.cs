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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace fyiReporting.RdlViewer
{
    /// <summary>
    /// DialogMessage is used in place of a message box when the text can be large
    /// </summary>
    public partial class DialogMessages 
    {

        public DialogMessages(IList msgs)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            string[] lines = new string[msgs.Count];
            int l = 0;
            foreach (string msg in msgs)
            {
                lines[l++] = msg;
            }
            tbMessages.Lines = lines;
            return;
        }
    }
}
