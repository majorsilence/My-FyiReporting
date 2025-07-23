using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Majorsilence.Reporting.Data
{
    public class DictionaryDataReader : IDataReader
    {
        private readonly List<Dictionary<string, object>> _rows;
        private readonly List<string> _columns;
        private int _currentIndex = -1;

        public DictionaryDataReader(List<Dictionary<string, object>> rows)
        {
            _rows = rows;
            _columns = new HashSet<string>(rows.SelectMany(r => r.Keys)).ToList();
        }

        public bool Read() => ++_currentIndex < _rows.Count;
        public int FieldCount => _columns.Count;
        public string GetName(int i) => _columns[i];
        public int GetOrdinal(string name) => _columns.IndexOf(name);

        public object GetValue(int i)
        {
            if (_currentIndex < 0 || _currentIndex >= _rows.Count || i < 0 || i >= _columns.Count)
                return DBNull.Value;
            return _rows[_currentIndex].TryGetValue(_columns[i], out var val) ? val ?? DBNull.Value : DBNull.Value;
        }

        public int GetValues(object[] values)
        {
            int count = Math.Min(values.Length, FieldCount);
            for (int i = 0; i < count; i++)
            {
                values[i] = GetValue(i);
            }

            return count;
        }

        public bool IsDBNull(int i) => GetValue(i) == DBNull.Value;
        public object this[int i] => GetValue(i);
        public object this[string name] => GetValue(GetOrdinal(name));
        public Type GetFieldType(int i) => GetValue(i)?.GetType() ?? typeof(object);

        // Minimal additional methods
        public void Close() { }
        public void Dispose() { }
        public bool NextResult() => false;
        public int Depth => 0;
        public bool IsClosed => false;
        public int RecordsAffected => -1;

        // Not implemented for brevity
        public DataTable GetSchemaTable()
        {
            DataTable schemaTable = new DataTable();

            // Add standard schema columns
            schemaTable.Columns.Add("ColumnName", typeof(string));
            schemaTable.Columns.Add("ColumnOrdinal", typeof(int));
            schemaTable.Columns.Add("DataType", typeof(Type));
            schemaTable.Columns.Add("IsKey", typeof(bool));
            schemaTable.Columns.Add("IsUnique", typeof(bool));
            schemaTable.Columns.Add("IsAutoIncrement", typeof(bool));
            schemaTable.Columns.Add("AllowDBNull", typeof(bool));

            // Populate schema information for each column
            for (int i = 0; i < FieldCount; i++)
            {
                DataRow row = schemaTable.NewRow();
                row["ColumnName"] = GetName(i);
                row["ColumnOrdinal"] = i;
                row["DataType"] = GetFieldType(i);
                row["IsKey"] = false;
                row["IsUnique"] = false;
                row["IsAutoIncrement"] = false;
                row["AllowDBNull"] = true;

                schemaTable.Rows.Add(row);
            }

            return schemaTable;
        }

        public bool GetBoolean(int i) => (bool)GetValue(i);
        public byte GetByte(int i) => (byte)GetValue(i);

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            object value = GetValue(i);
            if (value is byte[] bytes)
            {
                int bytesToCopy = (int)Math.Min(length, bytes.Length - fieldOffset);
                if (bytesToCopy > 0 && buffer != null)
                {
                    Array.Copy(bytes, fieldOffset, buffer, bufferoffset, bytesToCopy);
                }

                return bytesToCopy;
            }

            return 0;
        }

        public char GetChar(int i) => (char)GetValue(i);

        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            object value = GetValue(i);
            if (value is string str)
            {
                // If value is a string, we can extract characters from it
                int charsToCopy = (int)Math.Min(length, str.Length - fieldOffset);
                if (charsToCopy > 0 && buffer != null)
                {
                    str.CopyTo((int)fieldOffset, buffer, bufferOffset, charsToCopy);
                }

                return charsToCopy;
            }
            else if (value is char[] chars)
            {
                // If value is already a char array, copy directly
                int charsToCopy = (int)Math.Min(length, chars.Length - fieldOffset);
                if (charsToCopy > 0 && buffer != null)
                {
                    Array.Copy(chars, fieldOffset, buffer, bufferOffset, charsToCopy);
                }

                return charsToCopy;
            }

            return 0;
        }

        public IDataReader GetData(int i)
        {
            object value = GetValue(i);

            // Handle collections that can be converted to a data reader
            if (value is IEnumerable<Dictionary<string, object>> dictList)
            {
                return new DictionaryDataReader(dictList.ToList());
            }
            else if (value is DataTable dataTable)
            {
                // Convert DataTable to list of dictionaries
                var rows = new List<Dictionary<string, object>>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        dict[col.ColumnName] = row[col];
                    }

                    rows.Add(dict);
                }

                return new DictionaryDataReader(rows);
            }

            // For other types that can't be easily converted to a data reader
            throw new InvalidCastException($"Cannot convert value at index {i} to IDataReader");
        }

        public string GetDataTypeName(int i) => GetFieldType(i).Name;
        public DateTime GetDateTime(int i) => (DateTime)GetValue(i);
        public decimal GetDecimal(int i) => (decimal)GetValue(i);
        public double GetDouble(int i) => (double)GetValue(i);
        public float GetFloat(int i) => (float)GetValue(i);
        public Guid GetGuid(int i) => (Guid)GetValue(i);
        public short GetInt16(int i) => (short)GetValue(i);
        public int GetInt32(int i) => (int)GetValue(i);
        public long GetInt64(int i) => (long)GetValue(i);
        public string GetString(int i) => GetValue(i)?.ToString();
    }
}