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
	/// FileDirCommand allows specifying the command for the web log.
	/// </summary>
	public class FileDirCommand : IDbCommand
	{
		FileDirConnection _fdc;		// connection we're running under
		string _cmd;				// command to execute
		// parsed constituents of the command
		string _Directory;			// Directory
		string _FilePattern;		// SearchPattern when doing the file lookup
		string _DirectoryPattern;	// SearchPattern when doing the directory lookup
		string _TrimEmpty="yes";	// Directory with no files will be omitted from result set
		DataParameterCollection _Parameters = new DataParameterCollection();

		public FileDirCommand(FileDirConnection conn)
		{
			_fdc = conn;  
		}

		internal string Directory
		{
			get 
			{
				// Check to see if "Directory" or "@Directory" is a parameter
				IDbDataParameter dp= _Parameters["Directory"] as IDbDataParameter;
				if (dp == null)
					dp= _Parameters["@Directory"] as IDbDataParameter;
				// Then check to see if the Directory value is a parameter?
				if (dp == null)
					dp = _Parameters[_Directory] as IDbDataParameter;
				if (dp != null)
					return dp.Value != null? dp.Value.ToString(): _Directory;	// don't pass null; pass existing value
				return _Directory == null? _fdc.Directory: _Directory;
			}
			set {_Directory = value;}
		}

		internal string FilePattern
		{
			get 
			{
				// Check to see if "FilePattern" or "@FilePattern" is a parameter
				IDbDataParameter dp= _Parameters["FilePattern"] as IDbDataParameter;
				if (dp == null)
					dp= _Parameters["@FilePattern"] as IDbDataParameter;
				// Then check to see if the FilePattern value is a parameter?
				if (dp == null)
					dp = _Parameters[_FilePattern] as IDbDataParameter;
				if (dp != null)
					return dp.Value as string;
				return _FilePattern;
			}
			set {_FilePattern = value;}
		}

		internal string DirectoryPattern
		{
			get 
			{
				// Check to see if "DirectoryPattern" or "@DirectoryPattern" is a parameter
				IDbDataParameter dp= _Parameters["DirectoryPattern"] as IDbDataParameter;
				if (dp == null)
					dp= _Parameters["@DirectoryPattern"] as IDbDataParameter;
				// Then check to see if the DirectoryPattern value is a parameter?
				if (dp == null)
					dp = _Parameters[_DirectoryPattern] as IDbDataParameter;
				if (dp != null)
					return dp.Value as string;
				return _DirectoryPattern;
			}
			set {_DirectoryPattern = value;}
		}

		internal bool TrimEmpty
		{
			get 
			{
				// Check to see if "TrimEmpty" or "@TrimEmpty" is a parameter
				IDbDataParameter dp= _Parameters["TrimEmpty"] as IDbDataParameter;
				if (dp == null)
					dp= _Parameters["@TrimEmpty"] as IDbDataParameter;
				// Then check to see if the TrimEmpty value is a parameter?
				if (dp == null)
					dp = _Parameters[_TrimEmpty] as IDbDataParameter;
				if (dp != null)
				{
					string tf = dp.Value as string;
					if (tf == null)
						return false;
					tf = tf.ToLower();
					return (tf == "true" || tf == "yes");
				}
				return _TrimEmpty=="yes"? true: false;	// the value must be a constant
			}
			set {_TrimEmpty = value? "yes": "no";}
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
			return new FileDirDataReader(behavior, _fdc, this);
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
			return new FileDirDataParameter();
		}

		public IDbConnection Connection
		{
			get
			{
				return this._fdc;
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
				_FilePattern = null;
				_DirectoryPattern = null;
				_Directory = null;
				string[] args = value.Split(';');
				foreach(string arg in args)
				{
					string[] param = arg.Trim().Split('=');
					if (param == null || param.Length != 2)
						continue;
					string key = param[0].Trim().ToLower();
					string val = param[1];
					switch (key)
					{
						case "directory":
							_Directory = val;
							break;
						case "filepattern":
							_FilePattern = val;
							break;
						case "directorypattern":
							_DirectoryPattern = val;
							break;
						default:
							throw new ArgumentException(string.Format("{0} is an unknown parameter key", param[0]));
					}
				}
				// User must specify both the url and the RowsXPath
				if (_Directory == null && this._fdc.Directory == null)
				{
					if (_Directory == null)
						throw new ArgumentException("CommandText requires a 'Directory=' parameter.");
				}
				_cmd = value;
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
