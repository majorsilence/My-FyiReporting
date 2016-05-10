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
using fyiReporting.RdlDesign.Resources;

namespace fyiReporting.RdlDesign
{
    /// <summary>
    /// Summary description for FindTab.
    /// </summary>
    internal partial class FindTab 
    {
        public FindTab()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.AcceptButton = btnNext;
            this.CancelButton = btnCancel;
            txtFind.Focus();
        }

        internal FindTab(RdlEditPreview pad)
        {
            rdlEdit = pad;
			rdlEdit.FindTab = this;
            InitializeComponent();

            this.AcceptButton = btnNext;
            this.CancelButton = btnCancel;
            txtFind.Focus();
		}

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            rdlEdit.FindNext(this, txtFind.Text, chkCase.Checked, radioUp.Checked);
        }

        private void txtFind_TextChanged(object sender, System.EventArgs e)
        {
            if (txtFind.Text == "")
                btnNext.Enabled = false;
            else
                btnNext.Enabled = true;
        }

        private void btnFindNext_Click(object sender, System.EventArgs e)
        {
            rdlEdit.FindNext(this, txtFindR.Text, chkMatchCase.Checked, false);
            txtFind.Focus();
        }

        private void btnReplace_Click(object sender, System.EventArgs e)
        {
            rdlEdit.ReplaceNext(this, txtFindR.Text, txtReplace.Text, chkCase.Checked);
            txtFindR.Focus();
        }

        private void txtFindR_TextChanged(object sender, System.EventArgs e)
        {
            bool bEnable = (txtFindR.Text == "") ? false : true;
            btnFindNext.Enabled = bEnable;
            btnReplace.Enabled = bEnable;
            btnReplaceAll.Enabled = bEnable;

        }

        private void btnReplaceAll_Click(object sender, System.EventArgs e)
        {

            rdlEdit.ReplaceAll(this, txtFindR.Text, txtReplace.Text, chkMatchCase.Checked);
            txtFindR.Focus();
        }

        private void btnGoto_Click(object sender, System.EventArgs e)
        {
            try
            {
                try
                {
                    int nLine = Int32.Parse(txtLine.Text);
                    rdlEdit.Goto(this, nLine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Strings.FindTab_ShowE_InvalidLN);
                }

                txtLine.Focus();

            }
            catch (Exception er)
            {
                er.ToString();
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void tcFRG_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TabControl tc = (TabControl)sender;
            string tag = (string)tc.TabPages[tc.SelectedIndex].Tag;
            switch (tag)
            {
                case "find":
                    this.AcceptButton = btnNext;
                    this.CancelButton = btnCancel;
                    txtFind.Focus();
                    break;
                case "replace":
                    this.AcceptButton = this.btnFindNext;
                    this.CancelButton = this.bCloseReplace;
                    txtFindR.Focus();
                    break;
                case "goto":
                    this.AcceptButton = btnGoto;
                    this.CancelButton = this.bCloseGoto;
                    txtLine.Focus();
                    break;
            }
        }

        private void tcFRG_Enter(object sender, System.EventArgs e)
        {
            tcFRG_SelectedIndexChanged(this.tcFRG, new EventArgs());
        }

		private void FindTab_FormClosed(object sender, FormClosedEventArgs e)
		{
			rdlEdit.ClearSearchHighlight();
		}

		public void FindNextClick()
		{
			if (!String.IsNullOrEmpty(txtFind.Text))
				btnNext.PerformClick();
		}
    }
}
