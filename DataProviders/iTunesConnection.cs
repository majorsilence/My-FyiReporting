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
using System.Xml;
using System.Data;
using System.IO;

namespace fyiReporting.Data
{
	/// <summary>
	/// iTunesConnection handles connections for the iTunes XML file
	/// </summary>
	public class iTunesConnection : IDbConnection
	{
		string _Connection;				// the connection string; of format directory=
        string _File;
		bool bOpen=false;
        public iTunesConnection(string conn)
		{
			ConnectionString = conn;
		}

		internal bool IsOpen
		{
			get {return bOpen;}
		}

		#region IDbConnection Members

		public void ChangeDatabase(string databaseName)
		{
			throw new NotImplementedException("ChangeDatabase method not supported.");
		}

		public IDbTransaction BeginTransaction(System.Data.IsolationLevel il)
		{
			throw new NotImplementedException("BeginTransaction method not supported.");
		}

		IDbTransaction System.Data.IDbConnection.BeginTransaction()
		{
			throw new NotImplementedException("BeginTransaction method not supported.");
		}

		public System.Data.ConnectionState State
		{
			get
			{
				throw new NotImplementedException("State not implemented");
			}
		}

		public string ConnectionString
		{
			get
			{
				return _Connection;
			}
			set
			{
				string c = value;
				// Now parse the connection string;
				Array args = c.Split (',');
				_File=null;
				foreach(string arg in args)
				{
					if (arg.Trim().ToLower().StartsWith("file="))	// Only have one type of argument right now
						_File = arg.Trim().Split('=').GetValue(1) as string;
				}
				_Connection = value;
			}
		}

        public string File
        {
            get 
            { 
                if (_File != null)
                    return _File;

                // Search for the iTunes file; In My Documents/My Music/iTunes/iTunes Music Library.xml
                string f = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) +
                            Path.DirectorySeparatorChar + "iTunes" +
                            Path.DirectorySeparatorChar + "iTunes Music Library.xml";
                return f;
            }
        }

		public IDbCommand CreateCommand()
		{
			return new iTunesCommand(this);
		}

		public void Open()
		{
			bOpen = true;
		}

		public void Close()
		{
			bOpen = false;
		}

		public string Database
		{
			get
			{
				return null;			// don't really have a database
			}
		}

		public int ConnectionTimeout
		{
			get
			{
				return 0;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
