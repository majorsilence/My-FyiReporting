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

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for DialogNew.
    /// </summary>
    public partial class DialogNew 
    {
        private string _resultType;

        public DialogNew()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            listNewChoices.Clear();
            listNewChoices.BeginUpdate();

            ListViewItem lvi = new ListViewItem("Blank");
            listNewChoices.Items.Add(lvi);

            lvi = new ListViewItem("Data Base");
            listNewChoices.Items.Add(lvi);

            listNewChoices.LabelWrap = true;
            listNewChoices.Select();
            listNewChoices.EndUpdate();

        }

        private void listNewChoices_ItemActivate(object sender, System.EventArgs e)
        {
            foreach (ListViewItem lvi in listNewChoices.SelectedItems)
            {
                _resultType = lvi.Text;
                break;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listNewChoices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            foreach (ListViewItem lvi in listNewChoices.SelectedItems)
            {
                _resultType = lvi.Text;
                break;
            }
            btnOK.Enabled = true;
        }

        public string ResultType
        {
            get { return _resultType; }
        }
    }
}
