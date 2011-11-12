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

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// DialogListOfStrings: puts up a dialog that lets a user enter a list of strings
	/// </summary>
	public class DialogListOfStrings : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbStrings;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			int l=0;
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
			this.tbStrings = new System.Windows.Forms.TextBox();
			this.bCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOK.Location = new System.Drawing.Point(96, 192);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 0;
			this.bOK.Text = "OK";
			// 
			// tbStrings
			// 
			this.tbStrings.Location = new System.Drawing.Point(8, 40);
			this.tbStrings.Multiline = true;
			this.tbStrings.Name = "tbStrings";
			this.tbStrings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbStrings.Size = new System.Drawing.Size(256, 144);
			this.tbStrings.TabIndex = 9;
			this.tbStrings.Text = "";
			// 
			// bCancel
			// 
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(192, 192);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 10;
			this.bCancel.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(240, 23);
			this.label1.TabIndex = 11;
			this.label1.Text = "Enter separate values on multiple lines below";
			// 
			// DialogListOfStrings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(282, 224);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.tbStrings);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogListOfStrings";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}
		#endregion

		private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs ea)
		{
			LinkLabel lnk = (LinkLabel) sender;
			lnk.Links[lnk.Links.IndexOf(ea.Link)].Visited = true;
			System.Diagnostics.Process.Start(lnk.Tag.ToString());
		}
	}

}
