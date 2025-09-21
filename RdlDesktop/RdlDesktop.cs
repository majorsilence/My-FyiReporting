

using System;	
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using System.Collections;
using System.Reflection;
using Majorsilence.Reporting.RdlDesktop.Resources;


namespace Majorsilence.Reporting.RdlDesktop
{
	class RdlDesktop
	{
		private TcpListener myListener ;
		// These parameters are usually override in the config.xml file 
		private int port = 8080 ;			// port
		private bool bLocalOnly = true;		// restrict access to the machine it's running on
		private string wd = "tempreports";	// subdirectory name under server root to place generated reports
		private string sr = null;
		private int maxReadCache=100;
		private int maxReadCacheEntrySize=50000;
		private int _TraceLevel=0;			// Controls level of messages to console

		private bool _continue=true;
		private FileCache _cache;
		private FileReadCache _readCache;
		private Hashtable _mimes;
		private string _dsrPassword;		// data source resource password

		public RdlDesktop()
		{
			GetConfigInfo();				// obtain the configuation information

			_cache = new FileCache();
			_readCache = new FileReadCache(maxReadCache, maxReadCacheEntrySize);	
		}
		
		public FileCache Cache
		{
			get { return _cache; }
		}

		public string DataSourceReferencePassword
		{
			get { return _dsrPassword; }
			set { _dsrPassword = value; }
		}

		public FileReadCache ReadCache
		{
			get { return _readCache; }
		}
	
		public Hashtable Mimes
		{
			get { return _mimes; }
		}

		public bool Continue
		{
			get { return _continue; }
			set { _continue = value; }
		}
		
		public int TraceLevel
		{
			get { return _TraceLevel; }
			set { _TraceLevel = value; }
		}

		// RunServer makes TcpListener start listening on the
		// given port.  It also calls a Thread on the method StartListen(). 
		public void RunServer()
		{
			// main server loop
			try
			{
                //start listing on the given port
                if (this.bLocalOnly)
                {
                    IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
                    myListener = new TcpListener(ipAddress, port);
                }
                else
                {
                    IPAddress ipAddress = Dns.GetHostEntry("0.0.0.0").AddressList[0];
                    myListener = new TcpListener(ipAddress, port);
                }
				myListener.Start();
//				int maxThreads;				// worker threads in the thread pool
//				int completionPortThreads;	// asynchronous I/O threads in the thread pool
//				ThreadPool.GetMaxThreads(out maxThreads, out completionPortThreads);
				
				Console.WriteLine(string.Format("RDL Desktop version {0}, Copyright (C) 2005 fyiReporting Software, LLC",
							Assembly.GetExecutingAssembly().GetName().Version.ToString()));

				Console.WriteLine("");
				Console.WriteLine("RDL Desktop comes with ABSOLUTELY NO WARRANTY.  This is free software,");
				Console.WriteLine("and you are welcome to redistribute it under certain conditions.");
				Console.WriteLine("Type 'license' for details.");
				Console.WriteLine("");
				Console.WriteLine("RDL Desktop running on port {0}", port);
				Console.WriteLine("Type '?' for list of console commands.");
				Console.Write(">");
				while(_continue)
				{
					while(!myListener.Pending() && _continue)
					{
						Thread.Sleep(100);
					}
					if (_continue)
					{
						ConnectionThread c = new ConnectionThread(this, myListener, sr, wd);
                        ThreadPool.QueueUserWorkItem(async _ => await c.HandleConnection());
                    }
				}
			}
			catch(Exception e)
			{
				if (this._TraceLevel >= 0) 
					Console.WriteLine("An Exception Occurred while Listening :" +e.ToString());
			}
		}

		// Application Starts Here..
		public static void Main(string[] args) 
		{
			RdlDesktop server = new RdlDesktop();
			server.HandleArguments(args);

			// start up the background thread to handle additional work
			BackgroundThread bk = new BackgroundThread(server, 60 * 5, 60 * 5, 60 * 5);
			Thread bThread = new Thread(new ThreadStart(bk.HandleBackground));
			bThread.Name = "background";
			bThread.Start();
			
			// start up the console thread so user can see what's going on
			ConsoleThread ct = new ConsoleThread(server, bk);
			Thread cThread = new Thread(new ThreadStart(ct.HandleConsole));
			cThread.Name = "console";
			cThread.Start();

			// Run the server
			server.RunServer();
			server.Cache.ClearAll();			// clean up the report file cache when done
		}

