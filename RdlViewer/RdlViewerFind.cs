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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Text;
using fyiReporting.RDL;

namespace fyiReporting.RdlViewer
{
	/// <summary>
	/// RdlViewerFind finds text inside of the RdlViewer control
	/// </summary>
	public class RdlViewerFind : System.Windows.Forms.UserControl
    {
        private Button bClose;
        private Button bFindNext;
        private Button bFindPrevious;
        private CheckBox ckHighlightAll;
        private CheckBox ckMatchCase;
        private Label lFind;
        private Label lStatus;
        private TextBox tbFind;
        private PageItem position = null;

        private RdlViewer _Viewer;

        public RdlViewer Viewer
        {
            get { return _Viewer; }
            set { _Viewer = value; }
        }

        public RdlViewerFind()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.bClose = new System.Windows.Forms.Button();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.bFindNext = new System.Windows.Forms.Button();
            this.bFindPrevious = new System.Windows.Forms.Button();
            this.ckHighlightAll = new System.Windows.Forms.CheckBox();
            this.ckMatchCase = new System.Windows.Forms.CheckBox();
            this.lFind = new System.Windows.Forms.Label();
            this.lStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bClose
            // 
            this.bClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bClose.FlatAppearance.BorderSize = 0;
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bClose.Location = new System.Drawing.Point(2, 4);
            this.bClose.Margin = new System.Windows.Forms.Padding(0);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(18, 18);
            this.bClose.TabIndex = 0;
            this.bClose.Text = "X";
            this.bClose.UseVisualStyleBackColor = false;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // tbFind
            // 
            this.tbFind.Location = new System.Drawing.Point(53, 4);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(118, 20);
            this.tbFind.TabIndex = 1;
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            // 
            // bFindNext
            // 
            this.bFindNext.Location = new System.Drawing.Point(177, 2);
            this.bFindNext.Name = "bFindNext";
            this.bFindNext.Size = new System.Drawing.Size(61, 23);
            this.bFindNext.TabIndex = 2;
            this.bFindNext.Text = "Find Next";
            this.bFindNext.UseVisualStyleBackColor = true;
            this.bFindNext.Click += new System.EventHandler(this.bFindNext_Click);
            // 
            // bFindPrevious
            // 
            this.bFindPrevious.Location = new System.Drawing.Point(242, 2);
            this.bFindPrevious.Name = "bFindPrevious";
            this.bFindPrevious.Size = new System.Drawing.Size(82, 23);
            this.bFindPrevious.TabIndex = 3;
            this.bFindPrevious.Text = "Find Previous";
            this.bFindPrevious.UseVisualStyleBackColor = true;
            this.bFindPrevious.Click += new System.EventHandler(this.bFindPrevious_Click);
            // 
            // ckHighlightAll
            // 
            this.ckHighlightAll.AutoSize = true;
            this.ckHighlightAll.Location = new System.Drawing.Point(330, 6);
            this.ckHighlightAll.Name = "ckHighlightAll";
            this.ckHighlightAll.Size = new System.Drawing.Size(81, 17);
            this.ckHighlightAll.TabIndex = 4;
            this.ckHighlightAll.Text = "Highlight All";
            this.ckHighlightAll.UseVisualStyleBackColor = true;
            this.ckHighlightAll.CheckedChanged += new System.EventHandler(this.ckHighlightAll_CheckedChanged);
            // 
            // ckMatchCase
            // 
            this.ckMatchCase.AutoSize = true;
            this.ckMatchCase.Location = new System.Drawing.Point(410, 6);
            this.ckMatchCase.Name = "ckMatchCase";
            this.ckMatchCase.Size = new System.Drawing.Size(83, 17);
            this.ckMatchCase.TabIndex = 5;
            this.ckMatchCase.Text = "Match Case";
            this.ckMatchCase.UseVisualStyleBackColor = true;
            this.ckMatchCase.CheckedChanged += new System.EventHandler(this.ckMatchCase_CheckedChanged);
            // 
            // lFind
            // 
            this.lFind.AutoSize = true;
            this.lFind.Location = new System.Drawing.Point(20, 7);
            this.lFind.Name = "lFind";
            this.lFind.Size = new System.Drawing.Size(30, 13);
            this.lFind.TabIndex = 6;
            this.lFind.Text = "Find:";
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.ForeColor = System.Drawing.Color.Salmon;
            this.lStatus.Location = new System.Drawing.Point(501, 7);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(0, 13);
            this.lStatus.TabIndex = 7;
            // 
            // RdlViewerFind
            // 
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lStatus);
            this.Controls.Add(this.lFind);
            this.Controls.Add(this.ckMatchCase);
            this.Controls.Add(this.ckHighlightAll);
            this.Controls.Add(this.bFindPrevious);
            this.Controls.Add(this.bFindNext);
            this.Controls.Add(this.tbFind);
            this.Controls.Add(this.bClose);
            this.Name = "RdlViewerFind";
            this.Size = new System.Drawing.Size(740, 27);
            this.VisibleChanged += new System.EventHandler(this.RdlViewerFind_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void bFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        public void FindNext()
        {
            if (_Viewer == null)
                throw new ApplicationException("Viewer property must be set prior to issuing FindNext.");

            if (tbFind.Text.Length == 0)    // must have something to find
                return;

            RdlViewerFinds findOptions =
                ckMatchCase.Checked ?
                RdlViewerFinds.MatchCase :
                RdlViewerFinds.None;

            bool begin = position == null;
            position = _Viewer.Find(tbFind.Text, position, findOptions);
            if (position == null)
            {   
                if (!begin)     // if we didn't start from beginning already; try from beginning
                    position = _Viewer.Find(tbFind.Text, position, findOptions);

                lStatus.Text = position == null ? 
                    "Phrase not found" : "Reached end of report, continued from top";

                _Viewer.HighlightPageItem = position;
                if (position != null)
                    _Viewer.ScrollToPageItem(position);
            }
            else
            {
                lStatus.Text = "";
                _Viewer.HighlightPageItem = position;
                _Viewer.ScrollToPageItem(position);
            }
        }

        private void bFindPrevious_Click(object sender, EventArgs e)
        {
            FindPrevious();
        }

        public void FindPrevious()
        {
            if (_Viewer == null)
                throw new ApplicationException("Viewer property must be set prior to issuing FindPrevious.");

            if (tbFind.Text.Length == 0)    // must have something to find
                return;

            RdlViewerFinds findOptions = RdlViewerFinds.Backward |
                (ckMatchCase.Checked ? RdlViewerFinds.MatchCase : RdlViewerFinds.None);

            bool begin = position == null;
            position = _Viewer.Find(tbFind.Text, position, findOptions);
            if (position == null)
            {
                if (!begin)     // if we didn't start from beginning already; try from bottom
                    position = _Viewer.Find(tbFind.Text, position, findOptions);

                lStatus.Text = position == null ?
                    "Phrase not found" : "Reached top of report, continued from end";

                _Viewer.HighlightPageItem = position;
                if (position != null)
                    _Viewer.ScrollToPageItem(position);
            }
            else
            {
                lStatus.Text = "";
                _Viewer.HighlightPageItem = position;
                _Viewer.ScrollToPageItem(position);
            }
        }

        private void RdlViewerFind_VisibleChanged(object sender, EventArgs e)
        {
            lStatus.Text = "";
            if (this.Visible)
            {
                _Viewer.HighlightText = tbFind.Text;
                tbFind.Focus();
                FindNext();         // and go find the contents of the textbox
            }
            else
            {   // turn off any highlighting when find control not visible
                _Viewer.HighlightPageItem = position = null;
                _Viewer.HighlightText = null;
                _Viewer.HighlightAll = false;
                ckHighlightAll.Checked = false;
            }
        }

        private void tbFind_TextChanged(object sender, EventArgs e)
        {
            lStatus.Text = "";
            position = null;        // reset position when edit changes?? todo not really
            _Viewer.HighlightText = tbFind.Text;
            ckHighlightAll.Enabled = bFindNext.Enabled = bFindPrevious.Enabled =
                    tbFind.Text.Length > 0;
            if (tbFind.Text.Length > 0)
                FindNext();
        }

        private void ckHighlightAll_CheckedChanged(object sender, EventArgs e)
        {
            _Viewer.HighlightAll = ckHighlightAll.Checked;
        }

        private void ckMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            _Viewer.HighlightCaseSensitive = ckMatchCase.Checked;
        }
    }
}
