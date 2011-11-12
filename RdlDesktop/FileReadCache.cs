/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC

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
using System.Collections;
using System.IO;
using System.Text;

namespace fyiReporting.RdlDesktop
{
	class FileReadCache
	{
		Hashtable files;			// hashtable of file names and contents
		int maxFiles;				// maximum number of files allowed in cache
		int cachehits;
		int cacherequests;
		int maxSize;				// maximum size of a file allowed in cache

		public FileReadCache (int maxCount, int maxEntrySize)
		{
			maxFiles = maxCount;
			maxSize = maxEntrySize;
			files = new Hashtable();
			cachehits = 0;
			cacherequests = 0;
		}

		public byte[] Read(string file)
		{
			CacheReadEntry ce=null;
			lock (this)
			{
				cacherequests++;
				ce = (CacheReadEntry) files[file];
				if (ce == null)
				{	// entry isn't found; create new one
					if (files.Count >= maxFiles)	// Exceeded cache count?
						Reduce();					// yes, we need to reduce the file count

					FileStream fs = null;
					BinaryReader reader = null;
					byte[] bytes;
					try
					{
						fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
						reader = new BinaryReader(fs);
						bytes = new byte[fs.Length];
						int read;
						int totalRead=0;
						while((read = reader.Read(bytes, 0, bytes.Length)) != 0) 
						{
							totalRead += read;
						}
						ce = new CacheReadEntry(file, bytes);
						if (bytes.Length <= this.maxSize)
							files.Add(file, ce);		// don't actually cache if too big
					}
					catch (Exception e)
					{
						Console.WriteLine("File read error: {0} ", e );
						throw e;
					}
					finally
					{
						if (reader != null)
							reader.Close(); 
						if (fs != null)
							fs.Close();
					}
				}
				else
				{
					ce.Timestamp = DateTime.Now;
					cachehits++;
				}
			}
			if (ce != null)
				return ce.Value;
			else
				return null;
		}

		public string ReadString(string file)
		{
			byte[] bytes = this.Read(file);
		
			return Encoding.ASCII.GetString(bytes);
		}

		public int CacheHits
		{
			get { return  cachehits; }
			set {  cachehits = value; }
		}

		public int Count
		{
			get { return files.Count; }
		}

		// Clear out the files based on when they were last referenced.  Caller passes
		//  the timespan they want to retain.  Anything older gets tossed.   
		public int Clear(TimeSpan ts)
		{
			int numClearedFiles=0;
			lock (this)
			{
				DateTime ctime = DateTime.Now - ts;		// anything older than this is deleted
				// Build list of entries to be deleted
				ArrayList f = new ArrayList();
				foreach (CacheReadEntry ce in files.Values)
				{
					if (ce.Timestamp < ctime)
						f.Add(ce);
				}
				// Now delete them from the File hash
				foreach (CacheReadEntry ce in f)
				{
					files.Remove(ce.File);
				}
				numClearedFiles = f.Count;
			}
			return numClearedFiles;
		}

		// Clear out all the cached files.   
		public int ClearAll()
		{
			int numClearedFiles;
			lock (this)
			{
				// restart the cache
				numClearedFiles = files.Count;
				files = new Hashtable();
				cachehits=0;
				cacherequests=0;
			}
			return numClearedFiles;
		}

		// Reduce the number of entries in the list.  We're about to exceed our size.
		private int Reduce()
		{
			// Build list of entries to be deleted
			ArrayList f = new ArrayList(files.Values);
			f.Sort();		// comparer sorts by last reference time
			// Now delete them from the File hash
			int max = (int) (maxFiles / 4);
			foreach (CacheReadEntry ce in f)
			{
				files.Remove(ce.File);
				max--;
				if (max <= 0)
					break;
			}
			return max;
		}
	}

	class CacheReadEntry : IComparable
	{
		string _File;
		byte[] _Value;
		DateTime _Timestamp;
		DateTime _CreatedTime;			// time cache entry created

		public CacheReadEntry(string file, byte[] ba)
		{
			_File = file;
			_CreatedTime = _Timestamp = DateTime.Now;
			_Value = ba;
		}

		public string File
		{
			get { return  _File; }
		}

		public byte[] Value
		{
			get { return  _Value; }
		}

		public DateTime CreatedTime
		{
			get { return  _CreatedTime; }
		}

		public DateTime Timestamp
		{
			get { return  _Timestamp; }
			set {  _Timestamp = value; }
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			CacheReadEntry ce = obj as CacheReadEntry;

			long t = this.Timestamp.Ticks - ce.Timestamp.Ticks;
			if (t < 0)
				return -1;
			else if (t > 0)
				return 1;
			return 0;
		}

		#endregion
	}
}
