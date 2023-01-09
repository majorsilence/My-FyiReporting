// 
//  ReportArea.cs
//  
//  Author:
//       Krzysztof Marecki 
// 
//  Copyright (c) 2010 Krzysztof Marecki
// 
// This file is part of the NReports project
// This file is part of the My-FyiReporting project 
//	
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Globalization;
using System.Threading;
using Cairo;
using fyiReporting.RDL;
using System.Collections.Generic;
using Gtk;
using System.Linq;

namespace fyiReporting.RdlGtkViewer
{
	[System.ComponentModel.ToolboxItem(true)]
	public class ReportArea : Gtk.DrawingArea
	{
		Page pages;
		Report report;
		int rep_padding = 10;
		int shadow_padding = 16;
		float scale = 1.0f;

		public float Scale {
			get { return scale; }
			set {
				if(value != scale && value != 0) {
					scale = value;

					this.QueueResize();
					GdkWindow.Invalidate();
				}
			}
		}

		public ReportArea()
		{
			// Insert initialization code here.
			AddEvents((int)Gdk.EventMask.ButtonPressMask);
		}

		public void SetReport(Report report, Page pages)
		{
			this.pages = pages;
			this.report = report;

			this.QueueResize();
			GdkWindow.Invalidate();
		}

		PageItem selectedItem;

		private Rectangle GetSelectedItemRectangle()
		{
			return new Rectangle(selectedItem.X * scale, selectedItem.Y * scale, selectedItem.W * scale, selectedItem.H * scale);
		}

		protected override bool OnButtonPressEvent(Gdk.EventButton ev)
		{
			if(ev.Button == 3) {
				var hitAreaItem = hitList.FirstOrDefault(x => x.Contains(new PointD((double)ev.X, (double)ev.Y)));
				if(hitAreaItem == null) {
					return false;
				}

				string text;
				if(hitAreaItem.pi is PageText) {
					text = (hitAreaItem.pi as PageText).Text;
				} else if(hitAreaItem.pi is PageTextHtml) {
					text = (hitAreaItem.pi as PageTextHtml).Text;
				} else {
					return false;
				}
				selectedItem = hitAreaItem.pi;
				QueueDraw();
				GdkWindow.Invalidate();
				Menu popupMenu = new Menu();
				MenuItem menuItem = new MenuItem("Копировать");
				menuItem.Activated += (sender, e) => {
					Gtk.Clipboard clipboard = Gtk.Clipboard.Get(Gdk.Atom.Intern("CLIPBOARD", false));
					clipboard.Text = text;
					selectedItem = null;
					QueueDraw();
					GdkWindow.Invalidate();
				};
				popupMenu.Add(menuItem);
				popupMenu.ShowAll();
				popupMenu.Popup();
				popupMenu.Hidden += (sender, e) => { 
					selectedItem = null;
					QueueDraw();
					GdkWindow.Invalidate();
				};
			}
			// Insert button press handling code here.
			return base.OnButtonPressEvent(ev);
		}

		internal float DpiY = 96;
		internal float DpiX = 96;

		internal float PixelsX(float x)
		{
			return (float)(x * DpiX / 96.0f);
		}

		internal float PixelsY(float y)
		{
			return (float)(y * DpiY / 96.0f);
		}

		private List<HitListEntry> hitList = new List<HitListEntry>();

		private void SetItemsHitArea()
		{
			float XAdditional = GetLeftAreaPosition();
			float YAdditional = rep_padding;
			hitList.Clear();
			foreach(PageItem pi in pages) {
				if(pi is PageTextHtml) {    // PageTextHtml is actually a composite object (just like a page)
					Rectangle hr = new Rectangle(PixelsX(pi.X * scale) + XAdditional, PixelsY(pi.Y * scale) + YAdditional,
																		PixelsX(pi.W * scale), PixelsY(pi.H * scale));
					hitList.Add(new HitListEntry(hr, pi));
					continue;
				}

				Rectangle rect = new Rectangle(PixelsX(pi.X * scale) + XAdditional, PixelsY(pi.Y * scale) + YAdditional,
																	PixelsX(pi.W * scale), PixelsY(pi.H * scale));

				if(pi is PageText || pi is PageImage) {
					hitList.Add(new HitListEntry(rect, pi));
				}
				// Only care about items with links and tips
				else if(pi.HyperLink != null || pi.BookmarkLink != null || pi.Tooltip != null) {
					HitListEntry hle;
					if(pi is PagePolygon)
						hle = new HitListEntry(pi as PagePolygon, XAdditional, YAdditional, this);
					else
						hle = new HitListEntry(rect, pi);
					hitList.Add(hle);
				}
			}
		}

