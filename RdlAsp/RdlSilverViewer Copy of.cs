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
using System.Web;
using System.Web.UI;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Web.Caching;
using fyiReporting.RDL;

namespace fyiReporting.RdlAsp
{
	/// <summary>
	/// RdlSilverViewer runs a report and provides visual access via a SilverLight control.
	/// </summary>
	public class RdlSilverViewer : Control
	{
		private string _ReportFile=null;
		private List<string> _Errors=null;
		private int _MaxSeverity=0;
        private string _PassPhrase = null;
        private bool _NoShow;
        private Pages _Pages;               // the report (after it has been run);
        private string[] _XamlPages;    // the XAML for a report
        private string _ParameterHtml;
        private float DpiX=96;
        private float DpiY=96;
        private int _UniqueName = 0;
        private Dictionary<string, PageImage> _Images = new Dictionary<string, PageImage>();
        private string _Url = "ShowSilverlightReport.aspx";
        private string _ReportDirectory = "";
        private string _ImageDirectory = "";
        private ZipOutputStream _ZipStream = null;
        private Dictionary<string, string> _ZipImages = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// When true report won't be shown but parameters (if any) will be
        /// </summary>
        public bool NoShow
        {
            get { return _NoShow; }
            set { _NoShow = value; }
        }
        /// <summary>
        /// Gets the page count 
        /// </summary>
        public int PageCount
        {
            get { return _XamlPages.Length; }
        }
        /// <summary>
        /// Sets the report RDL file name.
        /// </summary>
		public string ReportFile
		{
			get {return _ReportFile;}
			set 
			{
				_ReportFile = value;
				// Clear out old report information (if any)
				this._Errors = null;
				this._MaxSeverity = 0;
                _Pages = null;
                _XamlPages = null;
			}
		}

        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        public string ReportDirectory
        {
            get { return _ReportDirectory; }
            set { _ReportDirectory = value; }
        }

        public string ImageDirectory
        {
            get { return _ImageDirectory; }
            set { _ImageDirectory = value; }
        }

        public string PassPhrase
        {
            set { _PassPhrase = value; }
        }

        private string GetPassword()
        {
            return _PassPhrase;
        }

        private bool GetXaml()
        {
            if (_XamlPages == null)         // nothing to retrieve (Yet)
            {
                // Build the new report
                string pfile = this.MapPathSecure(this._ReportDirectory + "/" + _ReportFile);
                DoRender(pfile);
            }
            return _XamlPages != null;
        }
/// <summary>
/// Get the XAML representation for the specified page.  When null is returned; look at the Errors property for
/// a list of the problems encountered rendering the report.
/// </summary>
/// <param name="pageno">1 for first page, 2 for second, and so on</param>
/// <returns></returns>
		public string GetXaml(int pageno)
		{
            if (!GetXaml())         // nothing to retrieve (Yet)
                return null;
            
            int page = Math.Min(pageno - 1, _XamlPages.Length - 1);
            string xaml = CreateXamlFromPage(Math.Max(page,0));

            return xaml;
		}

        public byte[] GetZip()
        {
            if (!GetXaml())
                return null;

            MemoryStream ms = new MemoryStream();
            _ZipStream = null;

            ZipEntry ze;
            try
            {
                ZipWrap.Init();             // initialize the zipping utility (if needed)
                _ZipImages = new Dictionary<string, string>(); 

                _ZipStream = new ZipOutputStream(ms);

                // Need to build the Xaml
                int pageno = 1;
                foreach (fyiReporting.RDL.Page page in _Pages)
                {
                    string xaml = GenerateXaml(page);
                    string zp = string.Format("pg_{0}.xaml", pageno++);
                    ze = new ZipEntry(zp);
                    _ZipStream.PutNextEntry(ze);
                    _ZipStream.Write(xaml);
                }
                // output some meta data
                ze = new ZipEntry("meta.txt");
                _ZipStream.PutNextEntry(ze);
                _ZipStream.Write(string.Format("pagecount={0}", _Pages.Count)); // to do more than one use & e.g. pagecount=1&pagewidth=121

                // all done
                _ZipStream.Finish();
                _ZipStream = null;
                return ms.ToArray();
            }
            finally
            {
                if (_ZipStream != null)
                {
                    _ZipStream.Finish();
                    _ZipStream = null;
                }
                ms.Close();
                _ZipImages = null;              // no longer need zipimage cache
            }
        }
        private string CreateXamlFromPage(int pageNo)
        {
            try
            {
                if (_XamlPages[pageNo] == null)           // if already built don't do again
                {
                    // Need to build the Xaml
                    fyiReporting.RDL.Page page = _Pages[pageNo];

                    _XamlPages[pageNo] = GenerateXaml(page);
                }
                return _XamlPages[pageNo];
            }
            finally
            {
                _Pages.CleanUp();
            }
        }
        /// <summary>
        /// List of errors encountered when rendering the report.
        /// </summary>
        public List<string> Errors
        {
            get { return _Errors; }
        }

