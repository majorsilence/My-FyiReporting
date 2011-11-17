using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fyiReporting.RdlDesign
{
    internal partial class DialogEmbeddedImages : System.Windows.Forms.Form
	{
		#region Windows Form Designer generated code
		DesignXmlDraw _Draw;
private System.Windows.Forms.Button bOK;
private System.Windows.Forms.Button bCancel;
private System.Windows.Forms.Button bRemove;
private System.Windows.Forms.Label label1;
private System.Windows.Forms.Label lDataProvider;
private System.Windows.Forms.ListBox lbImages;
private System.Windows.Forms.Button bImport;
private System.Windows.Forms.TextBox tbEIName;
private System.Windows.Forms.Button bPaste;
private System.Windows.Forms.PictureBox pictureImage;
private System.Windows.Forms.Label lbMIMEType;
private System.ComponentModel.Container components = null;

		private void InitializeComponent()
		{
			this.lDataProvider = new System.Windows.Forms.Label();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.lbImages = new System.Windows.Forms.ListBox();
			this.bRemove = new System.Windows.Forms.Button();
			this.bImport = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbEIName = new System.Windows.Forms.TextBox();
			this.bPaste = new System.Windows.Forms.Button();
			this.pictureImage = new System.Windows.Forms.PictureBox();
			this.lbMIMEType = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lDataProvider
			// 
			this.lDataProvider.Location = new System.Drawing.Point(216, 72);
			this.lDataProvider.Name = "lDataProvider";
			this.lDataProvider.Size = new System.Drawing.Size(72, 23);
			this.lDataProvider.TabIndex = 7;
			this.lDataProvider.Text = "MIME Type:";
			// 
			// bOK
			// 
			this.bOK.Location = new System.Drawing.Point(272, 344);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 5;
			this.bOK.Text = "OK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			this.bCancel.CausesValidation = false;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new System.Drawing.Point(368, 344);
			this.bCancel.Name = "bCancel";
			this.bCancel.TabIndex = 6;
			this.bCancel.Text = "Cancel";
			// 
			// lbImages
			// 
			this.lbImages.Location = new System.Drawing.Point(16, 8);
			this.lbImages.Name = "lbImages";
			this.lbImages.Size = new System.Drawing.Size(120, 95);
			this.lbImages.TabIndex = 0;
			this.lbImages.SelectedIndexChanged += new System.EventHandler(this.lbImages_SelectedIndexChanged);
			// 
			// bRemove
			// 
			this.bRemove.Location = new System.Drawing.Point(144, 80);
			this.bRemove.Name = "bRemove";
			this.bRemove.Size = new System.Drawing.Size(56, 23);
			this.bRemove.TabIndex = 3;
			this.bRemove.Text = "Remove";
			this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
			// 
			// bImport
			// 
			this.bImport.Location = new System.Drawing.Point(144, 8);
			this.bImport.Name = "bImport";
			this.bImport.Size = new System.Drawing.Size(56, 23);
			this.bImport.TabIndex = 1;
			this.bImport.Text = "Import...";
			this.bImport.Click += new System.EventHandler(this.bImport_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(216, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 23);
			this.label1.TabIndex = 22;
			this.label1.Text = "Embedded Image Name";
			// 
			// tbEIName
			// 
			this.tbEIName.Location = new System.Drawing.Point(216, 24);
			this.tbEIName.Name = "tbEIName";
			this.tbEIName.Size = new System.Drawing.Size(216, 20);
			this.tbEIName.TabIndex = 4;
			this.tbEIName.Text = "";
			this.tbEIName.Validating += new System.ComponentModel.CancelEventHandler(this.tbEIName_Validating);
			this.tbEIName.TextChanged += new System.EventHandler(this.tbEIName_TextChanged);
			// 
			// bPaste
			// 
			this.bPaste.Location = new System.Drawing.Point(144, 44);
			this.bPaste.Name = "bPaste";
			this.bPaste.Size = new System.Drawing.Size(56, 23);
			this.bPaste.TabIndex = 2;
			this.bPaste.Text = "Paste";
			this.bPaste.Click += new System.EventHandler(this.bPaste_Click);
			// 
			// pictureImage
			// 
			this.pictureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureImage.Location = new System.Drawing.Point(16, 120);
			this.pictureImage.Name = "pictureImage";
			this.pictureImage.Size = new System.Drawing.Size(424, 208);
			this.pictureImage.TabIndex = 24;
			this.pictureImage.TabStop = false;
			// 
			// lbMIMEType
			// 
			this.lbMIMEType.Location = new System.Drawing.Point(296, 72);
			this.lbMIMEType.Name = "lbMIMEType";
			this.lbMIMEType.TabIndex = 25;
			this.lbMIMEType.Text = "image/png";
			// 
			// DialogEmbeddedImages
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bCancel;
			this.ClientSize = new System.Drawing.Size(456, 374);
			this.Controls.Add(this.lbMIMEType);
			this.Controls.Add(this.pictureImage);
			this.Controls.Add(this.bPaste);
			this.Controls.Add(this.tbEIName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bRemove);
			this.Controls.Add(this.bImport);
			this.Controls.Add(this.lbImages);
			this.Controls.Add(this.bCancel);
			this.Controls.Add(this.bOK);
			this.Controls.Add(this.lDataProvider);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogEmbeddedImages";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Embedded Images";
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
