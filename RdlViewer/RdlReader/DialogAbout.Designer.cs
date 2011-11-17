using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlReader
{
    public partial class DialogAbout : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.Label label3;
private System.Windows.Forms.Label label4;
private System.Windows.Forms.LinkLabel linkLabel1;
private System.Windows.Forms.LinkLabel linkLabel2;
private System.Windows.Forms.PictureBox pictureBox1;
private System.Windows.Forms.TextBox tbLicense;
private System.Windows.Forms.Label lVersion;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DialogAbout));
			this.bOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lVersion = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tbLicense = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bOK.Location = new System.Drawing.Point(200, 272);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 0;
			this.bOK.Text = "OK";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(240, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "fyiReporting Reader";
			// 
			// lVersion
			// 
			this.lVersion.Location = new System.Drawing.Point(288, 48);
			this.lVersion.Name = "lVersion";
			this.lVersion.Size = new System.Drawing.Size(104, 24);
			this.lVersion.TabIndex = 2;
			this.lVersion.Text = "Version 1.9.6";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 23);
			this.label3.TabIndex = 3;
			this.label3.Text = "Website:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(240, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 23);
			this.label4.TabIndex = 4;
			this.label4.Text = "E-mail:";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(72, 88);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(144, 16);
			this.linkLabel1.TabIndex = 6;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Tag = "http://www.fyireporting.com";
			this.linkLabel1.Text = "http://www.fyireporting.com";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_LinkClicked);
			// 
			// linkLabel2
			// 
			this.linkLabel2.Location = new System.Drawing.Point(280, 88);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(152, 16);
			this.linkLabel2.TabIndex = 7;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Tag = "mailto:comments@fyireporting.com";
			this.linkLabel2.Text = "comments@fyireporting.com";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_LinkClicked);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(212, 72);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// tbLicense
			// 
			this.tbLicense.Location = new System.Drawing.Point(16, 120);
			this.tbLicense.Multiline = true;
			this.tbLicense.Name = "tbLicense";
			this.tbLicense.ReadOnly = true;
			this.tbLicense.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbLicense.Size = new System.Drawing.Size(448, 136);
			this.tbLicense.TabIndex = 9;
			this.tbLicense.Text = "";
			// 
			// DialogAbout
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bOK;
			this.ClientSize = new System.Drawing.Size(482, 304);
			this.Controls.Add(this.tbLicense);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lVersion);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogAbout";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion

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
	}
}