        public int MaxErrorSeverity
        {
            get { return _MaxSeverity; }
        }

        public string ParameterHtml
        {
            get
            {
                return _ParameterHtml;
            }
        }

		// Render the report files with the requested types
		private void DoRender(string file)
		{
           
			string source;
			Report report=null;
			NameValueCollection nvc;

			nvc = this.Context.Request.QueryString;		// parameters
			ListDictionary ld = new ListDictionary();
            try
            {
			    for (int i=0; i < nvc.Count; i++)
			    {
				    ld.Add(nvc.GetKey(i), nvc[i]);
			    }

 //               if (!_NoShow) { report = GetCachedReport(file); }
                report = ReportHelper.GetCachedReport(file, this.Context.Cache, this.Context.Application);
			    if (report == null) // couldn't obtain report definition from cache
			    {
				    // Obtain the source
				    source = ReportHelper.GetSource(file);
				    if (source == null)			
					    return;					// GetSource reported the error

				    // Compile the report
				    report = this.GetReport(source, file);
				    if (report == null)
					    return;
                    ReportHelper.SaveCachedReport(report, file, this.Context.Cache);
			    }
			    // Set the user context information: ID, language
                ReportHelper.SetUserContext(report, this.Context, new RDL.NeedPassword(GetPassword));

			    // Obtain the data if report is being generated
                if (!_NoShow)
                {
			        report.RunGetData(ld);
			        Generate(report);
                }
            }
            catch (Exception exe)
            {
                AddError(8, "Error: {0}", exe.Message);
            }

            if (_ParameterHtml == null)
                _ParameterHtml = ReportHelper.GetParameterHtml(report, ld, this.Context, this._ReportFile, _NoShow);	// build the parameter html
		}

		private void AddError(int severity, string err, params object[] args)
		{
			if (_MaxSeverity < severity)
				_MaxSeverity = severity;

			string error = string.Format(err, args);
			if (_Errors == null)
				_Errors = new List<string>();
			_Errors.Add(error);
		}

		private void AddError(int severity, IList errors)
		{
			if (_MaxSeverity < severity)
				_MaxSeverity = severity;
			if (_Errors == null)
			{	// if we don't have any we can just start with this list
                _Errors = new List<string>();
                return;
			}
			
			// Need to copy all items in the errors array
			foreach(string err in errors)
				_Errors.Add(err);

			return;
		}

		private void Generate(Report report)
		{
			try
			{
                _Pages =  report.BuildPages();
                // create array for XAML pages.   Actual XAML will be created on demand.
                _XamlPages = new string[_Pages.Count];
                for (int i = 0; i < _XamlPages.Length; i++)
                    _XamlPages[i] = null;
			}
			catch(Exception e)
			{
                AddError(8, string.Format("Exception generating report {0}", e.Message));
			}

			if (report.ErrorMaxSeverity > 0) 
			{
				AddError(report.ErrorMaxSeverity, report.ErrorItems);
				report.ErrorReset();
			}

			return;
		}

