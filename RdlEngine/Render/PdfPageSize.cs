
namespace Majorsilence.Reporting.Rdl
{
    /// <summary>
    /// Specify the page size in 1/72 inches units.
    /// </summary>
    internal struct PdfPageSize
	{
		internal int xWidth;
		internal int yHeight;
		internal int leftMargin;
		internal int rightMargin;
		internal int topMargin;
		internal int bottomMargin;

		internal PdfPageSize(int width,int height)
		{
			xWidth=width;
			yHeight=height;
			leftMargin=0;
			rightMargin=0;
			topMargin=0;
			bottomMargin=0;
		}
		internal void SetMargins(int L,int T,int R,int B)
		{
			leftMargin=L;
			rightMargin=R;
			topMargin=T;
			bottomMargin=B;
		}
	}

}
