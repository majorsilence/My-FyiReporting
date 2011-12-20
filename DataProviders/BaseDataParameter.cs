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

namespace fyiReporting.Data
{
	/// <summary>
	/// BaseDataParameter is the base class for handling parameters
	/// </summary>
	public class BaseDataParameter : IDbDataParameter
	{
		string _Name;			// parameter name
		string _Value;			// parameter value

		public BaseDataParameter()
		{
		}


		#region IDbDataParameter Members

		public byte Precision
		{
			get
			{
				return 0;
			}
			set
			{
				throw new NotImplementedException("Precision setting is not implemented");
			}
		}

		public byte Scale
		{
			get
			{
				return 0;
			}
			set
			{
				throw new NotImplementedException("Scale setting is not implemented");
			}
		}

		public int Size
		{
			get
			{
				return 0;
			}
			set
			{
				throw new NotImplementedException("Size setting is not implemented");
			}
		}

		#endregion

		#region IDataParameter Members

		public System.Data.ParameterDirection Direction
		{
			get
			{
				return System.Data.ParameterDirection.Input;	// only support input parameter
			}
			set
			{
				if (value != ParameterDirection.Input)
					throw new Exception("Parameter Direction must be Input");
			}
		}

		public System.Data.DbType DbType
		{
			get
			{			  
				return DbType.String;
			}
			set
			{
				if (value != DbType.String)
					throw new Exception("DbType must always be String");
			}
		}

		public object Value
		{
			get {return _Value;}
			set {_Value = value != null? value.ToString(): null;}
		}

		public bool IsNullable
		{
			get
			{
				return false;
			}
		}

		public System.Data.DataRowVersion SourceVersion
		{
			get
			{									   
				return DataRowVersion.Current;
			}
			set
			{
				throw new NotImplementedException("Setting DataRowVersion is not implemented.");
			}
		}

		public string ParameterName
		{
			get {return _Name;}
			set {_Name = value;}
		}

		public string SourceColumn
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotImplementedException("Setting SourceColumn is not implemented.");
			}
		}

		#endregion
	}
}
