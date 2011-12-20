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
