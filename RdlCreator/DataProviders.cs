namespace Majorsilence.Reporting.RdlCreator
{
    public enum DataProviders
    {
        /// <summary>
        /// Microsoft.Data.SqlClient provider
        /// </summary>
        SqlServer_MicrosoftDataSqlClient = 1,
        Oracle = 2,
#if WINDOWS
        OleDb = 3,
#endif
        Odbc = 4,
        
        /// <summary>
        /// Majorsilence.Reporting.DataProviders XML provider
        /// </summary>
        Xml = 5,
        
        /// <summary>
        /// Majorsilence.Reporting.DataProviders Text provider
        /// </summary>
        Text = 6,
        MySql = 7,
        /// <summary>
        /// Npgsql provider
        /// </summary>
        PostgreSQL = 8,
        /// <summary>
        /// System.Data.SqlClient provider
        /// </summary>
        SqlServer_SystemData = 9,
        
        Firebird = 10,
        
        /// <summary>
        /// Microsoft.Data.Sqlite provider
        /// </summary>
        SQLite_MicrosoftData = 11,
        
        /// <summary>
        /// System.Data.SQLite provider
        /// </summary>
        SQLite_SystemData = 12,
        
        /// <summary>
        /// Majorsilence.Reporting.DataProviders Json provider
        /// </summary>
        Json = 13,
        
        /// <summary>
        /// Majorsilence.Reporting.DataProviders FileDirectory provider
        /// </summary>
        FileDirectory = 14,
        
        /// <summary>
        /// PostgreSQL Devart dotConnect provider
        /// </summary>
        PostgreSQL_Devart = 15
    }
}