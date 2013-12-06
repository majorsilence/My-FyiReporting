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
using System.IO;
using RdlEngine.Resources;

namespace fyiReporting.RDL
{
	/// <summary>
	/// char reader simply reads entire file into a string and processes.
	/// </summary>
	internal class CharReader
	{
		string file = null;
		int    ptr  = 0;

		int col = 1;				// column within line
		int savecol = 1;			//   saved column before a line feed
		int line = 1;				// line within file

		/// <summary>
		/// Initializes a new instance of the CharReader class.
		/// </summary>
		/// <param name="textReader">TextReader with DPL definition.</param>
		internal CharReader(TextReader textReader)
		{
			file = textReader.ReadToEnd();
			textReader.Close();
		}
		
		/// <summary>
		/// Returns the next char from the stream.
		/// </summary>
		/// <returns>The next char.</returns>
		internal char GetNext()
		{
			if (EndOfInput()) 
			{
				Console.WriteLine("warning : FileReader.GetNext : Read char over EndOfInput.");
				return '\0';
			}
			char ch = file[ptr++];
			col++;					// increment column counter

			if(ch == '\n') 
			{
				line++;				// got new line
				savecol = col;
				col = 1;			// restart column counter
			}
			return ch;
		}
		
		/// <summary>
		/// Returns the next char from the stream without removing it.
		/// </summary>
		/// <returns>The top char.</returns>
		internal char Peek()
		{
			if (EndOfInput()) // ok to peek at end of file
				return '\0';

			return file[ptr];
		}
		
		/// <summary>
		/// Undoes the extracting of the last char.
		/// </summary>
		internal void UnGet()
		{
			--ptr;
			if (ptr < 0) 
				throw new Exception(Strings.CharReader_Error_FileReaderUnGetFirstChar);
			
			char ch = file[ptr];
			if (ch == '\n')				// did we unget a new line?
			{
				line--;					// back up a line
				col = savecol;			// go back to previous column too
			}
	}
		
		/// <summary>
		/// Returns True if end of input was reached; otherwise False.
		/// </summary>
		/// <returns>True if end of input was reached; otherwise False.</returns>
		internal bool EndOfInput()
		{
			return ptr >= file.Length;
		}

		/// <summary>
		/// Gets the current column.
		/// </summary>
		internal int Column 
		{
			get
			{
				return col;
			}
		}

		/// <summary>
		/// Gets the current line.
		/// </summary>
		internal int Line
		{
			get
			{
				return line;
			}
		}
	}
}
