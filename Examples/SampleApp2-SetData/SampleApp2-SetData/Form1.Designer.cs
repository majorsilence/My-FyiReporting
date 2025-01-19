namespace SampleApp2_SetData
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
            this.ButtonReloadReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdlViewer1
            // 
            this.rdlViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rdlViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.rdlViewer1.Folder = null;
            this.rdlViewer1.HighlightAll = false;
            this.rdlViewer1.HighlightAllColor = System.Drawing.Color.Fuchsia;
            this.rdlViewer1.HighlightCaseSensitive = false;
            this.rdlViewer1.HighlightItemColor = System.Drawing.Color.Aqua;
            this.rdlViewer1.HighlightPageItem = null;
            this.rdlViewer1.HighlightText = null;
            this.rdlViewer1.Location = new System.Drawing.Point(40, 69);
            this.rdlViewer1.Name = "rdlViewer1";
            this.rdlViewer1.PageCurrent = 1;
            this.rdlViewer1.Parameters = null;
            this.rdlViewer1.ReportName = null;
            this.rdlViewer1.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous;
            this.rdlViewer1.SelectTool = false;
            this.rdlViewer1.ShowFindPanel = false;
            this.rdlViewer1.ShowParameterPanel = true;
            this.rdlViewer1.ShowWaitDialog = true;
            this.rdlViewer1.Size = new System.Drawing.Size(731, 381);
            this.rdlViewer1.SourceFile = null;
            this.rdlViewer1.TabIndex = 0;
            this.rdlViewer1.UseTrueMargins = true;
            this.rdlViewer1.Zoom = 0.8775232F;
            this.rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
            // 
            // ButtonReloadReport
            // 
            this.ButtonReloadReport.Location = new System.Drawing.Point(64, 31);
            this.ButtonReloadReport.Name = "ButtonReloadReport";
            this.ButtonReloadReport.Size = new System.Drawing.Size(141, 23);
            this.ButtonReloadReport.TabIndex = 5;
            this.ButtonReloadReport.Text = "Reload Report";
            this.ButtonReloadReport.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 462);
            this.Controls.Add(this.ButtonReloadReport);
            this.Controls.Add(this.rdlViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private fyiReporting.RdlViewer.RdlViewer rdlViewer1;
        internal System.Windows.Forms.Button ButtonReloadReport;
    }
}

