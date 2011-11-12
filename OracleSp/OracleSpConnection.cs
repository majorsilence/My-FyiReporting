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
using System.Xml;
using System.Data;
using Oracle.DataAccess.Client;

namespace fyiReporting.OracleSp
{
	/// <summary>
	/// LogConnection handles connections to web log.
	/// </summary>
	public class OracleSpConnection : IDbConnection
	{
        OracleConnection _OC;           // the Oracle connection

        public OracleSpConnection(string conn)
		{
            _OC = new OracleConnection(conn);
		}

        internal OracleConnection InternalConnection
        {
            get { return _OC; }
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
				return _OC.ConnectionString;
			}
			set
			{
                _OC.ConnectionString = value;
			}
		}

		public IDbCommand CreateCommand()
		{
			return new OracleSpCommand(this);
		}

		public void Open()
		{
            _OC.Open();
		}

		public void Close()
		{
			_OC.Close();
		}

		public string Database
		{
			get
			{
				return _OC.DataSource;			// don't really have a database
			}
		}

		public int ConnectionTimeout
		{
			get { return _OC.ConnectionTimeout;	}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			_OC.Dispose();
		}

		#endregion
	}
}
