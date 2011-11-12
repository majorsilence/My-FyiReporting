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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace fyiReporting.RDL
{
	///<summary>
	/// Represents all the pages of a report.  Needed when you need
	/// render based on pages.  e.g. PDF
	///</summary>
	public class Pages : IEnumerable
	{
		Bitmap _bm;						// bitmap to build graphics object 
		Graphics _g;					// graphics object
		Report _report;					// owner report
        List<Page> _pages;				// array of pages
		Page _currentPage;				// the current page; 1st page if null
		float _BottomOfPage;			// the bottom of the page
		float _PageHeight;				// default height for all pages
		float _PageWidth;				// default width for all pages
	
		public Pages(Report r)
		{
			_report = r;
            _pages = new List<Page>();	// array of Page objects

			_bm = new Bitmap(10, 10);	// create a small bitmap to base our graphics
			_g = Graphics.FromImage(_bm);
		}

		internal Report Report
		{
			get { return _report;}
		}

		public Page this[int index]
		{
			get {return _pages[index];}
		}

		public int Count
		{
			get {return _pages.Count;}
		}

		public void AddPage(Page p)
		{
			_pages.Add(p);
			_currentPage = p;
		}

		public void NextOrNew()
		{
			if (_currentPage == this.LastPage)
				AddPage(new Page(PageCount+1));
			else
			{
				_currentPage = _pages[_currentPage.PageNumber];  
				_currentPage.SetEmpty();			
			}
		}

		/// <summary>
		/// CleanUp should be called after every render to reduce resource utilization.
		/// </summary>
		public void CleanUp()
		{
			if (_g != null)
			{
				_g.Dispose();
				_g = null;
			}
			if (_bm != null)
			{
				_bm.Dispose();
				_bm = null;
			}
		}

        public void SortPageItems()
        {
            foreach (Page p in this)
            {
                p.SortPageItems();
            }
        }

		public float BottomOfPage
		{
			get { return _BottomOfPage; }
			set { _BottomOfPage = value; }
		}

		public Page CurrentPage
		{
			get 
			{ 
				if (_currentPage != null)
					return _currentPage;
				
				if (_pages.Count >= 1)
				{
					_currentPage = _pages[0];
					return _currentPage;
				}

				return null;
			}

			set
			{
                _currentPage = value;
#if DEBUG
                if (value == null)
                    return;
                foreach (Page p in _pages)
                {
                    if (p == value)
                        return;
                }
                throw new Exception("CurrentPage must be in the list of pages");
#endif
			}
		}

		public Page FirstPage
		{
			get 
			{
				if (_pages.Count <= 0)
					return null;
				else
					return _pages[0];
			}
		}

		public Page LastPage
		{
			get 
			{
				if (_pages.Count <= 0)
					return null;
				else
					return _pages[_pages.Count-1];
			}
		}

		public float PageHeight
		{
			get {return _PageHeight;}
			set {_PageHeight = value;}
		}

		public float PageWidth
		{
			get {return _PageWidth;}
			set {_PageWidth = value;}
		}

		public void RemoveLastPage()
		{
			Page lp = LastPage;

			if (lp == null)				// if no last page nothing to do
				return;			

			_pages.RemoveAt(_pages.Count-1);	// remove the page

			if (this.CurrentPage == lp)	// reset the current if necessary
			{
				if (_pages.Count <= 0)
					CurrentPage = null;
				else
					CurrentPage = _pages[_pages.Count-1];
			}

			return;
		}

		public Graphics G
		{
			get 
			{
				if (_g == null)
				{
					_bm = new Bitmap(10, 10);	// create a small bitmap to base our graphics
					_g = Graphics.FromImage(_bm);
                }
				return _g; 
			}
		}

		public int PageCount
		{
			get { return _pages.Count; }
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()		// just loop thru the pages
		{
			return _pages.GetEnumerator();
		}

		#endregion
	}

	public class Page : IEnumerable
	{
		// note: all sizes are in points
		int _pageno;
        List<PageItem> _items;				// array of items on the page
		float _yOffset;					// current y offset; top margin, page header, other details, ... 
		float _xOffset;					// current x offset; margin, body taken into account?
		int _emptyItems;				// # of items which constitute empty
        bool _needSort;                 // need sort
        int _lastZIndex;                // last ZIndex
		System.Collections.Generic.Dictionary<string, Rows> _PageExprReferences;	// needed to save page header/footer expressions

		public Page(int page)
		{
			_pageno = page;
            _items = new List<PageItem>();
			_emptyItems = 0;
            _needSort = false;
		}

        public PageItem this[int index]
        {
            get { return _items[index]; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

		public void InsertObject(PageItem pi)
		{
			AddObjectInternal(pi);
			_items.Insert(0, pi);
		}

		public void AddObject(PageItem pi)
		{
			AddObjectInternal(pi);
			_items.Add(pi);
		}
		
		private void AddObjectInternal(PageItem pi)
		{
            pi.Page = this;
            pi.ItemNumber = _items.Count;
            if (_items.Count == 0)
                _lastZIndex = pi.ZIndex;
            else if (_lastZIndex != pi.ZIndex)
                _needSort = true;

			// adjust the page item locations
			pi.X += _xOffset;
			pi.Y += _yOffset;
			if (pi is PageLine)
			{
				PageLine pl = pi as PageLine;
				pl.X2 += _xOffset;
				pl.Y2 += _yOffset;
			}
            else if (pi is PagePolygon)
            {
                PagePolygon pp = pi as PagePolygon;
                for (int i = 0; i < pp.Points.Length; i++ )
                {
                    pp.Points[i].X += _xOffset;
                    pp.Points[i].Y += _yOffset;
                }
            }
            else if (pi is PageCurve)
            {
                PageCurve pc = pi as PageCurve;
                for (int i = 0; i < pc.Points.Length; i++)
                {
                    pc.Points[i].X += _xOffset;
                    pc.Points[i].Y += _yOffset;
                }
            }
		}

		public bool IsEmpty()
		{
			return _items.Count > _emptyItems? false: true;
		}

        public void SortPageItems()
        {
            if (!_needSort)
                return;
            _items.Sort();
        }

		public void ResetEmpty()
		{
			_emptyItems = 0;
		}

		public void SetEmpty()
		{
			_emptyItems = _items.Count;
		}

		public int PageNumber
		{
			get { return _pageno;}
		}

		public float XOffset
		{
			get { return _xOffset; }
			set { _xOffset = value; }
		}

		public float YOffset
		{
			get { return _yOffset; }
			set { _yOffset = value; }
		}

		internal void AddPageExpressionRow(Report rpt, string exprname, Row r)
		{
			if (exprname == null || r == null)
				return;

			if (_PageExprReferences == null)
				_PageExprReferences = new Dictionary<string, Rows>();

			Rows rows=null;
            _PageExprReferences.TryGetValue(exprname, out rows);
            if (rows == null)
			{
				rows = new Rows(rpt);
				rows.Data = new List<Row>();
				_PageExprReferences.Add(exprname, rows);
			}
			Row row = new Row(rows, r);	// have to make a new copy
			row.RowNumber = rows.Data.Count;
			rows.Data.Add(row);			// add row to rows
			return;
		}

		internal Rows GetPageExpressionRows(string exprname)
		{
			if (_PageExprReferences == null)
				return null;

            Rows rows=null;
            _PageExprReferences.TryGetValue(exprname, out rows);
            return rows;
		}

		internal void ResetPageExpressions()
		{
			_PageExprReferences = null;		// clear it out; not needed once page header/footer are processed
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()		// just loop thru the pages
		{
			return _items.GetEnumerator();
		}

		#endregion
	}

    public class PageItem : ICloneable, IComparable
	{
        Page parent;            // parent page
		float x;				// x coordinate
		float y;				// y coordinate
		float h;				// height  --- line redefines as Y2
		float w;				// width   --- line redefines as X2
		string hyperlink;		//  a hyperlink the object should link to
		string bookmarklink;	//  a hyperlink within the report object should link to
        string bookmark;        //  bookmark text for this pageItem
		string tooltip;			//  a message to display when user hovers with mouse
        int zindex;             // zindex; items will be sorted by this
        int itemNumber;         //  original number of item
		StyleInfo si;			// all the style information evaluated
        bool allowselect = true;  // allow selection of this item

        public Page Page
        {
            get { return parent; }
            set { parent = value; }
        }

        public bool AllowSelect
        {
            get { return allowselect; }
            set { allowselect = value; }
        }
		public float X
		{
			get { return x;}
			set { x = value;}
		}

		public float Y
		{
			get { return y;}
			set { y = value;}
		}

        public int ZIndex
        {
            get { return zindex; }
            set { zindex = value; }
        }

        public int ItemNumber
        {
            get { return itemNumber; }
            set { itemNumber = value; }
        }

		public float H
		{
			get { return h;}
			set { h = value;}
		}

		public float W
		{
			get { return w;}
			set { w = value;}
		}

		public string HyperLink
		{
			get { return hyperlink;}
			set { hyperlink = value;}
		}

		public string BookmarkLink
		{
			get { return bookmarklink;}
			set { bookmarklink = value;}
		}

        public string Bookmark
        {
            get { return bookmark; }
            set { bookmark = value; }
        }

		public string Tooltip
		{
			get { return tooltip;}
			set { tooltip = value;}
		}

		public StyleInfo SI
		{
			get { return si;}
			set { si = value;}
		}
		#region ICloneable Members

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
        #region IComparable Members

        // Sort items based on zindex, then on order items were added to array
        public int CompareTo(object obj)
        {
            PageItem pi = obj as PageItem;

            int rc = this.zindex - pi.zindex;
            if (rc == 0)
                rc = this.itemNumber - pi.itemNumber;
            return rc;
        }

        #endregion
	}

	public class PageImage : PageItem, ICloneable
	{
		string name;				// name of object if constant image
		ImageFormat imf;			// type of image; png, jpeg are supported
		byte[] imageData;
		int samplesW;
		int samplesH;
		ImageRepeat repeat;
		ImageSizingEnum sizing;

		public PageImage(ImageFormat im, byte[] image, int w, int h)
		{
			Debug.Assert(im == ImageFormat.Jpeg || im == ImageFormat.Png || im == ImageFormat.Gif || im == ImageFormat.Wmf, 
							"PageImage only supports Jpeg, Gif and Png and WMF image formats (Thanks HYNE!).");
			imf = im;
			imageData = image;
			samplesW = w;
			samplesH = h;
			repeat = ImageRepeat.NoRepeat;
			sizing = ImageSizingEnum.AutoSize;
		}

		public byte[] ImageData
		{
			get {return imageData;}
		}

		public ImageFormat ImgFormat
		{
			get {return imf;}
		}

		public string Name
		{
			get {return name;}
			set {name = value;}
		}

		public ImageRepeat Repeat
		{
			get {return repeat;}
			set {repeat = value;}
		}

		public ImageSizingEnum Sizing
		{
			get {return sizing;}
			set {sizing = value;}
		}

		public int SamplesW
		{
			get {return samplesW;}
		}

		public int SamplesH
		{
			get {return samplesH;}
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}

	public enum ImageRepeat
	{
		Repeat,			// repeat image in both x and y directions
		NoRepeat,		// don't repeat
		RepeatX,		// repeat image in x direction
		RepeatY			// repeat image in y direction
	}

    public class PageEllipse : PageItem, ICloneable
    {
        public PageEllipse()
        {
 
        }

        #region ICloneable Members

        new public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

	public class PageLine : PageItem, ICloneable
	{
		public PageLine()
		{
		}

		public float X2
		{
			get {return W;}
			set {W = value;}
		}

		public float Y2
		{
			get {return H;}
			set {H = value;}
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}

    public class PageCurve : PageItem, ICloneable
    {
        PointF[] _pointsF;
        int _offset;
        float _Tension;

        public PageCurve()
        {
        }

        public PointF[] Points
        {
            get { return _pointsF; }
            set { _pointsF = value; }
        }

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public float Tension
        {
            get { return _Tension; }
            set { _Tension = value; }
        }
        #region ICloneable Members

        new public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }


    public class PagePolygon : PageItem, ICloneable
    {
        PointF[] Ps;
        public PagePolygon()
        { 
        }
        public PointF[] Points
        {
            get {return Ps;}
            set {Ps = value;}
        }
    }

    public class PagePie : PageItem, ICloneable
    {
        Single SA;
        Single SW;
        public PagePie()
        {
        }
        public Single StartAngle
        {
            get { return SA; }
            set { SA = value; }
        }
        public Single SweepAngle
        {
            get { return SW; }
            set { SW = value; }
        }

        #region ICloneable Members

        new public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

	public class PageRectangle : PageItem, ICloneable
	{
		public PageRectangle()
		{
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}

	public class PageText : PageItem, ICloneable
	{
		string text;			// the text
		float descent;			// in some cases the Font descent will be recorded; 0 otherwise
		bool bGrow;
		bool _NoClip=false;		// on drawing disallow clipping
        PageTextHtml _HtmlParent=null;

		public PageText(string t)
		{
			text = t;
			descent=0;
			bGrow=false;
		}

        public PageTextHtml HtmlParent
        {
            get { return _HtmlParent; }
            set { _HtmlParent = value; }
        }

		public string Text
		{
			get {return text;}
			set {text = value;}
		}

		public float Descent
		{
			get {return descent;}
			set {descent = value;}
		}
		public bool NoClip
		{
			get {return _NoClip;}
			set {_NoClip = value;}
		}

		public bool CanGrow
		{
			get {return bGrow;}
			set {bGrow = value;}
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}
