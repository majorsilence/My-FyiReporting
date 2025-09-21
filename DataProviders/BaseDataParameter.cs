
using System;
using System.Xml;
using System.Data;

namespace Majorsilence.Reporting.Data
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
