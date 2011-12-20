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
using System.Collections;

namespace fyiReporting.Data
{
	/// <summary>
	/// iTunesCommand represents the command.  The user is allowed to specify the table needed.
	/// </summary>
	public class iTunesCommand : IDbCommand
	{
		iTunesConnection _xc;			// connection we're running under
		string _cmd;				// command to execute
		// parsed constituents of the command
		string _Table;				// table requested
		DataParameterCollection _Parameters = new DataParameterCollection();

        public iTunesCommand(iTunesConnection conn)
		{
			_xc = conn;
		}

		internal string Table
		{
			get 
			{
				IDbDataParameter dp= _Parameters["Table"] as IDbDataParameter;
				if (dp == null)
					dp= _Parameters["@Table"] as IDbDataParameter;
				// Then check to see if the Table value is a parameter?
				if (dp == null)
					dp = _Parameters[_Table] as IDbDataParameter;
				if (dp != null)
					return dp.Value != null? dp.Value.ToString(): _Table;	// don't pass null; pass existing value
				return _Table;	// the value must be a constant
			}
			set {_Table = value;}
		}


		#region IDbCommand Members

		public void Cancel()
		{
			throw new NotImplementedException("Cancel not implemented");
		}

		public void Prepare()
		{
			return;			// Prepare is a noop
		}

		public System.Data.CommandType CommandType
		{
			get
			{
				throw new NotImplementedException("CommandType not implemented");
			}
			set
			{
				throw new NotImplementedException("CommandType not implemented");
			}
		}

		public IDataReader ExecuteReader(System.Data.CommandBehavior behavior)
		{
			if (!(behavior == CommandBehavior.SingleResult || 
				  behavior == CommandBehavior.SchemaOnly))
				throw new ArgumentException("ExecuteReader supports SingleResult and SchemaOnly only.");
			return new iTunesDataReader(behavior, _xc, this);
		}

		IDataReader System.Data.IDbCommand.ExecuteReader()
		{
			return ExecuteReader(System.Data.CommandBehavior.SingleResult);
		}

		public object ExecuteScalar()
		{
			throw new NotImplementedException("ExecuteScalar not implemented");
		}

		public int ExecuteNonQuery()
		{
			throw new NotImplementedException("ExecuteNonQuery not implemented");
		}

		public int CommandTimeout
		{
			get
			{
				return 0;
			}
			set
			{
				throw new NotImplementedException("CommandTimeout not implemented");
			}
		}

		public IDbDataParameter CreateParameter()
		{
			return new XmlDataParameter();
		}

		public IDbConnection Connection
		{
			get
			{
				return this._xc;
			}
			set
			{
				throw new NotImplementedException("Setting Connection not implemented");
			}
		}

		public System.Data.UpdateRowSource UpdatedRowSource
		{
			get
			{
				throw new NotImplementedException("UpdatedRowSource not implemented");
			}
			set
			{
				throw new NotImplementedException("UpdatedRowSource not implemented");
			}
		}

		public string CommandText
		{
			get
			{
				return this._cmd;
			}
			set
			{
				// Parse the command string for keyword value pairs separated by ';'
				string[] args = value.Split(';');
				string table=null;
				foreach(string arg in args)
				{
					string[] param = arg.Trim().Split('=');
					if (param == null || param.Length != 2)
						continue;
					string key = param[0].Trim().ToLower();
					string val = param[1];
					switch (key)
					{
						case "table":
							table = val;
							break;
						default:
							throw new ArgumentException(string.Format("{0} is an unknown parameter key", param[0]));
					}
				}
				// User must specify both the url and the RowsXPath
                if (table == null)
                    table = "Tracks";
				_cmd = value;
				_Table = table;
			}
		}

		public IDataParameterCollection Parameters
		{
			get
			{
				return _Parameters;
			}
		}

		public IDbTransaction Transaction
		{
			get
			{
				throw new NotImplementedException("Transaction not implemented");
			}
			set
			{
				throw new NotImplementedException("Transaction not implemented");
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			// nothing to dispose of
		}

		#endregion
	}
}
