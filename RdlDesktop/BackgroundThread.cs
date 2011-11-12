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
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace fyiReporting.RdlDesktop
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
