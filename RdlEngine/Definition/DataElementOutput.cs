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
	/// Filter operators
	///</summary>
	public enum DataElementOutputEnum
	{
		Output,		// Indicates the item should appear in the output
		NoOutput,	// Indicates the item should not appear in the output
		ContentsOnly,	// Indicates the item should not appear in the XML, but its contents should be
						// rendered as if they were in this item�s
						// container. Only applies to Lists.
		Auto		// (Default): Will behave as NoOutput for
					// Textboxes with constant values,
					// ContentsOnly for Rectangles and Output for
					// all other items		

	}

	public class DataElementOutput
	{
        static public DataElementOutputEnum GetStyle(string s)
        {
            return GetStyle(s, null);
        }

		static internal DataElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			DataElementOutputEnum rs;

			switch (s)
			{		
				case "Output":
					rs = DataElementOutputEnum.Output;
					break;
				case "NoOutput":
					rs = DataElementOutputEnum.NoOutput;
					break;
				case "ContentsOnly":
					rs = DataElementOutputEnum.ContentsOnly;
					break;
				case "Auto":
					rs = DataElementOutputEnum.Auto;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown DataElementOutput '" + s + "'.  Auto assumed.");
					rs = DataElementOutputEnum.Auto;
					break;
			}
			return rs;
		}
	}

}
