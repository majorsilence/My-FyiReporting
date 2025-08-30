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


namespace Majorsilence.Reporting.Rdl
{
	internal enum ThreeDPropertiesProjectionModeEnum
	{
		Perspective,
		Orthographic
	}

	internal class ThreeDPropertiesProjectionMode
	{
		static internal ThreeDPropertiesProjectionModeEnum GetStyle(string s)
		{
			ThreeDPropertiesProjectionModeEnum pm;

			switch (s.ToLowerInvariant())
			{		
				case "perspective":
					pm = ThreeDPropertiesProjectionModeEnum.Perspective;
					break;
				case "orthographic":
					pm = ThreeDPropertiesProjectionModeEnum.Orthographic;
					break;
				default:
					pm = ThreeDPropertiesProjectionModeEnum.Perspective;
					break;
			}
			return pm;
		}
	}


}
