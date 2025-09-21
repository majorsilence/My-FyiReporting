

using System;
using Majorsilence.Reporting.Rdl;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Majorsilence.Reporting.Rdl
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