		class HitListEntry
		{
			internal Rectangle rect;
			internal PageItem pi;
			internal System.Drawing.PointF[] poly;
			internal HitListEntry(Rectangle r, PageItem pitem)
			{
				rect = r;
				pi = pitem;
				poly = null;
			}
			internal HitListEntry(PagePolygon pp, float x, float y, ReportArea ra)
			{
				pi = pp;
				poly = new System.Drawing.PointF[pp.Points.Length];
				for(int i = 0; i < pp.Points.Length; i++) {
					poly[i].X = ra.PixelsX(pp.Points[i].X + x);
					poly[i].Y = ra.PixelsY(pp.Points[i].Y + y);
				}
				rect = new Rectangle(0, 0, 0, 0);
			}
			/// <summary>
			/// Contains- determine whether point in the pageitem
			/// </summary>
			/// <param name="p"></param>
			/// <returns></returns>
			internal bool Contains(PointD p)
			{
				return (pi is PagePolygon) ? PointInPolygon(p) : rect.ContainsPoint(p.X, p.Y);
			}

			/// <summary>
			/// PointInPolygon: uses ray casting algorithm ( http://en.wikipedia.org/wiki/Point_in_polygon )
			/// </summary>
			/// <param name="p"></param>
			/// <returns></returns>
			bool PointInPolygon(PointD p)
			{
				System.Drawing.PointF p1, p2;
				bool bIn = false;
				if(poly.Length < 3) {
					return false;
				}

				System.Drawing.PointF op = new System.Drawing.PointF(poly[poly.Length - 1].X, poly[poly.Length - 1].Y);
				for(int i = 0; i < poly.Length; i++) {
					System.Drawing.PointF np = new System.Drawing.PointF(poly[i].X, poly[i].Y);
					if(np.X > op.X) {
						p1 = op;
						p2 = np;
					} else {
						p1 = np;
						p2 = op;
					}

					if((np.X < p.X) == (p.X <= op.X)
						&& (p.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (p.X - p1.X)) {
						bIn = !bIn;
					}
					op = np;
				}
				return bIn;
			}
		}

		private int GetLeftAreaPosition()
		{
			int width = (int)(report.PageWidthPoints * Scale);
			int widgetWidth, widgetHeight;
			GdkWindow.GetSize(out widgetWidth, out widgetHeight);
			int position = ((widgetWidth - width) / 2);
			return position;
		}

