

using System;	
using System.Collections;
using System.IO;

namespace Majorsilence.Reporting.RdlDesktop
{
	class FileCache
	{
		Hashtable urls;
		int cachehits;
		int cacherequests;

		public FileCache ()
		{
			urls = new Hashtable();
			cachehits = 0;
			cacherequests = 0;
		}
		
		// Add an URL to the cache.   There are times where double work is performed.
		//   e.g. if we get multiple simultaneous requests for the same URL
		//    both might have gotten not found requests and proceeded to create
		//    new entries.  This is a waste as we end up only using one.   But this 
		//    shouldn't occur very often and the inefficiency should be slight and 
		//    temporary.
		//	 In this case, the passed files are deleted and the old ones are used
		public IList Add(string url, IList files)
		{
			CacheEntry ce=null;
			lock (this)
			{
				ce = (CacheEntry) urls[url];
				if (ce == null)
				{	// entry isn't found; create new one
					ce = new CacheEntry(url, files);
					urls.Add(url, ce);
				}
				else
				{	// We already had this entry; delete the new files
					Console.WriteLine("debug: Double execution of url {0}", url);
					ce.Timestamp = DateTime.Now;	// update the timestamp
					foreach (string file in files)
					{
						try 
						{
							File.Delete(file);
						}
						catch (Exception e)
						{
							Console.WriteLine("An Exception occurred while clearing file cache :" +e.ToString());
						}
					}
				}
			}
			return ce.Files;
		}

		public int CacheHits
		{
			get { return  cachehits; }
			set {  cachehits = value; }
		}

		public int Count
		{
			get { return urls.Count; }
		}

		public IList Find(string url)
		{
			return Find(url, DateTime.MinValue);
		}

		public IList Find(string url, DateTime lastUpdateTime)
		{
			CacheEntry ce = null;
			lock (this)
			{
				cacherequests++;
				ce = (CacheEntry) urls[url];
				if (ce != null)
				{
					if (lastUpdateTime > ce.CreatedTime)
					{	// the url has been updated since the cache entry was created
						DeleteUrlEntry(ce);		// remove from cache
						ce = null;				// tell caller we didn't find it
					}
					else
					{	// cache entry should still be valid
						ce.Timestamp = DateTime.Now;	// update the reference timestamp
						cachehits++;
					}
				}
			}
			if (ce == null)
				return null;
			else
				return ce.Files;
		}

		// Clear out the files based on the when they were last accessed.  Caller passes
		//  the timespan they want to retain.  Anything older gets tossed.   
		public int Clear(TimeSpan ts)
		{
			int numDeletedFiles=0;
			lock (this)
			{
				DateTime ctime = DateTime.Now - ts;		// anything older than this is deleted
				// Build list of entries to be deleted
				ArrayList f = new ArrayList();
				foreach (CacheEntry ce in urls.Values)
				{
					if (ce.Timestamp < ctime)
						f.Add(ce);
				}
				// Now delete them from the URL hash (and from the file system)
				foreach (CacheEntry ce in f)
				{
					numDeletedFiles += DeleteUrlEntry(ce);
				}
			}
			return numDeletedFiles;
		}

		private int DeleteUrlEntry(CacheEntry ce)
		{
			int numDeletedFiles=0;
			urls.Remove(ce.Url);
			foreach (string file in ce.Files)
			{
				try 
				{
					File.Delete(file);
					numDeletedFiles++;
				}
				catch (Exception e)
				{
					Console.WriteLine("An Exception occurred while clearing file cache :" +e.ToString());
				}
			}
			return numDeletedFiles;
		}
		// Clear out all the cached files.   
		public int ClearAll()
		{
			int numDeletedFiles=0;
			lock (this)
			{
				// Build list of entries to be deleted
				foreach (CacheEntry ce in urls.Values)
				{
					foreach (string file in ce.Files)
					{
						try 
						{
							File.Delete(file);
							numDeletedFiles++;
						}
						catch (Exception e)
						{
							Console.WriteLine("An Exception occurred while clearing file cache :" +e.ToString());
						}
					}
				}
				// restart the cache
				urls = new Hashtable();
				cachehits=0;
				cacherequests=0;
			}
			return numDeletedFiles;
		}
	}

	class CacheEntry
	{
		string _Url;					// URL of the report (including args)
		IList _Files;					// list of files associated with URL
		DateTime _Timestamp;			// time cache entry last referenced
		DateTime _CreatedTime;			// time cache entry created

		public CacheEntry(string url, IList files)
		{
			_Url = url;
			_Files = files;
			_CreatedTime = _Timestamp = DateTime.Now;
		}

		public string Url
		{
			get { return  _Url; }
		}

		public IList Files
		{
			get { return  _Files; }
		}

		public DateTime Timestamp
		{
			get { return  _Timestamp; }
			set {  _Timestamp = value; }
		}

		public DateTime CreatedTime
		{
			get { return  _CreatedTime; }
		}

	}
}
