

using System;


namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Style Background image source enumeration
	///</summary>

	internal enum StyleBackgroundImageSourceEnum
	{
		External,		// The Value contains a constant or
		// expression that evaluates to for the location
		// of the image
		Embedded,		// The Value contains a constant
		// or expression that evaluates to the name of
		// an EmbeddedImage within the report
		Database,		// The Value contains an expression
		// (a field in the database) that evaluates to the
		// binary data for the image.
		Unknown			// Illegal (or no) value specified
	}

	internal class StyleBackgroundImageSource
	{
		static internal StyleBackgroundImageSourceEnum GetStyle(string s)
		{
			StyleBackgroundImageSourceEnum rs;

			switch (s)
			{		
				case "External":
					rs = StyleBackgroundImageSourceEnum.External;
					break;
				case "Embedded":
					rs = StyleBackgroundImageSourceEnum.Embedded;
					break;
				case "Database":
					rs = StyleBackgroundImageSourceEnum.Database;
					break;
				default:		// user error just force to normal TODO
					rs = StyleBackgroundImageSourceEnum.External;
					break;
			}
			return rs;
		}
	}

}