		protected override bool OnExposeEvent(Gdk.EventExpose ev)
		{
			base.OnExposeEvent(ev);

			if(pages == null)
				return false;

			int width = (int)(report.PageWidthPoints * Scale);
			int height = (int)(report.PageHeightPoints * Scale);
			Rectangle rep_r = new Rectangle(1, 1, width - 1, height - 1);

			int widgetWidth, widgetHeight;
			ev.Window.GetSize(out widgetWidth, out widgetHeight);

			using(var g = Gdk.CairoHelper.Create(this.GdkWindow))
			using(var repS = new ImageSurface(Format.Argb32, width, height))
			using(var repG = new Context(repS))
			using(var shadowS = repS.Clone())
			using(var shadowG = new Context(shadowS))
			{
				g.Translate(((widgetWidth - width) / 2) - rep_padding, 0);

				using(var shadowGPattern = new SolidPattern(new Color(0.6, 0.6, 0.6)))
				{
					shadowG.SetSource(shadowGPattern);
					shadowG.Paint();
					g.SetSourceSurface(shadowS, shadow_padding, shadow_padding);
					g.Paint();

					using(var repGPattern = new SolidPattern(new Color(1, 1, 1)))
					{
						repG.SetSource(repGPattern);
						repG.Paint();
						repG.DrawRectangle(rep_r, new Color(0.1, 0.1, 0.1), 1);

						SetItemsHitArea();

						Pattern currentRepGPattern = null;
						if(selectedItem != null)
						{
							currentRepGPattern = new SolidPattern(new Color(0.4, 0.4, 1));
							repG.SetSource(currentRepGPattern);
							repG.Rectangle(GetSelectedItemRectangle());
							repG.Fill();
						}

						using(var render = new RenderCairo(repG, Scale))
						{
							render.RunPage(pages);
						}

						g.SetSourceSurface(repS, rep_padding, rep_padding);
						g.Paint();
						
						currentRepGPattern?.Dispose();
					}
				}
			}

			return true;
		}


		private void DrawString(string s, Cairo.Context g, Cairo.Rectangle r)
		{
			StyleInfo si = new StyleInfo();
			g.Save();

			Pango.Layout layout;

			layout = Pango.CairoHelper.CreateLayout(g);

			float fontsize = (si.FontSize * 72 / 96);
			var font = Pango.FontDescription.FromString(string.Format("{0} {1}", si.GetFontFamily().Name,
							   fontsize * PixelsX(1)));
			if(si.FontStyle == FontStyleEnum.Italic)
				font.Style = Pango.Style.Italic;

			switch(si.FontWeight) {
				case FontWeightEnum.Bold:
				case FontWeightEnum.Bolder:
				case FontWeightEnum.W500:
				case FontWeightEnum.W600:
				case FontWeightEnum.W700:
				case FontWeightEnum.W800:
				case FontWeightEnum.W900:
					font.Weight = Pango.Weight.Bold;
					break;
			}

			Pango.FontDescription oldfont = layout.FontDescription;
			layout.FontDescription = font;

			switch(si.TextAlign) {
				case TextAlignEnum.Right:
					layout.Alignment = Pango.Alignment.Right;
					break;
				case TextAlignEnum.Center:
					layout.Alignment = Pango.Alignment.Center;
					break;
				case TextAlignEnum.Left:
				default:
					layout.Alignment = Pango.Alignment.Left;
					break;
			}

			layout.Width = Pango.Units.FromPixels((int)(r.Width - si.PaddingLeft - si.PaddingRight - 2));
			//				layout.Width = 	(int)Pango.Units.FromPixels((int)r.Width);

			layout.SetText(s);

			Pango.Rectangle logical;
			Pango.Rectangle ink;
			layout.GetExtents(out ink, out logical);
			double height = logical.Height / Pango.Scale.PangoScale;
			double y = 0;
			switch(si.VerticalAlign) {
				case VerticalAlignEnum.Top:
					y = r.Y + si.PaddingTop;
					break;
				case VerticalAlignEnum.Middle:
					y = r.Y + (r.Height - height) / 2;
					break;
				case VerticalAlignEnum.Bottom:
					y = r.Y + (r.Height - height) - si.PaddingBottom;
					break;
			}

			Cairo.Rectangle box = new Cairo.Rectangle(
									  r.X + si.PaddingLeft + 1,
									  y,
									  r.Width,
									  r.Height);

			g.Color = si.Color.ToCairoColor();

			g.MoveTo(box.X, box.Y);

			Pango.CairoHelper.ShowLayout(g, layout);

			layout.FontDescription = oldfont;
			g.Restore();
		}


		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			// Insert layout code here.
		}

		protected override void OnSizeRequested(ref Gtk.Requisition requisition)
		{
			if(report != null) {
				requisition.Width = (int)(report.PageWidthPoints * scale) + rep_padding * 2;
				requisition.Height = (int)(report.PageHeightPoints * scale) + rep_padding * 2;
			}
		}
	}
}

