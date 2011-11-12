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
using System.Data;
using System.Drawing.Printing;
using fyiReporting.RDL;
using fyiReporting.RdlViewer;

namespace fyiReporting.RdlReader
{
	/// <summary>
	/// RdlReader is a application for displaying reports based on RDL.
	/// </summary>
	public class MDIChild : Form
	{
		private fyiReporting.RdlViewer.RdlViewer rdlViewer1;

		public MDIChild(int width, int height)
		{
			this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
			this.SuspendLayout();
			// 
			// rdlViewer1
			// 
			this.rdlViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rdlViewer1.Location = new System.Drawing.Point(0, 0);
			this.rdlViewer1.Name = "rdlViewer1";
			this.rdlViewer1.Size = new System.Drawing.Size(width, height);
			this.rdlViewer1.TabIndex = 0;
			// 
			// RdlReader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(width, height);
			this.Controls.Add(this.rdlViewer1);
			this.Name = "";
			this.Text = "";
			this.ResumeLayout(false);
		}

		/// <summary>
		/// The RDL file that should be displayed.
		/// </summary>
		public string SourceFile
		{
			get {return this.rdlViewer1.SourceFile;}
			set 
			{
				this.rdlViewer1.SourceFile = value;
				this.rdlViewer1.Refresh();		// force the repaint
			}
		}

		public RdlViewer.RdlViewer Viewer
		{
			get {return this.rdlViewer1;}
		}
	}
}
