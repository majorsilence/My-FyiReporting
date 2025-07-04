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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Majorsilence.Reporting.Data
{
    /// <summary>
    /// TxtDataReader handles reading txt files
    /// </summary>
    public class JsonDataReader : IDataReader
    {
        JsonConnection _tconn;
        JsonCommand _tcmd;
        System.Data.CommandBehavior _behavior;

        StreamReader _sr;           // StreamReader for _Url
                                    // column information
        private List<Dictionary<string, object>> _rows;
        private int _currentIndex = -1;
        string[] _Names;            // names of the columns
        Type[] _Types;              // types of the columns

        public JsonDataReader(System.Data.CommandBehavior behavior, JsonConnection conn, JsonCommand cmd)
        {
            _behavior = behavior;
            _tcmd = cmd ?? throw new ArgumentNullException(nameof(cmd), "Command cannot be null.");
            _tconn = conn ?? throw new ArgumentNullException(nameof(conn), "Connection cannot be null.");
            Initialize();
        }

        private async Task<string> ReadAllJsonAsync()
        {
            using var sr = await GetStream();
            return await sr.ReadToEndAsync();
        }

        private List<Dictionary<string, object>> ParseJsonRows(string json)
        {
            // Assumes the JSON is an array of objects
            var rows = new List<Dictionary<string, object>>();
            var jsonArray = JsonNode.Parse(json)?.AsArray();
            if (jsonArray != null)
            {
                foreach (var item in jsonArray)
                {
                    var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    foreach (var prop in item.AsObject())
                    {
                        dict[prop.Key] = prop.Value?.GetValue<object>();
                    }
                    rows.Add(dict);
                }
            }
            return rows;
        }

        async Task<StreamReader> GetStream()
        {
            string fname = _tcmd.Url;
            Stream strm = null;

            if (fname.StartsWith("http:") || fname.StartsWith("https:"))
            {
                _tconn.Client.AddMajorsilenceReportingUserAgent();
                // set auth if found in connection string
                // via a request object

                var request = new HttpRequestMessage(HttpMethod.Get, fname);

                if (!string.IsNullOrWhiteSpace(_tconn.Auth))
                {

                    var authParts = _tconn.Auth.Split(':');
                    string authScheme = authParts[0];
                    string authParameters = authParts[1];
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authScheme, authParameters);
                }

                HttpResponseMessage response = await _tconn.Client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                strm = await response.Content.ReadAsStreamAsync();
            }
            else if (fname.StartsWith("file:"))
            {
                strm = new FileStream(fname.Substring(5), System.IO.FileMode.Open, FileAccess.Read);
            }
            else
            {
                strm = new FileStream(fname, System.IO.FileMode.Open, FileAccess.Read);
            }

            _sr = new StreamReader(strm);

            return _sr;
        }

        public object this[int i] => _rows[_currentIndex][_Names[i]];

        public object this[string name] => _rows[_currentIndex][name] ?? throw new IndexOutOfRangeException($"Column '{name}' does not exist.");

        public int Depth => 0;

        public bool IsClosed => _sr == null || _sr.EndOfStream;

        public int RecordsAffected => _rows.Count;

        public int FieldCount => _Names.Length;

        public void Close()
        {
            _sr?.Close();
            _sr = null;
            _currentIndex = -1;
        }

        public void Dispose()
        {
            Close();
            _sr?.Dispose();
            _sr = null;
            _rows = null;
            _Names = null;
            _Types = null;
        }

        public bool GetBoolean(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is bool value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Boolean.");
        }

        public byte GetByte(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is byte value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Byte.");
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            if (_rows[_currentIndex][_Names[i]] is byte[] bytes)
            {
                if (fieldOffset < 0 || fieldOffset >= bytes.Length)
                    throw new ArgumentOutOfRangeException(nameof(fieldOffset), "Field offset is out of range.");
                int bytesToCopy = Math.Min(length, bytes.Length - (int)fieldOffset);
                Array.Copy(bytes, fieldOffset, buffer, bufferoffset, bytesToCopy);
                return bytesToCopy;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Byte array.");
        }

        public char GetChar(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is char value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Char.");
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            if (_rows[_currentIndex][_Names[i]] is string strValue)
            {
                if (fieldoffset < 0 || fieldoffset >= strValue.Length)
                    throw new ArgumentOutOfRangeException(nameof(fieldoffset), "Field offset is out of range.");
                int charsToCopy = Math.Min(length, strValue.Length - (int)fieldoffset);
                strValue.CopyTo((int)fieldoffset, buffer, bufferoffset, charsToCopy);
                return charsToCopy;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Char array.");
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException("GetData is not implemented for JsonDataReader.");
        }

        public string GetDataTypeName(int i)
        {
            if (i < 0 || i >= _Names.Length)
                throw new IndexOutOfRangeException($"Index {i} is out of range for field names.");
            var type = _Types[i] ?? typeof(object);
            return type.Name;
        }

        public DateTime GetDateTime(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is DateTime value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to DateTime.");
        }

        public decimal GetDecimal(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is decimal value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Decimal.");
        }

        public double GetDouble(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is double value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Double.");
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
        public Type GetFieldType(int i)
        {
            return _Types[i] ?? typeof(object);
        }

        public float GetFloat(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is float value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Float.");
        }

        public Guid GetGuid(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is Guid value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Guid.");
        }

        public short GetInt16(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is short value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Int16.");
        }

        public int GetInt32(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is int value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Int32.");
        }

        public long GetInt64(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is long value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to Int64.");
        }

        public string GetName(int i)
        {
            if (i < 0 || i >= _Names.Length)
                throw new IndexOutOfRangeException($"Index {i} is out of range for field names.");
            return _Names[i];
        }

        public int GetOrdinal(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Column name cannot be null or empty.");
            int index = Array.IndexOf(_Names, name);
            if (index < 0)
                throw new IndexOutOfRangeException($"Column '{name}' does not exist.");
            return index;
        }

        public DataTable GetSchemaTable()
        {
            Initialize();

            var schemaTable = new DataTable("SchemaTable");
            schemaTable.Columns.Add("ColumnName", typeof(string));
            schemaTable.Columns.Add("DataType", typeof(Type));
            schemaTable.Columns.Add("AllowDBNull", typeof(bool));
            schemaTable.Columns.Add("IsUnique", typeof(bool));
            schemaTable.Columns.Add("IsKey", typeof(bool));
            schemaTable.Columns.Add("BaseSchemaName", typeof(string));
            foreach (var name in _Names)
            {
                var row = schemaTable.NewRow();
                row["ColumnName"] = name;
                row["DataType"] = _Types[Array.IndexOf(_Names, name)];
                row["AllowDBNull"] = true; // Assuming all columns allow nulls
                row["IsUnique"] = false; // Assuming no uniqueness constraint
                row["IsKey"] = false; // Assuming no key constraint
                row["BaseSchemaName"] = string.Empty; // No base schema name
                schemaTable.Rows.Add(row);
            }
            return schemaTable;
        }

        public string GetString(int i)
        {
            if (_rows[_currentIndex][_Names[i]] is string value)
            {
                return value;
            }
            throw new InvalidCastException($"Cannot convert column '{_Names[i]}' to String.");
        }

        public object GetValue(int i)
        {
            var row = _rows[_currentIndex];
            return row[_Names[i]];
        }

        public int GetValues(object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), "Values array cannot be null.");
            if (values.Length < _Names.Length)
                throw new ArgumentException("Values array is too small to hold all column values.");
            for (int i = 0; i < _Names.Length; i++)
            {
                values[i] = _rows[_currentIndex][_Names[i]];
            }
            return _Names.Length;
        }

        public bool IsDBNull(int i)
        {
            if (i < 0 || i >= _Names.Length)
                throw new IndexOutOfRangeException($"Index {i} is out of range for field names.");
            return _rows[_currentIndex][_Names[i]] == null;
        }

        public bool NextResult()
        {
            Initialize();

            if (_currentIndex >= _rows.Count - 1)
            {
                return false; // No more results
            }
            _currentIndex = -1; // Reset to before the first row
            return true; // Indicate that there are more results
        }

        public bool Read()
        {
            Initialize();

            _currentIndex++;
            return _currentIndex < _rows.Count;
        }

        private void Initialize()
        {

            if (_Names != null && _Names.Length != 0)
            {
                return;
            }

            string json = Task.Run(async () => await ReadAllJsonAsync()).GetAwaiter().GetResult();
            _rows = ParseJsonRows(json);

            _Names = _tcmd.Columns ?? _rows.SelectMany(row => row.Keys).Distinct().ToArray();

            if (_rows.Count > 0)
            {
                if (_Names.Length == 0)
                {
                    _Names = _rows[0].Keys.ToArray();
                }

                _Types = new Type[_Names.Length];
                for (int i = 0; i < _Names.Length; i++)
                {
                    if (_rows[0].TryGetValue(_Names[i], out var value) && value != null)
                    {
                        _Types[i] = value.GetType();
                    }
                    else
                    {
                        _Types[i] = typeof(string);
                    }
                }
            }
            else
            {
                _Names = Array.Empty<string>();
                _Types = Array.Empty<Type>();
            }
        }
    }
}