		private void GetConfigInfo()
		{
			_mimes = new Hashtable();
			string optFileName = Path.Combine(Majorsilence.Reporting.Rdl.Utility.Paths.MajorsilenceRoamingFolder(), "config.xml");
			if (!File.Exists(optFileName))
			{
				if (File.Exists("config.xml"))
				{
					File.Copy("config.xml", optFileName);
				}
			}

			try
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.PreserveWhitespace = false;
				xDoc.Load(optFileName);
				XmlNode xNode;
				xNode = xDoc.SelectSingleNode("//config");

				// Loop thru all the child nodes
				foreach(XmlNode xNodeLoop in xNode.ChildNodes)
				{
					if (xNodeLoop.NodeType != XmlNodeType.Element)
						continue;
					switch (xNodeLoop.Name.ToLower())
					{
						case "port":
							try
							{
								port = Convert.ToInt32(xNodeLoop.InnerText);
							}
							catch
							{
								port = 8080;
								Console.WriteLine("config.xml file: port value is not a valid number.  Defaulting to {0}", port);
							}
							break;
						case "localhostonly":
							string tf = xNodeLoop.InnerText.ToLower();
							if (tf == "false")
								this.bLocalOnly = false;
							break;
						case "serverroot":
							sr = xNodeLoop.InnerText;
							break;
						case "cachedirectory":
							wd = xNodeLoop.InnerText;
							break;
						case "tracelevel":
							try
							{
								_TraceLevel = Convert.ToInt32(xNodeLoop.InnerText);
							}
							catch
							{
								_TraceLevel = 0;
								Console.WriteLine("config.xml file: tracelevel value is not a valid number.  Defaulting to {0}", _TraceLevel);
							}
							break;
						case "maxreadcache":
							try
							{
								maxReadCache = Convert.ToInt32(xNodeLoop.InnerText);
							}
							catch
							{
								maxReadCache = 100;
								Console.WriteLine("config.xml file: maxreadcache value is not a valid number.  Defaulting to {0}", maxReadCache);
							}
							break;
						case "maxreadcacheentrysize":
							try
							{
								maxReadCacheEntrySize = Convert.ToInt32(xNodeLoop.InnerText);
							}
							catch
							{
								maxReadCacheEntrySize = 50000;
								Console.WriteLine("config.xml file: maxReadCacheEntrySize value is not a valid number.  Defaulting to {0}", maxReadCacheEntrySize);
							}
							break;
						case "mimetypes":
							GetConfigInfoMimes(xNodeLoop);
							break;
						default:
							Console.WriteLine("config.xml file: {0} is unknown and will be ignored.", xNodeLoop.Name);
							break;
					}
				}
				if (sr == null)			// no server root specified?
					throw new Exception(Strings.RdlDesktop_Error_ServerrootMustSpecified);
			}
			catch (Exception ex)
			{		// Didn't sucessfully get the startup state don't use
				Console.WriteLine("Error processing config.xml. {0}", ex.Message);
				throw;
			}

			return;
		}

		private void HandleArguments(string[] args)
		{
			// Handle command line arguments
			if (args == null || args.Length==0)
				return;

			foreach(string s in args)
			{
				string t = s.Substring(0,2);
				switch (t)
				{
					case "/p":
						this._dsrPassword = s.Substring(2);
						break;
					case "/t":
						try
						{
							_TraceLevel = Convert.ToInt32(s.Substring(2));
						}
						catch
						{
							Console.WriteLine("/t value is not a valid number.  Using {0}", _TraceLevel);
						}
						break;
					default:
						Console.WriteLine("Unknown command line argument '{0}' ignored.", s);
						break;
				}
			}
		}

		private void GetConfigInfoMimes(XmlNode xNode)
		{
			foreach(XmlNode xN in xNode.ChildNodes)
			{
				if (xN.NodeType != XmlNodeType.Element)
					continue;
				switch (xN.Name.ToLower())
				{
					case "mimetype":
						string extension=null;
						string type=null;
						foreach(XmlNode xN2 in xN.ChildNodes)
						{
							switch (xN2.Name.ToLower())
							{
								case "extension":
									extension = xN2.InnerText.ToLower();
									break;
								case "type":
									type = xN2.InnerText.ToLower();
									break;
								default:
									Console.WriteLine("config.xml file: {0} is unknown and will be ignored.", xN.Name);
									break;
							}
						}
						if (extension != null && type != null)
						{
							if (_mimes[extension] == null)
								_mimes.Add(extension, type);
						}
						break;
					default:
						Console.WriteLine("config.xml file: {0} is unknown and will be ignored.", xN.Name);
						break;
				}
			}
			return;
		}
	}
}
