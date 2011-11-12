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
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace fyiReporting.RdlDesign
{
	internal class SimpleButton : Button
	{
		private Color _Transparency;
		private bool bDown=false;
		private bool bIn=false;

		public SimpleButton(Control parent) 
		{	
			this.Parent = parent;
			this.TranparencyColor = Color.White;

			this.BackColor = parent.BackColor;
			this.ForeColor = this.Enabled? Color.Black: Color.Gray;
			this.MouseDown +=new MouseEventHandler(SimpleButton_MouseDown);
			this.MouseUp +=new MouseEventHandler(SimpleButton_MouseUp);
			this.MouseEnter +=new EventHandler(SimpleButton_MouseEnter);
			this.MouseLeave +=new EventHandler(SimpleButton_MouseLeave);
			this.Paint += new PaintEventHandler(this.DrawPanelPaint);
		}

		private void DrawPanelPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

			Graphics g = e.Graphics;
			Brush b = null;
			Pen p = null;

			try			// never want to die in here
			{
				b = new SolidBrush(this.Enabled? this.BackColor: Color.LightGray);
				g.FillRectangle(b, e.ClipRectangle);
				if (bIn && this.Enabled)
					g.DrawRectangle(Pens.Blue, 0, 0, this.Width-1, this.Height-1);

				if (this.Image != null)
				{
					int x = (this.Width - this.Image.Width) / 2;
					int y = (this.Height - this.Image.Height) / 2;
					if (bDown && bIn)
					{
						x += 1;
						y += 1;
					}

					// Draw Image using the transparency color
					ImageAttributes imageAttr = new ImageAttributes();
					imageAttr.SetColorKey(_Transparency, _Transparency,
						ColorAdjustType.Default);

					g.DrawImage(this.Image,         // Image
						new Rectangle(x, y, this.Image.Width, this.Image.Height),    // Dest. rect.
						0,							// srcX
						0,							// srcY
						this.Image.Width,           // srcWidth
						this.Image.Height,          // srcHeight
						GraphicsUnit.Pixel,			// srcUnit
						imageAttr);					// ImageAttributes
				}
				else
				{
					StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
					g.DrawString(this.Text, this.Font, Brushes.Black, new Rectangle(2, 2, this.Width, this.Height), format);
				}
			}
			catch {}	// todo draw the error message
			finally
			{
				if (b != null)
					b.Dispose();
				if (p != null)
					p.Dispose();
			}
		}

		public Color TranparencyColor
		{
			get { return this._Transparency;	}
			set { this._Transparency = value; }
		}

		private void SimpleButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				bDown = true;
		}

		private void SimpleButton_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				bDown = false;
		}

		private void SimpleButton_MouseEnter(object sender, EventArgs e)
		{
			bIn = true;
		}

		private void SimpleButton_MouseLeave(object sender, EventArgs e)
		{
			bIn = false;
		}
	}
}
