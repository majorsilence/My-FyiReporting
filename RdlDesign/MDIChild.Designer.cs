using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class MDIChild : Form
	{
		#region Windows Form Designer generated code
        private RdlEditPreview rdlDesigner;
TabPage _Tab;

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIChild));
			this.rdlDesigner = new fyiReporting.RdlDesign.RdlEditPreview();
			this.SuspendLayout();
			// 
			// rdlDesigner
			// 
			resources.ApplyResources(this.rdlDesigner, "rdlDesigner");
			this.rdlDesigner.CurrentInsert = null;
			this.rdlDesigner.Modified = false;
			this.rdlDesigner.Name = "rdlDesigner";
			this.rdlDesigner.SelectedText = "";
			this.rdlDesigner.SelectionTool = false;
			this.rdlDesigner.Zoom = 1F;
			this.rdlDesigner.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
			this.rdlDesigner.OnRdlChanged += new fyiReporting.RdlDesign.RdlEditPreview.RdlChangeHandler(this.rdlDesigner_RdlChanged);
			this.rdlDesigner.OnHeightChanged += new fyiReporting.RdlDesign.DesignCtl.HeightEventHandler(this.rdlDesigner_HeightChanged);
			this.rdlDesigner.OnSelectionChanged += new fyiReporting.RdlDesign.RdlEditPreview.RdlChangeHandler(this.rdlDesigner_SelectionChanged);
			this.rdlDesigner.OnSelectionMoved += new fyiReporting.RdlDesign.RdlEditPreview.RdlChangeHandler(this.rdlDesigner_SelectionMoved);
			this.rdlDesigner.OnReportItemInserted += new fyiReporting.RdlDesign.RdlEditPreview.RdlChangeHandler(this.rdlDesigner_ReportItemInserted);
			this.rdlDesigner.OnDesignTabChanged += new fyiReporting.RdlDesign.RdlEditPreview.RdlChangeHandler(this.rdlDesigner_DesignTabChanged);
			this.rdlDesigner.OnOpenSubreport += new fyiReporting.RdlDesign.DesignCtl.OpenSubreportEventHandler(this.rdlDesigner_OpenSubreport);
			// 
			// MDIChild
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.rdlDesigner);
			this.Name = "MDIChild";
			this.Load += new System.EventHandler(this.MDIChild_Load);
			this.ResumeLayout(false);

		}
		#endregion

        

		
	}
}
