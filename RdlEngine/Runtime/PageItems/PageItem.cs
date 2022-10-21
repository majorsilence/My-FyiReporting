using System;

namespace fyiReporting.RDL
{
	public class PageItem : ICloneable, IComparable
	{
		Page parent;            // parent page
		float x;                // x coordinate
		float y;                // y coordinate
		float h;                // height  --- line redefines as Y2
		float w;                // width   --- line redefines as X2
		string hyperlink;       //  a hyperlink the object should link to
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
			get { return x; }
			set { x = value; }
		}

		public float Y
		{
			get { return y; }
			set { y = value; }
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
			get { return h; }
			set { h = value; }
		}

		public float W
		{
			get { return w; }
			set { w = value; }
		}

		public string HyperLink
		{
			get { return hyperlink; }
			set { hyperlink = value; }
		}

		public string BookmarkLink
		{
			get { return bookmarklink; }
			set { bookmarklink = value; }
		}

		public string Bookmark
		{
			get { return bookmark; }
			set { bookmark = value; }
		}

		public string Tooltip
		{
			get { return tooltip; }
			set { tooltip = value; }
		}

		public StyleInfo SI
		{
			get { return si; }
			set { si = value; }
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

		internal void Dispose()
		{
			si = null;
			parent = null;
		}

		#endregion
	}
}