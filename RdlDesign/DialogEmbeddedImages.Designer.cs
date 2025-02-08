using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Majorsilence.Reporting.RdlDesign
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogEmbeddedImages));
			this.lDataProvider = new System.Windows.Forms.Label();
			this.bOK = new System.Windows.Forms.Button();
			this.bCancel = new System.Windows.Forms.Button();
			this.lbImages = new System.Windows.Forms.ListBox();
			this.bRemove = new System.Windows.Forms.Button();
			this.bImport = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbEIName = new System.Windows.Forms.TextBox();
			this.bPaste = new System.Windows.Forms.Button();
			this.lbMIMEType = new System.Windows.Forms.Label();
			this.pictureImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureImage)).BeginInit();
			this.SuspendLayout();
			// 
			// lDataProvider
			// 
			resources.ApplyResources(this.lDataProvider, "lDataProvider");
			this.lDataProvider.Name = "lDataProvider";
			// 
			// bOK
			// 
			resources.ApplyResources(this.bOK, "bOK");
			this.bOK.Name = "bOK";
			this.bOK.Click += new System.EventHandler(this.bOK_Click);
			// 
			// bCancel
			// 
			resources.ApplyResources(this.bCancel, "bCancel");
			this.bCancel.CausesValidation = false;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Name = "bCancel";
			// 
			// lbImages
			// 
			resources.ApplyResources(this.lbImages, "lbImages");
			this.lbImages.Name = "lbImages";
			this.lbImages.SelectedIndexChanged += new System.EventHandler(this.lbImages_SelectedIndexChanged);
			// 
			// bRemove
			// 
			resources.ApplyResources(this.bRemove, "bRemove");
			this.bRemove.Name = "bRemove";
			this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
			// 
			// bImport
			// 
			resources.ApplyResources(this.bImport, "bImport");
			this.bImport.Name = "bImport";
			this.bImport.Click += new System.EventHandler(this.bImport_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tbEIName
			// 
			resources.ApplyResources(this.tbEIName, "tbEIName");
			this.tbEIName.Name = "tbEIName";
			this.tbEIName.TextChanged += new System.EventHandler(this.tbEIName_TextChanged);
			this.tbEIName.Validating += new System.ComponentModel.CancelEventHandler(this.tbEIName_Validating);
			// 
			// bPaste
			// 
			resources.ApplyResources(this.bPaste, "bPaste");
			this.bPaste.Name = "bPaste";
			this.bPaste.Click += new System.EventHandler(this.bPaste_Click);
			// 
			// lbMIMEType
			// 
			resources.ApplyResources(this.lbMIMEType, "lbMIMEType");
			this.lbMIMEType.Name = "lbMIMEType";
			// 
			// pictureImage
			// 
			resources.ApplyResources(this.pictureImage, "pictureImage");
			this.pictureImage.Name = "pictureImage";
			this.pictureImage.TabStop = false;
			// 
			// DialogEmbeddedImages
			// 
			this.AcceptButton = this.bOK;
			resources.ApplyResources(this, "$this");
			this.CancelButton = this.bCancel;
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
			((System.ComponentModel.ISupportInitialize)(this.pictureImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
