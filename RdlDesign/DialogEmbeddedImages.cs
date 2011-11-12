using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using fyiReporting.RDL;


namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for DialogDataSourceRef.
	/// </summary>
	internal class DialogEmbeddedImages : System.Windows.Forms.Form
	{
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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal DialogEmbeddedImages(DesignXmlDraw draw)
		{
			_Draw = draw;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			InitValues();
		}

		private void InitValues()
		{
			//
			// Obtain the existing DataSets info
			//
			XmlNode rNode = _Draw.GetReportNode();
			XmlNode eiNode = _Draw.GetNamedChildNode(rNode, "EmbeddedImages");
			if (eiNode == null)
				return;
			foreach (XmlNode iNode in eiNode)
			{	
				if (iNode.Name != "EmbeddedImage")
					continue;
				XmlAttribute nAttr = iNode.Attributes["Name"];
				if (nAttr == null)	// shouldn't really happen
					continue;

				EmbeddedImageValues eiv = new EmbeddedImageValues(nAttr.Value);
				eiv.MIMEType = _Draw.GetElementValue(iNode, "MIMEType", "image/png");
				eiv.ImageData = _Draw.GetElementValue(iNode, "ImageData", "");
				this.lbImages.Items.Add(eiv);
			}
			if (lbImages.Items.Count > 0)
				lbImages.SelectedIndex = 0;
			else
				this.bOK.Enabled = false;		
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
		public void Apply()
		{
			XmlNode rNode = _Draw.GetReportNode();
			_Draw.RemoveElement(rNode, "EmbeddedImages");	// remove old EmbeddedImages
			if (this.lbImages.Items.Count <= 0)
				return;			// nothing in list?  all done

			XmlNode eiNode = _Draw.SetElement(rNode, "EmbeddedImages", null);
			foreach (EmbeddedImageValues eiv in lbImages.Items)
			{
				if (eiv.Name == null || eiv.Name.Length <= 0)
					continue;					// shouldn't really happen
				XmlNode iNode = _Draw.CreateElement(eiNode, "EmbeddedImage", null);

				// Create the name attribute
				_Draw.SetElementAttribute(iNode, "Name", eiv.Name);
				_Draw.SetElement(iNode, "MIMEType", eiv.MIMEType);
				_Draw.SetElement(iNode, "ImageData", eiv.ImageData);
			}
		}

		private void bPaste_Click(object sender, System.EventArgs e)
		{
			// Make sure we have an image on the clipboard
			IDataObject iData = Clipboard.GetDataObject();
			if (iData == null || !iData.GetDataPresent(DataFormats.Bitmap))
			{
				MessageBox.Show(this, "Copy image into clipboard before attempting to paste.", "Image");
				return;
			}

			System.Drawing.Bitmap img = (System.Drawing.Bitmap) iData.GetData(DataFormats.Bitmap);

			// convert the image to the png format and create a base 64	string representation
			string imagedata=GetBase64Image(img);
			img.Dispose();

			if (imagedata == null)
				return;

			EmbeddedImageValues eiv = new EmbeddedImageValues("embeddedimage");
			eiv.MIMEType = "image/png";
			eiv.ImageData = imagedata;
			int cur = this.lbImages.Items.Add(eiv);

			lbImages.SelectedIndex = cur;

			this.tbEIName.Focus();
		}

		private void bImport_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();	    
			ofd.Filter = "Bitmap Files (*.bmp)|*.bmp" +
				"|JPEG (*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif" +
				"|GIF (*.gif)|*.gif" +
				"|TIFF (*.tif;*.tiff)|*.tif;*.tiff" +
				"|PNG (*.png)|*.png" +
				"|All Picture Files|*.bmp;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.tif;*.tiff;*.png" +
				"|All files (*.*)|*.*";
			ofd.FilterIndex = 6;
			ofd.CheckFileExists = true;
			ofd.Multiselect = true;
            try
            {
                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                // need to create a new embedded image(s)
                int cur = 0;
                foreach (string filename in ofd.FileNames)
                {
                    Stream strm = null;
                    System.Drawing.Image im = null;
                    string imagedata = null;
                    try
                    {
                        strm = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                        im = System.Drawing.Image.FromStream(strm);
                        imagedata = this.GetBase64Image(im);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Image");
                    }
                    finally
                    {
                        if (strm != null)
                            strm.Close();
                        if (im != null)
                            im.Dispose();
                    }

                    if (imagedata != null)
                    {
                        FileInfo fi = new FileInfo(filename);

                        string fname;
                        int offset = fi.Name.IndexOf('.');
                        if (offset >= 0)
                            fname = fi.Name.Substring(0, offset);
                        else
                            fname = fi.Name;

                        if (!ReportNames.IsNameValid(fname))
                            fname = "embeddedimage";
                        // Now check to see if we already have one with that name
                        int index = 1;
                        bool bDup = true;
                        while (bDup)
                        {
                            bDup = false;
                            foreach (EmbeddedImageValues ev in lbImages.Items)
                            {
                                if (fname == ev.Name)
                                {
                                    bDup = true;
                                    break;
                                }
                            }
                            if (bDup)
                            {	// we have a duplicate name; try adding an index number
                                fname = Regex.Replace(fname, "[0-9]*", "");		// remove old numbers (side effect removes all numbers)
                                fname += index.ToString();
                                index++;
                            }
                        }

                        EmbeddedImageValues eiv = new EmbeddedImageValues(fname);
                        eiv.MIMEType = "image/png";
                        eiv.ImageData = imagedata;
                        cur = this.lbImages.Items.Add(eiv);
                    }
                }
                lbImages.SelectedIndex = cur;
            }
            finally
            {
                ofd.Dispose();
            }
			this.tbEIName.Focus();
		}

		private void bRemove_Click(object sender, System.EventArgs e)
		{
			int cur = lbImages.SelectedIndex;
			if (cur < 0)
				return;
			lbImages.Items.RemoveAt(cur);
			if (lbImages.Items.Count <= 0)
				return;
			cur--;
			if (cur < 0)
				cur = 0;
			lbImages.SelectedIndex = cur;
		}

		private void lbImages_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int cur = lbImages.SelectedIndex;
			if (cur < 0)
				return;

			EmbeddedImageValues eiv = lbImages.Items[cur] as EmbeddedImageValues;
			if (eiv == null)
				return;

			tbEIName.Text = eiv.Name;
			lbMIMEType.Text = eiv.MIMEType;
			this.pictureImage.Image = GetImage(eiv.ImageData);
		}
		
		private Image GetImage(string imdata)
		{
			byte[] ba = Convert.FromBase64String(imdata);
			
			Stream strm=null;
			System.Drawing.Image im=null;
			try 
			{
				strm = new MemoryStream(ba);
				im = System.Drawing.Image.FromStream(strm);	 
			}
			catch (Exception e)
			{
				MessageBox.Show(this, e.Message, "Error converting image data");
			}
			finally
			{
				if (strm != null)
					strm.Close();
			}
			return im;
		}	   

		private string GetBase64Image(Image img)
		{
			string imagedata=null;
			try
			{
				MemoryStream ostrm = new MemoryStream();
				ImageFormat imf = ImageFormat.Png;
				img.Save(ostrm, imf);
				byte[] ba = ostrm.ToArray();
				ostrm.Close();
				imagedata = Convert.ToBase64String(ba);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Image");
				imagedata=null;
			}
			return imagedata;
		}

		private void tbEIName_TextChanged(object sender, System.EventArgs e)
		{
			int cur = lbImages.SelectedIndex;
			if (cur < 0)
				return;

			EmbeddedImageValues eiv = lbImages.Items[cur] as EmbeddedImageValues;
			if (eiv == null)
				return;

			if (eiv.Name == tbEIName.Text)
				return;

			eiv.Name = tbEIName.Text;
			// text doesn't change in listbox; force change by removing and re-adding item
			lbImages.BeginUpdate();
			lbImages.Items.RemoveAt(cur);
			lbImages.Items.Insert(cur, eiv);
			lbImages.SelectedIndex = cur;
			lbImages.EndUpdate();

		}

		private void bOK_Click(object sender, System.EventArgs e)
		{
			// Verify there are no duplicate names in the embedded list
			Hashtable ht = new Hashtable(lbImages.Items.Count+1);
			foreach (EmbeddedImageValues eiv in lbImages.Items)
			{
				if (eiv.Name == null || eiv.Name.Length == 0)
				{
					MessageBox.Show(this, "Name must be specified for all embedded images.", "Image");
					return;
				}

				if (!ReportNames.IsNameValid(eiv.Name))
				{
					MessageBox.Show(this, 
						string.Format("Name '{0}' contains invalid characters.", eiv.Name), "Image");
					return;
				}

				string name = (string) ht[eiv.Name];
				if (name != null)
				{
					MessageBox.Show(this, 
						string.Format("Each embedded image must have a unique name. '{0}' is repeated.", eiv.Name), "Image");
					return;
				}
				ht.Add(eiv.Name, eiv.Name);
			}

			Apply();
			DialogResult = DialogResult.OK;
		}

		private void tbEIName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!ReportNames.IsNameValid(tbEIName.Text))
			{
				e.Cancel = true;
				MessageBox.Show(this, 
					string.Format("Name '{0}' contains invalid characters.", tbEIName.Text), "Image");
			}
		}
	}

	class EmbeddedImageValues
	{
		string _Name;
		string _ImageData;		// the embedded image value
		string _MIMEType;

		internal EmbeddedImageValues(string name)
		{
			_Name = name;
		}

		internal string Name
		{
			get {return _Name;}
			set {_Name = value;}
		}

		internal string ImageData
		{
			get {return _ImageData;}
			set {_ImageData = value;}
		}

		internal string MIMEType
		{
			get {return _MIMEType;}
			set {_MIMEType = value;}
		}

		override public string ToString()
		{
			return _Name;
		}
	}
}
