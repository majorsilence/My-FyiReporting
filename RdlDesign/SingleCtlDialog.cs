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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace fyiReporting.RdlDesign
{
	internal enum SingleCtlTypeEnum
	{
        InteractivityCtl, VisibilityCtl, BorderCtl, FontCtl, BackgroundCtl, BackgroundImage,
        ReportParameterCtl, ReportCodeCtl, ReportModulesClassesCtl, ImageCtl, SubreportCtl,
        FiltersCtl, SortingCtl, GroupingCtl
	}

	/// <summary>
	/// Summary description for PropertyDialog.
	/// </summary>
	internal class SingleCtlDialog : System.Windows.Forms.Form
	{
        private DesignCtl _DesignCtl;
		private DesignXmlDraw _Draw;		// design draw 
		private List<XmlNode> _Nodes;			// selected nodes
        private SingleCtlTypeEnum _Type;
		IProperty _Ctl;
        private Button bOK;
        private Button bCancel;
        private Panel pMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal SingleCtlDialog(DesignCtl dc, DesignXmlDraw dxDraw, List<XmlNode> sNodes, 
            SingleCtlTypeEnum type, string[] names)
		{
            this._Type = type;
            this._DesignCtl = dc;
			this._Draw = dxDraw;
			this._Nodes = sNodes;
            
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

 			//   Add the control for the selected ReportItems
            //     We could have forced the user to create this (and maybe should have) 
            //     instead of using an enum.
            UserControl uc = null;
            string title = null;
            switch (type)
            {
                case SingleCtlTypeEnum.InteractivityCtl:
                    title = " - Interactivty";
                    uc = new InteractivityCtl(dxDraw, sNodes);
                    break;
                case SingleCtlTypeEnum.VisibilityCtl:
                    title = " - Visibility";
                    uc = new VisibilityCtl(dxDraw, sNodes);
                    break;
                case SingleCtlTypeEnum.BorderCtl:
                    title = " - Borders";
                    uc = new StyleBorderCtl(dxDraw, names, sNodes);
                    break;
                case SingleCtlTypeEnum.FontCtl:
                    title = " - Font";
                    uc = new FontCtl(dxDraw, names, sNodes);
                    break;
                case SingleCtlTypeEnum.BackgroundCtl:
                    title = " - Background";
                    uc = new BackgroundCtl(dxDraw, names, sNodes);
                    break;
                case SingleCtlTypeEnum.ImageCtl:
                    title = " - Image";
                    uc = new ImageCtl(dxDraw, sNodes);
                    break;
                case SingleCtlTypeEnum.SubreportCtl:
                    title = " - Subreport";
                    uc = new SubreportCtl(dxDraw, sNodes[0]);
                    break;
                case SingleCtlTypeEnum.FiltersCtl:
                    title = " - Filter";
                    uc = new FiltersCtl(dxDraw, sNodes[0]);
                    break;
                case SingleCtlTypeEnum.SortingCtl:
                    title = " - Sorting";
                    uc = new SortingCtl(dxDraw, sNodes[0]);
                    break;
                case SingleCtlTypeEnum.GroupingCtl:
                    title = " - Grouping";
                    uc = new GroupingCtl(dxDraw, sNodes[0]);
                    break;
                case SingleCtlTypeEnum.ReportParameterCtl:
                    title = " - Report Parameters";
                    uc = new ReportParameterCtl(dxDraw);
                    break;
                case SingleCtlTypeEnum.ReportCodeCtl:
                    title = " - Code";
                    uc = new CodeCtl(dxDraw);
                    break;
                case SingleCtlTypeEnum.ReportModulesClassesCtl:
                    title = " - Modules and Classes";
                    uc = new ModulesClassesCtl(dxDraw);
                    break;
            }
            _Ctl = uc as IProperty;
            if (title != null)
                this.Text = this.Text + title;

            if (uc == null)
                return;
            int h = uc.Height;
            int w = uc.Width;
            uc.Top = 0;
            uc.Left = 0;
            uc.Dock = DockStyle.Fill;
            uc.Parent = this.pMain;
            this.Height = h + (this.Height - pMain.Height);
            this.Width = w + (this.Width - pMain.Width);
            this.ResumeLayout(true);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.pMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Location = new System.Drawing.Point(452, 410);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 0;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.CausesValidation = false;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(542, 410);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // pMain
            // 
            this.pMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pMain.Location = new System.Drawing.Point(3, 3);
            this.pMain.Name = "pMain";
            this.pMain.Size = new System.Drawing.Size(614, 401);
            this.pMain.TabIndex = 2;
            // 
            // SingleCtlDialog
            // 
            this.AcceptButton = this.bOK;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(620, 436);
            this.Controls.Add(this.pMain);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SingleCtlDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.ResumeLayout(false);

		}
		#endregion

		private void bOK_Click(object sender, System.EventArgs e)
		{
            string c = "";
            switch (_Type)
            {
                case SingleCtlTypeEnum.InteractivityCtl:
                    c = "Interactivity change";
                    break;
                case SingleCtlTypeEnum.VisibilityCtl:
                    c = "Visibility change";
                    break;
                case SingleCtlTypeEnum.BorderCtl:
                    c = "Border change";
                    break;
                case SingleCtlTypeEnum.FontCtl:
                    c = "Appearance change";
                    break;
                case SingleCtlTypeEnum.BackgroundCtl:
                case SingleCtlTypeEnum.BackgroundImage:
                    c = "Background change";
                    break;
                case SingleCtlTypeEnum.FiltersCtl:
                    c = "Filters change";
                    break;
                case SingleCtlTypeEnum.SortingCtl:
                    c = "Sort change";
                    break;
                case SingleCtlTypeEnum.GroupingCtl:
                    c = "Grouping change";
                    break;
                case SingleCtlTypeEnum.ReportCodeCtl:
                    c = "Report code change";
                    break;
                case SingleCtlTypeEnum.ImageCtl:
                    c = "Image change";
                    break;
                case SingleCtlTypeEnum.SubreportCtl:
                    c = "Subreport change";
                    break;
                case SingleCtlTypeEnum.ReportModulesClassesCtl:
                    c = "Report Modules/Classes change";
                    break;
            }
            this._DesignCtl.StartUndoGroup(c);

            this._Ctl.Apply();

            this._DesignCtl.EndUndoGroup(true);
            _DesignCtl.SignalReportChanged();
            _Draw.Invalidate();

			this.DialogResult = DialogResult.OK;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


	}
}
