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
using fyiReporting.RDL;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace fyiReporting.RDL
{
	
	///<summary>
	/// An implementation of IStreamGen.  Used for single file with memory stream.
	/// XML and PDF are the only types that will work with this implementation.
	///</summary>

	public class MemoryStreamGen : IStreamGen, IDisposable
	{
		static internal long Counter;			// counter used for unique expression count
		MemoryStream _io;
		StreamWriter _sw=null;
        List<MemoryStream> _MemoryList;			// array of MemoryStreams - 1st is main stream; others are generated 
										//   for charts, images, ...
		List<string> _MemoryNames;			// names associated with the MemoryStreams
		internal string Prefix;			// used as prefix to names generated
		internal string Suffix;			// used as a suffix to names generated
		internal string Extension="";		// extension name for first file

		public MemoryStreamGen() : this(null, null, null) {}
		public MemoryStreamGen(string prefix, string suffix, string extension)
		{
			Prefix = prefix;
			Suffix = suffix;
			Extension = extension;

			_io = new MemoryStream();
            _MemoryList = new List<MemoryStream>();
			_MemoryList.Add(_io);
			_MemoryNames = new List<string>();

			// create the first name
			string unique = Interlocked.Increment(ref MemoryStreamGen.Counter).ToString();
			string name;
			if (Prefix == null)
				name = "a" + Extension + "&unique=" + unique;
			else
				name = Prefix + Extension + "&unique=" + unique;

			_MemoryNames.Add(name);
		}

		public string GetText()
		{
			_sw.Flush();
			StreamReader sr = null; 
			string t=null;
			try
			{
				_io.Position = 0;
				sr = new StreamReader(_io);
				t = sr.ReadToEnd();
			}
			finally
			{
				sr.Close();
			}
			return t;
		}

		public IList MemoryList
		{
			get {return _MemoryList;}
		}

		public IList MemoryNames
		{
			get {return _MemoryNames;}
		}

		#region IStreamGen Members
		public void CloseMainStream()
		{
		//	_io.Close();   // TODO  --- need to make this more robust; need multiple streams as well
			return;
		}

        public Stream GetStream()
        {
            return this._io;
        }

        public void SetStream(MemoryStream input)
        {
            this._io = input;
        }

		public TextWriter GetTextWriter()
		{
            if (_sw == null)
            {
                _sw = new StreamWriter(_io);
                _sw.AutoFlush = true;           // fix from jonh of forum to allow class to work with CSV
            }
			return _sw;
		}

		/// <summary>
		/// Createa a new stream and return a Stream caller can then write to.  
		/// </summary>
		/// <param name="relativeName">Filled in with a name</param>
		/// <param name="extension">????</param>
		/// <returns></returns>
		public Stream GetIOStream(out string relativeName, string extension)
		{
			MemoryStream ms = new MemoryStream();
			_MemoryList.Add(ms);
			string unique = Interlocked.Increment(ref MemoryStreamGen.Counter).ToString();

			if (Prefix == null)
				relativeName = "a" + extension + "&unique=" + unique;
			else
				relativeName = Prefix + extension + "&unique=" + unique;
			if (Suffix != null)
				relativeName += Suffix;

			_MemoryNames.Add(relativeName);

			return ms;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_sw != null)
			{
				_sw.Close();
				_sw = null;
			}
			if (_io != null)
			{
				_io.Close();
				_io = null;
			}
		}

		#endregion
	}
}