		private Report GetReport(string prog, string file)
		{
			// Now parse the file
			RDLParser rdlp;
			Report r;
			try
			{
                // Make sure RdlEngine is configed before we ever parse a program
                //   The config file must exist in the Bin directory.
                string searchDir = this.MapPathSecure(this.ReportFile.StartsWith("~") ? "~/Bin" : "/Bin") + Path.DirectorySeparatorChar;
                RdlEngineConfig.RdlEngineConfigInit(searchDir);

				rdlp =  new RDLParser(prog);
				string folder = Path.GetDirectoryName(file);
				if (folder == "")
					folder = Environment.CurrentDirectory;
				rdlp.Folder = folder;
				rdlp.DataSourceReferencePassword = new NeedPassword(this.GetPassword);

				r = rdlp.Parse();
				if (r.ErrorMaxSeverity > 0)
				{
					AddError(r.ErrorMaxSeverity, r.ErrorItems);
					if (r.ErrorMaxSeverity >= 8)
						r = null;
					r.ErrorReset();
				}

				// If we've loaded the report; we should tell it where it got loaded from
				if (r != null)
				{
					r.Folder = folder;
					r.Name = Path.GetFileNameWithoutExtension(file);
					r.GetDataSourceReferencePassword = new RDL.NeedPassword(GetPassword);
				}
			}
			catch(Exception e)
			{
				r = null;
				AddError(8, "Exception parsing report {0}.  {1}", file, e.Message);
			}
			return r;
		}

