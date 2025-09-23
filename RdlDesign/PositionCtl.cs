
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Majorsilence.Reporting.RdlDesign.Resources;

namespace Majorsilence.Reporting.RdlDesign
{
	/// <summary>
	/// Summary description for ReportCtl.
	/// </summary>
	internal class PositionCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
		private DesignXmlDraw _Draw;
		bool fName, fLeft, fTop, fWidth, fHeight, fZIndex, fColSpan;
		bool fCanGrow, fCanShrink, fHideDuplicates, fToggleImage, fDataElementStyle;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox tbWidth;
		private System.Windows.Forms.TextBox tbTop;
		private System.Windows.Forms.TextBox tbLeft;
		private System.Windows.Forms.NumericUpDown tbZIndex;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbHeight;
		private System.Windows.Forms.GroupBox gbPosition;
		private System.Windows.Forms.Label lblColSpan;
		private System.Windows.Forms.NumericUpDown tbColSpan;
		private System.Windows.Forms.GroupBox gbText;
		private System.Windows.Forms.CheckBox chkCanGrow;
		private System.Windows.Forms.CheckBox chkCanShrink;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbHideDuplicates;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbDataElementStyle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbToggleImage;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal PositionCtl(DesignXmlDraw dxDraw, List<XmlNode> ris)
		{
			_ReportItems = ris;
			_Draw = dxDraw;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
			XmlNode riNode = _ReportItems[0];
			XmlNode tcell = null;

			if (_ReportItems.Count > 1)
			{
				tbName.Text = Strings.PositionCtl_InitValues_GroupSelected;
				tbName.Enabled = false;
				lblColSpan.Visible = tbColSpan.Visible = false;
			}
			else
			{
				XmlAttribute xa = riNode.Attributes["Name"];
				tbName.Text = xa == null? "": xa.Value;
				XmlNode ris = riNode.ParentNode;
				tcell = ris.ParentNode;
				if (tcell.Name != "TableCell")
					tcell = null;
			}
			
			this.tbZIndex.Value = Convert.ToInt32(_Draw.GetElementValue(riNode, "ZIndex", "0"));

			if (tcell != null)
			{
				gbPosition.Visible = false;
				this.gbText.Location = gbPosition.Location;
				string colspan = _Draw.GetElementValue(tcell, "ColSpan", "1");
				tbColSpan.Value = Convert.ToDecimal(colspan);
			}
			else
			{
				lblColSpan.Visible = tbColSpan.Visible = false;
				tbLeft.Text = _Draw.GetElementValue(riNode, "Left", "0pt");
				tbTop.Text = _Draw.GetElementValue(riNode, "Top", "0pt");
				tbWidth.Text = _Draw.GetElementValue(riNode, "Width", "");
				tbHeight.Text = _Draw.GetElementValue(riNode, "Height", "");
			}

			if (riNode.Name == "Textbox")
			{
				this.cbDataElementStyle.Text = _Draw.GetElementValue(riNode, "DataElementStyle", "Auto");
				cbHideDuplicates.Items.Add("");
				object[] dsn = _Draw.DataSetNames;
				if (dsn != null)
					cbHideDuplicates.Items.AddRange(dsn);
				object[] grps = _Draw.GroupingNames;
				if (grps != null)
					cbHideDuplicates.Items.AddRange(grps);
				this.cbHideDuplicates.Text = _Draw.GetElementValue(riNode, "HideDuplicates", "");
				this.chkCanGrow.Checked = _Draw.GetElementValue(riNode, "CanGrow", "false").ToLower() == "true";
				this.chkCanShrink.Checked = _Draw.GetElementValue(riNode, "CanShrink", "false").ToLower() == "true";
				XmlNode initstate = DesignXmlDraw.FindNextInHierarchy(riNode, "ToggleImage", "InitialState");
				this.cbToggleImage.Text = initstate == null? "": initstate.InnerText;
			}
			else
			{
				this.gbText.Visible = false;
			}

			fName = fLeft = fTop = fWidth = fHeight = fZIndex = fColSpan = 
				fCanGrow = fCanShrink = fHideDuplicates = fToggleImage = fDataElementStyle = false;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionCtl));
            this.DoubleBuffered = true;
			this.gbPosition = new System.Windows.Forms.GroupBox();
			this.tbHeight = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tbWidth = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbTop = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbLeft = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.tbZIndex = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.lblColSpan = new System.Windows.Forms.Label();
			this.tbColSpan = new System.Windows.Forms.NumericUpDown();
			this.gbText = new System.Windows.Forms.GroupBox();
			this.cbToggleImage = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbDataElementStyle = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbHideDuplicates = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkCanShrink = new System.Windows.Forms.CheckBox();
			this.chkCanGrow = new System.Windows.Forms.CheckBox();
			this.gbPosition.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbZIndex)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbColSpan)).BeginInit();
			this.gbText.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbPosition
			// 
			resources.ApplyResources(this.gbPosition, "gbPosition");
			this.gbPosition.Controls.Add(this.tbHeight);
			this.gbPosition.Controls.Add(this.label7);
			this.gbPosition.Controls.Add(this.tbWidth);
			this.gbPosition.Controls.Add(this.label8);
			this.gbPosition.Controls.Add(this.tbTop);
			this.gbPosition.Controls.Add(this.label6);
			this.gbPosition.Controls.Add(this.tbLeft);
			this.gbPosition.Controls.Add(this.label5);
			this.gbPosition.Controls.Add(this.label9);
			this.gbPosition.Controls.Add(this.tbZIndex);
			this.gbPosition.Name = "gbPosition";
			this.gbPosition.TabStop = false;
			// 
			// tbHeight
			// 
			resources.ApplyResources(this.tbHeight, "tbHeight");
			this.tbHeight.Name = "tbHeight";
			this.tbHeight.TextChanged += new System.EventHandler(this.tbHeight_TextChanged);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// tbWidth
			// 
			resources.ApplyResources(this.tbWidth, "tbWidth");
			this.tbWidth.Name = "tbWidth";
			this.tbWidth.TextChanged += new System.EventHandler(this.tbWidth_TextChanged);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// tbTop
			// 
			resources.ApplyResources(this.tbTop, "tbTop");
			this.tbTop.Name = "tbTop";
			this.tbTop.TextChanged += new System.EventHandler(this.tbTop_TextChanged);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// tbLeft
			// 
			resources.ApplyResources(this.tbLeft, "tbLeft");
			this.tbLeft.Name = "tbLeft";
			this.tbLeft.TextChanged += new System.EventHandler(this.tbLeft_TextChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// tbZIndex
			// 
			resources.ApplyResources(this.tbZIndex, "tbZIndex");
			this.tbZIndex.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.tbZIndex.Name = "tbZIndex";
			this.tbZIndex.ValueChanged += new System.EventHandler(this.tbZIndex_ValueChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tbName
			// 
			resources.ApplyResources(this.tbName, "tbName");
			this.tbName.Name = "tbName";
			this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			this.tbName.Validating += new System.ComponentModel.CancelEventHandler(this.tbName_Validating);
			// 
			// lblColSpan
			// 
			resources.ApplyResources(this.lblColSpan, "lblColSpan");
			this.lblColSpan.Name = "lblColSpan";
			// 
			// tbColSpan
			// 
			resources.ApplyResources(this.tbColSpan, "tbColSpan");
			this.tbColSpan.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.tbColSpan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.tbColSpan.Name = "tbColSpan";
			this.tbColSpan.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.tbColSpan.ValueChanged += new System.EventHandler(this.tbColSpan_ValueChanged);
			// 
			// gbText
			// 
			resources.ApplyResources(this.gbText, "gbText");
			this.gbText.Controls.Add(this.cbToggleImage);
			this.gbText.Controls.Add(this.label4);
			this.gbText.Controls.Add(this.cbDataElementStyle);
			this.gbText.Controls.Add(this.label3);
			this.gbText.Controls.Add(this.cbHideDuplicates);
			this.gbText.Controls.Add(this.label2);
			this.gbText.Controls.Add(this.chkCanShrink);
			this.gbText.Controls.Add(this.chkCanGrow);
			this.gbText.Name = "gbText";
			this.gbText.TabStop = false;
			// 
			// cbToggleImage
			// 
			resources.ApplyResources(this.cbToggleImage, "cbToggleImage");
			this.cbToggleImage.Items.AddRange(new object[] {
            resources.GetString("cbToggleImage.Items"),
            resources.GetString("cbToggleImage.Items1"),
            resources.GetString("cbToggleImage.Items2")});
			this.cbToggleImage.Name = "cbToggleImage";
			this.cbToggleImage.SelectedIndexChanged += new System.EventHandler(this.cbToggleImage_Changed);
			this.cbToggleImage.TextChanged += new System.EventHandler(this.cbToggleImage_Changed);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// cbDataElementStyle
			// 
			resources.ApplyResources(this.cbDataElementStyle, "cbDataElementStyle");
			this.cbDataElementStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataElementStyle.Items.AddRange(new object[] {
            resources.GetString("cbDataElementStyle.Items"),
            resources.GetString("cbDataElementStyle.Items1"),
            resources.GetString("cbDataElementStyle.Items2")});
			this.cbDataElementStyle.Name = "cbDataElementStyle";
			this.cbDataElementStyle.SelectedIndexChanged += new System.EventHandler(this.cbDataElementStyle_SelectedIndexChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// cbHideDuplicates
			// 
			resources.ApplyResources(this.cbHideDuplicates, "cbHideDuplicates");
			this.cbHideDuplicates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHideDuplicates.Name = "cbHideDuplicates";
			this.cbHideDuplicates.SelectedIndexChanged += new System.EventHandler(this.cbHideDuplicates_SelectedIndexChanged);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// chkCanShrink
			// 
			resources.ApplyResources(this.chkCanShrink, "chkCanShrink");
			this.chkCanShrink.Name = "chkCanShrink";
			this.chkCanShrink.CheckedChanged += new System.EventHandler(this.chkCanShrink_CheckedChanged);
			// 
			// chkCanGrow
			// 
			resources.ApplyResources(this.chkCanGrow, "chkCanGrow");
			this.chkCanGrow.Name = "chkCanGrow";
			this.chkCanGrow.CheckedChanged += new System.EventHandler(this.chkCanGrow_CheckedChanged);
			// 
			// PositionCtl
			// 
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.gbText);
			this.Controls.Add(this.tbColSpan);
			this.Controls.Add(this.lblColSpan);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gbPosition);
			this.Name = "PositionCtl";
			this.gbPosition.ResumeLayout(false);
			this.gbPosition.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbZIndex)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbColSpan)).EndInit();
			this.gbText.ResumeLayout(false);
			this.gbText.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public bool IsValid()
		{
			XmlNode ri = this._ReportItems[0] as XmlNode;
			if (tbName.Enabled && fName)
			{
				string nerr = _Draw.NameError(ri, this.tbName.Text);
				if (nerr != null)
				{
					MessageBox.Show(nerr, Strings.PositionCtl_Show_Name);
					return false;
				}
			}
			string name="";
			try
			{	// allow minus if Line and only one item selected
				bool bMinus= ri.Name == "Line" && tbName.Enabled? true: false;
				if (fLeft)
				{
					name = Strings.PositionCtl_Show_Left;
					DesignerUtility.ValidateSize(this.tbLeft.Text, true, false);
				}
				if (fTop)
				{
					name = Strings.PositionCtl_Show_Top;
					DesignerUtility.ValidateSize(this.tbTop.Text, true, false);
				}
				if (fWidth)
				{
					name = Strings.PositionCtl_Show_Width;
					DesignerUtility.ValidateSize(this.tbWidth.Text, true, bMinus);
				}
				if (fHeight)
				{
					name = Strings.PositionCtl_Show_Height;
					DesignerUtility.ValidateSize(this.tbHeight.Text, true, bMinus);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, name + " " + Strings.PositionCtl_Show_SizeInvalid);
				return false;
			}

			return true;
		}

		public void Apply()
		{
			// take information in control and apply to all the style nodes
			//  Only change information that has been marked as modified;
			//   this way when group is selected it is possible to change just
			//   the items you want and keep the rest the same.
				
			foreach (XmlNode riNode in this._ReportItems)
				ApplyChanges(riNode);

			// No more changes
			fName = fLeft = fTop = fWidth = fHeight = fZIndex = fColSpan = 
				fCanGrow = fCanShrink = fHideDuplicates = fToggleImage = fDataElementStyle = false;
		}

		public void ApplyChanges(XmlNode node)
		{
			if (tbName.Enabled && fName)
			{
				_Draw.SetName(node, tbName.Text.Trim());
			}

			if (fLeft)
				_Draw.SetElement(node, "Left", DesignerUtility.MakeValidSize(tbLeft.Text, true));

			if (fTop)
				_Draw.SetElement(node, "Top", DesignerUtility.MakeValidSize(tbTop.Text, true));

			bool bLine = node.Name == "Line";

			if (fWidth)
				_Draw.SetElement(node, "Width", DesignerUtility.MakeValidSize(tbWidth.Text, bLine, bLine));

			if (fHeight)
				_Draw.SetElement(node, "Height", DesignerUtility.MakeValidSize(tbHeight.Text, bLine, bLine));

			if (fZIndex)
				_Draw.SetElement(node, "ZIndex", tbZIndex.Text);

			if (fColSpan)
			{
				XmlNode ris = node.ParentNode;
				XmlNode tcell = ris.ParentNode;
				if (tcell.Name == "TableCell")
				{	// SetTableCellColSpan does all the heavy lifting; 
					//    ie making sure the # of columns continue to match
					_Draw.SetTableCellColSpan(tcell, tbColSpan.Value.ToString());	 
				}
			}

			if (fDataElementStyle)
				_Draw.SetElement(node, "DataElementStyle", this.cbDataElementStyle.Text);
            if (fHideDuplicates)
            {
                if(this.cbHideDuplicates.Text == "")
                    _Draw.RemoveElement(node, "HideDuplicates");
                else
                    _Draw.SetElement(node, "HideDuplicates", this.cbHideDuplicates.Text);
            }
			if (fCanGrow)
				_Draw.SetElement(node, "CanGrow", this.chkCanGrow.Checked? "true": "false");
			if (fCanShrink)
				_Draw.SetElement(node, "CanShrink", this.chkCanShrink.Checked? "true": "false");
			if (fDataElementStyle)
				_Draw.SetElement(node, "DataElementStyle", this.cbDataElementStyle.Text);
			if (fToggleImage)
			{
				if (cbToggleImage.Text.Length <= 0)
				{
					_Draw.RemoveElement(node, "ToggleImage");
				}
				else
				{
					XmlNode ti = _Draw.SetElement(node, "ToggleImage", null);
					_Draw.SetElement(ti, "InitialState", cbToggleImage.Text);
				}
			}
		}

		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
			fName = true;
		}

		private void tbLeft_TextChanged(object sender, System.EventArgs e)
		{
			fLeft = true;
		}

		private void tbTop_TextChanged(object sender, System.EventArgs e)
		{
			fTop = true;
		}

		private void tbWidth_TextChanged(object sender, System.EventArgs e)
		{
			fWidth = true;
		}

		private void tbHeight_TextChanged(object sender, System.EventArgs e)
		{
			fHeight = true;
		}

		private void tbZIndex_ValueChanged(object sender, System.EventArgs e)
		{
			fZIndex = true;
		}

		private void tbName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			XmlNode xNode = this._ReportItems[0];

			string err = _Draw.NameError(xNode, tbName.Text.Trim());
			if (err != null)
			{
				e.Cancel = true;
				MessageBox.Show(string.Format(Strings.PositionCtl_Show_Invalid, tbName.Text, err), Strings.PositionCtl_Show_Name);
				return;
			}
		}

		private void tbColSpan_ValueChanged(object sender, System.EventArgs e)
		{
			fColSpan = true;
		}

		private void cbToggleImage_Changed(object sender, System.EventArgs e)
		{
			fToggleImage = true;
		}

		private void cbDataElementStyle_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fDataElementStyle = true;
		}

		private void cbHideDuplicates_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			fHideDuplicates = true;
		}

		private void chkCanShrink_CheckedChanged(object sender, System.EventArgs e)
		{
			fCanShrink = true;
		}

		private void chkCanGrow_CheckedChanged(object sender, System.EventArgs e)
		{
			fCanGrow = true;
		}
	}
}
