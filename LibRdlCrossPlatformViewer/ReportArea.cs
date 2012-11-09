using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fyiReporting.RDL;

namespace LibRdlCrossPlatformViewer
{
    public class ReportArea : Xwt.Canvas
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
                }
            }
        }

        public ReportArea()
        {
            // Insert initialization code here.
            this.BackgroundColor = Xwt.Drawing.Colors.White;
        }

        public void SetReport(Report report, Page pages)
        {
            this.pages = pages;
            this.report = report;
        
            this.NaturalWidth = (int)report.PageWidthPoints + rep_padding * 2;
            this.NaturalHeight = (int)report.PageHeightPoints + rep_padding * 2;

        }

        protected override void OnDraw(Xwt.Drawing.Context ctx, Xwt.Rectangle dirtyRect)
        {
            base.OnDraw(ctx, dirtyRect);

            if (pages == null)
            {
                return;
            }
            ctx.Font = this.Font;
            ctx.Save();

            int width = (int)(report.PageWidthPoints * Scale);
            int height = (int)(report.PageHeightPoints * Scale);
            Xwt.Rectangle rep_r = new Xwt.Rectangle(1, 1, width - 1, height - 1);


            RenderXwt render = new RenderXwt(ctx, Scale);
            render.RunPage(pages);
            ctx.Stroke();

        }

    }
}