        private string GenerateXaml(fyiReporting.RDL.Page page)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Canvas xmlns=\"http://schemas.microsoft.com/client/2007\">");
            ProcessPage(sb, page, new Rectangle(0, 0, PixelsX(_Pages.PageWidth), PixelsY(_Pages.PageHeight)));
            sb.Append("</Canvas>");
            return sb.ToString() ;
        }

        // render all the objects in a page (or any composite object
        private void ProcessPage(StringBuilder sb, IEnumerable p, Rectangle offset)
        {
            foreach (PageItem pi in p)
            {
                if (pi is PageTextHtml)
                {	// PageTextHtml is actually a composite object (just like a page)
                    ProcessHtml(pi as PageTextHtml, sb, new Rectangle(PixelsX(pi.X), PixelsY(pi.Y), PixelsX(pi.W), PixelsY(pi.H)));
                    continue;
                }

                if (pi is PageLine)
                {
                    PageLine pl = pi as PageLine;
                    DrawLine(pl.SI.BColorLeft, pl.SI.BStyleLeft, PixelsX(pl.SI.BWidthLeft),
                        sb, PixelsX(pl.X), PixelsY(pl.Y),
                        PixelsX(pl.X2), PixelsY(pl.Y2));
                    continue;
                }

                Rectangle rect = new Rectangle(PixelsX(pi.X), PixelsY(pi.Y),
                                                                    PixelsX(pi.W), PixelsY(pi.H));

                if (pi.SI.BackgroundImage != null)
                {	// put out any background image
                    PageImage i = pi.SI.BackgroundImage;
                    DrawImageBackground(i, pi.SI, sb, rect);
                }

                if (pi is PageText)
                {
                    PageText pt = pi as PageText;
                    DrawString(pt, sb, rect);
                }
                else if (pi is PageImage)
                {
                    PageImage i = pi as PageImage;
                    DrawImage(i, sb, rect);
                }
                else if (pi is PageRectangle)
                {
                    this.DrawRectangle(pi as PageRectangle, sb, rect);
                }
                else if (pi is PageEllipse)
                {
                    PageEllipse pe = pi as PageEllipse;
                    DrawEllipse(pe, sb, rect);
                }
                else if (pi is PagePie)
                {
                    PagePie pp = pi as PagePie;
                    // DrawPie(pp, g, rect);
                }
                else if (pi is PagePolygon)
                {
                    PagePolygon ppo = pi as PagePolygon;
                    DrawPolygon(ppo, sb);
                }

                DrawBorder(pi, sb, rect);
            }
        }

        private void DrawBackground(StringBuilder sb, StyleInfo si)
        {
            if (si.BackgroundGradientType != BackgroundGradientTypeEnum.None &&
                !si.BackgroundGradientEndColor.IsEmpty &&
                !si.BackgroundColor.IsEmpty)
            {
                string c = GetColor(si.BackgroundColor);
                string ec = GetColor(si.BackgroundGradientEndColor);
                switch (si.BackgroundGradientType)
                {
                    case BackgroundGradientTypeEnum.LeftRight:
                        sb.Append("<LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"1,0\">");
                        sb.AppendFormat("<GradientStop Offset=\"0\" Color=\"{0}\"/><GradientStop Offset=\"1\" Color=\"{1}\"/>", c, ec);
                        break;
                    case BackgroundGradientTypeEnum.TopBottom:
                        sb.Append("<LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"0,1\">");
                        sb.AppendFormat("<GradientStop Offset=\"0\" Color=\"{0}\"/><GradientStop Offset=\"1\" Color=\"{1}\"/>", c, ec);
                        break;
                    case BackgroundGradientTypeEnum.Center:
                        sb.Append("<LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"1,0\">");
                        sb.AppendFormat("<GradientStop Offset=\"0\" Color=\"{0}\"/><GradientStop Offset=\".5\" Color=\"{1}\"/><GradientStop Offset=\".5\" Color=\"{1}\"/>", c, ec, c);
                        break;
                    case BackgroundGradientTypeEnum.DiagonalLeft:
                        sb.Append("<LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"1,1\">");
                        sb.AppendFormat("<GradientStop Offset=\"0\" Color=\"{0}\"/><GradientStop Offset=\"1\" Color=\"{1}\"/>", c, ec);
                        break;
                    case BackgroundGradientTypeEnum.DiagonalRight:
                        sb.Append("<LinearGradientBrush StartPoint=\"1,1\" EndPoint=\"0,0\">");
                        sb.AppendFormat("<GradientStop Offset=\"0\" Color=\"{0}\"/><GradientStop Offset=\"1\" Color=\"{1}\"/>", c, ec);
                        break;
                    // TODO:  figure out the mappings for these
                    case BackgroundGradientTypeEnum.HorizontalCenter:
                    case BackgroundGradientTypeEnum.VerticalCenter:
                    default:
                        sb.Append("<LinearGradientBrush StartPoint=\"0,0\" EndPoint=\"1,0\">");
                        sb.AppendFormat("<GradientStop Offset=\"0\" Color=\"{0}\"/><GradientStop Offset=\"1\" Color=\"{1}\"/>", c, ec);
                        break;
                }
                sb.Append("</LinearGradientBrush>");
                return;
            }
            else        // always put out (even when empty)
            {
                sb.AppendFormat("<SolidColorBrush Color=\"{0}\"/>", GetColor(si.BackgroundColor));
            }
            return;
        }

        private void DrawBorder(PageItem pi, StringBuilder sb, Rectangle r)
        {
            if (r.Height <= 0 || r.Width <= 0)		// no bounding box to use
                return;

            StyleInfo si = pi.SI;

            DrawLine(si.BColorTop, si.BStyleTop, PixelsX(si.BWidthTop), sb, r.X, r.Y, r.Right, r.Y);

            DrawLine(si.BColorRight, si.BStyleRight, PixelsX(si.BWidthRight), sb, r.Right, r.Y, r.Right, r.Bottom);

            DrawLine(si.BColorLeft, si.BStyleLeft, PixelsX(si.BWidthLeft), sb, r.X, r.Y, r.X, r.Bottom);

            DrawLine(si.BColorBottom, si.BStyleBottom, PixelsX(si.BWidthBottom), sb, r.X, r.Bottom, r.Right, r.Bottom);

            return;

        }

        private void DrawEllipse(PageEllipse pe, StringBuilder sb, Rectangle r)
        {
            StyleInfo si = pe.SI;

            // adjust drawing rectangle based on padding
            Rectangle r2 = new Rectangle(r.Left + PixelsX(si.PaddingLeft),
                                           r.Top + PixelsY(si.PaddingTop),
                                           r.Width - PixelsX(si.PaddingLeft - si.PaddingRight),
                                           r.Height - PixelsY(si.PaddingTop - si.PaddingBottom));

            sb.AppendFormat("<Ellipse Canvas.Left=\"{0}\" Canvas.Top=\"{1}\" Height=\"{2}\" Width=\"{3}\">",
                r2.Left, r2.Top, r2.Height, r2.Width);
            sb.Append("<Ellipse.Fill>");
            DrawBackground(sb, si);
            sb.Append("</Ellipse.Fill>");
            sb.Append("</Ellipse>");
        }

        public string GetImageType(string name)
        {
            return "image/png";             // always the same for now
        }

        public byte[] GetImage(string name)
        {
            PageImage pi;
            if (!_Images.TryGetValue(name, out pi))
                return null;

            Stream strm = null;
            MemoryStream ms = null;
            System.Drawing.Image im = null;
            try
            {
                strm = new MemoryStream(pi.ImageData);
                im = System.Drawing.Image.FromStream(strm);
                ms = new MemoryStream(pi.ImageData.Length);
                im.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] ba = ms.ToArray();

                return ba;
            }
            finally
            {
                if (strm != null)
                    strm.Close();
                if (ms != null)
                    ms.Close();
                if (im != null)
                    im.Dispose();
            }
        }
        private void DrawImage(PageImage pi, StringBuilder sb, Rectangle r)
        {
            string name = GetName(pi);
            _Images.Add(name, pi);              // TODO optimize this so that common images are only one URL (see PDF generation)

            StyleInfo si = pi.SI;

            string stretch;
            switch (pi.Sizing)
            {
                case ImageSizingEnum.AutoSize:
                    stretch = "None";
                    break;
                case ImageSizingEnum.Clip:
                    stretch = "None";
                    break;
                case ImageSizingEnum.FitProportional:
                    stretch = "Uniform";
                    break;
                case ImageSizingEnum.Fit:
                default:
                    stretch = "Fill";
                    break;
            }
            Rectangle r2 = new Rectangle(r.Left + PixelsX(si.PaddingLeft),
                                           r.Top + PixelsY(si.PaddingTop),
                                           r.Width - PixelsX(si.PaddingLeft - si.PaddingRight),
                                           r.Height - PixelsY(si.PaddingTop - si.PaddingBottom));

            string url;
            if (_ZipStream != null)
            {
                url = ImageHelper.SaveZipImage(_ZipImages, _ZipStream, pi);
            }
            else
            {
                url = ImageHelper.SaveImage(this.MapPathSecure(this.ImageDirectory), pi.ImageData, this.Context.Cache);

                url = this.ImageDirectory + "/" + url;
            }
            sb.AppendFormat("<Image Source=\"{0}\" Stretch=\"{1}\" " +
                            "Canvas.Left=\"{2}\" Canvas.Top=\"{3}\" Width=\"{4}\" Height=\"{5}\"/>", 
                url, stretch, r2.Left, r2.Top, r2.Width, r2.Height);
            
//            sb.Append("<Image Source=\"/dscrnshot1tn.jpg?rs:url=JustAnImage.rdl&rs:pageno=1&rs:image=n1\" Stretch=\"None\" Canvas.Left=\"260\" Canvas.Top=\"85\" Width=\"211\" Height=\"71\"/>");

        }

        private void DrawImageBackground(PageImage pi, StyleInfo si, StringBuilder sb, Rectangle r)
        {
            //Stream strm = null;
            //System.Drawing.Image im = null;
            //try
            //{
            //    strm = new MemoryStream(pi.ImageData);
            //    im = System.Drawing.Image.FromStream(strm);

            //    Rectangle r2 = new Rectangle(r.Left + PixelsX(si.PaddingLeft),
            //        r.Top + PixelsY(si.PaddingTop),
            //        r.Width - PixelsX(si.PaddingLeft + si.PaddingRight),
            //        r.Height - PixelsY(si.PaddingTop + si.PaddingBottom));

            //    int repeatX = 0;
            //    int repeatY = 0;
            //    switch (pi.Repeat)
            //    {
            //        case ImageRepeat.Repeat:
            //            repeatX = (int)Math.Floor(r2.Width / pi.SamplesW);
            //            repeatY = (int)Math.Floor(r2.Height / pi.SamplesH);
            //            break;
            //        case ImageRepeat.RepeatX:
            //            repeatX = (int)Math.Floor(r2.Width / pi.SamplesW);
            //            repeatY = 1;
            //            break;
            //        case ImageRepeat.RepeatY:
            //            repeatY = (int)Math.Floor(r2.Height / pi.SamplesH);
            //            repeatX = 1;
            //            break;
            //        case ImageRepeat.NoRepeat:
            //        default:
            //            repeatX = repeatY = 1;
            //            break;
            //    }

            //    //make sure the image is drawn at least 1 times 
            //    repeatX = Math.Max(repeatX, 1);
            //    repeatY = Math.Max(repeatY, 1);

            //    float startX = r2.Left;
            //    float startY = r2.Top;

            //    Region saveRegion = g.Clip;
            //    Region clipRegion = new Region(g.Clip.GetRegionData());
            //    clipRegion.Intersect(r2);
            //    g.Clip = clipRegion;

            //    for (int i = 0; i < repeatX; i++)
            //    {
            //        for (int j = 0; j < repeatY; j++)
            //        {
            //            float currX = startX + i * pi.SamplesW;
            //            float currY = startY + j * pi.SamplesH;
            //            g.DrawImage(im, new Rectangle(currX, currY, pi.SamplesW, pi.SamplesH));
            //        }
            //    }
            //    g.Clip = saveRegion;
            //}
            //finally
            //{
            //    if (strm != null)
            //        strm.Close();
            //    if (im != null)
            //        im.Dispose();
            //}
        }

        private void DrawLine(Color c, BorderStyleEnum bs, float w, StringBuilder sb,
                                int x, int y, int x2, int y2)
        {
            if (bs == BorderStyleEnum.None || c.IsEmpty || w <= 0)	// nothing to draw
                return;

            sb.AppendFormat("<Line X1=\"{0}\" Y1=\"{1}\" X2=\"{2}\" Y2=\"{3}\" Stroke=\"{4}\" StrokeThickness=\"{5}\"",
                x, y, x2, y2, GetColor(c), w);

            switch (bs)
            {
                case BorderStyleEnum.Dashed:
                    sb.Append(" StrokeDashArray=\"2,2\"");
                    break;
                case BorderStyleEnum.Dotted:
                    sb.Append(" StrokeDashArray=\"1,1\"");
                    break;
                default:
                    break;
            }
            sb.Append("/>");
        }

        private void ProcessHtml(PageTextHtml pth, StringBuilder sb, Rectangle clipRect)
        {
            pth.Build(_Pages.G);				// Builds the subobjects that make up the html
            this.ProcessPage(sb, pth, clipRect);
        }

        private void DrawPolygon(PagePolygon pp, StringBuilder sb)
        {
            StyleInfo si = pp.SI;

            sb.Append("<Polygon Points=\"");
            foreach (PointF p in pp.Points)
            {
                sb.AppendFormat("{0}, {1} ", PixelsX(p.X + si.PaddingLeft), PixelsY(p.Y + si.PaddingTop)); 
            }
            sb.Append("\">");
            sb.Append("<Polygon.Fill>");
            DrawBackground(sb, si);
            sb.Append("</Polygon.Fill>");
            sb.Append("</Polygon>");
        }
        
        private void DrawRectangle(PageRectangle pr, StringBuilder sb, Rectangle r)
        {
            StyleInfo si = pr.SI;

            // adjust drawing rectangle based on padding
            Rectangle r2 = new Rectangle(r.Left + PixelsX(si.PaddingLeft),
                                           r.Top + PixelsY(si.PaddingTop),
                                           r.Width - PixelsX(si.PaddingLeft - si.PaddingRight),
                                           r.Height - PixelsY(si.PaddingTop - si.PaddingBottom));

            sb.AppendFormat("<Rectangle Canvas.Left=\"{0}\" Canvas.Top=\"{1}\" Height=\"{2}\" Width=\"{3}\">",
                r2.Left, r2.Top, r2.Height, r2.Width);
            sb.Append("<Rectangle.Fill>");
            DrawBackground(sb, si);
            sb.Append("</Rectangle.Fill>");
            sb.Append("</Rectangle>");
        }

        private void DrawString(PageText pt, StringBuilder sb, Rectangle r)
        {
            StyleInfo si = pt.SI;
            string s = pt.Text;

            /*
			<Canvas Name ="obj3" Canvas.Left="10" Canvas.Top="2" Height="20" Width="180" Background="Blue"
					MouseLeftButtonDown="riDown" MouseMove="riMove" MouseLeftButtonUp="riUp" >
				<Canvas.Clip>
					<RectangleGeometry Rect="0,0,180,20" />
				</Canvas.Clip>

				<TextBlock FontFamily="Arial"
				   FontSize="25" Text="This text is in xxx footer">
				</TextBlock>
			</Canvas>		
             * */

            // adjust drawing rectangle based on padding
            Rectangle r2;
            //if (si.WritingMode == WritingModeEnum.tb_rl)
            //{   // we're going to rotate the text so width will be height and height will be width
            //    r2 = new Rectangle(r.Left + PixelsX(si.PaddingLeft),
            //                               r.Top + PixelsY(si.PaddingTop),
            //                               r.Height - PixelsY(si.PaddingTop - si.PaddingBottom),
            //                               r.Width - PixelsX(si.PaddingLeft - si.PaddingRight));
            //}
            //else
            r2 = new Rectangle(r.Left + PixelsX(si.PaddingLeft),
                                           r.Top + PixelsY(si.PaddingTop),
                                           r.Width - PixelsX(si.PaddingLeft - si.PaddingRight),
                                           r.Height - PixelsY(si.PaddingTop - si.PaddingBottom));
            if (r2.Height == 0 || r2.Width == 0)
            {
                sb.AppendFormat("<Canvas Name=\"{0}\" Canvas.Left=\"{1}\" Canvas.Top=\"{2}\" Background=\"{3}\" >",
                    GetName(pt), r2.Left, r2.Top, GetColor(si.BackgroundColor));
            }
            else
            {
                sb.AppendFormat("<Canvas Name=\"{0}\" Canvas.Left=\"{1}\" Canvas.Top=\"{2}\" Height=\"{3}\" Width=\"{4}\" Background=\"{5}\" >",
                    GetName(pt), r2.Left, r2.Top, r2.Height, r2.Width, GetColor(si.BackgroundColor));
                //            DrawBackground(sb, si);  // this would need some additional work to support gradient and textblock

                sb.AppendFormat("<Canvas.Clip><RectangleGeometry Rect=\"0,0,{0},{1}\" /></Canvas.Clip>", PixelsX(pt.W), PixelsY(pt.H));
            }

            sb.AppendFormat("<TextBlock TextWrapping=\"Wrap\" FontFamily=\"{0}\" Foreground=\"{1}\" FontSize=\"{2}\"",
                si.FontFamily, GetColor(si.Color), si.FontSize);

            if (si.FontStyle == FontStyleEnum.Italic)
                sb.Append(" FontStyle=\"Italic\"");

            if (si.TextDecoration == TextDecorationEnum.Underline)
                sb.Append(" TextDecorations=\"Underline\"");

            if (si.WritingMode == WritingModeEnum.tb_rl)        // we need to rotate text
                sb.AppendFormat(" RenderTransformOrigin=\"0.0,0.0\" Canvas.Left=\"{0}\"", r2.Width);

            // WEIGHT
            switch (si.FontWeight)
            {
                case FontWeightEnum.Lighter:
                    sb.Append(" FontWeight=\"Light\"");
                    break;
                case FontWeightEnum.W500:
                case FontWeightEnum.Bold:
                    sb.Append(" FontWeight=\"Bold\"");
                    break;
                case FontWeightEnum.Bolder:
                    sb.Append(" FontWeight=\"ExtraBold\"");
                    break;
                case FontWeightEnum.W600:
                case FontWeightEnum.W700:
                    sb.Append(" FontWeight=\"Black\"");
                    break;
                case FontWeightEnum.W800:
                case FontWeightEnum.W900:
                    sb.Append(" FontWeight=\"ExtraBlack\"");
                    break;
                case FontWeightEnum.Normal:
                default:
                    sb.Append(" FontWeight=\"Normal\"");
                    break;
            }
            /*
            // ALIGNMENT not supported in SilverLight 1.0
                switch (si.TextAlign)
                {
                    case TextAlignEnum.Right:
                        break;
                    case TextAlignEnum.Center:
                        break;
                    case TextAlignEnum.Left:
                    default:
                        break;
                }
                switch (si.VerticalAlign)
                {
                    case VerticalAlignEnum.Bottom:
                        break;
                    case VerticalAlignEnum.Middle:
                        break;
                    case VerticalAlignEnum.Top:
                    default:
                        break;
                }
            */
            sb.Append("><Run>");
            sb.Append(fyiReporting.RDL.XmlUtil.XmlAnsi(pt.Text));
            sb.Append("</Run>");
            if (si.WritingMode == WritingModeEnum.tb_rl)
            {
                sb.AppendFormat("<TextBlock.RenderTransform><RotateTransform Angle=\"90\"/></TextBlock.RenderTransform>");
                //sb.AppendFormat("<TextBlock.RenderTransform><RotateTransform Angle=\"90\" CenterX=\"{0}\" CenterY=\"{1}\"/></TextBlock.RenderTransform>",
                //    r2.Width, r2.Height);
            }
            sb.Append("</TextBlock>");
            sb.Append("</Canvas>");
        }

        string GetColor(Color c)
        {
            if (c.IsEmpty)
                return "#00000000";     // this should be transparent since alpha channel is 0

            string s = string.Format("#{0}{1}{2}", GetColor(c.R), GetColor(c.G), GetColor(c.B));
            return s;
        }

        string GetColor(byte b)
        {
            string sb = Convert.ToString(b, 16).ToUpperInvariant();

            return sb.Length > 1 ? sb : "0" + sb;
        }

        private string GetName(PageItem pi)
        {
            _UniqueName++; 
            return string.Format("n{0}", _UniqueName);
        }

        private float POINTSIZEF = 72.27f;

        private float PointsX(float x)		// pixels to points
        {
            return x * POINTSIZEF / DpiX;
        }

        private float PointsY(float y)
        {
            return y * POINTSIZEF / DpiY;
        }

        private int PixelsX(float x)		// points to pixels
        {
            int r = (int)((double)x * DpiX / POINTSIZEF);
            if (r == 0 && x > .0001f)
                r = 1;
            return r;
        }

        private int PixelsY(float y)
        {
            int r= (int)((double)y * DpiY / POINTSIZEF);
            if (r == 0 && y > .0001f)
                r = 1;
            return r;
        }

    }

    internal class ImageHelper
    {
        /// <summary>
        /// SaveZipImage saves the bytes to a zip entry and returns the name.  The passed cache is used
        /// to remember file names so that the same image isn't repeatedly saved.
        /// </summary>
        /// <param name="zipCache"></param>
        /// <param name="_ZipStream"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        internal static string SaveZipImage(Dictionary<string, string> zipCache, ZipOutputStream _ZipStream, PageImage pi)
        { 
            string imgname;
            imgname = (pi.Name != null)?
                string.Format("{0}.{1}", pi.Name, GetExtension(pi.ImgFormat)): 
                string.Format("i_{0}.{1}", zipCache.Count + 1, GetExtension(pi.ImgFormat));

            if (zipCache.ContainsKey(imgname))
                return imgname;

            zipCache.Add(imgname, imgname);

            ZipEntry ze = new ZipEntry(imgname);
            _ZipStream.PutNextEntry(ze);
            _ZipStream.Write(pi.ImageData, 0, pi.ImageData.Length);

            return imgname; 
        }

        private static string GetExtension(System.Drawing.Imaging.ImageFormat imf)
        {
			string suffix;

			if (imf == System.Drawing.Imaging.ImageFormat.Jpeg)
				suffix = "jpeg";
			else if (imf == System.Drawing.Imaging.ImageFormat.Gif)
				suffix = "gif";
			else if (imf == System.Drawing.Imaging.ImageFormat.Png)
				suffix = "png";
			else
				suffix = "unkimg";
            return suffix;
        }

        /// <summary>
        /// SaveImage saves the bytes to a file and returns the file name.  The cache is used to remember
        /// the temporary file names and to timeout (and delete) the file.
        /// </summary>
        /// <param name="ba"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        static internal string SaveImage(string path, byte[] ba, Cache c)
        {
            Stream io = null;
            string filename;
            string url;
            try
            {
                io = ImageHelper.GetIOStream(path, out filename, out url);

                io.Write(ba, 0, ba.Length);
                io.Close();
                io = null;
                ImageCallBack icb = new ImageCallBack();

                CacheItemRemovedCallback onRemove = new CacheItemRemovedCallback(icb.RemovedCallback);
  
                c.Insert(Path.GetFileNameWithoutExtension(filename), 
                    filename, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero, CacheItemPriority.Normal, onRemove);
            
            }
            finally
            {
                if (io != null)
                    io.Close();
            }
            return url;
        }

        static private Stream GetIOStream(string path, out string file, out string url)
        {
            Stream io = null;
            lock (typeof(ImageHelper))	// single thread lock while we're creating a file
            {							//  this assumes no other routine creates files in this directory with same naming scheme
                // Obtain a new file name
                Random rand = new Random(DateTime.Now.Millisecond);
                int rnd;
                FileInfo fi;
                while (true)
                {
                    rnd = rand.Next();
                    url = "fyisv" + rnd.ToString() + ".png";
                    file = path + Path.DirectorySeparatorChar + url;
                    fi = new FileInfo(file);
                    if (!fi.Exists)
                        break;
                }

                io = fi.Create();
 //               fi.Attributes |= FileAttributes.Temporary;
            }
            return io;
        }
        
        private class ImageCallBack
        {
            public void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
            {
                string filename = v as string;
                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex)
                {       // not sure where we can report this error    
                    string msg = ex.Message;
                }
            }
        }

    }
    
}
