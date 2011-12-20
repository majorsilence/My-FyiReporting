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
using System.Collections;
using System.Net;
using System.Text;

namespace fyiReporting.Data
{
	/// <summary>
	/// MultipleStreamReader provides a logical single stream over multiple streams.  Only support ReadLine.
	/// </summary>
	public class MultipleStreamReader : TextReader
	{
		Queue _files;
		StreamReader _sr=null;

		/// <summary>
		/// Constructor taking a path list string.  Files are separated with blanks.  
		/// If a file contains a blank then it should be enclosed with quotes (").
		/// </summary>
		/// <param name="pathlist"></param>
		public MultipleStreamReader(string pathlist)
		{
			GetFileList(pathlist);			// split the string into multiple files
			GetStream();					// get the first file
		}

		void GetFileList(string pathlist)
		{
			_files = new Queue();

			StringBuilder sb=null;
			
			int STARTFILE=0;
			int INFILE=1;
			int state = STARTFILE;
			bool bQuote=false;

			foreach (char c in pathlist)
			{
				if (state == STARTFILE)
				{
					sb = new StringBuilder();
					bQuote = (c == '"');

					if (!bQuote)
						sb.Append(c);
					state = INFILE;
				}
				else if (state == INFILE)
				{
					if ( (bQuote && c == '"') ||	// quoted file and on last quote
						 (!bQuote && c == ' '))		// not quoted file and blank
					{
						AddFileToQueue(sb.ToString(), bQuote);

						state = STARTFILE;
					}
					else
						sb.Append(c);
				}
			}

			if (sb != null && sb.Length > 0)
				AddFileToQueue(sb.ToString(), bQuote);

			return;
		}

		void AddFileToQueue(string f, bool asis)
		{
			if (!asis)
				f = f.Trim();			// get rid of extraneous blanks

			if (f.Length <= 0)
				return;

			if (Path.GetFileNameWithoutExtension(f) == "*")
			{	
				int i = f.LastIndexOf('*');
				string path = f.Substring(0, i);
				string[] fl = Directory.GetFiles(path, "*" + Path.GetExtension(f)); 
				foreach (string file in fl)
					_files.Enqueue(file);
			}
			else
				_files.Enqueue(f);
		}

		StreamReader GetStream() 
		{
			if (_sr != null)		// close out the previous file before getting another
			{
				_sr.Close();
				_sr = null;
			}

			if (_files == null || _files.Count == 0)
				return null;

			string fname = _files.Dequeue() as string;
			Stream strm=null;

			if (fname.StartsWith("http:") ||
				fname.StartsWith("file:") ||
				fname.StartsWith("https:"))
			{
				WebRequest wreq = WebRequest.Create(fname);
				WebResponse wres = wreq.GetResponse();
				strm = wres.GetResponseStream();
			}
			else
				strm = new FileStream(fname, System.IO.FileMode.Open, FileAccess.Read);		

			_sr = new StreamReader(strm);

			return _sr;
		}

		public override void Close()
		{
			if (_sr != null)
			{
				_sr.Close();
				_sr = null;
			}

			_files.Clear();
		}

		public override string ReadLine()
		{
			if (_sr == null)
				return null;

			string rs = _sr.ReadLine();

			if (rs == null)
			{
				GetStream();
				rs = ReadLine();
			}

			return rs;
		}

	}
}
