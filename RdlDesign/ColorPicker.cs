using System.Drawing;
/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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
using System.Windows.Forms;

namespace Majorsilence.Reporting.RdlDesign
{
	/// <summary>
	/// It's very crazy control. Need replace it. TODO
	/// </summary>
    public class ColorPicker : ComboBox
    { 
        private const int RECTCOLOR_LEFT = 4;
        private const int RECTCOLOR_TOP = 2;
        private const int RECTCOLOR_WIDTH = 10;
        ColorPickerPopup _DropListBox;

        public ColorPicker()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList; // DropDownList
            DropDownHeight = 1;
            Font = new Font("Arial", 8, FontStyle.Bold | FontStyle.Italic);

            _DropListBox = new ColorPickerPopup(this);

			if (!DesignMode)
			{
				Items.AddRange(StaticLists.ColorList);
			}
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                string v = value == null ? "" : value;
                if (!this.Items.Contains(v))    // make sure item is always in the list
                    this.Items.Add(v);
                base.Text = v;
            }
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Color BlockColor = Color.Empty;
            int left = RECTCOLOR_LEFT;
            if (e.State == DrawItemState.Selected || e.State == DrawItemState.None)
                e.DrawBackground();
            if (e.Index == -1)
            {
                BlockColor = SelectedIndex < 0 ? BackColor : DesignerUtility.ColorFromHtml(this.Text, Color.Empty);
            }
            else
                BlockColor = DesignerUtility.ColorFromHtml((string)this.Items[e.Index], Color.Empty);
            // Fill rectangle
            if (BlockColor.IsEmpty && this.Text.StartsWith("="))
            {
                g.DrawString("fx", this.Font, Brushes.Black, e.Bounds);
            }
            else
            {
                g.FillRectangle(new SolidBrush(BlockColor), left, e.Bounds.Top + RECTCOLOR_TOP, RECTCOLOR_WIDTH,
                    ItemHeight - 2 * RECTCOLOR_TOP);
            }
            base.OnDrawItem(e);
        }

        protected override void OnDropDown(System.EventArgs e)
        {
            _DropListBox.Location = this.PointToScreen(new Point(0, this.Height));
            _DropListBox.Show();
        }
    }
}
