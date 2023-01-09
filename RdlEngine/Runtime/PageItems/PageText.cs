using System;

namespace fyiReporting.RDL
{
	public class PageText : PageItem, ICloneable
	{
		string text;            // the text
		float descent;          // in some cases the Font descent will be recorded; 0 otherwise
		bool bGrow;
		bool _NoClip = false;		// on drawing disallow clipping
		PageTextHtml _HtmlParent = null;

		public PageText(string t)
		{
			text = t;
			descent = 0;
			bGrow = false;
		}

		public PageTextHtml HtmlParent
		{
			get { return _HtmlParent; }
			set { _HtmlParent = value; }
		}

		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		public float Descent
		{
			get { return descent; }
			set { descent = value; }
		}
		public bool NoClip
		{
			get { return _NoClip; }
			set { _NoClip = value; }
		}

		public bool CanGrow
		{
			get { return bGrow; }
			set { bGrow = value; }
		}
		#region ICloneable Members

		new public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion
	}
}