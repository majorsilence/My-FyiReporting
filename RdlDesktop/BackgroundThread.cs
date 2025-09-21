

using System;	
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Majorsilence.Reporting.RdlDesktop
{
	class BackgroundThread
	{
		RdlDesktop rserver;					// server we're watching
		int cacheCheck;							// check the cache dates every X seconds
		int fileCacheTimeout;					// files last referenced more than X seconds will go
		int readCacheTimeout;					// read files last reference more than X seconds will go
		bool _continue=true;

		public BackgroundThread (RdlDesktop rws, int checkSeconds, int fileSeconds, int readSeconds)
		{
			rserver = rws;
			cacheCheck = checkSeconds;
			fileCacheTimeout = fileSeconds;
			readCacheTimeout = readSeconds;
		}

		public bool Continue
		{
			get { return _continue; }
			set { _continue = value; }
		}

		public void HandleBackground()
		{
			long count=0;
			while(_continue)
			{
				Thread.Sleep(1000);		// sleep for a second
				if (!_continue)			// before we do any work check to see 	
					break;
				count++;
				// Check on the caches
				if (count % cacheCheck == 0)	// every 5 minutes check on the caches
				{	// run as higher priority because server blocks on the caches
					Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;		// let's get our work done
					int citems, ritems;
					citems = rserver.Cache.Clear(new TimeSpan(0, 0, 0, fileCacheTimeout, 0));		// clear out file cache 
					ritems = rserver.ReadCache.Clear(new TimeSpan(0, 0, 0, readCacheTimeout, 0));	// clear out read cache 
					Thread.CurrentThread.Priority = ThreadPriority.Normal;
					//Console.WriteLine("Cache items cleared {0:#,##0}", citems);
					//Console.WriteLine("Read cache items cleared {0:#,##0}", ritems);
				}
			}
			return;
		}
	}
}
