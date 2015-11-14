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
using Gtk;
using fyiReporting.RDL;

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

        public float Scale
        {
            get { return scale; }
            set
            {
                if (value != scale && value != 0)
                {
                    scale = value;

                    this.QueueResize();
                    GdkWindow.Invalidate();
                }	
            }
        }

        public ReportArea()
        {
            // Insert initialization code here.
        }

        public void SetReport(Report report, Page pages)
        {
            this.pages = pages;
            this.report = report;

            this.QueueResize();
            GdkWindow.Invalidate();
        }

        protected override bool OnButtonPressEvent(Gdk.EventButton ev)
        {
            // Insert button press handling code here.
            return base.OnButtonPressEvent(ev);
        }

        protected override bool OnExposeEvent(Gdk.EventExpose ev)
        {
            base.OnExposeEvent(ev);
			
            if (pages == null)
                return false;
				
            int width = (int)(report.PageWidthPoints * Scale);
            int height = (int)(report.PageHeightPoints * Scale);
            Cairo.Rectangle rep_r = new Cairo.Rectangle(1, 1, width - 1, height - 1);

            int widgetWidth, widgetHeight;
            ev.Window.GetSize(out widgetWidth, out widgetHeight);
			
            using (Context g = Gdk.CairoHelper.Create(this.GdkWindow))
            using (ImageSurface rep_s = new ImageSurface(Format.Argb32, width, height))
            using (Context rep_g = new Context(rep_s))
            using (ImageSurface shadow_s = rep_s.Clone())
            using (Context shadow_g = new Context(shadow_s))
			using (SolidPattern shadow_pattern = new SolidPattern(new Color(0.6, 0.6, 0.6)))
			using (SolidPattern rep_pattern = new SolidPattern(new Color(1, 1, 1)))
            {
				
                g.Translate(((widgetWidth - width) / 2) - rep_padding, 0);
				shadow_g.SetSource (shadow_pattern);
                shadow_g.Paint();
                g.SetSourceSurface(shadow_s, shadow_padding, shadow_padding);
                g.Paint();
				
				rep_g.SetSource (rep_pattern);
                rep_g.Paint();
				
                rep_g.DrawRectangle(rep_r, new Color(0.1, 0.1, 0.1), 1);

                using (var render = new RenderCairo(rep_g, Scale))
                {
                    render.RunPage(pages);
                }

                g.SetSourceSurface(rep_s, rep_padding, rep_padding);
                g.Paint();
				
            }
            return true;
        }

        protected override void OnSizeAllocated(Gdk.Rectangle allocation)
        {
            base.OnSizeAllocated(allocation);
            // Insert layout code here.
        }

        protected override void OnSizeRequested(ref Gtk.Requisition requisition)
        {
            if (report != null)
            {
                requisition.Width = (int)(report.PageWidthPoints * scale) + rep_padding * 2; 
                requisition.Height = (int)(report.PageHeightPoints * scale) + rep_padding * 2;
            }
        }
    }
}

