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

namespace Majorsilence.Reporting.Data
{
	/// <summary>
	/// TxtCommand allows specifying the command for a text file.
	/// </summary>
	public class JsonCommand : IDbCommand
	{
		JsonConnection _tc;			// connection we're running under
		string _cmd;				// command to execute
		// parsed constituents of the command
		string[] _Columns;			// hold the column list
		DataParameterCollection _Parameters = new DataParameterCollection();

		public JsonCommand(JsonConnection conn)
		{
			_tc = conn;
        }

		internal string[] Columns
		{
			get {return _Columns;}
		}

		internal string Url
		{
			get 
			{
				// Check to see if "Url" or "@Url" is a parameter
				IDbDataParameter dp= _Parameters["Url"] as IDbDataParameter;
				if (dp == null)
					dp= _Parameters["@Url"] as IDbDataParameter;
				// Then check to see if the Url value is a parameter?
				if (dp == null)
					dp = _Parameters[_tc.Url] as IDbDataParameter;
				if (dp != null)
					return dp.Value != null? dp.Value.ToString(): _tc.Url;	// don't pass null; pass existing value
				return _tc.Url;	// the value must be a constant
			}
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
			}
		}

		public IDataReader ExecuteReader(System.Data.CommandBehavior behavior)
		{
			if (!(behavior == CommandBehavior.SingleResult || 
				  behavior == CommandBehavior.SchemaOnly))
				throw new ArgumentException("ExecuteReader supports SingleResult and SchemaOnly only.");
			return new JsonDataReader(behavior, _tc, this);
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
			return new TxtDataParameter();
		}

		public IDbConnection Connection
		{
			get
			{
				return this._tc;
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
				string[] columns = null;
				foreach(string arg in args)
				{
					string[] param = arg.Trim().Split('=');
					if (param == null || param.Length != 2)
						continue;
					string key = param[0].Trim().ToLower();
					string val = param[1];
					switch (key)
					{
						case "columns":
							// column list is separated by ','
							columns = val.Trim().Split(',');
							break;
						default:
							throw new ArgumentException(string.Format("{0} is an unknown parameter key", param[0]));
					}
				}

				_cmd = value;
				_Columns = columns;
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
