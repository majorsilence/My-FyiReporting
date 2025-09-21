

using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.IO;

namespace Majorsilence.Reporting.Rdl
{
	///<summary>
	/// Information about the data source (e.g. a database connection string).
	///</summary>
	[Serializable]
	public class DataSource
	{
		Report _rpt;	// Runtime report
		DataSourceDefn _dsd;	// DataSource definition

		internal DataSource(Report rpt, DataSourceDefn dsd)
		{
			_rpt = rpt;
			_dsd = dsd;
		}

		/// <summary>
		/// Get/Set the database connection.  User must handle closing of connection.
		/// </summary>
		public IDbConnection UserConnection
		{
			get {return _dsd.IsUserConnection(_rpt)? _dsd.GetConnection(_rpt): null;}	// never reveal connection internally connected
			set
			{
				_dsd.CleanUp(_rpt);					// clean up prior connection if necessary

				_dsd.SetUserConnection(_rpt, value);
			}
		}
	}
}
