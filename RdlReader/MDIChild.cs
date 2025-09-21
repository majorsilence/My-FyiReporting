
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;
using Majorsilence.Reporting.Rdl;
using Majorsilence.Reporting.RdlViewer;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.RdlReader
{
    /// <summary>
    /// RdlReader is a application for displaying reports based on RDL.
    /// </summary>
    public partial class MDIChild 
    {


        public MDIChild(int width, int height)
        {
            this.rdlViewer1 = new Majorsilence.Reporting.RdlViewer.RdlViewer();
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
        public Uri SourceFile
        {
            get { return this.rdlViewer1.SourceFile; }
        }

        public async Task SetSourceFile(Uri file)
        {
            await this.rdlViewer1.SetSourceFile(file);
            this.rdlViewer1.Refresh();		// force the repaint
        }

        /// <summary>
        /// Gets or sets the report parameters. Each parameter should be separated with an & character.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public string Parameters
        {
            get { return this.rdlViewer1.Parameters; }
            set
            {
                this.rdlViewer1.Parameters = value;
            }
        }


        public RdlViewer.RdlViewer Viewer
        {
            get { return this.rdlViewer1; }
        }

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIChild));
			this.SuspendLayout();
			// 
			// MDIChild
			// 
			resources.ApplyResources(this, "$this");
			this.Name = "MDIChild";
			this.ResumeLayout(false);

		}
    }
}
