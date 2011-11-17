/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for DialogNew.
	/// </summary>
	public partial class ColorPickerPopup 
    {
        private const int ITEM_HEIGHT = 12;
        private const int ITEM_PAD = 2;
        
        public ColorPickerPopup(ColorPicker cp)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            lStatus.Text = "";
            _ColorPicker = cp;
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //base.OnPaint(e);
            int row = 0;
            int col = 0;
            int max_rows = MaxRows;
            int max_cols = MaxColumns;
            int col_width = ColumnWidth;

            foreach (string c in StaticLists.ColorListColorSort)
            {
                Color clr = DesignerUtility.ColorFromHtml(c, Color.Empty);
                g.FillRectangle(new SolidBrush(clr), 
                    new Rectangle((col * col_width)+ITEM_PAD, (row * ITEM_HEIGHT)+ITEM_PAD, col_width-ITEM_PAD, ITEM_HEIGHT-ITEM_PAD));
                row++;
                if (row >= max_rows)
                {
                    row = 0;
                    col++;
                }
            }
        }

        private void lbColors_Hide(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ColorPickerPopup_MouseDown(object sender, MouseEventArgs e)
        {
            int col = e.Location.X / ColumnWidth;
            int row = e.Location.Y / ITEM_HEIGHT;

            int item = col * MaxRows + row;

            if (item >= StaticLists.ColorListColorSort.Length)
                return;

            _ColorPicker.Text = StaticLists.ColorListColorSort[item];
            this.Hide();
        }

        private void ColorPickerPopup_MouseMove(object sender, MouseEventArgs e)
        {
            string status;
            if (e.Location.Y > this.Height - lStatus.Height)    // past bottom of rectangle
                status = "";
            else
            {                                                   // calc position in box
                int col = e.Location.X / ColumnWidth;
                int row = e.Location.Y / ITEM_HEIGHT;

                int item = col * MaxRows + row;

                status = item >= StaticLists.ColorListColorSort.Length ? "" : StaticLists.ColorListColorSort[item] as string;
            }
            lStatus.Text = status;
        }

        private int MaxRows
        {
            get { return (this.Height - lStatus.Height) / ITEM_HEIGHT; }
        }
        private int MaxColumns
        {
            get
            {
                int max_rows = MaxRows;
                return (StaticLists.ColorListColorSort.Length / max_rows) + (StaticLists.ColorListColorSort.Length % max_rows == 0 ? 0 : 1);
            }
        }
        private int ColumnWidth
        {
            get { return this.Width / MaxColumns; }
        }

        private void ColorPickerPopup_Shown(object sender, EventArgs e)
        {
            lStatus.Text = _ColorPicker.Text;
        }

        private void ColorPickerPopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)System.Windows.Forms.Keys.Escape)
            {
                Hide();
            }
        }
        private void ColorPickerPopup_Load(object sender, EventArgs e)
        {
        }
	}
}
