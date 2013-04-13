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
            this.rdlUserControl1 = new fyiReporting.RdlDesign.RdlUserControl();
            this.SuspendLayout();
            // 
            // rdlUserControl1
            // 
            this.rdlUserControl1.Location = new System.Drawing.Point(29, 24);
            this.rdlUserControl1.Name = "rdlUserControl1";
            this.rdlUserControl1.Size = new System.Drawing.Size(998, 469);
            this.rdlUserControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 655);
            this.Controls.Add(this.rdlUserControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private fyiReporting.RdlDesign.RdlUserControl rdlUserControl1;
    }
}