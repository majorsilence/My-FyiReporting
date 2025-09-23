﻿namespace Majorsilence.Reporting.RdlDesign
{
    partial class UserZoomControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DoubleBuffered = true;
            this.BtnPlus = new System.Windows.Forms.Button();
            this.BtnMinus = new System.Windows.Forms.Button();
            this.TxtZoomValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnPlus
            // 
            this.BtnPlus.AutoSize = true;
            this.BtnPlus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPlus.Location = new System.Drawing.Point(86, 1);
            this.BtnPlus.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPlus.Name = "BtnPlus";
            this.BtnPlus.Size = new System.Drawing.Size(36, 30);
            this.BtnPlus.TabIndex = 0;
            this.BtnPlus.TabStop = false;
            this.BtnPlus.Text = "+";
            this.BtnPlus.UseVisualStyleBackColor = true;
            this.BtnPlus.Click += new System.EventHandler(this.BtnPlus_Click);
            // 
            // BtnMinus
            // 
            this.BtnMinus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnMinus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMinus.Location = new System.Drawing.Point(2, 1);
            this.BtnMinus.Margin = new System.Windows.Forms.Padding(0);
            this.BtnMinus.Name = "BtnMinus";
            this.BtnMinus.Size = new System.Drawing.Size(36, 30);
            this.BtnMinus.TabIndex = 1;
            this.BtnMinus.TabStop = false;
            this.BtnMinus.Text = "-";
            this.BtnMinus.UseVisualStyleBackColor = true;
            this.BtnMinus.Click += new System.EventHandler(this.BtnMinus_Click);
            // 
            // TxtZoomValue
            // 
            this.TxtZoomValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtZoomValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtZoomValue.Location = new System.Drawing.Point(38, 6);
            this.TxtZoomValue.Margin = new System.Windows.Forms.Padding(0);
            this.TxtZoomValue.Name = "TxtZoomValue";
            this.TxtZoomValue.Size = new System.Drawing.Size(48, 20);
            this.TxtZoomValue.TabIndex = 2;
            this.TxtZoomValue.TabStop = false;
            this.TxtZoomValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UserZoomControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.TxtZoomValue);
            this.Controls.Add(this.BtnMinus);
            this.Controls.Add(this.BtnPlus);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserZoomControl";
            this.Size = new System.Drawing.Size(122, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnPlus;
        private System.Windows.Forms.Button BtnMinus;
        private System.Windows.Forms.TextBox TxtZoomValue;
    }
}
