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
using System.Reflection;

namespace Majorsilence.Reporting.RdlMapFile
{
    /// <summary>
    /// Summary description for DialogFindByKey.
    /// </summary>
    public partial class DialogFindByKey 
    {

        internal DialogFindByKey(DesignXmlDraw dxd)
        {
            _Draw = dxd;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // populate the keys
            SortedList<string, string> keys = _Draw.GetAllKeys();
            foreach (string key in keys.Keys)
                lbKeyList.Items.Add(key);
            return;
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            List<string> select = new List<string>(lbKeyList.SelectedIndices.Count);
            foreach (int si in lbKeyList.SelectedIndices)
            {
                select.Add(lbKeyList.Items[si].ToString());
            }

            _Draw.SelectByKey(select);

            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
