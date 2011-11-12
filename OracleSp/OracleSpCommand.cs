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
using System.Collections;
using System.Collections.Generic;
using Oracle.DataAccess.Client;

namespace fyiReporting.OracleSp
{
	/// <summary>
	/// OracleSpCommand allows specifying the command for the web log.
	/// </summary>
	public class OracleSpCommand : IDbCommand
	{
		OracleSpConnection _oc;		// connection we're running under
        OracleCommand _cmd;
        DataParameterCollection _Parameters = new DataParameterCollection();

		public OracleSpCommand(OracleSpConnection conn)
		{
			_oc = conn;
            _cmd = _oc.InternalConnection.CreateCommand();
		}

		#region IDbCommand Members

		public void Cancel()
		{
            _cmd.Cancel();
		}

		public void Prepare()
		{
            _cmd.Prepare();
		}

		public System.Data.CommandType CommandType
		{
			get
			{
                return _cmd.CommandType;
			}
			set
            {
                _cmd.CommandType = value;
            }     
		}

		public IDataReader ExecuteReader(System.Data.CommandBehavior behavior)
		{
			if (!(behavior == CommandBehavior.SingleResult || 
				  behavior == CommandBehavior.SchemaOnly))
				throw new ArgumentException("ExecuteReader supports SingleResult and SchemaOnly only.");

            foreach (DataParameter dp in _Parameters)
            {
                OracleParameter op = _cmd.CreateParameter();
                op.ParameterName = dp.ParameterName;
                if (_cmd.CommandType == CommandType.StoredProcedure &&
                    dp.ParameterName.ToLower().Contains("cursor"))
                {
                    op.OracleDbType = OracleDbType.RefCursor;
                    op.Direction = ParameterDirection.Output;
                }
                else
                {
                    op.Value = dp.Value;
                }
                _cmd.Parameters.Add(op);
            }

            return _cmd.ExecuteReader(behavior);
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
			get { return _cmd.CommandTimeout; }
			set	{ _cmd.CommandTimeout = value;}
		}

		public IDbDataParameter CreateParameter()
		{
            DataParameter dp = new DataParameter();
            return dp;
		}

		public IDbConnection Connection
		{
			get
			{
				return this._oc;
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
			get	{	return _cmd.CommandText;	}
            set { _cmd.CommandText = value; }
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
            _cmd.Dispose();
		}

		#endregion
	}
}
