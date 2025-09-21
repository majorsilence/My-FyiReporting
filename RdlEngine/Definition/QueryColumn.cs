

using System;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// When Query is database SQL; QueryColumn represents actual database column.
	///</summary>
	[Serializable]
	internal class QueryColumn
	{
		internal int colNum;			// Column # in query select
		internal string colName;		// Column name in query
		internal TypeCode _colType;	// TypeCode in query

		internal QueryColumn(int colnum, string name, TypeCode c)
		{
			colNum = colnum;
            colName = name.TrimEnd('\0');
			_colType = c;
		}

		internal TypeCode colType
		{
			// Treat Char as String for queries: <sigh> drivers sometimes confuse char and string types
			//    telling me a type is char but actually returning a string (Mono work around)
			get {return _colType == TypeCode.Char? TypeCode.String: _colType; }
		}
	}
}
