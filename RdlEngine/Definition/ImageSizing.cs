/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

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


namespace fyiReporting.RDL
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
