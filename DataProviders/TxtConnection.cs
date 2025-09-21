
using System;
using System.Xml;
using System.Data;

namespace Majorsilence.Reporting.Data
{
	/// <summary>
	/// LogConnection handles connections to web log.
	/// </summary>
	public class TxtConnection : IDbConnection
	{
		string _Connection;				// the connection string; of format file=
		bool bOpen=false;
		public TxtConnection(string conn)
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
				string directory=null;
				foreach(string arg in args)
				{
					if (arg.Trim().ToLower().StartsWith("directory="))	// Only have one type of argument right now
						directory = arg.Trim().Split('=').GetValue(1) as string;
				}
				_Connection = value;
			}
		}

		public IDbCommand CreateCommand()
		{
			return new TxtCommand(this);
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
