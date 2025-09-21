

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	///  Handles the Image source enumeration.  External, Embedded, Database
	///</summary>
	internal enum ImageSourceEnum
	{
		External,	// The Value contains a constant or
					// expression that evaluates to the location of
					// the image
		Embedded,	// The Value contains a constant
					// or expression that evaluates to the name of
					// an EmbeddedImage within the report.
		Database,	// The Value contains an
					// expression (typically a field in the database)
					// that evaluates to the binary data for the
					// image.
		Unknown		// Illegal or unspecified
	}
	internal class ImageSource
	{
		static internal ImageSourceEnum GetStyle(string s)
		{
			ImageSourceEnum rs;

			switch (s)
			{		
				case "External":
					rs = ImageSourceEnum.External;
					break;
				case "Embedded":
					rs = ImageSourceEnum.Embedded;
					break;
				case "Database":
					rs = ImageSourceEnum.Database;
					break;
				default:		
					rs = ImageSourceEnum.Unknown;
					break;
			}
			return rs;
		}
	}

}
