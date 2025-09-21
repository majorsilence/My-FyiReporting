

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Handle the image size enumeration.  AutoSize, Fit, FitProportional, Clip
	///</summary>
	public enum ImageSizingEnum
	{
		/// <summary>
		/// The borders should grow/shrink to accommodate the image (Default).
		/// </summary>
		AutoSize,	
		/// <summary>
		/// The image is resized to exactly match the height and width of the image element.
		/// </summary>
		Fit,		
		/// <summary>
		/// The image should be resized to fit, preserving aspect ratio.
		/// </summary>
		FitProportional,	
		/// <summary>
		/// The image should be clipped to fit.		
		/// </summary>
		Clip		
	}
	/// <summary>
	/// Use ImageSizing when you want to take a string and map it to the ImageSizingEnum. 
	/// </summary>
	public class ImageSizing
	{
		/// <summary>
		/// Given a string return the cooresponding enumeration.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>The enumerated value corresponding to the string.</returns>
        //[System.Diagnostics.DebuggerStepThrough] 
		static public ImageSizingEnum GetStyle(string s)
		{
			return GetStyle(s, null);
		}

		static internal ImageSizingEnum GetStyle(string s, ReportLog rl)
			{
			ImageSizingEnum rs;

            try
            {
                rs = (ImageSizingEnum)Enum.Parse(typeof(ImageSizingEnum), s);
            }
            catch 
            {
                if (rl != null)
                {
                    rl.LogError(4, "Unknown ImageSizing '" + s + "'. AutoSize assumed.");
                }
                rs = ImageSizingEnum.AutoSize; 
            }
            
            /*
			switch (s)
			{		
				case "AutoSize":
					rs = ImageSizingEnum.AutoSize;
					break;
				case "Fit":
					rs = ImageSizingEnum.Fit;
					break;
				case "FitProportional":
					rs = ImageSizingEnum.FitProportional;
					break;
				case "Clip":
					rs = ImageSizingEnum.Clip;
					break;
				default:		
					if (rl != null)
						rl.LogError(4, "Unknown ImageSizing '" + s + "'.  AutoSize assumed.");

					rs = ImageSizingEnum.AutoSize;
					break;
			}
            */
			return rs;
		}
	}

}
